using System;
using OpenQA.Selenium;

namespace Riganti.Selenium.Core.Abstractions
{
    public interface IBrowserWrapper : ISupportedByValidator
    {
        int ActionWaitTime { get; set; }
        string BaseUrl { get; }
        string CurrentUrl { get; }
        string CurrentUrlPath { get; }
        IWebDriver Driver { get; }
        Func<string, By> SelectMethod { get; set; }
        IServiceFactory ServiceFactory { get; }

        void ActivateScope();

        IBrowserWrapper ClearElementsContent(string selector, Func<string, By> tmpSelectMethod = null);

        IBrowserWrapper Click(string selector);

        IBrowserWrapper ConfirmAlert();

        IBrowserWrapper DismissAlert();

        void Dispose();

        IElementWrapper ElementAt(string selector, int index, Func<string, By> tmpSelectMethod = null);

        IElementWrapperCollection<IElementWrapper, IBrowserWrapper> FindElements(By selector);

        IElementWrapperCollection<IElementWrapper, IBrowserWrapper> FindElements(string cssSelector,
            Func<string, By> tmpSelectMethod = null);

        IBrowserWrapper FireJsBlur();

        IElementWrapper First(string selector, Func<string, By> tmpSelectMethod = null);

        IElementWrapper FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null);

        IBrowserWrapper ForEach(string selector, Action<IElementWrapper> action,
            Func<string, By> tmpSelectMethod = null);

        string GetAbsoluteUrl(string relativeUrl);

        IAlert GetAlert();

        string GetAlertText();

        IBrowserWrapper GetFrameScope(string selector);

        IJavaScriptExecutor GetJavaScriptExecutor();

        string GetTitle();

        bool HasAlert();

        bool IsDisplayed(string selector, Func<string, By> tmpSelectMethod = null);

        IElementWrapper Last(string selector, Func<string, By> tmpSelectMethod = null);

        IElementWrapper LastOrDefault(string selector, Func<string, By> tmpSelectMethod = null);

        void LogError(string message, Exception ex);

        void LogInfo(string message);

        void LogVerbose(string message);

        /// <summary>
        /// Navigates the browser one step back.
        /// </summary>
        void NavigateBack();

        void NavigateForward();

        /// <summary>
        /// Navigates to base url which is specified in seleniumconfig.json
        /// </summary>
        void NavigateToUrl();

        /// <summary>
        /// Navigates to specified url.
        /// </summary>
        void NavigateToUrl(string url);

        /// <summary>
        /// Refresh current page.
        /// </summary>
        void Refresh();

#if net461
        IBrowserWrapper SendEnterKey();
        IBrowserWrapper SendEscKey();
        void OpenFileDialog(IElementWrapper fileInputElement, string file);
#endif

        void OpenInputFileDialog(IElementWrapper fileInputElement, string file);

        IBrowserWrapper SendKeys(string selector, string text, Func<string, By> tmpSelectMethod = null);

        void SetCssSelector();

        void SetTimeouts(TimeSpan pageLoadTimeout, TimeSpan implicitlyWait);

        IElementWrapper Single(string selector, Func<string, By> tmpSelectMethod = null);

        IElementWrapper SingleOrDefault(string selector, Func<string, By> tmpSelectMethod = null);

        IBrowserWrapper Submit(string selector);

        /// <summary>
        /// Ends the test and marks it as a failed.
        /// </summary>
        /// <param name="message">reason of failure</param>
        void DropTest(string message);

        IBrowserWrapper DragAndDrop(IElementWrapper elementWrapper, IElementWrapper dropToElement, int offsetX = 0,
            int offsetY = 0);

        IBrowserWrapper SwitchToTab(int index);

        /// <summary>
        /// Takes screenshot of current screen.
        /// </summary>
        void TakeScreenshot(string filename, ScreenshotImageFormat? format = null);

        T ThrowIfIsNull<T>(T obj, string message);

        /// <summary>
        /// Waits the ActionWaitTime before next step.
        /// </summary>
        IBrowserWrapper Wait();

        /// <summary>
        /// Waits the specified time before next step.
        /// </summary>
        IBrowserWrapper Wait(int milliseconds);

        /// <summary>
        /// Waits the specified time before next step.
        /// </summary>
        IBrowserWrapper Wait(TimeSpan interval);

        /// <summary>
        /// Waits until the condition is true.
        /// </summary>
        /// <param name="action">Expression that determine whether test should wait or continue</param>
        /// <param name="timeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="failureMessage">Message which is displayed in exception log in case that the condition is not reached</param>
        /// <param name="checkInterval">Interval in milliseconds. RECOMMENDATION: let the interval greater than 30ms</param>
        IBrowserWrapper WaitFor(Action action, int timeout, int checkInterval = 30, string failureMessage = null);

        /// <summary>
        /// Waits until the condition is true.
        /// </summary>
        /// <param name="action">Expression that determine whether test should wait or continue</param>
        /// <param name="timeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="failureMessage">Message which is displayed in exception log in case that the condition is not reached</param>
        /// <param name="checkInterval">Interval in milliseconds. RECOMMENDATION: let the interval greater than 30ms</param>
        IBrowserWrapper WaitFor(Action action, int timeout, string failureMessage, int checkInterval = 30);

        /// <summary>
        /// Waits until the condition is true.
        /// </summary>
        /// <param name="condition">Expression that determine whether test should wait or continue</param>
        /// <param name="timeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="failureMessage">Message which is displayed in exception log in case that the condition is not reached</param>
        /// <param name="ignoreCertainException">When StaleElementReferenceException or InvalidElementStateException is thrown than it would be ignored.</param>
        /// <param name="checkInterval">Interval in milliseconds. RECOMMENDATION: let the interval greater than 250ms</param>
        IBrowserWrapper WaitFor(Func<bool> condition, int timeout, string failureMessage,
            bool ignoreCertainException = true, int checkInterval = 30);

        /// <summary>
        /// Waits until the condition is true.
        /// </summary>
        /// <param name="condition">Expression that determine whether test should wait or continue</param>
        /// <param name="timeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="failureMessage">Message which is displayed in exception log in case that the condition is not reached</param>
        /// <param name="checkInterval">Interval in milliseconds. RECOMMENDATION: let the interval greater than 30ms</param>
        IBrowserWrapper WaitFor(Func<bool> condition, int timeout, int checkInterval = 30,
            string failureMessage = null);

        /// <summary>
        /// Returns WebDriver without scope activation. Be careful!!! This is unsecure!
        /// This method can be used only for operations that do NOT use DOM.
        /// </summary>
        IWebDriver _GetInternalWebDriver();
        IElementWrapper WaitFor(Func<IBrowserWrapper, IElementWrapper> selector, WaitForOptions options = null);
        IElementWrapperCollection<IElementWrapper, IBrowserWrapper> WaitFor(Func<IBrowserWrapper, IElementWrapperCollection<IElementWrapper, IBrowserWrapper>> selector, WaitForOptions options = null);
    }
}