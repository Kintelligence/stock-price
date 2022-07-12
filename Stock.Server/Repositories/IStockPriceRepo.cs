using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Server.Repositories;

public interface IStockPriceRepo
{
    public IAsyncEnumerable<StockPrice> GetStockPrices();
    public Task UpsertStockPrice(StockPrice stockPrice);
}