<%@ WebHandler Language="C#" Class="datafeed" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Net;
using NHG.Core.Functions;
using NHG.Web.Data.Services;
using NHG.Web.Models;

public class datafeed : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    
    public void ProcessRequest (HttpContext context) {
        //kiem tra neu user chua co dang nhap
        string strFormatDate = "dd/MM/yyyy HH:mm";
        if (context.Session["userid"] != null)
        {
            StringBuilder sbResult = new StringBuilder();
            if (context.Request.QueryString["method"] != null)
            {
                string strMethod = context.Request.QueryString["method"];
                System.IO.StreamReader sr = new System.IO.StreamReader(context.Request.InputStream);
               
                int iID = 0;
                LichLamViecServices services = new LichLamViecServices();
                LichLamViecModels clParam = new LichLamViecModels();
                List<LichLamViecModels> lstSuKien = new List<LichLamViecModels>();
                switch (strMethod)
                {
                    case "add":
                        //$ret = addCalendar($_POST["CalendarStartTime"], $_POST["CalendarEndTime"], $_POST["CalendarTitle"], $_POST["IsAllDayEvent"]);
                        //check login
                        string strMauLich = "0";
                        string strNgayBatDau = context.Request.Form["CalendarStartTime"];
                        string strNgayKetThuc = context.Request.Form["CalendarEndTime"];
                        string strIsAllDayEvent = context.Request.Form["IsAllDayEvent"];

                        string dtngaybatdau = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayBatDau, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
                        string dtngayketthuc = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayKetThuc, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");

                        if (strIsAllDayEvent == "1")
                        {
                            dtngaybatdau = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayBatDau, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
                            dtngayketthuc = FunctionsDateTime.ConvertToSQLTimeStamp(FunctionsDateTime.ConvertDateWithFormat(strNgayBatDau, Functiontring.ReturnStringFormatID("NgayThangNam")), "00");
                        }
                        LichLamViecModels clParamSuKien = new LichLamViecModels()
                        {
                            noidung = context.Request.Form["CalendarTitle"],
                            diadiem = "",
                            maulich = strMauLich,
                            maphongban = "NHG.PB.000008",
                            ngaybatdau = dtngaybatdau,
                            ngayketthuc = dtngayketthuc,
                            nguoitao = context.Session["userid"].ToString(),
                            nguoihieuchinh = context.Session["userid"].ToString()
                        };
                        services = new LichLamViecServices();
                        sbResult.Append(services.InsertRow(clParamSuKien));
                        break;
                    case "list":   
                         string strShowDate = context.Request.Form.GetValues("showdate")[0].ToString();
                        string strViewType = context.Request.Form.GetValues("viewtype")[0].ToString();
                        string strTimeZone = context.Request.Form.GetValues("timezone")[0].ToString();
                        services = new LichLamViecServices();
                        clParam = new LichLamViecModels();
                        switch (strViewType)
                        {
                            case "day":
                                string[] splitDate = strShowDate.Split('/');
                                int iNgay = int.Parse(splitDate[0]);
                                int iThang = int.Parse(splitDate[1]);
                                int iNam = int.Parse(splitDate[2]);
                                clParam.ngaybatdau = string.Format("{0}/{1}/{2}", iNam, iThang, iNgay);
                                clParam.ngayketthuc = string.Format("{0}/{1}/{2}", iNam, iThang, iNgay);
                                break;
                            case "week":
                                splitDate = strShowDate.Split('/');
                                int star = 1;
                                int end = 31;
                                iNgay = int.Parse(splitDate[0]);
                                iThang = int.Parse(splitDate[1]);
                                iNam = int.Parse(splitDate[2]);
                                DateTime dateSearch = FunctionsDateTime.ConvertStringToDate(string.Format("{0}/{1}/{2}", iNgay.ToString("00"), iThang.ToString("00"), iNam));
                                FunctionsDateTime.DayStartEndWeek(dateSearch, out star, out end);
                                //string strDateInWeek = string.Format("{0}/{1}/{2}", splitDate[0], splitDate[1], splitDate[2]);
                                clParam.ngaybatdau = string.Format("{0}/{1}/{2}", iNam, iThang, star);
                                clParam.ngayketthuc = string.Format("{0}/{1}/{2}", iNam, iThang, end);
                                break;
                            case "month":
                                splitDate = strShowDate.Split('/');
                                iNgay = int.Parse(splitDate[0]);
                                iThang = int.Parse(splitDate[1]);
                                iNam = int.Parse(splitDate[2]);
                                clParam.ngaybatdau = string.Format("{0}/{1}/1", iNam, iThang);
                                clParam.ngayketthuc = string.Format("{0}/{1}/31", iNam, iThang);
                                break;
                        }
                        lstSuKien = services.selectEventForIdEvent(clParam).ToList();
                        StringBuilder sbEvents = new StringBuilder();
                        foreach (var item in lstSuKien)
                        {
                            /*[0] malich, 
                            *[1] tieu de, 
                            *[2] ngay bat dau, 
                            *[3] ngay ket thuc, 
                            *[4] hien thi gio cua chu thich trong su kien ca ngay, 
                            *[5] hien thi gio tren la bel, 
                            *[6] , 
                            *[7] mau lich, 
                            *[8] clich de xem thong tin ngan tren: 1 - lich xem; 0 - lich khong xem, 
                            *[9] dia diem, 
                            *[10] */
                            sbEvents.Append("[");
                            sbEvents.Append("\"" + AES.Encrypt(item.malichlamviec, Convert.FromBase64String(context.Session.SessionID)) + "\","); 
                            sbEvents.Append("\"" + item.noidung+ "\",");
                            sbEvents.Append("\""+item.ngaybatdau+"\",");
                            sbEvents.Append("\""+item.ngayketthuc+"\",");
                            sbEvents.AppendFormat("{0},", item.sukiencangay);
                            sbEvents.Append("0,");
                            sbEvents.Append("0,");
                            sbEvents.AppendFormat("{0},", item.maulich);
                            sbEvents.Append("1,");
                            sbEvents.Append("\"" + item.diadiem + "\",");
                            sbEvents.Append("\"\"");
                            sbEvents.Append("],");
                        }
                        if (lstSuKien.Count > 0)
                            sbEvents.Remove(sbEvents.Length - 1, 1);                        
                                             //[44727,'remote meeting','04/11/2014 21:10','01/01/1970 02:40',0,1,0,-1,1,'Bytelin',''],
                                             //[61317,'remote meeting','04/11/2014 18:28','01/01/1970 02:32',0,0,0,8,1,'Bytelin',''],
                                             //[51890,'remote meeting','04/11/2014 23:26','01/01/1970 02:57',0,0,0,13,1,'Bytelin',''],
                                             //[83948,'remote meeting','04\/12\/2014 18:06','01\/01\/1970 03:25',0,0,0,8,1,'Bytelin',''],
                                             //[44991,'remote meeting','04\/07\/2014 08:27','01\/01\/1970 03:26',1,0,0,7,1,'Newswer',''],
                                             //[44991,'remote meeting','04\/07\/2014 08:27','01\/01\/1970 03:26',1,0,0,7,1,'Newswer','']";
                        sbResult.Append("{");
                        sbResult.Append("\"events\":[" + sbEvents + "],");
                        sbResult.Append("\"issort\":true,");
                        sbResult.Append("\"error\":null");
                        sbResult.Append("}");                        
                        break;
                    case "update":
                        //$ret = updateCalendar($_POST["calendarId"], $_POST["CalendarStartTime"], $_POST["CalendarEndTime"]);
                        services = new LichLamViecServices();
                        int.TryParse(context.Request.Form.GetValues("calendarId")[0].ToString(), out iID);
                        clParam = new LichLamViecModels();
                        clParam.malichlamviec = iID.ToString();
                        DateTime dateCalendarStartTime = FunctionsDateTime.ConvertDateWithFormat(context.Request.Form["CalendarStartTime"], "dd/MM/yyyy HH:mm");
                        DateTime dateCalendarEndTime = FunctionsDateTime.ConvertDateWithFormat(context.Request.Form["CalendarEndTime"], "dd/MM/yyyy HH:mm");
                        clParam.ngaybatdau = string.Format("{0}/{1}/{2} {3}:{4}",
                                                            dateCalendarStartTime.Year,
                                                            dateCalendarStartTime.Month.ToString("00"),
                                                            dateCalendarStartTime.Day.ToString("00"),
                                                            dateCalendarStartTime.Hour.ToString("00"),
                                                            dateCalendarStartTime.Minute.ToString("00"));
                        clParam.ngayketthuc = string.Format("{0}/{1}/{2} {3}:{4}",
                                                            dateCalendarEndTime.Year,
                                                            dateCalendarEndTime.Month.ToString("00"),
                                                            dateCalendarEndTime.Day.ToString("00"),
                                                            dateCalendarEndTime.Hour.ToString("00"),
                                                            dateCalendarEndTime.Minute.ToString("00"));
                        bool strResult = services.UpdateRow(clParam);
                        if (strResult)
                        {
                            sbResult.Append("{");
                            sbResult.Append("\"IsSuccess\":true,");
                            sbResult.Append("\"error\":null");
                            sbResult.Append("}");
                        }
                        else
                        {
                            sbResult.Append("{");
                            sbResult.Append("\"error\":null");
                            sbResult.Append("}");
                        }
                        break;
                    case "remove":
                        services = new LichLamViecServices();
                        int.TryParse(context.Request.Form.GetValues("calendarId")[0].ToString(), out iID);
                        List<LichLamViecModels> lstRow = new List<LichLamViecModels>();
                        lstRow.Add(new LichLamViecModels() { malichlamviec = iID.ToString() });
                        if (services.DeleteRows(lstRow))
                        {
                            sbResult.Append("{");
                            sbResult.Append("\"IsSuccess\":true,");
                            sbResult.Append("\"error\":null");
                            sbResult.Append("}");
                        }
                        else
                        {
                            sbResult.Append("{");
                            sbResult.Append("\"error\":null");
                            sbResult.Append("}");
                        }
                        break;
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(System.Text.RegularExpressions.Regex.Replace(sbResult.ToString(), @"\t|\n|\r", ""));
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private ArrayList listCalendar(string day, string type)
    {
        //$phpTime = js2PhpTime(day);
        //echo $phpTime . "+" . $type;
        switch (type)
        {
            case "month":
                //$st = mktime(0, 0, 0, date("m", $phpTime), 1, date("Y", $phpTime));
                //$et = mktime(0, 0, -1, date("m", $phpTime)+1, 1, date("Y", $phpTime));
                //$cnt = 50;
                break;
            case "week":
                //suppose first day of a week is monday 
                //$monday  =  date("d", $phpTime) - date('N', $phpTime) + 1;
                ////echo date('N', $phpTime);
                //$st = mktime(0,0,0,date("m", $phpTime), $monday, date("Y", $phpTime));
                //$et = mktime(0,0,-1,date("m", $phpTime), $monday+7, date("Y", $phpTime));
                //$cnt = 20;
                break;
            case "day":
                //$st = mktime(0, 0, 0, date("m", $phpTime), date("d", $phpTime), date("Y", $phpTime));
                //$et = mktime(0, 0, -1, date("m", $phpTime), date("d", $phpTime)+1, date("Y", $phpTime));
                //$cnt = 5;
                break;
        }
        //echo $st . "--" . $et;
        return listCalendarByRange("", "", "");
    }
    
    private ArrayList listCalendarByRange(string sd, string ed, string cnt){
      ArrayList ret = new ArrayList();
      //$ret['events'] = array();
      //$ret["issort"] =true;
      //$ret["start"] = php2JsTime($sd);
      //$ret["end"] = php2JsTime($ed);
      //$ret['error'] = null;
      //$title = array('team meeting', 'remote meeting', 'project plan review', 'annual report', 'go to dinner');
      //$location = array('Lodan', 'Newswer', 'Belion', 'Moore', 'Bytelin');
      //for($i=0; $i<$cnt; $i++) {
      //    $rsd = rand($sd, $ed);
      //    $red = rand(3600, 10800);
      //    if(rand(0,10) > 8){
      //        $alld = 1;
      //    }else{
      //        $alld=0;
      //    }
      //    $ret['events'][] = array(
      //      rand(10000, 99999),
      //      $title[rand(0,4)],
      //      php2JsTime($rsd),
      //      php2JsTime($red),
      //      rand(0,1),
      //      $alld, //more than one day event
      //      0,//Recurring event
      //      rand(-1,13),
      //      1, //editable
      //      $location[rand(0,4)], 
      //      ''//$attends
      //    );
      //}
      return ret;
    }

}