using Riganti.Selenium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riganti.Selenium.DotVVM;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Riganti.Selenium.DotVVM3.Samples.Tests
{
    public class FileUploadTests : AppSeleniumTest
    {
        public FileUploadTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void FileUpload_ElementWrapperByDiv()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/FileUpload");

                var tempPath = Path.GetTempFileName();
                File.WriteAllBytes(tempPath, Enumerable.Range(0, 255).Select(i => (byte)i).ToArray());

                var elm = browser.First("#FUpload", SelectBy.CssSelector);
                DotVVMAssert.UploadFile(browser.First("#FUpload"), tempPath);

                browser.WaitFor(() =>
                {
                    AssertUI.InnerTextEquals(browser.First("#FUpload .dotvvm-upload-files"), "1 files", false);
                }, 4000, "File upload failed.");
            });
        }
        [Fact]
        public void FileUpload_ElementWrapperByA()
        {
            RunInAllBrowsers(browser =>
            {
                browser.NavigateToUrl("/FileUpload");

                var tempPath = Path.GetTempFileName();
                File.WriteAllBytes(tempPath, Enumerable.Range(0, 255).Select(i => (byte)i).ToArray());

                DotVVMAssert.UploadFile(browser.First(".dotvvm-upload-button a"), tempPath);

                browser.WaitFor(() =>
                {
                    AssertUI.InnerTextEquals(browser.First("#FUpload .dotvvm-upload-files"), "1 files", false);
                }, 4000, "File upload failed.");
            });
        }
    }
}
