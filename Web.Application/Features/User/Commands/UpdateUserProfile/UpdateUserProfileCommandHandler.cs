using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Features.User.Queries.GetUser;
using Web.Application.Utility;
using Web.Domain.Entites;
using Web.Domain.Interfaces;
using Web.Domain.Response;

namespace Web.Application.Features.User.Commands.UpdateUserProfile
{
	public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, BaseResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IMediaService _mediaService;
		private readonly IValidator<UpdateUserProfileCommand> _validator;
		private readonly IHttpContextAccessor _contextAccessor;

		public UpdateUserProfileCommandHandler(UserManager<AppUser> userManager, IMediaService mediaService, IValidator<UpdateUserProfileCommand> validator, IHttpContextAccessor contextAccessor)
		{
			_userManager = userManager;
			_mediaService = mediaService;
			_validator = validator;
			_contextAccessor = contextAccessor;
		}

		public async Task<BaseResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
		{
			var validatonResult = await _validator.ValidateAsync(request);
			if (!validatonResult.IsValid)
			{
				return await BaseResponse.ValidationError(validatonResult.Errors.Select(x => x.ErrorMessage).ToList(), "Validation Error", System.Net.HttpStatusCode.BadRequest);
			}

			var currentUser = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			var user = await _userManager.FindByIdAsync(currentUser.Id);
			if (currentUser == null || user == null)
			{
				var errors = new List<string> { "UnAuthorized!" };
				return await BaseResponse.Fail(errors, statusCode: System.Net.HttpStatusCode.Unauthorized);
			}

			user.FullName = request.FullName ?? user.FullName;
			user.Email = request.Email ?? user.Email;
			user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;

			if (request.ProfilePicture != null)
			{
				if (user.ProfilePicture != null)
				{
					await _mediaService.DeleteAsync(user.ProfilePicture);
				}

				var profilePicture = await _mediaService.UploadImageAsync(request.ProfilePicture);
				if (profilePicture == null)
				{
					var errors = new List<string> { "Profile picture upload failed!, please try later." };
					return await BaseResponse.Fail(errors);
				}

				user.ProfilePicture = profilePicture;
			}
			await _userManager.UpdateAsync(user);
			return await BaseResponse.Success(user.Adapt<GetUserQueryDto>());
		}
	}
}
