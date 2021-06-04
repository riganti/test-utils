using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Riganti.Selenium.Core;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;
using Riganti.Selenium.Core.Api;
using Riganti.Selenium.Core.Drivers;
using Riganti.Selenium.Core.Factories;
using Riganti.Selenium.Core.UnitTests.Mock;
using Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers;
using TestConfiguration = Riganti.Selenium.Core.TestConfiguration;

namespace Selenium.Core.UnitTests
{
    [TestClass]
    public class TimeOutTests : MockingTest
    {
        private IBrowserWrapper browser;

        #region Browser tests

        [TestInitialize]
        public void TestInit()
        {
            browser = CreateMockedIBrowserWrapper();
        }

        [TestMethod, Timeout(5000), ExpectedException(typeof(WaitBlockException), AllowDerivedTypes = true)]
        public void BrowserWaitForTimeoutTest()
        {
            browser.WaitFor(() => false, 2000, "test timeouted");
        }

        [TestMethod, Timeout(5000), ExpectedException(typeof(WaitBlockException), AllowDerivedTypes = true)]
        public void BrowserWaitForTimeoutTest2()
        {
            var i = 0;
            browser.WaitFor(() => i++ == 5, 2000, 500, "test timeouted");
        }

        [TestMethod, Timeout(5000)]
        public void BrowserWaitForTimeoutTest3()
        {
            var i = 0;
            browser.WaitFor(() => i++ == 5, 2000, "test timeouted", checkInterval: 100);
        }

        [TestMethod, Timeout(5000), ExpectedException(typeof(WaitBlockException), AllowDerivedTypes = true)]
        public void BrowserWaitForTimeoutTest5()
        {
            browser.WaitFor(new Action(() => throw new Exception("Condition is not valid.")), 2000, "test timeouted", checkInterval: 100);
        }

        [TestMethod, Timeout(5000)]
        public void BrowserWaitForTimeoutTest6()
        {
            var i = 0;
            browser.WaitFor(() =>
            {
                if (i++ == 3)
                {
                    return;
                }
                throw new Exception("Not valid.");
            }, 2000, "test timeouted", checkInterval: 100);
        }

        #endregion Browser tests

        #region Element tests

        [TestMethod, Timeout(5000), ExpectedException(typeof(WaitBlockException), AllowDerivedTypes = true)]
        public void ElementWaitForTimeoutTest()
        {
            var element = new ElementWrapper(() => new MockIWebElement(), browser);
            element.WaitFor(elm => false, 2000, "test timeouted");
        }

        [TestMethod, Timeout(5000), ExpectedException(typeof(WaitBlockException), AllowDerivedTypes = true)]
        public void ElementWaitForTimeoutTest2()
        {
            var element = new ElementWrapper(() => new MockIWebElement(), browser);
            var i = 0;
            element.WaitFor(elm => i++ == 5, 2000, "test timeouted", checkInterval: 800);
        }

        [TestMethod, Timeout(5000)]
        public void ElementWaitForTimeoutTest4()
        {
            var element = new ElementWrapper(() => new MockIWebElement(), browser);
            var i = 0;
            element.WaitFor(elm => i++ == 5, 2000, "test timeouted", checkInterval: 100);
        }

        [TestMethod, Timeout(5000), ExpectedException(typeof(WaitBlockException), AllowDerivedTypes = true)]
        public void ElementWaitForTimeoutTest5()
        {
            var element = new ElementWrapper(() => new MockIWebElement(), browser);
            element.WaitFor((elm) =>
            {
                var valueValidator = new ValueValidator("asdasdasdasd");
                var v = new OperationResultValidator();
                var result = valueValidator.Validate(elm);
                v.Validate<UnexpectedElementException>(result);
            }, 2000, "test timeouted", checkInterval: 100);
        }

        [TestMethod, Timeout(5000)]
        public void ElementWaitForTimeoutTest6()
        {
            var element = new ElementWrapper(() => new MockIWebElement(), browser);
            var i = 0;
            element.WaitFor((elm) =>
            {
                if (i++ == 3)
                {
                    return;
                }
                throw new Exception("Not valid.");
            }, 2000, "test timeouted", checkInterval: 100);
        }

        #endregion Element tests
    }
}