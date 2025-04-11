using AutoMapper;
using Blogapp.API.Domain.DTO;
using Blogapp.API.Domain.Model;

namespace Blogapp.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Post, AddPostRequestDto>().ReverseMap();
            CreateMap<Post, UpdatePostDto>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, AddCommentRequestDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
        }
    }
}