using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
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
    private readonly StockHub _hub;
    private readonly Random _random;

    public StockPriceController(ILogger<StockPriceController> logger, IStockPriceRepo repo, StockHub hub)
    {
        _logger = logger;
        _repo = repo;
        _hub = hub;
        _random = new Random();
    }

    [HttpGet("StockPrices")]
    public async IAsyncEnumerable<StockPrice> GetStockPrices()
    {
        var stockPrices = _repo.GetStockPrices();

        await foreach (var stockPrice in stockPrices)
        {
            yield return stockPrice;
        }
    }

    [HttpPost("StockPrices")]
    public async Task<StatusCodeResult> SetStockPrice(StockPrice stockPrice)
    {
        try
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _repo.UpsertStockPrice(stockPrice);
                await _hub.UpdateStockPrice(stockPrice);
            }

            return StatusCode(200);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed setting stock price {StockPrice}", stockPrice);
            return StatusCode(500);
        }
    }

    [HttpPost("StockPrices/{symbol}/random")]
    public async Task<StatusCodeResult> RandomizeStockPrice(string symbol)
    {
        var bid = _random.Next();
        var ask = bid + _random.Next();

        var stockPrice = new StockPrice
        {
            Symbol = symbol,
            BidCents = bid,
            AskCents = ask
        };

        return await SetStockPrice(stockPrice);
    }
}
