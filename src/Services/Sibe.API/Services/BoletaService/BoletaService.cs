using AutoMapper;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using JwtAuthenticationHandler;
using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models;
using Sibe.API.Models.Boletas;
using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.AsistenteService;
using Sibe.API.Utils;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Xml;

namespace Sibe.API.Services.BoletaService
{
    public class BoletaService : IBoletaService
    {

        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAsistenteService _asistenteService;
        private readonly JwtCredentialProvider _jwtCredentialProvider;

        public BoletaService(IConfiguration configuration, DataContext context, IMapper mapper, IAsistenteService asistenteService, JwtCredentialProvider jwtCredentialProvider)
        {
            _messages = configuration.GetSection("BoletaService");
            _context = context;
            _mapper = mapper;
            _asistenteService = asistenteService;
            _jwtCredentialProvider = jwtCredentialProvider;

        }

        public async Task<ServiceResponse<List<ReadBoletaDto>>> ReadAll()
        {
            var response = new ServiceResponse<List<ReadBoletaDto>>();

            try
            {
                // Obtener todas las boletas de la base de datos
                var boletas = await _context.Boleta.ToListAsync();

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



        public Solicitante AuthenticateSolicitante(string carne)
        {
            bool local = true;

            if (local)
            {
                return new Solicitante
                {
                    Nombre = "prueba estudiante",
                    Correo = "estudiante@gmail.com",
                    Carne = carne
                };
            }

            else
            {
                // DO SOMETHING WITH LDAP
                throw new NotImplementedException();
            }
        }

        // Recupera las boletas pendientes que tiene un solicitante
        public async Task<ServiceResponse<object>> ReadSolicitanteBoletasPendientes(string carne)
        {
            var response = new ServiceResponse<object>();

            try
            {

                // Recuperar la informacion del solicitante
                var solicitante = AuthenticateSolicitante(carne);

                // Realizar la consulta a la base de datos para obtener las boletas pendientes
                var boletas = await _context.Boleta
                    .Where(b => b.CarneSolicitante == carne && b.Estado == BoletaEstado.PENDIENTE)
                    .ToListAsync();

                // Mapear las boletas a ReadBoletaDto usando AutoMapper
                var boletasDto = _mapper.Map<List<ReadBoletaDto>>(boletas);


                var data = new
                {
                    Nombre = solicitante.Nombre,
                    Correo = solicitante.Correo,
                    Carne = carne,
                    Boletas = boletasDto
                };

                // Configurar respuesta
                response.SetSuccess(_messages["ReadSuccess"], data);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }


        private async Task<Boleta> FetchBoletaById(int id)
        {
            // Recuperar boleta con propiedades relacionadas
            return await _context.Boleta
                .Include(b => b.BoletaComponentes)
                    .ThenInclude(bc => bc.Componente)
                .Include(b => b.BoletaEquipo)
                    .ThenInclude(be => be.Equipo)
                .FirstOrDefaultAsync(b => b.Id == id)
                ?? throw new Exception(_messages["NotFound"]);
        }


        private static string GenerateConsecutivo(DateTime time)
        {
            // Crear el consecutivo combinando la fecha y hora en el formato especificado
            return time.ToString("yyyyddMMHHmmss");
        }


        // Agrega equipos a una boleta nueva y recupera información relevante de cada equipo de la base de datos.
        private async Task AddAndRetrieveBoletaEquipo(Boleta boleta, List<BoletaEquipoDto> boletaList)
        {
            foreach (var boletaEquipo in boletaList)
            {
                // Recuperar el equipo
                var equipo = await _context.Equipo
                    .Include(e => e.Estado)
                    .SingleOrDefaultAsync(e => e.Id == boletaEquipo.Id)
                    ?? throw new Exception($"{_messages["EquipoIdNotFound"]} {boletaEquipo.Id}");

                // Verificar si el equipo tiene el estado disponible
                var expectedEstadoId = (boleta.TipoBoleta == TipoBoleta.PRESTAMO) ? 1 : 2; // 1: DISPONIBLE - 2: PRESTADO
                if (equipo.Estado.Id != expectedEstadoId)
                {
                    string errorMessage = (expectedEstadoId == 1)
                        ? $"{_messages["EquipoNotAvailable"]} {boletaEquipo.Id}"  // Estado esperado: DISPONIBLE
                        : $"{_messages["EquipoNotPrestado"]} {boletaEquipo.Id}";  // Estado esperado: PRESTADO

                    throw new Exception(errorMessage);
                }

                // Agregar el equipo a la lista de la boleta creada.
                boleta.BoletaEquipo.Add(new BoletaEquipo
                {
                    Boleta = boleta,
                    Equipo = equipo,

                    // Si es de prestamo, se adjunta las observaciones del activo
                    // Si es de devolución, se adjuntan las observaciones a la hora de devolución
                    Observaciones = boleta.TipoBoleta == TipoBoleta.PRESTAMO
                        ? equipo.Observaciones
                        : boletaEquipo.Observaciones
                });

                equipo.EstadoId = (boleta.TipoBoleta == TipoBoleta.PRESTAMO) ? 2 : 1;
            }
        }

        // Agrega equipos a una boleta nueva y recupera información relevante de cada equipo de la base de datos.
        private async Task AddAndRetrieveBoletaComponentes(Boleta boleta, List<BoletaComponenteDto> boletaList)
        {
            foreach (var boletaComponente in boletaList)
            {
                // Recuperar el componente
                var componente = await _context.Componente
                    .Include(e => e.Estado)
                    .SingleOrDefaultAsync(e => e.Id == boletaComponente.Id)
                    ?? throw new Exception($"{_messages["ComponenteIdNotFound"]} {boletaComponente.Id}");

                var cantidadBoleta = boletaComponente.Cantidad;

                if (boleta.TipoBoleta == TipoBoleta.PRESTAMO)
                {
                    if (cantidadBoleta == 0)
                    {
                        throw new Exception($"{_messages["ComponenteRequestZero"]} {componente.ActivoBodega}");
                    }
                    // Verificar si hay suficiente componentes para el prestamo solicitado
                    if (componente.CantidadDisponible == 0 || componente.CantidadDisponible < cantidadBoleta)
                    {
                        throw new Exception($"{_messages["ComponenteQuantityRequestError"]} {componente.ActivoBodega}");
                    }
                    else
                    {
                        // Descontar los componentes que se estan prestando
                        componente.CantidadDisponible -= cantidadBoleta;
                    }
                }
                else
                {
                    // Volver a sumar los componentes que se estan devolviendo.
                    componente.CantidadDisponible += cantidadBoleta;
                }

                // Si pasa a ser cero se pone en agotado, de lo contrario, deberia quedar en disponible.
                componente.EstadoId = componente.CantidadDisponible > 0 ? 1 : 3;

                // Agregar el componente a la lista de la boleta creada.
                boleta.BoletaComponentes.Add(new BoletaComponente
                {
                    Boleta = boleta,
                    Componente = componente,
                    Cantidad = cantidadBoleta,

                    // Si es de prestamo, se adjunta las observaciones del activo
                    // Si es de devolución, se adjuntan las observaciones a la hora de devolución
                    Observaciones = boleta.TipoBoleta == TipoBoleta.PRESTAMO
                        ? componente.Observaciones
                        : boletaComponente.Observaciones
                });
            }
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

                // No se permiten préstamos sin la aprobación del profesor.
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
                    NombreAsistente = asistente.Nombre,
                    CarneAsistente = carne,
                    TipoSolicitante = info.TipoSolicitante,
                    NombreSolicitante = solicitante.Nombre,
                    CorreoSolicitante = solicitante.Correo,
                    CarneSolicitante = solicitante.Carne
                };
                await AddAndRetrieveBoletaEquipo(boletaPrestamo, info.Equipo);
                await AddAndRetrieveBoletaComponentes(boletaPrestamo, info.Componentes);

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
                if (boletaPrestamo.CarneSolicitante != infoDevolucion.CarneSolicitante) throw new Exception(_messages["UnmatchedBoletaSolicitanteCarne"]);

                // Crear la boleta de devolución.
                var boletaDevolucion = new Boleta
                {
                    Consecutivo = boletaPrestamo.Consecutivo,
                    Aprobador = boletaPrestamo.Aprobador?.ToUpper(),
                    TipoBoleta = TipoBoleta.DEVOLUCION,
                    Estado = BoletaEstado.CERRADO,
                    FechaEmision = TimeZoneHelper.Now(),
                    NombreAsistente = asistente.Nombre,
                    CarneAsistente = asistente.Carne,
                    TipoSolicitante = boletaPrestamo.TipoSolicitante,
                    NombreSolicitante = boletaPrestamo.NombreSolicitante,
                    CorreoSolicitante = boletaPrestamo.CorreoSolicitante,
                    CarneSolicitante = boletaPrestamo.CarneSolicitante
                };
                await AddAndRetrieveBoletaEquipo(boletaDevolucion, infoDevolucion.Equipo);
                await AddAndRetrieveBoletaComponentes(boletaDevolucion, infoDevolucion.Componentes);

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

        public async Task<ServiceResponse<string>> GetBoletaPdf(int boletaId)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Paso 1: Recuperar la boleta respectiva con la informacion de los activos y demás.
                Boleta boleta = await FetchBoletaById(boletaId);

                // Paso 2: Crear el XML
                string xml = CreateXml(boleta);

                // Paso 3: Crear el PDF
                //byte[] pdfBytes = CreatePdf(xml);

                // Enviar como respuesta
                response.SetSuccess(_messages["CreateSuccess"], xml);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<string>> SendBoletaByEmail(int boletaId)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Paso 1: Recuperar la boleta respectiva con la informacion de los activos y demás.
                Boleta boleta = await FetchBoletaById(boletaId);

                // Paso 2: Crear el XML
                string xml = CreateXml(boleta);

                // Paso 3: Crear el PDF
                // byte[] pdfBytes = await CreatePdfAsync(xml);

                // Paso 4: Enviar por correo electrónico
                //await SendEmailAsync(boleta.Email, "Datos de la boleta", "Adjunto encontrarás los archivos XML y PDF con la boleta.", xmlContent, pdfFilePath);
                response.SetSuccess(_messages["CreateSuccess"], xml);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        private string CreateXml(Boleta boleta)
        {
            try
            {
                // Creamos un documento XML
                var xmlDoc = new XmlDocument();

                // Creamos el elemento raíz "Boleta"
                var boletaElement = xmlDoc.CreateElement("Boleta");
                xmlDoc.AppendChild(boletaElement);

                // Agregamos elementos hijos con los datos de la boleta
                var consecutivoElement = xmlDoc.CreateElement("Consecutivo");
                consecutivoElement.InnerText = boleta.Consecutivo;
                boletaElement.AppendChild(consecutivoElement);

                var estadoElement = xmlDoc.CreateElement("Estado");
                estadoElement.InnerText = boleta.Estado.ToString();
                boletaElement.AppendChild(estadoElement);

                var carneAsistenteElement = xmlDoc.CreateElement("CarneAsistente");
                carneAsistenteElement.InnerText = boleta.CarneAsistente;
                boletaElement.AppendChild(carneAsistenteElement);

                var nombreAsistenteElement = xmlDoc.CreateElement("NombreAsistente");
                nombreAsistenteElement.InnerText = boleta.NombreAsistente;
                boletaElement.AppendChild(nombreAsistenteElement);

                var fechaElement = xmlDoc.CreateElement("Fecha");
                fechaElement.InnerText = boleta.FechaEmision.ToString("dd/MM/yyyy");
                boletaElement.AppendChild(fechaElement);

                // Crear subelemento para la hora específica
                var horaElement = xmlDoc.CreateElement("Hora");
                horaElement.InnerText = boleta.FechaEmision.ToString("hh:mm tt", new CultureInfo("es-ES"));
                boletaElement.AppendChild(horaElement);


                // -- SOLICITANTE
                var solicitanteElement = xmlDoc.CreateElement("Solicitante");

                // Subelemento "Nombre"
                var nombreSolicitanteElement = xmlDoc.CreateElement("Nombre");
                nombreSolicitanteElement.InnerText = boleta.NombreSolicitante;
                solicitanteElement.AppendChild(nombreSolicitanteElement);

                // Subelemento "Tipo"
                var tipoSolicitanteElement = xmlDoc.CreateElement("Tipo");
                tipoSolicitanteElement.InnerText = boleta.TipoSolicitante.ToString();
                solicitanteElement.AppendChild(tipoSolicitanteElement);

                // Subelemento "Correo"
                var correoSolicitanteElement = xmlDoc.CreateElement("Correo");
                correoSolicitanteElement.InnerText = boleta.CorreoSolicitante;
                solicitanteElement.AppendChild(correoSolicitanteElement);

                boletaElement.AppendChild(solicitanteElement);

                // -- DETALLE ACTIVOS
                var detalleActivosElement = xmlDoc.CreateElement("DetalleActivos");

                // -- EQUIPO
                var equipoElement = xmlDoc.CreateElement("Equipo");
                foreach (var be in boleta.BoletaEquipo)
                {

                    if (be.Equipo == null)
                    {
                        throw new Exception("No se ha encontrado equipo");
                    }

                    var equipoItemElement = xmlDoc.CreateElement("Item");

                    var codigoEquipoElement = xmlDoc.CreateElement("Codigo");
                    codigoEquipoElement.InnerText = be.Equipo.ActivoBodega;
                    equipoItemElement.AppendChild(codigoEquipoElement);

                    var detalleEquipoElement = xmlDoc.CreateElement("Detalle");
                    detalleEquipoElement.InnerText = be.Equipo.Descripcion;
                    equipoItemElement.AppendChild(detalleEquipoElement);

                    var observacionesEquipoElement = xmlDoc.CreateElement("Observaciones");
                    observacionesEquipoElement.InnerText = be.Observaciones ?? "";
                    equipoItemElement.AppendChild(observacionesEquipoElement);

                    equipoElement.AppendChild(equipoItemElement);
                }

                // -- COMPONENTES
                var componentesElement = xmlDoc.CreateElement("Componentes");
                foreach (var bc in boleta.BoletaComponentes)
                {

                    if (bc.Componente == null)
                    {
                        throw new Exception("No se ha encontrado componente");
                    }

                    var componenteItemElement = xmlDoc.CreateElement("Item");

                    var codigoComponenteElement = xmlDoc.CreateElement("Codigo");
                    codigoComponenteElement.InnerText = bc.Componente.ActivoBodega;
                    componenteItemElement.AppendChild(codigoComponenteElement);

                    var detalleComponenteElement = xmlDoc.CreateElement("Detalle");
                    detalleComponenteElement.InnerText = bc.Componente.Descripcion;
                    componenteItemElement.AppendChild(detalleComponenteElement);

                    var cantidadComponenteElement = xmlDoc.CreateElement("Cantidad");
                    cantidadComponenteElement.InnerText = bc.Cantidad.ToString();
                    componenteItemElement.AppendChild(cantidadComponenteElement);

                    var observacionesComponenteElement = xmlDoc.CreateElement("Observaciones");
                    observacionesComponenteElement.InnerText = bc.Observaciones ?? "";
                    componenteItemElement.AppendChild(observacionesComponenteElement);

                    componentesElement.AppendChild(componenteItemElement);
                }

                detalleActivosElement.AppendChild(equipoElement);
                detalleActivosElement.AppendChild(componentesElement);

                boletaElement.AppendChild(detalleActivosElement);

                // Convertimos el documento XML a cadena y lo retornamos
                return xmlDoc.OuterXml;
            }
            catch (Exception)
            {
                // Maneja errores o logea según sea necesario
                throw;
            }
        }

        // Implementa la lógica para crear el archivo PDF basado en el XML
        private byte[] CreatePdf(string xmlString)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Crear un documento PDF con iTextSharp
                    using (var writer = new PdfWriter(memoryStream))
                    using (var pdfDoc = new PdfDocument(writer))
                    using (iText.Layout.Document document = new iText.Layout.Document(pdfDoc))
                    {
                        // Agregar contenido del XML al PDF
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlString);

                        //// Obtener el elemento "Boleta"
                        //XmlNode boletaNode = xmlDoc.SelectSingleNode("//Boleta");

                        //// Agregar contenido al PDF

                        //// Informacion inicial
                        //document.Add(new Paragraph("INSTITUTO TECNOLÓGICO DE COSTA RICA").SetFontSize(12));
                        //document.Add(new Paragraph("SISTEMA INTEGRADO DE BODEGA ELECTRÓNICA (SIBE)").SetFontSize(16));
                        //document.Add(new Paragraph("COMPROBANTE ELECTRONICO").SetFontSize(12));
                        //document.Add(new Paragraph($"Consecutivo: {boletaNode.SelectSingleNode("Consecutivo").InnerText}"));
                        //document.Add(new Paragraph($"Estado: {boletaNode.SelectSingleNode("Estado").InnerText}"));
                        //document.Add(new Paragraph($"Carne Asistente: {boletaNode.SelectSingleNode("CarneAsistente").InnerText}"));
                        //document.Add(new Paragraph($"Nombre Asistente: {boletaNode.SelectSingleNode("NombreAsistente").InnerText}"));
                        //document.Add(new Paragraph($"Fecha Emisión: {boletaNode.SelectSingleNode("Fecha").InnerText}"));

                        //// Información del Solicitante
                        //XmlNode solicitanteNode = boletaNode.SelectSingleNode("Solicitante");
                        //document.Add(new Paragraph("Solicitante:"));
                        //document.Add(new Paragraph($"  Nombre: {solicitanteNode.SelectSingleNode("Nombre").InnerText}"));
                        //document.Add(new Paragraph($"  Tipo: {solicitanteNode.SelectSingleNode("Tipo").InnerText}"));
                        //document.Add(new Paragraph($"  Correo: {solicitanteNode.SelectSingleNode("Correo").InnerText}"));

                        //// Información del Detalle de Activos
                        //XmlNode detalleActivosNode = boletaNode.SelectSingleNode("DetalleActivos");

                        //XmlNode equipoNode = detalleActivosNode.SelectSingleNode("Equipo");
                        //document.Add(new Paragraph("Equipo:"));
                        //foreach (XmlNode equipoItemNode in equipoNode.SelectNodes("Item"))
                        //{
                        //    document.Add(new Paragraph($"  Código: {equipoItemNode.SelectSingleNode("Codigo").InnerText}"));
                        //    document.Add(new Paragraph($"  Detalle: {equipoItemNode.SelectSingleNode("Detalle").InnerText}"));
                        //    document.Add(new Paragraph($"  Observaciones: {equipoItemNode.SelectSingleNode("Observaciones").InnerText}"));
                        //}

                        //XmlNode componentesNode = detalleActivosNode.SelectSingleNode("Componentes");
                        //document.Add(new Paragraph("Componentes:"));
                        //foreach (XmlNode componenteItemNode in componentesNode.SelectNodes("Item"))
                        //{
                        //    document.Add(new Paragraph($"  Código: {componenteItemNode.SelectSingleNode("Codigo").InnerText}"));
                        //    document.Add(new Paragraph($"  Detalle: {componenteItemNode.SelectSingleNode("Detalle").InnerText}"));
                        //    document.Add(new Paragraph($"  Cantidad: {componenteItemNode.SelectSingleNode("Cantidad").InnerText}"));
                        //    document.Add(new Paragraph($"  Observaciones: {componenteItemNode.SelectSingleNode("Observaciones").InnerText}"));
                        //}

                        // Cerrar el objeto Document y el archivo PDF
                        document.Close();

                    }
                    // Devolver el array de bytes del PDF generado
                    return memoryStream.ToArray();
                }
            }

            catch (Exception)
            {
                // Maneja errores o logea según sea necesario
                throw;
            }
        }



        //private async Task SendEmailAsync(string toEmail, string subject, string body, string xmlContent, string pdfFilePath)
        //{
        //    // Implementa la lógica para enviar el correo electrónico con archivos adjuntos
        //    // Utiliza xmlContent para el contenido del XML y pdfFilePath para la ruta del archivo PDF
        //    // Ejemplo:
        //    // await _emailService.SendEmailAsync(toEmail, subject, body, xmlContent, pdfFilePath);
        //}








        //private string GenerateXML(CreateBoletaEmisionDto info)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();

        //    // Crear el elemento raíz
        //    XmlElement rootElement = xmlDoc.CreateElement("Boleta");

        //    // Añadir elementos con datos
        //    XmlElement consecutivoElement = xmlDoc.CreateElement("Consecutivo");
        //    consecutivoElement.InnerText = GenerateConsecutivo();
        //    rootElement.AppendChild(consecutivoElement);

        //    XmlElement carneAsistenteEmisorElement = xmlDoc.CreateElement("CarneAsistenteEmisor");
        //    carneAsistenteEmisorElement.InnerText = info.CarneAsistenteEmisor;
        //    rootElement.AppendChild(carneAsistenteEmisorElement);


        //    // Solicitante

        //    XmlElement solicitanteElement = xmlDoc.CreateElement("Solicitante");

        //    XmlElement tipoSolicitanteElement = xmlDoc.CreateElement("TipoSolicitante");
        //    tipoSolicitanteElement.InnerText = info.TipoSolicitante.ToString();
        //    rootElement.AppendChild(tipoSolicitanteElement);

        //    if (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE)
        //    {
        //        XmlElement idSolicitanteElement = xmlDoc.CreateElement("Carne");
        //        carneElement.InnerText = info.IdSolicitante;
        //        solicitanteElement.AppendChild(idSolicitanteElement);
        //    }




        //    // Añadir elementos para la lista Equipo
        //    XmlElement equipoElement = xmlDoc.CreateElement("Equipo");
        //    foreach (int equipoItem in info.Equipo)
        //    {
        //        XmlElement equipoItemElement = xmlDoc.CreateElement("EquipoItem");
        //        equipoItemElement.InnerText = equipoItem.ToString();
        //        equipoElement.AppendChild(equipoItemElement);
        //    }
        //    rootElement.AppendChild(equipoElement);

        //    // Añadir elementos para la lista Componentes
        //    XmlElement componentesElement = xmlDoc.CreateElement("Componentes");
        //    foreach (BoletaComponenteDto componente in info.Componentes)
        //    {
        //        XmlElement componenteElement = xmlDoc.CreateElement("Componente");

        //        XmlElement componenteIdElement = xmlDoc.CreateElement("ComponenteId");
        //        componenteIdElement.InnerText = componente.ComponenteId.ToString();
        //        componenteElement.AppendChild(componenteIdElement);

        //        XmlElement cantidadElement = xmlDoc.CreateElement("Cantidad");
        //        cantidadElement.InnerText = componente.Cantidad.ToString();
        //        componenteElement.AppendChild(cantidadElement);

        //        componentesElement.AppendChild(componenteElement);
        //    }
        //    rootElement.AppendChild(componentesElement);

        //    xmlDoc.AppendChild(rootElement);

        //    return xmlDoc.InnerXml;

        //    //// Obtener el XML como cadena y asignarlo a la propiedad Data del serverResponse
        //    //var output = xmlDoc.OuterXml.Replace("schemaLocation", "xsi:schemaLocation");

        //    //return output;
        //}





        //public async Task<ServiceResponse<string>> CreateBoletaPrestamoXML(CreateBoletaDto info)
        //{ 
        //    var response = new ServiceResponse<string>();

        //    try
        //    {
        //        // Crear Boleta
        //        //var boleta = await CreateBoleta(TipoBoleta.PRESTAMO, info);
        //        //var xml = GenerateXML(info);
        //        /////QUEDE AQUI
        //        ///

        //        // Obtener la informacion de los activos
        //        var equipoList = _context.Equipo
        //            //.AsNoTracking()
        //            .Where(x => info.Equipo.Select(e => e.Id).Contains(x.Id))
        //            .Select(x => new { x.Id, x.ActivoBodega, x.Descripcion, x.Observaciones })
        //            .ToList();

        //        var componentesList = _context.Componente
        //            //.AsNoTracking()
        //            .Where(x => info.Componentes.Select(c => c.Id).Contains(x.Id))
        //            .Select(x => new { x.Id, x.ActivoBodega, x.Descripcion, x.Observaciones })
        //            .ToList();


        //        // Configurar respuesta
        //        response.SetSuccess(_messages["CreateSuccess"]);
        //    }

        //    catch (Exception ex)
        //    {
        //        // Configurar error
        //        response.SetError(ex.Message);
        //    }

        //    return response;
        //}

        //public Task<ServiceResponse<string>> CreateBoletaPrestamoXMLToBase64(CreateBoletaDto info)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}

