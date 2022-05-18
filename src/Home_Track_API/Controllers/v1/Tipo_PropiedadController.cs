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
    public class Tipo_PropiedadController : ControllerBase
    {
        private readonly ITipo_PropiedadService _Tipo_PropiedadService;
        private IMapper _mapper;

        public Tipo_PropiedadController(ITipo_PropiedadService Tipo_PropiedadService, IMapper mapper)
        {
            _Tipo_PropiedadService = Tipo_PropiedadService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Obten()
        {
            try
            {
                var Lst_Tipo_Propiedad = await _Tipo_PropiedadService.Obten();

                return Ok(_mapper.Map<IEnumerable<Tipo_PropiedadDTO>>(Lst_Tipo_Propiedad));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}