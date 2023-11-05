using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace CIS341_project.Models
{
    public class Account
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
