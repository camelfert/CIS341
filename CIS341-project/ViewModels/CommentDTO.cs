using CIS341_project.Models;

namespace CIS341_project.ViewModels
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public int BlogPostId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public int? ParentCommentId { get; set; }
        public List<CommentDTO>? Replies { get; set; }
    }
}