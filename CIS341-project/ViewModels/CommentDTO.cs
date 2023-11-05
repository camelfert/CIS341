using CIS341_project.Models;

namespace CIS341_project.ViewModels
{
    public class CommentDTO
    {
        public int CommentId { get; set; }

        public string? CommentContent { get; set; }

        public string? Author { get; set; }

        public int? UpvoteCount { get; set; }
        public int? DownvoteCount { get; set; }

    }
}