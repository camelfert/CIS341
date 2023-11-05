using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class Comment
    {
        [BindNever]
        [Key]
        public int CommentId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        public string? CommentContent { get; set; }

        [Required]
        public Account Author { get; set; }

        public ICollection<Reaction> Reactions { get; set; }

        public ICollection<Comment> Replies { get; set; }

        // keeping these here in case necessary later on to attach
        // comments to specific posts
        //public int BlogPostId { get; set; }
        //public BlogPost BlogPost { get; set; }
    }
}
