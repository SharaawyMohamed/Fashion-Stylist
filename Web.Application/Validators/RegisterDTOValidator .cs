using FluentValidation;
using Microsoft.AspNetCore.Http;
using Web.Domain.DTOs.AccountDTO;

namespace Web.APIs.Validators
{
	public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
	{
		public RegisterDTOValidator()
		{
			RuleFor(x => x.FullName)
				.NotEmpty()
				.MinimumLength(3)
				.MaximumLength(100);

			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress()
				.MaximumLength(100);

			RuleFor(x => x.PhoneNumber)
				.NotEmpty()
				.Matches(@"^\+?[0-9\s-]{10,}$");

			RuleFor(x => x.Password)
				.NotEmpty()
				.MinimumLength(8)
				.Matches("[A-Z]")
				.Matches("[a-z]")
				.Matches("[0-9]")
				.Matches("[^a-zA-Z0-9]");

			RuleFor(x => x.ProfilePicture)
				.Must(BeAValidImage).When(x => x.ProfilePicture != null)
				.Must(BeUnderMaxSize).When(x => x.ProfilePicture != null);
		}

		private bool BeAValidImage(IFormFile file)
		{
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
			var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
			return allowedExtensions.Contains(extension);
		}

		private bool BeUnderMaxSize(IFormFile file)
		{
			const int maxSizeInBytes = 5 * 1024 * 1024;
			return file.Length <= maxSizeInBytes;
		}
	}
}
