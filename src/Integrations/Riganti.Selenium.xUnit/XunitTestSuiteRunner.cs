using System;
using System.Diagnostics;
using System.Linq;
using Riganti.Selenium.Core;
using Riganti.Selenium.Core.Abstractions;
using Riganti.Selenium.Core.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Riganti.Selenium.xUnitIntegration
{
    public class XunitTestSuiteRunner : TestSuiteRunner
    {
        public XunitTestSuiteRunner(SeleniumTestsConfiguration configuration, TestContextProvider testContextProvider, Action<ServiceFactory, TestSuiteRunner> registerServices = null) : base(configuration, (ITestContextProvider)testContextProvider, registerServices)
        {
        }

        public override void RunInAllBrowsers(ISeleniumTest testClass, Action<IBrowserWrapper> action, string callerMemberName, string callerFilePath,
            int callerLineNumber)
        {
            //TODO: make a review
            var context = (TestContextWrapper)TestContextProvider.GetGlobalScopeTestContext();
            context.TestName = callerMemberName;
            context.FullyQualifiedTestClassName = new System.Diagnostics.StackTrace().GetFrames()?.Select(s => s.GetMethod()?
                .ReflectedType?.FullName).FirstOrDefault(s => !s?.Contains(this.GetType()?.Namespace ?? "") ?? false);
            base.RunInAllBrowsers(testClass, action, callerMemberName, callerFilePath, callerLineNumber);
        }
    }

    //public class SeleniumInternalXunitRunnerReporter : Xunit.IRunnerReporter
    //{
    //    public string Description => "This reporter is a bit hack to close all resources when test execution ends.";

    //    public bool IsEnvironmentallyEnabled => true;

    //    public string RunnerSwitch => "riganti-selenium";

    //    public IMessageSink CreateMessageHandler(IRunnerLogger logger)
    //    {
    //        return new SeleniumMessageSink();
    //    }
    //}
    //public class SeleniumMessageSink : IMessageSink
    //{
    //    public bool OnMessage(IMessageSinkMessage message)
    //    {
    //        if (message is TestAssemblyExecutionFinished)
    //        {
    //            if (Debugger.IsAttached)
    //                Debugger.Break();
    //        }
    //        return true;
    //    }
    //}
}