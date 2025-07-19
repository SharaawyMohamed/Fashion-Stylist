using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Utility;
using Web.Domain.Entites;
using Web.Domain.Response;

namespace Web.Application.Features.User.Commands.ResetFCMToken
{
	public class ResetFCMTokenCommandHandler : IRequestHandler<ResetFCMTokenCommand, BaseResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		public ResetFCMTokenCommandHandler(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
		}
		public async Task<BaseResponse> Handle(ResetFCMTokenCommand request, CancellationToken cancellationToken)
		{
			var currentUser = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			var user = await _userManager.FindByIdAsync(currentUser.Id);
			if (currentUser == null || user == null)
			{
				var errors = new List<string> { "UnAuthorized!" };
				return await BaseResponse.Fail(errors, statusCode: System.Net.HttpStatusCode.Unauthorized);
			}
			user.FCM_Token = request.fcmToken;
			await _userManager.UpdateAsync(user);
			return await BaseResponse.Success("FCM Token reset successfully.");
		}
	}

}
