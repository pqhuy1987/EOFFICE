<%@ WebHandler Language="C#" Class="InsertEventHandle" %>
using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using COMMONLIB;
using System.Text;

public class InsertEventHandle : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    DT.COMMONLIB.DTLogger logger = new DT.COMMONLIB.DTLogger("InsertEventHandle--");
    public void ProcessRequest (HttpContext context) {
        string strResult = "-1";
        if (context.Session["userid"] != null)
        {
            WEB_SOAP.calendar.SuKien010000Service services = new WEB_SOAP.calendar.SuKien010000Service();
            System.IO.StreamReader sr = new System.IO.StreamReader(context.Request.InputStream);
            string line = "";
            line = sr.ReadToEnd();
            Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Parse(line);
            CustomDataSet.clSuKien clParam = new CustomDataSet.clSuKien()
            {
                tieude = (string)jo["strTieuDe"],
                noidung = (string)jo["strNoiDung"],
                vitrikhac = (string)jo["strPhongHop"],
                dexuat = (string)jo["strDeXuat"],
                ngaybatdau = (string)jo["strNgayBatDau"],
                ngayketthuc = (string)jo["strNgayKetThuc"],
                loainhacnho = (string)jo["flagNhacNho"],
                songaynhacnho = (string)jo["strNhacNho"],
                mucdouutien = (string)jo["strDoUuTien"],
                nguoitaoghichucongviec = context.Session["userid"].ToString()
            };
            strResult = "";
            string strtemp = clParam.ngaybatdau.Split(' ')[0].Split('-')[2] + "-" + clParam.ngaybatdau.Split(' ')[0].Split('-')[1] + "-" + clParam.ngaybatdau.Split(' ')[0].Split('-')[0];
            
            try
            {
                if (clParam.loainhacnho == "1" && clParam.songaynhacnho != "")
                {
                    //Khoang cach: datetime - int
                    DateTime dtime = FunctionsDateTime.ConvertStringToDate(strtemp);
                    int temp = int.Parse(clParam.songaynhacnho);
                    DateTime dtNhacNho = dtime.AddDays(-temp);
                    if (dtNhacNho.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dtNhacNho = dtNhacNho.AddDays(-2);
                    }
                    else if (dtNhacNho.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dtNhacNho = dtNhacNho.AddDays(-1);
                    }
                    clParam.thoigiannhacnho = dtNhacNho.ToString("yyyy-MM-dd");
                }
                else
                {
                    clParam.songaynhacnho = "0";
                    clParam.thoigiannhacnho = clParam.ngaybatdau;
                }

                if (context.Request.QueryString["p"] != "1")
                {
                    strResult = services.insertEvent(clParam);
                }
                else if (context.Request.QueryString["p"] == "1")
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
                    {
                        strResult = services.insertEvent(clParam);
                        if (!context.Session["machucnang"].ToString().Split(',').Contains("32"))
                            services.insertEventWaitAccept(context.Session["userid"].ToString(), strResult, Constants.CST_EVENT_WAITING_FLAG);
                        else
                            services.insertEventWaitAccept(context.Session["userid"].ToString(), strResult, Constants.CST_EVENT_ACCEPTED_FLAG);
                    }
                    else
                    {
                        strResult = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        else
        {
            context.Response.Redirect("index.aspx");
            return;
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

    bool TimeBetween(DateTime dtStart, DateTime dtEnd, DateTime start, DateTime end)
    {
        // see if start comes before end
        if ((start < dtStart && dtStart < end) || (start < dtEnd && dtEnd < end)
            || (dtStart < start && start < dtEnd) || (dtStart < end && end < dtEnd))
            return false;
        // start is after end, so do the inverse comparison
        return true;
    }

}