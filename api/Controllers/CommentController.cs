using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo,
        UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetComments([FromQuery]CommentQueryObject queryObject)
        {
            var commentsModel = await _commentRepo.GetCommentsAsync(queryObject);
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

        [HttpPost("{symbol}")]
        public async Task<IActionResult> CreateComment([FromRoute] string symbol, CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);

                if(stock == null) return BadRequest("Stock does not exist");

                await _stockRepo.CreateStockAsync(stock);
            }

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            Comment? commentModel = commentDto.ToCommentFromCreateDto(stock.Id);

            commentModel.AppUserId = appUser.Id;

            await _commentRepo.CreateCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetComment), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            Comment? commentModel = await _commentRepo.UpdateCommentAsync(id, commentDto.ToCommentFromUpdateDto(id));

            if (commentModel == null) return NotFound("Comment Not Found");

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