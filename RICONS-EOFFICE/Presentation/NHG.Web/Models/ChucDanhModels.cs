using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Models
{
    public class ChucDanhModels
    {
        public string stt { get; set; }
        public int machucdanh { get; set; }
        //public string maphongban { get; set; }
        //public string tenphongban { get; set; }
        public string tenchucdanh { get; set; }
        public string tengiaodich { get; set; }
        public string xoa { get; set; }
        public string ghichu { get; set; }
        public int nguoitao { get; set; }
        public string ngaytao { get; set; }
        public int nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }
    }

    public class Danhmuc_VanPhongPhamModels
    {
        public string stt { get; set; }
        public int mavanphongpham { get; set; }
        public string tenvanphongpham { get; set; }
        public string dongia { get; set; }
        public string donvitinh { get; set; }
        public string danhmuccha { get; set; }
        public string danhmucgoc { get; set; }
        public string ghichu { get; set; }
        public string xoa { get; set; }
        public int nguoitao { get; set; }
        public string ngaytao { get; set; }
        public int nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }
    }

    public class DangkyvppModels
    {
        public string stt { get; set; }
        public int madangky { get; set; }
        public string hovaten { get; set; }
        public int machucdanh { get; set; }
        public string tenchucdanh { get; set; }
        public string maphongban { get; set; }
        public string tenphongban { get; set; }
        public string ngaydangky { get; set; }
        public string tongtien { get; set; }
        public string ghichu { get; set; }

        public string dongia { get; set; }

        public string xoa { get; set; }
        public int nguoitao { get; set; }
        public string ngaytao { get; set; }
        public int nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }
    }

    public class Dangkyvpp_chitietModels
    {
        public string stt { get; set; }
        public int madangky_chitiet { get; set; }
        public int madangky { get; set; }
        public int madanhmuc { get; set; }
        public string tendanhmuc { get; set; }
        public string dongia { get; set; }
        public string soluong { get; set; }
        public string thanhtien { get; set; }
        public string donvitinh { get; set; }
        public string xoa { get; set; }
        public int nguoitao { get; set; }
        public string ngaytao { get; set; }
        public int nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }  
        public string danhmuccha { get; set; }
        public string danhmucgoc { get; set; }
    }

}