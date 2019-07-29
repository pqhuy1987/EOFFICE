using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NHG.Logger;
using NHG.Core.Functions;
using NHG.Web.Data.Services;
using System.Reflection;
using System.IO;
using NHG.Web.Controllers;

namespace NHG.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Code that runs on application startup
            Application["countvisitor"] = 0;
            Application["countonline"] = 0;
            // Register the default hubs route: ~/signalr/hubs
            //RouteTable.Routes.MapHubs();
            Application["dirCache"] = Server.MapPath("~");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                if (Functions.CheckSession(Session["loginid"], "string"))
                {
                    LoginServices service = new LoginServices();
                    service.LogoutHistory(Session["loginid"].ToString(), Session["sessionid"].ToString());

                    //set thuoc tinh de xoa thu muc trong sessiong end
                    PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                    object o = p.GetValue(null, null);
                    FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    object monitor = f.GetValue(o);
                    MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
                    m.Invoke(monitor, new object[] { });
                    //delete folder
                    DirectoryInfo dirRemove = new DirectoryInfo(string.Format("{0}/Profiles/{1}/Temps/{2}_{3}", Application["dirCache"], EncDec.Encrypt(Session["loginid"].ToString()), Session["userid"].ToString(), Session.SessionID));
                    if (dirRemove.Exists)
                        dirRemove.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Log4Net logger = new Log4Net("Global");
                logger.Error(ex);
            }
        }

        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            string _sessionIPAddress = string.Empty;
            string _encryptedString = string.Empty;
            if (HttpContext.Current.Session != null)
            {
                _encryptedString = Convert.ToString(Session["encryptedSession"]);
                byte[] _encodeAsBytes = Convert.FromBase64String(_encryptedString);
                string _decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(_encodeAsBytes);

                char[] _separator = new char[] { '^' };
                if (!string.IsNullOrEmpty(_decryptedString) && !string.IsNullOrWhiteSpace(_decryptedString))
                {
                    string[] _splitStrings = _decryptedString.Split(_separator);
                    if (_splitStrings.Count() > 0)
                    {
                        //string userID = _splitString[0]
                        //string ticks = _splitString[1]
                        //string dummyGuid = _splitString[3]
                        if (_splitStrings[2].Count() > 0)
                        {
                            string[] _userBrowserInfo = _splitStrings[2].Split('~');
                            if (_userBrowserInfo.Count() > 0)
                            {
                                //_sessionBrowserInfo = _userBrowserInfot[0]
                                _sessionIPAddress = _userBrowserInfo[1];
                            }
                        }
                    }
                }

                string _currentUserIPAddress = string.Empty;
                if (string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARD_FOR"]))
                {
                    _currentUserIPAddress = Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    _currentUserIPAddress = Request.ServerVariables["HTTP_X_FORWARD_FOR"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                }

                System.Net.IPAddress result;
                if (!System.Net.IPAddress.TryParse(_currentUserIPAddress, out result))
                {
                    result = System.Net.IPAddress.None;
                }

                if (!string.IsNullOrEmpty(_sessionIPAddress) && !string.IsNullOrWhiteSpace(_sessionIPAddress))
                {
                    //xac thuc trinh duyet web
                    //string _currentBrowserInfo = Request.Browser.Browser + Request.Browser.Version + Request.UserAgent
                    if (_sessionIPAddress != _currentUserIPAddress)
                    {
                        Session.RemoveAll();
                        Session.Clear();
                        Session.Abandon();
                        Response.Cookies["ASP.NET_SessionID"].Expires = DateTime.Now.AddDays(-30);
                        Response.Cookies.Add(new HttpCookie("ASP_NET_SessionID", ""));
                        Response.Redirect(CommonHelper.GetLoginPageURL());
                    }
                    else
                    {
                        //user is valid
                    }
                }
            }
        }
    }
}
