using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHG.DataServices.DataClass
{
    public class CV_CongViec_TapTin : BaseClass
    {
        public string macongviec { get; set; }
        public string mataptin { get; set; }
        public string tentaptin { get; set; }
        public string extensionfile { get; set; }
        public string filecontenttype { get; set; }
        public string binarydata { get; set; }
        public string xoa { get; set; }
        public string nguoitao { get; set; }
        public string ngaytao { get; set; }
        public string nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }
    }
}
