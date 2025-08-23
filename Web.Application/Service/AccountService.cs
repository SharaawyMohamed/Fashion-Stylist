using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web.APIs.Validators;
using Web.Application.Service;
using Web.Domain.DTOs.AccountDTO;
using Web.Domain.Entites;
using Web.Domain.Enums;
using Web.Domain.Interfaces;
using Web.Domain.Response;

namespace Web.Infrastructure.Service
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IAuthService _authService;
		private readonly IMediaService _mediaService;
		private readonly IEmailService _emailService;
		private readonly IMemoryCache _memoryCache;
		private readonly IValidator<RegisterDTO> _registerValidator;
		private readonly IValidator<LoginDTO> _loginValidator;
		private readonly IValidator<ForgetPasswordDto> _forgetPasswordValidator;
		private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
		private readonly IValidator<VerfiyCodeDto> _verifyCodeValidator;

		public AccountService(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			IAuthService authService,
			IMemoryCache memoryCache,
			IEmailService emailService,
			IMediaService mediaService,
			IValidator<RegisterDTO> registerValidator,
			IValidator<LoginDTO> loginValidator,
			IValidator<ForgetPasswordDto> forgetPasswordValidator,
			IValidator<ResetPasswordDto> resetPasswordValidator,
			IValidator<VerfiyCodeDto> verifyCodeValidator)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_authService = authService;
			_memoryCache = memoryCache;
			_emailService = emailService;
			_mediaService = mediaService;
			_registerValidator = registerValidator;
			_loginValidator = loginValidator;
			_forgetPasswordValidator = forgetPasswordValidator;
			_resetPasswordValidator = resetPasswordValidator;
			_verifyCodeValidator = verifyCodeValidator;
		}


		public async Task<BaseResponse> RegisterAsync(RegisterDTO registerDto)
		{
			var validation=await _registerValidator.ValidateAsync(registerDto);
			if (!validation.IsValid)
			{
				return await BaseResponse.ValidationError(validation.Errors.Select(e => e.ErrorMessage).ToList(), "Validation failed!", HttpStatusCode.BadRequest);
			}

			var user = await _userManager.FindByEmailAsync(registerDto.Email);
			if(user != null)
			{
				var errors = new List<string> { "Email already exist!" };
				return await BaseResponse.Fail(errors);
			}

			var newUser = new AppUser()
			{
				FullName = registerDto.FullName,
				UserName = registerDto.Email.Split('@')[0],
				FCM_Token = registerDto.FCM_Token,
				Email = registerDto.Email,
			};

			if (registerDto.ProfilePicture != null)
			{
				var profilePicture = await _mediaService.UploadImageAsync(registerDto.ProfilePicture);
				if (profilePicture == null)
				{
					var errors = new List<string> { "Profile picture upload failed!" };
					return await BaseResponse.Fail(errors);
				}

				newUser.ProfilePicture = profilePicture;
			}

			var Result = await _userManager.CreateAsync(newUser, registerDto.Password);

			if (!Result.Succeeded)
			{
				var errors = Result.Errors.Select(E => E.Description).ToList();
				return await BaseResponse.Fail(errors, "UnExpected error!", HttpStatusCode.InternalServerError);
			}
			var Token = await _authService.CreateTokenAsync(newUser, _userManager);
			return await BaseResponse.Success(Token, "Your account created successfully!");
		}

		public async Task<BaseResponse> LoginAsync(LoginDTO loginDto)
		{
			var validation=await _loginValidator.ValidateAsync(loginDto);
			if (!validation.IsValid)
			{
				return await BaseResponse.ValidationError(validation.Errors.Select(x => x.ErrorMessage).ToList(), "Validation Error", HttpStatusCode.BadRequest);
			}

			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user == null)
			{
				var errors = new List<string> { "Email not found!." };
				return await BaseResponse.Fail(errors);
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

			if (!result.Succeeded)
			{
				var errors= new List<string> { "Invalid email or password!" };
				return await BaseResponse.Fail(errors);
			}

			user.FCM_Token=loginDto.FCM_Token;
		    await _userManager.UpdateAsync(user);
			var obj = new LoginResDto
            {
				UserId=user.Id,
				Token = await _authService.CreateTokenAsync(user, _userManager)
            };

			return await BaseResponse.Success(obj);
		}

		public async Task<BaseResponse> ForgotPasswordAsync(ForgetPasswordDto request)
		{
			var validation = await _forgetPasswordValidator.ValidateAsync(request);
			if (!validation.IsValid)
			{
				return await BaseResponse.ValidationError(validation.Errors.Select(e => e.ErrorMessage).ToList(), "Validation failed!", HttpStatusCode.BadRequest);
			}

			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				var errors = new List<string> { "Email not found!" };
				return await BaseResponse.Fail(errors);
			}

			var otp = new Random().Next(100000, 999999).ToString();
			_memoryCache.Set(request.Email, otp, TimeSpan.FromMinutes(5));
			await _emailService.SendEmailAsync(request.Email, "Fashion Ecommerce", $"OTP: {otp}");
			var Token = await _userManager.GeneratePasswordResetTokenAsync(user);

			return await BaseResponse.Success(Token, "Check your email!");
		}

		public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordDto resetPassword)
		{
			var validation=await _resetPasswordValidator.ValidateAsync(resetPassword);
			if (!validation.IsValid)
			{
				return await BaseResponse.ValidationError(validation.Errors.Select(e => e.ErrorMessage).ToList(), "Validation failed!", HttpStatusCode.BadRequest);
			}

			var user = await _userManager.FindByEmailAsync(resetPassword.Email);
			if (user == null)
			{
				var errors = new List<string> { "Email not found!" };
				return await BaseResponse.Fail(errors);
			}

			var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description).ToList();
				return await BaseResponse.Fail(errors,"Failed",HttpStatusCode.InternalServerError);
			}
			return await BaseResponse.Success(message: "Your password changed successfully!");
		}

		public async Task<BaseResponse> VerifyOTPAsync(VerfiyCodeDto verify)
		{
			var validation = await _verifyCodeValidator.ValidateAsync(verify);
			if (!validation.IsValid)
			{
				return await BaseResponse.ValidationError(validation.Errors.Select(e => e.ErrorMessage).ToList(), "Validation failed!", HttpStatusCode.BadRequest);
			}
			try
			{
				var user = await _userManager.FindByEmailAsync(verify.Email);
				if (user == null)
				{
					var errors = new List<string> { $"Your email '{verify.Email}' is not found!" };
					return await BaseResponse.Fail(errors);
				}

				var cachedOtp = _memoryCache.Get(verify.Email)?.ToString()!.Trim();
				if (string.IsNullOrEmpty(cachedOtp))
				{
					var errors = new List<string> { "OTP expired, please request a new one!" };
					return await BaseResponse.Fail(errors);
				}

				if (!string.Equals(verify.OTP.Trim(), cachedOtp, StringComparison.OrdinalIgnoreCase))
				{
					var errors = new List<string> { "Invalid OTP, please try again!" };
					return await BaseResponse.Fail(errors);
				}

				_memoryCache.Remove(verify.Email);

				return await BaseResponse.Success(message: "OTP confirmed successfully!");
			}
			catch (Exception ex)
			{
				var errors = new List<string> { ex.Message };
				return await BaseResponse.Fail(errors);
			}

		}
	}
}
