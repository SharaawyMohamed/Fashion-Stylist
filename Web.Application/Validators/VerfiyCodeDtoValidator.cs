using FluentValidation;
using Web.Domain.DTOs.AccountDTO;

namespace Web.Application.Validators
{
	public class VerfiyCodeDtoValidator : AbstractValidator<VerfiyCodeDto>
	{
		public VerfiyCodeDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email address is required")
				.EmailAddress().WithMessage("A valid email address is required")
				.MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

			RuleFor(x => x.OTP)
				.NotEmpty().WithMessage("OTP code is required")
				.Length(6).WithMessage("OTP must be exactly 6 digits")
				.Matches("^[0-9]+$").WithMessage("OTP must contain only numbers");
		}
	}
}