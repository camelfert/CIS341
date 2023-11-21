using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIS341_project.Controllers
{
    public class AccountController : Controller
    {
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet("Account/AdminPanel")]
        [Authorize(Roles = "Admin, SuperUser")]
        public IActionResult AdminPanel()
        {
            return View("AdminPanel");
        }

    }
}
