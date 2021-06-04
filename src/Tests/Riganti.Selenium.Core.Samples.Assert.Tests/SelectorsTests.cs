using OpenQA.Selenium;
using Riganti.Selenium.AssertApi;
using System.Security.Principal;
using Xunit;
using Xunit.Abstractions;

namespace Riganti.Selenium.Core.Samples.AssertApi.Tests
{

    public class SelectorsTests : AppSeleniumTest
    {
        public SelectorsTests(ITestOutputHelper output) : base(output)
        {

        }
        public By SelectByDataUi(string selector)
            => SelectBy.CssSelector($"[data-ui='{selector}']");

        [Fact]
        public void SelectByCssSelectorTest()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/Index");

                var div = browser.Single("selector-test", SelectByDataUi);
                var span = div.Single("span");
            });
        }


    }


}