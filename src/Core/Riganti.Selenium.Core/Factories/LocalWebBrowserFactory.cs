using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riganti.Selenium.Core.Drivers;
using Riganti.Selenium.Core.Configuration;
using Riganti.Selenium.Core.Logging;

namespace Riganti.Selenium.Core.Factories
{
    public abstract class LocalWebBrowserFactory : IWebBrowserFactory
    {
        
        public abstract string Name { get; }

        public IDictionary<string, string> Options { get; } = new Dictionary<string, string>();
        public IList<string> Capabilities { get; } = new List<string>();


        public TestSuiteRunner TestSuiteRunner { get; }


        private readonly object locker = new object();
        private IWebBrowser currentBrowser = null;


        public LocalWebBrowserFactory(TestSuiteRunner runner)
        {
            TestSuiteRunner = runner;
        }


        public Task<IWebBrowser> AcquireBrowser()
        {
            lock (locker)
            {
                if (currentBrowser != null)
                {
                    return Task.FromResult<IWebBrowser>(null);
                }

                currentBrowser = CreateBrowser();
                return Task.FromResult(currentBrowser);
            }
        }

        public Task ReleaseBrowser(IWebBrowser browser)
        {
            lock (locker)
            {
                if (browser != currentBrowser)
                {
                    throw new InvalidOperationException($"The LocalWebBrowserFactory received a request to release an unknown browser instance!");
                }

                DisposeBrowser(browser);
                currentBrowser = null;

                return Task.FromResult(0);
            }
        }

        protected abstract IWebBrowser CreateBrowser();

        protected virtual void DisposeBrowser(IWebBrowser browser)
        {
            browser.Dispose();
        }
    }
}
