namespace Blogapp.API.Domain.Model
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Comment> Comments { get; set; }  = new List<Comment>();
    }
}