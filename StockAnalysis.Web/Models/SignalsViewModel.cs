using StockAnalysis.Models;

namespace StockAnalysis.Web.Models
{
    public class SignalsViewModel
    {
        public List<string> AvailableSymbols { get; set; } = new List<string>();
    }

    public class SignalDetailsViewModel
    {
        public string Symbol { get; set; }
        public List<StockSignal> Signals { get; set; } = new List<StockSignal>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}