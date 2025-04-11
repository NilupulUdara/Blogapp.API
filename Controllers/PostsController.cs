using AutoMapper;
using Blogapp.API.Domain.DTO;
using Blogapp.API.Domain.Model;
using Blogapp.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Blogapp.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await postRepository.GetAllAsync();

            return Ok(mapper.Map<List<PostDto>>(posts));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var post = await postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PostDto>(post));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPostRequestDto addPostRequestDto)
        {
            var postDomain = mapper.Map<Post>(addPostRequestDto);

            postDomain = await postRepository.CreateAsync(postDomain);

            var postDto = mapper.Map<PostDto>(postDomain);

            return CreatedAtAction(nameof(GetById), new { id = postDomain.Id }, postDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            var postDomain = mapper.Map<Post>(updatePostDto);

            postDomain = await postRepository.UpdateAsync(id, postDomain);

            if (postDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PostDto>(postDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var post = await postRepository.DeleteAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PostDto>(post));
        }
    }
}