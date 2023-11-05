using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class BlogPost 
    {
        [BindNever]
        [Key]
        public int BlogPostId { get; set; }

        [Display(Name = "Author")]
        public Account PostAuthor { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Post Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime DatePublished { get; set; }

        public ICollection<Reaction> Reactions { get; set; }

        public int UpvoteCount { get; set; }

        public int DownvoteCount { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }

}
