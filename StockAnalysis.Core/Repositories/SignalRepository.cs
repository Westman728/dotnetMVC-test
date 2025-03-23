using Microsoft.EntityFrameworkCore;
using StockAnalysis.Models;

namespace StockAnalysis.Core.Repositories
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<StockSignal> Signals { get; set; }
    }

    public interface ISignalRepository
    {
        Task<List<StockSignal>> GetSignalsAsync(string symbol, DateTime startDate, DateTime endDate);
        Task<StockSignal> GetLatestSignalAsync(string symbol);
        Task SaveSignalAsync(StockSignal signal);
        Task SaveSignalsAsync(List<StockSignal> signals);
    }

    public class SignalRepository : ISignalRepository
    {
        private readonly ApplicationDbContext _context;

        public SignalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockSignal>> GetSignalsAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            return await _context.Signals
                .Where(s => s.Symbol == symbol && s.TimeStamp >= startDate && s.TimeStamp <= endDate)
                .OrderBy(s => s.TimeStamp)
                .ToListAsync();
        }

        public async Task<StockSignal> GetLatestSignalAsync(string symbol)
        {
            return await _context.Signals
                .Where(s => s.Symbol == symbol)
                .OrderByDescending(s => s.TimeStamp)
                .FirstOrDefaultAsync();
        }

        public async Task SaveSignalAsync(StockSignal signal)
        {
            _context.Signals.Add(signal);
            await _context.SaveChangesAsync();
        }

        public async Task SaveSignalsAsync(List<StockSignal> signals)
        {
            _context.Signals.AddRange(signals);
            await _context.SaveChangesAsync();
        }
    }
}