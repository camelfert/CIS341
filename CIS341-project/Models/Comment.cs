using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual ICollection<Reaction> Reactions { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }

        [ForeignKey(nameof(ParentComment))]
        public int? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }

        [Required]
        public virtual Account Author { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public int AuthorAccountId { get; set; }

        public int BlogPostId { get; set; }
        [ForeignKey(nameof(BlogPostId))]
        public virtual BlogPost BlogPost { get; set; }

    }
}
