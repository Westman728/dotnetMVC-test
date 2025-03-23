// SERVICE TO INTERACT WITH THE PYTHON MODEL
using System.Diagnostics;
using System.Text.Json;
using StockAnalysis.Models;

namespace StockAnalysis.Core.Services
{
    public interface IModelIntegrationService
    {
        Task<List<StockSignal>> GetSignalsAsync(string symbol, DateTime startdate, DateTime enddate);
        Task<StockSignal> GetLatestSignalAsync(string symbol);
    }
    public class ModelIntegrationService : IModelIntegrationService
    {
        private readonly string _pythonPath;
        private readonly string _modelScriptPath;

        public ModelIntegrationService(string pythonPath, string modelScriptPath)
        {
            _pythonPath = pythonPath;
            _modelScriptPath = modelScriptPath;
        }
        public async Task<List<StockSignal>> GetSignalsAsync(string symbol, DateTime startdate, DateTime enddate)
        {
            //set up better error ahndling, security and communication with pyscript later, when model is ready to use 
            var startDateString = startdate.ToString("yyyy-MM-dd");
            var endDateString = enddate.ToString("yyyy-MM-dd");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                Arguments = $"{_modelScriptPath} --symbol {symbol} --start {startDateString} --end {endDateString}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };
            using var process = Process.Start(processStartInfo);
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            //parse the output
            return JsonSerializer.Deserialize<List<StockSignal>>(output);
            }
            public async Task<StockSignal> GetLatestSignalAsync(string symbol)
            {
                var today = DateTime.Today;
                var signals = await GetSignalsAsync(symbol, today.AddDays(-5), today);
                return signals.OrderByDescending(s => s.TimeStamp).FirstOrDefault();
            }
        }
    }
