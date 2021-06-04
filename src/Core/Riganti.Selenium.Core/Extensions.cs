using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Riganti.Selenium.Core.Abstractions;

namespace Riganti.Selenium.Core
{
    public static class Extensions
    {
        public static IElementWrapperCollection<TElement, TBrowser> ToElementsList<TElement, TBrowser>(Func<IEnumerable<IWebElement>> elements, TBrowser browserWrapper,
            string selector, Func<string, By> selectMethod, IServiceFactory serviceFactory) where TElement : IElementWrapper where TBrowser : IBrowserWrapper
        {
            Func<IWebElement, IElementWrapper> initElementWrapper = (s) => serviceFactory.Resolve<IElementWrapper>(new Func<IWebElement>(() => s), browserWrapper);

            var result = serviceFactory.Resolve<ISeleniumWrapperCollection>(
                                                        new Func<IEnumerable<IElementWrapper>>(() => elements().Select(initElementWrapper).ToList()),
                                                        selector,
                                                        selectMethod,
                                                        (IBrowserWrapper)browserWrapper);

            return result.Convert<TElement, TBrowser>();
        }


        public static IElementWrapperCollection<TElement, TBrowser> ToElementsList<TElement, TBrowser>(Func<IEnumerable<IWebElement>> elements,
            TBrowser browserWrapper,
            string selector,
            Func<string, By> selectMethod,
            TElement elementWrapper,
            IServiceFactory serviceFactory) where TElement : IElementWrapper where TBrowser : IBrowserWrapper
        {

            Func<IWebElement, IElementWrapper> initElementWrapper = (s) => serviceFactory.Resolve<IElementWrapper>(new Func<IWebElement>(() => s), browserWrapper);

            var result = serviceFactory.Resolve<ISeleniumWrapperCollection>(
                                                      new Func<IEnumerable<IElementWrapper>>(() => elements().Select(initElementWrapper).ToList()),
                                                      selector, selectMethod, (IElementWrapper)elementWrapper, browserWrapper);

            return result.Convert<TElement, TBrowser>();

        }
    }
}