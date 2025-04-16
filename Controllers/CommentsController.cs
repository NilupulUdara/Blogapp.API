using AutoMapper;
using Blogapp.API.CustomActionFilter;
using Blogapp.API.Domain.DTO;
using Blogapp.API.Domain.Model;
using Blogapp.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogapp.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public CommentsController(ICommentRepository commentRepository, IPostRepository postRepository, IMapper mapper)
        {
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await commentRepository.GetAllAsync();

            return Ok(mapper.Map<List<CommentDto>>(comments));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var comment = await commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CommentDto>(comment));
        }

        [HttpPost]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] AddCommentRequestDto addCommentRequestDto)
        {
            var post = await postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound("Post not found.");
            }

            var commentDomain = mapper.Map<Comment>(addCommentRequestDto);

            commentDomain.PostId = id;
            commentDomain.Post = post;

            commentDomain = await commentRepository.CreateAsync(commentDomain);

            var commentDto = mapper.Map<CommentDto>(commentDomain);

            return CreatedAtAction(nameof(GetById), new { id = commentDomain.Id }, commentDto);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            var commentDomain = mapper.Map<Comment>(updateCommentDto);

            commentDomain = await commentRepository.UpdateAsync(id, commentDomain);

            if (commentDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CommentDto>(commentDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var comment = await commentRepository.DeleteAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CommentDto>(comment));
        }
    }
}