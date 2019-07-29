using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NHG.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        public string mataikhoan { get; set; }
        public string manhansu { get; set; }
        public string tendangnhap { get; set; }
        public string matkhau { get; set; }
        public string thudientu { get; set; }

        public string hoten { get; set; }
        public string diachi { get; set; }
        public string sodienthoai { get; set; }
        public string tenchucdanh { get; set; }

        public int madonvi { get; set; }

        public string maphongban { get; set; }
        public int machucdanh { get; set; }

        public string kichhoat { get; set; }

        public List<PhongBanModels> PhongBans { get; set; }
        public List<DonviModels> DonVis { get; set; }
        public List<ChucDanhModels> ChucDanhs { get; set; }

        public int nguoitao { get; set; }
        public int nguoihieuchinh { get; set; }
        public string ngaytao { get; set; }
        public string xoa { get; set; }
        public string grouptk { get; set; }

    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
