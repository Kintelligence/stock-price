namespace Stock.Client;

public record StockPrice
{
    public string Symbol { init; get; }
    public int BidCents { init; get; }
    public int AskCents { init; get; }
}