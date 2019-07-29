using NHG.Core;
using NHG.Core.Functions;
using NHG.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace NHG.Web.Controllers
{
    public class CommonHelper
    {
        static Log4Net _logger = new Log4Net(typeof(CommonHelper));

        public static string CreateFirstProfileUser()
        {
            string strPath = "";
            try
            {
                strPath = HttpContext.Current.Request.PhysicalApplicationPath + string.Format(ResourcePathUrl.FolderUser, EncDec.Encrypt(HttpContext.Current.Session["loginid"].ToString()));
                if (!Directory.Exists(strPath))
                {
                    //tao cau truc thu muc profile
                    Directory.CreateDirectory(strPath);
                    //tao file index.html trong
                    FunctionsFile.WriteFile(strPath + "index.html", "");
                    //tao thu muc config
                    Directory.CreateDirectory(strPath + "Config/");
                    //tao file index.html trong
                    FunctionsFile.WriteFile(strPath + "Config/index.html", "");
                    //tao thu muc uploads
                    Directory.CreateDirectory(strPath + "Uploads/");
                    //tao file index.html trong
                    FunctionsFile.WriteFile(strPath + "Uploads/index.html", "");
                    //tao thu muc uploads
                    Directory.CreateDirectory(strPath + "Temps/");
                    //tao file index.html trong
                    FunctionsFile.WriteFile(strPath + "Temps/index.html", "");
                }
                _logger.Info(strPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return strPath;
        }

        public static string GetFolderProfileUser()
        {
            string strPath = "";
            try
            {
                strPath = HttpContext.Current.Request.PhysicalApplicationPath + string.Format(ResourcePathUrl.Folder_Temp_User, EncDec.Encrypt(HttpContext.Current.Session["loginid"].ToString()), HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session.SessionID);
                _logger.Info(strPath);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return strPath;
        }

        public static string CreateFolderTempControllerProfile(string controllerName)
        {
            string strPath = "";
            try
            {
                string strProfileUser = string.Format(ResourcePathUrl.Folder_Temp_User, EncDec.Encrypt(HttpContext.Current.Session["loginid"].ToString()), HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session.SessionID);
                strPath = Functions.MapPath(string.Format("/{0}Controllers/{1}/", strProfileUser, controllerName));

                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

                _logger.Info(strPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return strPath;
        }

        /// <summary>
        /// Gets login page URL
        /// </summary>
        /// <returns>Login page URL</returns>
        public static string GetLoginPageURL()
        {
            return GetLoginPageURL(string.Empty);
        }

        /// <summary>
        /// Gets login page URL
        /// </summary>
        /// <param name="ReturnUrl">Return url</param>
        /// <returns>Login page URL</returns>
        public static string GetLoginPageURL(string ReturnUrl)
        {
            string redirectUrl = string.Empty;
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                redirectUrl = string.Format(CultureInfo.InvariantCulture, "{0}Account/Login?pageUrl={1}",
                    CommonHelper.GetStoreLocation(),
                    HttpUtility.UrlEncode(ReturnUrl));
            }
            else
            {
                redirectUrl = string.Format(CultureInfo.InvariantCulture, "{0}Account/Login",
                    CommonHelper.GetStoreLocation());
            }
            return redirectUrl;
        }

        /// <summary>
        /// Gets login page URL
        /// </summary>
        /// <param name="AddCurrentPageURL">A value indicating whether add current page url as "ReturnURL" parameter</param>
        /// <returns>Login page URL</returns>
        public static string GetLoginPageURL(bool AddCurrentPageURL)
        {
            return GetLoginPageURL(AddCurrentPageURL, false);
        }

        /// <summary>
        /// Gets login page URL
        /// </summary>
        /// <param name="AddCurrentPageURL">A value indicating whether add current page url as "ReturnURL" parameter</param>
        /// <param name="CheckoutAthanhdoanest">A value indicating whether login page will show "Checkout as a guest or Register" message</param>
        /// <returns>Login page URL</returns>
        public static string GetLoginPageURL(bool AddCurrentPageURL, bool CheckoutAthanhdoanest)
        {
            string redirectUrl = string.Empty;
            if (AddCurrentPageURL)
            {
                redirectUrl = string.Format(CultureInfo.InvariantCulture, "{0}Account/Login?pageUrl={1}",
                    CommonHelper.GetStoreLocation(),
                    HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
            }
            else
            {
                redirectUrl = GetLoginPageURL();
            }

            if (CheckoutAthanhdoanest)
            {
                redirectUrl = ModifyQueryString(redirectUrl, "CheckoutAthanhdoanest=true", string.Empty);
            }
            return redirectUrl;
        }

        /// <summary>
        /// Modifies query string
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryStringModification">Query string modification</param>
        /// <param name="targetLocationModification">Target location modification</param>
        /// <returns>New url</returns>
        public static string ModifyQueryString(string url, string queryStringModification, string targetLocationModification)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in queryStringModification.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new char[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }
                    StringBuilder builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }
            if (!string.IsNullOrEmpty(targetLocationModification))
            {
                str2 = targetLocationModification;
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2)));
        }

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <returns>Store location</returns>
        public static string GetStoreLocation()
        {
            bool useSSL = false;
            if (HttpContext.Current != null)
                useSSL = HttpContext.Current.Request.IsSecureConnection;
            return GetStoreLocation(useSSL);
        }

        /// <summary>
        /// Gets home page URL
        /// </summary>
        /// <returns>home page URL</returns>
        public static string GetHomePageURL()
        {
            return CommonHelper.GetStoreLocation();
        }


        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="UseSSL">Use SSL</param>
        /// <returns>Store location</returns>
        public static string GetStoreLocation(bool UseSSL)
        {
            string result = GetStoreHost(UseSSL);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            result = result + HttpContext.Current.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="UseSSL">Use SSL</param>
        /// <returns>Store host location</returns>
        public static string GetStoreHost(bool UseSSL)
        {
            string result = "http://" + ServerVariables("HTTP_HOST");
            if (!result.EndsWith("/"))
                result += "/";

            if (UseSSL)
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SharedSSL"]))
                {
                    result = ConfigurationManager.AppSettings["SharedSSL"];
                }
                else
                {
                    result = result.Replace("http:/", "https:/");
                }
            }

            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string Name)
        {
            string tmpS = String.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables[Name] != null)
                {

                    tmpS = HttpContext.Current.Request.ServerVariables[Name].ToString();

                }
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }
    }
}