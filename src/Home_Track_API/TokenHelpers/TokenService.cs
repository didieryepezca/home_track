﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Home_Track_API.TokenHelpers
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;
		private readonly IConfigurationSection _jwtSettings;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
			_jwtSettings = _configuration.GetSection("JwtSettings");
		}

		public SigningCredentials GetSigningCredentials()
		{
			var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
			var secret = new SymmetricSecurityKey(key);

			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}

		public List<Claim> GetClaims(Ent_Usuario Usuario)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, Usuario.Usu_Email),
				new Claim(ClaimTypes.Role, Usuario.eRol.Rol_Nombre)
			};

			return claims;
		}

		public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
		{
			var tokenOptions = new JwtSecurityToken(
				issuer: _jwtSettings.GetSection("validIssuer").Value,
				audience: _jwtSettings.GetSection("validAudience").Value,
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
				signingCredentials: signingCredentials);

			return tokenOptions;
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_jwtSettings["securityKey"])),
				ValidateLifetime = false,
				ValidIssuer = _jwtSettings["validIssuer"],
				ValidAudience = _jwtSettings["validAudience"],
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			SecurityToken securityToken;

			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
				StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}
