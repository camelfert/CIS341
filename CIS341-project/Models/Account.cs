using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CIS341_project.Models
{
    public class Account
    {
        [BindNever]
        [Key]
        public int AccountId { get; set; }

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
