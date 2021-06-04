using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Riganti.Selenium.Core.Abstractions;

namespace Riganti.Selenium.Core
{
    public interface IElementWrapperFluentApi : IElementWrapper
    {
        new IElementWrapperFluentApi ParentElement { get; }
        new IBrowserWrapperFluentApi BrowserWrapper { get; }

        IElementWrapperFluentApi CheckAttribute(string attributeName, Func<string, bool> expression, string failureMessage = null);
        IElementWrapperFluentApi CheckAttribute(string attributeName, string value, bool caseInsensitive = false, bool trimValue = true, string failureMessage = null);
        IElementWrapperFluentApi CheckAttribute(string attributeName, string[] allowedValues, bool caseInsensitive = false, bool trimValue = true, string failureMessage = null);
        IElementWrapperFluentApi CheckCssStyle(string styleName, string value, bool caseSensitive = false, bool trimValue = true, string failureMessage = null);
        IElementWrapperFluentApi CheckClassAttribute(Func<string, bool> expression, string failureMessage = null);
        IElementWrapperFluentApi CheckClassAttribute(string[] allowedValues, string failureMessage = null, bool caseInsensitive = true, bool trimValue = true);
        IElementWrapperFluentApi CheckClassAttribute(string value, string failureMessage = null, bool caseInsensitive = true, bool trimValue = true);
        IElementWrapperFluentApi CheckIfContainsElement(string cssSelector, Func<string, By> tmpSelectMethod = null);
        IElementWrapperFluentApi CheckIfContainsText();
        IElementWrapperFluentApi CheckIfTextEmpty();
        IElementWrapperFluentApi CheckIfHasAttribute(string name);
        IElementWrapperFluentApi CheckIfHasClass(string value, bool caseInsensitive = true);
        IElementWrapperFluentApi CheckIfHasNotAttribute(string name);
        IElementWrapperFluentApi CheckIfHasNotClass(string value, bool caseInsensitive = true);
        IElementWrapperFluentApi CheckIfHyperLinkEquals(string url, UrlKind kind, params UriComponents[] components);
        IElementWrapperFluentApi CheckIfInnerText(Func<string, bool> expression, string failureMessage = null);
        IElementWrapperFluentApi CheckIfInnerTextEquals(string text, bool caseSensitive = true, bool trim = true);
        IElementWrapperFluentApi CheckIfIsChecked();
        IElementWrapperFluentApi CheckIfIsClickable();
        IElementWrapperFluentApi CheckIfIsDisplayed();
        IElementWrapperFluentApi CheckIfIsElementInView(IElementWrapperFluentApi element);
        IElementWrapperFluentApi CheckIfIsElementNotInView(IElementWrapperFluentApi element);
        IElementWrapperFluentApi CheckIfIsEnabled();
        IElementWrapperFluentApi CheckIfIsNotChecked();
        IElementWrapperFluentApi CheckIfIsNotClickable();
        IElementWrapperFluentApi CheckIfIsNotDisplayed();
        IElementWrapperFluentApi CheckIfIsNotEnabled();
        IElementWrapperFluentApi CheckIfIsNotSelected();
        IElementWrapperFluentApi CheckIfIsSelected();
        IElementWrapperFluentApi CheckIfJsPropertyInnerHtml(Func<string, bool> expression, string failureMessage = null);
        IElementWrapperFluentApi CheckIfJsPropertyInnerHtmlEquals(string text, bool caseSensitive = true, bool trim = true);
        IElementWrapperFluentApi CheckIfJsPropertyInnerText(Func<string, bool> expression, string failureMesssage = null, bool trim = true);
        IElementWrapperFluentApi CheckIfJsPropertyInnerTextEquals(string text, bool caseSensitive = true, bool trim = true);
        IElementWrapperFluentApi CheckIfNotContainsElement(string cssSelector, Func<string, By> tmpSelectMethod = null);
        IElementWrapperFluentApi CheckIfTagName(Func<string, bool> expression, string failureMessage = null);
        IElementWrapperFluentApi CheckIfTagName(string expectedTagName, string failureMessage = null);
        IElementWrapperFluentApi CheckIfTagName(string[] expectedTagNames, string failureMessage = null);
        IElementWrapperFluentApi CheckIfText(Func<string, bool> expression, string failureMessage = null);
        IElementWrapperFluentApi CheckIfTextEquals(string text, bool caseSensitive = true, bool trim = true);
        IElementWrapperFluentApi CheckIfTextNotEquals(string text, bool caseSensitive = true, bool trim = true);
        IElementWrapperFluentApi CheckIfValue(string value, bool caseInsensitive = false, bool trimValue = true);
        IElementWrapperFluentApi CheckTagName(Func<string, bool> expression, string failureMessage = null);
        IElementWrapperFluentApi CheckTagName(string expectedTagName, string failureMessage = null);
        new IElementWrapperFluentApi Clear();
        new IElementWrapperFluentApi Click();
        IElementWrapperFluentApi DropTo(IElementWrapperFluentApi dropToElement, int offsetX = 0, int offsetY = 0);
        new IElementWrapperFluentApi ElementAt(string selector, int index, Func<string, By> tmpSelectMethod = null);
        new IElementWrapperFluentApi First(string selector, Func<string, By> tmpSelectMethod = null);
        new IElementWrapperFluentApi FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null);

        new IElementWrapperFluentApi Last(string selector, Func<string, By> tmpSelectMethod = null);
        new IElementWrapperFluentApi LastOrDefault(string selector, Func<string, By> tmpSelectMethod = null);
        new IElementWrapperFluentApi PerformActionOnSelectElement(Action<SelectElement> process);
        new IElementWrapperFluentApi ScrollTo(WaitForOptions waitForOptions = null);
        new IElementWrapperFluentApi Select(Action<SelectElement> process);
        new IElementWrapperFluentApi Select(int index);
        new IElementWrapperFluentApi Select(string value);
        new IElementWrapperFluentApi SendEnterKey();
        new IElementWrapperFluentApi SendKeys(string text);
        new IElementWrapperFluentApi SetJsElementProperty(string propertyName, object propertyValue);
        new IElementWrapperFluentApi Single(string selector, Func<string, By> tmpSelectMethod = null);
        new IElementWrapperFluentApi SingleOrDefault(string selector, Func<string, By> tmpSelectMethod = null);
        new IElementWrapperFluentApi Submit();
        new IElementWrapperFluentApi Wait();
        new IElementWrapperFluentApi Wait(int milliseconds);
        new IElementWrapperFluentApi Wait(TimeSpan interval);
        IElementWrapperFluentApi WaitFor(Action<IElementWrapperFluentApi> checkExpression, int maxTimeout, string failureMessage, int checkInterval = 30);
        IElementWrapperFluentApi WaitFor(Func<IElementWrapperFluentApi, bool> condition, int maxTimeout, string failureMessage, bool ignoreCertainException = true, int checkInterval = 30);

        new IElementWrapperCollection<IElementWrapperFluentApi, IBrowserWrapperFluentApi> Children { get; }
        new IElementWrapperCollection<IElementWrapperFluentApi, IBrowserWrapperFluentApi> FindElements(string selector, Func<string, By> tmpSelectMethod = null);
    }
}