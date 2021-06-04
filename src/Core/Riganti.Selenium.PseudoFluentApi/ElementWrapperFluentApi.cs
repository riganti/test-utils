using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Riganti.Selenium.Core;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;
using Riganti.Selenium.Core.Api;
using Riganti.Selenium.Validators.Checkers;
using Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers;

namespace Riganti.Selenium.Core
{
    public class ElementWrapperFluentApi : ElementWrapper, IElementWrapperFluentApi
    {


        public new IBrowserWrapperFluentApi BrowserWrapper => (IBrowserWrapperFluentApi)browser;



        /// <summary>
        /// Returns html direct parent element.
        /// </summary>
        public new IElementWrapperFluentApi ParentElement => (IElementWrapperFluentApi)base.ParentElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.ElementWrapper"/> class.
        /// </summary>
        /// <param name="webElementSelector">The selector that gets web element.</param>
        /// <param name="browserWrapper">The browser wrapper.</param>
        /// <param name="selector">The selector.</param>
        public ElementWrapperFluentApi(Func<IWebElement> webElementSelector, IBrowserWrapper browserWrapper)
            : base(webElementSelector, browserWrapper)
        {
        }



        /// <summary>
        /// Sends enter key to browser.
        /// </summary>
        /// <returns></returns>
        public new IElementWrapperFluentApi SendEnterKey()
        {
            return (IElementWrapperFluentApi)base.SendEnterKey();
        }


        /// <summary>
        /// Checks the name of the tag.
        /// </summary>
        /// <param name="expectedTagName">Expected name of the tag.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns></returns>
        /// <exception cref="UnexpectedElementStateException"></exception>
        public virtual IElementWrapperFluentApi CheckTagName(string expectedTagName, string failureMessage = null)
        {
            return (IElementWrapperFluentApi)EvaluateElementCheck<UnexpectedElementStateException>(new TagNameValidator(expectedTagName,
                failureMessage));
        }

        /// <summary>
        /// Checks the name of tag.
        /// </summary>
        /// <param name="expectedTagNames">The expected tag names.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns></returns>
        /// <exception cref="UnexpectedElementStateException"></exception>
        public virtual IElementWrapperFluentApi CheckIfTagName(string[] expectedTagNames, string failureMessage = null)
        {
            return (IElementWrapperFluentApi)EvaluateElementCheck<UnexpectedElementStateException>(new TagNameValidator(expectedTagNames,
                failureMessage));
        }

        /// <summary>
        /// Checks the name of tag.
        /// </summary>
        /// <param name="expectedTagName">Expected name of the tag.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns></returns>
        public virtual IElementWrapperFluentApi CheckIfTagName(string expectedTagName, string failureMessage = null)
        {
            return CheckTagName(expectedTagName, failureMessage);
        }




        /// <summary>
        /// Checks if this element contains other element(s) selected by <see cref="cssSelector"/>.
        /// </summary>
        /// <param name="cssSelector">The CSS selector.</param>
        /// <param name="tmpSelectMethod">The temporary select method.</param>
        /// <returns></returns>
        /// <exception cref="EmptySequenceException"></exception>
        public virtual IElementWrapperFluentApi CheckIfContainsElement(string cssSelector, Func<string, By> tmpSelectMethod = null)
        {
            return EvaluateElementCheck<EmptySequenceException>(
                new ContainsElementValidator(cssSelector, tmpSelectMethod));
        }

        public virtual IElementWrapperFluentApi CheckIfNotContainsElement(string cssSelector, Func<string, By> tmpSelectMethod = null)
        {
            return EvaluateElementCheck<MoreElementsInSequenceException>(
                new NotContainsElementValidator(cssSelector, tmpSelectMethod));
        }



        /// <summary>
        /// Inserts javascript to the site and returns value of innerText/textContent property of this element.
        /// </summary>
        public new virtual IElementWrapperFluentApi SetJsElementProperty(string propertyName, object propertyValue)
        {
            browser.GetJavaScriptExecutor()?.ExecuteScript(" var a = (arguments || [{}, null, null]);    var element = a[0];    var propertyName = a[1];    var propertyValue = a[2]; element[propertyName] = propertyValue;", WebElement, propertyName, propertyValue);
            return this;
        }


        public virtual IElementWrapperFluentApi CheckIfIsClickable()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsClickableValidator());
        }

        public virtual IElementWrapperFluentApi CheckIfIsNotClickable()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsNotClickableValidator());
        }



        /// <summary>
        /// This check-method inserts javascript to the site and checks returned value of innerText/textContent property of specific element.
        /// </summary>
        public virtual IElementWrapperFluentApi CheckIfJsPropertyInnerTextEquals(string text, bool caseSensitive = true, bool trim = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new JsPropertyInnerTextEqualsValidator(text, caseSensitive, trim));
        }

        /// <summary>
        /// This check-method inserts javascript to the site and checks returned value of innerText/textContent property of specific element.
        /// </summary>
        public virtual IElementWrapperFluentApi CheckIfJsPropertyInnerText(Func<string, bool> expression, string failureMesssage = null, bool trim = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new JsPropertyInnerTextValidator(s => expression(s), failureMesssage));
        }


        /// <summary>
        /// This check-method inserts javascript to the site and checks returned value of innerHTML property of specific element.
        /// </summary>
        /// <remarks>Some browsers adds unnecessary attributes to InnerHtml property. Be sure that all browsers you are using are generating the same result to prevent unexpected results of this method.</remarks>
        public virtual IElementWrapperFluentApi CheckIfJsPropertyInnerHtmlEquals(string text, bool caseSensitive = true, bool trim = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new JsPropertyInnerTextEqualsValidator(text, caseSensitive, trim));
        }

        /// <summary>
        /// This check-method inserts javascript to the site and checks returned value of innerHTML property of specific element.
        /// </summary>
        /// <remarks>Some browsers adds unnecessary attributes to InnerHtml property. Be sure that all browsers you are using are generate the same result to prevent unexpected results of this method.</remarks>

        public virtual IElementWrapperFluentApi CheckIfJsPropertyInnerHtml(Func<string, bool> expression, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new JsPropertyInnerHtmlValidator(s => expression(s), failureMessage));
        }

        /// <summary>
        /// Checks name of tag
        /// </summary>
        [Obsolete]
        public virtual IElementWrapperFluentApi CheckTagName(Func<string, bool> expression, string failureMessage = null)
        {
            return CheckIfTagName(expression, failureMessage);
        }

        /// <summary>
        /// Checks name of tag
        /// </summary>
        public virtual IElementWrapperFluentApi CheckIfTagName(Func<string, bool> expression, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new TagNameValidator(s => expression(s),
                failureMessage));
        }

        /// <param name="attributeName">write name of attribute to check</param>
        /// <param name="expression">define condition</param>
        /// <param name="failureMessage">When value of the element does not satisfy the condition this fail failureMessage is written to the throwen exception in the output.</param>
        /// <returns></returns>
        public virtual IElementWrapperFluentApi CheckAttribute(string attributeName, Func<string, bool> expression, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new AttributeValidator(attributeName, s => expression(s), failureMessage));
        }

        public virtual IElementWrapperFluentApi CheckAttribute(string attributeName, string value, bool caseInsensitive = false, bool trimValue = true, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new AttributeValuesValidator(attributeName, value,
                caseInsensitive, trimValue, failureMessage));
        }

        public virtual IElementWrapperFluentApi CheckAttribute(string attributeName, string[] allowedValues, bool caseInsensitive = false, bool trimValue = true, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new AttributeValuesValidator(attributeName, allowedValues,
                caseInsensitive, trimValue, failureMessage));
        }

        public virtual IElementWrapperFluentApi CheckCssStyle(string styleName, string value, bool caseSensitive = false, bool trimValue = true, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new CssStyleValidator(styleName, value,
                caseSensitive, trimValue, failureMessage));
        }

        public virtual IElementWrapperFluentApi CheckClassAttribute(Func<string, bool> expression, string failureMessage = null)
        {
            return CheckAttribute("class", expression, failureMessage);
        }
        public IElementWrapperFluentApi CheckClassAttribute(string value, string failureMessage = null, bool caseInsensitive = true,
            bool trimValue = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new ClassAttributeValidator(value, !caseInsensitive, trimValue, failureMessage));
        }

        public virtual IElementWrapperFluentApi CheckClassAttribute(string value, bool caseInsensitive = true, bool trimValue = true, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new ClassAttributeValidator(value, !caseInsensitive, trimValue, failureMessage));
        }
        public virtual IElementWrapperFluentApi CheckClassAttribute(string[] value, string failureMessage = null, bool caseInsensitive = true, bool trimValue = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new ClassAttributeValidator(value, !caseInsensitive, trimValue, failureMessage));
        }

        public virtual IElementWrapperFluentApi CheckIfHasClass(string value, bool caseInsensitive = true)
        {
            return CheckAttribute("class", p => p.Split(' ').Any(c => string.Equals(c, value, caseInsensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)), $"Expected value: '{value}'.");
        }

        public virtual IElementWrapperFluentApi CheckIfHasNotClass(string value, bool caseInsensitive = true)
        {
            return CheckAttribute("class", p => !p.Split(' ').Select(s => s.Trim()).Any(c => string.Equals(c, value, caseInsensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)), $"Expected value should not contain: '{value}'.");
        }

        public IElementWrapperFluentApi CheckIfHasAttribute(string name)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new HasAttributeValidator(name));
        }

        public IElementWrapperFluentApi CheckIfHasNotAttribute(string name)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new HasNotAttributeValidator(name));
        }

        public virtual IElementWrapperFluentApi CheckIfInnerTextEquals(string text, bool caseSensitive = true, bool trim = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new InnerTextEqualsValidator(text, caseSensitive, trim));
        }

        public virtual IElementWrapperFluentApi CheckIfInnerText(Func<string, bool> expression, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new InnerTextValidator(s => expression(s), failureMessage));
        }

        public virtual IElementWrapperFluentApi CheckIfTextEquals(string text, bool caseSensitive = true, bool trim = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new TextEqualsValidator(text, caseSensitive, trim));
        }


        public virtual IElementWrapperFluentApi CheckIfTextNotEquals(string text, bool caseSensitive = true, bool trim = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new TextNotEqualsValidator(text, caseSensitive, trim));
        }

        public virtual IElementWrapperFluentApi CheckIfText(Func<string, bool> expression, string failureMessage = null)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new TextValidator(s => expression(s),
                failureMessage));
        }

        /// <summary>
        /// Sets current option of element &lt;select&gt;.
        /// </summary>
        /// <param name="index">Index of  &lt;option&gt; that should be selected.</param>
        public new virtual IElementWrapperFluentApi Select(int index)
        {
            return Select(e => e.SelectByIndex(index));
        }

        /// <summary>
        /// Sets current option of element &lt;select&gt;.
        /// </summary>
        /// <param name="value">Value of  &lt;option&gt; that should be selected.</param>
        public new virtual IElementWrapperFluentApi Select(string value)
        {
            return Select(e => e.SelectByValue(value));
        }

        /// <summary>
        /// Sets current option of element &lt;select&gt;.
        /// </summary>
        public new virtual IElementWrapperFluentApi Select(Action<SelectElement> process)
        {
            return PerformActionOnSelectElement(process);
        }

        public new virtual IElementWrapperFluentApi PerformActionOnSelectElement(Action<SelectElement> process)
        {
            return (IElementWrapperFluentApi)base.PerformActionOnSelectElement(process);
        }



        /// <summary>
        /// Submits this element to the web server.
        /// </summary>
        /// <remarks>
        /// If this current element is a form, or an element within a form,
        ///             then this will be submitted to the web server. If this causes the current
        ///             page to change, then this method will block until the new page is loaded.
        /// </remarks>

        public new virtual IElementWrapperFluentApi Submit()
        {
            WebElement.Submit();
            return this;
        }

        public new virtual IElementWrapperFluentApi SendKeys(string text)
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
        public new virtual IElementWrapperCollection<IElementWrapperFluentApi, IBrowserWrapperFluentApi> FindElements(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var usedSelectMethod = (tmpSelectMethod ?? SelectMethod);
            var collection = Extensions.ToElementsList<IElementWrapperFluentApi, IBrowserWrapperFluentApi>(
                () => WebElement.FindElements(usedSelectMethod(selector)),
                (IBrowserWrapperFluentApi)browser, selector, usedSelectMethod, this, browser.ServiceFactory);
            collection.ParentWrapper = this;
            return collection;
        }

        /// <summary>
        /// Returns first element that satisfies the selector.
        /// </summary>
        public new virtual IElementWrapperFluentApi FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var elms = FindElements(selector, tmpSelectMethod);
            return (IElementWrapperFluentApi)elms.FirstOrDefault();
        }

        /// <summary>
        /// Returns first element that satisfies the selector. Throws exception when no element is found.
        /// </summary>
        public new virtual IElementWrapperFluentApi First(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return ThrowIfIsNull(FirstOrDefault(selector, tmpSelectMethod), $"Element not found. Selector: {selector}");
        }

        /// <summary>
        /// Returns one element and throws exception when more then one element is found.
        /// </summary>
        public new virtual IElementWrapperFluentApi SingleOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return (IElementWrapperFluentApi)FindElements(selector, tmpSelectMethod).SingleOrDefault();
        }

        /// <summary>
        /// Returns one element and throws exception when no element or more then one element is found.
        /// </summary>
        public new virtual IElementWrapperFluentApi Single(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return (IElementWrapperFluentApi)FindElements(selector, tmpSelectMethod).Single();
        }

        /// <summary>
        /// Returns one element from collection at specified position and throws exception when collection contains less elements then is expected.
        /// </summary>
        public new virtual IElementWrapperFluentApi ElementAt(string selector, int index, Func<string, By> tmpSelectMethod = null)
        {
            return (IElementWrapperFluentApi)FindElements(selector, tmpSelectMethod).ElementAt(index);
        }

        /// <summary>
        /// Returns last element from collection and Throws exception when the collection is empty.
        /// </summary>
        public new virtual IElementWrapperFluentApi Last(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return (IElementWrapperFluentApi)FindElements(selector, tmpSelectMethod).Last();
        }

        /// <summary>
        /// Returns last element from collection or null when collection is empty.
        /// </summary>
        public new virtual IElementWrapperFluentApi LastOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return (IElementWrapperFluentApi)FindElements(selector, tmpSelectMethod).LastOrDefault();
        }


        public new virtual IElementWrapperFluentApi Click()
        {
            return (IElementWrapperFluentApi)base.Click();
        }

        public virtual IElementWrapperFluentApi CheckIfIsDisplayed()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsDisplayedValidator());
        }

        public virtual IElementWrapperFluentApi CheckIfIsNotDisplayed()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsNotDisplayedValidator());
        }

        public virtual IElementWrapperFluentApi CheckIfIsChecked()
        {
            CheckTagName("input", "Function IsNotCheckedValidator() can be used on input element only.");
            CheckAttribute("type", new[] { "checkbox", "radio" }, failureMessage: "Input element must be type of checkbox.");

            return EvaluateElementCheck<UnexpectedElementStateException>(new IsCheckedValidator());
        }

        public virtual IElementWrapperFluentApi CheckIfIsNotChecked()
        {
            CheckTagName("input", "Function IsNotCheckedValidator() can be used on input element only.");
            CheckAttribute("type", new[] { "checkbox", "radio" }, failureMessage: "Input element must be type of checkbox or radio.");

            return EvaluateElementCheck<UnexpectedElementStateException>(new IsNotCheckedValidator());
        }



        public virtual IElementWrapperFluentApi CheckIfValue(string value, bool caseInsensitive = false, bool trimValue = true)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new ValueValidator(value, !caseInsensitive,
                trimValue));
        }


        public virtual IElementWrapperFluentApi CheckIfIsEnabled()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsEnabledValidator());
        }

        public IElementWrapperFluentApi CheckIfIsSelected()
        {
            if (!IsSelected())
            {
                throw new UnexpectedElementStateException($"Element is not selected. \r\n Element selector: {Selector} \r\n");
            }
            return this;
        }



        public virtual IElementWrapperFluentApi CheckIfContainsText()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new ContainsTextValidator());
        }


        public virtual IElementWrapperFluentApi CheckIfIsNotEnabled()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsNotEnabledValidator());
        }

        public IElementWrapperFluentApi CheckIfIsNotSelected()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsNotSelectedValidator());
        }

        /// <summary>
        /// Checks whether element contains some text.
        /// </summary>
        public virtual IElementWrapperFluentApi CheckIfTextEmpty()
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new TextEmptyValidator());
        }



        /// <summary>
        /// Removes content of element.
        /// </summary>
        public new IElementWrapperFluentApi Clear()
        {
            return (IElementWrapperFluentApi)base.Clear();
        }

        /// <summary>
        /// Waits the ActionWaitTime before next step.
        /// </summary>
        [Obsolete("Please use WaitFor or specify exact timeout.")]
        public new virtual IElementWrapperFluentApi Wait()
        {
            return (IElementWrapperFluentApi)base.Wait();
        }

        /// <summary>
        /// Waits the specified time before next step.
        /// </summary>
        public new virtual IElementWrapperFluentApi Wait(int milliseconds)
        {
            return (IElementWrapperFluentApi)base.Wait(milliseconds);
        }

        /// <summary>
        /// Waits until the condition is true.
        /// </summary>
        /// <param name="condition">Expression that determine whether test should wait or continue</param>
        /// <param name="maxTimeout">If condition is not reached in this timeout (ms) test is dropped.</param>
        /// <param name="failureMessage">Message which is displayed in exception log in case that the condition is not reached</param>
        /// <param name="ignoreCertainException">When StaleElementReferenceException or InvalidElementStateException is thrown than it would be ignored.</param>
        /// <param name="checkInterval">Interval in milliseconds. RECOMMENDATION: let the interval greater than 250ms</param>
        IElementWrapperFluentApi IElementWrapperFluentApi.WaitFor(Func<IElementWrapperFluentApi, bool> condition, int maxTimeout, string failureMessage, bool ignoreCertainException = true, int checkInterval = 30)
        {
            return (IElementWrapperFluentApi)base.WaitFor((e) => condition((IElementWrapperFluentApi)e), maxTimeout, failureMessage, ignoreCertainException, checkInterval);
        }

        /// <inheritdoc />
        public new IElementWrapperCollection<IElementWrapperFluentApi, IBrowserWrapperFluentApi> Children => base.Children.Convert<IElementWrapperFluentApi, IBrowserWrapperFluentApi>();

        IElementWrapperFluentApi IElementWrapperFluentApi.WaitFor(Action<IElementWrapperFluentApi> checkExpression, int maxTimeout, string failureMessage, int checkInterval = 30)
        {
            return (IElementWrapperFluentApi)base.WaitFor((e) => checkExpression((IElementWrapperFluentApi)e), maxTimeout, failureMessage, checkInterval);
        }

        /// <summary>
        /// Waits the specified time before next step.
        /// </summary>
        public new virtual IElementWrapperFluentApi Wait(TimeSpan interval)
        {
            return (IElementWrapperFluentApi)base.Wait(interval);
        }

        /// <summary>
        /// Checks href value of element A (hyperlink)
        /// </summary>
        /// <param name="url">Expected value of href.</param>
        /// <param name="kind">Type of url of expected href.</param>
        /// <param name="components">Determines what parts of url should be compared.</param>
        /// <returns></returns>
        public IElementWrapperFluentApi CheckIfHyperLinkEquals(string url, UrlKind kind, params UriComponents[] components)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(
                new HyperLinkEqualsValidator(url, kind, true, components));
        }

        /// <inheritdoc cref="IElementWrapper.ScrollTo()" />
        public new IElementWrapperFluentApi ScrollTo(WaitForOptions waitForOptions = null)
        {
            return (IElementWrapperFluentApi)base.ScrollTo(waitForOptions);
        }

        public IElementWrapperFluentApi CheckIfIsElementInView(IElementWrapperFluentApi element)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsElementInViewValidator(element));
        }

        public IElementWrapperFluentApi CheckIfIsElementNotInView(IElementWrapperFluentApi element)
        {
            return EvaluateElementCheck<UnexpectedElementStateException>(new IsElementNotInViewValidator(element));
        }


        /// <summary>
        /// Drag this element and drop to dropToElement with offsetX and offsetY.
        /// </summary>
        /// <param name="dropToElement"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        public IElementWrapperFluentApi DropTo(IElementWrapperFluentApi dropToElement, int offsetX = 0, int offsetY = 0)
        {
            return (IElementWrapperFluentApi)base.DropTo(dropToElement, offsetX, offsetY);
        }


        protected IElementWrapperFluentApi EvaluateElementCheck<TException>(IValidator<IElementWrapperFluentApi> validator)
            where TException : TestExceptionBase, new()
        {
            var operationResult = validator.Validate(this);
            operationResultValidator.Validate<TException>(operationResult);
            return this;
        }
    }
}
