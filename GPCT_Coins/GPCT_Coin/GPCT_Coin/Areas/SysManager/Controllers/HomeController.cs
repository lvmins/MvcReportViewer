using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPCT_Coin.Areas.SysManager.Controllers
{
    public class HomeController : Controller
    {
        // GET: SysManager/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}