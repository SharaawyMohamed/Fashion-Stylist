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
            if (currentUser == null)
            {
                var errors = new List<string> { "UnAuthorized!" };
                return await BaseResponse.Fail(errors, statusCode: System.Net.HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(currentUser.Id);
            if (user == null)
            {
                var errors = new List<string> { "UnAuthorized!" };
                return await BaseResponse.Fail(errors, statusCode: System.Net.HttpStatusCode.Unauthorized);
            }

            user.FCM_Token = request.fcmToken;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return await BaseResponse.Fail(errors, statusCode: System.Net.HttpStatusCode.BadRequest);
            }

            return await BaseResponse.Success("FCM Token reset successfully.");
        }

    }

}
