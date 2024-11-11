//using PremierLeagueTests.Pages;
using PremierLeagueTests.TestUtils;
using Xunit;

namespace PremierLeagueTests.Tests
{
    public class GoogleTests : TestBase
    {
        [Fact]
        public void PremierLeagueTitleAssertion()
        {
            Assert.Contains("Premier League", Driver.Title);

            // Test Change
        }
    }
}
