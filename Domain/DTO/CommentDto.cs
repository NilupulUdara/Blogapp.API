namespace Blogapp.API.Domain.DTO
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid PostId { get; set; }
        public PostDto Post { get; set; }
    }
}