using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Models
{
    public class KeHoachModels
    {
        public int madonvi { get; set; }
        public string stt { get; set; }
        public string noidungmuctieu { get; set; }
        public string tytrong { get; set; }
        public string chitieunam { get; set; }
        public string ngaybatdau { get; set; }
        public string ngayketthuc { get; set; }
        public string ngayhoanthanh { get; set; }
        public string hoanthanh { get; set; }
        public string manguoithuchien { get; set; }
        public string nguoithuchien { get; set; }
        public string ghichu { get; set; }
        public string nguoitao { get; set; }
        public string makehoachgoc { get; set; }
        public string makehoach { get; set; }
        public int thangbatdau { get; set; }
        public int thangketthuc { get; set; }
        public int sothangthuchien { get; set; }
        
        public string t1 { get; set; }
        public string t2 { get; set; }
        public string t3 { get; set; }
        public string t4 { get; set; }
        public string t5 { get; set; }
        public string t6 { get; set; }
        public string t7 { get; set; }
        public string t8 { get; set; }
        public string t9 { get; set; }
        public string t10 { get; set; }
        public string t11 { get; set; }
        public string t12 { get; set; }
    }

    public class MucTieuCongViecModels
    {
        public int madonvi { get; set; }
        public string makehoach { get; set; }
        public string stt { get; set; }
        public string noidungmuctieu { get; set; }
        public string tytrong { get; set; }
        public string chitieunam { get; set; }
        public string ngaybatdau { get; set; }
        public string ngayketthuc { get; set; }
        public string ngayhoanthanh { get; set; }
        public string hoanthanh { get; set; }
        public string chiphi { get; set; }
        public string ghichu { get; set; }
        public string nguoinhanmuctieu { get; set; }

        public string nguoinhanmuctieuphu { get; set; }

        public string nguoiduyetmuctieu { get; set; }

        public string xoa { get; set; }
        public string nguoitao { get; set; }
        public string ngaytao { get; set; }
        // Danh cho qua trinh insert vao trong Ke hoach hanh dong
        public string makehoachgoc { get; set; }
        public string danhgia { get; set; }
        public string ketqua { get; set; }
        public string tinhtrang { get; set; }

    }

    public class KeHoachLichSuModels
    {
        public string MaLichSu { get; set; }
        public string MaKeHoach { get; set; }
        public string NoiDung { get; set; }
        
    }

    public class KeHoachNguoiThucHienModels
    {
        public string MaKeHoach { get; set; }
        public string NguoiThucHien { get; set; }
    }

    public class KeHoachForCreate
    {
        public List<TaiKhoanForCombobox> nguoiThucHiens { get; set; }
        public List<KeHoachForCombobox> keHoachs { get; set; }
    }

    public class KeHoachForCombobox
    {
        public string maKeHoach { get; set; }
        public string noidungmuctieu { get; set; }
        public string makehoachgoc { get; set; }
    }
}