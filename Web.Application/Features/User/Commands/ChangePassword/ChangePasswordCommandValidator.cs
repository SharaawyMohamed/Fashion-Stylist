using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Features.User.Commands.ChangePassword
{
	public class ChangePasswordCommandValidator:AbstractValidator<ChangePasswordCommand>
	{
		public ChangePasswordCommandValidator()
		{
			RuleFor(x => x.OldPassword)
			.NotEmpty().WithMessage("Old password is required.");

			RuleFor(x => x.NewPassword)
				.NotEmpty().WithMessage("New password is required.")
				.MinimumLength(6).WithMessage("New password must be at least 6 characters.")
				.Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
				.Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
				.Matches(@"\d").WithMessage("New password must contain at least one digit.")
				.Matches(@"[\W_]").WithMessage("New password must contain at least one special character.");

			RuleFor(x => x.NewPasswordComfirmation)
				.Equal(x => x.NewPassword)
				.WithMessage("Password confirmation does not match.");
		}
	}
}
