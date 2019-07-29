using NHG.DataServices;
using NHG.Logger;
using NHG.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Data.Services
{
    public class LichSuHoatDongServices : BaseData
    {
        public LichSuHoatDongServices()
        {
            logger = new Log4Net(typeof(LichSuHoatDongServices));
        }

        #region select
        public List<LichSuHoatDongModels> SelectRows(LichSuHoatDongModels clParam)
        {
            logger.Start("SelectRows");
            List<LichSuHoatDongModels> lstResult = new List<LichSuHoatDongModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_lichsuhoatdong.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichSuHoatDongModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public int CountRows(LichSuHoatDongModels clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("m_lichsuhoatdong.CountRows", param);
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

        public LichSuHoatDongModels RowDetail(LichSuHoatDongModels clparam)
        {
            logger.Start("RowDetail");
            LichSuHoatDongModels vbResult = new LichSuHoatDongModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("m_lichsuhoatdong.RowDetail", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<LichSuHoatDongModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new LichSuHoatDongModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }
        #endregion

        #region insert
        
        public string InsertRow(LichSuHoatDongModels clParam)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                param["seqname"] = "seq_vb_capdouutien";
                string strid = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
                param["macapdouutien"] = strid;
                sqlMap.Insert("m_lichsuhoatdong.InsertRow", param);
                sqlMap.CommitTransaction();
                strResult = param["macapdouutien"].ToString();
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("InsertRow");
            return strResult;
        }
        #endregion

        #region update
        public bool UpdateRow(LichSuHoatDongModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("m_lichsuhoatdong.UpdateRow", param);
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
                sqlMap.Update("m_lichsuhoatdong.UpdateRow_OnCell", param);
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
        public bool DeleteRows(List<LichSuHoatDongModels> clParam)
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
                    sqlMap.Update("m_lichsuhoatdong.DeleteRow", param);
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