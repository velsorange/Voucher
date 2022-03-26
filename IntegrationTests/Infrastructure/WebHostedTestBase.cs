using System.Net.Http;

namespace IntegrationTests.Infrastructure;

public abstract class WebHostedTestBase : CustomWebApplicationFactory<TestStartup>
{
    protected HttpClient Client { get; private set; }

    protected WebHostedTestBase()
    {
        Client = CreateClient();
    }
}