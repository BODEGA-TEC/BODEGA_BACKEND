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

        private void CheckIdsNotFound(IEnumerable<int> inputIds, IEnumerable<int> existingIds, string? errorMessageTemplate)
        {
            var idsNotFound = inputIds.Except(existingIds);
            if (idsNotFound.Any())
            {
                string errorMessage = $"{errorMessageTemplate} {string.Join(", ", idsNotFound)}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<ServiceResponse<int>> CreateBoletaPrestamo(CreateBoletaDto info)
        {
            var response = new ServiceResponse<int>();

            try
            {
                // Verificar existencia del asistente
                var asistente = await _asistenteService.FetchByCarne(info.CarneAsistente);

                // No se permiten préstamos sin la aprobación del profesor
                if (string.IsNullOrEmpty(info.Aprobador) && info.Componentes.Any())
                    throw new Exception("No se permiten préstamos de componentes sin la aprobación de un profesor.");

                // Verificar la existencia de los IDs de los activos
                CheckIdsNotFound(info.Equipo.Select(c => c.Id), _context.Equipo.Select(c => c.Id), _messages["EquipoIdNotFound"]);
                CheckIdsNotFound(info.Componentes.Select(c => c.Id), _context.Componente.Select(c => c.Id), _messages["ComponenteIdNotFound"]);

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

                foreach (var equipo in info.Equipo)
                {
                    boletaPrestamo.BoletaEquipo.Add(new BoletaEquipo
                    {
                        Boleta = boletaPrestamo,
                        EquipoId = equipo.Id,
                        Observaciones = equipo.Observaciones
                    });
                }

                foreach (var componente in info.Componentes)
                {
                    boletaPrestamo.BoletaComponentes.Add(new BoletaComponente
                    {
                        Boleta = boletaPrestamo,
                        ComponenteId = componente.Id,
                        Cantidad = componente.Cantidad,
                        Observaciones = componente.Observaciones
                    });
                }

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
                CheckIdsNotFound(info.Equipo.Select(c => c.Id), _context.Equipo.Select(c => c.Id), _messages["EquipoIdNotFound"]);
                CheckIdsNotFound(info.Componentes.Select(c => c.Id), _context.Componente.Select(c => c.Id), _messages["ComponenteIdNotFound"]);

                // Recuperar la informacion del solicitante
                var solicitante = new
                {
                    Tipo = info.TipoSolicitante,
                    Nombre = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? "PRUEBA ESTUDIANTE" : "PRUEBA PROFESOR",
                    Correo = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? "ESTUDIANTE@gmail.com" : "PROFESOR@gmail.com",
                    Carne = (info.TipoSolicitante == TipoSolicitante.ESTUDIANTE) ? info.IdSolicitante : null
                };


                // Recuperar el consecutivo anterior
                var boletaPrestamo = _context.Boleta
                    .Where(b => b.CorreoSolicitante == solicitante.Correo &&
                                b.Estado == BoletaEstado.PENDIENTE &&
                                b.BoletaEquipo.Any(be => info.Equipo.Select(eq => eq.Id).Contains(be.EquipoId)))
                    .Single()
                    ?? throw new Exception(_messages["BoletaPrestamoNotFound"]);

                var boletaDevolucion = new Boleta
                {
                    Consecutivo = boletaPrestamo.Consecutivo,
                    Aprobador = info.Aprobador,
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

                foreach (var equipo in info.Equipo)
                {
                    boletaDevolucion.BoletaEquipo.Add(new BoletaEquipo
                    {
                        Boleta = boletaDevolucion,
                        EquipoId = equipo.Id,
                        Observaciones = equipo.Observaciones
                    });
                }

                foreach (var componente in info.Componentes)
                {
                    boletaDevolucion.BoletaComponentes.Add(new BoletaComponente
                    {
                        Boleta = boletaDevolucion,
                        ComponenteId = componente.Id,
                        Cantidad = componente.Cantidad,
                        Observaciones = componente.Observaciones
                    });
                }

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

