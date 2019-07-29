using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Models
{
    public class PhongBanModels
    {
        public string stt { get; set; }
        public string madonvi { get; set; }
        public string tendonvi { get; set; }
        public string maphongban { get; set; }
        public string tenphongban { get; set; }
        public string sodienthoai { get; set; }
        public string email { get; set; }
        public string hovaten { get; set; }
        public string thuocquanly { get; set; }
        public string xoa { get; set; }
        public string ghichu { get; set; }
        public int nguoitao { get; set; }
        public string ngaytao { get; set; }
        public int nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }
        public int phongban_congtruong { get; set; }
    }

    public class thongtingiamdocModels
    {
        public string stt { get; set; }
        public string mathongtin { get; set; }
        public string hovaten { get; set; }
        public string email { get; set; }
        public string xoa { get; set; }
    }

   
}