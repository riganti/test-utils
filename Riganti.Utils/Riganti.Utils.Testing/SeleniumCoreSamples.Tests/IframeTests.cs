﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Riganti.Utils.Testing.SeleniumCore;

namespace WebApplication1.Tests
{
    [TestClass]
    public class IframeTests : SeleniumTestBase
    {
        [TestMethod]
        public void IFrameTest()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("frametest1.aspx");
                browser.First("#top");
                browser.First("#topframe");

                var frame = browser.GetFrameScope("#topframe");
                frame.First("#frame2_text");
            });
        }
        [TestMethod]
        public void IFrameTest2()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("frametest1.aspx");
                var elm = browser.First("#top");
                browser.First("#topframe");

                var frame = browser.GetFrameScope("#topframe");
                frame.First("#frame2_text");
                elm.First("#child");

            });
        }
    }
}