using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        //dependency injection
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            Stock? stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel == null) return null;

            _context.Stocks.Remove(stockModel);

            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> GetStockAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            return await _context.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto updateStockDto)
        {
            Stock? stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel == null) return null;

            stockModel.UpdateFromDto(updateStockDto);

            await _context.SaveChangesAsync();

            return stockModel;
        }
    }
}