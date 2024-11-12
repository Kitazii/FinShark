using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsAsync();
        Task<Comment?> GetCommentAsync(int id);
        Task<Comment> CreateCommentAsync(Comment commentModel);
        Task<Comment?> UpdateCommentAsync(int id, Comment commentModel);
        Task<Comment?> DeleteCommentAsync(int id);
    }
}