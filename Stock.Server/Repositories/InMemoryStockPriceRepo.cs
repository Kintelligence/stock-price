
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Server.Repositories;

public class InMemoryStockPriceRepo : IStockPriceRepo
{
    private ConcurrentDictionary<string, StockPrice> _dict = new ConcurrentDictionary<string, StockPrice>();

    public async IAsyncEnumerable<StockPrice> GetStockPrices()
    {
        foreach (var value in _dict.Values)
        {
            yield return value;
            await Task.Delay(500);
        }
    }

    public async Task UpsertStockPrice(StockPrice stockPrice)
    {
        _dict.AddOrUpdate(stockPrice.Symbol, (symbol) => stockPrice, (symbol, prev) => stockPrice);
        await Task.Delay(500);
    }
}