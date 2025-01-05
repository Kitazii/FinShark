using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using api.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using api.Helpers;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        //dependency injection
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            Comment? commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (commentModel == null) return null;

            _context.Comments.Remove(commentModel);

            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> GetCommentAsync(int id)
        {
            return await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetCommentsAsync(CommentQueryObject queryObject)
        {
            var comments = _context.Comments.Include(c => c.AppUser).AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                comments = comments.Where(c => c.Stock.Symbol == queryObject.Symbol);
            }

            if (queryObject.IsDescending == true)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }

            return await comments.ToListAsync();
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comments.FindAsync(id);

            if (existingComment == null) return null;

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }
    }
}