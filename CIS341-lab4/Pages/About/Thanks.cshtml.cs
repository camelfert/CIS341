using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIS341_lab4.Pages.About
{
    public class ThanksModel : PageModel
    {

        private readonly LinkGenerator _linkGenerator;

        public ThanksModel(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public IActionResult OnGet()
        {
            var contactPageLink = _linkGenerator.GetPathByPage("/About/Contact");
            ViewData["ContactPageLink"] = contactPageLink;
            return Page();
        }
    }
}
