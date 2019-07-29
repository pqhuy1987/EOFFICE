using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHG.DataServices.DataClass
{
    public class M_TaiKhoan : BaseClass
    {
        public string mataikhoan { get; set; }
        public string tendangnhap { get; set; }
        public string matkhau { get; set; }
        public string hoten { get; set; }
        public string thudientu { get; set; }
        public int madonvi { get; set; }
        public string tendonvi { get; set; }
        public int machucdanh { get; set; }
        public string tenchucdanh { get; set; }
        public string maphongban { get; set; }
        public string tenphongban { get; set; }
        public string kichhoat { get; set; }
        public string tenhinh { get; set; }
        public string extensionfile { get; set; }
        public string filecontenttype { get; set; }
        public byte[] binarydata { get; set; }
        public string is_ada { get; set; }
        public string xoa { get; set; }
        public string nguoitao { get; set; }
        public string ngaytao { get; set; }
        public string nguoihieuchinh { get; set; }
        public string ngayhieuchinh { get; set; }
    }
}
