using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleUploadExcelWeb.Controllers
{
    public class NewUploadRequirementController : Controller
    {
        // GET: NewUploadRequirement
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Upload()
        {
            return View();
        }
    }
}