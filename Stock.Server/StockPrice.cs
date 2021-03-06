using System.ComponentModel.DataAnnotations;

namespace Stock.Server;

public record StockPrice
{
    [MaxLength(10)]
    public string Symbol { init; get; }
    [Range(1, 999999)]
    public int BidCents { init; get; }
    [Range(1, 999999)]
    public int AskCents { init; get; }
}