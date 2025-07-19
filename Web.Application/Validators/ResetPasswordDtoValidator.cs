using FluentValidation;
using Web.Domain.DTOs.AccountDTO;

namespace Web.Application.Validators
{
	public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
	{
		public ResetPasswordDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required")
				.EmailAddress().WithMessage("Valid email is required")
				.MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

			RuleFor(x => x.Token)
				.NotEmpty().WithMessage("Token is required");

			RuleFor(x => x.NewPassword)
				.NotEmpty().WithMessage("Password is required")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
				.Matches("[0-9]").WithMessage("Password must contain at least one number")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

			RuleFor(x => x.ConfirmNewPassword)
				.Equal(x => x.NewPassword)
				.WithMessage("Passwords do not match");
		}
	}
}