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
    public class Estado_PropiedadController : ControllerBase
    {
        private readonly IEstado_PropiedadService _Estado_PropiedadService;
        private IMapper _mapper;

        public Estado_PropiedadController(IEstado_PropiedadService Estado_PropiedadService, IMapper mapper)
        {
            _Estado_PropiedadService = Estado_PropiedadService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Obten()
        {
            try
            {
                var Lst_Estado_Propiedad = await _Estado_PropiedadService.Obten();

                return Ok(_mapper.Map<IEnumerable<Estado_PropiedadDTO>>(Lst_Estado_Propiedad));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}