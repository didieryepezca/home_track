using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Dto.v1;
using AutoMapper;
using Service;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Home_Track_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropiedadController : ControllerBase
    {
        private readonly IPropiedadService _PropiedadService;
        private IMapper _mapper;

        public PropiedadController(IPropiedadService PropiedadService, IMapper mapper)
        {
            _PropiedadService = PropiedadService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorNombre)
        {
            try
            {
                int TotalPagina, TotalRegistro;
                bool TienePaginaAnterior, TienePaginaProximo;

                (TotalPagina, TotalRegistro, TienePaginaAnterior, TienePaginaProximo, var Lst_Propiedad) = await _PropiedadService.Obten_Paginado(RegistroPagina, NumeroPagina, PorNombre);

                var metadata = new
                {
                    RegistroPagina,
                    NumeroPagina,
                    TotalPagina,
                    TotalRegistro,
                    TienePaginaAnterior,
                    TienePaginaProximo
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

                return Ok(_mapper.Map<IEnumerable<PropiedadDTO>>(Lst_Propiedad));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        //[HttpGet("{Usu_Id}", Name = "Obten_x_Id")]
        //public async Task<IActionResult> Obten_x_Id(int Usu_Id)
        //{
        //    try
        //    {
        //        var Propiedad = await _PropiedadService.Obten_x_Id(Usu_Id);

        //        if (Propiedad is null)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            return Ok(_mapper.Map<PropiedadDto_Obten_x_Id>(Propiedad));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Error interno del servidor.");
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> Crea([FromBody] PropiedadNuevoDTO _PropiedadNuevoDTO)
        //{
        //    try
        //    {
        //        if (_PropiedadNuevoDTO is null)
        //        {
        //            return BadRequest("El Propiedad es nulo.");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest("Propiedad inválido.");
        //        }

        //        var CantidadHabilitado = await _PropiedadService.Existe(_PropiedadNuevoDTO.Usu_NumRUT, true);
        //        var CantidadInhabilitado = await _PropiedadService.Existe(_PropiedadNuevoDTO.Usu_NumRUT, false);

        //        if (CantidadHabilitado > 0)
        //        {
        //            return BadRequest(new RegistrationResponseDto { Errors = "Propiedad ya existente" });
        //        }

        //        if (CantidadInhabilitado > 0)
        //        {
        //            return BadRequest(new RegistrationResponseDto { Errors = "Propiedad ya existente con condición Inhabilitado." });
        //        }

        //        var Propiedad = _mapper.Map<Ent_Propiedad>(_PropiedadNuevoDTO);
                
        //        Propiedad.Usu_Clave = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10).ToUpper();

        //        Propiedad.Usu_Id = await _PropiedadService.Crea(Propiedad);

        //        await EnviarCorreo("0.3.1",Propiedad.Usu_Email,Propiedad.Usu_NomApeRazSoc,Propiedad.Usu_Clave);

        //        var PropiedadDTO = _mapper.Map<PropiedadDto>(Propiedad);

        //        return CreatedAtRoute("Obten_x_Id", new { Usu_Id = Propiedad.Usu_Id }, PropiedadDTO);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Error interno del servidor.");
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> Actualiza(int Usu_Id, [FromBody] PropiedadActualizadoDTO _PropiedadActualizadoDTO)
        //{
        //    try
        //    {
        //        if (_PropiedadActualizadoDTO is null)
        //        {
        //            return BadRequest("El Propiedad es nulo.");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest("Propiedad inválido.");
        //        }

        //        var PropiedadExistente = await _PropiedadService.Obten_x_Id(Usu_Id);

        //        if (PropiedadExistente is null)
        //        {
        //            return BadRequest(new RegistrationResponseDto { Errors = "Propiedad no registrado." });
        //        }

        //        var CantidadHabilitado = await _PropiedadService.Existente(Usu_Id, _PropiedadActualizadoDTO.Usu_NumRUT, true);
        //        var CantidadInhabilitado = await _PropiedadService.Existente(Usu_Id, _PropiedadActualizadoDTO.Usu_NumRUT, false);

        //        if (CantidadHabilitado > 0)
        //        {
        //            return BadRequest(new RegistrationResponseDto { Errors = "Propiedad ya existente" });
        //        }

        //        if (CantidadInhabilitado > 0)
        //        {
        //            return BadRequest(new RegistrationResponseDto { Errors = "Propiedad ya existente con condición inhabilitado." });
        //        }

        //        _mapper.Map(_PropiedadActualizadoDTO, PropiedadExistente);

        //        PropiedadExistente.Usu_Id = Usu_Id;

        //        await _PropiedadService.Actualiza(PropiedadExistente);

        //        return NoContent();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Error interno del servidor.");
        //    }
        //}

        //private async Task EnviarCorreo(string VersionApp, string EmailDestino, string Usu_Nombre, string ClaveAcceso)
        //{
        //    var message = new MimeMessage();

        //    var ListMail = new InternetAddressList();

        //    message.From.Add(new MailboxAddress("Home Track", "notificaciones@ibrlatam.com"));

        //    ListMail.Add(MailboxAddress.Parse(EmailDestino));

        //    message.To.AddRange(ListMail);
        //    message.Subject = $"Notificación | Home Track v{VersionApp}";

        //    var textPart = new TextPart(TextFormat.Html)
        //    {
        //        Text = CreateBody(VersionApp, Usu_Nombre, ClaveAcceso)
        //    };

        //    message.Body = textPart;

        //    using var client = new SmtpClient();

        //    await client.ConnectAsync("mail.iepfrayluisdeleon.edu.pe", 465, SecureSocketOptions.Auto);
        //    await client.AuthenticateAsync("notificaciones@iepfrayluisdeleon.edu.pe", "ingfa10");
        //    await client.SendAsync(message);
        //    await client.DisconnectAsync(true);
        //}

        //private static string CreateBody(string VersionApp, string Usu_Nombre, string ClaveAcceso)
        //{
        //    string body;

        //    using StreamReader reader = new(@".\index.html");

        //    body = reader.ReadToEnd();
        //    body = body.Replace("{Txt1}", Usu_Nombre);
        //    body = body.Replace("{Txt2}", ClaveAcceso);
        //    body = body.Replace("{Txt3}", $"Aplicación de Créditos Hipotecarios | Home Track v{VersionApp}.");

        //    return body;
        //}
    }
}