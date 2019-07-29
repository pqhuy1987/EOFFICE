using NHG.Logger;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class DocumentController : BaseController
    {
        #region Fields
        Log4Net _logger = new Log4Net(typeof(DocumentController));
        #endregion

        //
        // GET: /Document/
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }
	}
}