using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Stock.Server.Controllers;
using Stock.Server.Hubs;
using Stock.Server.Repositories;
using Xunit;
namespace Stock.Server.Tests;

public class UnitTStockPriceControllerTests
{
    [Theory]
    [AutoData]
    public async Task SetStockPrice_WhenOk_Returns200(StockPrice stockPrice)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        fixture.Customize<StockPriceController>(c => c.Without(x => x.ControllerContext));
        var controller = fixture.Create<StockPriceController>();

        var response = await controller.SetStockPrice(stockPrice);

        response.StatusCode.Should().Be(200);
    }

    [Theory]
    [AutoData]
    public async Task SetStockPrice_WhenOk_BroadcastsNewPrice(StockPrice stockPrice)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        fixture.Customize<StockPriceController>(c => c.Without(x => x.ControllerContext));
        var mockBroadcaster = fixture.Freeze<IStockBroadcaster>();
        var controller = fixture.Create<StockPriceController>();

        await controller.SetStockPrice(stockPrice);

        await mockBroadcaster.Received().UpdateStockPrice(stockPrice);
    }

    [Theory]
    [AutoData]
    public async Task SetStockPrice_WhenRepositoryFails_DoesNotBroadcastsNewPrice(StockPrice stockPrice)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        fixture.Customize<StockPriceController>(c => c.Without(x => x.ControllerContext));
        var mockBroadcaster = fixture.Freeze<IStockBroadcaster>();
        var mockRepo = fixture.Freeze<IStockPriceRepo>();
        var controller = fixture.Create<StockPriceController>();
        mockRepo.When(c => c.UpsertStockPrice(stockPrice)).Do(c => { throw new System.Exception(); });

        await controller.SetStockPrice(stockPrice);

        await mockBroadcaster.DidNotReceive().UpdateStockPrice(Arg.Any<StockPrice>());
    }
}