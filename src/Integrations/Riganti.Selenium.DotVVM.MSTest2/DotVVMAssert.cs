﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riganti.Selenium.Core;
using Riganti.Selenium.Core.Abstractions;

namespace Riganti.Selenium.DotVVM
{
    public static class DotVVMAssert
    {
        public static void UploadFile(IElementWrapper element, string fullFileName)
        {

            if (element.BrowserWrapper.IsDotvvmPage())
            {
                element.BrowserWrapper.LogVerbose("Selenium.DotVVM : Uploading file");
                var name = element.GetTagName();
                if (name == "a" && element.HasAttribute("onclick") && (element.GetAttribute("onclick")?.Contains("showUploadDialog") ?? false))
                {
                    UploadFileByA(element, fullFileName);
                    return;
                }

                if (name == "div" && element.FindElements("iframe", SelectBy.CssSelector).Count == 1)
                {
                    UploadFileByDiv(element, fullFileName);
                    return;
                }
                else
                {
                    element.BrowserWrapper.LogVerbose("Selenium.DotVVM : Cannot identify DotVVM scenario. Uploading over standard procedure.");

                    element.BrowserWrapper.OpenInputFileDialog(element, fullFileName);
                    return;
                }
            }

            element.BrowserWrapper.OpenInputFileDialog(element, fullFileName);
        }

        private static void UploadFileByDiv(IElementWrapper element, string fullFileName)
        {
            element.BrowserWrapper.GetJavaScriptExecutor()
           .ExecuteScript("dotvvm.fileUpload.createUploadId(arguments[0])", element.First("a", SelectBy.CssSelector).WebElement);

            var iframe = element.First("iframe", SelectBy.CssSelector).WebElement;
            element.BrowserWrapper.Driver.SwitchTo().Frame(iframe);

            var fileInput = element.BrowserWrapper._GetInternalWebDriver()
                .FindElement(SelectBy.CssSelector("input[type=file]"));
            fileInput.SendKeys(fullFileName);

            element.Wait(element.ActionWaitTime);
            element.ActivateScope();
            
        }

        private static void UploadFileByA(IElementWrapper element, string fullFileName)
        {
            element.BrowserWrapper.GetJavaScriptExecutor()
                .ExecuteScript("dotvvm.fileUpload.createUploadId(arguments[0])", element.WebElement);

            var iframe = element.ParentElement.ParentElement.First("iframe", SelectBy.CssSelector).WebElement;
            element.BrowserWrapper.Driver.SwitchTo().Frame(iframe);

            var fileInput = element.BrowserWrapper._GetInternalWebDriver()
                .FindElement(SelectBy.CssSelector("input[type=file]"));
            fileInput.SendKeys(fullFileName);

            element.Wait(element.ActionWaitTime);
            element.ActivateScope();
        }
    }
}