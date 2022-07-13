using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Client;

public class ConsoleStockDisplay
{
    public void Print(IEnumerable<StockPrice> stockPrices)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"[{"Symbol",-10}][{"Bid",7}][{"Ask",7}]");

        foreach (var stockPrice in stockPrices)
        {
            builder.AppendLine($"[{stockPrice.Symbol,-10}][{(Decimal)stockPrice.BidCents / 100,7:F2}][{(Decimal)stockPrice.AskCents / 100,7:F2}]");
        }

        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);
        Console.Write(builder.ToString());
    }

    public void PrintEmpty()
    {
        var builder = new StringBuilder();

        builder.AppendLine($"[{"Symbol",-10}][{"Bid",7}][{"Ask",7}]");

        builder.AppendLine("Waiting for stock prices");

        Console.Clear();
        Console.Write(builder.ToString());
    }
}