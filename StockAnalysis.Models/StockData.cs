namespace StockAnalysis.Models
{
    public class StockData
    {
        public string Symbol { get; set; }
        public List<PricePoint> HistoricalPrices { get; set; }
    }
    public class PricePoint
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
    }
}