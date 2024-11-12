using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo) { _stockRepo = stockRepo; }

        [HttpGet]
        public async Task<IActionResult> GetStocks([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var stocks = await _stockRepo.GetStocksAsync(query);
            var stocksDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStock([FromRoute] int id)
        {
            Stock? stock = await _stockRepo.GetStockAsync(id);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            Stock stockModel = stockDto.ToStockFromCreateDto();

            await _stockRepo.CreateStockAsync(stockModel);

            return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        //[Route("{id}")] //same as declaring it in the put, use case is better when you have multiple vars
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            Stock? stockModel = await _stockRepo.UpdateStockAsync(id, updateDto);

            if (stockModel == null) return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete("{id}")]
        //[Route("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            Stock? stockModel = await _stockRepo.DeleteStockAsync(id);

            if (stockModel == null) return NotFound();

            return NoContent();
        }
    }
}