using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Models
{
    public class CongViecModels
    {
        public string macongviec { get; set; }
        public string madonvi { get; set; }
        public string congviecgoc { get; set; }
        public string makehoach { get; set; }
        public string tieude { get; set; }
        public string noidung { get; set; }
        public string ghichu { get; set; }
        public string ngaybatdau { get; set; }
        public string ngayketthuc { get; set; }
        public string ngayhoanthanh { get; set; }
        public string hoanthanh { get; set; }
        public string tinhtrang { get; set; }
        public string capdouutien { get; set; }
        public string is_taptin { get; set; }
        public string xoa { get; set; }
        public string nguoitao { get; set; }
        public string ngaytao { get; set; }
        public string nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }

        //ngoai bang
        public string tenviettat { get; set; }
        public string limit { get; set; }
        public string offset { get; set; }
    }

    public class CongViecLichSuModels
    {
        public string macongviec { get; set; }
        public string ngaythaydoi { get; set; }
        public string nguoithaydoi { get; set; }
        public string makehoach { get; set; }
        public string noidung { get; set; }
        public string taptin { get; set; }
        public string extensionfile { get; set; }
        public string filecontenttype { get; set; }
        public byte[] binarydata { get; set; }

        //ngoai bang
        public string limit { get; set; }
        public string offset { get; set; }
    }

    public class TapTinCongViecModels
    {
        public string macongviec { get; set; }
        public string mataptin { get; set; }
        public string tentaptin { get; set; }
        public string extensionfile { get; set; }
        public string filecontenttype { get; set; }
        public byte[] binarydata { get; set; }
        public string xoa { get; set; }
        public string nguoitao { get; set; }
        public string ngaytao { get; set; }
        public string nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }


        //ngoai bang
        public string limit { get; set; }
        public string offset { get; set; }
    }
}