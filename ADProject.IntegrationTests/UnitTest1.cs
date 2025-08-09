using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ADProject.IntegrationTests;

public class HealthTests : IClassFixture<TestAppFactory>
{
    private readonly TestAppFactory _factory;
    public HealthTests(TestAppFactory factory) => _factory = factory;

    [Fact]
    public async Task GET_health_should_return_OK()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/health");
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await res.Content.ReadAsStringAsync();
        body.Should().Contain("healthy");
    }
}
