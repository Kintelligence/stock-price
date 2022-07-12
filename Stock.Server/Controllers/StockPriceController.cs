using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Stock.Server.Hubs;
using Stock.Server.Repositories;

namespace Stock.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class StockPriceController : ControllerBase
{
    private readonly ILogger<StockPriceController> _logger;
    private readonly IStockPriceRepo _repo;
    private readonly StockBroadcaster _broadcaster;
    private readonly Random _random;

    public StockPriceController(ILogger<StockPriceController> logger, IStockPriceRepo repo, StockBroadcaster broadcaster)
    {
        _logger = logger;
        _repo = repo;
        _broadcaster = broadcaster;
        _random = new Random();
    }

    [HttpGet("stock-prices")]
    public async IAsyncEnumerable<StockPrice> GetStockPrices()
    {
        var stockPrices = _repo.GetStockPrices();

        await foreach (var stockPrice in stockPrices)
        {
            yield return stockPrice;
        }
    }

    [HttpPost("stock-prices")]
    public async Task<StatusCodeResult> SetStockPrice(StockPrice stockPrice)
    {
        try
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _repo.UpsertStockPrice(stockPrice);
                await _broadcaster.UpdateStockPrice(stockPrice);
            }

            return StatusCode(200);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed setting stock price {StockPrice}", stockPrice);
            return StatusCode(500);
        }
    }

    [HttpPost("stock-prices/{symbol}/random")]
    public async Task<StatusCodeResult> RandomizeStockPrice(string symbol)
    {
        var bid = _random.Next(1, 100000);
        var ask = bid + _random.Next(1, 100);

        var stockPrice = new StockPrice
        {
            Symbol = symbol,
            BidCents = bid,
            AskCents = ask
        };

        return await SetStockPrice(stockPrice);
    }

    [HttpPost("stock-prices/randomize")]
    public async Task<StatusCodeResult> RandomizeAllStockPrices()
    {
        var oldPrices = _repo.GetStockPrices();

        await foreach (var oldPrice in oldPrices)
        {
            await RandomizeStockPrice(oldPrice.Symbol);
        }

        return Ok();
    }
}
