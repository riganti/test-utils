using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Riganti.Selenium.Core.Abstractions
{
    public interface IElementWrapperCollection<TElement, TBrowser> : ISeleniumWrapperCollection,
        IReadOnlyCollection<TElement> where TElement : IElementWrapper where TBrowser : IBrowserWrapper
    {
        TElement this[int index] { get; }

        TBrowser BrowserWrapper { get; set; }
        bool IsReadOnly { get; }

        bool Contains(TElement item);
        TElement ElementAt(int index);

        IElementWrapperCollection<TElement, TBrowser> FindElements(string selector,
            Func<string, By> tmpSelectMethod = null);

        TElement First(string selector, Func<string, By> tmpSelectMethod = null);
        TElement FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null);
        void ForEach(Action<TElement> action);
        int IndexOf(TElement item);
        TElement Last();
        bool Remove(TElement item);
        void RemoveAt(int index);
        IEnumerable<TResult> Select<TResult>(Func<TElement, TResult> selector);
        TElement Single();
        TElement SingleOrDefault();
        IElementWrapperCollection<TElement, TBrowser> ThrowIfDifferentCountThan(int count);
        IElementWrapperCollection<TElement, TBrowser> ThrowIfEmptyOrMoreThanOne();
        IElementWrapperCollection<TElement, TBrowser> ThrowIfSequenceContainsMoreThanOneElement();
        IElementWrapperCollection<TElement, TBrowser> ThrowIfSequenceEmpty();
    }

    public interface ISeleniumWrapperCollection : ISeleniumWrapper
    {
        IElementWrapperCollection<TElement, TBrowser> Convert<TElement, TBrowser>() where TElement : IElementWrapper
            where TBrowser : IBrowserWrapper;
    }
}