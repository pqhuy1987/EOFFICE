using Newtonsoft.Json.Linq;
using NHG.Core.Functions;
using NHG.Core.Message;
using NHG.Logger;
using NHG.Web.Data.Services;
using NHG.Web.Models;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class HomeController : BaseController
    {
        

        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

    }
}