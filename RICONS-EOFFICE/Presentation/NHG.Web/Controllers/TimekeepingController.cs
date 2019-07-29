using ClosedXML.Excel;
using ExportHelper.Repository;
using Newtonsoft.Json.Linq;
using NHG.Core.Functions;
using NHG.Logger;
using NHG.Web.Data.Services;
using NHG.Web.Models;
using Nop.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class TimekeepingController : BaseController
    {
        Log4Net _logger = new Log4Net(typeof(TimekeepingController));
        // GET: /Timekeeping/
        private static double songayduoctraluong = 0;
        private static double snphucapcom = 0;
        private static double ngaycong_le = 0;
        private static double nghiphep = 0;
        private static double nghihieuhi = 0;
        private static double nghiom_ts = 0;
        private static double nghikhongluong = 0;
        private static double songaycongchuan = 0;

        private static double songayduoctraluong1 = 0;
        private static double snphucapcom1 = 0;
        private static double ngaycong_le1 = 0;
        private static double nghiphep1 = 0;
        private static double nghihieuhi1 = 0;
        private static double nghiom_ts1 = 0;
        private static double nghikhongluong1 = 0;
        private static double songaycongchuan1 = 0;

        private static string idmachamcong = "0";

        public static string maphongbanexcel = "0";
        public static string thangnam = "";
        public static int songaytrongthang = 0;
        public static int phongban_congtruong = 0; // neu la cong truong =1

        public static List<PhongBanModels> lstResult_phongban = new List<PhongBanModels>();

        private static List<AtHolidayModels> listHoliday = new List<AtHolidayModels>();

        private static List<AbsentModels> lstResult_xinnghiphep = new List<AbsentModels>();

        private static List<TableModels> lstResult_xinnghiphep_manv = new List<TableModels>();

        //private static List<TimekeepingModels> lstResult_Allnhanvien = new List<TimekeepingModels>();

        public static string chkchamcong = "0";
        public static string chkmaphongban = "0";

        int songay(int thang, int nam)
        {
            switch (thang)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 2:
                    if (namnhuan(nam))
                        return 29;
                    else
                        return 28;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
            }
            return 0;
        }
        bool namnhuan(int nam)
        {
            return ((nam % 4 == 0 && nam % 100 != 0) || (nam % 400 == 0));
        }

        public ActionResult Index()
        {
            if (!IsLogged())
                return BackToLogin();
            songayduoctraluong = 0;
            snphucapcom = 0;
            ngaycong_le = 0;
            nghiphep = 0;
            nghihieuhi = 0;
            nghiom_ts = 0;
            nghikhongluong = 0;
            DanhmucServices service = new DanhmucServices();
            PhongBanModels parampb = new PhongBanModels();
            StringBuilder sbphongban = new StringBuilder();
            if (Session["grouptk"].ToString().Trim() != "1")
            {
                parampb.maphongban = Session["maphongban"].ToString().Trim();
            }
            else
            {
                sbphongban.Append(string.Format("<option value='{0}'>{1}</option>", 0, "Chọn phòng ban"));
            }
            lstResult_phongban = new List<PhongBanModels>();
            lstResult_phongban = service.SelectRows(parampb);

            foreach (var item in lstResult_phongban)
            {
                sbphongban.Append(string.Format("<option value='{0}'>{1}</option>", item.maphongban, item.tenphongban));
            }
            ViewBag.sbphongban = sbphongban.ToString();

            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            lstResult.Add(new TimekeepingModels() { thang = DateTime.Now.ToString("MM/yyyy"), chamcong = chkchamcong, maphongban = chkmaphongban });
            return View(lstResult[0]);
        }

        string thutrongtuan = "";

        private string kiemtrangay(string ngaythangnam, string manv, string phongban_congtruong)
        {
            string kq = "X";

            if (phongban_congtruong.Trim() == "True") phongban_congtruong = "1";

            var ktngayle = listHoliday.Where(p => p.datevalue == ngaythangnam.Trim()).ToList();
            DateTime date = FunctionsDateTime.ConvertStringToDate(ngaythangnam);
            string thucuatuan = thutrongtuan = FunctionsDateTime.DayOfWeek_CN(date);

            string ngayle=ngaythangnam.Substring(0,5);

            if (ktngayle.Count > 0)
            {
                string ngayle111 = ktngayle[0].datevalue;
            }
            

            if ((ktngayle.Count() > 0 && thucuatuan != "T7" && thucuatuan != "CN") || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le = ngaycong_le + 1;
                songaycongchuan = songaycongchuan + 1;
            }
            else if ((  ktngayle.Count() > 0 && thucuatuan == "T7" && phongban_congtruong.Trim() != "1") || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le = ngaycong_le + 0.5;
                songaycongchuan = songaycongchuan + 0.5;
            }
            else if (( ktngayle.Count() > 0 && thucuatuan == "T7" && phongban_congtruong.Trim() == "1" ) || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le = ngaycong_le + 1;
                songaycongchuan = songaycongchuan + 1;
            }
            else
            {
                List<TableModels> ngaynghi = lstResult_xinnghiphep_manv.Where(p => p.ngayxinnghitu == ngaythangnam).ToList();
                if (thucuatuan == "CN") kq = "CN";
                else if (thucuatuan == "T7" && phongban_congtruong.Trim() != "True" && phongban_congtruong.Trim() != "1")
                {
                    kq = "X/2";
                    songaycongchuan = songaycongchuan + 0.5;
                    if (ngaynghi.Count > 0)
                    {
                        kq = ngaynghi[0].nghiphep;
                        if (kq == "Fo/2" || kq == "Fo") //Cha me,vc,con mat
                            nghihieuhi = nghihieuhi + 0.5;
                        else if (kq == "TS")
                            nghiom_ts = nghiom_ts + 0.5;
                        else if (kq == "Ro/2" || kq == "Ro")
                            nghikhongluong = nghikhongluong + 0.5;
                        else if (kq == "F/2" || kq == "F")
                            nghiphep = nghiphep + 0.5;
                    }
                }
                else
                {
                    kq = "X";
                    songaycongchuan = songaycongchuan + 1;
                    if (ngaynghi.Count > 0)
                    {
                        kq = ngaynghi[0].nghiphep;
                        //Nghi phep
                        if (kq == "F/2")
                            nghiphep = nghiphep + 0.5;
                        else if (kq == "F")
                            nghiphep = nghiphep + 1;
                        //Nghỉ hiếu hỉ
                        else if (kq == "TS/2")
                            nghiom_ts = nghiom_ts + 0.5;
                        else if (kq == "TS")
                            nghiom_ts = nghiom_ts + 1;
                        else if (kq == "Fo/2")
                            nghihieuhi = nghihieuhi + 0.5;
                        else if (kq == "Fo")
                            nghihieuhi = nghihieuhi + 1;
                        else if (kq == "Ro/2")
                            nghikhongluong = nghikhongluong + 0.5;
                        else if (kq == "Ro")
                            nghikhongluong = nghikhongluong + 1;
                    }
                    else snphucapcom = snphucapcom + 1;

                }
            }
            return kq;
        }

        private string kiemtrangay_hieuchinh(string ngaythangnam, string manv, string phongban_congtruong)
        {
            string kq = "X";
            var ktngayle = listHoliday.Where(p => p.datevalue == ngaythangnam.Trim());
            DateTime date = FunctionsDateTime.ConvertStringToDate(ngaythangnam);
            string thucuatuan = thutrongtuan = FunctionsDateTime.DayOfWeek_CN(date);

            string ngayle = ngaythangnam.Substring(0, 5);

            if ((ktngayle.Count() > 0 && thucuatuan != "T7" && thucuatuan != "CN") || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le1 = ngaycong_le1 + 1;
                songaycongchuan1 = songaycongchuan1 + 1;
            }
            else if ((ktngayle.Count() > 0 && thucuatuan == "T7"&& phongban_congtruong.Trim() != "1") || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le1 = ngaycong_le1 + 0.5;
                songaycongchuan1 = songaycongchuan1 + 0.5;
            }
            else if ((ktngayle.Count() > 0 && thucuatuan == "T7" && phongban_congtruong.Trim() == "1") || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le1 = ngaycong_le1 + 1;
                songaycongchuan1 = songaycongchuan1 + 1;
            }

            else
            {
                //lstResult_xinnghiphep = service.SelectRows_Danhsachnghiphep(manhanvien, tungay, denngay);
                List<TableModels> ngaynghi = lstResult_xinnghiphep_manv.Where(p => p.ngayxinnghitu == ngaythangnam).ToList();
                if (thucuatuan == "CN") kq = "CN";
                else if (thucuatuan == "T7" && phongban_congtruong.Trim() != "True" && phongban_congtruong.Trim() != "1")
                {
                    kq = "X/2";
                    songaycongchuan1 = songaycongchuan1 + 0.5;
                    if (ngaynghi.Count > 0)
                    {
                        kq = ngaynghi[0].nghiphep;
                        if (kq == "Fo/2" || kq == "Fo") //Cha me,vc,con mat
                            nghihieuhi1 = nghihieuhi1 + 0.5;
                        else if (kq == "TS")
                            nghiom_ts1 = nghiom_ts1 + 0.5;
                        else if (kq == "Ro/2" || kq == "Ro")
                            nghikhongluong1 = nghikhongluong1 + 0.5;
                        else if (kq == "F/2" || kq == "F")
                            nghiphep1 = nghiphep1 + 0.5;
                    }
                }
                else
                {
                    kq = "X";
                    songaycongchuan1 = songaycongchuan1 + 1;
                    if (ngaynghi.Count > 0)
                    {
                        kq = ngaynghi[0].nghiphep;
                        //Nghi phep
                        if (kq == "F/2")
                            nghiphep1 = nghiphep1 + 0.5;
                        else if (kq == "F")
                            nghiphep1 = nghiphep1 + 1;
                        //Nghỉ hiếu hỉ
                        else if (kq == "TS/2")
                            nghiom_ts1 = nghiom_ts1 + 0.5;
                        else if (kq == "TS")
                            nghiom_ts1 = nghiom_ts1 + 1;
                        else if (kq == "Fo/2")
                            nghihieuhi1 = nghihieuhi1 + 0.5;
                        else if (kq == "Fo")
                            nghihieuhi1 = nghihieuhi1 + 1;
                        else if (kq == "Ro/2")
                            nghikhongluong1 = nghikhongluong1 + 0.5;
                        else if (kq == "Ro")
                            nghikhongluong1 = nghikhongluong1 + 1;
                    }
                    else snphucapcom1 = snphucapcom1 + 1;
                }
            }
            return kq;
        }

        void loaddulieu_ngayle_thang(int thang, int nam)
        {
            TimekeepingServices service = new TimekeepingServices();
            // Kiểm tra ngày lễ của tháng
            if (thang == 1)
            {
                listHoliday = service.SelectRows_Danhsachngayle("21/" + (12).ToString("00") +"/"+ (nam - 1), "20/" + thang.ToString("00")+"/" + nam);
            }
            else
            {
                listHoliday = service.SelectRows_Danhsachngayle("21/" + (thang - 1).ToString("00") +"/"+ nam, "20/" + thang.ToString("00")+"/" + nam);
            }
            
            foreach (var item in listHoliday)
                item.datevalue = item.datevalue.Split(' ')[0].ToString();
        }

        void loaddulieu_xinnghiphep_thang(string manhanvien, int thang, int nam)
        {
            string tungay = "21/" + (thang - 1).ToString("00") + "/" + nam;
            if(thang==1)
                tungay = "21/12/" + (nam-1).ToString();
            string denngay = "20/" + thang.ToString("00") + "/" + nam;
            TimekeepingServices service = new TimekeepingServices();
            // Kiểm tra danh sach nhân viên nghỉ phép của tháng.
            lstResult_xinnghiphep = service.SelectRows_Danhsachnghiphep(manhanvien, tungay, denngay);
            lstResult_xinnghiphep_manv = new List<TableModels>();
            //List<TableModels>  lstResult_Table_manv = new List<TableModels>();
            foreach (var item in lstResult_xinnghiphep)
            {
                try
                {
                    DateTime ngaynghitu = FunctionsDateTime.ConvertStringToDate(item.ngayxinnghitu);
                    DateTime ngaynghiden = FunctionsDateTime.ConvertStringToDate(item.ngayxinnghiden);

                    DateTime ngaynghitu_truyen = FunctionsDateTime.ConvertStringToDate(tungay);
                    DateTime ngaynghiden_truyen = FunctionsDateTime.ConvertStringToDate(denngay);

                    if (item.nghithaisan.Trim() == "1")
                    {
                        if (ngaynghitu_truyen > ngaynghitu)
                            ngaynghitu = ngaynghitu_truyen;
                        if (ngaynghiden_truyen < ngaynghitu)
                            ngaynghiden = ngaynghitu;
                        else if (ngaynghiden_truyen < ngaynghiden)
                            ngaynghiden = ngaynghiden_truyen;
                    }

                    //lstResult_Table_manv = new List<TableModels>();
                    double songayxinnghi = double.Parse(item.songayxinnghi);
                    double songayphepconlai = 0;
                    if (item.songayphepconlai.Trim() != "")
                        songayphepconlai = double.Parse(item.songayphepconlai);
                    if (songayxinnghi < 1 && songayphepconlai >= 0)
                    {
                        item.nghiphep = "F/2";
                        lstResult_xinnghiphep_manv.Add(new TableModels() { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "F/2" });
                    }
                    else
                    {
                        double ngaynghiphepcoluong = 0; double nghikhongluong = 0;
                        if (songayphepconlai >= 0)
                        {
                            ngaynghiphepcoluong = songayxinnghi;
                        }
                        if (songayphepconlai < 0)
                        {
                            ngaynghiphepcoluong = songayxinnghi + songayphepconlai;
                            nghikhongluong = songayphepconlai;
                        }
                        //Tinh ngay nghi phep
                        double chame_mat = 0; double conkh = 0; double canhankh = 0; double ongba_mat = 0;
                        for (DateTime i = ngaynghitu; i <= ngaynghiden;)
                        {
                            string ngaysub = i.ToString().Substring(0,5);
                            string thucuatuan = FunctionsDateTime.DayOfWeek_CN(ngaynghitu);
                            if(ngaysub=="30/04"||ngaysub=="01/05"||ngaysub=="02/09")
                            {

                            }
                            else if (item.nghithaisan.Trim() == "1") //Nghỉ thai san 5
                            {
                                lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "TS" });
                                chame_mat = chame_mat + 1;
                            }
                            else if (thucuatuan == "T7" && ngaynghiphepcoluong >= 0.5)
                            {
                                #region
                                if (item.chame_mat.Trim() == "1" && chame_mat <= 3)//Cha me, vc, con mat
                                {
                                    chame_mat = chame_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }
                                else if (item.conkh.Trim() == "1" && conkh <= 1) //Con kết hôn
                                {
                                    conkh = conkh + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }
                                else if (item.canhankh.Trim() == "1" && canhankh <= 3) //Cá nhân kh
                                {
                                    canhankh = canhankh + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }
                                else if (item.ongba_mat.Trim() == "1" && ongba_mat <= 1)//Ông bà mất
                                {
                                    ongba_mat = ongba_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }

                                else if (item.nghithaisan.Trim() == "1") //Nghỉ thai san 5
                                {
                                    chame_mat = chame_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "TS/2" });
                                }
                                else if (item.nghiphep.Trim() == "1") // nghỉ phép năm
                                {
                                    chame_mat = chame_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "F/2" });
                                }
                                #endregion
                                ngaynghiphepcoluong = ngaynghiphepcoluong - 0.5;
                            }
                            else if (thucuatuan != "T7" && thucuatuan != "CN" && ngaynghiphepcoluong > 0.5)
                            {
                                #region
                                if (item.chame_mat.Trim() == "1" && chame_mat <= 3)//Cha me, vc, con mat
                                {
                                    chame_mat = chame_mat + 1;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo" });
                                }
                                else if (item.conkh.Trim() == "1" && conkh <= 1) //Con kết hôn
                                {
                                    conkh = conkh + 1;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo" });
                                }
                                else if (item.canhankh.Trim() == "1" && canhankh <= 3) //Cá nhân kh
                                {
                                    canhankh = canhankh + 1;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo" });
                                }
                                else if (item.ongba_mat.Trim() == "1" && ongba_mat <= 1)//Ông bà mất
                                {
                                    ongba_mat = ongba_mat + 1;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo" });
                                }

                                else if (item.nghithaisan.Trim() == "1") //Nghỉ thai san 5
                                {
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "TS" });
                                    chame_mat = chame_mat + 1;
                                }
                                else if (item.nghiphep.Trim() == "1") // nghỉ phép năm
                                {
                                    chame_mat = chame_mat + 1;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "F" });
                                }
                                #endregion
                                ngaynghiphepcoluong = ngaynghiphepcoluong - 1;
                            }
                            else if (thucuatuan != "T7" && thucuatuan != "CN" && ngaynghiphepcoluong == 0.5)
                            {
                                #region
                                if (item.chame_mat.Trim() == "1" && chame_mat <= 3)//Cha me, vc, con mat
                                {
                                    chame_mat = chame_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }
                                else if (item.conkh.Trim() == "1" && conkh <= 1) //Con kết hôn
                                {
                                    conkh = conkh + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }
                                else if (item.canhankh.Trim() == "1" && canhankh <= 3) //Cá nhân kh
                                {
                                    canhankh = canhankh + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }
                                else if (item.ongba_mat.Trim() == "1" && ongba_mat <= 1)//Ông bà mất
                                {
                                    ongba_mat = ongba_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Fo/2" });
                                }

                                else if (item.nghithaisan.Trim() == "1") //Nghỉ thai san 5
                                {
                                    chame_mat = chame_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "TS/2" });
                                }
                                else if (item.nghiphep.Trim() == "1") // nghỉ phép năm
                                {
                                    chame_mat = chame_mat + 0.5;
                                    lstResult_xinnghiphep_manv.Add(new TableModels { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "F/2" });
                                }
                                #endregion
                                ngaynghiphepcoluong = ngaynghiphepcoluong - 0.5;
                            }
                            else if (thucuatuan == "T7" && nghikhongluong <= 0 && ngaynghiphepcoluong == 0)
                            {
                                item.nghiphep = "Ro/2";
                                item.ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy");
                                lstResult_xinnghiphep_manv.Add(new TableModels() { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Ro/2" });
                                nghikhongluong = nghikhongluong + 0.5;
                            }
                            else if (thucuatuan != "T7" && thucuatuan != "CN" && nghikhongluong < -0.5 && ngaynghiphepcoluong == 0)
                            {
                                lstResult_xinnghiphep_manv.Add(new TableModels() { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Ro" });
                                nghikhongluong = nghikhongluong + 1;
                            }
                            else if (thucuatuan != "T7" && thucuatuan != "CN" && nghikhongluong == -0.5 && ngaynghiphepcoluong == 0)
                            {
                                lstResult_xinnghiphep_manv.Add(new TableModels() { ngayxinnghitu = ngaynghitu.ToString("dd/MM/yyyy"), nghiphep = "Ro/2" });
                                nghikhongluong = nghikhongluong + 0.5;
                            }
                            i = ngaynghitu = ngaynghitu.AddDays(1);
                        }

                    }
                }
                catch (Exception) { }
            }

        }

        [HttpPost]
        public JsonResult SelectRows(TimekeepingModels model, int curentPage, string manhanvien, string hovaten, string chamcong, string trangthaiduyet)
        {
            songayduoctraluong = 0;
            snphucapcom = 0;
            ngaycong_le = 0;
            nghiphep = 0;
            nghihieuhi = 0;
            nghiom_ts = 0;
            nghikhongluong = 0;
            TimekeepingModels param = new TimekeepingModels();
            TimekeepingServices service = new TimekeepingServices();
            int loginid = 0;
            try
            {
                if ((Session["grouptk"].ToString().Trim() == "2" || Session["grouptk"].ToString().Trim() == "3") && Session["maphongban"].ToString().Trim() != "")
                {
                    model.maphongban = Session["maphongban"].ToString().Trim();
                    if (model.maphongban == "71" || model.maphongban == "74")
                        model.phongban_congtruong = "1";
                }
                else if (Session["userid"].ToString().Trim() != "1" && Session["grouptk"].ToString().Trim() != "1" && Session["grouptk"].ToString().Trim() != "2")
                {
                    loginid = int.Parse(Session["userid"].ToString());
                    model.maphongban = Session["maphongban"].ToString().Trim();
                    model.manhanvien = Session["manhansu"].ToString().Trim();
                    if (model.maphongban == "71" || model.maphongban == "74")
                        model.phongban_congtruong = "1";
                }
            }
            catch (Exception) { }

            param.nguoitao = loginid;
            param.maphongban = model.maphongban;
            param.manhanvien = model.manhanvien;
            chkmaphongban = model.maphongban;
            if (model.maphongban == "71" || model.maphongban == "74")
                model.phongban_congtruong = "1";
            chkchamcong = chamcong;
            maphongbanexcel = model.maphongban;

            int thang = int.Parse(DateTime.Now.Month.ToString("00"));
            int nam = int.Parse(DateTime.Now.Year.ToString("0000"));
            if (model.thang == null)
            {
                int ngaykt = int.Parse(DateTime.Now.Day.ToString());
                if (ngaykt > 20)
                    thang = int.Parse(DateTime.Now.Month.ToString("00")) + 1;
                else thang = int.Parse(DateTime.Now.Month.ToString("00"));
                nam = int.Parse(DateTime.Now.Year.ToString("0000"));

                thangnam = thang.ToString("00") + "/" + nam.ToString("0000");
            }
            else if (model.thang.Trim().Length == 7)
            {
                thang = int.Parse(model.thang.Split('/')[0]);
                nam = int.Parse(model.thang.Split('/')[1]);

                thangnam = thang.ToString("00") + "/" + nam.ToString("0000");
            }

            int tongsodong = service.CountRows(param, chamcong, thangnam, manhanvien, hovaten,trangthaiduyet);

            int sotrang = 1;
            if (tongsodong > 15)
            {
                if (tongsodong % 15 > 0) sotrang = (tongsodong / 15) + 1;
                else sotrang = (tongsodong / 15);
            }

            int trangbd = 1; int trangkt = 15;
            if (curentPage != 1 && curentPage <= sotrang)
            {
                trangbd = (trangkt * (curentPage - 1)) + 1;
                trangkt = trangkt * curentPage;
            }

            List<TimekeepingModels> lstResult = service.SelectRows(param, trangbd, trangkt, thangnam, manhanvien, hovaten, chamcong, trangthaiduyet);

            if (thang == 1)
            {
                songaytrongthang = songay(12, nam-1);
            }
            else
            {
                songaytrongthang = songay(thang - 1, nam);
            }
            
            loaddulieu_ngayle_thang(thang, nam);

            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            int tongdong = 0;
            if (lstResult.Count > 0)
            {
                string strSTT = "";
                int i = trangbd;
                foreach (var item in lstResult)
                {
                    item.songaycuathang = songaytrongthang;
                    songaycongchuan = snphucapcom = songayduoctraluong = ngaycong_le = nghiphep = nghihieuhi = nghiom_ts = nghikhongluong = 0;
                    loaddulieu_xinnghiphep_thang(item.manhanvien, thang, nam);
                    strSTT = i.ToString();
                    if (item.daduyet != null)
                    {
                        if (item.daduyet.Trim() == "1")
                            item.daduyet_ten = "Đã duyệt";
                        else if (item.daduyet.Trim() == "2")
                            item.daduyet_ten = "Không duyệt";
                        else if (item.daduyet.Trim() == "3")
                            item.daduyet_ten = "Đã gửi mail";
                        else if (item.daduyet.Trim() == "0")
                            item.daduyet_ten = "Chưa gửi";
                    }
                    sbRows.Append(PrepareDataJson(item, strSTT, songaytrongthang, thang, nam, chamcong));
                    i++;
                }
                tongdong = i - 1;
                if (sbRows.Length > 0)
                    sbRows.Remove(sbRows.Length - 1, 1);
            }

            if (curentPage == 1 && sotrang == 1 && lstResult.Count == 0)
            {
                curentPage = 0; sotrang = 0;
            }

            sbResult.Append("{");

            sbResult.Append("\"isHeader\":\"" + "111" + "\",");

            sbResult.Append("\"tongdong\":\"" + "" + tongsodong + "" + "\",");

            sbResult.Append("\"Pages\":\"" + "" + sotrang + "" + "\",");

            sbResult.Append("\"songaytrongthang\":\"" + songaytrongthang + "\",");
            sbResult.Append("\"thangnam\":\"" + DateTime.Now.ToString("MM/yyyy") + "\",");

            if (model.maphongban.ToString() != "0")
            {
                sbResult.Append("\"SubRow\":\"" + "true" + "\",");
                sbResult.Append("\"RowID\":\"" + model.maphongban + "\",");
            }
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        private StringBuilder PrepareDataJson(TimekeepingModels model, string couter, int songaytrongthang, int thang, int nam, string chamcong)
        {
            //duong dan file encryption key

            if (model.maphongban == "71" || model.maphongban == "74")
            {
                model.phongban_congtruong = "True";
            }


            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.machamcong.ToString(), function.ReadXMLGetKeyEncrypt());
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.machamcong.ToString(), function.ReadXMLGetKeyEncrypt()) + "\"},{\"name\":\"" + "ngaylamviectungay" + "\", \"value\":\"" + model.ngaylamviectu + "\"},{\"name\":\"" + "ngaylamviecden" + "\", \"value\":\"" + model.ngaylamviecden + "\"} ],");

                sbResult.Append("\"col_value\":[");

                #region Data cell
                //colum checkbox
                //string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}' machamcong='" + strEncryptCode + "'/>", model.machamcong);
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"rowspan\":\"2\",");
                sbResult.Append("\"col_class\":\"ovh col32\",");
                sbResult.Append("\"col_id\":\"32\",");
                sbResult.Append("\"col_value\":\"" + couter + "\"");
                sbResult.Append("},");
                //stt

                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col56\",");
                sbResult.Append("\"col_id\":\"56\",");
                sbResult.Append("\"col_value\":\"" + model.daduyet_ten + "\"");
                sbResult.Append("},");

                //dinh kem tap tin
                string strHTML_Attachment = "";
                #region
                //string link = Url.Action("Edit", "Account", new { id = EncDec.EncodeCrypto(model.mataikhoan) });
                strHTML_Attachment = "<a href='#' class='del'><i class='fa fa-trash-o' ></i></a>";
                #endregion
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col55\",");
                sbResult.Append("\"col_id\":\"55\",");
                sbResult.Append("\"title\":\"" + model.machamcong.ToString() + "\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Attachment + "\"");
                sbResult.Append("},");

                sbResult.Append("{");
                sbResult.Append("\"rowspan\":\"2\",");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col33\",");
                sbResult.Append("\"col_id\":\"33\",");
                sbResult.Append("\"title\":\"" + model.email + "\",");
                sbResult.Append("\"col_value\":\"" + model.manhanvien.Trim() + "\"");
                sbResult.Append("},");


                sbResult.Append("{");
                sbResult.Append("\"rowspan\":\"2\",");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col34\",");
                sbResult.Append("\"col_id\":\"34\",");
                sbResult.Append("\"title\":\"" + model.ngaysinh.Split(' ')[0].Trim() + "\",");
                sbResult.Append("\"col_value\":\"" + "<a href='" + Url.Action("Edit", "Timekeeping", new { mccchamcong = strEncryptCode, manhanvien = model.manhanvien }) + "' title='" + model.hovaten + "'>" + model.hovaten + "</a>\"");
                sbResult.Append("},");

                ////Phong ban
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col35\",");
                sbResult.Append("\"col_id\":\"35\",");
                sbResult.Append("\"title\":\"" + model.maphongban + "\",");
                sbResult.Append("\"col_value\":\"" + model.tenphongban.Trim() + "\"");
                sbResult.Append("},");

                // Ngày chấm công của tháng từ 21/01-20/02
                if (model.daduyet.Trim() == "0" || model.daduyet.Trim() == "1" || model.daduyet.Trim() == "2"|| model.daduyet.Trim() == "3")
                {
                    for (int i = 21; i <= songaytrongthang; i++) //
                    {
                        #region
                        sbResult.Append("{");
                        sbResult.Append("\"colspan\":\"1\",");
                        string kq = "";
                        if (i == 21) kq = model.ngay21.Trim();
                        else if (i == 22) kq = model.ngay22.Trim();
                        else if (i == 23) kq = model.ngay23.Trim();
                        else if (i == 24) kq = model.ngay24.Trim();
                        else if (i == 25) kq = model.ngay25.Trim();
                        else if (i == 26) kq = model.ngay26.Trim();
                        else if (i == 27) kq = model.ngay27.Trim();
                        else if (i == 28) kq = model.ngay28.Trim();
                        else if (i == 29) kq = model.ngay29.Trim();
                        else if (i == 30) kq = model.ngay30.Trim();
                        else if (i == 31) kq = model.ngay31.Trim();

                        if (kq == "F" || kq == "F/2" || kq == "Fo" || kq == "Fo/2" || kq == "TS" || kq == "TS/2" || kq == "Ro/2" || kq == "Ro")
                            sbResult.Append("\"col_class\":\"ovhnghi col" + i + "\",");
                        else if (kq == "X/2" || kq == "L")
                        {
                            if (model.phongban_congtruong == "1" && kq == "X/2" && chkchamcong!="1")
                            {
                                kq = "X";
                                sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");
                            }
                            else sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");

                        }

                        else if (kq == "CN")
                            sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");
                        else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                        sbResult.Append("\"col_id\":\"" + i + "\",");

                        if (i == 21) sbResult.Append("\"col_value\":\"" + (model.ngay21 = kq) + "\"");
                        else if (i == 22) sbResult.Append("\"col_value\":\"" + (model.ngay22 = kq) + "\"");
                        else if (i == 23) sbResult.Append("\"col_value\":\"" + (model.ngay23 = kq) + "\"");
                        else if (i == 24) sbResult.Append("\"col_value\":\"" + (model.ngay24 = kq) + "\"");
                        else if (i == 25) sbResult.Append("\"col_value\":\"" + (model.ngay25 = kq) + "\"");
                        else if (i == 26) sbResult.Append("\"col_value\":\"" + (model.ngay26 = kq) + "\"");
                        else if (i == 27) sbResult.Append("\"col_value\":\"" + (model.ngay27 = kq) + "\"");
                        else if (i == 28) sbResult.Append("\"col_value\":\"" + (model.ngay28 = kq) + "\"");
                        else if (i == 29) sbResult.Append("\"col_value\":\"" + (model.ngay29 = kq) + "\"");
                        else if (i == 30) sbResult.Append("\"col_value\":\"" + (model.ngay30 = kq) + "\"");
                        else if (i == 31) sbResult.Append("\"col_value\":\"" + (model.ngay31 = kq) + "\"");
                        sbResult.Append("},");
                        #endregion
                    }

                    for (int i = 1; i <= 20; i++)
                    {
                        #region
                        sbResult.Append("{");
                        sbResult.Append("\"colspan\":\"1\",");

                        string kq = "";
                        if (i == 1) kq = model.ngay1.Trim();
                        else if (i == 2) kq = model.ngay2.Trim();
                        else if (i == 3) kq = model.ngay3.Trim();
                        else if (i == 4) kq = model.ngay4.Trim();
                        else if (i == 5) kq = model.ngay5.Trim();
                        else if (i == 6) kq = model.ngay6.Trim();
                        else if (i == 7) kq = model.ngay7.Trim();
                        else if (i == 8) kq = model.ngay8.Trim();
                        else if (i == 9) kq = model.ngay9.Trim();
                        else if (i == 10) kq = model.ngay10.Trim();
                        else if (i == 11) kq = model.ngay11.Trim();
                        else if (i == 12) kq = model.ngay12.Trim();
                        else if (i == 13) kq = model.ngay13.Trim();
                        else if (i == 14) kq = model.ngay14.Trim();
                        else if (i == 15) kq = model.ngay15.Trim();
                        else if (i == 16) kq = model.ngay16.Trim();
                        else if (i == 17) kq = model.ngay17.Trim();
                        else if (i == 18) kq = model.ngay18.Trim();
                        else if (i == 19) kq = model.ngay19.Trim();
                        else if (i == 20) kq = model.ngay20.Trim();

                        if (kq == "F" || kq == "F/2" || kq == "Fo" || kq == "Fo/2" || kq == "TS" || kq == "TS/2" || kq == "Ro/2" || kq == "Ro")
                            sbResult.Append("\"col_class\":\"ovhnghi col" + i + "\",");
                        else if (kq == "X/2" || kq == "L")
                            sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");
                        else if (kq == "CN")
                            sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");
                        else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                        sbResult.Append("\"col_id\":\"" + i + "\",");

                        if (i == 1) sbResult.Append("\"col_value\":\"" + (model.ngay1 = kq) + "\"");
                        else if (i == 2) sbResult.Append("\"col_value\":\"" + (model.ngay2 = kq) + "\"");
                        else if (i == 3) sbResult.Append("\"col_value\":\"" + (model.ngay3 = kq) + "\"");
                        else if (i == 4) sbResult.Append("\"col_value\":\"" + (model.ngay4 = kq) + "\"");
                        else if (i == 5) sbResult.Append("\"col_value\":\"" + (model.ngay5 = kq) + "\"");
                        else if (i == 6) sbResult.Append("\"col_value\":\"" + (model.ngay6 = kq) + "\"");
                        else if (i == 7) sbResult.Append("\"col_value\":\"" + (model.ngay7 = kq) + "\"");
                        else if (i == 8) sbResult.Append("\"col_value\":\"" + (model.ngay8 = kq) + "\"");
                        else if (i == 9) sbResult.Append("\"col_value\":\"" + (model.ngay9 = kq) + "\"");
                        else if (i == 10) sbResult.Append("\"col_value\":\"" + (model.ngay10 = kq) + "\"");
                        else if (i == 11) sbResult.Append("\"col_value\":\"" + (model.ngay11 = kq) + "\"");
                        else if (i == 12) sbResult.Append("\"col_value\":\"" + (model.ngay12 = kq) + "\"");
                        else if (i == 13) sbResult.Append("\"col_value\":\"" + (model.ngay13 = kq) + "\"");
                        else if (i == 14) sbResult.Append("\"col_value\":\"" + (model.ngay14 = kq) + "\"");
                        else if (i == 15) sbResult.Append("\"col_value\":\"" + (model.ngay15 = kq) + "\"");
                        else if (i == 16) sbResult.Append("\"col_value\":\"" + (model.ngay16 = kq) + "\"");
                        else if (i == 17) sbResult.Append("\"col_value\":\"" + (model.ngay17 = kq) + "\"");
                        else if (i == 18) sbResult.Append("\"col_value\":\"" + (model.ngay18 = kq) + "\"");
                        else if (i == 19) sbResult.Append("\"col_value\":\"" + (model.ngay19 = kq) + "\"");
                        else if (i == 20) sbResult.Append("\"col_value\":\"" + (model.ngay20 = kq) + "\"");
                        sbResult.Append("},");
                        #endregion
                    }
                    for (int i = 36; i <= 50; i++)
                    {
                        #region
                        sbResult.Append("{");
                        sbResult.Append("\"colspan\":\"1\",");
                        sbResult.Append("\"col_class\":\"ovh col" + i + "\",");
                        sbResult.Append("\"col_id\":\"" + i + "\",");

                        if (i == 36) sbResult.Append("\"col_value\":\"" + model.ngaycong_le + "\"");
                        else if (i == 37) sbResult.Append("\"col_value\":\"" + model.nghiphep + "\"");
                        else if (i == 38) sbResult.Append("\"col_value\":\"" + model.nghihieu_hi + "\"");
                        else if (i == 39) sbResult.Append("\"col_value\":\"" + model.nghiom_thaisan + "\"");
                        else if (i == 40) sbResult.Append("\"col_value\":\"" + model.nghikhongluong + "\"");
                        else if (i == 41) sbResult.Append("\"col_value\":\"" + model.songayduoc_traluong + "\"");
                        else if (i == 42) sbResult.Append("\"col_value\":\"" + model.songaypc_tiencom + "\"");
                        else if (i == 43) sbResult.Append("\"col_value\":\"" + model.songay_congchuan + "\"");

                        else if (i == 44) sbResult.Append("\"col_value\":\"" + model.chenhlech_ngaycong + "\"");
                        else if (i == 45) sbResult.Append("\"col_value\":\"" + model.tieuchi + "\"");

                        else if (i == 46) sbResult.Append("\"col_value\":\"" + model.hoanthanh_congviec + "\"");

                        else if (i == 47) sbResult.Append("\"col_value\":\"" + model.tinhthan_trachnhiem + "\"");
                        else if (i == 48) sbResult.Append("\"col_value\":\"" + model.cuxu_hoptac + "\"");
                        else if (i == 49) sbResult.Append("\"col_value\":\"" + model.tuanthu_giogiac + "\"");
                        else if (i == 50) sbResult.Append("\"col_value\":\"" + model.xeploai + "\"");
                        sbResult.Append("},");
                        #endregion
                    }
                }
                else
                {
                    for (int i = 21; i <= songaytrongthang; i++) //
                    {
                        #region
                        sbResult.Append("{");
                        sbResult.Append("\"colspan\":\"1\",");

                        string kq = "";

                        if (thang==1)
                            kq = kiemtrangay(i + "/12"  + "/" + (nam-1).ToString(), model.manhanvien, model.phongban_congtruong);
                        else
                            kq = kiemtrangay(i + "/" + (thang - 1).ToString("00") + "/" + nam, model.manhanvien, model.phongban_congtruong);


                        if (kq == "F" || kq == "F/2" || kq == "Fo" || kq == "Fo/2"
                                 || kq == "TS" || kq == "TS/2" || kq == "Ro/2" || kq == "Ro")
                            sbResult.Append("\"col_class\":\"ovhnghi col" + i + "\",");

                        else if (thutrongtuan == "T7")
                            sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");

                        else if (thutrongtuan == "CN")
                            sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");

                        else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                        sbResult.Append("\"col_id\":\"" + i + "\",");
                        int thangsau = thang - 1;

                        if (i == 21)
                        {
                            sbResult.Append("\"col_value\":\"" + (model.ngay21 = kq) + "\"");
                        }
                        else if (i == 22) sbResult.Append("\"col_value\":\"" + (model.ngay22 = kq) + "\"");
                        else if (i == 23) sbResult.Append("\"col_value\":\"" + (model.ngay23 = kq) + "\"");
                        else if (i == 24) sbResult.Append("\"col_value\":\"" + (model.ngay24 = kq) + "\"");
                        else if (i == 25) sbResult.Append("\"col_value\":\"" + (model.ngay25 = kq) + "\"");
                        else if (i == 26) sbResult.Append("\"col_value\":\"" + (model.ngay26 = kq) + "\"");
                        else if (i == 27) sbResult.Append("\"col_value\":\"" + (model.ngay27 = kq) + "\"");
                        else if (i == 28) sbResult.Append("\"col_value\":\"" + (model.ngay28 = kq) + "\"");
                        else if (i == 29) sbResult.Append("\"col_value\":\"" + (model.ngay29 = kq) + "\"");
                        else if (i == 30) sbResult.Append("\"col_value\":\"" + (model.ngay30 = kq) + "\"");
                        else if (i == 31) sbResult.Append("\"col_value\":\"" + (model.ngay31 = kq) + "\"");
                        sbResult.Append("},");
                        #endregion
                    }

                    for (int i = 1; i <= 20; i++)
                    {
                        #region
                        sbResult.Append("{");
                        sbResult.Append("\"colspan\":\"1\",");

                        string kq = kiemtrangay(i.ToString("00") + "/" + thang.ToString("00") + "/" + nam.ToString("0000"), model.manhanvien, model.phongban_congtruong);

                        if (kq == "F" || kq == "F/2" || kq == "Fo" || kq == "Fo/2"
                                || kq == "TS" || kq == "TS/2" || kq == "Ro/2" || kq == "Ro")
                            sbResult.Append("\"col_class\":\"ovhnghi col" + i + "\",");

                        else if (thutrongtuan == "T7")
                            sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");
                        else if (thutrongtuan == "CN")
                            sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");

                        else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                        sbResult.Append("\"col_id\":\"" + i + "\",");
                        if (i == 1) sbResult.Append("\"col_value\":\"" + (model.ngay1 = kq) + "\"");
                        else if (i == 2) sbResult.Append("\"col_value\":\"" + (model.ngay2 = kq) + "\"");
                        else if (i == 3) sbResult.Append("\"col_value\":\"" + (model.ngay3 = kq) + "\"");
                        else if (i == 4) sbResult.Append("\"col_value\":\"" + (model.ngay4 = kq) + "\"");
                        else if (i == 5) sbResult.Append("\"col_value\":\"" + (model.ngay5 = kq) + "\"");
                        else if (i == 6) sbResult.Append("\"col_value\":\"" + (model.ngay6 = kq) + "\"");
                        else if (i == 7) sbResult.Append("\"col_value\":\"" + (model.ngay7 = kq) + "\"");
                        else if (i == 8) sbResult.Append("\"col_value\":\"" + (model.ngay8 = kq) + "\"");
                        else if (i == 9) sbResult.Append("\"col_value\":\"" + (model.ngay9 = kq) + "\"");
                        else if (i == 10) sbResult.Append("\"col_value\":\"" + (model.ngay10 = kq) + "\"");
                        else if (i == 11) sbResult.Append("\"col_value\":\"" + (model.ngay11 = kq) + "\"");
                        else if (i == 12) sbResult.Append("\"col_value\":\"" + (model.ngay12 = kq) + "\"");
                        else if (i == 13) sbResult.Append("\"col_value\":\"" + (model.ngay13 = kq) + "\"");
                        else if (i == 14) sbResult.Append("\"col_value\":\"" + (model.ngay14 = kq) + "\"");
                        else if (i == 15) sbResult.Append("\"col_value\":\"" + (model.ngay15 = kq) + "\"");
                        else if (i == 16) sbResult.Append("\"col_value\":\"" + (model.ngay16 = kq) + "\"");
                        else if (i == 17) sbResult.Append("\"col_value\":\"" + (model.ngay17 = kq) + "\"");
                        else if (i == 18) sbResult.Append("\"col_value\":\"" + (model.ngay18 = kq) + "\"");
                        else if (i == 19) sbResult.Append("\"col_value\":\"" + (model.ngay19 = kq) + "\"");
                        else if (i == 20) sbResult.Append("\"col_value\":\"" + (model.ngay20 = kq) + "\"");
                        sbResult.Append("},");
                        #endregion
                    }
                    for (int i = 36; i <= 50; i++)
                    {
                        #region
                        sbResult.Append("{");
                        sbResult.Append("\"colspan\":\"1\",");
                        sbResult.Append("\"col_class\":\"ovh col" + i + "\",");
                        sbResult.Append("\"col_id\":\"" + i + "\",");

                        ngaycong_le = songaycongchuan - nghiphep - nghihieuhi - nghiom_ts - nghikhongluong;
                        songayduoctraluong = songaycongchuan - nghiom_ts - nghikhongluong;

                        if (i == 36) sbResult.Append("\"col_value\":\"" + (model.ngaycong_le = ngaycong_le.ToString()) + "\"");
                        else if (i == 37) sbResult.Append("\"col_value\":\"" + (model.nghiphep = nghiphep.ToString()) + "\"");
                        else if (i == 38) sbResult.Append("\"col_value\":\"" + (model.nghihieu_hi = nghihieuhi.ToString()) + "\"");
                        else if (i == 39) sbResult.Append("\"col_value\":\"" + (model.nghiom_thaisan = nghiom_ts.ToString()) + "\"");
                        else if (i == 40) sbResult.Append("\"col_value\":\"" + (model.nghikhongluong = nghikhongluong.ToString()) + "\"");
                        else if (i == 41) sbResult.Append("\"col_value\":\"" + (model.songayduoc_traluong = songayduoctraluong.ToString()) + "\"");
                        else if (i == 42) sbResult.Append("\"col_value\":\"" + (model.songaypc_tiencom = snphucapcom.ToString()) + "\"");
                        else if (i == 43) sbResult.Append("\"col_value\":\"" + (model.songay_congchuan = songaycongchuan.ToString()) + "\"");

                        else if (i == 44) sbResult.Append("\"col_value\":\"" + (model.chenhlech_ngaycong = "0") + "\"");
                        else if (i == 45) sbResult.Append("\"col_value\":\"" + (model.tieuchi = "Mức độ") + "\"");

                        else if (i == 46)
                        {
                            if (model.hoanthanh_congviec == "") model.hoanthanh_congviec = "4";
                            sbResult.Append("\"col_value\":\"" + model.hoanthanh_congviec + "\"");
                        }

                        else if (i == 47)
                        {
                            if (model.tinhthan_trachnhiem == "") model.tinhthan_trachnhiem = "4";
                            sbResult.Append("\"col_value\":\"" + model.tinhthan_trachnhiem + "\"");
                        }
                        else if (i == 48)
                        {
                            if (model.cuxu_hoptac == "") model.cuxu_hoptac = "4";
                            sbResult.Append("\"col_value\":\"" + model.cuxu_hoptac + "\"");
                        }
                        else if (i == 49)
                        {
                            if (model.tuanthu_giogiac == "") model.tuanthu_giogiac = "5";
                            sbResult.Append("\"col_value\":\"" + model.tuanthu_giogiac + "\"");
                        }
                        else if (i == 50)
                        {
                            if (model.xeploai == "") model.xeploai = "A";
                            sbResult.Append("\"col_value\":\"" + model.xeploai + "\"");
                        }
                        sbResult.Append("},");
                        #endregion
                    }
                }

                //Ghi chú
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col51\",");
                sbResult.Append("\"col_id\":\"51\",");
                sbResult.Append("\"col_value\":\"" + model.ghichu.Trim() + "\"");
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

        public ActionResult Create()
        {
            if (!IsLogged())
                return BackToLogin();
            idmachamcong = "0";
            DanhmucServices service = new DanhmucServices();
            PhongBanModels parampb = new PhongBanModels();
            lstResult_phongban = new List<PhongBanModels>();
            lstResult_phongban = service.SelectRows(parampb);
            StringBuilder sbphongban = new StringBuilder();
            foreach (var item in lstResult_phongban)
            {
                sbphongban.Append(string.Format("<option value='{0}'>{1}</option>", item.maphongban, item.tenphongban));
            }
            ViewBag.sbphongban = sbphongban.ToString();
            return View();
        }

        public static List<TimekeepingModels> lsthieuchinhchamcong = new List<TimekeepingModels>();

        public ActionResult Edit(string mccchamcong, string manhanvien)
        {
            if (!IsLogged())
                return BackToLogin();
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            mccchamcong = AES.DecryptText(mccchamcong, function.ReadXMLGetKeyEncrypt());
            idmachamcong = mccchamcong;
            DanhmucServices service = new DanhmucServices();
            PhongBanModels parampb = new PhongBanModels();
            lstResult_phongban = new List<PhongBanModels>();
            lstResult_phongban = service.SelectRows(parampb);
            StringBuilder sbphongban = new StringBuilder();
            foreach (var item in lstResult_phongban)
            {
                sbphongban.Append(string.Format("<option value='{0}'>{1}</option>", item.maphongban, item.tenphongban));
            }
            ViewBag.sbphongban = sbphongban.ToString();

            TimekeepingServices service_cc = new TimekeepingServices();
            lsthieuchinhchamcong = new List<TimekeepingModels>();
            lsthieuchinhchamcong = service_cc.SelectRows_hieuchinh_machamcong(idmachamcong, manhanvien);
            if (lsthieuchinhchamcong.Count > 0)
            {
                try
                {
                    lsthieuchinhchamcong[0].ngaysinh = lsthieuchinhchamcong[0].ngaysinh.Split(' ')[0];

                    if (thangnam != "" && (lsthieuchinhchamcong[0].ngaylamviectu.Trim() == "" || lsthieuchinhchamcong[0].ngaylamviectu.Trim() == null)
                        && (lsthieuchinhchamcong[0].ngaylamviecden.Trim() == "" || lsthieuchinhchamcong[0].ngaylamviecden.Trim() == null))
                    {
                        string[] thangnams = thangnam.Split('/');
                        lsthieuchinhchamcong[0].ngaylamviectu = "21/" + (int.Parse(thangnams[0].ToString()) - 1).ToString("00") + "/" + thangnams[1];
                        lsthieuchinhchamcong[0].ngaylamviecden = "20/" + thangnam;
                    }
                    if (lsthieuchinhchamcong[0].thang.Trim() == "" || lsthieuchinhchamcong[0].thang.Trim() == null)
                        lsthieuchinhchamcong[0].thang = thangnam;

                    if (lsthieuchinhchamcong[0].tieuchi.Trim() == "" || lsthieuchinhchamcong[0].tieuchi.Trim() == null)
                        lsthieuchinhchamcong[0].tieuchi = "Mức độ";

                    if (lsthieuchinhchamcong[0].hoanthanh_congviec.Trim() == "" || lsthieuchinhchamcong[0].hoanthanh_congviec.Trim() == null)
                        lsthieuchinhchamcong[0].hoanthanh_congviec = "4";

                    if (lsthieuchinhchamcong[0].tinhthan_trachnhiem.Trim() == "" || lsthieuchinhchamcong[0].tinhthan_trachnhiem.Trim() == null)
                        lsthieuchinhchamcong[0].tinhthan_trachnhiem = "4";

                    if (lsthieuchinhchamcong[0].cuxu_hoptac.Trim() == "" || lsthieuchinhchamcong[0].cuxu_hoptac.Trim() == null)
                        lsthieuchinhchamcong[0].cuxu_hoptac = "4";

                    if (lsthieuchinhchamcong[0].tuanthu_giogiac.Trim() == "" || lsthieuchinhchamcong[0].tuanthu_giogiac.Trim() == null)
                        lsthieuchinhchamcong[0].tuanthu_giogiac = "5";

                    if (lsthieuchinhchamcong[0].xeploai.Trim() == "" || lsthieuchinhchamcong[0].xeploai.Trim() == null)
                        lsthieuchinhchamcong[0].xeploai = "A";

                }
                catch (Exception) { }

                var phongban_congtruongchon = lstResult_phongban.Where(p => p.maphongban == lsthieuchinhchamcong[0].maphongban).ToList();
                if (phongban_congtruongchon.Count > 0)
                {
                    phongban_congtruong = phongban_congtruongchon[0].phongban_congtruong;
                    lsthieuchinhchamcong[0].phongban_congtruong = phongban_congtruongchon[0].phongban_congtruong.ToString().Trim();
                }
                    
                return View(lsthieuchinhchamcong[0]);
            }

            return View();
        }

        //public static double tongngaycong = 0;

        private string kiemtrangay_themmoi(string ngaythangnam, int phongban_congtruongs)
        {
            string kq = "X";
            
            var ktngayle = listHoliday.Where(p => p.datevalue == ngaythangnam.Trim());
            DateTime date = FunctionsDateTime.ConvertStringToDate(ngaythangnam);
            string thucuatuan = thutrongtuan = FunctionsDateTime.DayOfWeek_CN(date);

            string ngayle = ngaythangnam.Substring(0, 5);

            if ((ktngayle.Count() > 0 && thucuatuan != "T7" && thucuatuan != "CN") || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le1 = ngaycong_le1 + 1;
                songaycongchuan1 = songaycongchuan1 + 1;
            }
            else if ((ktngayle.Count() > 0 && thucuatuan == "T7" && phongban_congtruongs==0) || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le1 = ngaycong_le1 + 0.5;
                songaycongchuan1 = songaycongchuan1 + 0.5;
            }

            else if ((ktngayle.Count() > 0 && thucuatuan == "T7" && phongban_congtruongs==1) || (ngayle == "01/01" || ngayle == "30/04" || ngayle == "01/05" || ngayle == "02/09"))
            {
                kq = "L";
                ngaycong_le1 = ngaycong_le1 + 1;
                songaycongchuan1 = songaycongchuan1 + 1;
            }

            if (ktngayle.Count() > 0 && thucuatuan != "T7" && thucuatuan != "CN")
            {
                kq = "L";
                
            }
            else if (ktngayle.Count() > 0 && thucuatuan == "T7" && phongban_congtruongs == 1)
            {
                kq = "L";
              
            }
            else if (ktngayle.Count() > 0 && thucuatuan == "T7" && phongban_congtruongs == 0)
            {
                kq = "L";
               
            }
                
            else
            {
                if (thucuatuan == "CN") kq = "CN";
                else if (thucuatuan == "T7")
                {
                    if (phongban_congtruongs == 1)
                    {
                        kq = "X";
                        ngaycong_le1 = ngaycong_le1 + 1;
                        songaycongchuan1 = songaycongchuan1 + 1;
                    }
                    else
                    {
                        kq = "X/2";
                        ngaycong_le1 = ngaycong_le1 + 0.5;
                        songaycongchuan1 = songaycongchuan1 + 0.5;
                    } 
                }
                else
                {
                    kq = "X";
                    ngaycong_le1 = ngaycong_le1 + 1;
                    songaycongchuan1 = songaycongchuan1 + 1;
                }
                    
            }
            return kq;
        }

        [HttpPost]
        public JsonResult SelectRows_themmoi(TimekeepingModels model)
        {
            TimekeepingModels param = new TimekeepingModels();
            TimekeepingServices service = new TimekeepingServices();
            int phongban_congtruongs=0;
            var phongban_congtruongchon = lstResult_phongban.Where(p => p.maphongban == model.maphongban).ToList();
            if (phongban_congtruongchon.Count > 0)
                phongban_congtruongs = phongban_congtruongchon[0].phongban_congtruong;

            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();

            ngaycong_le1 = 0;
            nghiphep1 = 0;
            nghihieuhi1 = 0;
            nghiom_ts1 = 0;
            nghikhongluong1 = 0;
            songayduoctraluong1 = 0;
            snphucapcom1 = 0;
            songaycongchuan1 = 0;

            if (model.machamcong != 0 && model.ngaylamviectu.Trim().Length == 10 && model.ngaylamviecden.Trim().Length == 10 && lsthieuchinhchamcong.Count > 0)
            {
                //string thangchamcong = model.thang;
                #region
                lstResult = lsthieuchinhchamcong;
                //lstResult[0].thang = thangchamcong;

                lstResult[0].ngaylamviectu = model.ngaylamviectu.Trim();
                lstResult[0].ngaylamviecden = model.ngaylamviecden.Trim();

                int thang = int.Parse(lstResult[0].thang.Split('/')[0]);
                int nam = int.Parse(lstResult[0].thang.Split('/')[1]);

                int songaytrongthang = 31;

                if (thang == 1) songaytrongthang = songay(12, nam - 1);
                else songaytrongthang = songay(thang - 1, nam);

                if (lstResult.Count > 0)
                {
                    foreach (var item in lstResult)
                    {
                        item.songaycuathang = songaytrongthang;
                        sbRows.Append(PrepareDataJson_themmoi_machamcong(item, songaytrongthang, phongban_congtruongs));
                    }
                    if (sbRows.Length > 0)
                        sbRows.Remove(sbRows.Length - 1, 1);
                }
                #endregion
            }
            else
            {
                #region

                int thang = int.Parse(DateTime.Now.Month.ToString("00"));
                int nam = int.Parse(DateTime.Now.Year.ToString("0000"));
                if (model.thang == null || model.thang == "")
                {
                    int ngaykt = int.Parse(DateTime.Now.Day.ToString());
                    if (ngaykt > 20)
                        thang = int.Parse(DateTime.Now.Month.ToString("00")) + 1;
                    else thang = int.Parse(DateTime.Now.Month.ToString("00"));
                    nam = int.Parse(DateTime.Now.Year.ToString("0000"));

                    if (thangnam != "")
                    {
                        string[] thangnams = thangnam.Split('/');
                        model.ngaylamviectu = "21/" + (int.Parse(thangnams[0].ToString()) - 1).ToString("00") + "/" + thangnams[1];
                        model.ngaylamviecden = "20/" + thangnam;
                    }
                    else
                    {
                        model.ngaylamviectu = "21/" + (thang - 1).ToString("00") + "/" + nam;
                        model.ngaylamviectu = "20/" + thang.ToString("00") + "/" + nam;
                    }

                }
                else if (model.thang.Trim().Length == 7)
                {
                    thang = int.Parse(model.thang.Split('/')[0]);
                    nam = int.Parse(model.thang.Split('/')[1]);
                }


                int songaytrongthang = 31;
                if (thang == 1)songaytrongthang = songay(12, nam-1);
                else songaytrongthang = songay(thang - 1, nam);
                loaddulieu_ngayle_thang(thang, nam);
                if(model.manhanvien!=null && model.manhanvien!="")
                    loaddulieu_xinnghiphep_thang(model.manhanvien, thang, nam);

                lstResult.Add(new TimekeepingModels() { maphongban = model.maphongban, manhanvien = model.manhanvien, ngaylamviectu = model.ngaylamviectu, ngaylamviecden = model.ngaylamviecden });
                if (lstResult.Count > 0)
                {
                    string strSTT = "";
                    foreach (var item in lstResult)
                    {
                        item.songaycuathang = songaytrongthang;
                        sbRows.Append(PrepareDataJson_themmoi(item, strSTT, songaytrongthang, thang, nam, phongban_congtruongs));
                    }
                    if (sbRows.Length > 0)
                        sbRows.Remove(sbRows.Length - 1, 1);
                }
                #endregion
            }

            
            
            
            sbResult.Append("{");
            sbResult.Append("\"phongban_congtruong\":\"" + "" + phongban_congtruong + "" + "\",");

            if (phongban_congtruong == 0)
            {
                ngaycong_le1 = songaycongchuan1 - nghiphep1 - nghihieuhi1 - nghiom_ts1 - nghikhongluong1;
                songayduoctraluong1 = songaycongchuan1 - nghiom_ts1 - nghikhongluong1;
            }

            else if (phongban_congtruong == 1)
            {
                ngaycong_le1 = songaycongchuan1 - nghiphep1 - nghihieuhi1 - nghiom_ts1 - nghikhongluong1;
                songayduoctraluong1 = songaycongchuan1 - nghiom_ts1 - nghikhongluong1;
            }
            sbResult.Append("\"ngaycong_le1\":\"" + "" + ngaycong_le1 + "" + "\",");
            sbResult.Append("\"nghiphep1\":\"" + "" + nghiphep1 + "" + "\",");
            sbResult.Append("\"nghihieuhi1\":\"" + "" + nghihieuhi1 + "" + "\",");
            sbResult.Append("\"nghiom_ts1\":\"" + "" + nghiom_ts1 + "" + "\",");
            sbResult.Append("\"nghikhongluong1\":\"" + "" + nghikhongluong1 + "" + "\",");
            sbResult.Append("\"songayduoctraluong1\":\"" + "" + songayduoctraluong1 + "" + "\",");
            sbResult.Append("\"snphucapcom1\":\"" + "" + snphucapcom1 + "" + "\",");
            sbResult.Append("\"songaycongchuan1\":\"" + "" + songaycongchuan1 + "" + "\",");
                 double chenhlechngaycong=0;
                        try{
                            chenhlechngaycong = songaycongchuan1 - ngaycong_le1;
                        }
                        catch(Exception){}

            sbResult.Append("\"chenhlechngaycong1\":\"" + "" + chenhlechngaycong + "" + "\",");
            sbResult.Append("\"songaytrongthang\":\"" + 30 + "\",");
            sbResult.Append("\"thangnam\":\"" + DateTime.Now.ToString("MM/yyyy") + "\",");

          
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SelectRows_themmoi_Edit(TimekeepingModels model)
        {
            TimekeepingModels param = new TimekeepingModels();
            TimekeepingServices service = new TimekeepingServices();
            int phongban_congtruongs = 0;
            var phongban_congtruongchon = lstResult_phongban.Where(p => p.maphongban == model.maphongban).ToList();
            if (phongban_congtruongchon.Count > 0)
                phongban_congtruongs = phongban_congtruongchon[0].phongban_congtruong;

            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();

            ngaycong_le1 = 0;
            nghiphep1 = 0;
            nghihieuhi1 = 0;
            nghiom_ts1 = 0;
            nghikhongluong1 = 0;
            songayduoctraluong1 = 0;
            snphucapcom1 = 0;
            songaycongchuan1 = 0;


            #region

            int thang = int.Parse(DateTime.Now.Month.ToString("00"));
            int nam = int.Parse(DateTime.Now.Year.ToString("0000"));
            if (model.thang == null || model.thang == "")
            {
                int ngaykt = int.Parse(DateTime.Now.Day.ToString());
                if (ngaykt > 20)
                    thang = int.Parse(DateTime.Now.Month.ToString("00")) + 1;
                else thang = int.Parse(DateTime.Now.Month.ToString("00"));
                nam = int.Parse(DateTime.Now.Year.ToString("0000"));

                if (thangnam != "")
                {
                    string[] thangnams = thangnam.Split('/');
                    model.ngaylamviectu = "21/" + (int.Parse(thangnams[0].ToString()) - 1).ToString("00") + "/" + thangnams[1];
                    model.ngaylamviecden = "20/" + thangnam;
                }
                else
                {
                    model.ngaylamviectu = "21/" + (thang - 1).ToString("00") + "/" + nam;
                    model.ngaylamviectu = "20/" + thang.ToString("00") + "/" + nam;
                }

            }
            else if (model.thang.Trim().Length == 7)
            {
                thang = int.Parse(model.thang.Split('/')[0]);
                nam = int.Parse(model.thang.Split('/')[1]);
            }


            int songaytrongthang = 31;
            if (thang == 1) songaytrongthang = songay(12, nam - 1);
            else songaytrongthang = songay(thang - 1, nam);
            loaddulieu_ngayle_thang(thang, nam);
            if (model.manhanvien != null && model.manhanvien != "")
                loaddulieu_xinnghiphep_thang(model.manhanvien, thang, nam);

            lstResult.Add(new TimekeepingModels() { maphongban = model.maphongban, manhanvien = model.manhanvien, ngaylamviectu = model.ngaylamviectu, ngaylamviecden = model.ngaylamviecden });
            if (lstResult.Count > 0)
            {
                string strSTT = "";
                foreach (var item in lstResult)
                {
                    item.songaycuathang = songaytrongthang;
                    sbRows.Append(PrepareDataJson_themmoi(item, strSTT, songaytrongthang, thang, nam, phongban_congtruongs));
                }
                if (sbRows.Length > 0)
                    sbRows.Remove(sbRows.Length - 1, 1);
            }
            #endregion


            sbResult.Append("{");
            sbResult.Append("\"phongban_congtruong\":\"" + "" + phongban_congtruong + "" + "\",");

            if (phongban_congtruong == 0)
            {
                ngaycong_le1 = songaycongchuan1 - nghiphep1 - nghihieuhi1 - nghiom_ts1 - nghikhongluong1;
                songayduoctraluong1 = songaycongchuan1 - nghiom_ts1 - nghikhongluong1;
            }

            else if (phongban_congtruong == 1)
            {
                ngaycong_le1 = songaycongchuan1 - nghiphep1 - nghihieuhi1 - nghiom_ts1 - nghikhongluong1;
                songayduoctraluong1 = songaycongchuan1 - nghiom_ts1 - nghikhongluong1;
            }
            sbResult.Append("\"ngaycong_le1\":\"" + "" + ngaycong_le1 + "" + "\",");
            sbResult.Append("\"nghiphep1\":\"" + "" + nghiphep1 + "" + "\",");
            sbResult.Append("\"nghihieuhi1\":\"" + "" + nghihieuhi1 + "" + "\",");
            sbResult.Append("\"nghiom_ts1\":\"" + "" + nghiom_ts1 + "" + "\",");
            sbResult.Append("\"nghikhongluong1\":\"" + "" + nghikhongluong1 + "" + "\",");
            sbResult.Append("\"songayduoctraluong1\":\"" + "" + songayduoctraluong1 + "" + "\",");
            sbResult.Append("\"snphucapcom1\":\"" + "" + snphucapcom1 + "" + "\",");
            sbResult.Append("\"songaycongchuan1\":\"" + "" + songaycongchuan1 + "" + "\",");
            double chenhlechngaycong = 0;
            try
            {
                chenhlechngaycong = songaycongchuan1 - ngaycong_le1;
            }
            catch (Exception) { }

            sbResult.Append("\"chenhlechngaycong1\":\"" + "" + chenhlechngaycong + "" + "\",");
            sbResult.Append("\"songaytrongthang\":\"" + 30 + "\",");
            sbResult.Append("\"thangnam\":\"" + DateTime.Now.ToString("MM/yyyy") + "\",");


            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }



        private StringBuilder PrepareDataJson_themmoi(TimekeepingModels model, string couter, int songaytrongthang, int thang, int nam, int phongban_congtruongs)
        {
            //duong dan file encryption key
            if (model.maphongban == "71" || model.maphongban == "74")
            {
                model.phongban_congtruong = "True";
                phongban_congtruongs = 1;
            }
                
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.machamcong.ToString(), function.ReadXMLGetKeyEncrypt());
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.machamcong.ToString(), function.ReadXMLGetKeyEncrypt()) + "\"}],");
                sbResult.Append("\"col_value\":[");

                #region Data cell

                // Ngày chấm công của tháng từ 21/01-20/02

                for (int i = 21; i <= songaytrongthang; i++) //
                {
                    #region
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");

                    string kq = "X";
                    int ngaybdlv = 21;
                    int ngayktlv = songaytrongthang;
                    int thangbdlv = 0;
                    int thangktlv = 0;
                    if (model.ngaylamviectu.Trim().Length == 10)
                    {
                        try
                        {
                            ngaybdlv = int.Parse(model.ngaylamviectu.Split('/')[0]);
                            ngayktlv = int.Parse(model.ngaylamviecden.Split('/')[0]);
                            thangbdlv = int.Parse(model.ngaylamviectu.Split('/')[1]);
                            thangktlv = int.Parse(model.ngaylamviecden.Split('/')[1]);
                        }
                        catch (Exception) { ngaybdlv = i; }
                    }

                    if ((ngaybdlv > i && thang == 1) || (ngaybdlv < 20 && thang == 1))
                    {
                        kq = "0";
                    }
                    else if ((ngaybdlv > i && thangbdlv <= thang) || (ngaybdlv < 20 && thangbdlv == thang) || (ngayktlv < i && thangbdlv == thangktlv))
                        kq = "0";
                    else
                    {
                        if (thang == 1)
                        {
                            if (model.manhanvien == "" || model.manhanvien == null)
                                kq = kiemtrangay_themmoi(i + "/12" + "/" + (nam - 1).ToString(), phongban_congtruongs);
                            else kq = kiemtrangay_hieuchinh(i + "/12" + "/" + (nam - 1).ToString(), model.manhanvien.ToString(), phongban_congtruongs.ToString());

                        }
                        else
                        {
                            if (model.manhanvien == "" || model.manhanvien == null)
                                kq = kiemtrangay_themmoi(i + "/" + (thang - 1).ToString("00") + "/" + nam, phongban_congtruongs);
                            else kq = kiemtrangay_hieuchinh(i + "/" + (thang - 1).ToString("00") + "/" + nam, model.manhanvien.ToString(), phongban_congtruongs.ToString()); 
                        }
                            
                    }
                        

                    //if (kq == "F" || kq == "F/2" || kq == "Fo" || kq == "Fo/2"
                    //         || kq == "TS" || kq == "TS/2" || kq == "Ro/2" || kq == "Ro")
                    //    sbResult.Append("\"col_class\":\"ovhnghi col" + i + "\",");

                    if (thutrongtuan == "T7")
                        sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");

                    else if (thutrongtuan == "CN")
                        sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");

                    else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                    sbResult.Append("\"col_id\":\"" + i + "\",");
                    //int thangsau = thang - 1;

                    if (i == 21) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay21 = kq) + " ></input>" + "\"");
                    else if (i == 22) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay22 = kq) + " ></input>" + "\"");
                    else if (i == 23) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay23 = kq) + " ></input>" + "\"");
                    else if (i == 24) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay24 = kq) + " ></input>" + "\"");
                    else if (i == 25) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay25 = kq) + " ></input>" + "\"");
                    else if (i == 26) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay26 = kq) + " ></input>" + "\"");
                    else if (i == 27) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay27 = kq) + " ></input>" + "\"");
                    else if (i == 28) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay28 = kq) + " ></input>" + "\"");
                    else if (i == 29) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay29 = kq) + " ></input>" + "\"");
                    else if (i == 30) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay30 = kq) + " ></input>" + "\"");
                    else if (i == 31) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay31 = kq) + " ></input>" + "\"");
                    sbResult.Append("},");
                    #endregion
                }

                for (int i = 1; i <= 20; i++)
                {
                    #region
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");

                    string kq = "X";
                    int ngalvden = 20;
                    int ngaybdlv = 21;
                    int thangbdlv = 0;
                    int thangktlv = 0;
                    int nambd = 0;
                    int namkt = 0;
                    if (model.ngaylamviecden.Trim().Length == 10)
                    {
                        try
                        {
                            ngaybdlv = int.Parse(model.ngaylamviectu.Split('/')[0]);
                            ngalvden = int.Parse(model.ngaylamviecden.Split('/')[0]);
                            thangbdlv = int.Parse(model.ngaylamviectu.Split('/')[1]);
                            thangktlv = int.Parse(model.ngaylamviecden.Split('/')[1]);
                            nambd = int.Parse(model.ngaylamviectu.Split('/')[2]);
                            namkt = int.Parse(model.ngaylamviecden.Split('/')[2]);
                            
                        }
                        catch (Exception) { ngalvden = i; }
                        //if ((ngaybdlv < i && thangbdlv < thangktlv) || (ngaybdlv > i && thangbdlv == thangktlv) || (ngalvden < i && thangbdlv == thangktlv) || (ngalvden < i && thangbdlv < thangktlv))
                    }
                    if ((ngaybdlv < i && thang == 1 && thangbdlv == 12) || (ngalvden < i && thang == 1 && thangbdlv == 12))
                        kq = "0";

                    else if ((ngaybdlv < i && thangbdlv < thangktlv) || (ngaybdlv > i && thangbdlv == thangktlv) || (ngalvden < i && thangbdlv == thangktlv) || (ngalvden < i && thangbdlv < thangktlv))
                        kq = "0";
                    else
                    {
                        if (model.manhanvien == "" || model.manhanvien == null)
                            kq = kiemtrangay_themmoi(i.ToString("00") + "/" + (thang).ToString("00") + "/" + nam, phongban_congtruongs);
                        else kq = kiemtrangay_hieuchinh(i.ToString("00") + "/" + (thang).ToString("00") + "/" + nam, model.manhanvien.ToString(), phongban_congtruongs.ToString()); 
                    }
                        

                    if (thutrongtuan == "T7")
                        sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");

                    else if (thutrongtuan == "CN")
                        sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");

                    else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                    sbResult.Append("\"col_id\":\"" + i + "\",");

                    if (i == 1) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay1 = kq) + " ></input>" + "\"");
                    else if (i == 2) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay2 = kq) + " ></input>" + "\"");
                    else if (i == 3) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay3 = kq) + " ></input>" + "\"");
                    else if (i == 4) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay4 = kq) + " ></input>" + "\"");
                    else if (i == 5) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay5 = kq) + " ></input>" + "\"");
                    else if (i == 6) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay6 = kq) + " ></input>" + "\"");
                    else if (i == 7) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay7 = kq) + " ></input>" + "\"");
                    else if (i == 8) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay8 = kq) + " ></input>" + "\"");
                    else if (i == 9) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay9 = kq) + " ></input>" + "\"");
                    else if (i == 10) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay10 = kq) + " ></input>" + "\"");
                    else if (i == 11) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay11 = kq) + " ></input>" + "\"");
                    else if (i == 12) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay12 = kq) + " ></input>" + "\"");
                    else if (i == 13) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay13 = kq) + " ></input>" + "\"");
                    else if (i == 14) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay14 = kq) + " ></input>" + "\"");
                    else if (i == 15) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay15 = kq) + " ></input>" + "\"");
                    else if (i == 16) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay16 = kq) + " ></input>" + "\"");
                    else if (i == 17) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay17 = kq) + " ></input>" + "\"");
                    else if (i == 18) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay18 = kq) + " ></input>" + "\"");
                    else if (i == 19) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay19 = kq) + " ></input>" + "\"");
                    else if (i == 20) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + (model.ngay20 = kq) + " ></input>" + "\"");
                    if (i < 20)
                        sbResult.Append("},");
                    else sbResult.Append("}");
                    #endregion
                }

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

        private StringBuilder PrepareDataJson_themmoi_machamcong(TimekeepingModels model, int songaytrongthang, int phongban_congtruong)
        {
            //duong dan file encryption key
            FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
            StringBuilder sbResult = new StringBuilder();
            string strEncryptCode = AES.EncryptText(model.machamcong.ToString(), function.ReadXMLGetKeyEncrypt());
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + strEncryptCode + "\",");
                sbResult.Append("\"col_attr\":[{\"name\":\"" + "subparent" + "\", \"value\":\"" + AES.EncryptText(model.machamcong.ToString(), function.ReadXMLGetKeyEncrypt()) + "\"}],");
                sbResult.Append("\"col_value\":[");

                #region Data cell

                // Ngày chấm công của tháng từ 21/01-20/02

                for (int i = 21; i <= songaytrongthang; i++) //
                {
                    #region
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");

                    string kq = "";


                    if (i == 21) kq = model.ngay21.Trim();
                    else if (i == 22) kq = model.ngay22.Trim();
                    else if (i == 23) kq = model.ngay23.Trim();
                    else if (i == 24) kq = model.ngay24.Trim();
                    else if (i == 25) kq = model.ngay25.Trim();
                    else if (i == 26) kq = model.ngay26.Trim();
                    else if (i == 27) kq = model.ngay27.Trim();
                    else if (i == 28) kq = model.ngay28.Trim();
                    else if (i == 29) kq = model.ngay29.Trim();
                    else if (i == 30) kq = model.ngay30.Trim();
                    else if (i == 31) kq = model.ngay31.Trim();

                    if (model.ngaylamviectu.Trim().Length == 10)
                    {
                        int ngaybdlv = int.Parse(model.ngaylamviectu.Split('/')[0]);
                        int thangbdlv = int.Parse(model.ngaylamviectu.Split('/')[1]);
                        int thangktlv = int.Parse(model.ngaylamviecden.Split('/')[1]);
                        if (ngaybdlv > i && thangbdlv < thangktlv)
                            kq = "0";
                        else if (thangbdlv == thangktlv)
                            kq = "0";
                    }

                    if (kq == "F" || kq == "F/2" || kq == "Fo" || kq == "Fo/2" || kq == "TS" || kq == "TS/2" || kq == "Ro/2" || kq == "Ro")
                        sbResult.Append("\"col_class\":\"ovhnghi col" + i + "\",");
                    else if (kq == "X/2" || kq == "L")
                        sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");
                    else if (kq == "CN")
                        sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");
                    else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                    sbResult.Append("\"col_id\":\"" + i + "\",");

                    if (kq == "X/2" && phongban_congtruong == 1 && model.machamcong ==0)
                        kq = "X";


                    if (i == 21) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 22) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 23) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 24) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 25) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 26) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 27) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 28) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 29) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 30) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 31) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    sbResult.Append("},");

                    #endregion
                }

                for (int i = 1; i <= 20; i++)
                {
                    #region
                    sbResult.Append("{");
                    sbResult.Append("\"colspan\":\"1\",");

                    string kq = "";
                    if (i == 1) kq = model.ngay1.Trim();
                    else if (i == 2) kq = model.ngay2.Trim();
                    else if (i == 3) kq = model.ngay3.Trim();
                    else if (i == 4) kq = model.ngay4.Trim();
                    else if (i == 5) kq = model.ngay5.Trim();
                    else if (i == 6) kq = model.ngay6.Trim();
                    else if (i == 7) kq = model.ngay7.Trim();
                    else if (i == 8) kq = model.ngay8.Trim();
                    else if (i == 9) kq = model.ngay9.Trim();
                    else if (i == 10) kq = model.ngay10.Trim();
                    else if (i == 11) kq = model.ngay11.Trim();
                    else if (i == 12) kq = model.ngay12.Trim();
                    else if (i == 13) kq = model.ngay13.Trim();
                    else if (i == 14) kq = model.ngay14.Trim();
                    else if (i == 15) kq = model.ngay15.Trim();
                    else if (i == 16) kq = model.ngay16.Trim();
                    else if (i == 17) kq = model.ngay17.Trim();
                    else if (i == 18) kq = model.ngay18.Trim();
                    else if (i == 19) kq = model.ngay19.Trim();
                    else if (i == 20) kq = model.ngay20.Trim();

                    if (model.ngaylamviecden.Trim().Length == 10)
                    {
                        int ngaybdlv = int.Parse(model.ngaylamviectu.Split('/')[0]);
                        int thangbdlv = int.Parse(model.ngaylamviectu.Split('/')[1]);

                        int ngayktlv = int.Parse(model.ngaylamviecden.Split('/')[0]);
                        int thangktlv = int.Parse(model.ngaylamviecden.Split('/')[1]);

                        if (ngaybdlv > i && thangbdlv == thangktlv) kq = "0";
                        else if (ngayktlv < i && thangbdlv == thangktlv) kq = "0";
                        else if (ngayktlv < 21 && ngayktlv < i) kq = "0";
                    }

                    if (kq == "F" || kq == "F/2" || kq == "Fo" || kq == "Fo/2" || kq == "TS" || kq == "TS/2" || kq == "Ro/2" || kq == "Ro")
                        sbResult.Append("\"col_class\":\"ovhnghi col" + i + "\",");
                    else if (kq == "X/2" || kq == "L")
                        sbResult.Append("\"col_class\":\"ovht7 col" + i + "\",");
                    else if (kq == "CN")
                        sbResult.Append("\"col_class\":\"ovhcn col" + i + "\",");
                    else sbResult.Append("\"col_class\":\"ovh col" + i + "\",");

                    sbResult.Append("\"col_id\":\"" + i + "\",");

                    if (kq == "X/2" && phongban_congtruong == 1 && model.machamcong ==0)
                        kq = "X";

                    if (i == 1) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 2) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 3) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 4) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 5) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 6) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 7) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 8) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 9) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 10) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 11) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 12) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 13) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 14) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 15) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 16) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 17) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 18) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 19) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    else if (i == 20) sbResult.Append("\"col_value\":\"" + "<input id='" + i + "' style='width:100%; height:25px; text-align:center;' value=" + kq + " ></input>" + "\"");
                    if (i < 20)
                        sbResult.Append("},");
                    else sbResult.Append("}");

                    #endregion
                }

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
        public JsonResult SelectRows_themmoi_searchmnv(TimekeepingModels model, string manhanvien, string email)
        {
            TimekeepingModels param = new TimekeepingModels();
            TimekeepingServices service = new TimekeepingServices();

            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            lstResult = service.SelectRows_hieuchinh_manhanvien_email(manhanvien, email);
            string hovaten = "";
            string manv = manhanvien;
            string email1 = email;
            string ngaysinh = "";
            string maphongban = "0";

            if (lstResult.Count > 0)
            {
                hovaten = lstResult[0].hovaten;
                manv = lstResult[0].manhanvien;
                email1 = lstResult[0].email;
                ngaysinh = lstResult[0].ngaysinh.Trim().Split(' ')[0];
                maphongban = lstResult[0].maphongban;
            }
            phongban_congtruong = lstResult_phongban.Where(p => p.maphongban.Trim() == maphongban.ToString()).ToList()[0].phongban_congtruong;


            //lstResult_phongban
            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            sbResult.Append("{");
            sbResult.Append("\"email\":\"" + email1 + "\",");
            sbResult.Append("\"hovaten\":\"" + "" + hovaten + "" + "\",");
            sbResult.Append("\"manhanvien\":\"" + "" + manv + "" + "\",");
            sbResult.Append("\"ngaysinh\":\"" + ngaysinh + "\",");
            sbResult.Append("\"maphongban\":\"" + maphongban + "\",");
            sbResult.Append("\"data\":[" + sbRows.ToString() + "]");
            sbResult.Append("}");

            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SelectRows_themmoi_laymaphongban(string DataJson)
        {
            //lstResult_phongban
            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            phongban_congtruong = 0;
            var phongbancongtruong = lstResult_phongban.Where(p => p.maphongban == DataJson).ToList();
            if (phongbancongtruong.Count > 0)
                phongban_congtruong = phongbancongtruong[0].phongban_congtruong;
            return Json(sbResult.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(string DataJson)
        {
            JsonResult Data = new JsonResult();
            TimekeepingServices servicevpp = new TimekeepingServices();
            string nguoitao = Session["userid"].ToString();
            string iresult = servicevpp.Save_chamcong_New(DataJson, nguoitao, idmachamcong);
            idmachamcong = iresult;
            if (iresult != "-1")
            {
                //string strEncryptCode = idmadangky + "0070pXQSeNsQRuzoCmUYfuX/vA==6";
                //JObject json = JObject.Parse(DataJson);
                //string mailcanhan = json["data1"]["email"].ToString();
                //DanhmucServices service_pb = new DanhmucServices();
                //PhongBanModels parampb = new PhongBanModels();
                //parampb.maphongban = json["data1"]["maphongban"].ToString().Trim();
                //List<PhongBanModels> lstResult_phongban = service_pb.SelectRows(parampb);
                //if (lstResult_phongban.Count > 0)
                //{
                //    string mailto = lstResult_phongban[0].email.ToString().Trim();
                //    string hotento = lstResult_phongban[0].hovaten.ToString().Trim();
                //    JArray json_vpp_chitiet = (JArray)json["data2"];
                //    send_Mail(strEncryptCode, mailto, hotento, "Đăng ký văn phòng phẩm", json_vpp_chitiet, mailcanhan);
                //}
                return Json(new { success = true, machamcong = int.Parse(iresult.Trim()) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, machamcong=0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Save_index(string DataJson)
        {
            JsonResult Data = new JsonResult();
            TimekeepingServices servicevpp = new TimekeepingServices();
            string nguoitao = Session["userid"].ToString();
            string iresult = servicevpp.Save_chamcong(DataJson, nguoitao);
            idmachamcong = iresult;
            if (iresult != "-1")
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Xoadulieu(string DataJson)
        {
            JsonResult Data = new JsonResult();
            TimekeepingServices servicevpp = new TimekeepingServices();
            string nguoitao = Session["userid"].ToString();
            string iresult = "";
            if (DataJson.Trim() != "0")
                iresult = servicevpp.Deleted_chamcongthang(DataJson, nguoitao);
            if (iresult != "-1")
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportLicensing()
        {
            //check login
            if (!IsLogged())
                return BackToLogin();
            string id = this.Request.Url.Segments[3];
            string avvvaa = "xslt/Fileinhangthang/";
            string filepacth = Server.MapPath(avvvaa).Replace("Timekeeping\\ExportLicensing\\","");

            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            TimekeepingModels param = new TimekeepingModels();
            TimekeepingServices service = new TimekeepingServices();
            param.maphongban = id;

            int loginid = 0;
            if (Session["userid"].ToString().Trim() != "1" && Session["grouptk"].ToString().Trim() != "1" && Session["grouptk"].ToString().Trim() != "2")
            {
                loginid = int.Parse(Session["userid"].ToString());
            }

            param.nguoitao = loginid;

            lstResult = service.SelectRows_xuatexcel(param, thangnam);

            DataTable dt = new DataTable();
            dt.TableName = "Employee";
            dt.Columns.Add("Stt");
            dt.Columns.Add("Mã NV");
            dt.Columns.Add("Họ và tên");
            dt.Columns.Add("Mã phòng ban");

            for (int i = 21; i <= songaytrongthang; i++)
                dt.Columns.Add(i.ToString());

            for (int j = 1; j < 21; j++)
                dt.Columns.Add(j.ToString());

            dt.Columns.Add("Ngày công lễ");
            dt.Columns.Add("Nghỉ phép");
            dt.Columns.Add("Nghỉ hiếu hỉ");
            dt.Columns.Add("Nghỉ ốm thai sản");
            dt.Columns.Add("Nghỉ không lương");
            dt.Columns.Add("Số ngày trả lương");
            dt.Columns.Add("Số ngày phụ cấp tiền cơm");
            dt.Columns.Add("Số ngày công chuẩn");
            dt.Columns.Add("Chênh lệch ngày công");
            dt.Columns.Add("Tiêu chí");
            dt.Columns.Add("Hoàn thành công việc");
            dt.Columns.Add("Tinh thần trách nhiệm");
            dt.Columns.Add("Cư xử hợp tác");
            dt.Columns.Add("Tuân thủ giờ giấc");
            dt.Columns.Add("Xếp loại");
            dt.Columns.Add("Ghi chú");

            foreach (var item_phongban in lstResult_phongban)
            {
                int i = 1;
                foreach (var item in lstResult.Where(p => p.maphongban.ToString().Trim() == item_phongban.maphongban))
                {
                    #region
                    if (i == 1)
                    {
                        DataRow rowpb = dt.NewRow();
                        dt.Rows.Add(rowpb);
                        rowpb["Stt"] = item_phongban.tenphongban;
                        dt.AcceptChanges();
                    }
                    item.stt = i.ToString();
                    i++;

                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);

                    row["Stt"] = item.stt;
                    row["Mã NV"] = item.manhanvien;
                    row["Họ và tên"] = item.hovaten;
                    row["Mã phòng ban"] = item.tenphongban;
                    row["21"] = item.ngay21.ToLower();
                    row["22"] = item.ngay22.ToLower();
                    row["23"] = item.ngay23.ToLower();
                    row["24"] = item.ngay24.ToLower();
                    row["25"] = item.ngay25.ToLower();
                    row["26"] = item.ngay26.ToLower();
                    row["27"] = item.ngay27.ToLower();
                    row["28"] = item.ngay28.ToLower();
                    if (songaytrongthang >= 29)
                        row["29"] = item.ngay29.ToLower();
                    if (songaytrongthang >= 30)
                        row["30"] = item.ngay30.ToLower();
                    if (songaytrongthang >= 31)
                        row["31"] = item.ngay31.ToLower();
                    row["1"] = item.ngay1.ToLower();
                    row["2"] = item.ngay2.ToLower();
                    row["3"] = item.ngay3.ToLower();
                    row["4"] = item.ngay4.ToLower();
                    row["5"] = item.ngay5.ToLower();
                    row["6"] = item.ngay6.ToLower();
                    row["7"] = item.ngay7.ToLower();
                    row["8"] = item.ngay8.ToLower();
                    row["9"] = item.ngay9.ToLower();
                    row["10"] = item.ngay10.ToLower();
                    row["11"] = item.ngay11.ToLower();
                    row["12"] = item.ngay12.ToLower();
                    row["13"] = item.ngay13.ToLower();
                    row["14"] = item.ngay14.ToLower();
                    row["15"] = item.ngay15.ToLower();
                    row["16"] = item.ngay16.ToLower();
                    row["17"] = item.ngay17.ToLower();
                    row["18"] = item.ngay18.ToLower();
                    row["19"] = item.ngay19.ToLower();
                    row["20"] = item.ngay20.ToLower();

                    row["Ngày công lễ"] = item.ngaycong_le;
                    row["Nghỉ phép"] = item.nghiphep;
                    row["Nghỉ hiếu hỉ"] = item.nghihieu_hi;
                    row["Nghỉ ốm thai sản"] = item.nghiom_thaisan;
                    row["Nghỉ không lương"] = item.nghikhongluong;
                    row["Số ngày trả lương"] = item.songayduoc_traluong;
                    row["Số ngày phụ cấp tiền cơm"] = item.songaypc_tiencom;
                    row["Số ngày công chuẩn"] = item.songay_congchuan;
                    row["Chênh lệch ngày công"] = item.chenhlech_ngaycong;
                    row["Tiêu chí"] = item.tieuchi;
                    row["Hoàn thành công việc"] = item.hoanthanh_congviec;
                    row["Tinh thần trách nhiệm"] = item.tinhthan_trachnhiem;
                    row["Cư xử hợp tác"] = item.cuxu_hoptac;
                    row["Tuân thủ giờ giấc"] = item.tuanthu_giogiac;
                    row["Xếp loại"] = item.xeploai;
                    row["Ghi chú"] = "";

                    dt.AcceptChanges();
                    #endregion
                }
            }
            int thangchamcong = int.Parse(DateTime.Now.Month.ToString("00"));
            int namchamcong = int.Parse(DateTime.Now.Year.ToString("0000"));
            try
            {
                thangchamcong = int.Parse(lstResult[0].thang.Split('/')[0].ToString().Trim());
                namchamcong = int.Parse(lstResult[0].thang.Split('/')[1].ToString().Trim());
            }
            catch (Exception) { }


            List<SetCellValue> list = new List<SetCellValue>();

            list.Add(new SetCellValue { RowIndex = 0, ColumnIndex = 0, Value = "21/" + (thangchamcong - 1).ToString("00") + "/" + namchamcong.ToString() });

            list.Add(new SetCellValue { RowIndex = 1, ColumnIndex = 5, Value = "BẢNG CHẤM CÔNG VÀ  ĐÁNH GIÁ THỰC HIỆN CÔNG VIỆC HÀNG THÁNG CỦA CBNV " + thangnam });
            string tenfile = "";
            if (songaytrongthang == 28) tenfile = "Ketquachamcong28.xls";
            else if (songaytrongthang == 29) tenfile = "Ketquachamcong29.xls";
            else if (songaytrongthang == 30) tenfile = "Ketquachamcong30.xls";
            else tenfile = "Ketquachamcong31.xls";

            string filedownload = DateTime.Now.ToString("ddMMyyyyHHmmss") + "bangchamcongthang.xls";
            string fileName = filepacth + filedownload;
            ExportToExcel export = new ExportToExcel(Functions.GetTemplateFileName(tenfile, filepacth));
            export.TemplateExportDenghicungcapvattuthietbi(dt, 7, 0, list, fileName);

            var FileVirtualPath = "~/xslt/Fileinhangthang/" + filedownload;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
            //FileDialogs.OpenFile(fileName);
            //return RedirectToAction("Index");
        }

        public JsonResult Guimailxacnhan(string DataJson)
        {
            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            TimekeepingModels param = new TimekeepingModels();
            TimekeepingServices service = new TimekeepingServices();

            JObject json = JObject.Parse(DataJson);
            param.maphongban = json["maphongban"].ToString().Trim();
            param.thang = json["thang"].ToString().Trim();

            int loginid = 0;
            if (Session["userid"].ToString().Trim() != "1" && Session["grouptk"].ToString().Trim() != "1" && Session["grouptk"].ToString().Trim() != "2")
            {
                loginid = int.Parse(Session["userid"].ToString());
            }
            param.nguoitao = loginid;
            lstResult = service.SelectRows_xuatexcel(param, thangnam);
            var layphongban = lstResult_phongban.Where(p => p.maphongban == param.maphongban.Trim()).ToList();
            if (layphongban.Count == 1)
            {
                string kq = send_Mail(layphongban[0].hovaten, layphongban[0].email, lstResult, param.maphongban, param.thang);
                if (kq == "1")
                {
                    //Da gui email=3;
                    string iresult = service.Update_trangthaiguiemail(param.maphongban, param.thang);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public string send_Mail(string hotentruongphong, string emailtruongphong, List<TimekeepingModels> model,string maphongban,string thang)
        {
            _logger.Start("send_Mail");
            string kq = "";
            try
            {
                string linkname = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("linknamechamcong"));
                //FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                //string mailcanhan = AES.EncryptText(Session["thudientu"].ToString().Trim(), function.ReadXMLGetKeyEncrypt());
                string mailcanhan = Session["thudientu"].ToString().Trim();
                string strEncryptCode = linkname.Trim() + maphongban.Trim() + "0070pXQSeNsQRuzoCmUYfuX" + "&mca=" + mailcanhan + "&thang=" + thang;

                string smtp_host = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_host"));
                string smtp_user = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_user"));

                #region

                StringBuilder sb = new StringBuilder();
                sb.Append("<html><head>");
                sb.Append("<link rel='stylesheet' type='text/css' href='theme.css' />");
                sb.Append("</head>");
                sb.Append("<body style='margin-top: 20px; padding: 0; width: 850px; font-size: 1em;color:black;'>"); //margin: 0 auto;  de canh giua

                sb.Append("<table cellpadding='0' cellspacing='0' width='850px' >");
                sb.Append("<tr>");

                sb.Append("<td>");
                sb.Append("<div style='width:150px;float:left;height :45px; line-height:45px; padding-top:10px;'>");
                sb.Append("<img src='http://i.imgur.com/yKqNNy2.png'  alt='ddd' style='width:100px; height:45px;'/>");
                sb.Append("</div>");
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append("<div style='width:400px; text-align:center;font-weight: bold;float:left;line-height:45px'>");
                sb.Append("<p style= 'width:400px;text-align:center;font-size:18px;font-weight:bold;line-height:45px;padding-left:80px;float:left;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BẢNG CHẤM CÔNG ONLINE</p>");
                sb.Append("</div>");
                sb.Append("</td>");

                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</body>");


                sb.Append("<hr style=border: 1px solid #000; width: 100% />");

                sb.Append("<table style='width:850px; font-size:14px;'>");
                sb.Append("<tr><td style='padding-left:10px;colspan=5'><p><strong><em><u>Kính gửi Anh/Chị:</u></em>&nbsp;" + hotentruongphong + "</strong></p></td></tr>");
                sb.Append("<tr><td style='padding-left:10px;colspan=5;padding-bottom:10px;'><p><strong>Anh/chị Duyệt danh sách chấm công nhân viên làm việc trong tháng: " + thang + "</strong></p></td></tr>");
                sb.Append("</table>");


                sb.Append("<table style='width:850px; font-size:14px;'>");
                sb.Append("<tr style='float:left;height :27px; line-height:27px; padding-left:10px;border: 1px solid #ddd;'>");
                sb.Append("<td style='width:40px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>STT</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>MSNV</td>");
                sb.Append("<td style='width:300px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Họ và tên</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Ngày công+lễ</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Nghỉ phép</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Nghỉ hiếu hỉ</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Nghỉ ốm-TS</td>");

                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Nghỉ không lương</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Số ngày trả lương</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>PC tiền cơm</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Công chuẩn</td>");
                sb.Append("<td style='width:80px; text-align:center; font-weight:bold; font-size:14px;border: 1px solid #ddd;'>Chênh lệch ngày công</td>");
                sb.Append("</tr>");

                for (int i = 0; i < model.Count(); i++)
                {
                    sb.Append("<tr style='float:left;height :22px; line-height:24px; padding-left:10px;border: 1px solid #ddd;'>");
                    sb.Append("<td style='width:40px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + (i + 1).ToString() + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].manhanvien.ToString() + "</td>");
                    sb.Append("<td style='width:300px; font-size:14px; line-height:24px; text-align:left;   border: 1px solid #ddd;'>" + model[i].hovaten.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].ngaycong_le.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].nghiphep.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].nghihieu_hi.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].nghiom_thaisan.ToString() + "" + "</td>");

                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].nghikhongluong.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].songayduoc_traluong.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].songaypc_tiencom.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].songay_congchuan.ToString() + "" + "</td>");
                    sb.Append("<td style='width:80px;  font-size:14px; line-height:24px; text-align:center; border: 1px solid #ddd;'>" + model[i].chenhlech_ngaycong.ToString() + "" + "</td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;border: 1px solid #ddd;'>" + DataJson[i]["hovaten"].ToString() + ":" + DataJson[i]["tongsoluong"].ToString() + "" + "</td></tr>");
                }

                //background-color:0090d9;

                sb.Append("</table>");
                sb.Append("<table style='width:850px;'>");
                sb.Append("<tr><td style='float:left; padding-left:10px; font-size:20px; height :30px; line-height:31px; padding-top:10px;'><a href='" + strEncryptCode + "&dy=1'> Duyệt bảng chấm công</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href='" + strEncryptCode + "&dy=2'>Không duyệt bảng chấm công</a></td></tr>");
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");


                #endregion
                
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(smtp_user, emailtruongphong.Trim());
                message.From = new MailAddress(smtp_user.Trim(), "Chấm công online", System.Text.Encoding.UTF8);
                message.Subject = "Chấm công online";
                message.Body = sb.ToString();
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient(smtp_host);
                client.UseDefaultCredentials = true;
                try
                {
                    client.Send(message);
                    kq = "1";
                }
                catch (Exception)
                {
                    kq= "-1";
                }
                
            }
            catch (Exception ms)
            {
                _logger.Error(ms);
                kq="-1";
            }
            _logger.End("send_Mail");
            return kq;
        }
    }
}