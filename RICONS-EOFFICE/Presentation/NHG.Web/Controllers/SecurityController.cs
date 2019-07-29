using NHG.Logger;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class SecurityController : BaseController
    {
        #region Fields
        Log4Net logger = new Log4Net(typeof(SecurityController));

        #endregion
        //
        // GET: /Security/
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        public ActionResult AccessDenied(string pageUrl)
        {
            if (!IsLogged())
                return BackToLogin();
            //var currentCustomer = _workContext.CurrentCustomer;
            //if (currentCustomer == null || currentCustomer.IsGuest())
            //{
            //    _logger.Information(string.Format("Access denied to anonymous request on {0}", pageUrl));
            //    return View();
            //}

            logger.Info(string.Format("Access denied to user #{0} '{1}' on {2}", "", "", pageUrl));


            return View();
        }
	}
}