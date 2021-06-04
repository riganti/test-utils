using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;
using Riganti.Selenium.Validators.Checkers;
using Riganti.Selenium.Validators.Checkers.BrowserWrapperCheckers;
using Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers;
using HyperLinkEquals = Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers.HyperLinkEqualsValidator;
using IsDisplayed = Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers.IsDisplayedValidator;
using IsNotDisplayed = Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers.IsNotDisplayedValidator;
using Riganti.Selenium.Validators;

namespace Riganti.Selenium.Core.Api
{
    public static class OperationRunnerExtensions
    {
        public static void InnerText(this IOperationRunner<IElementWrapper> operationRunner, Expression<Func<string, bool>> rule)
        {
            var innerText = new InnerTextValidator(rule);
            operationRunner.Evaluate<UnexpectedElementStateException>(innerText);
        }

        public static void Value(this IOperationRunner<IElementWrapper> operationRunner, string value, bool caseSensitive = false, bool trim = true)
        {
            var Value = new ValueValidator(value, caseSensitive, trim);
            operationRunner.Evaluate<UnexpectedElementStateException>(Value);
        }

        public static void InnerTextEquals(this IOperationRunner<IElementWrapper> operationRunner, string text, bool caseSensitive = false, bool trim = true, string failureMessage = null)
        {
            var innerTextEquals = new InnerTextEqualsValidator(text, caseSensitive, trim);
            operationRunner.Evaluate<UnexpectedElementStateException>(innerTextEquals);
        }

        public static void Text(this IOperationRunner<IElementWrapper> operationRunner, Expression<Func<string, bool>> rule, string failureMessage = null)
        {
            var text = new TextValidator(rule, failureMessage);
            operationRunner.Evaluate<UnexpectedElementStateException>(text);
        }

        public static void TextEquals(this IOperationRunner<IElementWrapper> operationRunner, string text, bool caseSensitive = false, bool trim = true, string failureMessage = null)
        {
            var textEquals = new TextEqualsValidator(text, caseSensitive, trim);
            operationRunner.Evaluate<UnexpectedElementStateException>(textEquals);
        }
        public static void TextNotEquals(this IOperationRunner<IElementWrapper> operationRunner, string text, bool caseSensitive = false, bool trim = true, string failureMessage = null)
        {
            var textNotEquals = new TextNotEqualsValidator(text, caseSensitive, trim);
            operationRunner.Evaluate<UnexpectedElementStateException>(textNotEquals);
        }

        public static void IsDisplayed(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var IsDisplayed = new IsDisplayedValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(IsDisplayed);
        }

        public static void IsNotDisplayed(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var isNotDisplayed = new IsNotDisplayedValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(isNotDisplayed);
        }

        public static void IsChecked(this IOperationRunner<IElementWrapper> operationRunner)
        {
            operationRunner.TagName("input", "Function IsNotChecked() can be used on input element only.");
            operationRunner.Attribute("type", new[] { "checkbox", "radio" }, failureMessage: "Input element must be type of checkbox.");

            var IsChecked = new IsCheckedValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(IsChecked);
        }

        public static void IsNotChecked(this IOperationRunner<IElementWrapper> operationRunner)
        {
            operationRunner.TagName("input", "Function IsNotChecked() can be used on input element only.");
            operationRunner.Attribute("type", new[] { "checkbox", "radio" }, failureMessage: "Input element must be type of checkbox or radio.");

            var IsNotChecked = new IsNotCheckedValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(IsNotChecked);
        }

        public static void IsSelected(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var IsSelected = new IsSelectedValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(IsSelected);
        }

        public static void IsNotSelected(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var IsNotSelected = new IsNotSelectedValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(IsNotSelected);
        }

        public static void IsEnabled(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var IsEnabled = new IsEnabledValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(IsEnabled);
        }

        public static void IsClickable(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var IsClickable = new IsClickableValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(IsClickable);
        }

        public static void IsNotClickable(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var isNotClickable = new IsNotClickableValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(isNotClickable);
        }

        public static void IsNotEnabled(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var isNotEnabled = new IsNotEnabledValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(isNotEnabled);
        }

        public static void ContainsText(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var ContainsText = new ContainsTextValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(ContainsText);
        }
        public static void DoesNotContainsText(this IOperationRunner<IElementWrapper> operationRunner)
        {
            var DoesNotContainsText = new TextEmptyValidator();
            operationRunner.Evaluate<UnexpectedElementStateException>(DoesNotContainsText);
        }

        public static void HyperLineEquals(this IOperationRunner<IElementWrapper> operationRunner, string url, UrlKind kind, params UriComponents[] components)
        {
            var HyperLinkEquals = new HyperLinkEqualsValidator(url, kind, true, components);
            operationRunner.Evaluate<UnexpectedElementStateException>(HyperLinkEquals);
        }

        public static void IsElementInView(this IOperationRunner<IElementWrapper> operationRunner, ElementWrapper element)
        {
            var IsElementInView = new IsElementInViewValidator(element);
            operationRunner.Evaluate<UnexpectedElementStateException>(IsElementInView);
        }

        public static void IsElementNotInView(this IOperationRunner<IElementWrapper> operationRunner, ElementWrapper element)
        {
            var IsElementNotInView = new IsElementNotInViewValidator(element);
            operationRunner.Evaluate<UnexpectedElementStateException>(IsElementNotInView);
        }


        public static void TagName(this IOperationRunner<IElementWrapper> operationRunner, string expectedTagName, string failureMessage = null)
        {
            var checkTagName = new TagNameValidator(expectedTagName, failureMessage);
            operationRunner.Evaluate<UnexpectedElementStateException>(checkTagName);
        }

        public static void TagName(this IOperationRunner<IElementWrapper> operationRunner, string[] expectedTagNames, string failureMessage = null)
        {
            var checkTagName = new TagNameValidator(expectedTagNames, failureMessage);
            operationRunner.Evaluate<UnexpectedElementStateException>(checkTagName);
        }



        public static void ContainsElement(this IOperationRunner<IElementWrapper> operationRunner, string cssSelector, Func<string, By> tmpSelectMethod = null)
        {
            var ContainsElement = new ContainsElementValidator(cssSelector, tmpSelectMethod);
            operationRunner.Evaluate<EmptySequenceException>(ContainsElement);
        }

        public static void NotContainsElement(this IOperationRunner<IElementWrapper> operationRunner, string cssSelector, Func<string, By> tmpSelectMethod = null)
        {
            var NotContainsElement = new NotContainsElementValidator(cssSelector, tmpSelectMethod);
            operationRunner.Evaluate<MoreElementsInSequenceException>(NotContainsElement);
        }

        public static void JsPropertyInnerText(this IOperationRunner<IElementWrapper> operationRunner, Expression<Func<string, bool>> rule, string failureMesssage = null, bool trim = true)
        {
            var JsPropertyInnerText = new JsPropertyInnerTextValidator(rule, failureMesssage, trim);
            operationRunner.Evaluate<UnexpectedElementStateException>(JsPropertyInnerText);
        }

        public static void JsPropertyInnerTextEquals(this IOperationRunner<IElementWrapper> operationRunner, string text, bool caseSensitive = false, bool trim = true)
        {
            var JsPropertyInnerTextEquals = new JsPropertyInnerTextEqualsValidator(text, caseSensitive, trim);
            operationRunner.Evaluate<UnexpectedElementStateException>(JsPropertyInnerTextEquals);
        }

        public static void JsPropertyInnerHtmlEquals(this IOperationRunner<IElementWrapper> operationRunner, string text, bool caseSensitive = false, bool trim = true)
        {
            var JsPropertyInnerHtmlEquals = new JsPropertyInnerHtmlEqualsValidator(text, caseSensitive, trim);
            operationRunner.Evaluate<UnexpectedElementStateException>(JsPropertyInnerHtmlEquals);
        }

        public static void JsPropertyInnerHtml(this IOperationRunner<IElementWrapper> operationRunner, Expression<Func<string, bool>> expression, string failureMessage = null)
        {
            var JsPropertyInnerHtml = new JsPropertyInnerHtmlValidator(expression, failureMessage);
            operationRunner.Evaluate<UnexpectedElementStateException>(JsPropertyInnerHtml);
        }

        public static void Attribute(this IOperationRunner<IElementWrapper> operationRunner, string attributeName, string value, bool caseSensitive = false, bool trimValue = true, string failureMessage = null)
        {
            var checkAttribute = new AttributeValuesValidator(attributeName, value, caseSensitive, trimValue, failureMessage);
            operationRunner.Evaluate<UnexpectedElementStateException>(checkAttribute);
        }

        public static void Attribute(this IOperationRunner<IElementWrapper> operationRunner, string attributeName, string[] allowedValues, bool caseSensitive = false, bool trimValue = true, string failureMessage = null)
        {
            var checkAttribute = new AttributeValuesValidator(attributeName, allowedValues, caseSensitive, trimValue, failureMessage);
            operationRunner.Evaluate<UnexpectedElementStateException>(checkAttribute);
        }

        public static void Attribute(this IOperationRunner<IElementWrapper> operationRunner, string attributeName, Expression<Func<string, bool>> expression, string failureMessage = null)
        {
            var Attribute = new AttributeValidator(attributeName, expression, failureMessage);
            operationRunner.Evaluate<UnexpectedElementStateException>(Attribute);
        }

        public static void ClassAttribute(this IOperationRunner<IElementWrapper> operationRunner, string value, bool caseSensitive = false, bool trimValue = true)
        {
            operationRunner.Attribute("class", value, caseSensitive, trimValue);
        }

        public static void ClassAttribute(this IOperationRunner<IElementWrapper> operationRunner, string attributeName, Expression<Func<string, bool>> expression, string failureMessage = "")
        {
            operationRunner.Attribute("class", expression, failureMessage);
        }

        public static void HasClass(this IOperationRunner<IElementWrapper> operationRunner, string value, bool caseSensitive = false)
        {
            operationRunner.Attribute("class",
                p =>
#if NET5_0_OR_GREATER
                p.Split(' ', StringSplitOptions.None)
#else
                p.Split(' ')
#endif
                .Any(c => string.Equals(c, value,
                caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)), $"Expected value: '{value}'.");
        }

        public static void HasNotClass(this IOperationRunner<IElementWrapper> operationRunner, string value, bool caseSensitive = false)
        {
            operationRunner.Attribute("class", p => !p.
#if NET5_0_OR_GREATER
            Split(' ', StringSplitOptions.None)
#else
            Split(' ')
#endif
            .Select(s => s.Trim()).Any(c => string.Equals(c, value,
                 caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)), $"Expected value: '{value}'.");
        }

        public static void HasAttribute(this IOperationRunner<IElementWrapper> operationRunner, string name)
        {
            var HasAttribute = new HasAttributeValidator(name);
            operationRunner.Evaluate<UnexpectedElementStateException>(HasAttribute);
        }

        public static void HasNotAttribute(this IOperationRunner<IElementWrapper> operationRunner, string name)
        {
            var HasNotAttribute = new HasNotAttributeValidator(name);
            operationRunner.Evaluate<UnexpectedElementStateException>(HasNotAttribute);
        }
        public static void AlertTextEquals(this IOperationRunner<IBrowserWrapper> operationRunner, string expectedValue,
            bool caseSensitive = false, bool trim = true)
        {
            var AlertTextEquals = new AlertTextEqualsValidator(expectedValue, caseSensitive, trim);
            operationRunner.Evaluate<AlertException>(AlertTextEquals);
        }

        public static void AlertText(this IOperationRunner<IBrowserWrapper> operationRunner, Expression<Func<string, bool>> expression, string failureMessage = "")
        {
            var AlertText = new AlertTextValidator(expression, failureMessage);
            operationRunner.Evaluate<AlertException>(AlertText);
        }

        public static void AlertTextContains(this IOperationRunner<IBrowserWrapper> operationRunner, string expectedValue, bool trim = true)
        {
            var AlertTextContains = new AlertTextContainsValidator(expectedValue, trim);
            operationRunner.Evaluate<AlertException>(AlertTextContains);
        }

        public static void Url(this IOperationRunner<IBrowserWrapper> operationRunner, Expression<Func<string, bool>> expression, string failureMessage = null)
        {
            var Url = new CurrentUrlValidator(expression, failureMessage);
            operationRunner.Evaluate<BrowserLocationException>(Url);
        }

        public static void Url(this IOperationRunner<IBrowserWrapper> operationRunner, string url, UrlKind urlKind, params UriComponents[] components)
        {
            var checkUrl = new UrlValidator(url, urlKind, components);
            operationRunner.Evaluate<BrowserLocationException>(checkUrl);
        }

        public static void UrlEquals(this IOperationRunner<IBrowserWrapper> operationRunner, string url)
        {
            var checkUrlEquals = new UrlEqualsValidator(url);
            operationRunner.Evaluate<BrowserLocationException>(checkUrlEquals);
        }
        //TODO
        //public static void HyperLinkEquals(this IOperationRunner<IBrowserWrapper> operationRunner, string selector, string url, UrlKind kind, params UriComponents[] components)
        //{
        //    var HyperLinkEquals = new HyperLinkEqualsValidator(selector, url, kind, components);
        //    operationRunner.Evaluate<UnexpectedElementStateException>(HyperLinkEquals);
        //}

        //public static void IsDisplayed(this IOperationRunner<IBrowserWrapper> operationRunner, string selector, Expression<Func<string, By>> tmpSelectedMethod = null)
        //{
        //    var IsDisplayed = new IsDisplayed();
        //    operationRunner.Evaluate<UnexpectedElementStateException>(IsDisplayed);
        //}
        //public static void IsNotDisplayed(this IOperationRunner<IBrowserWrapper> operationRunner, string selector, Expression<Func<string, By>> tmpSelectedMethod = null)
        //{
        //    var IsNotDisplayed = new Checkers.BrowserWrapperCheckers.IsNotDisplayed(selector, tmpSelectedMethod);
        //    operationRunner.Evaluate<BrowserException>(IsNotDisplayed);
        //}

        public static void UrlIsAccessible(this IOperationRunner<IBrowserWrapper> operationRunner, string url, UrlKind urlKind)
        {
            var UrlIsAccessible = new UrlIsAccessibleValidator(url, urlKind);
            operationRunner.Evaluate<BrowserException>(UrlIsAccessible);
        }

        public static void TitleEquals(this IOperationRunner<IBrowserWrapper> operationRunner, string title, bool caseSensitive = false, bool trim = true)
        {
            var TitleEquals = new TitleEqualsValidator(title, caseSensitive, trim);
            operationRunner.Evaluate<BrowserException>(TitleEquals);
        }

        public static void TitleNotEquals(this IOperationRunner<IBrowserWrapper> operationRunner, string title, bool caseSensitive = false, bool trim = true)
        {
            var TitleNotEquals = new TitleNotEqualsValidator(title, caseSensitive, trim);
            operationRunner.Evaluate<BrowserException>(TitleNotEquals);
        }

        public static void Title(this IOperationRunner<IBrowserWrapper> operationRunner, Expression<Func<string, bool>> expression, string failureMessage = "")
        {
            var Title = new TitleValidator(expression, failureMessage);
            operationRunner.Evaluate<BrowserException>(Title);
        }


    }
}
