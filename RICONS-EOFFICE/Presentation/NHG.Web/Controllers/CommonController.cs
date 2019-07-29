using NHG.Core.ClassData;
using NHG.Core.Functions;
using NHG.Web.Data.Services;
using NHG.Web.Models;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class CommonController : BaseController
    {
        //
        // GET: /Common/
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        public ActionResult MenuHeaderLink()
        {
            if (!IsLogged())
                return BackToLogin();

            //lay danh sach phan quyen
            LoginServices service = new LoginServices();
            
            FunctionXML fnc = new FunctionXML(Functions.MapPath("~/xml/Config/sitemap.config"));
            List<MenuLinks> model = fnc.ReadMenuConfig(service.GetRoles(new TaiKhoanModels() { mataikhoan = Session["userid"].ToString() }), base.isAdmin, Session["grouptk"].ToString());

            return PartialView(model);
        }
	}
}