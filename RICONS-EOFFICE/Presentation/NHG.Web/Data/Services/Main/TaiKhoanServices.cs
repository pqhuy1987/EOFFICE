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
    public class TaiKhoanServices : BaseData
    {
        #region Fields
        private const string strTableName = "m_taikhoan";
        #endregion

        public TaiKhoanServices()
        {
            //Uncomment the following line if using designed components 
            logger = new Log4Net(typeof(TaiKhoanServices));
        }

        #region user
        /// <summary>
        /// Them moi tai khoan
        /// </summary>
        /// <param name="taiKhoan"></param>
        /// <returns></returns>
        public bool AddUser(TaiKhoanModels taiKhoan,int nguoitao)
        {
            logger.Start("AddUser");
            bool resutl = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, taiKhoan);
                param["nguoitao"] = nguoitao;
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                param["kichhoat"] = CST_Common.CST_ACTIVE;
                sqlMap.Insert("COM_LoginService.AddUser", param);
                sqlMap.CommitTransaction();
                resutl = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("GetUserInfo");
            return resutl;
        }

        public bool Updatemanhansu(string mataikhoan,string manhansu,string machucdanh,string maphongban,string hoten)
        {
            logger.Start("Updatemanhansu");
            bool resutl = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param["mataikhoan"] = mataikhoan;
                param["manhansu"] = manhansu;
                param["machucdanh"] = machucdanh;
                param["maphongban"] = maphongban;
                param["hoten"] = hoten;
                sqlMap.Update("COM_LoginService.Updatemanhansu", param);
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("Updatemanhansu");
            return resutl;
        }






        public bool UpdateUser(TaiKhoanModels taiKhoan, int nguoihieuchinh)
        {
            logger.Start("UpdateUser");
            bool resutl = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, taiKhoan);
                param["nguoihieuchinh"] = nguoihieuchinh;
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                param["kichhoat"] = CST_Common.CST_ACTIVE;
                sqlMap.Update("COM_LoginService.UpdateUser", param);
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("UpdateUser");
            return resutl;
        }

        public bool DeletedUser(string mataikhoan, int nguoihieuchinh)
        {
            logger.Start("UpdateUser");
            bool resutl = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param["nguoihieuchinh"] = nguoihieuchinh;
                param["mataikhoan"] = mataikhoan;
                sqlMap.Update("COM_LoginService.DeletedUser", param);
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("UpdateUser");
            return resutl;
        }





        #region select
        public List<TaiKhoanForCombobox> SelectTaiKhoanForCombobox(M_TaiKhoan clParam)
        {
            logger.Start("SelectTaiKhoanForCombobox");
            List<TaiKhoanForCombobox> lstResult = new List<TaiKhoanForCombobox>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_taikhoan.SelectTaiKhoanForCombobox", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TaiKhoanForCombobox>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectTaiKhoanForCombobox");
            return lstResult;
        }

        public List<TaiKhoanForCombobox> SelectNguoiduyetCombobox()
        {
            logger.Start("SelectNguoiduyetCombobox");
            List<TaiKhoanForCombobox> lstResult = new List<TaiKhoanForCombobox>();
            try
            {
                Hashtable param = new Hashtable();
                IList ilist = sqlMap.ExecuteQueryForList("m_taikhoan.SelectNguoiduyetCombobox", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TaiKhoanForCombobox>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectNguoiduyetCombobox");
            return lstResult;
        }


        public List<TaiKhoanModels> SelectRows(M_TaiKhoan clParam)
        {
            logger.Start("SelectRows");
            List<TaiKhoanModels> lstResult = new List<TaiKhoanModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_taikhoan.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TaiKhoanModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }



        public TaiKhoanModels SelectDetail(M_TaiKhoan clParam)
        {
            logger.Start("SelectDetail");
            TaiKhoanModels result = new TaiKhoanModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_taikhoan.SelectDetail", param);
                CastDataType cast = new CastDataType();
                if(ilist.Count > 0)
                    result = cast.AdvanceCastDataToList<TaiKhoanModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectDetail");
            return result;
        }

        public TaiKhoanModels GetDataEmployee(string thudientu)
        {
            logger.Start("GetDataEmployee");
            List<TaiKhoanModels> lstResult = new List<TaiKhoanModels>();
            try
            {
                if (sqlMap.OpenConnection())
                {
                    Hashtable param = new Hashtable();
                    param["thudientu"] = thudientu;
                    IList ilist = sqlMap.ExecuteQueryForListNotLog("COM_LoginService.GetDataEmployees", param);
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
            logger.End("GetDataEmployee");
            if (lstResult.Count > 0)
                return lstResult[0];
            return null;
        }






        public List<PhongBanDonViModels> SelectPhongBanDonVi(M_TaiKhoan clParam)
        {
            logger.Start("SelectPhongBanDonVi");
            List<PhongBanDonViModels> lstResult = new List<PhongBanDonViModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_taikhoan.SelectPhongBanDonVi", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<PhongBanDonViModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectPhongBanDonVi");
            return lstResult;
        }
        #endregion

        public List<TaiKhoanModels> GetUsers(M_TaiKhoan clParam)
        {
            logger.Start("GetUsers");
            List<TaiKhoanModels> lstResult = new List<TaiKhoanModels>();
            try
            {
                ArrayList arrWhere = new ArrayList();
                arrWhere.Add("mataikhoan");
                arrWhere.Add("madonvi");
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TaiKhoanModels>(base.SelectRows(clParam, strTableName, arrWhere));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("GetUsers");
            return lstResult;
        }
        #endregion

        #region update
        public bool UpdateProfile(M_TaiKhoan param)
        {
            bool result = false;
            try
            {
                ArrayList arrWhere = new ArrayList();
                arrWhere.Add("mataikhoan");
                base.UpdateData(param, strTableName, arrWhere);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                result = false;
            }
            return result;
        }
        
        #endregion
    }
}