using System.IdentityModel.Tokens.Jwt;
using Home_Track_API.TokenHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Dto.v1;
using Service;

namespace Home_Track_API.Controllers.v1
{
	[Route("api/v{version:apiVersion}/[Controller]")]
	[ApiVersion("1")]
	[ApiController]
	public class TokenController : ControllerBase
	{
		private readonly IUsuarioService _UsuarioService;
		private readonly ITokenService _tokenService;

		public TokenController(IUsuarioService UsuarioService, ITokenService tokenService)
		{
			_UsuarioService = UsuarioService;
			_tokenService = tokenService;
		}

		[HttpPost]
		[Route("refresh")]
		public async Task<IActionResult> Refresh([FromBody]RefreshTokenDto tokenDto)
		{
			if (tokenDto is null)
			{
				return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Solicitud de cliente inválida" });
			}

			var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.Token);
			var username = principal.Identity.Name;

			var Usuario = await _UsuarioService.Obten_x_Email(username);

			if (Usuario == null || Usuario.Usu_TokenActualizado != tokenDto.RefreshToken || Usuario.Usu_FecHorTokenActualizado <= DateTime.Now)
				return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Solicitud de cliente inválida" });

			var signingCredentials = _tokenService.GetSigningCredentials();
			var claims = _tokenService.GetClaims(Usuario);
			var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			Usuario.Usu_TokenActualizado = _tokenService.GenerateRefreshToken();

			await _UsuarioService.Actualiza_Token(Usuario);

			return Ok(new AuthResponseDto { Token = token, RefreshToken = Usuario.Usu_TokenActualizado, IsAuthSuccessful = true });
		}
	}
}