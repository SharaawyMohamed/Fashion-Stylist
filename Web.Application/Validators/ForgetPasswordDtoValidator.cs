using FluentValidation;
using Web.Domain.DTOs.AccountDTO;

namespace Web.Application.Validators
{
	public class ForgetPasswordDtoValidator : AbstractValidator<ForgetPasswordDto>
	{
		public ForgetPasswordDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email address is required")
				.EmailAddress().WithMessage("A valid email address is required")
				.MaximumLength(100).WithMessage("Email address cannot exceed 100 characters");
		}
	}
}