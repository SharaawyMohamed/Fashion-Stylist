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
        Task<BaseResponse> AddTransAsync(string UserId, int OrderId, decimal Amount);
        Task<ICollection<AccountTransactionDto>> GetAllTransAsync(string UserId);
    }
}
