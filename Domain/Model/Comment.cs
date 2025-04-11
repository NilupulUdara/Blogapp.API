using System.Text.Json.Serialization;

namespace Blogapp.API.Domain.Model
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid PostId { get; set; }

        public Post Post { get; set; }
    }
}