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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;
        private IMapper _mapper;

        public UsuarioController(IUsuarioService UsuarioService, IMapper mapper)
        {
            _UsuarioService = UsuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Obten_Paginado(int RegistroPagina, int NumeroPagina, string PorNombre)
        {
            try
            {
                int TotalPagina, TotalRegistro;
                bool TienePaginaAnterior, TienePaginaProximo;

                (TotalPagina, TotalRegistro, TienePaginaAnterior, TienePaginaProximo, var Lst_Usuario) = await _UsuarioService.Obten_Paginado(RegistroPagina, NumeroPagina, PorNombre);

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

                return Ok(_mapper.Map<IEnumerable<UsuarioDto>>(Lst_Usuario));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{Usu_Id}", Name = "Obten_x_Id")]
        public async Task<IActionResult> Obten_x_Id(int Usu_Id)
        {
            try
            {
                var Usuario = await _UsuarioService.Obten_x_Id(Usu_Id);

                if (Usuario is null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_mapper.Map<UsuarioDto_Obten_x_Id>(Usuario));
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crea([FromBody] UsuarioNuevoDTO _UsuarioNuevoDTO)
        {
            try
            {
                if (_UsuarioNuevoDTO is null)
                {
                    return BadRequest("El Usuario es nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Usuario inválido.");
                }

                var CantidadHabilitado = await _UsuarioService.Existe(_UsuarioNuevoDTO.Usu_NumRUT, true);
                var CantidadInhabilitado = await _UsuarioService.Existe(_UsuarioNuevoDTO.Usu_NumRUT, false);

                if (CantidadHabilitado > 0)
                {
                    return BadRequest(new RegistrationResponseDto { Errors = "Usuario ya existente" });
                }

                if (CantidadInhabilitado > 0)
                {
                    return BadRequest(new RegistrationResponseDto { Errors = "Usuario ya existente con condición Inhabilitado." });
                }

                var Usuario = _mapper.Map<Ent_Usuario>(_UsuarioNuevoDTO);
                
                Usuario.Usu_Clave = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10).ToUpper();

                Usuario.Usu_Id = await _UsuarioService.Crea(Usuario);

                await EnviarCorreo("Registro de Usuario","0.3.1",Usuario.Usu_Email,Usuario.Usu_NomApeRazSoc,Usuario.Usu_Clave);

                var UsuarioDTO = _mapper.Map<UsuarioDto>(Usuario);

                return CreatedAtRoute("Obten_x_Id", new { Usu_Id = Usuario.Usu_Id }, UsuarioDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Actualiza(int Usu_Id, [FromBody] UsuarioActualizadoDTO _UsuarioActualizadoDTO)
        {
            try
            {
                if (_UsuarioActualizadoDTO is null)
                {
                    return BadRequest("El usuario es nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Usuario inválido.");
                }

                var UsuarioExistente = await _UsuarioService.Obten_x_Id(Usu_Id);

                if (UsuarioExistente is null)
                {
                    return BadRequest(new RegistrationResponseDto { Errors = "Usuario no registrado." });
                }

                var CantidadHabilitado = await _UsuarioService.Existente(Usu_Id, _UsuarioActualizadoDTO.Usu_NumRUT, true);
                var CantidadInhabilitado = await _UsuarioService.Existente(Usu_Id, _UsuarioActualizadoDTO.Usu_NumRUT, false);

                if (CantidadHabilitado > 0)
                {
                    return BadRequest(new RegistrationResponseDto { Errors = "Usuario ya existente" });
                }

                if (CantidadInhabilitado > 0)
                {
                    return BadRequest(new RegistrationResponseDto { Errors = "Usuario ya existente con condición inhabilitado." });
                }

                _mapper.Map(_UsuarioActualizadoDTO, UsuarioExistente);

                UsuarioExistente.Usu_Id = Usu_Id;

                await _UsuarioService.Actualiza(UsuarioExistente);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        private async Task EnviarCorreo(string NombreNotificacion, string VersionApp, string EmailDestino, string Usu_Nombre, string ClaveAcceso)
        {
            var message = new MimeMessage();

            var ListMail = new InternetAddressList();

            message.From.Add(new MailboxAddress("Home Track", "hometrack_notificaciones@ibrlatam.com"));

            ListMail.Add(MailboxAddress.Parse(EmailDestino));

            message.To.AddRange(ListMail);
            message.Subject = $"Notificación | {NombreNotificacion}";

            var textPart = new TextPart(TextFormat.Html)
            {
                Text = CreateBody(VersionApp, Usu_Nombre, ClaveAcceso)
            };

            message.Body = textPart;

            using var client = new SmtpClient();

            await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.Auto);
            await client.AuthenticateAsync("hometrack_notificaciones@ibrlatam.com", "Hag94602");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private static string CreateBody(string VersionApp, string Usu_Nombre, string ClaveAcceso)
        {
            string body;

            using StreamReader reader = new(@".\index.html");

            body = reader.ReadToEnd();
            body = body.Replace("{Txt1}", Usu_Nombre);
            body = body.Replace("{Txt2}", ClaveAcceso);
            body = body.Replace("{Txt3}", $"Aplicación de Créditos Hipotecarios | Home Track v{VersionApp}.");

            return body;
        }
    }
}