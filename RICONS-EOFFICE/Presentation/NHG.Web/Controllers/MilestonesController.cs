using NHG.Core.Functions;
using NHG.DataServices.DataClass;
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
    public class MilestonesController : BaseController
    {
        #region Fields
        Log4Net _logger = new Log4Net(typeof(MilestonesController));
        #endregion

       public static string makehoach = "";

        //
        // GET: /Milestones/
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        public ActionResult Create()
        {
            if (!IsLogged())
                return BackToLogin();
            KeHoachServices serKeHoach = new KeHoachServices();
            TaiKhoanServices serTaiKhoan = new TaiKhoanServices();
            StringBuilder sbHeHoach = new StringBuilder();
            StringBuilder sbNguoiThucHien = new StringBuilder();
            List<KeHoachForCombobox> lstKeHoachForCombobox = serKeHoach.SelectKeHoachForCombobox(new KeHoachModels());
            var keHoachGroup =
                    from p in lstKeHoachForCombobox
                    where p.makehoachgoc == "0"
                    select p;
            foreach (var item in keHoachGroup)
            {
                string strPref = "";
                sbHeHoach.Append(string.Format("<option value='{0}'>{1}</option>", item.maKeHoach, item.noidungmuctieu));
                var subItem =
                    from p in lstKeHoachForCombobox
                    where p.makehoachgoc == item.maKeHoach
                    select p;
                if (subItem.Count() > 0)
                {
                    sbHeHoach.Append(PrefAppendCombobox(lstKeHoachForCombobox, item.maKeHoach, ref strPref));
                }
            }

            foreach (var item in serTaiKhoan.SelectTaiKhoanForCombobox(new M_TaiKhoan()))
            {
                sbNguoiThucHien.Append(string.Format("<option value='{0}'>{1}</option>", item.mataikhoan, item.hoten));
            }
            ViewBag.keHoachs = sbHeHoach.ToString();
            ViewBag.nguoiThucHiens = sbNguoiThucHien.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Create(KeHoachModels model)
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            #region Set Param
            M_KeHoach clParamSuKien = new M_KeHoach();
            clParamSuKien.noidungmuctieu = model.noidungmuctieu;
            clParamSuKien.ghichu = model.ghichu;
            clParamSuKien.makehoach = makehoach;
            clParamSuKien.ngaybatdau = FunctionsDateTime.ConvertDate(FunctionsDateTime.ConvertStringToDate(model.ngaybatdau));
            clParamSuKien.ngayketthuc = FunctionsDateTime.ConvertDate(FunctionsDateTime.ConvertStringToDate(model.ngayketthuc));
            clParamSuKien.nguoitao = Session["userid"].ToString();
            clParamSuKien.nguoithuchien = model.nguoithuchien;
            clParamSuKien.makehoachgoc = model.makehoachgoc;
            clParamSuKien.tytrong = model.tytrong;
            clParamSuKien.chitieunam = model.chitieunam;
            clParamSuKien.ngaytao = "GETDATE()";
            clParamSuKien.nguoihieuchinh = Session["userid"].ToString();
            clParamSuKien.ngayhieuchinh = "GETDATE()";
            if (GetPhongBanDonVi() != null)
            {
                clParamSuKien.madonvi = GetPhongBanDonVi().madonvi;
            }
            #endregion

            KeHoachServices services = new KeHoachServices();

            if(makehoach.Trim()=="")
                services.InsertRow(clParamSuKien, Session["manhansu"].ToString());
            else
                services.UpdateRow(clParamSuKien);
            makehoach = "";
            return RedirectToAction("Index", "Milestones");
        }

        public ActionResult Edit(string makh)
        {
            if (!IsLogged())
                return BackToLogin();
            KeHoachServices serKeHoach = new KeHoachServices();
            TaiKhoanServices serTaiKhoan = new TaiKhoanServices();
            StringBuilder sbHeHoach = new StringBuilder();
            StringBuilder sbNguoiThucHien = new StringBuilder();
            List<KeHoachForCombobox> lstKeHoachForCombobox = serKeHoach.SelectKeHoachForCombobox(new KeHoachModels());
            var keHoachGroup =
                    from p in lstKeHoachForCombobox
                    where p.makehoachgoc == "0"
                    select p;
            foreach (var item in keHoachGroup)
            {
                string strPref = "";
                sbHeHoach.Append(string.Format("<option value='{0}'>{1}</option>", item.maKeHoach, item.noidungmuctieu));
                var subItem =
                    from p in lstKeHoachForCombobox
                    where p.makehoachgoc == item.maKeHoach
                    select p;
                if (subItem.Count() > 0)
                {
                    sbHeHoach.Append(PrefAppendCombobox(lstKeHoachForCombobox, item.maKeHoach, ref strPref));
                }
            }

            foreach (var item in serTaiKhoan.SelectTaiKhoanForCombobox(new M_TaiKhoan()))
            {
                sbNguoiThucHien.Append(string.Format("<option value='{0}'>{1}</option>", item.mataikhoan, item.hoten));
            }
            ViewBag.keHoachs = sbHeHoach.ToString();
            ViewBag.nguoiThucHiens = sbNguoiThucHien.ToString();

            #region Set Param
            M_KeHoach param = new M_KeHoach();
            string strMaKeHoach = "0";
            if (makh != "0" && makh!=null)
            {
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                strMaKeHoach = AES.DecryptText(makh, function.ReadXMLGetKeyEncrypt());
            }
            if (GetPhongBanDonVi() != null)
            {
                param.madonvi = GetPhongBanDonVi().madonvi;
                param.makehoach = strMaKeHoach;
            }
            #endregion
            makehoach = strMaKeHoach;
            KeHoachServices service = new KeHoachServices();
            List<KeHoachModels> lstResult = service.SelectRows(param);
            if (lstResult.Count > 0)
                return View(lstResult[0]);
            return View();
        }


        public ActionResult Deleted(string makh)
        {
            if (!IsLogged())
                return BackToLogin();
            string strMaKeHoach = "0";

            if (makh != "0" && makh != null)
            {
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                strMaKeHoach = AES.DecryptText(makh, function.ReadXMLGetKeyEncrypt());
            }
            else
            {
                
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpGet]
        public ActionResult Report1()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        #region Utilities
        /// <summary>
        /// LayDanhSach
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SelectRows(KeHoachModels model)
        {
            //lay phong ban don vi
            #region Set Param
            M_KeHoach param = new M_KeHoach();
            string strMaKeHoach = "0";
            if (model.makehoach != "0")
            {
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                strMaKeHoach = AES.DecryptText(model.makehoach, function.ReadXMLGetKeyEncrypt());
            }
            if (GetPhongBanDonVi() != null)
            {
                param.madonvi = GetPhongBanDonVi().madonvi;
                param.makehoachgoc = strMaKeHoach;
            }
            #endregion

            KeHoachServices service = new KeHoachServices();
            List<KeHoachModels> lstResult = service.SelectRows(param);
            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            if (lstResult.Count > 0)
            {
                string strSTT = "";
                int i = 1;

                foreach (var item in lstResult)
                {
                    bool isPlus = false;
                    param.makehoachgoc = item.makehoach;
                    if (service.CountRows(param) > 0)
                        isPlus = true;
                    if (model.stt == "0")
                    {
                        strSTT = i.ToString();
                    }
                    else
                    {
                        strSTT = string.Format("{0}.{1}", model.stt, i);
                    }
                    sbRows.Append(PrepareDataJson(item, strSTT, isPlus));
                    i++;
                }
                if (sbRows.Length > 0)
                    sbRows.Remove(sbRows.Length - 1, 1);
            }
            sbResult.Append("{");
            sbResult.Append("\"isHeader\":\"" + "111" + "\",");
            sbResult.Append("\"Pages\":\"" + "212" + "\",");
            if (model.makehoach != "0")
            {
                sbResult.Append("\"SubRow\":\"" + "true" + "\",");
                sbResult.Append("\"RowID\":\"" + model.makehoach + "\",");
            }
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region method
        private StringBuilder PrepareDataJson(KeHoachModels model, string couter, bool isPlus)
        {
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.makehoach, function.ReadXMLGetKeyEncrypt());
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.makehoachgoc, function.ReadXMLGetKeyEncrypt()) + "\"}],");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //colum checkbox
                string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}' mkh='" + strEncryptCode + "'/>", model.makehoach);
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                sbResult.Append("},");
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + couter + "\"");
                sbResult.Append("},");
                //noi dung
                string strNoiDUng = "";
                string strPref = "";
                if (isPlus)
                    strNoiDUng += "<i class='fa fa-plus' onclick='ShowSubLine(this);' mkh='" + strEncryptCode + "'></i>";
                if (couter.IndexOf('.') > -1)
                {
                    string[] capDo = couter.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in capDo)
                    {
                        if (capDo.Length < 3)
                            strPref += "&nbsp;&nbsp;&nbsp;&nbsp;";
                        else strPref += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                }

                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + strPref + strNoiDUng + "<a href='" + Url.Action("Edit", "Milestones", new { makh = strEncryptCode }) + "' title='" + model.noidungmuctieu + "'>" + model.noidungmuctieu + "</a>\"");
                sbResult.Append("},");

                //ty trọng
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.tytrong + "\"");
                sbResult.Append("},");

                //Chi tieu nam
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.chitieunam + "\"");
                sbResult.Append("},");

                //tháng 7
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.t7 + "\"");
                sbResult.Append("},");
                //thang 8
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"8\",");
                sbResult.Append("\"col_value\":\"" + model.t8 + "\"");
                sbResult.Append("},");
                //Thang 9 
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"9\",");
                sbResult.Append("\"col_value\":\"" + model.t9 + "\"");
                sbResult.Append("},");

                //Thang 10 
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"10\",");
                sbResult.Append("\"col_value\":\"" + model.t10 + "\"");
                sbResult.Append("},");

                //Thang 11 
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"11\",");
                sbResult.Append("\"col_value\":\"" + model.t11 + "\"");
                sbResult.Append("},");

                //Thang 12
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"12\",");
                sbResult.Append("\"col_value\":\"" + model.t12 + "\"");
                sbResult.Append("},");

                //tháng 1
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"1\",");
                sbResult.Append("\"col_value\":\"" + model.t1 + "\"");
                sbResult.Append("},");
                //thang 2
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"2\",");
                sbResult.Append("\"col_value\":\"" + model.t2 + "\"");
                sbResult.Append("},");
                //Thang 3
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"3\",");
                sbResult.Append("\"col_value\":\"" + model.t3 + "\"");
                sbResult.Append("},");

                //Thang 4
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"4\",");
                sbResult.Append("\"col_value\":\"" + model.t4 + "\"");
                sbResult.Append("},");

                //Thang 5 
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"5\",");
                sbResult.Append("\"col_value\":\"" + model.t5 + "\"");
                sbResult.Append("},");

                ////nguoi thuc hien
                //sbResult.Append("{");
                //sbResult.Append("\"colspan\":\"1\",");
                //sbResult.Append("\"col_class\":\"ovh col4\",");
                //sbResult.Append("\"col_id\":\"6\",");
                //sbResult.Append("\"title\":\"" + model.nguoithuchien + "\",");
                //sbResult.Append("\"col_value\":\"" + model.nguoithuchien + "\"");
                //sbResult.Append("},");

                //tháng 6
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.t6 + "\"");
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

        private void PrefAppend(List<KeHoachModels> lstSource, string maKeHoachGoc, ref string append)
        {
            var rows =
                    (from p in lstSource
                     where p.makehoach == maKeHoachGoc
                     select p.makehoachgoc).Single();
            if (rows != "0")
            {
                append += "&nbsp;&nbsp;&nbsp;&nbsp;";
                PrefAppend(lstSource, rows, ref append);
            }
        }

        private StringBuilder PrefAppendCombobox(List<KeHoachForCombobox> lstSource, string maKeHoachGoc, ref string append)
        {
            StringBuilder strResult = new StringBuilder();
            if (lstSource.Count > 0)
            {
                var lstTemp =
                from n in lstSource
                where n.makehoachgoc.Trim(' ') == maKeHoachGoc.Trim(' ')
                select n;
                append += " --- ";
                foreach (var item in lstTemp)
                {
                    strResult.AppendFormat("<option value='{0}'>{1}{2}</option>", item.maKeHoach, append, item.noidungmuctieu);
                    strResult.Append(PrefAppendCombobox(lstSource, item.maKeHoach, ref append));
                }
                return strResult;
            }
            return new StringBuilder();
        }

        #endregion
    }
}