using System.ComponentModel.DataAnnotations;

namespace  Blogapp.API.Domain.DTO
{
    public class AddPostRequestDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Content has to be minimum length of 3 characters")]
        public string Content { get; set; }
    }    
}