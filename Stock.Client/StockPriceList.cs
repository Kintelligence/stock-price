using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Stock.Client;

public class StockPriceList
{
    private readonly ConcurrentDictionary<string, StockPrice> _dict = new ConcurrentDictionary<string, StockPrice>();

    public void UpsertStockPrice(StockPrice stockPrice)
    {
        _dict.AddOrUpdate(stockPrice.Symbol, stockPrice, (key, value) => stockPrice);
    }

    public IEnumerable<StockPrice> ListStockPrices()
    {
        return _dict.Values;
    }
}