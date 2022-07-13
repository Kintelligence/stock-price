using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Stock.Client;

public class StockBroadcastClient
{
    private readonly HubConnection _connection;

    public StockBroadcastClient()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/stock-prices")
            .WithAutomaticReconnect().Build();
    }

    public async Task StartListening(Action<StockPrice> action)
    {
        await _connection.StartAsync();
        _connection.On<StockPrice>("UpdateStockPrice", action);

        var stockPrices = _connection.StreamAsync<StockPrice>("GetStockPrices");

        await foreach (var stockPrice in stockPrices)
        {
            action(stockPrice);
        }
    }
}