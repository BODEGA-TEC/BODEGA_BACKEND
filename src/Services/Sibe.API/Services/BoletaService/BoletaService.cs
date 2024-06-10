using AutoMapper;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models;
using Sibe.API.Models.Boletas;
using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.AsistenteService;
using Sibe.API.Services.ComponenteService;
using Sibe.API.Services.EmailService;
using Sibe.API.Services.EquipoService;
using Sibe.API.Utils;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net;

namespace Sibe.API.Services.BoletaService
{
    public class BoletaService : IBoletaService
    {

        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAsistenteService _asistenteService;
        private readonly IEquipoService _equipoService;
        private readonly IComponenteService _componenteService;
        private readonly IEmailService _emailService;

        private readonly JwtCredentialProvider _jwtCredentialProvider;
        private readonly string _templatePath = "wwwroot/html/BoletaPrestamoTemplate.html";
        private readonly string _tecLogoPath = "wwwroot/images/teclogo.png";
        private readonly string _sibeLogoPath = "wwwroot/images/sibelogo.png";

        public BoletaService(IConfiguration configuration, DataContext context, IMapper mapper,
            IAsistenteService asistenteService,
            IEquipoService equipoService,
            IComponenteService componenteService,
            IEmailService emailService,
            JwtCredentialProvider jwtCredentialProvider)
        {
            _messages = configuration.GetSection("BoletaService");
            _context = context;
            _mapper = mapper;
            _asistenteService = asistenteService;
            _equipoService = equipoService;
            _emailService = emailService;
            _componenteService = componenteService;
            _jwtCredentialProvider = jwtCredentialProvider;
        }


        // Esta funcion deberia estar en el LDAP
        // Debe ser un servicio aparte que se encargue de obtener la informaicon del profesor o estudiante que quiere solicitar un prestamo
        public Solicitante AuthenticateSolicitante(string carne)
        {
            // Intentar obtener el solicitante de la base de datos
            var solicitante = _context.Solicitante.FirstOrDefault(s => s.Carne == carne);

            if (solicitante != null)
            {
                // Si encontramos el solicitante en la base de datos, lo retornamos
                return solicitante;
            }
            else
            {
                // Si no existe en la base de datos, retornamos un nuevo solicitante con datos inventados
                return new Solicitante
                {
                    Nombre = "Carlos Vargas",
                    Correo = "carlos.vargas@estudiantec.cr",
                    Carne = carne,
                    Carrera = "INGENIERIA EN ELECTRONICA",
                    Tipo = TipoSolicitante.ESTUDIANTE
                };
            }
        }



        /// <summary>
        /// Método asíncrono para leer todas las boletas almacenadas en la base de datos.
        /// </summary>
        /// <returns>
        /// Una respuesta de servicio que contiene una lista de boletas en el formato de transferencia de datos (DTO),
        /// o un mensaje de error en caso de excepción.
        /// </returns>
        public async Task<ServiceResponse<List<ReadBoletaDto>>> ReadAll()
        {
            var response = new ServiceResponse<List<ReadBoletaDto>>();

            try
            {
                // Obtener todas las boletas de la base de datos
                var boletas = await _context.Boleta
                    .AsNoTracking()
                    .Include(b => b.Solicitante)
                    .ToListAsync();

                // Mapear las boletas a ReadBoletaDto
                var boletasDto = _mapper.Map<List<ReadBoletaDto>>(boletas);

                response.SetSuccess(_messages["ReadSuccess"], boletasDto);
            }
            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        /// <summary>
        /// Lee y obtiene todas las boletas dentro de un rango de fechas especificado.
        /// </summary>
        /// <param name="inicial">Fecha inicial del rango para buscar boletas.</param>
        /// <param name="final">Fecha final del rango para buscar boletas.</param>
        /// <returns>
        /// Una respuesta de servicio que incluye una lista de boletas en formato DTO si la operación es exitosa,
        /// o un mensaje de error en caso de fallo.
        /// </returns>
        public async Task<ServiceResponse<List<ReadBoletaDto>>> ReadByDateRange(DateTime inicial, DateTime final)
        {
            var response = new ServiceResponse<List<ReadBoletaDto>>();

            try
            {
                if (final.Date < inicial.Date)
                {
                    throw new Exception(_messages["FinalDateError"]);
                }


                // Obtener boletas dentro del rango de fechas
                var boletas = await _context.Boleta
                    .AsNoTracking()
                    .Include(b => b.Solicitante)
                    .Where(b => b.FechaEmision.Date >= inicial.Date && b.FechaEmision.Date <= final.Date)
                    .ToListAsync();

                // Mapear las boletas a ReadBoletaDto
                var boletasDto = _mapper.Map<List<ReadBoletaDto>>(boletas);

                response.SetSuccess(_messages["ReadSuccess"], boletasDto);
            }
            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        /// <summary>
        /// Lee y recupera todas las boletas pendientes asociadas a un solicitante específico, identificado por su carné.
        /// </summary>
        /// <param name="carne">El carné del solicitante para quien se buscan las boletas pendientes.</param>
        /// <returns>
        /// Una respuesta de servicio que incluye detalles del solicitante y sus boletas pendientes,
        /// o un mensaje de error en caso de fallo.
        /// </returns>
        public async Task<ServiceResponse<object>> ReadSolicitanteBoletasPendientes(string carne)
        {
            var response = new ServiceResponse<object>();

            try
            {

                // Recuperar la informacion del solicitante
                var solicitante = AuthenticateSolicitante(carne);

                // Realizar la consulta a la base de datos para obtener las boletas pendientes
                var boletas = await _context.Boleta
                    .AsNoTracking()
                    .Include(b => b.Solicitante)
                    .Where(b => b.Solicitante.Carne == carne && b.Estado == BoletaEstado.PENDIENTE)
                    .ToListAsync();

                // Mapear las boletas a ReadBoletaDto usando AutoMapper
                var boletasDto = _mapper.Map<List<ReadBoletaDto>>(boletas);

                // Configurar respuesta
                response.SetSuccess(_messages["ReadSuccess"], boletasDto);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        /// <summary>
        /// Recupera una boleta específica por ID desde la base de datos, incluyendo sus relaciones con componentes y equipos.
        /// </summary>
        /// <param name="id">El identificador de la boleta a recuperar.</param>
        /// <returns>
        /// La boleta solicitada con todas sus relaciones cargadas. Si la boleta no existe, lanza una excepción.
        /// </returns>
        private async Task<Boleta> FetchBoletaById(int id)
        {
            // Recuperar boleta con propiedades relacionadas
            return await _context.Boleta
                .Include(b => b.Solicitante)
                .Include(b => b.BoletaComponentes)
                    .ThenInclude(bc => bc.Componente)
                .Include(b => b.BoletaEquipo)
                    .ThenInclude(be => be.Equipo)
                .FirstOrDefaultAsync(b => b.Id == id)
                ?? throw new Exception(_messages["NotFound"]);
        }


        /// <summary>
        /// Genera un consecutivo único basado en la fecha y hora actual para identificar una boleta.
        /// </summary>
        /// <param name="time">La fecha y hora actual que se utilizará para generar el consecutivo.</param>
        /// <returns>
        /// Una cadena que representa el consecutivo generado, formateada como 'yyyyddMMHHmmss'.
        /// </returns>
        private static string GenerateConsecutivo(DateTime time)
        {
            // Crear el consecutivo combinando la fecha y hora en el formato especificado
            return time.ToString("yyyyddMMHHmmss");
        }





        public async Task<ServiceResponse<int>> CreateBoletaPrestamo(CreateBoletaDto info)
        {
            var response = new ServiceResponse<int>();

            try
            {
                // Obtener el carné del asistente.
                var carne = _jwtCredentialProvider.ExtractClaimsFromToken(info.Token, ClaimTypes.NameIdentifier);

                // Verificar existencia del asistente
                var asistente = await _asistenteService.FetchByCarne(carne);

                // No se permiten préstamos de componentes sin la aprobación del profesor.
                if (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE &&
                    string.IsNullOrEmpty(info.Aprobador) &&
                    info.Componentes.Any())
                    throw new Exception(_messages["AprobadorNeeded"]);

                // Recuperar la informacion del solicitante
                var solicitante = AuthenticateSolicitante(info.CarneSolicitante);

                // Obtener fecha emision
                var time = TimeZoneHelper.Now();

                var boletaPrestamo = new Boleta
                {
                    Consecutivo = GenerateConsecutivo(time),
                    Aprobador = info.Aprobador?.ToUpper(),
                    TipoBoleta = TipoBoleta.PRESTAMO,
                    Estado = BoletaEstado.PENDIENTE,
                    FechaEmision = time,
                    NombreAsistente = asistente.Nombre.ToUpper(),
                    CarneAsistente = carne,
                    Solicitante = solicitante
                };
                await ProcessBoletaEquipoTransaction(boletaPrestamo, info.Equipo);
                await ProcessBoletaComponenteTransaction(boletaPrestamo, info.Componentes);

                // Agregar al contexto
                _context.Boleta.Add(boletaPrestamo);
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], boletaPrestamo.Id);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }



        public async Task<ServiceResponse<int>> CreateBoletaDevolucion(int boletaPrestamoId, CreateBoletaDto infoDevolucion)
        {
            var response = new ServiceResponse<int>();

            try
            {
                // Obtener el carné del asistente.
                var carne = _jwtCredentialProvider.ExtractClaimsFromToken(infoDevolucion.Token, ClaimTypes.NameIdentifier);

                // Verificar existencia del asistente
                var asistente = await _asistenteService.FetchByCarne(carne);

                // Recuperar la boleta respectiva
                var boletaPrestamo = await FetchBoletaById(boletaPrestamoId);

                // Verificar que ambas boletas sean del mismo solicitante.
                if (boletaPrestamo.Solicitante.Carne != infoDevolucion.CarneSolicitante) throw new Exception(_messages["UnmatchedBoletaSolicitanteCarne"]);

                // Crear la boleta de devolución.
                var boletaDevolucion = new Boleta
                {
                    Consecutivo = boletaPrestamo.Consecutivo,
                    Aprobador = boletaPrestamo.Aprobador?.ToUpper(),
                    TipoBoleta = TipoBoleta.DEVOLUCION,
                    Estado = BoletaEstado.CERRADO,
                    FechaEmision = TimeZoneHelper.Now(),
                    NombreAsistente = asistente.Nombre.ToUpper(),
                    CarneAsistente = asistente.Carne,
                    Solicitante = boletaPrestamo.Solicitante
                };
                await ProcessBoletaEquipoTransaction(boletaDevolucion, infoDevolucion.Equipo);
                await ProcessBoletaComponenteTransaction(boletaDevolucion, infoDevolucion.Componentes);

                // Agregar al contexto
                _context.Boleta.Add(boletaDevolucion);
                boletaPrestamo.Estado = BoletaEstado.CERRADO;
                await _context.SaveChangesAsync();

                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"], boletaDevolucion.Id);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        /// <summary>
        /// Procesa la lista de equipos asociados con una boleta, manejando tanto préstamos como devoluciones.
        /// Este método delega la lógica específica a los métodos correspondientes basados en el tipo de transacción de la boleta.
        /// </summary>
        /// <param name="boleta">La boleta asociada con las operaciones de equipo.</param>
        /// <param name="boletaEquipos">Lista de DTOs de equipo asociados con la boleta.</param>
        private async Task ProcessBoletaEquipoTransaction(Boleta boleta, List<BoletaEquipoDto> boletaEquipos)
        {
            foreach (var boletaEquipo in boletaEquipos)
            {
                var equipo = await _equipoService.FetchByActivoBodega(boletaEquipo.ActivoBodega);

                if (boleta.TipoBoleta == TipoBoleta.PRESTAMO)
                {
                    HandleEquipoPrestamo(boleta, equipo, boletaEquipo);
                }
                else if (boleta.TipoBoleta == TipoBoleta.DEVOLUCION)
                {
                    HandleEquipoDevolucion(boleta, equipo, boletaEquipo);
                }
            }
        }

        // Maneja el proceso de préstamo de un equipo, verifica la disponibilidad,
        // registra el préstamo en la boleta, y actualiza el estado del equipo a "prestado".
        private void HandleEquipoPrestamo(Boleta boleta, Equipo equipo, BoletaEquipoDto boletaEquipo)
        {
            if (equipo.Estado.Id != 1) // 1: DISPONIBLE
            {
                throw new Exception($"{_messages["EquipoNotAvailable"]} {boletaEquipo.ActivoBodega}");
            }

            boleta.BoletaEquipo.Add(new BoletaEquipo
            {
                Boleta = boleta,
                Equipo = equipo,
                Observaciones = equipo.Observaciones // Observaciones del activo en el momento del préstamo
            });

            // Cambiar el estado del equipo a PRESTADO
            equipo.EstadoId = 2; // 2: PRESTADO
        }

        // Maneja el proceso de devolución de un equipo, verifica si está en estado "prestado",
        // registra la devolución en la boleta, y actualiza el estado del equipo a "disponible".
        private void HandleEquipoDevolucion(Boleta boleta, Equipo equipo, BoletaEquipoDto boletaEquipo)
        {
            if (equipo.Estado.Id != 2) // 2: PRESTADO
            {
                throw new Exception($"{_messages["EquipoNotPrestado"]} {boletaEquipo.ActivoBodega}");
            }

            boleta.BoletaEquipo.Add(new BoletaEquipo
            {
                Boleta = boleta,
                Equipo = equipo,
                Observaciones = boletaEquipo.Observaciones // Observaciones a la hora de devolución
            });

            // Actualiza las observaciones del equipo añadiendo las nuevas observaciones
            equipo.Observaciones += " | " + boletaEquipo.Observaciones;

            // Cambiar el estado del equipo a DISPONIBLE
            equipo.EstadoId = 1; // 1: DISPONIBLE
        }


        /// <summary>
        /// Procesa las transacciones de componentes para una boleta, manejando tanto préstamos como devoluciones.
        /// Este método decide la acción basada en el tipo de boleta y delega a los métodos específicos.
        /// </summary>
        /// <param name="boleta">La boleta para la que se procesan los componentes.</param>
        /// <param name="boletaComponentes">Lista de componentes involucrados en la transacción.</param>
        private async Task ProcessBoletaComponenteTransaction(Boleta boleta, List<BoletaComponenteDto> boletaComponentes)
        {
            foreach (var boletaComponente in boletaComponentes)
            {
                var componente = await _componenteService.FetchByActivoBodega(boletaComponente.ActivoBodega);

                if (boleta.TipoBoleta == TipoBoleta.PRESTAMO)
                {
                    HandleComponentePrestamo(componente, boletaComponente, boleta);
                }
                else if (boleta.TipoBoleta == TipoBoleta.DEVOLUCION)
                {
                    HandleComponenteDevolucion(componente, boletaComponente, boleta);
                }
            }
        }

        // Maneja el préstamo de componentes, verifica la disponibilidad y registra la transacción en la boleta.
        private void HandleComponentePrestamo(Componente componente, BoletaComponenteDto boletaComponente, Boleta boleta)
        {
            int cantidadSolicitada = boletaComponente.Cantidad;
            if (cantidadSolicitada == 0)
            {
                throw new Exception($"{_messages["ComponenteRequestZero"]} {componente.ActivoBodega}");
            }
            if (componente.CantidadDisponible < cantidadSolicitada)
            {
                throw new Exception($"{_messages["ComponenteQuantityRequestError"]} {componente.ActivoBodega}");
            }

            // Restar lo solicitado a la cantidad disponible
            componente.CantidadDisponible -= cantidadSolicitada;

            // Si la cantidad disponible es mayor que cero, el estado del componente se establece como "disponible"
            // Si la cantidad disponible es cero, el estado del componente se establece como "agotado"
            componente.EstadoId = componente.CantidadDisponible > 0 ? 1 : 3;

            // Agregar el componente a la lista de la boleta con las observaciones
            boleta.BoletaComponentes.Add(new BoletaComponente
            {
                Componente = componente,
                Boleta = boleta,
                Cantidad = cantidadSolicitada,
                Observaciones = boletaComponente.Observaciones
            });
        }

        // Maneja la devolución de componentes, actualiza la disponibilidad y registra la transacción en la boleta.
        private void HandleComponenteDevolucion(Componente componente, BoletaComponenteDto boletaComponente, Boleta boleta)
        {
            int cantidadDevuelta = boletaComponente.Cantidad;
            componente.CantidadDisponible += cantidadDevuelta;

            // Agregar el componente a la lista de la boleta con las observaciones
            boleta.BoletaComponentes.Add(new BoletaComponente
            {
                Componente = componente,
                Boleta = boleta,
                Cantidad = cantidadDevuelta,
                Observaciones = boletaComponente.Observaciones
            });

            // Actualiza las observaciones del equipo añadiendo las nuevas observaciones
            componente.Observaciones += " | " + boletaComponente.Observaciones;
        }


        public async Task<ServiceResponse<string>> SendBoletaByEmail(int boletaId)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Paso 1: Recuperar la boleta respectiva con la información de los activos y demás.
                Boleta boleta = await FetchBoletaById(boletaId);

                // Paso 2: Crear el PDF
                byte[] pdf = CreateBoletaPdf(boleta);

                // Paso 3: Enviar por correo electrónico
                string subject = $"Comprobante electrónico {boleta.Consecutivo}";
                string pdfName = $"{boleta.TipoBoleta}_{boleta.Solicitante.Carne}_{boleta.Consecutivo}.pdf";
                string body = $"<p>Estimado {boleta.Solicitante.Nombre},</p><p>Adjunto encontrará su comprobante electrónico.</p><p>Saludos,</p>";

                await _emailService.SendEmailAsync(boleta.Solicitante.Correo, subject, body, null, pdf, pdfName);
                response.SetSuccess("Email enviado exitosamente.");
            }
            catch (Exception ex)
            {
                response.SetError(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse<string>> GetBoletaHtml(int boletaId)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Paso 1: Recuperar la boleta respectiva con la informacion de los activos y demás.
                Boleta boleta = await FetchBoletaById(boletaId);

                // Paso 2: Crear el HTML
                string html = CreateBoletaHtml(boleta);

                // Paso 3: Crear el PDF
                //byte[] pdfBytes = CreatePdf(xml);

                // Enviar como respuesta
                response.SetSuccess(_messages["CreateSuccess"], html);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        /// <summary>
        /// Crea un PDF a partir de una boleta dada.
        /// </summary>
        /// <param name="boleta">La boleta que se utilizará para generar el PDF.</param>
        /// <returns>Un arreglo de bytes que representa el PDF generado.</returns>
        public byte[] CreateBoletaPdf(Boleta boleta)
        {
            // Paso 1: Crear el HTML usando la boleta proporcionada
            string html = CreateBoletaHtml(boleta);

            // Paso 2: Crear el PDF a partir del HTML generado
            byte[] pdf = CreateBoletaPdfFromHtml(html);

            return pdf;
        }


        /// <summary>
        /// Crea el contenido HTML para una boleta dada.
        /// </summary>
        /// <param name="boleta">La boleta para la cual se generará el HTML.</param>
        /// <returns>Una cadena que contiene el HTML generado.</returns>
        public string CreateBoletaHtml(Boleta boleta)
        {
            try
            {
                // Cargar la plantilla HTML desde un archivo
                string htmlTemplate = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), _templatePath), Encoding.UTF8);

                // Cargar la imagen del logo y convertirla a Base64
                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), _tecLogoPath);
                string logoImageHtml = $"file:///{logoPath.Replace("\\", "/")}";

                // Reemplazar marcadores de posición en la plantilla
                htmlTemplate = htmlTemplate.Replace("{{LogoPath}}", logoImageHtml);
                htmlTemplate = htmlTemplate.Replace("{{Consecutivo}}", boleta.Consecutivo);
                htmlTemplate = htmlTemplate.Replace("{{Periodo}}", TimeZoneHelper.GetSemesterPeriod(boleta.FechaEmision));
                htmlTemplate = htmlTemplate.Replace("{{TipoBoleta}}", boleta.TipoBoleta.ToString());
                htmlTemplate = htmlTemplate.Replace("{{Estado}}", boleta.Estado.ToString());
                htmlTemplate = htmlTemplate.Replace("{{Fecha}}", boleta.FechaEmision.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-US")));
                htmlTemplate = htmlTemplate.Replace("{{Hora}}", boleta.FechaEmision.ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")));
                htmlTemplate = htmlTemplate.Replace("{{Asistente}}", boleta.NombreAsistente);
                htmlTemplate = htmlTemplate.Replace("{{AprobadoPor}}", boleta.Aprobador);
                htmlTemplate = htmlTemplate.Replace("{{TipoSolicitante}}", boleta.Solicitante.Tipo.ToString());
                htmlTemplate = htmlTemplate.Replace("{{Carne}}", boleta.Solicitante.Carne);
                htmlTemplate = htmlTemplate.Replace("{{Nombre}}", boleta.Solicitante.Nombre);
                htmlTemplate = htmlTemplate.Replace("{{Correo}}", boleta.Solicitante.Correo);
                htmlTemplate = htmlTemplate.Replace("{{Carrera}}", boleta.Solicitante.Carrera);

                //// Construir la tabla de activos
                //StringBuilder stringBuilder = new StringBuilder();
                //stringBuilder.Append("<tr><th class='codigo'>Código</th><th class='tipo'>Tipo</th><th class='descripcion'>Descripción</th><th class='cantidad'>Cantidad</th><th class='observaciones'>Observaciones</th></tr>");
                //foreach (var be in boleta.BoletaEquipo)
                //{
                //    stringBuilder.AppendFormat("<tr><td class='codigo'>{0}</td><td class='tipo'>EQUIPO</td><td class='descripcion'>{1}</td><td class='cantidad'>{2}</td><td class='observaciones'>{3}</td></tr>",
                //        be.Equipo!.ActivoBodega, be.Equipo.Descripcion, 1, be.Equipo.Observaciones ?? "");
                //}
                //foreach (var bc in boleta.BoletaComponentes)
                //{
                //    stringBuilder.AppendFormat("<tr><td class='codigo'>{0}</td><td class='tipo'>COMPONENTE</td><td class='descripcion'>{1}</td><td class='cantidad'>{2}</td><td class='observaciones'>{3}</td></tr>",
                //        bc.Componente!.ActivoBodega, bc.Componente.Descripcion, bc.Cantidad, bc.Componente.Observaciones ?? "");
                //}
                //htmlTemplate = htmlTemplate.Replace("{{TablaActivos}}", stringBuilder.ToString());
                htmlTemplate = Regex.Replace(htmlTemplate, @"\s+", " ").Replace("> <", "><");
                return htmlTemplate;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al generar el HTML: " + ex.Message);
            }
        }


        /// <summary>
        /// Crea un PDF a partir de una cadena HTML dada.
        /// </summary>
        /// <param name="html">El contenido HTML que se utilizará para generar el PDF.</param>
        /// <returns>Un arreglo de bytes que representa el PDF generado.</returns>
        public byte[] CreateBoletaPdfFromHtml(string html)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);
                document.Open();
 

                using (StringReader stringReader = new StringReader(html))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                }
                document.Close();
                return stream.ToArray();
            }
        }

        //using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(html)))
        //using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
        //{
        //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
        //}
    }
}

