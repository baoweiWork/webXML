using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebXML.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }
        public static void ArrLists()
        {
            int[] arr = new int[16];
            for (int i = 0; i < 16; i++)
            {
                arr[i] = i + 1;
            }

        }
    }
}