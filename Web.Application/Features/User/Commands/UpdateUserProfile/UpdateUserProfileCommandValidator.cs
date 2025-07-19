using FluentValidation;
using Microsoft.AspNetCore.Http;
using Web.Application.Features.User.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
	public UpdateUserProfileCommandValidator()
	{
		RuleFor(x => x.FullName)
			.NotEmpty().WithMessage("Full name is required.")
			.MaximumLength(100).WithMessage("Full name must be less than 100 characters.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email is not valid.");

		RuleFor(x => x.PhoneNumber)
			.NotEmpty().WithMessage("Phone number is required.")
			.Matches(@"^\+?[0-9]{7,15}$").WithMessage("Invalid phone number format. Accepted format: +123456789 or 123456789.");

		RuleFor(x => x.ProfilePicture)
			.Must(BeAValidImage).When(x => x.ProfilePicture != null)
			.WithMessage("Only JPEG and PNG files are allowed.");
	}

	private bool BeAValidImage(IFormFile? file)
	{
		if (file == null) return true;

		var allowedContentTypes = new[] { "image/jpeg", "image/png" };
		return allowedContentTypes.Contains(file.ContentType);
	}
}