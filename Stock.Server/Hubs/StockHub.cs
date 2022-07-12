using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Stock.Server.Repositories;

namespace Stock.Server.Hubs;

public class StockHub : Hub
{
    private readonly IStockPriceRepo _repo;

    public StockHub(IStockPriceRepo repo)
    {
        _repo = repo;
    }

    public async IAsyncEnumerable<StockPrice> GetStockPrices()
    {
        var stockPrices = _repo.GetStockPrices();

        await foreach (var stockPrice in stockPrices)
        {
            yield return stockPrice;
        }
    }
}