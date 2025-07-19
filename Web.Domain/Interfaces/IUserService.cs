//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Web.Domain.DTOs.UserDTO;

//namespace Web.Domain.Interfaces
//{
//	public interface IUserService
//	{
//		Task<BaseResponse<UserDto>> GetUserDetailsAsync(string userId);
//		Task<BaseResponse<List<UserDto>>> GetAllUsersAsync();
//		Task<BaseResponse<bool>> EditUserAsync(UserDto model);

//		Task<BaseResponse<bool>> LockUserByEmailAsync(string email);
//		Task<BaseResponse<bool>> UnlockUserByEmailAsync(string email);
//		Task<BaseResponse<bool>> DeleteUserByEmailAsync(string email);
//	}
//}
