using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Riganti.Selenium.Core.Samples.FluentApi.Tests
{
    [TestClass]
    public class ElementCollectionTests : AppSeleniumTest
    {
        [TestMethod]
        public void TestBackingReferencesFromCollection()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/FindElements");
                var a = browser.FindElements("div");
                var b = a.FindElements("p");
                var c = b.BrowserWrapper;
                var d = c.FindElements("div");
                var e = d.FindElements("p");
                e.ThrowIfSequenceEmpty();
            });
        }
        [TestMethod]
        public void TestBackingReferencesFromCollection_WithExplicitSelector()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/FindElements");
                var a = browser.FindElements("innerDiv1", s => By.CssSelector($"[data-ui={s}]"));
                var b = a.FindElements("p");
                b.ThrowIfDifferentCountThan(3);
            });
        }


    }
}