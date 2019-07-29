using NHG.Core.ClassData;
using NHG.Core.Constants;
using NHG.DataServices;
using NHG.DataServices.DataClass;
using NHG.Logger;
using NHG.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Data.Services
{
    public class LoginServices : BaseData
    {
        public LoginServices()
        {
            //Uncomment the following line if using designed components 
            logger = new Log4Net(typeof(LoginServices));
        }

        /// <summary>
        /// Kiem tra neu nguoi dung su dung tai khoan AD 
        /// </summary>
        /// <param name="clParam"></param>
        /// <returns>
        /// - true : neu tai khoan thuoc AD
        /// - false: neu tai khoan khong thuoc AD
        /// </returns>
        public bool IsUserAD(M_TaiKhoan clParam)
        {
            logger.Start("IsUserAD");
            bool result = false;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                IList ilist = sqlMap.ExecuteQueryForListNotLog("COM_LoginService.GetUserAD", param);
                //chuyen doi du tu Ilist sang custom class
                CastDataType cast = new CastDataType();
                List<TaiKhoanModels> lstResult = cast.AdvanceCastDataToList<TaiKhoanModels>(ilist);
                if (lstResult.Count > 0)
                {
                    if (lstResult[0].is_ada == "1")
                        result = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.End("IsUserAD");
            return result;
        }

        /// <summary>
        /// Gets the login data.
        /// </summary>
        /// <param name="tenDangNhap">The login_id.</param>
        /// <param name="matKhau">The password.</param>
        /// <returns>DataSet containts Detail Login info, Return null if error</returns>
        public TaiKhoanModels GetLoginData(M_TaiKhoan clParam)
        {
            logger.Start("GetLoginData");
            List<TaiKhoanModels> lstResult = new List<TaiKhoanModels>();
            try
            {
                if (sqlMap.OpenConnection())
                {
                    Hashtable param = new Hashtable();
                    param = base.SetDataToHashtable(false, clParam);
                    IList ilist = sqlMap.ExecuteQueryForListNotLog("COM_LoginService.GetLogin", param);
                    CastDataType cast = new CastDataType();
                    lstResult = cast.AdvanceCastDataToList<TaiKhoanModels>(ilist);
                }
                else
                {
                    logger.Error(new Exception("DisConnectDB:ERR-134"));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            sqlMap.CloseConnection();
            logger.End("GetLoginData");
            if (lstResult.Count > 0)
                return lstResult[0];
            return null;
        }

        public TaiKhoanModels GetUserInfo(TaiKhoanModels clParam)
        {
            logger.Start("GetUserInfo");
            List<TaiKhoanModels> lstResult = new List<TaiKhoanModels>();
            try
            {
                    Hashtable param = new Hashtable();
                    param = base.SetDataToHashtable(false, clParam);
                    IList ilist = sqlMap.ExecuteQueryForListNotLog("COM_LoginService.GetLogin", param);
                    //chuyen doi du tu Ilist sang custom class
                    CastDataType cast = new CastDataType();
                    lstResult = cast.AdvanceCastDataToList<TaiKhoanModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.End("GetUserInfo");
            if (lstResult.Count > 0)
                return lstResult[0];
            return null;
        }

        #region user
        /// <summary>
        /// Them moi tai khoan
        /// </summary>
        /// <param name="taiKhoan"></param>
        /// <returns></returns>
        public bool AddUser(TaiKhoanModels taiKhoan)
        {
            logger.Start("AddUser");
            bool resutl = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, taiKhoan);
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                param["kichhoat"] = CST_Common.CST_ACTIVE;
                param["seqname"] = "seq_m_taikhoan";
                string strid = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
                param["mataikhoan"] = strid;
                sqlMap.Insert("COM_LoginService.AddUser", param);
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("GetUserInfo");
            return resutl;
        }

        public List<TaiKhoanModels> GetUsers()
        {
            logger.Start("GetUsers");
            List<TaiKhoanModels> lstResult = new List<TaiKhoanModels>();
            try
            {
                Hashtable param = new Hashtable();
                IList iList = null;
                iList = sqlMap.ExecuteQueryForListNotLog("COM_LoginService.GetUsers", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TaiKhoanModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("GetUsers");
            return lstResult;
        }

        /// <summary>
        /// lay danh sach phan quyen
        /// </summary>
        /// <returns></returns>
        public List<MenuRole> GetRoles(TaiKhoanModels clParam)
        {
            logger.Start("GetRoles");
            List<MenuRole> lstResult = new List<MenuRole>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList iList = null;
                iList = sqlMap.ExecuteQueryForListNotLog("COM_LoginService.GetRoles", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<MenuRole>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("GetRoles");
            return lstResult;
        }
        #endregion

        #region log out
        public bool CheckExistUserInfoHistory(string login_id, string sessiongid)
        {
            logger.Start("CheckExistUserInfoHistory");
            bool bResult = false;
            try
            {
                Hashtable param = new Hashtable();
                param["sessionid"] = sessiongid;
                param["tendangnhap"] = login_id;
                if ((int)sqlMap.ExecuteQueryForObject("COM_LoginService.CheckExistUserInfoHistory", param) > 0)
                    bResult = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("CheckExistUserInfoHistory");
            return bResult;
        }

        public int GetUserInfoHistory(string login_id, string sessiongid)
        {
            logger.Start("GetUserInfoHistory");
            int result = 0;
            try
            {
                Hashtable param = new Hashtable();
                param["sessionid"] = sessiongid;
                param["tendangnhap"] = login_id;
                Object objResult = sqlMap.ExecuteQueryForObject("COM_LoginService.GetUserInfoHistory", param);
                if (objResult != null)
                    result = (int)objResult;
            }
            catch (Exception ex)
            {
                logger.Error(ex);

            }
            logger.End("GetUserInfoHistory");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login_id"></param>
        /// <param name="sessiongid"></param>
        /// <param name="ipaddress"></param>
        /// <param name="macaddress"></param>
        /// <param name="solandangnhap">So lan dang nhap that bai</param>
        /// <param name="ketquadangnhap"></param>
        /// <param name="isOnline"></param>
        public void InsertHistoryLogin(string login_id,
                                        string sessiongid,
                                        string ipaddress,
                                        string browser,
                                        string useragent,
                                        string solandangnhap,
                                        string ketquadangnhap,
                                        string isOnline)
        {
            logger.Start("InsertHistoryLogin");
            try
            {
                Hashtable param = new Hashtable();
                param["sessionid"] = sessiongid;
                param["tendangnhap"] = login_id;
                param["ipaddress"] = ipaddress;
                param["browser"] = browser;
                param["useragent"] = useragent;
                param["dangnhapthatbai"] = solandangnhap;
                param["ketqua_dangnhap"] = ketquadangnhap;
                param["isonline"] = isOnline;
                sqlMap.Insert("COM_LoginService.HistoryLogin", param);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw (ex);
            }
            logger.End("InsertHistoryLogin");
        }

        public void UpdateHistoryLogin(string login_id, string sessiongid, string solandangnhap, string ketquadangnhap, string isOnline)
        {
            logger.Start("UpdateHistoryLogin");
            try
            {
                Hashtable param = new Hashtable();
                param["sessionid"] = sessiongid;
                param["tendangnhap"] = login_id;
                param["dangnhapthatbai"] = solandangnhap;
                param["ketqua_dangnhap"] = ketquadangnhap;
                param["thoigian_dangxuat"] = "null";
                param["isonline"] = isOnline;
                sqlMap.Update("COM_LoginService.UpdateHistoryLogin", param);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw (ex);
            }
            logger.End("UpdateHistoryLogin");
        }

        public void LogoutHistory(string login_id, string sessiongid)
        {
            logger.Start("LogoutHistory");
            try
            {
                Hashtable param = new Hashtable();
                param["sessionid"] = sessiongid;
                param["tendangnhap"] = login_id;
                param["thoigian_dangxuat"] = "GETDATE()";
                param["isonline"] = "0";

                sqlMap.BeginTransaction();
                sqlMap.Update("COM_LoginService.UpdateHistoryLogin", param);
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw (ex);
            }
            logger.End("LogoutHistory");
        }
        #endregion

    }
}