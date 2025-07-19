using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;
using Web.Domain.Interfaces;

namespace Web.Application.Service
{
	public class AuthService : IAuthService
	{

		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager)
		{
			var userClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id),
				new Claim(ClaimTypes.Name,$"{user.UserName}"),
				new Claim(ClaimTypes.Email,user.Email!),
			};

			var Roles = await _userManager.GetRolesAsync(user);
			foreach (var role in Roles)
			{
				userClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var authKeyInByets = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]!));

			var JwtObject = new JwtSecurityToken(
				issuer: _configuration["Token:Issuer"],
				audience: _configuration["Token:Audience"],
				claims: userClaims,
				expires: DateTime.Now.AddDays(double.Parse(_configuration["Token:ExpiryDays"]!)),
				signingCredentials: new SigningCredentials(authKeyInByets, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(JwtObject);
		}
	}
}

