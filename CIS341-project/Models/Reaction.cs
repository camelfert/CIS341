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
    }
}
