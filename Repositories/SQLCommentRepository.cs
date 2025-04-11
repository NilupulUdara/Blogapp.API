using Blogapp.API.Data;
using Blogapp.API.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Blogapp.API.Repositories
{
    public class SQLCommentRepository : ICommentRepository
    {
        private readonly BlogDbContext blogDbContext;

        public SQLCommentRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await blogDbContext.Comments.AddAsync(comment);
            await blogDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> DeleteAsync(Guid id)
        {
            var exitingPost = await blogDbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if(exitingPost == null)
            {
                return null;
            }

            blogDbContext.Comments.Remove(exitingPost);
            await blogDbContext.SaveChangesAsync();
            return exitingPost;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await blogDbContext.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await  blogDbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Comment> UpdateAsync(Guid id, Comment comment)
        {
            var exitingPost = await blogDbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if(exitingPost == null)
            {
                return null;
            }

            exitingPost.Message = comment.Message;

            await blogDbContext.SaveChangesAsync();
            return exitingPost;
        }
    }
}