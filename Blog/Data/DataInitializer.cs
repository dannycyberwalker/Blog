using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class DataInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string userEmail = "user@gmail.com";
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "admin",
                    LastName = "admin",
                    NickName = "admin",
                    
                };
                User user = new User
                {
                    Email = userEmail,
                    UserName = userEmail,
                    FirstName = "user",
                    LastName = "user",
                    NickName = "user",
                    
                };
                IdentityResult userResult = await userManager.CreateAsync(user, password);
                IdentityResult adminResult = await userManager.CreateAsync(admin, password);
                if (adminResult.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
                
            }
        }
    }
}