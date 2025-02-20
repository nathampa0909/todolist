using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Application.DTOs;

namespace ToDoList.Application.Utils;

public static class JwtUtils
{
	public static DTOTokenResponse GenerateJwtToken(string username, IConfiguration configuration)
	{
		var jwtSettings = configuration.GetSection("Jwt");
		var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
		var claims = new Dictionary<string, object>
		{
			{ "Sub", username },
			{ "Jti", Guid.NewGuid().ToString() }
		};

		var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
		var expireTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSettings["ExpireMinutes"]));

		var handler = new JwtSecurityTokenHandler();

		var jwtSecurityToken = new SecurityTokenDescriptor()
		{
			Issuer = jwtSettings["Issuer"],
			Audience = jwtSettings["Audience"],
			Claims = claims,
			Expires = expireTime,
			SigningCredentials = credentials
		};

		var tokenCreated = handler.CreateToken(jwtSecurityToken);
		var token = handler.WriteToken(tokenCreated);
		var dto = new DTOTokenResponse()
		{
			Token = token,
			CreatedAt = DateTime.UtcNow,
			DueDate = expireTime
		};

		return dto;
	}
}