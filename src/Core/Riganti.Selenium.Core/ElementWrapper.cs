using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using OpenQA.Selenium.Interactions;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;
using Riganti.Selenium.Core.Api;
using Riganti.Selenium.Validators.Checkers;
using Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers;
using Riganti.Selenium.Core.Comparators;
using System.Collections.Generic;

namespace Riganti.Selenium.Core
{
    /// <inheritdoc />
    public class ElementWrapper : IElementWrapper
    {
        protected readonly Func<IWebElement> element;
        protected readonly IBrowserWrapper browser;

        /// <inheritdoc cref="IElementWrapper.Selector"/>
        public string Selector { get; set; }


        /// <inheritdoc />
        public string FullSelector => GenerateFullSelector();


        /// <inheritdoc />
        public void ActivateScope()
        {
            ParentWrapper.ActivateScope();
        }


        /// <inheritdoc />
        public int ActionWaitTime { get; set; }


        /// <inheritdoc />
        public IWebElement WebElement
        {
            get
            {
                ParentWrapper.ActivateScope();
                return element();
            }
        }

        public IBrowserWrapper BrowserWrapper => browser;

        /// <summary>
        /// Parent wrapper
        /// </summary>
        public ISeleniumWrapper ParentWrapper { get; set; }

        protected Func<string, By> selectMethod = null;

        /// <inheritdoc />
        public Func<string, By> SelectMethod
        {
            get => selectMethod ?? browser.SelectMethod;
            set => selectMethod = value;
        }

        protected readonly OperationResultValidator operationResultValidator = new OperationResultValidator();

        /// <summary>
        /// Returns html direct parent element.
        /// </summary>
        public IElementWrapper ParentElement
        {
            get
            {
                IElementWrapper parent;
                try
                {
                    parent = First("..", By.XPath);
                }
                catch (Exception ex)
                {
                    throw new NoSuchElementException($"Parent element of '{FullSelector}' was not found!", ex);
                }
                if (parent == null)
                    throw new NoSuchElementException($"Parent element of '{FullSelector}' was not found!");
                return parent;
            }
        }

        /// <summary>
        /// Contains direct children of the element.
        /// </summary>
        public IElementWrapperCollection<IElementWrapper, IBrowserWrapper> Children => FindElements("child::*", By.XPath);

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementWrapper"/> class.
        /// </summary>
        /// <param name="webElementSelector">The web element.</param>
        /// <param name="browserWrapper">The browser wrapper.</param>
        public ElementWrapper(Func<IWebElement> webElementSelector, IBrowserWrapper browserWrapper)
        {
            element = webElementSelector;
            browser = browserWrapper;
            SelectMethod = browser.SelectMethod;
            BaseUrl = browser.BaseUrl;
            ActionWaitTime = browserWrapper.ActionWaitTime;
        }

        /// <summary>
        /// Absolute Url that is used to resolve relative addresses when function NavigateTo is used.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Set css selector as current select method.
        /// </summary>
        public void SetCssSelectMethod()
        {
            selectMethod = SelectBy.CssSelector;
        }

        /// <summary>
        /// Resets select method to value of BrowserWrapper.
        /// </summary>
        public void SetBrowserSelectMethod()
        {
            selectMethod = null;
        }


        /// <inheritdoc />
        public IElementWrapper SendEnterKey()
        {
            SendKeys(Keys.Enter);
            return this;
        }
        /// <summary>
        /// Generates full selector from all parents.
        /// </summary>
        /// <returns></returns>
        protected string GenerateFullSelector()
        {
            if (ParentWrapper is IElementWrapperCollection<IElementWrapper, IBrowserWrapper> parent)
            {
                var index = parent.IndexOf(this);
                var parentSelector = string.IsNullOrWhiteSpace(parent.FullSelector) ? "" : parent.FullSelector.Trim() + $":nth-child({index + 1})";
                return $"{parentSelector}".Trim();
            }
            return $"{Selector ?? ""}".Trim();
        }



        public virtual string GetJsElementPropertyValue(string elementPropertyName)
        {
            var obj = browser.GetJavaScriptExecutor()?.ExecuteScript(@"return (arguments || [{}])[0]['" + elementPropertyName + "'];", WebElement);
            return obj?.ToString();
        }

        /// <summary>
        /// Inserts javascript to the site and returns value of innerText/textContent property of this element.
        /// </summary>
        public virtual IElementWrapper SetJsElementProperty(string propertyName, object propertyValue)
        {
            browser.GetJavaScriptExecutor()?.ExecuteScript(" var a = (arguments || [{}, null, null]);    var element = a[0];    var propertyName = a[1];    var propertyValue = a[2]; element[propertyName] = propertyValue;", WebElement, propertyName, propertyValue);
            return this;
        }

        /// <summary>
        /// Inserts javascript to the site and returns value of innerText/textContent property of this element.
        /// </summary>
        public virtual string GetJsInnerText(bool trim = true)
        {
            var obj = browser.GetJavaScriptExecutor()?.ExecuteScript(@"var a = (arguments || [{}])[0];return a['innerText'] || a['textContent'];", WebElement);
            return trim ? obj?.ToString().Trim() : obj?.ToString();
        }


        public bool IsClickable()
        {
            var obj = this.browser.GetJavaScriptExecutor().ExecuteScript(@"
                if(arguments.length === 0) {
                    throw ""Function IsClickableValidator requires element in arguments."";
                }
                var elm = arguments[0];
                var rec = elm.getBoundingClientRect();

                // is not visible
                if (rec.width === 0 || rec.height === 0)
                {
                    return false;
                }
                //check if is on top

                var top = document.elementFromPoint(rec.left + (rec.width / 2), rec.top + (rec.height / 2));
                return top === elm;
            ", this.WebElement);
            var a = (bool)obj;
            return a;
        }

        /// <summary>
        /// Inserts javascript to the site and returns value of innerHTML property of this element.
        /// </summary>
        /// <remarks>Some browsers adds unneccessery attributes to InnerHtml property. Comparison of raw html strings is NOT recommended.</remarks>
        public string GetJsInnerHtml()
        {
            return GetJsElementPropertyValue("innerHTML");
        }


        public string GetAttribute(string name)
        {
            return WebElement.GetAttribute(name);
        }

        public bool HasAttribute(string name)
        {
            bool result = false;
            var obj = browser.GetJavaScriptExecutor()?.ExecuteScript("return (arguments || [{attributes:[]}])[0].attributes[\"" + name + "\"] !== undefined;", WebElement);
            bool.TryParse((obj?.ToString() ?? "false"), out result);
            return result;
        }

        /// <inheritdoc />
        public bool HasFocus()
        {
            return WebElement.Equals(BrowserWrapper.Driver.SwitchTo().ActiveElement());
        }

        /// <inheritdoc />
        public void SetFocus()
        {
            var executor = BrowserWrapper.GetJavaScriptExecutor();
            var result = executor.ExecuteScript(@"
function setFocus(elm) {
 if(elm.focus){
 {
	 elm.focus();
 }
	try{
        if(elm.setSelectionRange && elm.value && elm.value.length)
        {
    	    elm.setSelectionRange(elm.value.length,elm.value.length);
    	}
	}catch(e){
	}
	return true; 
}
return false;
}; return setFocus(arguments[0]);
                ", WebElement);


        }

        /// <summary>
        /// Returns trimmed text when trim parametr is true.
        /// </summary>
        protected string GetTrimmedText(bool trim)
        {
            if (trim)
            {
                return GetText()?.Trim();
            }
            return GetText();
        }


        /// <summary>
        /// Sets current option of element &lt;select&gt;.
        /// </summary>
        /// <param name="index">Index of  &lt;option&gt; that should be selected.</param>
        public virtual IElementWrapper Select(int index)
        {
            return Select(e => e.SelectByIndex(index));
        }

        /// <summary>
        /// Sets current option of element &lt;select&gt;.
        /// </summary>
        /// <param name="value">Value of  &lt;option&gt; that should be selected.</param>
        public virtual IElementWrapper Select(string value)
        {
            return Select(e => e.SelectByValue(value));
        }

        /// <summary>
        /// Sets current option of element &lt;select&gt;.
        /// </summary>
        public virtual IElementWrapper Select(Action<SelectElement> process)
        {
            return PerformActionOnSelectElement(process);
        }

        public virtual IElementWrapper PerformActionOnSelectElement(Action<SelectElement> process)
        {
            var selectElm = new SelectElement(WebElement);
            process(selectElm);
            return this;
        }

        public virtual string GetTagName()
        {
            return WebElement.TagName.Trim().ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Submits this element to the web server.
        /// </summary>
        /// <remarks>
        /// If this current element is a form, or an element within a form,
        ///             then this will be submitted to the web server. If this causes the current
        ///             page to change, then this method will block until the new page is loaded.
        /// </remarks>

        public virtual IElementWrapper Submit()
        {
            WebElement.Submit();
            return this;
        }

        public virtual IElementWrapper SendKeys(string text)
        {
            WebElement.SendKeys(text);
            return this;
        }

        /// <summary>
        /// Finds all elements that satisfy the condition of css selector.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="tmpSelectMethod"> This select method is used only for selection of elements in this query. Not in the future.</param>
        /// <returns></returns>
        public virtual IElementWrapperCollection<IElementWrapper, IBrowserWrapper> FindElements(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var usedSelectMethod = tmpSelectMethod ?? SelectMethod;
            var collection = Extensions.ToElementsList<IElementWrapper, IBrowserWrapper>(
                                                       () => WebElement.FindElements(usedSelectMethod(selector)),
                                                       browser, selector, usedSelectMethod, this, browser.ServiceFactory);
            collection.ParentWrapper = this;
            return collection;
        }

        /// <summary>
        /// Returns first element that satisfies the selector.
        /// </summary>
        public virtual IElementWrapper FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var elms = FindElements(selector, tmpSelectMethod);
            return elms.FirstOrDefault();
        }

        /// <summary>
        /// Returns first element that satisfies the selector. Throws exception when no element is found.
        /// </summary>
        public virtual IElementWrapper First(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return ThrowIfIsNull(FirstOrDefault(selector, tmpSelectMethod), $"Element not found. Selector: {selector}");
        }

        /// <summary>
        /// Returns one element and throws exception when more then one element is found.
        /// </summary>
        public virtual IElementWrapper SingleOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).SingleOrDefault();
        }

        /// <summary>
        /// Returns one element and throws exception when no element or more then one element is found.
        /// </summary>
        public virtual IElementWrapper Single(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).Single();
        }

        /// <summary>
        /// Returns one element from collection at specified position and throws exception when collection contains less elements then is expected.
        /// </summary>
        public virtual IElementWrapper ElementAt(string selector, int index, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).ElementAt(index);
        }

        /// <summary>
        /// Returns last element from collection and Throws exception when the collection is empty.
        /// </summary>
        public virtual IElementWrapper Last(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).Last();
        }

        /// <summary>
        /// Returns last element from collection or null when collection is empty.
        /// </summary>
        public virtual IElementWrapper LastOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return FindElements(selector, tmpSelectMethod).LastOrDefault();
        }

        /// <summary>
        /// Throws exception when provided object is null.
        /// </summary>
        public T ThrowIfIsNull<T>(T obj, string message)
        {
            if (obj == null)
            {
                throw new NoSuchElementException(message);
            }
            return obj;
        }

        public virtual string GetInnerText()
        {
            return WebElement.Text;
        }

        public virtual string GetText()
        {
            string[] valueElements = new[] { "input", "textarea" };
            if (valueElements.Contains(WebElement.TagName.Trim().ToLower()))
            {
                return GetAttribute("value");
            }
            return WebElement.Text;
        }
        /// <inheritdoc/>
        public Size GetSize()
        {
            return WebElement.Size;
        }

        /// <inheritdoc />
        public virtual IElementWrapper Click()
        {
            WebElement.Click();
            return this;
        }

        /// <inheritdoc />
        public string GetValue()
        {
            var tag = GetTagName();
            string elementValue = null;
            //input
            if (tag == "input")
            {
                elementValue = WebElement.GetAttribute("value");
            }
            //textarea
            if (tag == "textarea")
            {
                elementValue = GetInnerText();
            }

            return elementValue;
        }

        protected bool IsChecked()
        {
            return WebElement.Selected;
        }

        protected bool TryParseBool(string value)
        {
            bool tmp;
            bool.TryParse(value, out tmp);
            return tmp;
        }



        /// <summary>
        /// Indicates whether element is visible.
        /// An element that has zero width or height also counts as non visible.
        /// </summary>
        public bool IsDisplayedAndHasSizeGreaterThanZero()
        {
            return WebElement.Displayed && !(WebElement.Size.Height == 0 || WebElement.Size.Width == 0);
        }

        /// <summary>
        /// Indicates whether element is visible.
        /// An element that has zero width or height also counts as non visible.
        /// </summary>
        public bool IsDisplayed()
        {
            return WebElement.Displayed;
        }

        /// <summary>
        /// Indicates whether element is selected.
        /// </summary>
        public bool IsSelected()
        {
            return WebElement.Selected;
        }

        /// <summary>
        /// Indicates whether element is enabled.
        /// </summary>
        public bool IsEnabled()
        {
            return WebElement.Enabled;
        }

        /// <summary>
        /// Removes content of element.
        ///
        /// Note: Browser internally fires onchange event!
        /// </summary>
        public IElementWrapper Clear()
        {
            WebElement.Clear();
            return this;
        }
        /// <summary>
        /// Removes content of element by performing sequence of keyboard strokes.
        /// </summary>
        public IElementWrapper ClearInputByKeyboard()
        {
            var navigator = new Actions(BrowserWrapper.Driver);
            navigator.Click(WebElement)
                .SendKeys(Keys.End)
                .KeyDown(Keys.Shift)
                .SendKeys(Keys.Home)
                .KeyUp(Keys.Shift)
                .SendKeys(Keys.Backspace)
                .Perform();
            return this;
        }

        [Obsolete("Please use WaitFor or specify exact timeout.")]
        public virtual IElementWrapper Wait()
        {
            if (ActionWaitTime != 0)
                Thread.Sleep(ActionWaitTime);
            return this;
        }


        public virtual IElementWrapper Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        /// <summary>
        /// Waits until the condition is true.
        /// </summary>
        /// <param name="condition">Expression that determine whether test should wait or continue</param>
        /// <param name="maxTimeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="failureMessage">Message which is displayed in exception log in case that the condition is not reached</param>
        /// <param name="ignoreCertainException">When StaleElementReferenceException or InvalidElementStateException is thrown than it would be ignored.</param>
        /// <param name="checkInterval">Interval in milliseconds. RECOMMENDATION: let the interval greater than 30ms</param>
        public IElementWrapper WaitFor(Func<IElementWrapper, bool> condition, int maxTimeout, string failureMessage, bool ignoreCertainException = true, int checkInterval = 30)
        {
            if (condition == null)
            {
                throw new NullReferenceException("Condition cannot be null.");
            }
            var now = DateTime.UtcNow;

            bool isConditionMet = false;
            do
            {
                try
                {
                    isConditionMet = condition(this);
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

        public IElementWrapper WaitFor(Action<IElementWrapper> checkExpression, int maxTimeout, string failureMessage, int checkInterval = 30)
        {
            return WaitFor(elm =>
            {
                try
                {
                    checkExpression(elm);
                }
                catch
                {
                    return false;
                }
                return true;
            }, maxTimeout, failureMessage, true, checkInterval);
        }

        public IElementWrapper WaitFor(Func<IElementWrapper, IElementWrapper> selector, WaitForOptions options = null)
        {
            IElementWrapper wrapper = null;
            WaitForExecutor.WaitFor(() =>
            {
                wrapper = selector(this);
                if (wrapper is null) throw new ElementNotFoundException();
            }, options);

            return wrapper;
        }

        public IElementWrapperCollection<IElementWrapper, IBrowserWrapper> WaitFor(Func<IElementWrapper, IElementWrapperCollection<IElementWrapper, IBrowserWrapper>> selector, WaitForOptions options = null)
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
        /// Waits the specified time before next step.
        /// </summary>
        public virtual IElementWrapper Wait(TimeSpan interval)
        {
            Thread.Sleep(interval);
            return this;
        }


        public IElementWrapper ScrollTo(WaitForOptions waitForOptions = null)
        {
            var javascript = @"
            function findPosition(element) {
                var curtop = 0;
                if (element.offsetParent) {
                    do {
                        curtop += element.offsetTop;
                    } while (element = element.offsetParent);
                return [curtop];
                }
            }

            window.scroll(0,findPosition(arguments[0]));
        ";
            var executor = browser.GetJavaScriptExecutor();
            executor.ExecuteScript(javascript, WaitForInternalElement(waitForOptions));
            return this;
        }

        private IWebElement WaitForInternalElement(WaitForOptions waitForOptions)
        {
            IWebElement elm = null;
            var w = new WaitForExecutor();
            w.WaitFor(() =>
            {
                elm = element();
                return elm != null;
            }, waitForOptions);
            return elm;
        }

        public bool IsElementInView(IElementWrapper element)
        {
            var executor = element.BrowserWrapper.GetJavaScriptExecutor();

            var result = executor.ExecuteScript(@"
function elementInViewport2(el) {
  var top = el.offsetTop;
  var left = el.offsetLeft;
  var width = el.offsetWidth;
  var height = el.offsetHeight;

  while(el.offsetParent) {
    el = el.offsetParent;
    top += el.offsetTop;
    left += el.offsetLeft;
  }

  return (
    top < (window.pageYOffset + window.innerHeight) &&
    left < (window.pageXOffset + window.innerWidth) &&
    (top + height) > window.pageYOffset &&
    (left + width) > window.pageXOffset
  );
}

return elementInViewport2(arguments[0]);
                ", element.WebElement);

            return (bool)result;
        }

        /// <summary>
        /// Drag this element and drop to dropToElement with offsetX and offsetY.
        /// </summary>
        /// <param name="dropToElement"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        public IElementWrapper DropTo(IElementWrapper dropToElement, int offsetX = 0, int offsetY = 0)
        {
            browser.DragAndDrop(this, dropToElement, offsetX, offsetY);
            return this;
        }

        /// <summary>
        /// Determines whether the specified CSS class.
        /// </summary>
        /// <param name="cssClass">The CSS class.</param>
        /// <returns>
        ///   <c>true</c> if has the specified CSS class otherwise, <c>false</c>.
        /// </returns>
        public bool HasCssClass(string cssClass)
        {
            var attr = GetAttribute("class");
            return attr.Split(' ').Any(s => string.Equals(cssClass, s, StringComparison.OrdinalIgnoreCase));
        }
    }
}