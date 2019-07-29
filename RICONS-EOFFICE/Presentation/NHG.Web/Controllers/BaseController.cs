using System.Web.Mvc;
using NHG.Core;
using NHG.Logger;
using System.Web.Routing;
using System.Web;
using System.Collections.Generic;
using NHG.Web.Models;
using System;
using NHG.Core.Functions;

namespace Nop.Admin.Controllers
{
    public class BaseController : Controller
    {
        #region Fields
        protected FunctionXML defaultConst = new FunctionXML(Functions.MapPath("~/Xml/Const/Default.xml"));
        protected bool isAdmin = false;
        private Log4Net _logger = new Log4Net(typeof(BaseController));
        private string updating = "Đang cập nhật";
        #endregion

        protected override void Initialize(RequestContext requestContext)
        {
            HttpRequestBase request = requestContext.HttpContext.Request;
            System.Net.IPAddress[] a = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
            if (a.Length > 0)
                Log4Net.macaddress = a[0].ToString();
            Log4Net.clientIP = request.UserHostAddress;
            Log4Net.browser = request.Browser.Browser + ".v" + request.Browser.Version;
            Log4Net.useragent = request.UserAgent;
            ViewBag.Updating = updating;
            base.Initialize(requestContext);
        }


        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected ActionResult AccessDeniedView()
        {
            //return new HttpUnauthorizedResult();
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
        }

        /// <summary>
        /// Quay ve trang dang nhap
        /// </summary>
        /// <returns>Nguoi dung chua dang nhap</returns>
        protected ActionResult BackToLogin()
        {
            //return new HttpUnauthorizedResult();
            return RedirectToAction("Login", "Account", new { pageUrl = this.Request.RawUrl });
        }

        /// <summary>
        /// Quay ve trang chu
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if(string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Vacationlist", "Absent");
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Vacationlist", "Absent");
            }

            //if (string.IsNullOrWhiteSpace(returnUrl))
            //    return RedirectToAction("Index", "Home");
            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

        }


        /// <summary>
        /// kiem tra dang nhap
        /// </summary>
        /// <returns>
        /// true: neu nguoi dung da dang nhap
        /// false: neu nguoi dung chua dang nhap
        /// </returns>
        protected bool IsLogged()
        {
            if (Session != null)
            {
                if (Session["userid"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(Session["userid"].ToString()))
                    {
                        if (Session["userid"].ToString() == "1")
                            isAdmin = true;
                        return true;
                    }
                }
            }
            return false;
        }

        protected string GetControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }

        protected string GetAcctionName()
        {
            return this.ControllerContext.RouteData.Values["action"].ToString();
        }

        protected PhongBanDonViModels GetPhongBanDonVi()
        {
            try
            {
                List<PhongBanDonViModels> lst = (List<PhongBanDonViModels>)Session["phongBanDonVi"];
                if(lst.Count > 0)
                {
                    PhongBanDonViModels phongBanDonvi = lst[0];
                    return phongBanDonvi;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

    }
}
