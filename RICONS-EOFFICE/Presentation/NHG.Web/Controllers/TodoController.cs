using NHG.Logger;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class TodoController : BaseController
    {
        #region Fields
        Log4Net _logger = new Log4Net(typeof(MilestonesController));
        #endregion

        //
        // GET: /Todo/
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpPost]
        public ActionResult Create(string data)
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpPost]
        public JsonResult SelectRows()
        {
            StringBuilder result = new StringBuilder();
            StringBuilder lstRow = new StringBuilder();
            for (int i = 0; i < 50; i++)
            {
                lstRow.Append(PrepareDataJson());
            }
            if (lstRow.Length > 0)
                lstRow.Remove(lstRow.Length - 1, 1);
            result.Append("{");
            result.Append("\"isHeader\":\"" + "111" + "\",");
            result.Append("\"Pages\":\"" + "212" + "\",");
            result.Append("\"data\":[" + lstRow.ToString() + "]");
            result.Append("}");

            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        private StringBuilder PrepareDataJson()
        {
            StringBuilder sbResult = new StringBuilder();
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + "323" + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "style" + "\", \"value\":\"background-color:" + "434" + "\"}],");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //colum checkbox
                string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}'/>", "43434");
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                sbResult.Append("},");
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1 stt\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + "32323" + "\"");
                sbResult.Append("},");
                //noi dung
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"title\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + "32323" + "\"");
                sbResult.Append("},");
                //nguoi thuc hien
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"title\":\"" + "323" + "\",");
                sbResult.Append("\"col_value\":\"" + "3232" + "\"");
                sbResult.Append("},");
                //tinh trang           
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"title\":\"" + "323" + "\",");
                sbResult.Append("\"col_value\":\"" + "323" + "\"");
                sbResult.Append("},");
                //ngay nhap
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + "2323" + "\"");
                sbResult.Append("},");
                //dinh kem tap tin
                string strHTML_Attachment = "";
                #region
                strHTML_Attachment = "<i class='attach_file' ></i>";
                #endregion
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Attachment + "\"");
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
	}
}