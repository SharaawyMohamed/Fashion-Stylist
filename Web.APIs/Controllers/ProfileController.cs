using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.User.Commands.ChangePassword;
using Web.Application.Features.User.Commands.ResetFCMToken;
using Web.Application.Features.User.Commands.UpdateUserProfile;
using Web.Application.Features.User.Queries.GetFCMToken;
using Web.Application.Features.User.Queries.GetUser;
using Web.Domain.Response;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("get-user")]
        public async Task<ActionResult<BaseResponse>> GetUser()
        {
            return Ok(await _mediator.Send(new GetUserQuery()));
        }


        [HttpGet("get-admin-fcm-token")]
        public async Task<ActionResult<BaseResponse>> GetFCM_Token()
        {
            return Ok(await _mediator.Send(new GetUserFCMTokenQuery()));
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateUserProfile([FromForm] UpdateUserProfileCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpPatch("{fcmToken?}")]
        public async Task<ActionResult<BaseResponse>> ResetFCMToken(string? fcmToken)
        {
            var command = new ResetFCMTokenCommand(fcmToken ?? string.Empty);
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<BaseResponse>> ChangePassword(ChangePasswordCommand changPassword)
        {
            return Ok(await _mediator.Send(changPassword));
        }
    }
}
