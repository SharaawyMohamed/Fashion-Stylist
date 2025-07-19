using FluentValidation;
using Web.Domain.DTOs.AccountDTO;

namespace Web.Application.Validators
{
	public class LoginDTOValidator : AbstractValidator<LoginDTO>
	{
		public LoginDTOValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email is required.")
				.MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters.");

			RuleFor(x => x.FCM_Token)
				.MaximumLength(500).When(x => !string.IsNullOrEmpty(x.FCM_Token))
				.WithMessage("FCM token cannot exceed 500 characters.");
		}
	}
}