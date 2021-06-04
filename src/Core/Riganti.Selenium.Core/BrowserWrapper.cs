using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Schema;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;
using Riganti.Selenium.Core.Api;
using Riganti.Selenium.Core.Drivers;
using Riganti.Selenium.Validators.Checkers;
using Riganti.Selenium.Core.Configuration;
using Riganti.Selenium.Core;
using Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers;
using System.Net.Http;
using System.Collections.Generic;

namespace Riganti.Selenium.Core
{
    public class BrowserWrapper : IBrowserWrapper
    {

        protected readonly IWebBrowser browser;
        protected readonly IWebDriver driver;
        protected internal ITestInstance TestInstance { get; protected set; }
        public IServiceFactory ServiceFactory => TestInstance.TestClass.TestSuiteRunner.ServiceFactory;



        public int ActionWaitTime { get; set; }

        public string BaseUrl => TestInstance.TestConfiguration.BaseUrl;

        private readonly OperationResultValidator operationResultValidator = new OperationResultValidator();

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

            this.TestInstance = testInstance;
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
        /// <summary>
        /// Sets file to input dialog.
        /// </summary>
        ///<exception cref="UnexpectedElementException">The element is not input or type is not file.</exception>
        public void OpenInputFileDialog(IElementWrapper fileInputElement, string file)
        {
            if (!IsFileInput(fileInputElement))
            {
                throw new UnexpectedElementException("Tag name of the element has to be input and type has to be file.");
            }
            fileInputElement.SendKeys(file);
            Wait();
        }

#if net461
        [Obsolete]
        public void OpenFileDialog(IElementWrapper fileInputElement, string file)
        {
            // open file dialog
            fileInputElement.Click();
            Wait();

            //Another wait is needed because without it sometimes few chars from file path are skipped.
            Wait(1000);

            // write the full path to the dialog
            System.Windows.Forms.SendKeys.SendWait(file);
            Wait();
            SendEnterKey();
        }
#endif


        /// <summary>
        /// Determinates whether element is file dialog. (input[type=file])
        /// </summary>
        public bool IsFileInput(IElementWrapper fileInputElement)
        {
            return fileInputElement.GetTagName() == "input" && fileInputElement.HasAttribute("type") && fileInputElement.GetAttribute("type") == "file";
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
        public IElementWrapperCollection<IElementWrapper, IBrowserWrapper> FindElements(By selector)
        {
            return Extensions.ToElementsList<IElementWrapper, IBrowserWrapper>(() => Driver.FindElements(selector), this, selector.GetSelector(), _ => selector, TestInstance.TestClass.TestSuiteRunner.ServiceFactory);
        }

        /// <summary>
        /// Finds all elements that satisfy the condition of css selector.
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>
        public IElementWrapperCollection<IElementWrapper, IBrowserWrapper> FindElements(string cssSelector, Func<string, By> tmpSelectMethod = null)
        {
            var usedSelectMethod = (tmpSelectMethod ?? SelectMethod);
            return Extensions.ToElementsList<IElementWrapper, IBrowserWrapper>(() => Driver.FindElements(usedSelectMethod(cssSelector)), this, cssSelector, usedSelectMethod,
                TestInstance.TestClass.TestSuiteRunner.ServiceFactory);
        }

        /// <summary>
        /// Returns first element that is found by the selector or returns null.
        /// </summary>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>

        public IElementWrapper FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var elms = FindElements(selector, tmpSelectMethod);
            return elms.FirstOrDefault();
        }

        /// <summary>
        /// Returns first element that is found by the selector.
        /// </summary>
        /// <param name="selector">Defines path to select element.</param>
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
            FindElements(selector, tmpSelectMethod).ForEach(s => { s.SendKeys(text); });
            return this;
        }

        /// <summary>
        /// Removes content from selected elements
        /// </summary>
        /// <param name="tmpSelectMethod">temporary method which determine how the elements are selected</param>
        public IBrowserWrapper ClearElementsContent(string selector, Func<string, By> tmpSelectMethod = null)
        {
            FindElements(selector, tmpSelectMethod).ForEach(s => { s.Clear(); });
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
#if net461
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
#endif

        #endregion FileUploadDialog

        #region Frames support

        public IBrowserWrapper GetFrameScope(string selector)
        {
            var options = new ScopeOptions { FrameSelector = selector, Parent = this, CurrentWindowHandle = Driver.CurrentWindowHandle };
            var iframe = First(selector);
            //AssertUI.CheckIfTagName(iframe, new[] { "iframe", "frame" }, $"The selected element '{iframe.FullSelector}' is not a iframe element.");


            var resultValidator = new OperationResultValidator();
            var validator = new TagNameValidator(new[] { "iframe", "frame" },
                $"The selected element '{iframe.FullSelector}' is not a iframe element.");
            var results = validator.Validate(iframe);

            resultValidator.Validate<UnexpectedElementStateException>(results);

            var frame = browser.Driver.SwitchTo().Frame(iframe.WebElement);
            TestInstance.TestClass.CurrentScope = options.ScopeId;

            // create a new browser wrapper
            return TestInstance.TestClass.TestSuiteRunner.ServiceFactory.Resolve<IBrowserWrapper>(browser, frame, TestInstance, options);
        }

        #endregion Frames support


        /// <inheritdoc />
        public IBrowserWrapper WaitFor(Func<bool> condition, int timeout, string failureMessage, bool ignoreCertainException = true, int checkInterval = 30)
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

                if (DateTime.UtcNow.Subtract(now).TotalMilliseconds > timeout)
                {
                    throw new WaitBlockException(failureMessage);
                }
                Wait(checkInterval);
            } while (!isConditionMet);
            return this;
        }
        /// <inheritdoc />

        public IBrowserWrapper WaitFor(Func<bool> condition, int timeout, int checkInterval = 30, string failureMessage = null)
        {
            return WaitFor(condition, timeout, failureMessage, true, checkInterval);
        }
        /// <inheritdoc />

        public IBrowserWrapper WaitFor(Action action, int timeout, string failureMessage, int checkInterval = 30)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            Exception exception = null;
            return WaitFor(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
                return true;
            }, timeout, failureMessage ?? exception?.ToString(), true, checkInterval);
        }
        /// <inheritdoc />
        public IBrowserWrapper WaitFor(Action action, int timeout, int checkInterval = 30, string failureMessage = null)
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

                if (DateTime.UtcNow.Subtract(now).TotalMilliseconds > timeout)
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

        public IElementWrapper WaitFor(Func<IBrowserWrapper, IElementWrapper> selector, WaitForOptions options = null)
        {
            IElementWrapper wrapper = null;
            WaitForExecutor.WaitFor(() =>
            {
                wrapper = selector(this);
                if (wrapper is null) throw new ElementNotFoundException();
            }, options);

            return wrapper;
        }

        public IElementWrapperCollection<IElementWrapper, IBrowserWrapper> WaitFor(Func<IBrowserWrapper, IElementWrapperCollection<IElementWrapper, IBrowserWrapper>> selector, WaitForOptions options = null)
        {
            IElementWrapperCollection<IElementWrapper, IBrowserWrapper> wrappers = null;
            WaitForExecutor.WaitFor(() =>
            {
                wrappers = selector(this);
                if (wrappers is null) throw new ElementNotFoundException();
            }, options);

            return wrappers;
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

        public IBrowserWrapper DragAndDrop(IElementWrapper elementWrapper, IElementWrapper dropToElement, int offsetX = 0, int offsetY = 0)
        {
            Actions dragAndDrop = new Actions(_GetInternalWebDriver());
            dragAndDrop.ClickAndHold(elementWrapper.WebElement)
                .MoveToElement(dropToElement.WebElement, offsetX, offsetY)
                .Release(dropToElement.WebElement).Build().Perform();

            return this;
        }

        public void ActivateScope()
        {
            if (TestInstance.TestClass.CurrentScope == ScopeOptions.ScopeId)
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
            TestInstance.TestClass.CurrentScope = ScopeOptions.ScopeId;
        }

        public string GetTitle() => Driver.Title;


        /// <summary>
        /// Returns WebDriver without scope activation. Be carefull!!! This is unsecure!
        /// </summary>
        public IWebDriver _GetInternalWebDriver()
        {
            TestInstance.TestClass.CurrentScope = Guid.Empty;
            return driver;
        }

        protected IBrowserWrapper EvaluateBrowserCheck<TException>(IValidator<IBrowserWrapper> validator)
            where TException : TestExceptionBase, new()
        {
            var operationResult = validator.Validate(this);
            operationResultValidator.Validate<TException>(operationResult);
            return this;
        }
    }
}