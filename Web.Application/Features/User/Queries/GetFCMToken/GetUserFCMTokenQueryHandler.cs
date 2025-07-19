using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
using Web.Domain.Entites;
using Web.Domain.Response;

namespace Web.Application.Features.User.Queries.GetFCMToken
{
	public class GetUserFCMTokenQueryHandler : IRequestHandler<GetUserFCMTokenQuery, BaseResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IConfiguration _configuration;
		public GetUserFCMTokenQueryHandler(UserManager<AppUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		public async Task<BaseResponse> Handle(GetUserFCMTokenQuery request, CancellationToken cancellationToken)
		{
			var adminEmail = _configuration["Admin:Email"];
			var Admin = await _userManager.FindByEmailAsync(adminEmail);
			if (Admin == null)
			{
				var errors = new List<string> { $"We can't provide support now, please tray later." };
				return await BaseResponse.Fail(errors,statusCode:HttpStatusCode.NoContent);
			}

			return await BaseResponse.Success(Admin.FCM_Token);
		}
	}
}
