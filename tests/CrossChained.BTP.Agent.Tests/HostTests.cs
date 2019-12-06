using Xunit;

namespace CrossChained.BTP.Agent.Tests
{
    public class HostTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public HostTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
    }
}
