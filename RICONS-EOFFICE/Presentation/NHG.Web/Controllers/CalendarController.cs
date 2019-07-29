using NHG.Core.Constants;
using NHG.Core.Functions;
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
using System.Xml.Linq;

namespace NHG.Web.Controllers
{
    public class CalendarController : BaseController
    {
        #region Fields
        Log4Net _logger = new Log4Net(typeof(AccountController));
        #endregion

        //
        // GET: /Calendar/
        public ActionResult Index()
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            ViewBag.JsonChonGio = DataJsonChonGio().ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Create(LichLamViecModels model)
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            string strMauLich = "0";
            string strNgayBatDau = model.ngaybatdau;
            string strThoiGianBatDau = model.giobatdau;
            string strNgayKetThuc = model.ngayketthuc;
            string strThoiGianKetThuc = model.gioketthuc;
            string strIsAllDayEvent = model.sukiencangay;

            string dtngaybatdau = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayBatDau + " " + strThoiGianBatDau, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
            string dtngayketthuc = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayKetThuc + " " + strThoiGianKetThuc, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");

            if (strIsAllDayEvent == "1")
            {
                dtngaybatdau = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayBatDau + " 00:00", Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
                dtngayketthuc = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayKetThuc + " 23:00", Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
            }
            LichLamViecModels clParamSuKien = new LichLamViecModels()
            {
                noidung = model.noidung,
                diadiem = model.diadiem,
                maulich = strMauLich,
                maphongban = "NHG.PB.000008",
                ngaybatdau = dtngaybatdau,
                ngayketthuc = dtngayketthuc,
                nguoitao = Session["userid"].ToString(),
                nguoihieuchinh = Session["userid"].ToString()
            };
            LichLamViecServices services = new LichLamViecServices();
            services.InsertRow(clParamSuKien);
            return RedirectToAction("Index", "Calendar");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            //tao profile cho chuc nang tao moi lich
            CommonHelper.CreateFolderTempControllerProfile(base.GetControllerName());
            LichLamViecServices services = new LichLamViecServices();
            List<LichLamViecModels> lstResult = services.SelectRows(new LichLamViecModels() {
                //thay the khoang cach thanh dau "+", vi tu khi trinh duyen gui request len ky tu "+" duoc chuyen doi thanh khoang trang " "
                malichlamviec = AES.Decrypt(id.Replace(" ", "+"), Convert.FromBase64String(Session.SessionID)),
            });
            if (lstResult.Count > 0)
            {
                return View(lstResult[0]);
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult Edit(LichLamViecModels model)
        {
            string strMauLich = "0";
            string strNgayBatDau = model.ngaybatdau;
            string strThoiGianBatDau = model.giobatdau;
            string strNgayKetThuc = model.ngayketthuc;
            string strThoiGianKetThuc = model.gioketthuc;
            string strIsAllDayEvent = model.sukiencangay;

            string dtngaybatdau = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayBatDau + " " + strThoiGianBatDau, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
            string dtngayketthuc = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayKetThuc + " " + strThoiGianKetThuc, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");

            if (strIsAllDayEvent == "1")
            {
                dtngaybatdau = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayBatDau + " 00:00", Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
                dtngayketthuc = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayKetThuc + " 23:00", Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
            }
            LichLamViecModels clParamSuKien = new LichLamViecModels()
            {
                malichlamviec = AES.Decrypt(model.malichlamviec.Replace(" ", "+"), Convert.FromBase64String(Session.SessionID)),
                noidung = model.noidung,
                diadiem = model.diadiem,
                maulich = strMauLich,
                maphongban = "NHG.PB.000008",
                ngaybatdau = dtngaybatdau,
                ngayketthuc = dtngayketthuc,
                nguoihieuchinh = Session["userid"].ToString()
            };
            LichLamViecServices services = new LichLamViecServices();
            services.UpdateRow(clParamSuKien);
            return RedirectToAction("Index", "Calendar");
        }

        #region method
        private StringBuilder DataJsonChonGio()
        {
            StringBuilder sbResult = new StringBuilder();
            try
            {
                for (int i = 0; i < 23; i++)
                {
                    string tt = string.Format("{0}g00", i.ToString("00"));
                    string tt_hp = string.Format("{0}g30", i.ToString("00"));
                    sbResult.Append("{");
                    sbResult.Append("\"id\":\"" + tt + "\",");
                    sbResult.Append("\"value\":\"" + tt + "\"");
                    sbResult.Append("},");
                    sbResult.Append("{");
                    sbResult.Append("\"id\":\"" + tt_hp + "\",");
                    sbResult.Append("\"value\":\"" + tt_hp + "\"");
                    sbResult.Append("},");
                }
                if (sbResult.Length > 0)
                    sbResult.Remove(sbResult.Length - 1, 1);

            }
            catch (Exception ex)
            {
                sbResult = new StringBuilder();
                sbResult.Append("{");
                sbResult.Append("\"error\":\"" + "" + "\"");
                sbResult.Append("}");
                _logger.Error(ex);
            }
            return sbResult;
        }
        #endregion
    }
}