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
using System.Data;
namespace NHG.Web.Controllers
{
    public class TaskController : BaseController
    {
        #region Fields
        Log4Net _logger = new Log4Net(typeof(MilestonesController));
        #endregion
        //
        // GET: /Task/
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
            TaiKhoanServices serTaiKhoan = new TaiKhoanServices();
            StringBuilder sbNguoiThucHien = new StringBuilder();
            M_TaiKhoan tk = new M_TaiKhoan();
            tk.maphongban = Session["maphongban"].ToString();
            foreach (var item in serTaiKhoan.SelectTaiKhoanForCombobox(tk))
            {
                sbNguoiThucHien.Append(string.Format("<option value='{0}'>{1}</option>", item.mataikhoan, item.hoten));
            }
            ViewBag.nguoiThucHiens = sbNguoiThucHien.ToString();

            StringBuilder sbNguoiduyeths = new StringBuilder();
            foreach (var item in serTaiKhoan.SelectNguoiduyetCombobox())
            {
                sbNguoiduyeths.Append(string.Format("<option value='{0}'>{1}</option>", item.mataikhoan, item.hoten));
            }
            ViewBag.nguoiDuyetHoso = sbNguoiduyeths.ToString();

            return View();
        }

        [HttpPost]
        public ActionResult Create(MucTieuCongViecModels muctieu)
        {
            if (!IsLogged())
                return BackToLogin();
            muctieu.nguoitao = Session["userid"].ToString();
            muctieu.danhgia = "0%";
            muctieu.ketqua = "0%";
            muctieu.tinhtrang = "1";//Chưa duyệt
            muctieu.makehoachgoc = "0";
            muctieu.nguoitao = Session["userid"].ToString();
            if (GetPhongBanDonVi() != null)
            {
                muctieu.madonvi = GetPhongBanDonVi().madonvi;
            }
            
            MuctieucongviecServices services = new MuctieucongviecServices();
            services.InsertRow(muctieu, Session["manhansu"].ToString());
            return RedirectToAction("Index", "Task");
        }

        [HttpPost]
        public JsonResult SelectRows()
        {
            MuctieucongviecServices service = new MuctieucongviecServices();
            List<MucTieuCongViecModels> lstResult = service.SelectRows();
            StringBuilder result = new StringBuilder();
            StringBuilder lstRow = new StringBuilder();
            if (lstResult.Count > 0)
            {
                int i = 1;
                foreach (var item in lstResult)
                {
                    lstRow.Append(PrepareDataJson(item, i));
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

        #region method
        private StringBuilder PrepareDataJson(MucTieuCongViecModels model, int couter)
        {
            StringBuilder sbResult = new StringBuilder();
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + "323" + "\",");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //colum checkbox
                string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}'/>", model.makehoach);
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1\",");
                sbResult.Append("\"col_id\":\"1\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                sbResult.Append("},");
                //stt
                //sbResult.Append("{");
                //sbResult.Append("\"colspan\":\"1\",");
                //sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                //sbResult.Append("\"col_id\":\"2\",");
                //sbResult.Append("\"col_value\":\"" + couter.ToString() + "\"");
                //sbResult.Append("},");
                //noi dung
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"3\",");
                sbResult.Append("\"noidungmuctieu\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + model.noidungmuctieu + "\"");
                sbResult.Append("},");

                //tỷ trọng (%)
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"4\",");
                sbResult.Append("\"tytrong\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + model.tytrong + "\"");
                sbResult.Append("},");
                
                //chỉ tieu năm
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"5\",");
                sbResult.Append("\"chitieunam\":\"" + "323" + "\",");
                sbResult.Append("\"col_value\":\"" + model.chitieunam + "\"");
                sbResult.Append("},");

                //ngay bat dau
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"ngaybatdau\":\"8\",");
                sbResult.Append("\"col_value\":\"" + model.ngaybatdau + "\"");
                sbResult.Append("},");

                //ngay ket thuc
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"ngayketthuc\":\"8\",");
                sbResult.Append("\"col_value\":\"" + model.ngayketthuc + "\"");
                sbResult.Append("},");

                ////Tự danh gia ket qua
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"nguoithuchien\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + model.danhgia + "\"");
                sbResult.Append("}");

                ////Xếp đánh giá kết quả
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"nguoithuchien\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + model.ketqua + "\"");
                sbResult.Append("}");

                //Người duyệt mục tiêu
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col7\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"nguoithuchien\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + model.nguoiduyetmuctieu + "\"");
                sbResult.Append("}");

                //Tình trang
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col8\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"nguoithuchien\":\"" + "23" + "\",");
                sbResult.Append("\"col_value\":\"" + model.tinhtrang + "\"");
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