using Riganti.Selenium.Core.Abstractions.Exceptions;
using System;
using Xunit;

namespace Riganti.Selenium.Core.Samples.AssertApi.Tests
{
    public class WaitForExecutorTests
    {

        [Fact]
        public void DefaultOptionsTest() => TestWaitForBlock(null, 8000, 10000);

        [Fact]
        public void DefaultOptionsTest2() => TestWaitForBlock(WaitForOptions.DefaultOptions, 8000, 10000);

        [Fact]
        public void ShortOptionsTest() => TestWaitForBlock(WaitForOptions.ShortTimeout, 4000, 6000);

        [Fact]
        public void LongOptionsTest() => TestWaitForBlock(WaitForOptions.LongTimeout, 12000, 14000);

        [Fact]
        public void LongerOptionsTest() => TestWaitForBlock(WaitForOptions.LongerTimeout, 16000, 18000);

        [Fact]
        public void DisabledOptionsTest() => TestWaitForBlock(WaitForOptions.Disabled, 0, 100);


        /// <summary>
        /// This method tests whether wait-block throws exception after timing out in specified range min/max timeout.
        /// </summary>
        private static void TestWaitForBlock(WaitForOptions options, int minTimeout, int maxTimeout)
        {
            var startTime = DateTime.UtcNow;
            Assert.Throws<UnexpectedElementStateException>(() =>
            {
                WaitForExecutor.WaitFor(() =>
                {
                    throw new UnexpectedElementStateException();
                }, options);
            });
            var endTime = DateTime.UtcNow;
            var time = endTime - startTime;

            Assert.True(time.TotalMilliseconds >= minTimeout);
            Assert.True(time.TotalMilliseconds < maxTimeout);
        }

        [Fact]
        public void DefaultOptionsTest_Continue() => TestWaitForBlock_Continue(null, 200);

        [Fact]
        public void DefaultOptionsTest2_Continue() => TestWaitForBlock_Continue(WaitForOptions.DefaultOptions, 200);

        [Fact]
        public void ShortOptionsTest_Continue() => TestWaitForBlock_Continue(WaitForOptions.ShortTimeout, 200);

        [Fact]
        public void LongOptionsTest_Continue() => TestWaitForBlock_Continue(WaitForOptions.LongTimeout, 200);

        [Fact]
        public void LongerOptionsTest_Continue() => TestWaitForBlock_Continue(WaitForOptions.LongerTimeout, 200);

        [Fact]
        public void DisabledOptionsTest_Continue() => TestWaitForBlock_Continue(WaitForOptions.Disabled, 200);

        /// <summary>
        /// This method tests whether wait-block does not throw exception and continue as soon as possible.
        /// </summary>

        private static void TestWaitForBlock_Continue(WaitForOptions options, int maxTimeout)
        {
            var startTime = DateTime.UtcNow;
            WaitForExecutor.WaitFor(() =>
            {
                    // do nothing here 
            }, options);
            
            var endTime = DateTime.UtcNow;
            var time = endTime - startTime;

            Assert.True(time.TotalMilliseconds < maxTimeout);
        }
    }

}