
<%@ WebHandler Language="C#" Class="UpdateEventHandle" %>

using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using COMMONLIB;
using System.Text;

public class UpdateEventHandle : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    DT.COMMONLIB.DTLogger logger = new DT.COMMONLIB.DTLogger("UpdateEventHandle--");
    public void ProcessRequest(HttpContext context)
    {
        string strResult = "";
        if (context.Session["userid"] != null)
        {
            WEB_SOAP.calendar.SuKien010000Service services = new WEB_SOAP.calendar.SuKien010000Service();
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(context.Request.InputStream);
                string line = "";
                line = sr.ReadToEnd();
                Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Parse(line);

                string strVirginState = (string)jo["strTrangThai"];
                CustomDataSet.clSuKien clParam = new CustomDataSet.clSuKien()
                {
                    trangthai = Constants.CST_EVENT_ACCEPTED_FLAG,
                    maghichu = (string)jo["strid"],
                    tieude = (string)jo["strTieuDe"],
                    noidung = (string)jo["strNoiDung"],
                    vitrikhac = (string)jo["strPhongHop"],
                    dexuat = (string)jo["strDeXuat"],
                    ngaybatdau = (string)jo["strNgayBatDau"],
                    ngayketthuc = (string)jo["strNgayKetThuc"],
                    loainhacnho = (string)jo["flgNhacNho"],
                    songaynhacnho = (string)jo["strSoNgayNhacNho"],
                    mucdouutien = (string)jo["strDoUuTien"],
                    nguoihieuchinh = context.Session["userid"].ToString()
                };
                
                if (clParam.tieude == "")
                {
                    clParam.tieude = "Không có tiêu đề";
                }
                if (context.Request.QueryString["val"] == "accept")
                    strResult = services.updateSuKienRegist(clParam);
                else
                {
                    if (strVirginState == "0")
                    {
                        bool bCompare = true;
                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                        DateTime dateStart, dateEnd, dateStartCompare, dateEndCompare;
                        DateTime.TryParseExact(clParam.ngaybatdau, "yyyy-MM-dd HH:mm:ss", culture,
                            System.Globalization.DateTimeStyles.None, out dateStart);
                        DateTime.TryParseExact(clParam.ngayketthuc, "yyyy-MM-dd HH:mm:ss", culture,
                            System.Globalization.DateTimeStyles.None, out dateEnd);
                        List<CustomDataSet.clSuKien> lstCompare = services.getListCompare();
                        for (int i = 0; i < lstCompare.Count; i++)
                        {
                            DateTime.TryParseExact(lstCompare[i].ngaybatdau, "yyyy-MM-dd HH:mm:ss",
                                culture, System.Globalization.DateTimeStyles.None, out dateStartCompare);
                            DateTime.TryParseExact(lstCompare[i].ngayketthuc, "yyyy-MM-dd HH:mm:ss",
                                culture, System.Globalization.DateTimeStyles.None, out dateEndCompare);
                            bCompare = TimeBetween(dateStart, dateEnd, dateStartCompare, dateEndCompare);

                            if (bCompare == false && clParam.vitrikhac == lstCompare[i].vitrikhac && clParam.vitrikhac != "")
                                break;

                            bCompare = true;
                        }

                        if (bCompare == true)
                            strResult = services.updateEvent(clParam);
                        else
                            strResult = "0";
                    }
                    else if (strVirginState == "")
                        strResult = services.updateEvent(clParam);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        else 
        {
            context.Response.Redirect("index.aspx");
        }
        context.Response.ContentType = "application/json";
        context.Response.Write(strResult);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private bool TimeBetween(DateTime dtStart, DateTime dtEnd, DateTime start, DateTime end)
    {
        // see if start comes before end
        if ((start < dtStart && dtStart < end) || (start < dtEnd && dtEnd < end)
            || (dtStart < start && start < dtEnd) || (dtStart < end && end < dtEnd))
            return false;
        // start is after end, so do the inverse comparison
        return true;
    }
}