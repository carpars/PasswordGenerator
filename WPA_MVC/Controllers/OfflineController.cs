using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WPA_MVC.Controllers
{
    public class OfflineController : Controller
    {
        // GET: Offline
        public ActionResult Index()
        {
            return View();
        }        
    }
}