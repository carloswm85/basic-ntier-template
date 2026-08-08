using NLayerTemplate.Web.MVC;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace NLayerTemplate.Testing.Integration.Mvc;

public class MvcEndpointsAvailabilityTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MvcEndpointsAvailabilityTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HomePage_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/", TestContext.Current.CancellationToken);
        response.IsSuccessStatusCode.Should().BeTrue(because: "The home page should be reachable");
    }

    [Fact]
    public async Task StudentsListPage_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync(
            "/ContosoUniversity/List",
            TestContext.Current.CancellationToken
        );
        response
            .IsSuccessStatusCode.Should()
            .BeTrue(because: "The students listing page should be reachable");
    }

    [Fact]
    public async Task StudentsStatistics_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync(
            "/ContosoUniversity/Statistics",
            TestContext.Current.CancellationToken
        );
        response
            .IsSuccessStatusCode.Should()
            .BeTrue(because: "The student statistics page should be reachable");
    }

    [Fact]
    public async Task NewStudentCreation_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync(
            "/ContosoUniversity/Create",
            TestContext.Current.CancellationToken
        );
        response
            .IsSuccessStatusCode.Should()
            .BeTrue(because: "The student creation page should be reachable");
    }

    [Fact]
    public async Task SelectedStudentDetails_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync(
            "/ContosoUniversity/Details/40",
            TestContext.Current.CancellationToken
        );
        response
            .IsSuccessStatusCode.Should()
            .BeTrue(because: "The student details page should be reachable");
    }

    [Fact]
    public async Task SelectedStudentDelete_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync(
            "/ContosoUniversity/Delete/40",
            TestContext.Current.CancellationToken
        );
        response
            .IsSuccessStatusCode.Should()
            .BeTrue(because: "The student delete page should be reachable");
    }

    [Fact]
    public async Task SelectedStudentEdit_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync(
            "/ContosoUniversity/Edit/40",
            TestContext.Current.CancellationToken
        );
        response
            .IsSuccessStatusCode.Should()
            .BeTrue(because: "The student edit page should be reachable");
    }
}
