using Microsoft.AspNetCore.Identity;

namespace CIS341_project.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(string userId, string userName)> GetUserDetailsAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return (user?.Id, user?.UserName);
        }
    }

    public interface IUserService
    {
        Task<(string userId, string userName)> GetUserDetailsAsync();
    }
}
