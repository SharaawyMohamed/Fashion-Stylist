using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.DTOs.AccountDTO;
using Web.Domain.Response;

namespace Web.Domain.Interfaces
{
	public interface IAccountService
	{
		Task<BaseResponse> RegisterAsync(RegisterDTO registerDto);
		Task<BaseResponse> LoginAsync(LoginDTO loginDto);
		Task<BaseResponse> ForgotPasswordAsync(ForgetPasswordDto request);
		Task<BaseResponse> VerifyOTPAsync(VerfiyCodeDto verify);
		Task<BaseResponse> ResetPasswordAsync(ResetPasswordDto resetPassword);
	}
}
