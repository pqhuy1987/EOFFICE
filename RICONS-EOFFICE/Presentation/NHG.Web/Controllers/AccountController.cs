using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using NHG.Web.Models;
using NHG.Logger;
using NHG.Web.Data;
using NHG.Web.Data.Services;
using System.DirectoryServices.AccountManagement;
using Nop.Admin.Controllers;
using NHG.Core.Functions;
using System.Text;
using System.IO;
using NHG.Core.Constants;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using NHG.DataServices.DataClass;

namespace NHG.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Fields
        Log4Net _logger = new Log4Net(typeof(AccountController));
        /// <summary>
        /// Session luu captcha
        /// </summary>
        string strSessionCaptcha = "Captcha";
        /// <summary>
        /// Session luu so luong dang nhap loi hoac dang nhap khong thanh cong
        /// </summary>
        string strSessionLoginFail = "LoginFail";
        #endregion

        #region Action
        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            if (IsLogged())
                return RedirectToLocal("");
            if (ModelState.IsValid)
            {
                RandomStringGenerator random = new RandomStringGenerator();
                random.UseSpecialCharacters = false;
                random.UseUpperCaseCharacters = false;
                random.UseLowerCaseCharacters = false;
                ViewBag.key = random.Generate(16);
                Session["keylogin"] = ViewBag.key;
                ViewBag.isNhapCaptcha = false;
                ViewBag.linkCaptcha = "";
                ViewBag.ReturnUrl = returnUrl;
            }
            return View();
        }

        //return RedirectToAction("Index", "Home");
        // POST: /Account/Login
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TaiKhoanModels model, string returnUrl, string captcha)
        {
            _logger.Start("Login");
            bool bCaptcha = false;
            ViewBag.isNhapCaptcha = false;
            LoginServices service = new LoginServices();
            try
            {
                //neu so lan user dang nhap vuot qua gioi han se bi lock
                if (Session[strSessionLoginFail] == null)
                    Session[strSessionLoginFail] = 0;
                int loginFail = (int)Session[strSessionLoginFail];
                if (!string.IsNullOrEmpty(captcha) && !string.IsNullOrWhiteSpace(captcha))
                    if (captcha == Session[strSessionCaptcha].ToString())
                        bCaptcha = true;
                if (loginFail < NHG.Core.Constants.CST_Common.CST_Lock || bCaptcha)
                {
                    #region kiem tra user login tren he thong AD admin
                    bool validAD = false;

                    string matkhaudangnnhap = model.matkhau;

                    var passWordDecrypt = EncDec.DecryptStringAES(model.matkhau);
                    try
                    {
                        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "newtecons.vn"))
                        {
                            validAD = context.ValidateCredentials(model.tendangnhap, passWordDecrypt);
                            if (validAD)
                            {
                                #region
                                var user = service.GetLoginData(new M_TaiKhoan()
                                {
                                    tendangnhap = model.tendangnhap,
                                    xoa = CST_Common.CST_NOT_DELETE,
                                    kichhoat = CST_Common.CST_ACTIVE
                                });

                                TaiKhoanServices serTaiKhoan = new TaiKhoanServices();
                                if (user == null)
                                {
                                    TaiKhoanModels modeltk = new TaiKhoanModels();
                                    modeltk.tendangnhap = model.tendangnhap;
                                    model.matkhau = matkhaudangnnhap;
                                    model.madonvi = 1;
                                    modeltk.thudientu = model.tendangnhap.Trim() + "@newtecons.vn";
                                    modeltk.machucdanh = 0;
                                    modeltk.maphongban = "0";
                                    bool kq = serTaiKhoan.AddUser(modeltk, 0);
                                    user = service.GetLoginData(new M_TaiKhoan()
                                    {
                                        tendangnhap = model.tendangnhap,
                                        xoa = CST_Common.CST_NOT_DELETE,
                                        kichhoat = CST_Common.CST_ACTIVE
                                    });
                                }
                                var lstPhongBanDonVis = serTaiKhoan.SelectPhongBanDonVi(new M_TaiKhoan()
                                {
                                    mataikhoan = user.mataikhoan
                                });
                                user.phongBanDonVis = lstPhongBanDonVis;
                                AddSession(user);
                                UpdateLoginState(model.tendangnhap, false, loginFail);
                                //chuyen trang
                                _logger.End("Login");
                                return RedirectToAction("Index", "Home");
                                //return RedirectToLocal(returnUrl);
                                #endregion
                            }
                            if (!validAD)
                            {
                                var user = service.GetLoginData(new M_TaiKhoan()
                                {
                                    tendangnhap = model.tendangnhap,
                                    matkhau = EncDec.EncodePassword(passWordDecrypt),
                                    xoa = CST_Common.CST_NOT_DELETE,
                                    kichhoat = CST_Common.CST_ACTIVE
                                });
                                if (user != null)
                                {
                                    TaiKhoanServices serTaiKhoan = new TaiKhoanServices();
                                    var lstPhongBanDonVis = serTaiKhoan.SelectPhongBanDonVi(new M_TaiKhoan()
                                    {
                                        mataikhoan = user.mataikhoan
                                    });
                                    user.phongBanDonVis = lstPhongBanDonVis;
                                    AddSession(user);
                                    UpdateLoginState(model.tendangnhap, false, loginFail);
                                    _logger.End("Login");
                                    return RedirectToAction("Index", "Home");

                                }
                                else
                                {
                                    try
                                    {

                                        UpdateLoginState(model.tendangnhap, true, loginFail);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error(ex);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        validAD = false;
                        _logger.Info(ex);
                    }
                    #endregion

                    #region kiem tra user login local

                    if (!validAD)
                    {
                        var user = service.GetLoginData(new M_TaiKhoan()
                        {
                            tendangnhap = model.tendangnhap,
                            matkhau = EncDec.EncodePassword(passWordDecrypt),
                            xoa = CST_Common.CST_NOT_DELETE,
                            kichhoat = CST_Common.CST_ACTIVE
                        });
                        if (user != null)
                        {
                            TaiKhoanServices serTaiKhoan = new TaiKhoanServices();
                            var lstPhongBanDonVis = serTaiKhoan.SelectPhongBanDonVi(new M_TaiKhoan()
                            {
                                mataikhoan = user.mataikhoan
                            });
                            user.phongBanDonVis = lstPhongBanDonVis;
                            AddSession(user);
                            UpdateLoginState(model.tendangnhap, false, loginFail);
                            _logger.End("Login");
                            return RedirectToAction("Index", "Home");

                        }
                        else
                        {
                            try
                            {

                                UpdateLoginState(model.tendangnhap, true, loginFail);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    ViewBag.isNhapCaptcha = true;
                    UpdateLoginState(model.tendangnhap, true, loginFail);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _logger.End("CheckLogin");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpGet]
        public ActionResult LogOff()
        {
            LoginServices service = new LoginServices();
            service.LogoutHistory(Session["loginid"].ToString(), this.Session.SessionID);
            //clear profile user
            DirectoryInfo dirRemove = new DirectoryInfo(CommonHelper.GetFolderProfileUser());
            if (dirRemove.Exists)
                dirRemove.Delete(true);

            //xoa sessiong dang nhap
            RemoveSession();

            //AuthenticationManager.SignOut();
            _logger.Info("Log Off");
            return BackToLogin();
        }

        //
        // GET: /Account/Profile
        public ActionResult Personal()
        {
            if (!IsLogged())
                return BackToLogin();
            TaiKhoanServices service = new TaiKhoanServices();
            TaiKhoanModels result = service.SelectDetail(new M_TaiKhoan()
            {
                mataikhoan = Session["userid"].ToString()
            });
            if (result != null)
                return View(result);
            return View();
        }


        //
        // POST: /Account/Profile
        [HttpPost]
        public ActionResult Personal(M_TaiKhoan model, string returnUrl)
        {
            if (!IsLogged())
                return BackToLogin();
            TaiKhoanServices service = new TaiKhoanServices();
            model.mataikhoan = Session["userid"].ToString();
            model.nguoihieuchinh = Session["userid"].ToString();
            model.ngayhieuchinh = "GETDATE()";
            service.UpdateProfile(model);
            TaiKhoanModels result = service.SelectDetail(new M_TaiKhoan()
            {
                mataikhoan = Session["userid"].ToString()
            });
            if (result != null)
                return View(result);
            return View();
        }


        //
        // GET: /Account/Manage
        public ActionResult Manage()
        {
            if (!IsLogged())
                return BackToLogin();
            DonViServices donViSer = new DonViServices();
            PhongBanServices phongBanSer = new PhongBanServices();
            DanhmucServices chucdanhser = new DanhmucServices();
            ManageUserViewModel model = PrepareManageUserViewModel(phongBanSer.SelectRows(new PhongBanModels()), donViSer.SelectRows(new DonviModels()), chucdanhser.SelectRows_chucvu(new ChucDanhModels()));
            return View(model);
        }

       

        public ActionResult Edit(string id)
        {
            //AES.Decrypt(id.Replace(" ", "+"), Convert.FromBase64String(Session.SessionID))
            //kiem tra tai khoan dang nhap
            if (!IsLogged())
                return BackToLogin();
            string strID = EncDec.DecodeCrypto(id);
                return View();
        }
        #endregion

        #region manage session
        /// <summary>
        /// Neu dang nhap thanh cong add session cho user
        /// </summary>
        /// 
        private void AddSession(TaiKhoanModels model)
        {
            Log4Net.userID = model.mataikhoan;
            Session["userid"] = model.mataikhoan;
            Session["fullname"] = model.hoten;
            Session["loginid"] = model.tendangnhap;
            Session["thudientu"] = model.thudientu;
            Session["phongBanDonVi"] = model.phongBanDonVis;
            Session["maphongban"] = model.maphongban;
            Session["machucdanh"] = model.machucdanh;
            Session["tenchucdanh"] = model.tenchucdanh;
            Session["sodienthoai"] = model.sodienthoai;
            Session["manhansu"] = model.manhansu;
            Session["ngaysinh"] = model.ngaysinh;
            Session["grouptk"] = model.grouptk;
            if (GetPhongBanDonVi() != null)
            {
                Session["tenchucvu"] = GetPhongBanDonVi().tenchucdanh;
                Session["maphongban"] = GetPhongBanDonVi().maphongban;
            }
            else
            {
                Session["tenchucvu"] = model.tenchucdanh;
            }
            Session["manhansu"] = model.manhansu;
            //thong tin he thong
            Session["sessionid"] = Session.SessionID;
            Session["clientIP"] = Log4Net.clientIP;
            Session["browser"] = Log4Net.browser;
            //ma hoa session
            EncrypteSession();

            //khoi tao profile ban dau
            CommonHelper.CreateFirstProfileUser();

            //tao thu muc temp de user su dung trong qua trinh lam viec 
            string pathCache = CommonHelper.GetFolderProfileUser();
            if (!Directory.Exists(pathCache))
                Directory.CreateDirectory(pathCache);
        }

        private void RemoveSession()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();

        }

        private void EncrypteSession()
        {
            #region
            string _browserInfo = Request.Browser.Browser +
                                  Request.Browser.Version +
                                  Request.UserAgent + "~" +
                                  Request.ServerVariables["REMOTE_ADDR"];
            string _sessionValue = Convert.ToString(Session["userid"]) + "^" +
                                   DateTime.Now.Ticks + "^" +
                                   _browserInfo + "^" +
                                   System.Guid.NewGuid();
            byte[] _encodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_sessionValue);
            string _encryptedString = Convert.ToBase64String(_encodeAsBytes);
            Session["encryptedSession"] = _encryptedString;
            #endregion
        }
        #endregion

        #region ajax post link
        /// LayDanhSach
        [HttpPost]
        public JsonResult SelectRows(ManageUserViewModel model)
        {
            TaiKhoanServices service = new TaiKhoanServices();
            List<TaiKhoanModels> lstResult = service.SelectRows(new M_TaiKhoan() { 
                kichhoat = model.kichhoat,
                madonvi = model.madonvi,
                maphongban = model.maphongban,
                tendangnhap=model.tendangnhap
            });
            StringBuilder result = new StringBuilder();
            StringBuilder lstRow = new StringBuilder();
            if (lstResult.Count > 0)
            {
                int i = 1;
                foreach (var item in lstResult)
                {
                    lstRow.Append(PrepareDataJson_DanhSachTaiKhoan(item, i));
                    i++;
                }
                if (lstRow.Length > 0)
                    lstRow.Remove(lstRow.Length - 1, 1);
            }
            result.Append("{");
            result.Append("\"isHeader\":\"" + "111" + "\",");
            result.Append("\"Pages\":\"" + "212" + "\",");
            result.Append("\"data\":[" + lstRow.ToString() + "]");
            result.Append("}");
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Method

        private void UpdateLoginState(string username, bool isLoginFail, int loginFail)
        {
            LoginServices service = new LoginServices();
            string strDangNhapThanhCong = "1";
            string strOnline = "1";
            string strLoginFail = "";
            if (string.IsNullOrEmpty(username))
                username = Session["loginid"].ToString();
            if (isLoginFail)
            {
                strDangNhapThanhCong = strOnline = "0";
                strLoginFail = (loginFail + 1).ToString();
                Session[strSessionLoginFail] = loginFail + 1;
                ViewBag.strAlert = string.Format("Đăng nhập không thành công! <br/>(đăng nhập sai {0}/{1})", strLoginFail, CST_Common.CST_Lock);
            }
            if (loginFail == 0 && !isLoginFail)
            {
                strLoginFail = "0";
            }

            if (service.CheckExistUserInfoHistory(username, this.Session.SessionID))
                service.UpdateHistoryLogin(username, this.Session.SessionID, strLoginFail, strDangNhapThanhCong, strOnline);
            else
                service.InsertHistoryLogin(username, this.Session.SessionID, Log4Net.clientIP, Log4Net.browser, Log4Net.useragent, strLoginFail, strDangNhapThanhCong, strOnline);
        }

        private StringBuilder PrepareDataJson_DanhSachTaiKhoan(TaiKhoanModels model, int couter)
        {
            StringBuilder sbResult = new StringBuilder();
            try
            {
                sbResult.Append("{");
                sbResult.Append("\"col_class\":\"rows-box\",");
                sbResult.Append("\"col_id\":\"" + model.mataikhoan + "\",");
                sbResult.Append("\"col_value\":[");
                #region Data cell
                //colum checkbox
                string strHTML_Checkbox = string.Format("<input type='checkbox' onclick='Select(this);' class='chkCheck' codeid='{0}'/>", model.mataikhoan);
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col1\",");
                sbResult.Append("\"col_id\":\"1\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Checkbox + "\"");
                sbResult.Append("},");
                //stt
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col2 stt\",");
                sbResult.Append("\"col_id\":\"2\",");
                sbResult.Append("\"col_value\":\"" + model.stt + "\"");
                sbResult.Append("},");

                //tinh trang
                FunctionXML fnc = new FunctionXML(Functions.MapPath("~/Xml/Const/Default.xml"));
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col3\",");
                sbResult.Append("\"col_id\":\"3\",");
                sbResult.Append("\"title\":\""+model.tinhtrang.Trim()+"\",");
                sbResult.Append("\"col_value\":\"" + fnc.ReadConst("kichhoat", int.Parse(model.tinhtrang)) + "\"");
                sbResult.Append("},");

                //noi dung
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col4\",");
                sbResult.Append("\"col_id\":\"4\",");
                sbResult.Append("\"title\":\"" + model.grouptk + "\",");
                sbResult.Append("\"col_value\":\"" + model.tendangnhap + "\"");
                sbResult.Append("},");
                //ten dang nhap
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col5\",");
                sbResult.Append("\"col_id\":\"5\",");
                sbResult.Append("\"title\":\"" + model.grouptk + "\",");
                sbResult.Append("\"col_value\":\"" + model.hoten + "\"");
                sbResult.Append("},");

                //Email
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col6\",");
                sbResult.Append("\"col_id\":\"6\",");
                sbResult.Append("\"col_value\":\"" + model.thudientu + "\"");
                sbResult.Append("},");

                //phong ban
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col7\",");
                sbResult.Append("\"col_id\":\"7\",");
                sbResult.Append("\"title\":\"" + model.maphongban.Trim() + "\",");
                sbResult.Append("\"col_value\":\"" + model.tenphongban + "\"");
                sbResult.Append("},");

                //chucdanh
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col8\",");
                sbResult.Append("\"col_id\":\"8\",");
                sbResult.Append("\"title\":\"" + model.machucdanh + "\",");
                sbResult.Append("\"col_value\":\"" + model.tenchucdanh + "\"");
                sbResult.Append("},");

                //chucdanh
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col9\",");
                sbResult.Append("\"col_id\":\"9\",");
                sbResult.Append("\"title\":\""+model.madonvi+"\",");
                sbResult.Append("\"col_value\":\"" + model.tendonvi + "\"");
                sbResult.Append("},");

                //dinh kem tap tin
                string strHTML_Attachment = "";
                #region
                string link = Url.Action("Edit", "Account", new { id = EncDec.EncodeCrypto(model.mataikhoan)});
                strHTML_Attachment = "<a href='#' class='edit' ><i class='fa fa-pencil-square-o' ></i></a>&nbsp;    <a href='#' class='del'><i class='fa fa-trash-o' ></i></a>";
                #endregion
                sbResult.Append("{");
                sbResult.Append("\"colspan\":\"1\",");
                sbResult.Append("\"col_class\":\"ovh col10\",");
                sbResult.Append("\"col_id\":\"10\",");
                sbResult.Append("\"col_value\":\"" + strHTML_Attachment + "\"");
                sbResult.Append("}");

                ////chucdanh
                //sbResult.Append("{");
                //sbResult.Append("\"colspan\":\"1\",");
                //sbResult.Append("\"col_class\":\"ovh col11\",");
                //sbResult.Append("\"col_id\":\"11\",");
                //sbResult.Append("\"type\":\"hidden\",");
                //sbResult.Append("\"col_value\":\"" + model.mataikhoan + "\"");
                //sbResult.Append("}");


                #endregion

                sbResult.Append("]");
                sbResult.Append("},");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return sbResult;
        }


        private ManageUserViewModel PrepareManageUserViewModel(List<PhongBanModels> phongBans, List<DonviModels> donVis, List<ChucDanhModels> chucDanhs)
        {
            ManageUserViewModel model = new ManageUserViewModel();
            try
            {
                model.PhongBans = phongBans;
                model.DonVis = donVis;
                model.ChucDanhs = chucDanhs;
            }
            catch (Exception ex)
            {
                model = new ManageUserViewModel();
                _logger.Error(ex);
            }
            return model;
        }
        #endregion

        public ActionResult Create()
        {
            //kiem tra tai khoan dang nhap
            if (!IsLogged())
                return BackToLogin();

            return View();
        }

        [HttpPost]
        public ActionResult Create(TaiKhoanModels model)
        {
            //kiem tra tai khoan dang nhap
            if (!IsLogged())
                return BackToLogin();

            return View();
        }

        [HttpPost]
        public ActionResult Themmoi_capnhat_Account(string act, string mataikhoan, ManageUserViewModel model)
        {
            if (!IsLogged())
                return BackToLogin();
            if (act == "create")
            {
                TaiKhoanModels param = new TaiKhoanModels();
                param.hoten = model.hoten;
                param.tendangnhap = model.tendangnhap;

                if (model.matkhau != null)
                {
                    param.matkhau = EncDec.EncodePassword(model.matkhau);
                }
                param.thudientu = model.thudientu;
                param.maphongban = model.maphongban;
                param.machucdanh = model.machucdanh;
                param.madonvi = 1;
                int nguoitao = int.Parse(Session["userid"].ToString());
                TaiKhoanServices taikhoan = new TaiKhoanServices();
                bool kq = taikhoan.AddUser(param, nguoitao);
            }
            else if (act == "update")
            {

                TaiKhoanModels param = new TaiKhoanModels();
                param.hoten = model.hoten;
                param.tendangnhap = model.tendangnhap;

                if (model.grouptk == "on")
                {
                    param.grouptk = "2";
                }
                else param.grouptk = "";

                if (model.kichhoat == "on")
                {
                    param.kichhoat = "1";
                }
                else param.kichhoat = "";

                if (model.matkhau != null)
                {
                    param.matkhau = EncDec.EncodePassword(model.matkhau);
                }
                param.mataikhoan = mataikhoan;
                param.thudientu = model.thudientu;
                param.maphongban = model.maphongban;
                param.machucdanh = model.machucdanh;
                param.madonvi = 1;
                int nguoihieuchinh = int.Parse(Session["userid"].ToString());
                TaiKhoanServices taikhoan = new TaiKhoanServices();
                bool kq = taikhoan.UpdateUser(param, nguoihieuchinh);

            }
            else if (act == "del")
            {
                //ChucDanhModels param = new ChucDanhModels();
                //FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                //param.machucdanh = int.Parse(AES.DecryptText(mataikhoan, function.ReadXMLGetKeyEncrypt()));

                int nguoihieuchinh = int.Parse(Session["userid"].ToString());
                TaiKhoanServices taikhoan = new TaiKhoanServices();

                bool result = taikhoan.DeletedUser(mataikhoan, nguoihieuchinh);

            }
            return RedirectToAction("Manage", "Account");
        }

    }
}