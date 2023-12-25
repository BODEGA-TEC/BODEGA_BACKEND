using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sibe.API.Data;
using Sibe.API.Data.Dtos.Asistente;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models;
using Sibe.API.Models.Boletas;
using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Inventario;
using Sibe.API.Services.AsistenteService;
using Sibe.API.Services.ComponenteService;
using Sibe.API.Services.EquipoService;
using Sibe.API.Utils;
using System.Xml;

namespace Sibe.API.Services.BoletaService
{
    public class BoletaService : IBoletaService
    {

        private readonly IConfigurationSection _messages;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAsistenteService _asistenteService;

        public BoletaService(IConfiguration configuration, DataContext context, IMapper mapper, IAsistenteService asistenteService)
        {
            _messages = configuration.GetSection("BoletaService");
            _context = context;
            _mapper = mapper;
            _asistenteService = asistenteService;
        }

        public Task<ServiceResponse<List<string>>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<string>>> ReadByDateRange(DateTime inicial, DateTime final)
        {
            throw new NotImplementedException();
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
                    Observaciones = equipo.Observaciones
                });

                equipo.EstadoId = (boleta.TipoBoleta == TipoBoleta.PRESTAMO) ? 2 : 1;
            }
        }

        //// Agrega equipos a una boleta nueva y recupera información relevante de cada equipo de la base de datos.
        //private async Task AddAndRetrieveBoletaComponentes(Boleta boleta, List<BoletaComponenteDto> boletaList)
        //{
        //    foreach (var boletaComponente in boletaList)
        //    {
        //        // Recuperar el componente
        //        var componente = await _context.Componente
        //            .Include(e => e.Estado)
        //            .SingleOrDefaultAsync(e => e.Id == boletaComponente.Id)
        //            ?? throw new Exception($"{_messages["ComponenteIdNotFound"]} {boletaComponente.Id}");

        //        // Verificar si hay suficiente componentes para el prestamo solicitado
        //        if (boleta.po.Estado.Id != expectedEstadoId)
        //        {
        //            throw new Exception($"{_messages["ComponenteQuantityRequestError"]} {boletaComponente.Id}");
        //        }

        //        // Agregar el equipo a la lista de la boleta creada.
        //        boleta.BoletaEquipo.Add(new BoletaEquipo
        //        {
        //            Boleta = boleta,
        //            Equipo = equipo,
        //            Observaciones = equipo.Observaciones
        //        });
        //    }
        //}

        public async Task<ServiceResponse<int>> CreateBoletaPrestamo(CreateBoletaDto info)
        {
            var response = new ServiceResponse<int>();

            try
            {
                // Verificar existencia del asistente
                var asistente = await _asistenteService.FetchByCarne(info.CarneAsistente);

                // No se permiten préstamos sin la aprobación del profesor
                if (string.IsNullOrEmpty(info.Aprobador) && info.Componentes.Any())
                    throw new Exception(_messages["AprobadorNeeded"]);

                // Recuperar la informacion del solicitante
                var solicitante = new
                {
                    Tipo = info.TipoSolicitante,
                    Nombre = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? "PRUEBA ESTUDIANTE" : "PRUEBA PROFESOR",
                    Correo = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? "ESTUDIANTE@gmail.com" : "PROFESOR@gmail.com",
                    Carne = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? info.IdSolicitante : null
                };

                var time = TimeZoneHelper.Now();

                var boletaPrestamo = new Boleta
                {
                    Consecutivo = GenerateConsecutivo(time),
                    Aprobador = info.Aprobador,
                    TipoBoleta = TipoBoleta.PRESTAMO,
                    Estado = BoletaEstado.PENDIENTE,
                    FechaEmision = time,
                    NombreAsistente = asistente.Nombre,
                    CarneAsistente = asistente.Carne,
                    TipoSolicitante = info.TipoSolicitante,
                    NombreSolicitante = solicitante.Nombre,
                    CorreoSolicitante = solicitante.Correo,
                    CarneSolicitante = solicitante.Carne
                };
                await AddAndRetrieveBoletaEquipo(boletaPrestamo, info.Equipo);
                //await AddAndRetrieveBoletaComponentes(boletaPrestamo, info.Componentes);

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

        public async Task<ServiceResponse<int>> CreateBoletaDevolucion(CreateBoletaDto info)
        {
            var response = new ServiceResponse<int>();

            try
            {
                // Verificar existencia del asistente
                var asistente = await _asistenteService.FetchByCarne(info.CarneAsistente);

                // Verificar la existencia de los IDs de los activos

                // Recuperar la informacion del solicitante
                var solicitante = new
                {
                    Tipo = info.TipoSolicitante,
                    Nombre = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? "PRUEBA ESTUDIANTE" : "PRUEBA PROFESOR",
                    Correo = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? "ESTUDIANTE@gmail.com" : "PROFESOR@gmail.com",
                    Carne = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? info.IdSolicitante : null
                };

                // Recuperar el consecutivo anterior
                var boletaPrestamo = await _context.Boleta
                    .Where(b => b.CorreoSolicitante == solicitante.Correo &&
                                b.Estado == BoletaEstado.PENDIENTE &&
                                b.BoletaEquipo.Any(be => info.Equipo.Select(eq => eq.Id).Contains(be.EquipoId)))
                    .SingleOrDefaultAsync()
                    ?? throw new Exception(_messages["BoletaPrestamoNotFound"]);

                var boletaDevolucion = new Boleta
                {
                    Consecutivo = boletaPrestamo.Consecutivo,
                    Aprobador = boletaPrestamo.Aprobador,
                    TipoBoleta = TipoBoleta.DEVOLUCION,
                    Estado = BoletaEstado.CERRADO,
                    FechaEmision = TimeZoneHelper.Now(),
                    NombreAsistente = asistente.Nombre,
                    CarneAsistente = asistente.Carne,
                    TipoSolicitante = info.TipoSolicitante,
                    NombreSolicitante = solicitante.Nombre,
                    CorreoSolicitante = solicitante.Correo,
                    CarneSolicitante = solicitante.Carne
                };
                await AddAndRetrieveBoletaEquipo(boletaDevolucion, info.Equipo);
                //await AddAndRetrieveBoletaComponentes(boletaPrestamo, info.Componentes);

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





        public async Task<ServiceResponse<string>> CreateBoletaPrestamoXML(CreateBoletaDto info)
        { 
            var response = new ServiceResponse<string>();

            try
            {
                // Crear Boleta
                //var boleta = await CreateBoleta(TipoBoleta.PRESTAMO, info);
                //var xml = GenerateXML(info);
                /////QUEDE AQUI
                ///

                // Obtener la informacion de los activos
                var equipoList = _context.Equipo
                    //.AsNoTracking()
                    .Where(x => info.Equipo.Select(e => e.Id).Contains(x.Id))
                    .Select(x => new { x.Id, x.ActivoBodega, x.Descripcion, x.Observaciones })
                    .ToList();

                var componentesList = _context.Componente
                    //.AsNoTracking()
                    .Where(x => info.Componentes.Select(c => c.Id).Contains(x.Id))
                    .Select(x => new { x.Id, x.ActivoBodega, x.Descripcion, x.Observaciones })
                    .ToList();


                // Configurar respuesta
                response.SetSuccess(_messages["CreateSuccess"]);
            }

            catch (Exception ex)
            {
                // Configurar error
                response.SetError(ex.Message);
            }

            return response;
        }

        public Task<ServiceResponse<string>> CreateBoletaPrestamoXMLToBase64(CreateBoletaDto info)
            {
                throw new NotImplementedException();
            }
        }
}

