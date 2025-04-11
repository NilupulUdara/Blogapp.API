using Blogapp.API.Data;
using Blogapp.API.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Blogapp.API.Repositories
{
    public class SQLPostRepository : IPostRepository
    {
        private readonly BlogDbContext blogDbContext;

        public SQLPostRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            await blogDbContext.Posts.AddAsync(post);
            await blogDbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post> DeleteAsync(Guid id)
        {
            var exitingPost = await blogDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (exitingPost == null)
            {
                return null;
            }

            blogDbContext.Posts.Remove(exitingPost);
            await blogDbContext.SaveChangesAsync();
            return exitingPost;
        }

        // public async Task<List<Post>> GetAllAsync()
        // {
        //     return await blogDbContext.Posts.ToListAsync();
        // }

        public async Task<List<Post>> GetAllAsync()
        {
            return await blogDbContext.Posts
                .Include(p => p.Comments) // 👈 This brings in the related comments
                .ToListAsync();
        }


        public async Task<Post?> GetByIdAsync(Guid id)
        {
            return await blogDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Post> UpdateAsync(Guid id, Post post)
        {
            var exitingPost = await blogDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (exitingPost == null)
            {
                return null;
            }

            exitingPost.Title = post.Title;
            exitingPost.Content = post.Content;

            await blogDbContext.SaveChangesAsync();
            return exitingPost;
        }
    }
}