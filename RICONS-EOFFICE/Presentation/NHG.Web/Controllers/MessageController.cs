using NHG.Core.Functions;
using NHG.DataServices.DataClass;
using NHG.Logger;
using NHG.Web.Data.Services;
using NHG.Web.Models;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class MessageController : BaseController
    {
        #region Fields
        Log4Net _logger = new Log4Net(typeof(MessageController));
        #endregion

        #region action
        //
        // GET: /Message/
        public ActionResult Index()
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        public ActionResult Config()
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        public ActionResult QuickAlert()
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            ThongBaoServices service = new ThongBaoServices();
            List<ThongBaoModels> lstResult = service.SelectQuickAlerts(new ThongBaoModels() { nguoinhan = Session["userid"].ToString() });
            return PartialView(lstResult);
        }

        public ActionResult Alerts()
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            return View();
        }
        #endregion

        #region Utilities
        public JsonResult SelectAlerts(ThongBaoModels model)
        {
            int iCurrentPage =int.Parse(model.curentPage);
            
            //check login
            if (!IsLogged())
                return null;
            ThongBaoServices service = new ThongBaoServices();
            List<ThongBaoModels> lstResult = service.SelectRows(new M_ThongBao() { 
                noidung = model.noidung,
                limit = base.defaultConst.ReadConst("paging", 0).ToString(),
                offset = model.curentPage
            });
            int soluongTong = service.CountRows(new M_ThongBao() {
                noidung = model.noidung
            });
            StringBuilder result = new StringBuilder();
            StringBuilder lstRow = new StringBuilder();
            if (lstResult.Count > 0)
            {
                int i = 1;
                if (iCurrentPage > 0)
                    i = iCurrentPage + 1;
                foreach (var item in lstResult)
                {
                    lstRow.Append(PrepareDataJsonForSelectAlerts(item, i));
                    i++;
                }
                if (lstRow.Length > 0)
                    lstRow.Remove(lstRow.Length - 1, 1);
            }
            result.Append("{");
            result.Append("\"isHeader\":\"" + "111" + "\",");
            result.Append("\"curentPage\":\"" + (iCurrentPage + lstResult.Count) + "\",");
            result.Append("\"Pages\":\"" + soluongTong + "\",");
            result.Append("\"data\":[" + lstRow.ToString() + "]");
            result.Append("}");
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Method
        private StringBuilder PrepareDataJsonForSelectAlerts(ThongBaoModels model, int couter)
        {
            StringBuilder sbResult = new StringBuilder();
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + model.mathongbao + "\",");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1 stt\",");
                sbResult.Append("\"col_value\":\"" + couter.ToString() + "\"");
                sbResult.Append("},");
                //noi dung
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2\",");
                sbResult.Append("\"title\":\"" + model.noidung + "\",");
                sbResult.Append("\"col_value\":\"" + model.noidung + "\"");
                sbResult.Append("},");
                //ngay nhap
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_value\":\"" + FunctionsDateTime.GetDateTimeClientCustomFormat(model.ngaytao) + "\"");
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