using NHG.Core.Functions;
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
    public class ThongBaoServices : BaseData
    {
        public ThongBaoServices()
        {
            logger = new Log4Net(typeof(ThongBaoServices));
        }

        #region select
        public List<ThongBaoModels> SelectRows(M_ThongBao clParam)
        {
            logger.Start("SelectRows");
            List<ThongBaoModels> lstResult = new List<ThongBaoModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_thongbao.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<ThongBaoModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public List<ThongBaoModels> SelectQuickAlerts(ThongBaoModels clParam)
        {
            logger.Start("SelectQuickAlerts");
            List<ThongBaoModels> lstResult = new List<ThongBaoModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("m_thongbao.SelectQuickAlerts", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<ThongBaoModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectQuickAlerts");
            return lstResult;
        }

        public int CountRows(M_ThongBao clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("m_thongbao.CountRows", param);
            }
            catch (Exception ex)
            {
                iResult = 0;
                logger.Error(ex.Message);
            }
            logger.End("CountRows");
            return iResult;
        }

        public ThongBaoModels RowDetail(M_ThongBao clparam)
        {
            logger.Start("RowDetail");
            ThongBaoModels vbResult = new ThongBaoModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("m_thongbao.RowDetail", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<ThongBaoModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new ThongBaoModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }
        #endregion

        #region insert

        public string InsertRow(M_ThongBao clParam)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                param["seqname"] = "seq_m_thongbao";
                string strid = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
                int iID = -1;
                int.TryParse(strid, out iID);
                if (iID == -1)
                    return string.Empty;
                //tao ma cong viec
                param["mathongbao"] = string.Format(Functiontring.ReturnStringFormatID("mathongbao"),
                                                    clParam.manhansu,
                                                    DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    iID.ToString("000000"));
                sqlMap.Insert("m_thongbao.InsertRow", param);
                sqlMap.CommitTransaction();
                strResult = param["mathongbao"].ToString();
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
        public bool UpdateRow(ThongBaoModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("m_thongbao.UpdateRow", param);
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
                sqlMap.Update("m_thongbao.UpdateRow_OnCell", param);
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
        public bool DeleteRows(List<ThongBaoModels> clParam)
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
                    sqlMap.Update("m_thongbao.DeleteRow", param);
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