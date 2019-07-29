using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Models
{
    public class SystemInfoModel
    {
        public string OperatingSystem { get; set; }
        public string AspNetInfo { get; set; }
        public string IsFullTrust { get; set; }
        public string ServerTimeZone { get; set; }
        public string ServerLocalTime { get; set; }
        public string UtcTime { get; set; }
        public string HttpHost { get; set; }
    }

    public class LogModels
    {
        public string stt { get; set; }
        public string LogID { get; set; }
        public string NoiDung { get; set; }
        public string CapDo { get; set; }
        public string NgayTao { get; set; }
    }
}