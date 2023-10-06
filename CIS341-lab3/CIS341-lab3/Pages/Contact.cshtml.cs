using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIS341_lab3.Pages
{
    public class ContactModel : PageModel
    {

        [BindProperty]
        public ContactForm? Contact { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            // "For debug purposes, the request handler method in the page model should print the bound model data
            // to the console and then redirect the user back to the Index page."
            Console.WriteLine($"Name: {Contact.Name}");
            Console.WriteLine($"Email: {Contact.Email}");
            Console.WriteLine($"Message: {Contact.Message}");
                
            return RedirectToPage("/Index");

        }
    }

    public class ContactForm
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
    }
}
