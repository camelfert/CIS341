using CIS341_project.Data;
using CIS341_project.Models;

namespace CIS341_project.Areas.Identity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BlogAuthenticationContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }
            
            context.SaveChanges();
        }
    }
}
