﻿using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Abstractions.Exceptions;

namespace Riganti.Selenium.Core
{
    public class ElementWrapperCollection<TElement, TBrowser> : IElementWrapperCollection<TElement, TBrowser> where TBrowser : IBrowserWrapper where TElement : IElementWrapper
    {
        public string Selector { get; }
        public TElement this[int index]
        {
            get { return ElementAt(index); }
            set { collection[index] = value; }
        }


        public ISeleniumWrapper ParentWrapper { get; set; }
        /// <summary>
        /// Wrapper of a current browser. 
        /// </summary>
        public TBrowser BrowserWrapper { get; set; }
        /// <summary>
        /// Selector construced by provided partail selectors in a test. 
        /// </summary>
        public string FullSelector => (ParentWrapper == null ? (Selector ?? "") : $"{ParentWrapper.FullSelector} {Selector ?? ""}").Trim();

        public void ActivateScope()
        {
            if (ParentWrapper == null)
            {
                BrowserWrapper.ActivateScope();
            }
            else
            {
                ParentWrapper.ActivateScope();
            }
        }



        /// <inheritdoc />
        public ElementWrapperCollection(IEnumerable collection, string selector, IBrowserWrapper browserWrapper)
        {
            this.collection = new List<TElement>(collection.Cast<TElement>());
            Selector = selector;
            SetReferences(selector);
            BrowserWrapper = (TBrowser)browserWrapper;
        }


        /// <inheritdoc />
        public ElementWrapperCollection(IEnumerable collection, string selector, ISeleniumWrapper parentElement, IBrowserWrapper browserWrapper)
        {
            this.collection = new List<TElement>(collection.Cast<TElement>());
            SetReferences(selector);
            Selector = selector;
            ParentWrapper = parentElement;
            BrowserWrapper = (TBrowser)browserWrapper;
        }

        /// <inheritdoc />
        public ElementWrapperCollection(IEnumerable collection, string selector, IBrowserWrapper browserWrapper, ISeleniumWrapper parentCollection)
        {
            this.collection = new List<TElement>(collection.Cast<TElement>());
            SetReferences(selector);
            Selector = selector;
            BrowserWrapper = (TBrowser)browserWrapper;
            ParentWrapper = parentCollection;
        }


        /// <summary>
        /// Sets children reference to Parent wrapper.
        /// </summary>
        /// <param name="selector"></param>
        private void SetReferences(string selector)
        {
            foreach (var ew in collection)
            {
                ew.Selector = selector;
                ew.ParentWrapper = this;
            }
        }
        public IElementWrapperCollection<TElement, TBrowser> ThrowIfSequenceEmpty()
        {
            if (!collection.Any())
            {
                throw new EmptySequenceException($"Sequence contains no elements. Selector: '{FullSelector}'");
            }
            return this;
        }

        public IElementWrapperCollection<TElement, TBrowser> ThrowIfSequenceContainsMoreThanOneElement()
        {
            if (Count > 1)
            {
                throw new MoreElementsInSequenceException($"Sequence contains more than one element. Selector: '{FullSelector}'");
            }
            return this;
        }
        /// <summary>
        /// Throws exception when lenght of a sequence is empty or the sequence contains more than one element.
        /// </summary>
        /// <returns></returns>
        public IElementWrapperCollection<TElement, TBrowser> ThrowIfEmptyOrMoreThanOne()
        {
            return ThrowIfSequenceEmpty().ThrowIfSequenceContainsMoreThanOneElement();
        }
        /// <summary>
        /// Throws exception when lenght of sequence is different then specified number.
        /// </summary>
        /// <param name="count">expected sequence lenght</param>
        public IElementWrapperCollection<TElement, TBrowser> ThrowIfDifferentCountThan(int count)
        {
            if (Count != count)
            {
                throw new SequenceCountException($"Element count in sequence is different from the expected value. Selector: '{FullSelector}', Expected value: '{count}', Actual value: '{Count}'.");
            }
            return this;
        }
        /// <summary>
        /// Returns a single, specific web element of a sequence, or null value if that element is not found.
        /// </summary>
        public TElement SingleOrDefault()
        {
            var elements = ThrowIfSequenceContainsMoreThanOneElement();
            return elements.Count == 0 ? default : elements[0];
        }
        /// <summary>
        /// Returns the only web element of a sequence, and throws an exception if there is not exactly one element in the sequence.
        /// </summary>
        public TElement Single()
        {

            return (TElement)ThrowIfEmptyOrMoreThanOne()[0];
        }
        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>
        /// <param name="index">position of web element to return</param>
        public TElement ElementAt(int index)
        {
            if (Count <= index || index < 0)
            {
                throw new SequenceCountException($"Index is out of range. Selector: '{FullSelector}', Sequence contains {Count} elements, Current index: '{index}'");
            }
            return collection[index];
        }
        /// <summary>
        /// Returns last web element of a sequence.
        /// </summary>
        /// <returns></returns>
        public TElement Last()
        {
            ThrowIfSequenceEmpty();
            return (TElement)collection.Last();
        }

        protected readonly List<TElement> collection;

        public IEnumerator<TElement> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TElement item)
        {
            throw new NotSupportedException("Collection is readonly.");
        }

        public void Clear()
        {
            throw new NotSupportedException("Collection is readonly.");
        }

        public bool Contains(TElement item)
        {
            return collection.Contains(item);
        }

        public void CopyTo(IElementWrapper[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool Remove(TElement item)
        {
            throw new NotSupportedException("Collection is readonly.");
        }

        public int Count => collection.Count;
        public bool IsReadOnly => true;

        public int IndexOf(TElement item)
        {
            return collection.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
        }

        public IEnumerable<TResult> Select<TResult>(Func<TElement, TResult> selector)
        {
            return collection.Select(selector);
        }


        /// <summary>
        /// Performs the specified action on each element of the <see cref="IElementWrapperCollection{TElement,TBrowser}"/>.
        /// </summary>
        /// <param name="action">The <see cref="T:System.Action`1"/> delegate to perform on each element of the <see cref="T:System.Collections.Generic.List`1"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="action"/> is null.</exception>
        public void ForEach(Action<TElement> action)
        {
            collection.ForEach(action);
        }

        public IElementWrapperCollection<TElement, TBrowser> FindElements(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var results = collection.SelectMany(item => item.FindElements(selector, tmpSelectMethod));
            var result = BrowserWrapper.ServiceFactory.Resolve<ISeleniumWrapperCollection>(results, selector, this, BrowserWrapper);
            return result.Convert<TElement, TBrowser>();
        }

        /// <summary>
        /// Returns first element from sequence. If sequence contains no element, function throws exception.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="tmpSelectMethod">Defines what type of selector are you want to use only for this query.</param>
        /// <exception cref="EmptySequenceException"></exception>
        public TElement First(string selector, Func<string, By> tmpSelectMethod = null)
        {
            var element = FirstOrDefault(selector, tmpSelectMethod);
            if (element != null)
            {
                return element;
            }
            throw new EmptySequenceException($"Sequence contains no element with the selector: '{FullSelector}'");
        }

        /// <summary>
        /// Returns first element from sequence. If sequence doesn't contain the element, function returns null.
        /// </summary>
        /// <param name="selector">Describes what element to select.</param>
        /// <param name="tmpSelectMethod">Defines what type of selector are you want to use only for this query.</param>
        public TElement FirstOrDefault(string selector, Func<string, By> tmpSelectMethod = null)
        {
            return (TElement)collection.Select(item => item.FirstOrDefault(selector, tmpSelectMethod)).FirstOrDefault(element => element != null);
        }

        IElementWrapperCollection<TElement1, TBrowser1> ISeleniumWrapperCollection.Convert<TElement1, TBrowser1>()
        {
            return new ElementWrapperCollection<TElement1, TBrowser1>(collection.Cast<TElement1>(),
                Selector, BrowserWrapper, ParentWrapper);
        }
    }
}
