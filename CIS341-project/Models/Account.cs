using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
    }
}
