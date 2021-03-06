# stock-price

Small application showing "live" stock prices consisting of a server and a client. Written in C# using SignalR for communication between the server & client.

## Server
A simple ASP NET Web API application with a swagger page for creating & updating stock prices.

Run `dotnet run --project .\Stock.Server\Stock.Server.csproj` to start the server.
Access the Swagger page at https://localhost:5001/swagger/index.html to manipulate the stock prices

Call `POST /stock-prices/bootstrap` to fill the list of stocks with sample data.

Stocks do not update on their own. Call `POST /stock-prices/randomize` to generate new prices for all stocks.

## Client
A small console application showing a table of stock prices.
Whenever they are updated server side an update is pushed to the client and the table updates to reflect it.

Run `dotnet run --project .\Stock.Client\Stock.Client.csproj` to start the client.

## Thoughts & Reflections
SignalR seemed like an interesting way to solve the connectivity vs. polling a REST API from the client.

Initially considered a Blazor or Maui app for the client, but settled on a simple console app as the connectivity part was the interesting part for me.

Lack of requirements & features makes it hard to write meaningful tests.
SignalR / inexperience with SignalR complicates testing of the central parts.