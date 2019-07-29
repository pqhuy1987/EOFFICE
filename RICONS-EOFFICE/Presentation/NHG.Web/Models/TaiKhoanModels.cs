using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NHG.Web.Models
{
    public class TaiKhoanModels
    {
        public string stt { get; set; }
        public string mataikhoan { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string tendangnhap { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string thudientu { get; set; }

        [Required]
        [MaxLength(256)]
        [Column(TypeName = "varchar")]
        public string hoten { get; set; }

        [Required]
        [MaxLength(256)]
        [Column(TypeName = "varchar")]
        public string tenchucdanh { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar")]
        public string diachi { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(15)]
        public string matkhau { get; set; }
        public string manhansu { get; set; }
        public string gioitinh { get; set; }
        public string tenhinh { get; set; }
        public string tenphongban { get; set; }
        public string donvi { get; set; }
        public string extensionfile { get; set; }
        public string filecontenttype { get; set; }
        public byte[] binarydata { get; set; }
        public string is_ada { get; set; }
        public string sodienthoai { get; set; }
        public string tinhtrang { get; set; }
        public string grouptk { get; set; }
        public string kichhoat { get; set; }
        
        public string  ngaysinh { get; set; }

        public int madonvi { get; set; }
        public string tendonvi { get; set; }
        public int machucdanh { get; set; }
       
        public string maphongban { get; set; }
        public List<PhongBanDonViModels> phongBanDonVis { get; set; }

    }

    public class PhongBanDonViModels
    {
        public string maphongban { get; set; }
        public int madonvi { get; set; }
        public string machucdanh { get; set; }
        public string tenphongban { get; set; }
        public string tendonvi { get; set; }
        public string tenchucdanh { get; set; }
    }

    public class TaiKhoanForCombobox
    {
        public string mataikhoan { get; set; }
        public string hoten { get; set; }
    }
}