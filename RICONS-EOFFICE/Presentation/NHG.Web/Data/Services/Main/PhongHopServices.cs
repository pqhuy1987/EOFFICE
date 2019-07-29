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
    public class PhongHopServices : BaseData
    {
        public PhongHopServices()
        {
            logger = new Log4Net(typeof(PhongHopServices));
        }

        #region select
        public List<PhongHopModels> SelectRows(PhongHopModels clParam)
        {
            logger.Start("SelectRows");
            List<PhongHopModels> lstResult = new List<PhongHopModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_donvi_chucdanh.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<PhongHopModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public int CountRows(PhongHopModels clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("m_donvi_chucdanh.CountRows", param);
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

        public PhongHopModels RowDetail(PhongHopModels clparam)
        {
            logger.Start("RowDetail");
            PhongHopModels vbResult = new PhongHopModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("m_donvi_chucdanh.RowDetail", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<PhongHopModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new PhongHopModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }
        #endregion

        #region insert
        
        public string InsertRow(PhongHopModels clParam)
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
                sqlMap.Insert("m_donvi_chucdanh.InsertRow", param);
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
        public bool UpdateRow(PhongHopModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("m_donvi_chucdanh.UpdateRow", param);
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
                sqlMap.Update("m_donvi_chucdanh.UpdateRow_OnCell", param);
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
        public bool DeleteRows(List<PhongHopModels> clParam)
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
                    sqlMap.Update("m_donvi_chucdanh.DeleteRow", param);
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