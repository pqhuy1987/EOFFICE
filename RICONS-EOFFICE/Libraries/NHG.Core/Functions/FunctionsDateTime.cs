using NHG.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHG.Core.Functions
{
    public class FunctionsDateTime
    {
        private static Log4Net logger = new Log4Net(typeof(FunctionsDateTime));

        #region | output format is string |
        /// <summary>
        /// Chuyen sang dinh dang dd_MM_yyyy
        /// hoac sang dinh dang dd_MM_yyyy_hh_mm_ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateToUnderScoreBelow(DateTime date, bool isDay, bool isDateTime)
        {
            try
            {
                if (isDay)
                    return string.Format("{0}_{1}_{2}", 
                           DateTime.Now.Day.ToString("00"), 
                           DateTime.Now.Month.ToString("00"), 
                           DateTime.Now.Year.ToString("00"));
                if(isDateTime)
                    return string.Format("{0}_{1}_{2}_{3}_{4}_{5}",
                           DateTime.Now.Day.ToString("00"),
                           DateTime.Now.Month.ToString("00"),
                           DateTime.Now.Year.ToString("00"),
                           DateTime.Now.Hour.ToString("00"),
                           DateTime.Now.Minute.ToString("00"),
                           DateTime.Now.Second.ToString("00"));
                else
                    return string.Format("{0}_{1}_{2}",
                           DateTime.Now.Day.ToString("00"),
                           DateTime.Now.Month.ToString("00"),
                           DateTime.Now.Year.ToString("00"));
            }
            catch (Exception)
            {
            }
            return "";
            //return date.ToString("dd/MMM/yyyy").ToUpper();
        }
        /// <summary>
        /// Chuyen sang dinh dang yyyy/MM/dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDate(DateTime date)
        {
            try
            {
                return date.Year.ToString("0000") + "/" + date.Month.ToString("00") + "/" + date.Day.ToString("00");
            }
            catch (Exception)
            {
            }
            return "";
            //return date.ToString("dd/MMM/yyyy").ToUpper();
        }
        /// <summary>
        /// input : dd/MM/yyyy output : yyyy/MM/dd
        /// input yyyy/MM/dd output dd/MM/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDate(string date)
        {
            string strtmp = "";
            try
            {
                if (date.Length == 10)
                {
                    string[] tmp = date.Split(new string[] { "/", "-" }, StringSplitOptions.None);
                    strtmp = tmp[2] + "/" + tmp[1] + "/" + tmp[0];
                }
                else if (date.Length == 7)
                {
                    string[] tmp = date.Split(new string[] { "/", "-" }, StringSplitOptions.None);
                    strtmp = tmp[1] + "/" + tmp[0];
                }
                else if (date.Length == 4)
                    strtmp = date;
            }
            catch (Exception)
            {
            }
            return strtmp;
        }
        /// <summary>
        /// Chuyen sang dinh dang yyyy/MM/dd hh:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <param name="milisecond"></param>
        /// <returns></returns>
        public static string ConvertToSQLTimeStamp(DateTime date, string milisecond)
        {
            return date.Year.ToString("0000") + "/" + date.Month.ToString("00") + "/" +
                date.Day.ToString("00") + " " + date.Hour.ToString("00") + ":" +
                date.Minute.ToString("00");
        }

        /// <summary>
        /// Chuyen kieu DateTime sang dinh dang dd/MM/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateClient(DateTime date)
        {
            return date.Day.ToString("00") + "/" + date.Month.ToString("00") + "/" + date.Year.ToString("0000");
        }

        /// <summary>
        /// Chuyen kieu DateTime sang dinh dang dd/MM/yyyy hh:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateClientWithTimeStamp(DateTime date)
        {
            return date.Day.ToString("00") + "/" + date.Month.ToString("00") + "/" + date.Year.ToString("0000") + " " + date.Hour.ToString("00") + ":" +
                date.Minute.ToString("00") + ":" + date.Second.ToString("00");
        }

        public static string ConvertStringToDDMMYY(string strDate)
        {

            string[] tmp = strDate.Split(new string[] { "/", "-" }, StringSplitOptions.None);
            return tmp[2] + "/" + "/" + tmp[1] + "/" + tmp[0];
        }
        #endregion

        #region StringToDate
        /// <summary>
        ///<param name="source">yyyy/mm/dd hh:mm:ss or yyyy/mm/dd</param>
        /// </summary>
        public static String ConvertDateSeperator(String source, String strseperator)
        {
            String format = source;
            String year = "";
            String month = "";
            String day = "";
            String[] temp;
            String result = "";
            int index = 0;
            if (format.Trim().Length > 10)
                format = "yyyy-mm-dd hh:mm:ss";
            else
                format = "yyyy-mm-dd";
            try
            {
                switch (format)
                {
                    case "yyyy-mm-dd hh:mm:ss":
                        year = source.Substring(index, source.IndexOf('/', index));
                        index = source.IndexOf('/', index) + 1;
                        month = source.Substring(index, source.IndexOf('/', index) - index);
                        index = source.IndexOf('/', index) + 1;
                        day = source.Substring(index, source.IndexOf(' ', index) - index);
                        break;
                    case "yyyy-mm-dd":
                        temp = source.Split(new string[] { "/", "-", " ", "yyyy", "mm", "dd" }, StringSplitOptions.None);
                        year = temp[0];
                        month = temp[1];
                        day = temp[2];
                        break;
                }
            }
            catch (Exception)
            {
                return "";
            }
            switch (strseperator)
            {
                case "/":
                    result = year + "/" + month + "/" + day;
                    break;
                case "-":
                    result = year + "-" + month + "-" + day;
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// Lay ngay , bo gio
        /// </summary>
        /// <param name="source">Ngay duoc truyen vao</param>
        /// <param name="seperator">Chuoi cach khoang giau ngay, thang, nam</param>
        /// <example>
        /// ConvertDateTimeToDate("2007/07/07 07:07:07","/") --> "2007/07/07"
        /// <br></br>
        /// ConvertDateTimeToDate("2007/07/07","/") --> "2007/07/07"
        /// </example>
        /// <returns>
        /// Chuoi dinh dang ngay, khong co gio
        /// </returns>
        public static String ConvertDateTimeToDate(String source, String seperator)
        {
            if (source == "") return "";
            if (source.Length > 10)
                source = source.Substring(0, 10);
            return ConvertDateSeperator(source, seperator);
        }
        //thoi add      
        //
        public static String ConvertIngresDate(String source)
        {
            String[] temp = source.Split(new string[] { "-", "/", "yyyy", "mm", "dd" }, StringSplitOptions.None);
            String result = "";
            try
            {
                //yyyy-mm-dd
                result = temp[0] + "-" + temp[1] + "-" + temp[2];

            }
            catch (Exception)
            {
                return "";
            }
            return result;
        }

        /// <summary>
        /// Dinh dang DateTime theo format cua Ingres
        /// </summary>
        /// <param name="source">DateTime</param>
        /// <returns>string </returns>
        public static String ConvertDateToIngresString(DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }
        #endregion

        #region | output format is DateTime |
        /// <summary>
        /// chuyen doi chuoi ngaythang sang kieu datetime
        /// input: 20/02/2013 14:30 - format: dd/MM/yyyy HH:mm
        /// output: datetime
        /// </summary>
        /// <param name="date"></param>
        /// <param name="strFomat"></param>
        /// <returns></returns>
        public static DateTime ConvertDateWithFormat(string date, string strFormat)
        {
            DateTime dateResult = DateTime.Now;
            try
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                DateTime.TryParseExact(date, strFormat, culture,
                        System.Globalization.DateTimeStyles.None, out dateResult);
            }
            catch (Exception ex)
            {
                dateResult = DateTime.Now;
                logger.Error(ex);
            }
            return dateResult;
        }            
                
        /// <summary>
        /// chuoi ban dau dd/mm/yyyyy 
        /// Doi chuoi sang ngay thang 
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDate(string strDate)
        {

            DateTime _date = DateTime.Now;
            try
            {
                if (strDate.Length == 10)
                {
                    string[] tmp = strDate.Split(new string[] { "/", "-" }, StringSplitOptions.None);
                    if (tmp[2].Length > 4) tmp[2] = tmp[2].Substring(0, 4);
                    _date = new DateTime(int.Parse(tmp[2]), int.Parse(tmp[1]), int.Parse(tmp[0]));
                }
                else if (strDate.Length == 7)
                {
                    string[] tmp = strDate.Split(new string[] { "/", "-" }, StringSplitOptions.None);
                    _date = new DateTime(int.Parse(tmp[1]), int.Parse(tmp[0]), 1);
                }
                else if (strDate.Length == 4)
                {
                    _date = new DateTime(int.Parse(strDate), 1, 1);
                }

            }
            catch (Exception)
            {
                return _date;
            }
            return _date;
        }
        #endregion

        #region
        /// <summary>
        /// Lay so ngay cua tuan voi ngay bat dau cua tuan neu ngay bat dau cua tuan la Chu Nhat (sunday)
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="isSundayIsFirstDayOfWeek">
        ///     true: Ngay bat dau cua tuan la Chu Nhat (sunday) 
        ///     false: Ngay bat dau cua tuan la Thu Hai (Monday)
        /// </param>
        /// <returns></returns>
        public static int NumberDayOfWeek(DateTime dateStart, bool isSundayIsFirstDayOfWeek)
        {
            switch (dateStart.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (isSundayIsFirstDayOfWeek)
                        return 6;
                    else
                        return 7;
                case DayOfWeek.Tuesday:
                    if (isSundayIsFirstDayOfWeek)
                        return 5;
                    else
                        return 6;
                case DayOfWeek.Wednesday:
                    if (isSundayIsFirstDayOfWeek)
                        return 4;
                    else
                        return 5;
                case DayOfWeek.Thursday:
                    if (isSundayIsFirstDayOfWeek)
                        return 3;
                    else
                        return 4;
                case DayOfWeek.Friday:
                    if (isSundayIsFirstDayOfWeek)
                        return 2;
                    else
                        return 3;
                case DayOfWeek.Saturday:
                    if (isSundayIsFirstDayOfWeek)
                        return 1;
                    else
                        return 2;
                case DayOfWeek.Sunday:
                    if (isSundayIsFirstDayOfWeek)
                        return 7;
                    else
                        return 1;
                default: return 0;
            }
        }

        /// <summary>
        /// Ghi chu: ngay bat dau trong tuan la thu hai
        /// </summary>
        /// <param name="strDayOfWeek"></param>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeWithDayOfWeek(int dayOfWeek, string strTime)
        {            
            int dayOfWeek_now = NumberDayOfWeek(DateTime.Now, false);
            if (dayOfWeek_now == dayOfWeek)
                return FunctionsDateTime.ConvertDateWithFormat(string.Format("{0}/{1}/{2} {3}", DateTime.Now.Day.ToString("00"), DateTime.Now.Month.ToString("00"), DateTime.Now.Year, strTime), "dd/MM/yyyy HH:mm");
            else
                return FunctionsDateTime.ConvertDateWithFormat(string.Format("{0}/{1}/{2} {3}", (DateTime.Now.Day + (dayOfWeek_now - dayOfWeek)).ToString("00"), DateTime.Now.Month.ToString("00"), DateTime.Now.Year, strTime), "dd/MM/yyyy HH:mm");         
        }

        /// <summary>
        /// Lấy thứ trong tuần
        /// </summary>
        /// <param name="dateStart"></param>
        /// <returns>
        /// Thứ hai, Thứ ba, Thứ tư...
        /// </returns>
        public static string VietnameseTextDayOfWeek(DateTime dateStart)
        {
            switch (dateStart.DayOfWeek)
            {
                case DayOfWeek.Monday: return "Thứ hai";
                case DayOfWeek.Tuesday: return "Thứ ba";
                case DayOfWeek.Wednesday: return "Thứ tư";
                case DayOfWeek.Thursday: return "Thứ năm";
                case DayOfWeek.Friday: return "Thứ sáu";
                case DayOfWeek.Saturday: return "Thứ bảy";
                case DayOfWeek.Sunday: return "Chủ nhật";
                default: return "";
            }
        }

        public static string DayOfWeek_CN(DateTime dateStart)
        {
            switch (dateStart.DayOfWeek)
            {
                case DayOfWeek.Monday: return "Thứ hai";
                case DayOfWeek.Tuesday: return "Thứ ba";
                case DayOfWeek.Wednesday: return "Thứ tư";
                case DayOfWeek.Thursday: return "Thứ năm";
                case DayOfWeek.Friday: return "Thứ sáu";
                case DayOfWeek.Saturday: return "T7";
                case DayOfWeek.Sunday: return "CN";
                default: return "";
            }
        }


        /// <summary>
        /// Lay so tuan cua thang
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int NumberWeekOfMonth(DateTime date)
        {
            int week = 1;
            int numberdayofweek;
            int day = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime tmp = new DateTime(date.Year, date.Month, day);
            while (true)
            {
                numberdayofweek = NumberDayOfWeek(date, true);
                date = date.AddDays(numberdayofweek);
                if (date > tmp)
                    break;
                else
                    week++;
            }
            return week;
        }

        /// <summary>
        /// lay so tuan cua nam
        /// </summary>
        /// <param name="dateStart">DateTime</param>
        /// <returns></returns>
        public static int NumberWeekOfYear(DateTime dateStart)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo dfi = System.Globalization.DateTimeFormatInfo.CurrentInfo;
                DateTime date1 = dateStart;
                System.Globalization.Calendar cal = dfi.Calendar;

                return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return 0;
        }

        /// <summary>
        /// lay so tuan cua nam
        /// </summary>
        /// <param name="dateStart">datetime</param>
        /// <param name="isMondayIsFirstDayOfWeek">isMondayIsFirstDayOfWeek</param>
        /// <returns></returns>
        public static int NumberWeekOfYear(DateTime dateStart, bool isMondayIsFirstDayOfWeek)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo dfi = System.Globalization.DateTimeFormatInfo.CurrentInfo;
                DayOfWeek firstDay = dfi.FirstDayOfWeek;
                if (isMondayIsFirstDayOfWeek)
                    firstDay = DayOfWeek.Monday;
                DateTime date1 = dateStart;
                System.Globalization.Calendar cal = dfi.Calendar;

                return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, firstDay);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void DayStartEndWeek(DateTime date, out int start, out int end)
        {
            start = 0;
            end = 0;
            //So ngay den coi tuan
            int day = NumberDayOfWeek(date, false);
            start = date.Day + day - 7;
            if (start <= 0) start = 1;
            end = date.Day + day - 1;
            if (end > DateTime.DaysInMonth(date.Year, date.Month))
                end = DateTime.DaysInMonth(date.Year, date.Month);

        }

        /// <summary>
        /// SO ngay thu 7 va CN trong thang
        /// in : year ,month
        /// out : tong so ngay 
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns></returns>        
        public static int NumberHolidayofMonth(int year, int month)
        {
            int day = 0;
            int i = 1;
            while (i <= DateTime.DaysInMonth(year, month))
            {
                DateTime date = new DateTime(year, month, i);
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday: { i += 5; break; }
                    case DayOfWeek.Tuesday: { i += 4; break; }
                    case DayOfWeek.Wednesday: { i += 3; break; }
                    case DayOfWeek.Thursday: { i += 2; break; }
                    case DayOfWeek.Friday: { i += 1; break; }
                    case DayOfWeek.Saturday: { i += 1; day++; break; }
                    case DayOfWeek.Sunday: { i += 6; day++; break; }
                    default: break;
                }
            }
            return day;
        }

        /// <summary>
        /// So sanh ngay
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns>
        ///     kq = true : dateTime1 >= dateTiem2
        ///     kq = false : dateTime1 < dateTiem2
        /// </returns>
        public static bool CheckOverDate(DateTime dateTime1, DateTime dateTime2)
        {
            // lay TimeSpan cua hai ngay khac nhau
            TimeSpan elapsedNow = dateTime1.Subtract(dateTime2);
            int kq = ((elapsedNow.Days * 24) + (elapsedNow.Hours * 60) + (elapsedNow.Minutes * 60) + (elapsedNow.Seconds * 60));
            if (kq >= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">dd/MM/yyyy HH:mm</param>
        /// <returns>
        /// - hôm nay, 01/02/2014
        /// - hôm qua, 01/02/2014
        /// - Thứ hai, 01/02/2014
        /// </returns>
        public static string GetDateTimeClientCustomFormat(string date)
        {
            string result = "";
            DateTime dateTime1 = FunctionsDateTime.ConvertDateWithFormat(date, "dd/MM/yyyy HH:mm");
            if (dateTime1.Day == DateTime.Now.Day &&
                dateTime1.Month == DateTime.Now.Month &&
                dateTime1.Year == DateTime.Now.Year)
            {
                result = string.Format("Hôm nay, vào lúc {0}", dateTime1.ToString("HH':'mm"));
            }
            else
            {
                if (dateTime1.Day == DateTime.Now.Day - 1 &&
                dateTime1.Month == DateTime.Now.Month &&
                dateTime1.Year == DateTime.Now.Year)
                {
                    result = string.Format("Hôm qua, vào lúc {0}", dateTime1.ToString("HH':'mm"));
                }
                else
                {
                    int dayStart = 0;
                    int dayEnd = 0;
                    DayStartEndWeek(DateTime.Now, out dayStart, out dayEnd);
                    if ((dateTime1.Day <= dayEnd &&
                        dateTime1.Day >= dayStart) &&
                        dateTime1.Month == DateTime.Now.Month &&
                        dateTime1.Year == DateTime.Now.Year)
                    {
                        result = string.Format("{0}, {1}", VietnameseTextDayOfWeek(dateTime1), date);
                    }
                    else
                    {
                        return date;
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
