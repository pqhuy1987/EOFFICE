using NHG.Web.Models;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using NHG.Logger;
using System.Text;
using NHG.Web.Data.Services;
using System.Xml.Linq;
using NHG.Core.Functions;
using NHG.DataServices.DataClass;

namespace NHG.Web.Controllers
{
    public class SystemController : BaseController
    {
        Log4Net _logger = new Log4Net(typeof(SystemController));

        #region Action
        //
        // GET: /System/
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        //
        // GET: /SystemInfo/
        public ActionResult SystemInfo()
        {
            if (!IsLogged())
                return BackToLogin();
            var model = new SystemInfoModel();
            try
            {
                model.OperatingSystem = Environment.OSVersion.VersionString;
            }
            catch (Exception ex) {
                _logger.Error(ex);
            }
            try
            {
                model.AspNetInfo = RuntimeEnvironment.GetSystemVersion();
            }
            catch (Exception ex) {
                _logger.Error(ex);
            }
            try
            {
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted.ToString();
            }
            catch (Exception ex) {
                _logger.Error(ex);
            }
            model.ServerTimeZone = TimeZone.CurrentTimeZone.StandardName;
            model.ServerLocalTime = DateTime.Now.ToString("F");
            model.UtcTime = DateTime.UtcNow.ToString("F");
            model.HttpHost = this.Request.ServerVariables["HTTP_HOST"];
            ////Environment.GetEnvironmentVariable("USERNAME");
            //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    model.LoadedAssemblies.Add(new SystemInfoModel.LoadedAssembly
            //    {
            //        FullName = assembly.FullName,
            //        //we cannot use Location property in medium trust
            //        //Location = assembly.Location
            //    });
            //}
            return View(model);
        }

        public ActionResult SystemLog()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpGet]
        public ActionResult EmailSystem()
        {
            if (!IsLogged())
                return BackToLogin();
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkey.config"));
            //lay key da ma hoa
            string strKeyEncrypt = EncDec.Decrypt(function.ReadXMLGetKeyEncrypt());

            EmailSystem model = new Models.EmailSystem();
            #region doc thong tin chung (chuc vu, nguoi ky, )
            XElement doc = XElement.Load(Functions.MapPath("~/Xml/Config/email.xml"));
            var list = doc.Elements();
            model.host = list.ElementAt(0).Value;
            model.port = list.ElementAt(1).Value;
            model.user = AES.DecryptText(list.ElementAt(2).Value, strKeyEncrypt);
            model.pass = AES.DecryptText(list.ElementAt(3).Value, strKeyEncrypt);
            model.displayName = list.ElementAt(4).Value;
            if (list.ElementAt(5).Value == "1")
                model.enableSSL = true;
            else
                model.enableSSL = false;
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult EmailSystem(EmailSystem model)
        {
            if (!IsLogged())
                return BackToLogin();
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkey.config"));
            //lay key da ma hoa
            string strKeyEncrypt = EncDec.Decrypt(function.ReadXMLGetKeyEncrypt());
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XProcessingInstruction("test", "value"),
                new XComment(String.Format("Ngày {0}   author: {1}", DateTime.Now, Session["userid"])),

                new XElement("email",
                    new XElement("smtp_host", model.host),
                    new XElement("smtp_port", model.port),
                    new XElement("smtp_user", AES.EncryptText(model.user, strKeyEncrypt)),
                    new XElement("smtp_pass", AES.EncryptText(model.pass, strKeyEncrypt)),
                    new XElement("displayName", model.displayName),
                    new XElement("enableSSL", model.enableSSL))
            );

            doc.Save(Functions.MapPath("~/Xml/Config/email.xml"));
            return View();
        }
        #endregion

        #region ajax request
        [HttpPost]
        public ActionResult ListLog(string id = "")
        {
            if (!IsLogged())
                return BackToLogin();
            LogServices service = new LogServices();
            List<LogModels> lstResult = service.SelectRows(new M_Log() { loglevel = id});
            StringBuilder result = new StringBuilder();
            StringBuilder lstRow = new StringBuilder();
            if (lstResult.Count > 0)
            {
                int i = 1;
                foreach (var item in lstResult)
                {
                    lstRow.Append(PrepareDataJsonForListLog(item));
                    i++;
                }
                if (lstRow.Length > 0)
                    lstRow.Remove(lstRow.Length - 1, 1);
            }
            result.Append("{");
            result.Append("\"isHeader\":\"" + "111" + "\",");
            result.Append("\"Pages\":\"" + "212" + "\",");
            result.Append("\"data\":[" + lstRow.ToString() + "]");
            result.Append("}");
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Method
        private StringBuilder PrepareDataJsonForListLog(LogModels model)
        {
            StringBuilder sbResult = new StringBuilder();
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + "323" + "\",");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1 stt\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.stt + "\"");
                sbResult.Append("},");
                //noi dung
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"title\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + model.NoiDung + "\"");
                sbResult.Append("},");
                //cap do
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"title\":\"" + "323" + "\",");
                sbResult.Append("\"col_value\":\"" + model.CapDo + "\"");
                sbResult.Append("},");
                //ngay nhap
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.NgayTao + "\"");
                sbResult.Append("}");
                
                #endregion

                sbResult.Append("]");
                sbResult.Append("},");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return sbResult;
        }
        #endregion
    }
}