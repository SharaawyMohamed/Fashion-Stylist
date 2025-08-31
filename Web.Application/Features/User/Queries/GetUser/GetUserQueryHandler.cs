using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Web.Domain.Entites;
using Web.Domain.Interfaces;
using Web.Domain.Response;

namespace Web.Application.Features.User.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, BaseResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthService _authService;
        public GetUserQueryHandler(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IAuthService authService)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _authService = authService;
        }

        public async Task<BaseResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {

            var user = await Utility.GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null)
            {
                var errors = new List<string>
                {
                    "UnAuthorized!"
                };
                return await BaseResponse.Fail(errors, statusCode: HttpStatusCode.Unauthorized);
            };
            //var response = user.Adapt<GetUserQueryDto>();
            var response = new GetUserQueryDto
            {
                FullName = user.FullName,
                ProfilePicture = user.ProfilePicture,
                FCM_Token = user.FCM_Token,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            response.Token = await _authService.CreateTokenAsync(user, _userManager);
            return await BaseResponse.Success(response);
        }
    }
}
