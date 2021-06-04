using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System;
using System.Net;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;


namespace Riganti.Selenium.Core.Samples.FluentApi.Tests
{
    [TestClass]
    public class UrlTests : SeleniumTest
    {
        [TestMethod]
        public void Url_CheckHyperLink_Relative()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/HyperLink");
                browser.CheckIfHyperLinkEquals("#AbsoluteSameSchema", "/path/test?query=test#fragment",
                    UrlKind.Relative,
                    UriComponents.PathAndQuery);
                browser.CheckIfHyperLinkEquals("#RelativeLink", "/path/test?query=test#fragment", UrlKind.Relative,
                    UriComponents.PathAndQuery);
                browser.CheckIfHyperLinkEquals("#RelativeLink", "/path/test?query=test#fragment", UrlKind.Relative,
                    UriComponents.AbsoluteUri);
                browser.CheckIfHyperLinkEquals("#RelativeLink", "path/test?query=test#fragmentasd", UrlKind.Relative,
                    UriComponents.PathAndQuery);
                browser.CheckIfHyperLinkEquals("#RelativeLink", "path/test?query=test#fragment", UrlKind.Relative,
                    UriComponents.PathAndQuery); 
                browser.CheckIfHyperLinkEquals("#RelativeLink", "path/test", UrlKind.Relative,UriComponents.Path);
            });
        }

        [TestMethod]
        public void Url_CheckHyperLink_Absolute()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/HyperLink");
                browser.CheckIfHyperLinkEquals("#AbsoluteLink", "https://www.google.com/path/test?query=test#fragment",
                    UrlKind.Absolute, UriComponents.PathAndQuery);
                browser.CheckIfHyperLinkEquals("#AbsoluteLink", "https://www.google.com/path/test?query=test#fragment",
                    UrlKind.Absolute, UriComponents.AbsoluteUri);
                browser.CheckIfHyperLinkEquals("#AbsoluteSameSchema", "//localhost:1234/path/test?query=test#fragment",
                    UrlKind.Absolute, UriComponents.PathAndQuery);
                browser.CheckIfHyperLinkEquals("#AbsoluteSameSchema",
                    "//localhostads:1234/path/test?query=test#fragment",
                    UrlKind.Absolute, UriComponents.PathAndQuery);
            });
        }

        [TestMethod]
        public void Url_CheckHyperLink_Relative_ExpectedFailure()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/HyperLink");

                MSAssert.ThrowsException<UnexpectedElementStateException>(() =>
                {
                    browser.CheckIfHyperLinkEquals("#RelativeLink", "/path0/test?query=test#fragment", UrlKind.Relative,
                        UriComponents.PathAndQuery);
                });
                MSAssert.ThrowsException<UnexpectedElementStateException>(() =>
                {
                    browser.CheckIfHyperLinkEquals("#RelativeLink", "/path/test?query=test#fragment_nonexistent",
                        UrlKind.Relative, UriComponents.AbsoluteUri);
                });
            });
        }

        [TestMethod]
        public void Url_CheckHyperLink_Absolute_ExpectedFailure()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/HyperLink");

                MSAssert.ThrowsException<UnexpectedElementStateException>(() =>
                {
                    browser.CheckIfHyperLinkEquals("#AbsoluteLink", "https://www.google.com/path0/test?query=test#fragment",
                        UrlKind.Absolute, UriComponents.PathAndQuery);
                });
                MSAssert.ThrowsException<UnexpectedElementStateException>(() =>
                {
                    browser.CheckIfHyperLinkEquals("#AbsoluteSameSchema",
                        "https://localhost:1234/path/test?query=test#fragment", UrlKind.Absolute,
                        UriComponents.AbsoluteUri);
                });
                MSAssert.ThrowsException<UnexpectedElementStateException>(() =>
                {
                    browser.CheckIfHyperLinkEquals("#AbsoluteLink", "https://www.google.com/path/test?query=test#fragment_nonexistent",
                        UrlKind.Absolute, UriComponents.AbsoluteUri);
                });
            });
        }

        [TestMethod]
        public void Url_CheckUrl()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/HyperLink");
                browser.CheckUrl(url => url.Contains("/test/HyperLink"));
            });
        }

        [TestMethod]
        [ExpectedSeleniumException(typeof(BrowserLocationException))]
        public void Url_CheckUrl_ExpectedFailure()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/test/HyperLink");
                browser.CheckUrl(url => url.Contains("/test/HyperLink_nonexistent"));
            });
        }

        [TestMethod]
        public void Url_CheckIfUrlExists()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.CheckIfUrlIsAccessible("/test/HyperLink", UrlKind.Relative);
            });
        }

        [TestMethod]
        [ExpectedSeleniumException(typeof(BrowserLocationException))]
        public void Url_CheckIfUrlExists_ExpectedFailure()
        {
            this.RunInAllBrowsers(browser =>
            {
                browser.CheckIfUrlIsAccessible("/test/HyperLink_notexisting", UrlKind.Relative);
            });
        }
    }
}
