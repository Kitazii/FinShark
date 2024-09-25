using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var commentsModel = await _commentRepo.GetCommentsAsync();
            var CommentsDto = commentsModel.Select(s => s.ToCommentDto());

            return Ok(commentsModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            var commentModel = await _commentRepo.GetCommentAsync(id);

            if (commentModel == null) return NotFound();

            return Ok(commentModel.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateCommentRequestDto commentDto)
        {
            if (!await _stockRepo.StockExists(stockId))
                return BadRequest("Stock does not exist");

            Comment? commentModel = commentDto.ToCommentFromCreateDto(stockId);

            await _commentRepo.CreateCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetComment), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            Comment? commentModel = await _commentRepo.UpdateCommentAsync(id, commentDto);

            if (commentModel == null) return NotFound();

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            Comment? commentModel = await _commentRepo.DeleteCommentAsync(id);

            if (commentModel == null) return NotFound();

            return NoContent();
        }
    }
}