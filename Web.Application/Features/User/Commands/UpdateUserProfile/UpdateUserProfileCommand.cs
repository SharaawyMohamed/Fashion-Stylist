using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Response;

namespace Web.Application.Features.User.Commands.UpdateUserProfile
{
	public record UpdateUserProfileCommand(string? FullName,IFormFile? ProfilePicture, string? Email, string? PhoneNumber) : IRequest<BaseResponse>;
}
