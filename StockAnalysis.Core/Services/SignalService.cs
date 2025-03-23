// FETCHING AND STORING SIGNALS

using StockAnalysis.Core.Services;
using StockAnalysis.Core.Repositories;
using StockAnalysis.Models;

namespace StockAnalysis.Core.Services
{
    public interface ISignalService
    {
        Task<List<StockSignal>> GetSignalsAsync(string symbol, DateTime startDate, DateTime endDate);
        Task<StockSignal> GetLatestSignalAsync(string symbol);
        Task RefreshSignalsAsync(string symbol, DateTime startDate, DateTime endDate);
    }

    public class SignalService : ISignalService
    {
        private readonly ISignalRepository _repository;
        private readonly IModelIntegrationService _modelIntegrationService;

        public SignalService(
            ISignalRepository repository, 
            IModelIntegrationService modelIntegrationService)
        {
            _repository = repository;
            _modelIntegrationService = modelIntegrationService;
        }

        public async Task<List<StockSignal>> GetSignalsAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            return await _repository.GetSignalsAsync(symbol, startDate, endDate);
        }

        public async Task<StockSignal> GetLatestSignalAsync(string symbol)
        {
            return await _repository.GetLatestSignalAsync(symbol);
        }

        public async Task RefreshSignalsAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            // Get fresh signals from the model
            var signals = await _modelIntegrationService.GetSignalsAsync(symbol, startDate, endDate);
            
            // Save them to the database
            await _repository.SaveSignalsAsync(signals);
        }
    }
}