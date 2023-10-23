using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class BlogPost 
    {
        [BindNever]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
