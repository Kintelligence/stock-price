namespace Stock.Server;

public record StockPrice
{
    public string Symbol { init; get; }
    public decimal Bid { init; get; }
    public decimal Ask { init; get; }
}