namespace CIS341_project.ViewModels
{
    public class BlogPostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
        public DateTime DatePublished { get; set; }
        public string PostAuthor { get; set; }
        public int CommentCount { get; set; }
    }
}
