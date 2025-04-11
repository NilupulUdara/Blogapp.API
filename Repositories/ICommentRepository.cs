using Blogapp.API.Domain.Model;

namespace  Blogapp.API.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> CreateAsync(Comment comment);
        Task<List<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(Guid id);
        Task<Comment> UpdateAsync(Guid id, Comment comment);
        Task<Comment> DeleteAsync(Guid id);
    }
}