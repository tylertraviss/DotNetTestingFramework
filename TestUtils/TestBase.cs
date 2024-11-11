using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;


namespace PremierLeagueTests.TestUtils
{
    public class TestBase : IDisposable
    {
        protected IWebDriver Driver;

        public TestBase()
        {
            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl("https://www.premierleague.com/");
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    
    
    }
    }


