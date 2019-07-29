using NHG.Core.Constants;
using NHG.Core.Functions;
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
    public class LichLamViecServices : BaseData
    {
        public LichLamViecServices()
        {
            logger = new Log4Net(typeof(LichLamViecServices));
        }

        #region select
        public List<LichLamViecModels> SelectRows(LichLamViecModels clParam)
        {
            logger.Start("SelectRows");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_lichlamviec.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public int CountRows(LichLamViecModels clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("cv_lichlamviec.CountRows", param);
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

        public LichLamViecModels RowDetail(LichLamViecModels clparam)
        {
            logger.Start("RowDetail");
            LichLamViecModels vbResult = new LichLamViecModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_lichlamviec.RowDetail", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<LichLamViecModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new LichLamViecModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }

        /// <summary>
        /// dùng để xem chi tiết 1 sự kiện
        /// </summary>
        /// <param name="id">mã sự kiện</param>
        /// <returns></returns>

        public List<LichLamViecModels> selectEventForIdEvent(int id)
        {
            logger.Start("selectEventForIdEvent");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable parram = new Hashtable();
                parram["maghichu"] = id;
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                IList iList = sqlMap.ExecuteQueryForList("cv_lichlamviec.loadSuKienTheoIdEvent", parram);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("selectEventForIdEvent");
            return lstResult;
        }

        public List<LichLamViecModels> selectEventForIdEvent(LichLamViecModels clParam)
        {
            logger.Start("selectEventForIdEvent");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable param = base.SetDataToHashtable(false, clParam);
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                IList iList = sqlMap.ExecuteQueryForList("cv_lichlamviec.loadSuKien", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("selectEventForIdEvent");
            return lstResult;
        }
        #endregion

        #region insert
        
        public string InsertRow(LichLamViecModels clParam)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                param["seqname"] = "seq_cal_lichlamviec";
                string strid = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
                int iID = -1;
                int.TryParse(strid, out iID);
                if (iID == -1)
                    return string.Empty;
                //tao ma cong viec
                param["malichlamviec"] = string.Format(Functiontring.ReturnStringFormatID("malichlamviec"),
                                                    clParam.maphongban,
                                                    DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    iID.ToString("0000"));
                sqlMap.Insert("cv_lichlamviec.InsertRow", param);
                sqlMap.CommitTransaction();
                strResult = param["malichlamviec"].ToString();
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
        public bool UpdateRow(LichLamViecModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("cv_lichlamviec.UpdateRow", param);
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
                sqlMap.Update("cv_lichlamviec.UpdateRow_OnCell", param);
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
        public bool DeleteRows(List<LichLamViecModels> clParam)
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
                    sqlMap.Update("cv_lichlamviec.DeleteRow", param);
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