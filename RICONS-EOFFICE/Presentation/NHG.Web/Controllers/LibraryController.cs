using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class LibraryController : BaseController
    {
        //
        // GET: /Library/
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }
	}
}