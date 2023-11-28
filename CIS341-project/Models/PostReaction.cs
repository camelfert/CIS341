using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class PostReaction
    {
        [Key]
        public int ReactionId { get; set; }

        public enum ReactionType
        {
            Upvote = 0,
            Downvote = 1
        }

        public ReactionType Type { get; set; }

        public string ReactionAuthorId { get; set; }

        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
