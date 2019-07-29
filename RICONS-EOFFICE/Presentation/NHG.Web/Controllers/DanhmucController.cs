using NHG.Core.Functions;
using NHG.DataServices.DataClass;
using NHG.Logger;
using NHG.Web.Data.Services;
using NHG.Web.Models;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace NHG.Web.Controllers
{
    public class DanhmucController : BaseController
    {
        public static string maphongban = "";
        string ngaytao = "GETDATE()";
        #region Fields
        Log4Net _logger = new Log4Net(typeof(DanhmucController));
        #endregion
        // GET: /Danhmuc phong ban/
        public static List<thongtingiamdocModels> lstthongtingiamdoc = new List<thongtingiamdocModels>();
        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();


            DanhmucServices service = new DanhmucServices();

            thongtingiamdocModels param = new thongtingiamdocModels();

            lstthongtingiamdoc = service.SelectRows_thongtingiamdoc(param);
            StringBuilder sbTendonvi = new StringBuilder();
            foreach (var item in lstthongtingiamdoc)
            {
                sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", item.mathongtin, item.hovaten));
            }

           

            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "1", "Lê Miên Thụy"));
            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "2", "Trần Kim Long"));
            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "3", "Phạm Quân Lực"));
            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "4", "Nguyễn Thành Tâm"));
            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "5", "Nguyễn Văn Út"));
            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "6", "Trương Hoài Nam"));
            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "7", "Nguyễn Phi Khâm"));
            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "8", "Lê Văn Nhựt"));

            //sbTendonvi.Append(string.Format("<option value='{0}'>{1}</option>", "8", "Test"));

            ViewBag.keHoachs = sbTendonvi.ToString();
            return View();
        }
        [HttpPost]
        public JsonResult SelectRows(PhongBanModels model)
        {
            PhongBanModels param = new PhongBanModels();
            param.tenphongban = model.tenphongban;
            DanhmucServices service = new DanhmucServices();
            List<PhongBanModels> lstResult = service.SelectRows(param);

            foreach (var itemrow in lstResult)
            {
                var giamdoc = lstthongtingiamdoc.Where(p => p.mathongtin.Trim() == itemrow.thuocquanly.Trim()).ToList();
                if (giamdoc.Count > 0)
                {
                    itemrow.xoa = giamdoc[0].hovaten.Trim();
                }

                //if (itemrow.thuocquanly.Trim() == "1")
                //    itemrow.xoa = "Lê Miên Thụy";
                //else if (itemrow.thuocquanly.Trim() == "2")
                //    itemrow.xoa = "Trần Kim Long";
                //else if (itemrow.thuocquanly.Trim() == "3")
                //    itemrow.xoa = "Phạm Quân Lực";
                //else if (itemrow.thuocquanly.Trim() == "4")
                //    itemrow.xoa = "Nguyễn Thành Tâm";
                //else if (itemrow.thuocquanly.Trim() == "5")
                //    itemrow.xoa = "Nguyễn Văn Út";
                //else if (itemrow.thuocquanly.Trim() == "6")
                //    itemrow.xoa = "Trương Hoài Nam";
                //else if (itemrow.thuocquanly.Trim() == "7")
                //    itemrow.xoa = "Nguyễn Phi Khâm";
                //else if (itemrow.thuocquanly.Trim() == "8") //nhut.le@ricons.vn
                //    itemrow.xoa = "Lê Văn Nhựt";
            }

            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            if (lstResult.Count > 0)
            {
                string strSTT = "";
                int i = 1;

                foreach (var item in lstResult)
                {
                    strSTT = i.ToString();

                    if (item.madonvi == "1")
                        item.tendonvi = "Công ty cổ phẩn Ricons";
                    sbRows.Append(PrepareDataJson(item, strSTT));
                    i++;
                }
                if (sbRows.Length > 0)
                    sbRows.Remove(sbRows.Length - 1, 1);
            }
            sbResult.Append("{");
            sbResult.Append("\"curentPage\":\"" + lstResult.Count + "\",");
            sbResult.Append("\"isHeader\":\"" + "111" + "\",");
            sbResult.Append("\"Pages\":\"" + "212" + "\",");
            if (model.maphongban != "0")
            {
                sbResult.Append("\"SubRow\":\"" + "true" + "\",");
                sbResult.Append("\"RowID\":\"" + model.maphongban + "\",");
            }
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        private StringBuilder PrepareDataJson(PhongBanModels model, string couter)
        {
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.maphongban, function.ReadXMLGetKeyEncrypt());
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.maphongban, function.ReadXMLGetKeyEncrypt()) + "\"}],");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //colum checkbox
                string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}' mpb='" + strEncryptCode + "'/>", model.maphongban);
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1\",");
                sbResult.Append("\"col_id\":\"1\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                sbResult.Append("},");
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                sbResult.Append("\"col_id\":\"2\",");
                sbResult.Append("\"col_value\":\"" + couter + "\"");
                sbResult.Append("},");

                //Mã đơn vị
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"3\",");
                sbResult.Append("\"col_value\":\"" + model.maphongban.Trim() + "\"");
                sbResult.Append("},");

               //Mã phòng ban
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"4\",");
                sbResult.Append("\"col_value\":\"" + model.tenphongban.Trim() + "\"");
                sbResult.Append("},");

                //ho va ten
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"5\",");
                sbResult.Append("\"col_value\":\"" + model.hovaten.Trim() + "\"");
                sbResult.Append("},");

                //Email
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.email + "\"");
                sbResult.Append("},");

                //so dien thoai luu thanh email cap quan ly
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col8\",");
                sbResult.Append("\"col_id\":\"8\",");
                sbResult.Append("\"col_value\":\"" + model.sodienthoai.Trim() + "\"");
                sbResult.Append("},");

                //thuoc quan ly
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col7\",");
                sbResult.Append("\"col_id\":\"7\",");
                sbResult.Append("\"title\":\""+model.thuocquanly.Trim()+"\",");
                sbResult.Append("\"col_value\":\"" + model.xoa.Trim() + "\"");
                sbResult.Append("},");

              

                //Ghi chú
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col9\",");
                sbResult.Append("\"col_id\":\"9\",");
                sbResult.Append("\"col_value\":\"" + model.ghichu.Trim() + "\"");
                sbResult.Append("},");


                //dinh kem tap tin
                string strHTML_Attachment = "";
                #region
                //string link = Url.Action("Edit", "Account", new { id = EncDec.EncodeCrypto(model.mataikhoan) });
                strHTML_Attachment = "<a href='#' class='edit' ><i class='fa fa-pencil-square-o' ></i></a>&nbsp;<a href='#' class='del'><i class='fa fa-trash-o' ></i></a>";
                #endregion
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col10\",");
                sbResult.Append("\"col_id\":\"10\",");
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

        [HttpPost]
        public ActionResult Updatephongban(string act, string ID, PhongBanModels model)
        {
            if (!IsLogged())
                return BackToLogin();
            if (act == "create")
            {
                PhongBanModels paramphongban = new PhongBanModels();
                paramphongban.maphongban = "";
                paramphongban.madonvi = "1";
                paramphongban.tenphongban = model.tenphongban;
                paramphongban.sodienthoai = model.sodienthoai;

                paramphongban.email = model.email;
                paramphongban.hovaten = model.hovaten;
                paramphongban.thuocquanly = model.thuocquanly;
                paramphongban.ghichu = model.ghichu;
                paramphongban.xoa = "0";
                paramphongban.nguoitao = int.Parse(Session["userid"].ToString());
                paramphongban.ngaytao = ngaytao;
                DanhmucServices services = new DanhmucServices();
                string result = services.Insert_phongban(paramphongban);
                maphongban = result;
            }
            else if (act == "update")
            {
                PhongBanModels paramphongban = new PhongBanModels();
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                paramphongban.maphongban = AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt());
                paramphongban.madonvi = "1";
                paramphongban.tenphongban = model.tenphongban;
                paramphongban.sodienthoai = model.sodienthoai;

                paramphongban.email = model.email;
                paramphongban.hovaten = model.hovaten;
                paramphongban.thuocquanly = model.thuocquanly;

                paramphongban.ghichu = model.ghichu;
                paramphongban.xoa = "0";
                paramphongban.nguoitao = int.Parse(Session["userid"].ToString());
                paramphongban.ngaytao = ngaytao;
                DanhmucServices services = new DanhmucServices();
                string result = services.Insert_phongban(paramphongban);
                maphongban = result;
            }
            else if (act == "delete")
            {
                PhongBanModels paramphongban = new PhongBanModels();
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                paramphongban.maphongban = AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt());
                paramphongban.nguoihieuchinh = int.Parse(Session["userid"].ToString());
                DanhmucServices services = new DanhmucServices();
                string result = services.Deleted_phongban(paramphongban.maphongban.ToString(), paramphongban.nguoihieuchinh.ToString());
                maphongban = result;
            }
            return RedirectToAction("Index", "Danhmuc");
        }

        //// Danh muc ngày lễ

        public ActionResult Cate_holiday()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpPost]
        public JsonResult SelectRows_holiday(HolidayModels model)
        {
            HolidayModels param = new HolidayModels();
            DanhmucServices service = new DanhmucServices();
            List<HolidayModels> lstResult = service.SelectRows_holiday(param);

            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            if (lstResult.Count > 0)
            {
                string strSTT = "";
                int i = 1;
                foreach (var item in lstResult)
                {
                    strSTT = i.ToString();
                    if (item.datevalue.ToString().Length>0)
                        item.datevalue = item.datevalue.ToString().Split(' ')[0];

                    sbRows.Append(PrepareDataJson_holiday(item, strSTT));
                    i++;
                }
                if (sbRows.Length > 0)
                    sbRows.Remove(sbRows.Length - 1, 1);
            }
            sbResult.Append("{");
            sbResult.Append("\"curentPage\":\"" + lstResult.Count + "\",");
            sbResult.Append("\"isHeader\":\"" + "111" + "\",");
            sbResult.Append("\"Pages\":\"" + "212" + "\",");
            if (model.id != 0)
            {
                sbResult.Append("\"SubRow\":\"" + "true" + "\",");
                sbResult.Append("\"RowID\":\"" + model.id + "\",");
            }
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        private StringBuilder PrepareDataJson_holiday(HolidayModels model, string couter)
        {
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.id.ToString(), function.ReadXMLGetKeyEncrypt());
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.id.ToString(), function.ReadXMLGetKeyEncrypt()) + "\"}],");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //colum checkbox
                string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}' mangaynghi='" + strEncryptCode + "'/>", model.id.ToString());
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1\",");
                sbResult.Append("\"col_id\":\"1\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                sbResult.Append("},");
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                sbResult.Append("\"col_id\":\"2\",");
                sbResult.Append("\"col_value\":\"" + couter + "\"");
                sbResult.Append("},");

                //Mã phòng ban
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"3\",");
                sbResult.Append("\"col_value\":\"" + model.holidayname.Trim() + "\"");
                sbResult.Append("},");

                //ho va ten
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"4\",");
                sbResult.Append("\"col_value\":\"" + model.datevalue.Trim() + "\"");
                sbResult.Append("},");


                //dinh kem tap tin
                string strHTML_Attachment = "";
                #region
                //string link = Url.Action("Edit", "Account", new { id = EncDec.EncodeCrypto(model.mataikhoan) });
                strHTML_Attachment = "<a href='#' class='edit' ><i class='fa fa-pencil-square-o' ></i></a>&nbsp;<a href='#' class='del'><i class='fa fa-trash-o' ></i></a>";
                #endregion
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"5\",");
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

        [HttpPost]
        public ActionResult Update_holiday(string act, string ID, HolidayModels model)
        {
            if (!IsLogged())
                return BackToLogin();
            if (act == "create")
            {
                HolidayModels paramholiday = new HolidayModels();
                paramholiday.holidayname = model.holidayname;
                paramholiday.datevalue = model.datevalue;
                DanhmucServices services = new DanhmucServices();
                //string result = services.Insert_phongban(paramholiday);
                //maphongban = result;
            }
            else if (act == "update")
            {
                HolidayModels paramholiday = new HolidayModels();
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                paramholiday.id = int.Parse(AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt()));
                paramholiday.holidayname = model.holidayname;
                paramholiday.datevalue = model.datevalue;

             
                DanhmucServices services = new DanhmucServices();
                //string result = services.Insert_phongban(paramholiday);
                //maphongban = result;
            }
            else if (act == "delete")
            {
                //PhongBanModels paramphongban = new PhongBanModels();
                //FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                //paramphongban.maphongban = AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt());
                //paramphongban.nguoihieuchinh = int.Parse(Session["userid"].ToString());
                //DanhmucServices services = new DanhmucServices();
                //string result = services.Deleted_phongban(paramphongban.maphongban.ToString(), paramphongban.nguoihieuchinh.ToString());
                //maphongban = result;
            }
            return RedirectToAction("Index", "Danhmuc");
        }


        /// //Danh muc chuc danh chuc vu
        public ActionResult Indexchucdanh()
        {
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpPost]
        public JsonResult SelectRows_chucvu(ChucDanhModels model)
        {
            ChucDanhModels param = new ChucDanhModels();
            DanhmucServices service = new DanhmucServices();
            List<ChucDanhModels> lstResult = service.SelectRows_chucvu(param);
            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            if (lstResult.Count > 0)
            {
                string strSTT = "";
                int i = 1;

                foreach (var item in lstResult)
                {
                    strSTT = i.ToString();
                    sbRows.Append(PrepareDataJson_chucvu(item, strSTT));
                    i++;
                }
                if (sbRows.Length > 0)
                    sbRows.Remove(sbRows.Length - 1, 1);
            }
            sbResult.Append("{");
            sbResult.Append("\"isHeader\":\"" + "111" + "\",");
            sbResult.Append("\"Pages\":\"" + "212" + "\",");
            if (model.machucdanh.ToString() != "0")
            {
                sbResult.Append("\"SubRow\":\"" + "true" + "\",");
                sbResult.Append("\"RowID\":\"" + model.machucdanh + "\",");
            }
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        private StringBuilder PrepareDataJson_chucvu(ChucDanhModels model, string couter)
        {
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.machucdanh.ToString(), function.ReadXMLGetKeyEncrypt());
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.machucdanh.ToString(), function.ReadXMLGetKeyEncrypt()) + "\"}],");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //colum checkbox
                string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}' mpb='" + strEncryptCode + "'/>", model.machucdanh);
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1\",");
                sbResult.Append("\"col_id\":\"1\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                sbResult.Append("},");
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                sbResult.Append("\"col_id\":\"2\",");
                sbResult.Append("\"col_value\":\"" + couter + "\"");
                sbResult.Append("},");

                //Mã đơn vị
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"3\",");
                sbResult.Append("\"col_value\":\"" + model.machucdanh + "\"");
                sbResult.Append("},");

                //Tên phòng ban
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"4\",");
                sbResult.Append("\"col_value\":\"" + model.tenchucdanh + "<a href='" + Url.Action("Indexchucdanh", "Danhmuc", new { mapb = strEncryptCode }) + "' title='" + model.tenchucdanh + "'>" + model.tenchucdanh + "</a>\"");
                sbResult.Append("},");

                //Mã phòng ban
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"5\",");
                sbResult.Append("\"col_value\":\"" + model.tengiaodich + "\"");
                sbResult.Append("},");

                //// Số diện thoại
                //sbResult.Append("{");
                //sbResult.Append("\"colspan\":\"1\",");
                //sbResult.Append("\"col_class\":\"ovh col6\",");
                //sbResult.Append("\"col_id\":\"6\",");
                //sbResult.Append("\"col_value\":\"" + model.tenphongban + "\"");
                //sbResult.Append("},");

                // Số diện thoại
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.ghichu + "\"");
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

        [HttpPost]
        public ActionResult Updatechucvu(string act, string ID, ChucDanhModels model)
        {
            if (!IsLogged())
                return BackToLogin();
            if (act == "create")
            {
                ChucDanhModels param = new ChucDanhModels();
                param.machucdanh = model.machucdanh;
                param.tenchucdanh = model.tenchucdanh;
                param.tengiaodich = model.tengiaodich;
                param.ghichu = model.ghichu;
                param.xoa = "0";
                param.nguoitao = int.Parse(Session["userid"].ToString());
                param.ngaytao = ngaytao;
                DanhmucServices services = new DanhmucServices();
                string result = services.InsertRow_chucdanh(param);
            }
            else if (act == "update")
            {
                ChucDanhModels param = new ChucDanhModels();
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                param.machucdanh = int.Parse(AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt()));
                param.tenchucdanh = model.tenchucdanh;
                param.tengiaodich = model.tengiaodich;
                param.ghichu = model.ghichu;
                param.xoa = "0";
                param.nguoitao = int.Parse(Session["userid"].ToString());
                param.ngaytao = ngaytao;
                DanhmucServices services = new DanhmucServices();
                bool result = services.UpdateRow_chucdanh(param);
            }
            else if (act == "delete")
            {
                ChucDanhModels param = new ChucDanhModels();
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                param.machucdanh = int.Parse(AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt()));
              
                param.nguoihieuchinh = int.Parse(Session["userid"].ToString());
                DanhmucServices services = new DanhmucServices();

                bool result = services.DeleteRows_chucdanh(param);

            }
            return RedirectToAction("Indexchucdanh", "Danhmuc");
        }



        /// <summary>
        /// //Van phong pham
        /// </summary>
        /// <returns></returns>
        public ActionResult Indexvanphongpham()
        {
            if (!IsLogged())
                return BackToLogin();
            DanhmucServices service = new DanhmucServices();
            Danhmuc_VanPhongPhamModels param = new Danhmuc_VanPhongPhamModels();
            List<Danhmuc_VanPhongPhamModels> lstResult_vanphongpham = service.SelectRows_Danhmuccha_vpp(param);
            StringBuilder sbHeHoach = new StringBuilder();
            IEnumerable<Danhmuc_VanPhongPhamModels> results = lstResult_vanphongpham.Where(s => s.danhmucgoc == "0").ToList();
            foreach (var item in results)
            {
                sbHeHoach.Append(string.Format("<option value='{0}'>{1}</option>", item.mavanphongpham, item.tenvanphongpham));
            }
            ViewBag.keHoachs = sbHeHoach.ToString();
            return View();
        }

        [HttpPost]
        public JsonResult SelectRows_vanphongpham(Danhmuc_VanPhongPhamModels model)
        {
            DanhmucServices service = new DanhmucServices();
            Danhmuc_VanPhongPhamModels param = new Danhmuc_VanPhongPhamModels();
            List<Danhmuc_VanPhongPhamModels> lstResult = service.SelectRows_Vanphongpham(param);
            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            if (lstResult.Count > 0)
            {
                string strSTT = "";
                int i = 1;
                foreach (var item in lstResult)
                {
                    if (item.danhmucgoc == "0")
                    {
                        i = 1;
                    }
                    strSTT = (i-1).ToString();
                    sbRows.Append(PrepareDataJson_vanphongpham(item, strSTT));
                    i++;
                }
                if (sbRows.Length > 0)
                    sbRows.Remove(sbRows.Length - 1, 1);
            }
            sbResult.Append("{");
            sbResult.Append("\"isHeader\":\"" + "111" + "\",");
            sbResult.Append("\"Pages\":\"" + "212" + "\",");
            if (model.mavanphongpham.ToString() != "0")
            {
                sbResult.Append("\"SubRow\":\"" + "true" + "\",");
                sbResult.Append("\"RowID\":\"" + model.mavanphongpham.ToString() + "\",");
            }
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        private StringBuilder PrepareDataJson_vanphongpham(Danhmuc_VanPhongPhamModels model, string couter)
        {
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.mavanphongpham.ToString(), function.ReadXMLGetKeyEncrypt());
            try
            {
                if (model.danhmucgoc != "0")
                {
                    sbResult.Append("{");
                    sbResult.Append("\"col_class\":\"rows-box\",");
                    sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                    sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.mavanphongpham.ToString(), function.ReadXMLGetKeyEncrypt()) + "\"}],");
                    sbResult.Append("\"col_value\":[");

                    #region Data cell
                    //colum checkbox
                    string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}' mpb='" + strEncryptCode + "'/>", model.mavanphongpham.ToString());
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col1\",");
                    sbResult.Append("\"col_id\":\"1\",");
                    sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                    sbResult.Append("},");
                    //stt
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                    sbResult.Append("\"col_id\":\"2\",");
                    sbResult.Append("\"col_value\":\"" + couter + "\"");
                    sbResult.Append("},");

                    //Mã đơn vị
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col3\",");
                    sbResult.Append("\"col_id\":\"3\",");
                    sbResult.Append("\"col_value\":\"" + model.mavanphongpham + "\"");
                    sbResult.Append("},");

                    //Tên phòng ban
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col4\",");
                    sbResult.Append("\"col_id\":\"4\",");
                    sbResult.Append("\"col_value\":\"" + model.tenvanphongpham + "<a href='" + Url.Action("Index", "Danhmuc", new { mapb = strEncryptCode }) + "' title='" + model.tenvanphongpham + "'> </a>\""); //" ++ "
                    sbResult.Append("},");

                    //Danh muc cha
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col5\",");
                    sbResult.Append("\"col_id\":\"5\",");
                    sbResult.Append("\"col_value\":\"" + model.dongia + "\"");
                    sbResult.Append("},");

                    // Don vi tinh
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col6\",");
                    sbResult.Append("\"col_id\":\"6\",");
                    sbResult.Append("\"col_value\":\"" + model.donvitinh + "\"");
                    sbResult.Append("},");


                    // Số diện thoại
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col7\",");
                    sbResult.Append("\"col_id\":\"6\",");
                    sbResult.Append("\"col_value\":\"" + model.ghichu + "\"");
                    sbResult.Append("},");

                    //Danh muc cha
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"type\":\"hidden\",");
                    sbResult.Append("\"col_class\":\"ovh col8\",");
                    sbResult.Append("\"col_id\":\"8\",");
                    sbResult.Append("\"col_value\":\"" + model.danhmuccha + "\"");
                    sbResult.Append("},");

                    //Danh muc cha
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"type\":\"hidden\",");
                    sbResult.Append("\"col_class\":\"ovh col9\",");
                    sbResult.Append("\"col_id\":\"9\",");
                    sbResult.Append("\"col_value\":\"" + model.danhmucgoc + "\"");
                    sbResult.Append("}");

                    #endregion

                    sbResult.Append("]");
                    sbResult.Append("},");
                }

                else
                {
                    sbResult.Append("{");
                    sbResult.Append("\"col_class\":\"rows-box\",");
                    sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                    sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.mavanphongpham.ToString(), function.ReadXMLGetKeyEncrypt()) + "\"}],");
                    sbResult.Append("\"col_value\":[");

                    #region Data cell
                    //colum checkbox
                    string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}' mpb='" + strEncryptCode + "'/>", model.mavanphongpham.ToString());
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"type\":\"hidden\",");
                    sbResult.Append("\"col_class\":\"ovh col1\",");
                    sbResult.Append("\"col_id\":\"1\",");
                    sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                    sbResult.Append("},");

                    //Tên phòng ban
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"6\",");
                    sbResult.Append("\"style\":\"font-weight:bold\",");
                    sbResult.Append("\"col_class\":\"ovh col4\",");
                    sbResult.Append("\"col_id\":\"4\",");
                    sbResult.Append("\"col_value\":\"" + model.tenvanphongpham + "<a href='" + Url.Action("Index", "Danhmuc", new { mapb = strEncryptCode }) + "' title='" + model.tenvanphongpham + "'> </a>\""); //" ++ "
                    sbResult.Append("},");


                    //stt
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                    sbResult.Append("\"col_id\":\"2\",");
                    sbResult.Append("\"col_value\":\"" + couter + "\"");
                    sbResult.Append("},");

                    //Mã đơn vị
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"col_class\":\"ovh col3\",");
                    sbResult.Append("\"col_id\":\"3\",");
                    sbResult.Append("\"col_value\":\"" + model.mavanphongpham + "\"");
                    sbResult.Append("},");

                    //Danh muc cha
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"type\":\"hidden\",");
                    sbResult.Append("\"col_class\":\"ovh col5\",");
                    sbResult.Append("\"col_id\":\"5\",");
                    sbResult.Append("\"col_value\":\"" + model.dongia + "\"");
                    sbResult.Append("},");

                    ////Don vi tinh
                    //sbResult.Append("{");
                    //sbResult.Append("\"colspan\":\"1\",");
                    //sbResult.Append("\"type\":\"hidden\",");
                    //sbResult.Append("\"col_class\":\"ovh col6\",");
                    //sbResult.Append("\"col_id\":\"6\",");
                    //sbResult.Append("\"col_value\":\"" + model.donvitinh + "\"");
                    //sbResult.Append("},");


                    // Số diện thoại
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"type\":\"hidden\",");
                    sbResult.Append("\"col_class\":\"ovh col7\",");
                    sbResult.Append("\"col_id\":\"7\",");
                    sbResult.Append("\"col_value\":\"" + model.ghichu + "\"");
                    sbResult.Append("},");

                    //Danh muc cha
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"type\":\"hidden\",");
                    sbResult.Append("\"col_class\":\"ovh col8\",");
                    sbResult.Append("\"col_id\":\"8\",");
                    sbResult.Append("\"col_value\":\"" + model.danhmuccha + "\"");
                    sbResult.Append("},");

                    //Danh muc cha
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");
                    sbResult.Append("\"type\":\"hidden\",");
                    sbResult.Append("\"col_class\":\"ovh col9\",");
                    sbResult.Append("\"col_id\":\"9\",");
                    sbResult.Append("\"col_value\":\"" + model.danhmucgoc + "\"");
                    sbResult.Append("}");

                    #endregion

                    sbResult.Append("]");
                    sbResult.Append("},");
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return sbResult;
        }

        [HttpPost]
        public ActionResult UpdateVanphongpham(string act, string ID, Danhmuc_VanPhongPhamModels model)
        {
            if (!IsLogged())
                return BackToLogin();
            if (act == "create")
            {
                Danhmuc_VanPhongPhamModels param = new Danhmuc_VanPhongPhamModels();
                param.mavanphongpham = model.mavanphongpham;
                param.tenvanphongpham = model.tenvanphongpham;
                param.dongia = model.dongia;
                param.danhmuccha = model.danhmuccha;
                param.ghichu = model.ghichu;
                param.xoa = "0";
                param.nguoitao = int.Parse(Session["userid"].ToString());
                param.ngaytao = ngaytao;
                DanhmucServices services = new DanhmucServices();
                string result = services.InsertRow_vanphongpham(param);
            }
            else if (act == "update")
            {
                Danhmuc_VanPhongPhamModels param = new Danhmuc_VanPhongPhamModels();
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                param.mavanphongpham = int.Parse(AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt()));

                param.tenvanphongpham = model.tenvanphongpham;
                param.donvitinh = model.donvitinh;
                param.dongia = model.dongia;
                param.danhmuccha = model.danhmuccha;
                param.ghichu = model.ghichu;
                param.xoa = "0";
                param.nguoitao = int.Parse(Session["userid"].ToString());
                param.ngaytao = ngaytao;
                DanhmucServices services = new DanhmucServices();
                bool result = services.UpdateRow_vanphongpham(param);
            }
            else if (act == "delete")
            {
                Danhmuc_VanPhongPhamModels param = new Danhmuc_VanPhongPhamModels();
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                param.mavanphongpham = int.Parse(AES.DecryptText(ID, function.ReadXMLGetKeyEncrypt()));
                param.nguoihieuchinh = int.Parse(Session["userid"].ToString());

                DanhmucServices services = new DanhmucServices();
                bool result = services.DeleteRows_DM_Vanphongpham(param);
            }
            //return View();
            return RedirectToAction("Indexvanphongpham", "Danhmuc");
        }
      
	}
}