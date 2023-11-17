using System.ComponentModel.DataAnnotations;

namespace CIS341_project.ViewModels
{
    public class BlogPostDTO
    {

        public int BlogPostId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Post Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime DatePublished { get; set; } = DateTime.Now;

        [Display(Name = "Author")]
        public string PostAuthor { get; set; }

        public int CommentCount { get; set; }
    }
}
