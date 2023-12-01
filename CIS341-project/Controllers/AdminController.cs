using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CIS341_project.ViewModels;

namespace CIS341_project.Controllers
{
    /// <summary>
    /// AdminController manages the functionality for the Admin Panel, which lists every user in the database and allows for role management.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Displays the admin dashboard with a list of users and their roles.
        /// </summary>
        /// <returns>The Index view with a list of users and their roles.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (IdentityUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }

        [Authorize(Roles = "Admin")]
        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        /// <summary>
        /// Displays the user role management view for a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user to manage roles for.</param>
        /// <returns>The Manage view if the user is found; otherwise, NotFound.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"A user with ID = {userId} could not be found.";
                return View("NotFound");
            }

            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        /// <summary>
        /// Processes the changes to a user's roles via the Manage View.
        /// </summary>
        /// <param name="model">The user roles view model containing the roles to be updated.</param>
        /// <param name="userId">The ID of the user whose roles are being managed.</param>
        /// <returns>Redirects to the Admin Panel Index View if successful; otherwise returns the current view with validation errors/error messages.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"A user with ID = {userId} could not be found.";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "There was a problem removing the selected roles from this user.");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "There was an error when trying to give user a role.");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}