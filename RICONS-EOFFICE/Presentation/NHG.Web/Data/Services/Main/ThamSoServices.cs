using NHG.Core.Functions;
using NHG.DataServices;
using NHG.DataServices.Settings;
using NHG.Logger;
using NHG.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Data.Services
{
    public class ThamSoServices : BaseData
    {
        #region Fields
        private const string strTableName = "m_thamso";
        #endregion

        public ThamSoServices()
        {
            logger = new Log4Net(typeof(ChucDanhServices));
        }

        #region select
        public List<ThamSoModels> SelectRows(ThamSoModels clParam)
        {
            logger.Start("SelectRows");
            List<ThamSoModels> lstResult = new List<ThamSoModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_thamso.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<ThamSoModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public int CountRows(ThamSoModels clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("m_thamso.CountRows", param);
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

        public ThamSoModels RowDetail(ThamSoModels clparam)
        {
            logger.Start("RowDetail");
            ThamSoModels vbResult = new ThamSoModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("m_thamso.SelectRows", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<ThamSoModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new ThamSoModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }
        #endregion
        
        #region update
        public bool UpdateRow(ThamSoModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("m_thamso.UpdateRow", param);
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
                sqlMap.Update("m_thamso.UpdateRow_OnCell", param);
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

        public string Test(ThamSoModels param)
        {
            //SequenceValuecs seqValue = base.GetSequenceValue(strTableName, param.nguoitao.ToString());
            //int iID = -1;
            //int.TryParse(seqValue.giatri, out iID);
            //if (iID == -1)
            //    return string.Empty;
            ////tao ma makehoach
            //param.tenthamso = string.Format(Functiontring.ReturnStringFormatID("makehoach"),
            //                                    "NHG",
            //                                    "PB",
            //                                    iID.ToString(seqValue.dodai));
            //base.UpdateData(param, strTableName);
            ////cap nhat laij sequence cho user;
            //base.UpdateSequenceValue(strTableName, param.nguoitao.ToString(), seqValue.giatri);
            ArrayList arrWhere = new ArrayList();
            arrWhere.Add("mathamso");
            base.UpdateData(param, strTableName, arrWhere);
            return "complet";
        }
        #endregion

    }
}