using Riganti.Selenium.AssertApi;
using System.Security.Principal;
using Xunit;
using Xunit.Abstractions;

namespace Riganti.Selenium.Core.Samples.AssertApi.Tests
{
    public class ScrollToTests : AppSeleniumTest
    {
        public ScrollToTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ScrollToChild()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/ScrollTo");
                var child = browser.First("#child");
                child.ScrollTo();
                browser.Wait(10000);
                AssertUI.IsElementInView(child, child);
            });

        }
    }


}