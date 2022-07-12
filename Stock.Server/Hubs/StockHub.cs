using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Stock.Server.Hubs;

public class StockHub : Hub
{
    public async Task UpdateStockPrice(StockPrice stockPrice)
    {
        await Clients.All.SendAsync("UpdateStockPrice", stockPrice);
    }
}