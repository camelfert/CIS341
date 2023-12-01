using CIS341_project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CIS341_project.Data
{
    public class InitializeUsersRoles
    {
        private readonly static string AdministratorRole = "Admin";
        private readonly static string BannedRole = "Banned";
        private readonly static string Password = "AdminPass123!";

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BlogContext(serviceProvider.GetRequiredService<DbContextOptions<BlogContext>>()))
            {
                // Create identity user
                var adminID = await EnsureUser(serviceProvider, Password, "admin@lunchbox.com");
                // Add to role
                await EnsureRole(serviceProvider, adminID, AdministratorRole);

                // create Banned role, but do not assign any user to it by default
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleManager.CreateAsync(new IdentityRole(BannedRole));
            }
        }

        // Check that user exists with provided email address --> create new user if none exists
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string userPw, string UserName)
        {
            // Access the UserManager service
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            if(userManager != null)
            {
                // Find user by email address
                var user = await userManager.FindByNameAsync(UserName);
                if (user == null)
                {
                    // Create new user if none exists
                    user = new IdentityUser { UserName = UserName, Email = UserName };
                    await userManager.CreateAsync(user, userPw);
                }

                // Confirm the new user so that we can log in
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                return user.Id;
            }
            else
                throw new Exception("userManager null");
        }


        // Check that role exists --> create new rule if none exists
        private static async Task EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            // Access RoleManager service
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager != null)
            {
                // Check whether role exists --> if not, create new role with the provided role name
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Retrieve user with the provided ID and add to the specified role
                var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
                if (userManager != null)
                {
                    var user = await userManager.FindByIdAsync(uid);
                    if(user is not null)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
                else
                    throw new Exception("userManager null");
                
            }
            else
                throw new Exception("roleManager null");
        }
    }
}
