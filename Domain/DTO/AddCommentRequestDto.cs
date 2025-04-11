using System.ComponentModel.DataAnnotations;

namespace Blogapp.API.Domain.DTO
{
  public class AddCommentRequestDto
  {
    [Required]
    public string Author { get; set; }
    [Required]
    public string Message { get; set; }
    //public Guid? PostId { get; set; }
   // public Post Post { get; set; }
  }
}