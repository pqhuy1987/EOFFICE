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
    public class ViTriServices : BaseData
    {
        public ViTriServices()
        {
            logger = new Log4Net(typeof(ViTriServices));
        }

        #region select
        public List<ViTriModels> SelectRows(ViTriModels clParam)
        {
            logger.Start("SelectRows");
            List<ViTriModels> lstResult = new List<ViTriModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_vitri.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<ViTriModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public int CountRows(ViTriModels clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("m_vitri.CountRows", param);
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

        public ViTriModels RowDetail(ViTriModels clparam)
        {
            logger.Start("RowDetail");
            ViTriModels vbResult = new ViTriModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("m_vitri.RowDetail", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<ViTriModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new ViTriModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }
        #endregion

        #region insert
        
        public string InsertRow(ViTriModels clParam)
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
                sqlMap.Insert("m_vitri.InsertRow", param);
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
        public bool UpdateRow(ViTriModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("m_vitri.UpdateRow", param);
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
                sqlMap.Update("m_vitri.UpdateRow_OnCell", param);
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
        public bool DeleteRows(List<ViTriModels> clParam)
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
                    sqlMap.Update("m_vitri.DeleteRow", param);
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