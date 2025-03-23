// ASP .NET CORE MVC CONTROLLER
using Microsoft.AspNetCore.Mvc;
using StockAnalysis.Models;
using StockAnalysis.Core.Services;

namespace StockAnalysis.Web.Controllers
{
    public class SignalsController : Controller
    {
        private readonly ISignalService _signalService;

        public SignalsController(ISignalService signalService)
        {
            _signalService = signalService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new SignalsViewModel
            {
                AvailableSymbols = new List<string> { "AAPL", "MSFT", "GOOG", "AMZN", "META" }
            };
            
            return View(viewModel);
        }

        public async Task<IActionResult> Details(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Symbol is required");
            }

            var startDate = DateTime.Today.AddMonths(-1);
            var endDate = DateTime.Today;
            
            var signals = await _signalService.GetSignalsAsync(symbol, startDate, endDate);
            
            var viewModel = new SignalDetailsViewModel
            {
                Symbol = symbol,
                Signals = signals,
                StartDate = startDate,
                EndDate = endDate
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(string symbol, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Symbol is required");
            }

            await _signalService.RefreshSignalsAsync(symbol, startDate, endDate);
            
            return RedirectToAction(nameof(Details), new { symbol });
        }

        [HttpGet]
        public async Task<IActionResult> Api(string symbol, DateTime? startDate, DateTime? endDate)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Symbol is required");
            }

            var start = startDate ?? DateTime.Today.AddMonths(-1);
            var end = endDate ?? DateTime.Today;
            
            var signals = await _signalService.GetSignalsAsync(symbol, start, end);
            
            return Json(signals);
        }
    }
}