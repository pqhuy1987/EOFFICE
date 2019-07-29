using NHG.DataServices;
using NHG.Logger;
using NHG.Web.Models;
using NHG.Core.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Data.Services
{
    public class CongViecServices :BaseData
    {
        public CongViecServices()
        {
            logger = new Log4Net(typeof(LoginServices));
        }

        #region select
        /// <summary>
        /// Lay danh sach cong viec
        /// </summary>
        /// <param name="clParam">CongViecModels</param>
        /// <returns></returns>
        public List<CongViecModels> SelectRows(CongViecModels clParam)
        {
            logger.Start("SelectRows");
            List<CongViecModels> lstResult = new List<CongViecModels>();
            try
            {
                //chuyen doi models cong viec sang param
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_congviec.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<CongViecModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        /// <summary>
        /// Dem so luong cong viec
        /// </summary>
        /// <param name="clparam">CongViecModels</param>
        /// <returns></returns>
        public int CountRows(CongViecModels clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                //chuyen doi models cong viec sang param
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("cv_congviec.CountRows", param);
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                iResult = 0;
                logger.Error(ex.Message);
            }
            logger.End("CountRows");
            return iResult;
        }

        /// <summary>
        /// Lay chi tiet cong viec
        /// </summary>
        /// <param name="clparam"></param>
        /// <returns></returns>
        public CongViecModels RowDetail(CongViecModels clparam)
        {
            logger.Start("RowDetail");
            CongViecModels vbResult = new CongViecModels();
            try
            {
                //chuyen doi models cong viec sang param
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_congviec.RowDetail", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<CongViecModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new CongViecModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }
        #endregion

        #region insert
        
        /// <summary>
        /// Them cong viec
        /// </summary>
        /// <param name="clParam">CongViecModels</param>
        /// <returns></returns>
        public string InsertRow(CongViecModels clParam)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                //chuyen doi models cong viec sang param
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                //lay gia tri seqnam m_congviec
                param["seqname"] = "seq_m_congviec";
                string strid = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
                int iID = -1;
                int.TryParse(strid, out iID);
                if (iID == -1)
                    return string.Empty;
                //tao ma cong viec
                param["macongviec"] = string.Format(Functiontring.ReturnStringFormatID("macongviec"), 
                                                    clParam.tenviettat,
                                                    DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    iID.ToString("0000000000"));
                sqlMap.Insert("cv_congviec.InsertRow", param);
                sqlMap.CommitTransaction();
                //return macong
                strResult = param["macongviec"].ToString();
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                strResult = string.Empty;
                logger.Error(ex.Message);
            }
            logger.End("InsertRow");
            return strResult;
        }
        #endregion

        #region update

        /// <summary>
        /// Cap nhat cong viec
        /// </summary>
        /// <param name="clParam">CongViecModels</param>
        /// <returns></returns>
        public bool UpdateRow(CongViecModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("cv_congviec.UpdateRow", param);
                sqlMap.CommitTransaction();
                bResult = true;
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                bResult = false;
                logger.Error(ex.Message);
            }
            logger.End("UpdateRow");
            return bResult;
        }

        /// <summary>
        /// Cap nhat cong viec
        /// </summary>
        /// <param name="idRow">Ma cong viec</param>
        /// <param name="strColumn">Ten Cot</param>
        /// <param name="strValue">Gia tri</param>
        /// <param name="strNguoiHieuChinh">Nguoi hieu chinh</param>
        /// <returns></returns>
        public bool UpdateRow(string idRow, string strColumn, string strValue, string strNguoiHieuChinh)
        {
            logger.Start("UpdateRow_OnCell");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param["macapdouutien"] = idRow;
                param["column"] = strColumn;
                param["value"] = strValue;
                param["nguoihieuchinh"] = strNguoiHieuChinh;
                sqlMap.Update("cv_congviec.UpdateRow_OnCell", param);
                sqlMap.CommitTransaction();
                bResult = true;
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                bResult = false;
                logger.Error(ex.Message);
            }
            logger.End("UpdateRow_OnCell");
            return bResult;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Xoa cong viec
        /// </summary>
        /// <param name="clParam">CongViecModels</param>
        /// <returns></returns>
        public bool DeleteRows(List<CongViecModels> clParam)
        {
            logger.Start("DeleteRows");
            bool lstResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                for (int i = 0; i < clParam.Count; i++)
                {
                    param = new Hashtable();
                    param = base.SetDataToHashtable(false, clParam[i]);
                    sqlMap.Update("cv_congviec.DeleteRow", param);
                }
                sqlMap.CommitTransaction();
                lstResult = true;
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                logger.Error(ex.Message);
            }
            logger.End("DeleteRows");
            return lstResult;
        }
        #endregion
    }
}