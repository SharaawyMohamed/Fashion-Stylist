using FluentValidation;
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

namespace Web.Application.Features.User.Commands.ChangePassword
{
	public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, BaseResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IValidator<ChangePasswordCommand> _validator;
		public ChangePasswordCommandHandler(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IValidator<ChangePasswordCommand> validator)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_validator = validator;
		}

		public async Task<BaseResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(request);
			if (!validationResult.IsValid)
			{
				return await BaseResponse.ValidationError(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), "Validation Error", System.Net.HttpStatusCode.BadRequest);
			}

			var currentUser = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			var user = await _userManager.FindByIdAsync(currentUser.Id);
			if (currentUser == null || user==null)
			{
				var errors = new List<string> { "UnAuthorized!" };
				return await BaseResponse.Fail(errors, statusCode: System.Net.HttpStatusCode.Unauthorized);
			}

			var identityResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
			if (!identityResult.Succeeded)
			{
				var errors = identityResult.Errors.Select(x => x.Description).ToList();
				return await BaseResponse.Fail(errors, statusCode: System.Net.HttpStatusCode.BadRequest);
			}

			return await BaseResponse.Success(null, "Password changed successfully.");
		}
	}
}
