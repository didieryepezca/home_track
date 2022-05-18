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
using Home_Track_API.TokenHelpers;

namespace Home_Track_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;
        private IMapper _mapper;

        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;

        private readonly ITokenService _tokenService;

        public AccountsController(IConfiguration configuration, IUsuarioService UsuarioService, IMapper mapper, ITokenService tokenService)
        {
            _UsuarioService = UsuarioService;
            _mapper = mapper;

            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");

            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var Usuario = await _UsuarioService.Obten_x_Email(userForAuthentication.Adm_Email);

            if (Usuario == null || await _UsuarioService.Obten_Login(userForAuthentication.Adm_Email, userForAuthentication.Adm_Contra) == 0)
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Autenticación no válida." });

            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = _tokenService.GetClaims(Usuario);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            Usuario.Usu_TokenActualizado = _tokenService.GenerateRefreshToken();
            Usuario.Usu_FecHorTokenActualizado = DateTime.Now.AddDays(3);

            await _UsuarioService.Actualiza_Token(Usuario);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, RefreshToken = Usuario.Usu_TokenActualizado });
        }
    }
}
