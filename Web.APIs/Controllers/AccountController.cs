using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Domain.DTOs.AccountDTO;
using Web.Domain.Interfaces;
using Web.Domain.Response;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<BaseResponse>> Register([FromForm] RegisterDTO registerDTO)
        {
            return Ok(await _accountService.RegisterAsync(registerDTO));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse>> Login([FromBody] LoginDTO loginDto)
        {
            return Ok(await _accountService.LoginAsync(loginDto));
        }

        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<BaseResponse>> ForgetPassword([FromBody] ForgetPasswordDto request)
        {
            return Ok(await _accountService.ForgotPasswordAsync(request));
        }

        [HttpPost("VerifyOTP")]
        public async Task<ActionResult<BaseResponse>> VerifyOTP([FromBody] VerfiyCodeDto verify)
        {
            return Ok(await _accountService.VerifyOTPAsync(verify));
        }

        [HttpPut("ResetPassword")]
        public async Task<ActionResult<BaseResponse>> ResetPassword(ResetPasswordDto resetPassword)
        {
            return Ok(await _accountService.ResetPasswordAsync(resetPassword));
        }
        [Authorize]
        [HttpGet("GetAllTrans")]
        public async Task<ActionResult<ICollection<AccountTransactionDto>>> GetAllTrans()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(_accountService.GetAllTransAsync(userEmail));
        }
    }
}
