using CIS341_project.Models;
using System.ComponentModel.DataAnnotations;

namespace CIS341_project.ViewModels
{
    public class CommentDTO
    {
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Comments cannot be empty/blank.")]
        [StringLength(300, ErrorMessage = "Comments must be less than 300 characters in length.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        public string CommentContent { get; set; }

        public int BlogPostId { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public int? ParentCommentId { get; set; }

        public List<CommentDTO> Replies { get; set; } = new List<CommentDTO>();
    }
}