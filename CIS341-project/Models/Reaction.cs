using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class Reaction
    {
        [Key]
        public int ReactionId { get; set; }

        public enum ReactionType
        {
            Upvote,
            Downvote
        }

        [Required]
        public ReactionType Type { get; set; }

        public Account ReactionAuthor { get; set; }
        public int ReactionAuthorId { get; set; }

        // navigation properties for linking reactions to specific posts
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
