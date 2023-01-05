using MachineFaultsAPI.Controllers;
using MachineFaultsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MachineFaultsAPI.Tests;

public class KvaroviControllerTests
{
    private readonly KvaroviController _controller;

    public KvaroviControllerTests()
    {
        _controller = new KvaroviController();
    }

    [Fact]
    public async Task GetKvarovi_ReturnsOkResult()
    {
        // Act
        var result = await _controller.GetKvarovi();

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetKvarovi_ReturnsCorrectNumberOfKvarovi()
    {
        // Act
        var result = await _controller.GetKvarovi(2, 1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsType<List<Kvar>>(okResult.Value);
        var kvarovi = okResult.Value as List<Kvar>;
        Assert.Equal(2, kvarovi.Count());
    }

    [Fact]
    public async Task GetKvar_ReturnsNotFoundResult_WhenKvarIsNotFound()
    {
        // Act
        var result = await _controller.GetKvar(0);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetKvar_ReturnsOkResult_WhenKvarIsFound()
    {
        // Act
        var result = await _controller.GetKvar(6);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetKvar_ReturnsCorrectKvar_WhenKvarIsFound()
    {
        // Act
        var result = await _controller.GetKvar(6);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsType<Kvar>(okResult.Value);
        var kvar = okResult.Value as Kvar;
        Assert.Equal(6, kvar.Id);
    }
}