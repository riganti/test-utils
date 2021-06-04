using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OpenQA.Selenium;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;
using Riganti.Selenium.Core.Api;
using Riganti.Selenium.Validators.Checkers;
using Riganti.Selenium.Validators.Checkers.BrowserWrapperCheckers;
using Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers;
using Riganti.Selenium.AssertApi;
using Riganti.Selenium.Core;
using OpenQA.Selenium.Remote;

namespace Riganti.Selenium.Core
{
    public static class AssertUI
    {
        private static readonly OperationResultValidator OperationResultValidator = new OperationResultValidator();

        public static void InnerText(IElementWrapper wrapper, Expression<Func<string, bool>> rule, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var innerText = new InnerTextValidator(rule, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, innerText);
            }, waitForOptions);
        }

        public static void InnerTextEquals(IElementWrapper wrapper, string text, bool caseSensitive = false, bool trim = true, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var innerTextEquals = new InnerTextEqualsValidator(text, caseSensitive, trim);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, innerTextEquals);
            }, waitForOptions);
        }

        public static void Text(IElementWrapper wrapper, Expression<Func<string, bool>> rule, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var text = new TextValidator(rule, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, text);
            }, waitForOptions);
        }

        public static void TextEquals(IElementWrapper wrapper, string text, bool caseSensitive = false, bool trim = true, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var textEquals = new TextEqualsValidator(text, caseSensitive, trim);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, textEquals);
            }, waitForOptions);
        }
        public static void TextNotEquals(IElementWrapper wrapper, string text, bool caseSensitive = false, bool trim = true, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var textNotEquals = new TextNotEqualsValidator(text, caseSensitive, trim);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, textNotEquals);
            }, waitForOptions);
        }

        public static void IsDisplayed(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isDisplayed = new IsDisplayedValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isDisplayed);
            }, waitForOptions);
        }


        public static void IsNotDisplayed(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isNotDisplayed = new IsNotDisplayedValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isNotDisplayed);
            }, waitForOptions);
        }

        public static void IsChecked(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                TagName(wrapper, "input", "Function IsNotChecked() can be used on input element only.");
                Attribute(wrapper, "type", new[] { "checkbox", "radio" }, failureMessage: "Input element must be type of checkbox.");

                var isChecked = new IsCheckedValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isChecked);
            }, waitForOptions);
        }

        public static void IsNotChecked(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            TagName(wrapper, "input", "Function IsNotChecked() can be used on input element only.");
            Attribute(wrapper, "type", new[] { "checkbox", "radio" }, failureMessage: "Input element must be type of checkbox or radio.");

            var isNotChecked = new IsNotCheckedValidator();
            EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isNotChecked);
        }

        public static void IsSelected(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isSelected = new IsSelectedValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isSelected);
            }, waitForOptions);
        }

        public static void IsNotSelected(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isNotSelected = new IsNotSelectedValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isNotSelected);
            }, waitForOptions);
        }

        public static void IsEnabled(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isEnabled = new IsEnabledValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isEnabled);
            }, waitForOptions);
        }
        public static void IsClickable(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isClickable = new IsClickableValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isClickable);
            }, waitForOptions);
        }

        public static void IsNotClickable(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isNotClickable = new IsNotClickableValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isNotClickable);
            }, waitForOptions);
        }

        public static void IsNotEnabled(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isNotEnabled = new IsNotEnabledValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isNotEnabled);
            }, waitForOptions);
        }

        public static void Value(IElementWrapper wrapper, string value, bool caseSensitive = false, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var Value = new ValueValidator(value, caseSensitive, trim);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, Value);
            }, waitForOptions);
        }
        public static void ContainsText(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var containsText = new ContainsTextValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, containsText);
            }, waitForOptions);
        }
        public static void TextEmpty(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var doesNotContainsText = new TextEmptyValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, doesNotContainsText);
            }, waitForOptions);
        }
        public static void TextNotEmpty(IElementWrapper wrapper, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var doesNotContainsText = new TextNotEmptyValidator();
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, doesNotContainsText);
            }, waitForOptions);
        }

        public static void HyperLinkEquals(IElementWrapper wrapper, string url, UrlKind kind, params UriComponents[] components)
        {
            var hyperLinkEquals = new HyperLinkEqualsValidator(url, kind, true, components);
            EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, hyperLinkEquals);
        }
        public static void HyperLinkEquals(IElementWrapper wrapper, string url, UrlKind kind, bool strictQueryStringParamsOrder, params UriComponents[] components)
        {
            var hyperLinkEquals = new HyperLinkEqualsValidator(url, kind, strictQueryStringParamsOrder, components);
            EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, hyperLinkEquals);
        }
        public static void HyperLinkEquals(IBrowserWrapper wrapper, string selector, string url, UrlKind kind, params UriComponents[] components)
        {
            var elements = wrapper.FindElements(selector);
            var operationRunner = All(elements);
            var hyperLinkEquals = new HyperLinkEqualsValidator(url, kind, true, components);
            operationRunner.Evaluate<UnexpectedElementStateException>(hyperLinkEquals);
        }
        public static void IsElementInView(IElementWrapper wrapper, IElementWrapper element, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isElementInView = new IsElementInViewValidator(element);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isElementInView);
            }, waitForOptions);
        }

        public static void IsElementNotInView(IElementWrapper wrapper, ElementWrapper element, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var isElementNotInView = new IsElementNotInViewValidator(element);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, isElementNotInView);
            }, waitForOptions);
        }



        public static void TagName(IElementWrapper wrapper, string expectedTagName, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var checkTagName = new TagNameValidator(expectedTagName, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, checkTagName);
            }, waitForOptions);
        }

        public static void TagName(IElementWrapper wrapper, string[] expectedTagNames, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var checkTagName = new TagNameValidator(expectedTagNames, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, checkTagName);
            }, waitForOptions);
        }



        public static void TagName(IElementWrapper wrapper, Expression<Func<string, bool>> rule, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var tagName = new TagNameValidator(rule, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, tagName);
            }, waitForOptions);
        }


        public static void ContainsElement(IElementWrapper wrapper, string cssSelector, Func<string, By> tmpSelectMethod = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var containsElement = new ContainsElementValidator(cssSelector, tmpSelectMethod);
                EvaluateValidator<EmptySequenceException, IElementWrapper>(wrapper, containsElement);
            }, waitForOptions);
        }

        public static void NotContainsElement(IElementWrapper wrapper, string cssSelector, Func<string, By> tmpSelectMethod = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var notContainsElement = new NotContainsElementValidator(cssSelector, tmpSelectMethod);
                EvaluateValidator<MoreElementsInSequenceException, IElementWrapper>(wrapper, notContainsElement);
            }, waitForOptions);
        }

        public static void JsPropertyInnerText(IElementWrapper wrapper, Expression<Func<string, bool>> rule, string failureMesssage = null, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var jsPropertyInnerText = new JsPropertyInnerTextValidator(rule, failureMesssage, trim);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, jsPropertyInnerText);
            }, waitForOptions);
        }

        public static void JsPropertyInnerTextEquals(IElementWrapper wrapper, string text, bool caseSensitive = false, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var jsPropertyInnerTextEquals = new JsPropertyInnerTextEqualsValidator(text, caseSensitive, trim);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, jsPropertyInnerTextEquals);
            }, waitForOptions);
        }

        public static void JsPropertyInnerHtmlEquals(IElementWrapper wrapper, string text, bool caseSensitive = false, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var jsPropertyInnerHtmlEquals = new JsPropertyInnerHtmlEqualsValidator(text, caseSensitive, trim);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, jsPropertyInnerHtmlEquals);
            }, waitForOptions);
        }

        public static void JsPropertyInnerHtml(IElementWrapper wrapper, Expression<Func<string, bool>> expression, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var jsPropertyInnerHtml = new JsPropertyInnerHtmlValidator(expression, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, jsPropertyInnerHtml);
            }, waitForOptions);
        }

        public static void Attribute(IElementWrapper wrapper, string attributeName, string value, bool caseSensitive = false, bool trimValue = true, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var checkAttribute = new AttributeValuesValidator(attributeName, value, caseSensitive, trimValue, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, checkAttribute);
            }, waitForOptions);
        }

        public static void Attribute(IElementWrapper wrapper, string attributeName, string[] allowedValues, bool caseSensitive = false, bool trimValue = true, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var checkAttribute = new AttributeValuesValidator(attributeName, allowedValues, caseSensitive, trimValue, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, checkAttribute);
            }, waitForOptions);
        }

        public static void Attribute(IElementWrapper wrapper, string attributeName, Expression<Func<string, bool>> expression, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var attribute = new AttributeValidator(attributeName, expression, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, attribute);
            }, waitForOptions);
        }
        public static void ClassAttribute(IElementWrapper wrapper, string value, bool caseSensitive = true, bool trimValue = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                Attribute(wrapper, "class", value, caseSensitive, trimValue);
            }, waitForOptions);
        }

        public static void ClassAttribute(IElementWrapper wrapper, Expression<Func<string, bool>> expression, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                Attribute(wrapper, "class", expression, failureMessage);
            }, waitForOptions);
        }

        public static void HasClass(IElementWrapper wrapper, string value, bool caseSensitive = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var values = value.Split(' ');
                Attribute(wrapper, "class", p => values.All(v => p
#if NET5_0_OR_GREATER
                .Split(' ', StringSplitOptions.None)
#else
                .Split(' ')
#endif

                .Any(c => string.Equals(c, v, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))), $"Expected value: '{value}'.");
            }, waitForOptions);
        }
        public static void HasNotClass(IElementWrapper wrapper, string value, bool caseSensitive = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                Attribute(wrapper, "class", p => !p
#if NET5_0_OR_GREATER
                .Split(' ', StringSplitOptions.None)
#else
                .Split(' ')
#endif
                .Any(c => string.Equals(c, value,
        caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)), $"Expected value: '{value}'.");
            }, waitForOptions);
        }

        public static void HasAttribute(IElementWrapper wrapper, string name, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var hasAttribute = new HasAttributeValidator(name);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, hasAttribute);
            }, waitForOptions);
        }

        public static void HasNotAttribute(IElementWrapper wrapper, string name, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var hasNotAttribute = new HasNotAttributeValidator(name);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, hasNotAttribute);
            }, waitForOptions);
        }

        public static void CssStyle(IElementWrapper wrapper, string styleName, string value, bool caseSensitive = false, bool trimValue = true, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var checkCssStyle = new CssStyleValidator(styleName, value, caseSensitive, trimValue, failureMessage);
                EvaluateValidator<UnexpectedElementStateException, IElementWrapper>(wrapper, checkCssStyle);
            }, waitForOptions);
        }

        public static void AlertTextEquals(IBrowserWrapper wrapper, string expectedValue,
                    bool caseSensitive = false, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var alertTextEquals = new AlertTextEqualsValidator(expectedValue, caseSensitive, trim);
                EvaluateValidator<AlertException, IBrowserWrapper>(wrapper, alertTextEquals);
            }, waitForOptions);
        }

        public static void AlertText(IBrowserWrapper wrapper, Expression<Func<string, bool>> expression, string failureMessage = "", WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var alertText = new AlertTextValidator(expression, failureMessage);
                EvaluateValidator<AlertException, IBrowserWrapper>(wrapper, alertText);
            }, waitForOptions);
        }

        public static void AlertTextContains(IBrowserWrapper wrapper, string expectedValue, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var alertTextContains = new AlertTextContainsValidator(expectedValue, trim);
                EvaluateValidator<AlertException, IBrowserWrapper>(wrapper, alertTextContains);
            }, waitForOptions);
        }

        public static void Url(IBrowserWrapper wrapper, Expression<Func<string, bool>> expression, string failureMessage = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var url = new CurrentUrlValidator(expression, failureMessage);
                EvaluateValidator<BrowserLocationException, IBrowserWrapper>(wrapper, url);
            }, waitForOptions);
        }

        public static void Url(IBrowserWrapper wrapper, string url, UrlKind urlKind, params UriComponents[] components)
        {
            var checkUrl = new UrlValidator(url, urlKind, components);
            EvaluateValidator<BrowserLocationException, IBrowserWrapper>(wrapper, checkUrl);
        }

        public static void UrlEquals(IBrowserWrapper wrapper, string url, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var checkUrlExquals = new UrlEqualsValidator(url);
                EvaluateValidator<BrowserLocationException, IBrowserWrapper>(wrapper, checkUrlExquals);
            }, waitForOptions);
        }

        public static void IsDisplayed(IBrowserWrapper wrapper, string selector, Func<string, By> tmpSelectedMethod = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var elements = wrapper.FindElements(selector, tmpSelectedMethod);
                var operationRunner = All(elements);
                var isDisplayedValidator = new IsDisplayedValidator();
                operationRunner.Evaluate<UnexpectedElementStateException>(isDisplayedValidator);
            }, waitForOptions);
        }
        public static void IsNotDisplayed(IBrowserWrapper wrapper, string selector, Func<string, By> tmpSelectedMethod = null, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var elements = wrapper.FindElements(selector, tmpSelectedMethod);
                var operationRunner = All(elements);
                var isNotDisplayedValidator = new IsNotDisplayedValidator();
                operationRunner.Evaluate<BrowserException>(isNotDisplayedValidator);
            }, waitForOptions);
        }

        public static void UrlIsAccessible(IBrowserWrapper wrapper, string url, UrlKind urlKind, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var urlIsAccessible = new UrlIsAccessibleValidator(url, urlKind);
                EvaluateValidator<BrowserException, IBrowserWrapper>(wrapper, urlIsAccessible);
            }, waitForOptions);
        }

        public static void TitleEquals(IBrowserWrapper wrapper, string title, bool caseSensitive = false, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var titleEquals = new TitleEqualsValidator(title, caseSensitive, trim);
                EvaluateValidator<BrowserException, IBrowserWrapper>(wrapper, titleEquals);
            }, waitForOptions);
        }

        public static void TitleNotEquals(IBrowserWrapper wrapper, string title, bool caseSensitive = false, bool trim = true, WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var titleNotEquals = new TitleNotEqualsValidator(title, caseSensitive, trim);
                EvaluateValidator<BrowserException, IBrowserWrapper>(wrapper, titleNotEquals);
            }, waitForOptions);
        }

        public static void Title(IBrowserWrapper wrapper, Expression<Func<string, bool>> expression, string failureMessage = "", WaitForOptions waitForOptions = null)
        {
            WaitForExecutor.WaitFor(() =>
            {
                var title = new TitleValidator(expression, failureMessage);
                EvaluateValidator<BrowserException, IBrowserWrapper>(wrapper, title);
            }, waitForOptions);
        }

        public static void Check<TException, T>(IValidator<T> validator, T wrapper, WaitForOptions waitForOptions = null)
                    where TException : TestExceptionBase, new() where T : ISupportedByValidator
        {
            WaitForExecutor.WaitFor(() =>
            {
                EvaluateValidator<TException, T>(wrapper, validator);
            }, waitForOptions);
        }

        private static void EvaluateValidator<TException, T>(T wrapper, IValidator<T> validator, WaitForOptions waitForOptions = null)
                    where TException : TestExceptionBase, new() where T : ISupportedByValidator
        {
            var operationResult = validator.Validate(wrapper);
            OperationResultValidator.Validate<TException>(operationResult);
        }

        public static AnyOperationRunner<T> Any<T>(IEnumerable<T> wrappers, WaitForOptions waitForOptions = null) where T : ISupportedByValidator
        {
            return new AnyOperationRunner<T>(wrappers);
        }

        public static AllOperationRunner<T> All<T>(IEnumerable<T> elementWrappers, WaitForOptions waitForOptions = null) where T : ISupportedByValidator
        {
            return new AllOperationRunner<T>(elementWrappers);
        }
    }
}
