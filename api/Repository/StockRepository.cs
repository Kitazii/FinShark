using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
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
        public Task<List<Stock>> GetStocksAsync()
        {
            return _context.Stocks.ToListAsync();
        }
    }
}