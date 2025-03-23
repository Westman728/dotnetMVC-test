namespace StockAnalysis.Models
{
    public class StockSignal
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal PredictedPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public SignalType Signal { get; set; }
        public decimal Confidence { get; set; }
    }
    public enum SignalType
    {
        Buy,
        Sell,
        Hold
    }
}