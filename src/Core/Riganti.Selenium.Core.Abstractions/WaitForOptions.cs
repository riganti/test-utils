using System.Runtime.CompilerServices;

namespace Riganti.Selenium.Core
{
    /// <summary>
    /// Options to modify behavior of WaitFor block.
    /// </summary>
    public class WaitForOptions
    {
        public WaitForOptions()
        {
        }

        public WaitForOptions(int timeout, string message)
        {
            FailureMessage = message;
            Timeout = timeout;
        }
        public WaitForOptions(int timeout)
        {
            Timeout = timeout;
        }
        public WaitForOptions(bool enabled)
        {
            Enabled = enabled;
        }

        /// <summary>
        /// Defines whether WaitFor can be applied or no.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Defines whether WaitBlock should throw original exception or aggregated WaitBlockException in case of failure.
        /// </summary>
        public bool ThrowOriginalException { get; set; } = true;

        /// <summary>
        /// Custom message that is show when check fails.
        /// </summary>
        public string FailureMessage { get; set; }
        /// <summary>
        /// Maximum time within condition has to be satisfied.
        /// </summary>
        public int Timeout { get; set; } = 8000;
        /// <summary>
        /// Define interval between periodical check whether given condition is satisfied or not.
        /// </summary>
        public int CheckInterval { get; set; } = 30;
        /// <summary>
        /// This option disables usage of WaitFor block.
        /// </summary>
        public readonly static WaitForOptions Disabled = new WaitForOptions(false);
        /// <summary>
        /// Options set to total timeout 8s, check interval 30ms
        /// </summary>
        public readonly static WaitForOptions DefaultOptions = new WaitForOptions();
        /// <summary>
        /// Options set to total timeout 4s, check interval 30ms
        /// </summary>
        public readonly static WaitForOptions ShortTimeout = new WaitForOptions(4000);
        /// <summary>
        /// Options set to total timeout 12s, check interval 30ms
        /// </summary>
        public readonly static WaitForOptions LongTimeout = new WaitForOptions(12000);
        /// <summary>
        /// Options set to total timeout 16s, check interval 30ms
        /// </summary>
        public readonly static WaitForOptions LongerTimeout = new WaitForOptions(16000);

        public static WaitForOptions FromTimeout(int timeout)
        {
            return new WaitForOptions(timeout);
        }

        public static WaitForOptions FromMessage(string message)
        {
            return new WaitForOptions() { FailureMessage = message };
        }
    }
}
