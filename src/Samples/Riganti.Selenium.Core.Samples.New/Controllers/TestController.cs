using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Riganti.Selenium.Core.Samples.New.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Alert() { return View(null); }
        public ActionResult Attribute()
        {
            return View(null);
        }
        public ActionResult Checkboxes() { return View(null); }
        public ActionResult ClickTest() { return View(null); }
        public ActionResult Confirm() { return View(null); }

        public ActionResult Cookies()
        {
            if (!ControllerContext.HttpContext.Request.Cookies.ContainsKey("test_cookie"))
            {
                CookieOptions cookie = new CookieOptions();
                Response.Cookies.Append("test_cookie", "default value", cookie);
            }
            return View(new CookieModel(){Text = ControllerContext.HttpContext.Request.Cookies["test_cookie"] });
        }

        public ActionResult CookiesSetCookie()
        {
            CookieOptions cookie = new CookieOptions();
            Response.Cookies.Append("test_cookie", "new value", cookie);
            return View("Cookies", new CookieModel() { Text = ControllerContext.HttpContext.Request.Cookies["test_cookie"] });
        }
        public ActionResult Displayed()
        {
            return View(null);
        }
        public ActionResult ElementAtFirst() { return View(null); }
        public ActionResult ElementContained() { return View(null); }
        public ActionResult ElementSelection() { return View(null); }
        public ActionResult FileDialog() { return View(null); }
        public ActionResult IFrameInner() { return View(null); }
        public ActionResult IFrameMain() { return View(null); }
        public ActionResult FindElements() { return View(null); }
        public ActionResult HyperLink() { return View(null); }
        public ActionResult Index() { return View(null); }
        public ActionResult Focus() { return View(null); }
        public ActionResult JsView() { return View(null); }
        public ActionResult ScrollTo() { return View(null); }
        public ActionResult SelectMethod() { return View(null); }
        public ActionResult TemporarySelector() { return View(null); }
        public ActionResult Text() { return View(null); }
        public ActionResult Title()
        {
            return View(null);
        }
        public ActionResult Value() { return View(null); }
        public ActionResult DragAndDrop() { return View(null); }
        public ActionResult CssClasses() { return View(null); }
        public ActionResult CssStyles() { return View(null); }
    }

    public class CookieModel
    {
        public string Text { get; set; }
    }
}
