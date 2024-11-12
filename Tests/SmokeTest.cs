using OpenQA.Selenium;
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

            string discipline_xpath = "//a[normalize-space()='Discipline']";
            ClickElement(By.XPath(discipline_xpath));

        }
    }
}
