using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit;
using Xunit.Abstractions;

namespace Riganti.Selenium.Core.Samples.AssertApi.Tests
{
    public class BrowserLoggingTests : AppSeleniumTest
    {
        public BrowserLoggingTests(ITestOutputHelper output) : base(output)
        {
        }
        [Fact(Skip = "Internal WebDriver API is corrupted.")]
        public void Tests()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("http://localhost:7190/");
                var logs = browser.Driver.Manage().Logs.GetLog(LogType.Browser);
                var driverLogs = browser.Driver.Manage().Logs.GetLog(LogType.Driver);
            });
        }
    }
}   
