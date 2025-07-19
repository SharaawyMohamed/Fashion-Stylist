using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Web.Domain.Entites;

public static class IdentitySeeder
{
	public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
	{
		var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		string adminEmail = "abdullhahmed107@gmail.com";
		string adminPassword = "Admin@123";
		string adminRole = "Admin";

		if (!await roleManager.RoleExistsAsync(adminRole))
		{
			await roleManager.CreateAsync(new IdentityRole(adminRole));
		}

		var user = await userManager.FindByEmailAsync(adminEmail);
		if (user == null)
		{
			var newUser = new AppUser
			{
				UserName = adminEmail,
				Email = adminEmail,
				FullName = "Admin User",
				EmailConfirmed = true,
				FCM_Token = "SeededToken",
				ProfilePicture = "default.png"
			};

			var result = await userManager.CreateAsync(newUser, adminPassword);
			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(newUser, adminRole);
			}
			else
			{
				throw new Exception("Failed to create seed admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
			}
		}
	}
}
