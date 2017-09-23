﻿using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Threading;
using OpenQA.Selenium.Interactions;
using Riganti.Utils.Testing.Selenium.Core.Api;
using Riganti.Utils.Testing.Selenium.Core.Configuration;
using Riganti.Utils.Testing.Selenium.Core.Drivers;
using Riganti.Utils.Testing.Selenium.Core;
using Riganti.Utils.Testing.Selenium.Core.Abstractions;
using Riganti.Utils.Testing.Selenium.Core.Abstractions.Exceptions;
using Riganti.Utils.Testing.Selenium.Validators.Checkers.ElementWrapperCheckers;

namespace Riganti.Utils.Testing.Selenium.Core
{
    public abstract class BrowserWrapper : IBrowserWrapper
    {

        protected readonly IWebBrowser browser;
        protected readonly IWebDriver driver;
        protected readonly ITestInstance testInstance;

        public int ActionWaitTime { get; set; }

        public string BaseUrl => testInstance.TestConfiguration.BaseUrl;


        /// <summary>
        /// Generic representation of browser driver.
        /// </summary>
        public IWebDriver Driver
        {
            get
            {
                ActivateScope();
                return driver;
            }
        }

        protected ScopeOptions ScopeOptions { get; set; }

        public BrowserWrapper(IWebBrowser browser, IWebDriver driver, ITestInstance testInstance, ScopeOptions scope)
        {
            this.browser = browser;
            this.driver = driver;

            this.testInstance = testInstance;
            ActionWaitTime = browser.Factory?.TestSuiteRunner?.Configuration.TestRunOptions.ActionTimeout ?? 250;

            ScopeOptions = scope;
            SetCssSelector();
        }
        /// <summary>
        /// Sets implicit timeouts for page load and the time range between actions.
        /// </summary>
        public void SetTimeouts(TimeSpan pageLoadTimeout, TimeSpan implicitlyWait)
        {
            var timeouts = Driver.Manage().Timeouts();
            timeouts.PageLoad = pageLoadTimeout;
            timeouts.ImplicitWait = implicitlyWait;
        }

        protected Func<string, By> selectMethodFunc;

        public virtual Func<string, By> SelectMethod
        {
            get { return selectMethodFunc; }
            set
            {
                if (value == null)
                { throw new ArgumentException("SelectMethod cannot be null. This method is used to select elements from loaded page."); }
                selectMethodFunc = value;
            }
        }

        public void SetCssSelector()
        {
            selectMethodFunc = By.CssSelector;
        }

        /// <summary>
        /// Url of active browser tab.
        /// </summary>
        public string CurrentUrl => Driver.Url;

        /// <summary>
        /// Gives path of url of active browser tab.
        /// </summary>
        public string CurrentUrlPath => new Uri(CurrentUrl).GetLeftPart(UriPartial.Path);



        /// <summary>
        /// Clicks on element.
        /// </summary>
        public IBrowserWrapper Click(string selector)
        {
            First(selector).Click();
            Wait();
            return this;
        }

        /// <summary>
        /// Submits this element to the web server.
        /// </summary>
        /// <remarks>
        /// If this current element is a form, or an element within a form,
        ///             then this will be submitted to the web server. If this causes the current
        ///             page to change, then this method will block until the new page is loaded.
        /// </remarks>
        public IBrowserWrapper Submit(string selector)
        {
            First(selector).Submit();
            Wait();
            return this;
        }

        /// <summary>
        /// Navigates to specific url.
        /// </summary>
        /// <param name="url">url to navigate </param>
        /// <remarks>
        /// If url is ABSOLUTE, browser is navigated directly to url.
        /// If url is RELATIVE, browser is navigated to url combined from base url and relative url.
        /// Base url is specified in test configuration. (This is NOT url host of current page!)
        /// </remarks>
        public void NavigateToUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                if (string.IsNullOrWhiteSpace(BaseUrl))
                {
                    throw new InvalidRedirectException();
                }
                NavigateToUrlCore(BaseUrl);
                return;
            }
            //redirect if is absolute
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                NavigateToUrlCore(url);
                return;
            }
            //redirect absolute with same schema
            if (url.StartsWith("//"))
            {
                var schema = new Uri(CurrentUrl).Scheme;
                var navigateUrltmp = $"{schema}:{url}";

                NavigateToUrlCore(navigateUrltmp);
                return;
            }
            var builder = new UriBuilder(BaseUrl);

            // replace url fragments
            if (url.StartsWith("/"))
            {
                builder.Path = "";
                var urlToNavigate = builder.ToString().TrimEnd('/') + "/" + url.TrimStart('/');
                NavigateToUrlCore(urlToNavigate);
                return;
            }

            var navigateUrl = builder.ToString().TrimEnd('/') + "/" + url.TrimStart('/');
            NavigateToUrlCore(navigateUrl);
        }

        private void NavigateToUrlCore(string url)
        {
            StopWatchedAction(() =>
            {
                LogVerbose($"Start navigation to: {url}");
                Driver.Navigate().GoToUrl(url);
            }, s =>
            {
                LogVerbose($"Navigation to: '{url}' executed in {s.ElapsedMilliseconds} ms.");
            });
        }

        private void StopWatchedAction(Action action, Action<Stopwatch> afterActionExecuted)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            afterActionExecuted(stopwatch);
        }

        public void LogVerbose(string message)
        {
            browser.Factory.LogVerbose($"(#{Thread.CurrentThread.ManagedThreadId}) {message}");
        }

        public void LogInfo(string message)
        {
            browser.Factory.LogInfo($"(#{Thread.CurrentThread.ManagedThreadId}) {message}");
        }

        public void LogError(string message, Exception ex)
        {
            browser.Factory.LogError(new Exception($"(#{Thread.CurrentThread.ManagedThreadId}) {message}", ex));
        }

        /// <summary>
        /// Redirects to base url specified in test configuration
        /// </summary>
        public void NavigateToUrl()
        {
            NavigateToUrl(null);
        }

        /// <summary>
        /// Redirects to page back in Browser history
        /// </summary>
        public void NavigateBack()
        {
            Driver.Navigate().Back();
        }

        /// <summary>
        /// Redirects to page forward in Browser history
        /// </summary>
        public void NavigateForward()
        {
            Driver.Navigate().Forward();
        }

        /// <summary>
        /// Reloads current page.
        /// </summary>
        public void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        /// <summary>
        /// Forcibly ends test.
        /// </summary>
        /// <param name="message">Test failure message</param>
        public void DropTest(string message)
        {
            throw new WebDriverException($"Test forcibly dropped: {message}");
        }

        public string GetAlertText()
        {
            var alert = GetAlert();
            return alert?.Text;
        }


        public bool HasAlert()
        {
            try
            {
                GetAlert();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public IAlert GetAlert()
        {
            IAlert alert;
            try

            {
                alert = Driver.SwitchTo().Alert();
            }
            catch (Exception ex)
            {
                throw new AlertException("Alert not visible.", ex);
            }
            if (alert == null)
                throw new AlertException("Alert not visible.");
            return alert;
        }


        /// <summary>
        /// Confirms modal dialog (Alert).
        /// </summary>
        public IBrowserWrapper ConfirmAlert()
        {
            Driver.SwitchTo().Alert().Accept();
            Wait();
            return this;
        }

        /// <summary>
        /// Dismisses modal dialog (Alert).
        /// </summary>
        public IBrowserWrapper DismissAlert()
        {
            Driver.SwitchTo().Alert().Dismiss();
            Wait();
            return this;
        }

        /// <summary>
        /// Waits specified time in milliseconds.
        /// </summary>
        public IBrowserWrapper Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        /// <summary>
        /// Waits time specified by ActionWaitType property.
        /// </summary>
        public IBrowserWrapper Wait()
        {
            return Wait(ActionWaitTime);
        }

        /// <summary>
        /// Waits specified time.
        /// </summary>
        public IBrowserWrapper Wait(TimeSpan interval)
        {
            Thread.Sleep((int)interval.TotalMilliseconds);
            return this;
        }

        /// <summary>
        /// Finds all elements that satisfy the condition of css selector.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IElementWrapperCollection FindElements(By selector)
        {
            return Driver.FindElements(selector).ToElementsList(this, selector.GetSelector());
        }

        /// <summary>
        /// Finds all elements that satisfy the condition of css selector.
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>
        public IElementWrapperCollection FindElements(string cssSelector, Func<string, By> tmpSelectMethod = null)
        {
            return Driver.FindElements((tmpSelectMethod ?? SelectMethod)(cssSelector)).ToElementsList(this, (tmpSelectMethod ?? SelectMethod)(cssSelector).GetSelector());
        }

        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IElementWrapper FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var elms = FindElements(selector, tmpSelectMethod);
            return elms.FirstOrDefault();
        }

        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IElementWrapper First(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return ThrowIfIsNull(FirstOrDefault(selector, tmpSelectMethod), $"Element not found. Selector: {selector}");
        }

        /// <summary>
        /// Performs specified action on each element from a sequence.
        /// </summary>
        /// <param name="selector">Selector to find a sequence of elements.</param>
        /// <param name="action">Action to perform on each element of a sequence.</param>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>
        public IBrowserWrapper ForEach(string selector, Action<IElementWrapper> action, Func<string, By> tmpSelectMethod = null)
        {
            FindElements(selector, tmpSelectMethod).ForEach(action);
            return this;
        }

        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IElementWrapper SingleOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).SingleOrDefault();
        }


        /// <summary>
        /// Returns one element and throws exception when no element or more then one element is found.
        /// </summary>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IElementWrapper Single(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).Single();
        }

        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public bool IsDisplayed(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).All(s => s.IsDisplayed());
        }

        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        ///<summary>Provides elements that satisfies the selector condition at specific position.</summary>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IElementWrapper ElementAt(string selector, int index, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).ElementAt(index);
        }

        ///<summary>Provides the last element that satisfies the selector condition.</summary>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IElementWrapper Last(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).Last();
        }

        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>
        public IElementWrapper LastOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).LastOrDefault();
        }

        public IBrowserWrapper FireJsBlur()
        {
            GetJavaScriptExecutor()?.ExecuteScript("if(document.activeElement && document.activeElement.blur) {document.activeElement.blur()}");
            return this;
        }

        public IJavaScriptExecutor GetJavaScriptExecutor()
        {
            return Driver as IJavaScriptExecutor;
        }

        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IBrowserWrapper SendKeys(string selector, string text, Func<string, By> tmpSelectMethod = null)
        {
            FindElements(selector, tmpSelectMethod).ForEach(s => { s.SendKeys(text); s.Wait(); });
            return this;
        }

        /// <summary>
        /// Removes content from selected elements
        /// </summary>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>
        public IBrowserWrapper ClearElementsContent(string selector, Func<string, By> tmpSelectMethod = null)
        {
            FindElements(selector, tmpSelectMethod).ForEach(s => { s.Clear(); s.Wait(); });
            return this;
        }

        /// <summary>
        /// Throws exception when provided object is null
        /// </summary>
        /// <param name="obj">Tested object</param>
        /// <param name="message">Failure message</param>
        public T ThrowIfIsNull<T>(T obj, string message)
        {
            if (obj == null)
            {
                throw new NoSuchElementException(message);
            }
            return obj;
        }

        /// <summary>
        /// Takes a screenshot and returns a full path to the file.
        /// </summary>
        ///<param name="filename">Path where the screenshot is going to be saved.</param>
        ///<param name="format">Default value is PNG.</param>
        public void TakeScreenshot(string filename, ScreenshotImageFormat? format = null)
        {
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filename, format ?? ScreenshotImageFormat.Png);
        }

        /// <summary>
        /// Closes the current browser
        /// </summary>
        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }



        #region FileUploadDialog

        public virtual IBrowserWrapper SendEnterKey()
        {

            System.Windows.Forms.SendKeys.SendWait("{Enter}");
            Wait();
            return this;
        }

        public virtual IBrowserWrapper SendEscKey()
        {
            System.Windows.Forms.SendKeys.SendWait("{ESC}");
            Wait();
            return this;
        }

        #endregion FileUploadDialog

        #region Frames support

        public IBrowserWrapper GetFrameScope(string selector)
        {
            var options = new ScopeOptions { FrameSelector = selector, Parent = this, CurrentWindowHandle = Driver.CurrentWindowHandle };
            var iframe = First(selector);
            //AssertUI.CheckIfTagName(iframe, new[] { "iframe", "frame" }, $"The selected element '{iframe.FullSelector}' is not a iframe element.");

            iframe.CheckIfTagName(new[] { "iframe", "frame" }, $"The selected element '{iframe.FullSelector}' is not a iframe element.");
            var frame = browser.Driver.SwitchTo().Frame(iframe.WebElement);
            testInstance.TestClass.CurrentScope = options.ScopeId;

            // create a new browser wrapper
            var type = testInstance.TestClass.TestSuiteRunner.ServiceFactory.Resolve<BrowserWrapper>();
            var wrapper = (BrowserWrapper)Activator.CreateInstance(type, browser, frame, testInstance, options);

            return wrapper;
        }

        #endregion Frames support


        /// <summary>
        /// Waits until the condition is true.
        /// </summary>
        /// <param name="condition">Expression that determine whether test should wait or continue</param>
        /// <param name="maxTimeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="failureMessage">Message which is displayed in exception log in case that the condition is not reached</param>
        /// <param name="ignoreCertainException">When StaleElementReferenceException or InvalidElementStateException is thrown than it would be ignored.</param>
        /// <param name="checkInterval">Interval in miliseconds. RECOMMENDATION: let the interval greater than 250ms</param>
        public IBrowserWrapper WaitFor(Func<bool> condition, int maxTimeout, string failureMessage, bool ignoreCertainException = true, int checkInterval = 500)
        {
            if (condition == null)
            {
                throw new NullReferenceException("Condition cannot be null.");
            }
            var now = DateTime.UtcNow;

            bool isConditionMet = false;
            Exception ex = null;
            do
            {
                try
                {
                    isConditionMet = condition();
                }
                catch (StaleElementReferenceException)
                {
                    if (!ignoreCertainException)
                        throw;
                }
                catch (InvalidElementStateException)
                {
                    if (!ignoreCertainException)
                        throw;
                }

                if (DateTime.UtcNow.Subtract(now).TotalMilliseconds > maxTimeout)
                {
                    throw new WaitBlockException(failureMessage);
                }
                Wait(checkInterval);
            } while (!isConditionMet);
            return this;
        }
        public IBrowserWrapper WaitFor(Action checkExpression, int maxTimeout, string failureMessage, int checkInterval = 500)
        {
            return WaitFor(() =>
            {
                try
                {
                    checkExpression();
                }
                catch
                {
                    return false;
                }
                return true;
            }, maxTimeout, failureMessage, true, checkInterval);
        }
        /// <summary>
        /// Repeats execution of the action until the action is executed without exception.
        /// </summary>
        /// <param name="maxTimeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="checkInterval">Interval in miliseconds. RECOMMENDATION: let the interval greater than 250ms</param>
        public IBrowserWrapper WaitFor(Action action, int maxTimeout, int checkInterval = 500, string failureMessage = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            var now = DateTime.UtcNow;

            Exception exceptionThrown = null;
            do
            {
                try
                {
                    action();
                    exceptionThrown = null;
                }
                catch (Exception ex)
                {
                    exceptionThrown = ex;
                }

                if (DateTime.UtcNow.Subtract(now).TotalMilliseconds > maxTimeout)
                {
                    if (failureMessage != null)
                    {
                        throw new WaitBlockException(failureMessage, exceptionThrown);
                    }
                    throw exceptionThrown;
                }
                Wait(checkInterval);
            } while (exceptionThrown != null);
            return this;
        }


        /// <summary>
        /// Transforms relative Url to absolute. Uses base URL.
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public string GetAbsoluteUrl(string relativeUrl)
        {
            var currentUri = new Uri(BaseUrl);
            return relativeUrl.StartsWith("/") ? $"{currentUri.Scheme}://{currentUri.Host}:{currentUri.Port}{relativeUrl}" : $"{currentUri.Scheme}://{currentUri.Host}:{currentUri.Port}/{relativeUrl}";
        }

        /// <summary>
        /// Switches browser tabs.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IBrowserWrapper SwitchToTab(int index)
        {
            Driver.SwitchTo().Window(Driver.WindowHandles[index]);
            return this;
        }

        public void ActivateScope()
        {
            if (testInstance.TestClass.CurrentScope == ScopeOptions.ScopeId)
            {
                return;
            }

            if (ScopeOptions.Parent != null && ScopeOptions.Parent != this)
            {
                ScopeOptions.Parent.ActivateScope();
            }
            else
            {
                if (ScopeOptions.CurrentWindowHandle != null && driver.CurrentWindowHandle != ScopeOptions.CurrentWindowHandle)
                {
                    driver.SwitchTo().Window(ScopeOptions.CurrentWindowHandle);
                }
                if (ScopeOptions.Parent == null)
                {
                    driver.SwitchTo().DefaultContent();
                }

                if (ScopeOptions.FrameSelector != null)
                {
                    driver.SwitchTo().Frame(ScopeOptions.FrameSelector);
                }
            }
            testInstance.TestClass.CurrentScope = ScopeOptions.ScopeId;
        }

        public string GetTitle() => Driver.Title;


        /// <summary>
        /// Returns WebDriver without scope activation. Be carefull!!! This is unsecure!
        /// </summary>
        public IWebDriver _GetInternalWebDriver()
        {
            testInstance.TestClass.CurrentScope = Guid.Empty;
            return Driver;
        }


    }
}