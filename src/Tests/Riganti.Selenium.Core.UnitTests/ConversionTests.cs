using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.UnitTests.Mock;
using Selenium.Core.UnitTests;

namespace Riganti.Selenium.Core.UnitTests
{
    [TestClass]
    public class ConversionTests : MockingTest
    {
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void IEnumerableConversionTests()
        {
            var browser = CreateMockedIBrowserWrapper();
            var elements = new List<IElementWrapper>()
            {
                new ElementWrapperFluentApi(()=>new MockIWebElement(), browser)
            };

            var collection = new ElementWrapperCollection<IElementWrapperFluentApi, IBrowserWrapperFluentApi>(
            () => new IElementWrapperFluentApi[] { new ElementWrapperFluentApi(() => new MockIWebElement(), browser) }, "selector", By.CssSelector, browser);

            var collection2 = new ElementWrapperCollection<IElementWrapper, IBrowserWrapper>(() => elements, "selec2", By.CssSelector, collection, collection.BrowserWrapper);

        }


    }

}
