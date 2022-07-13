using System;
using Stock.Client;

// See https://aka.ms/new-console-template for more information
var client = new StockBroadcastClient();
var list = new StockPriceList();
var display = new ConsoleStockDisplay();

display.PrintEmpty();

await client.StartListening((StockPrice stockPrice) =>
{
    list.UpsertStockPrice(stockPrice);
    display.Print(list.ListStockPrices());
});


while (true)
{
}