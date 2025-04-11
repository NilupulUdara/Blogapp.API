using System.ComponentModel.DataAnnotations;

namespace Blogapp.API.Domain.DTO
{
    public class UpdateCommentDto
    {
        [Required]
        public string Author { get; set; }
        
        [Required]
        public string Message { get; set; }
    }
}