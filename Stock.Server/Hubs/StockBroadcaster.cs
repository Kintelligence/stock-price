
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Stock.Server.Hubs;

public class StockBroadcaster : IStockBroadcaster
{
    private readonly IHubContext<StockHub> _hubContext;

    public StockBroadcaster(IHubContext<StockHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task UpdateStockPrice(StockPrice stockPrice)
    {
        await _hubContext.Clients.All.SendAsync("UpdateStockPrice", stockPrice);
    }
}