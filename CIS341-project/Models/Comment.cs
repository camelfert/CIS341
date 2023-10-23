using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class Comment
    {
        [BindNever]
        public int Id { get; set; }
        public string? CommentContent { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
