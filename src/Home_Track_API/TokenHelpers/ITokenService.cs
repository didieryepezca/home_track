using Microsoft.IdentityModel.Tokens;
using Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Home_Track_API.TokenHelpers
{
	public interface ITokenService
	{
		SigningCredentials GetSigningCredentials();

		List<Claim> GetClaims(Ent_Usuario Usuario);

		JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);

		string GenerateRefreshToken();

		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}
