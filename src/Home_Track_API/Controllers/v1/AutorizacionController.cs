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

namespace Home_Track_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AutorizacionController : ControllerBase
    {
        private readonly IAutorizacionService _AutorizacionService;
        private IMapper _mapper;

        public AutorizacionController(IAutorizacionService AutorizacionService, IMapper mapper)
        {
            _AutorizacionService = AutorizacionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Obten_Paginado(string Rol_Nombre, string Mod_Nombre, string Ope_Nombre)
        {
            try
            {
                return Ok(await _AutorizacionService.Obten_Cantidad(Rol_Nombre, Mod_Nombre, Ope_Nombre));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
