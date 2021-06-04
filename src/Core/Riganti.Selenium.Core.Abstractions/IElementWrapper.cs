using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Riganti.Selenium.Core.Abstractions
{
    public interface IElementWrapper : ISeleniumWrapper, ISupportedByValidator
    {
        /// <summary>
        /// Default timeout for Wait function.
        /// </summary>
        int ActionWaitTime { get; set; }

        /// <summary>
        /// Gets selector used to get this element.
        /// </summary>
        new string Selector { get; set; }

        string BaseUrl { get; set; }
        IBrowserWrapper BrowserWrapper { get; }
        IElementWrapperCollection<IElementWrapper, IBrowserWrapper> Children { get; }
        IElementWrapper ParentElement { get; }

        /// <summary>
        /// Describes defautl way how to get element(s) from site. (by css selector, tag name, xpath,..)
        /// </summary>
        Func<string, By> SelectMethod { get; set; }

        /// <summary>
        /// Unwrapped IWebElement binding implementation (SeleniumHQ).
        /// </summary>
        IWebElement WebElement { get; }

        /// <summary>
        /// Clears input values. 
        ///
        /// Note: Browser internally fires onchange event!
        /// </summary>
        IElementWrapper Clear();

        /// <summary>
        /// Removes content of element by performing sequence of keyboard strokes.
        /// </summary>
        IElementWrapper ClearInputByKeyboard();
        /// <summary>
        /// Performs click on this element.
        /// </summary>
        IElementWrapper Click();

        IElementWrapper DropTo(IElementWrapper dropToElement, int offsetX = 0, int offsetY = 0);
        IElementWrapper ElementAt(string selector, int index, Func<string, By> tmpSelectMethod = null);

        IElementWrapperCollection<IElementWrapper, IBrowserWrapper> FindElements(string selector,
            Func<string, By> tmpSelectMethod = null);

        IElementWrapper First(string selector, Func<string, By> tmpSelectMethod = null);
        IElementWrapper FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null);

        /// <summary>
        /// Returns value of attribute specified by name.
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        string GetAttribute(string name);

        /// <summary>
        /// Returns inner text of element.
        /// </summary>
        string GetInnerText();

        string GetJsElementPropertyValue(string elementPropertyName);

        /// <summary>
        /// Returns html included in page by javascript execution. 
        /// </summary>
        string GetJsInnerHtml();

        string GetJsInnerText(bool trim = true);

        /// <summary>
        /// Returns element's tag name.
        /// </summary>
        string GetTagName();

        string GetText();

        /// <summary>
        /// Returns text from value attribute at input element or inner text for textarea element.
        /// </summary>
        string GetValue();

        bool HasAttribute(string name);

        /// <summary>
        /// Returns true when web element has focus.
        /// </summary>
        bool HasFocus();

        /// <summary>
        /// Sets element as active element in page.
        /// </summary>
        void SetFocus();

        bool HasCssClass(string cssClass);
        bool IsClickable();
        bool IsDisplayed();
        bool IsDisplayedAndHasSizeGreaterThanZero();
        bool IsElementInView(IElementWrapper element);
        bool IsEnabled();
        bool IsSelected();

        /// <summary>
        /// Returns size of the element.
        /// </summary>
        Size GetSize();

        IElementWrapper Last(string selector, Func<string, By> tmpSelectMethod = null);
        IElementWrapper LastOrDefault(string selector, Func<string, By> tmpSelectMethod = null);
        IElementWrapper PerformActionOnSelectElement(Action<SelectElement> process);
        IElementWrapper ScrollTo(WaitForOptions waitForOptions = null);
        IElementWrapper Select(Action<SelectElement> process);

        /// <summary>
        /// Selects option of SELECT element.
        /// </summary>
        IElementWrapper Select(int index);

        /// <summary>
        /// Selects option of SELECT element.
        /// </summary>
        IElementWrapper Select(string value);

        /// <summary>
        /// Sends enter key to browser.
        /// </summary>
        /// <returns></returns>
        IElementWrapper SendEnterKey();

        IElementWrapper SendKeys(string text);
        void SetBrowserSelectMethod();
        void SetCssSelectMethod();
        IElementWrapper SetJsElementProperty(string propertyName, object propertyValue);
        IElementWrapper Single(string selector, Func<string, By> tmpSelectMethod = null);
        IElementWrapper SingleOrDefault(string selector, Func<string, By> tmpSelectMethod = null);
        IElementWrapper Submit();
        T ThrowIfIsNull<T>(T obj, string message);

        /// <summary>
        /// Stops execution for time specified in ActionWaitTime.
        /// </summary>
        [Obsolete("Please use WaitFor or exact timeout.")]
        IElementWrapper Wait();

        /// <summary>
        /// Stops execution for specified time (ms).
        /// </summary>
        /// <param name="milliseconds">time to wait</param>
        /// <returns></returns>
        IElementWrapper Wait(int milliseconds);

        /// <summary>
        /// Stops execution for specified time.
        /// </summary>
        IElementWrapper Wait(TimeSpan interval);

        /// <summary>
        /// Waits until checkExpression is satisfied.
        /// </summary>
        /// <param name="checkExpression">rule to check</param>
        /// <param name="maxTimeout">maximum time to wait</param>
        /// <param name="failureMessage">message in case that rule is not satisfied</param>
        /// <param name="checkInterval">interval between individual checks</param>
        /// <returns></returns>
        IElementWrapper WaitFor(Action<IElementWrapper> checkExpression, int maxTimeout, string failureMessage,
            int checkInterval = 30);

        IElementWrapper WaitFor(Func<IElementWrapper, bool> condition, int maxTimeout, string failureMessage,
            bool ignoreCertainException = true, int checkInterval = 30);
        IElementWrapperCollection<IElementWrapper, IBrowserWrapper> WaitFor(Func<IElementWrapper, IElementWrapperCollection<IElementWrapper, IBrowserWrapper>> selector, WaitForOptions options = null);
        IElementWrapper WaitFor(Func<IElementWrapper, IElementWrapper> selector, WaitForOptions options = null);
    }
}