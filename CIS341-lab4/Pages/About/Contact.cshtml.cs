using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIS341_lab4.Pages.About
{
    public class ContactModel : PageModel
    {

        [BindProperty]
        public ContactForm Contact { get; set; }

        public IActionResult OnPost()
        {
            Console.WriteLine($"Name: {Contact.Name}");
            Console.WriteLine($"Email: {Contact.Email}");
            Console.WriteLine($"Message: {Contact.Message}");

            return RedirectToPage("/About/Thanks");

        }
    }
}
