
using System.Threading.Tasks;

namespace Stock.Server.Hubs;

public interface IStockBroadcaster
{
    Task UpdateStockPrice(StockPrice stockPrice);
}
