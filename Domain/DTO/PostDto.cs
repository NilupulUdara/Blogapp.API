namespace  Blogapp.API.Domain.DTO
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<CommentDto> Comments { get; set; }  
    }    
}