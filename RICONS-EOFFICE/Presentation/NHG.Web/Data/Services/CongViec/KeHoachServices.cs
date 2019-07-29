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
    public class KeHoachServices : BaseData
    {
        #region Fields
        private const string strTableName = "cv_kehoach";
        #endregion

        public KeHoachServices()
        {
            logger = new Log4Net(typeof(KeHoachServices));
        }

        #region select
        public List<KeHoachModels> SelectRows(M_KeHoach clParam)
        {
            logger.Start("SelectRows");
            List<KeHoachModels> lstResult = new List<KeHoachModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_kehoach.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<KeHoachModels>(ilist);
                for (int i = 0; i < lstResult.Count; i++)
                {
                    for (int j = lstResult[i].thangbatdau; j <= lstResult[i].thangbatdau + lstResult[i].sothangthuchien; j++)
                    {
                        if (j == 1) lstResult[i].t1 = "x";
                        else if (j == 2) lstResult[i].t2 = "x";
                        else if (j == 3) lstResult[i].t3 = "x";
                        else if (j == 4) lstResult[i].t4 = "x";
                        else if (j == 5) lstResult[i].t5 = "x";
                        else if (j == 6) lstResult[i].t6 = "x";
                        else if (j == 7) lstResult[i].t7 = "x";
                        else if (j == 8) lstResult[i].t8 = "x";
                        else if (j == 9) lstResult[i].t9 = "x";
                        else if (j == 10) lstResult[i].t10 = "x";
                        else if (j == 11) lstResult[i].t11 = "x";
                        else if (j == 12) lstResult[i].t12 = "x";
                        else if (j == 13) lstResult[i].t1 = "x";
                        else if (j == 14) lstResult[i].t2 = "x";
                        else if (j == 15) lstResult[i].t3 = "x";
                        else if (j == 16) lstResult[i].t4 = "x";
                        else if (j == 17) lstResult[i].t5 = "x";
                        else if (j == 18) lstResult[i].t6 = "x";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public int CountRows(M_KeHoach clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("cv_kehoach.CountRows", param);
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

        public List<KeHoachForCombobox> SelectKeHoachForCombobox(KeHoachModels clParam)
        {
            logger.Start("SelectKeHoachForCombobox");
            List<KeHoachForCombobox> lstResult = new List<KeHoachForCombobox>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_kehoach.SelectKeHoachForCombobox", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<KeHoachForCombobox>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectKeHoachForCombobox");
            return lstResult;
        }


        public KeHoachModels RowDetail(KeHoachModels clparam)
        {
            logger.Start("RowDetail");
            KeHoachModels vbResult = new KeHoachModels();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_kehoach.RowDetail", param);
                CastDataType cast = new CastDataType();
                vbResult = cast.AdvanceCastDataToList<KeHoachModels>(ilist)[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                vbResult = new KeHoachModels();
            }
            logger.End("RowDetail");
            return vbResult;
        }
        #endregion

        #region insert
        
        public string InsertRow(M_KeHoach clParam, string maNhanSu)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                Hashtable param = new Hashtable();
                param["seqname"] = "seq_cv_kehoach";
                string strid = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
                int iID = -1;
                int.TryParse(strid, out iID);
                if (iID == -1)
                    return string.Empty;
                //tao ma makehoach
                param["makehoach"] = string.Format(Functiontring.ReturnStringFormatID("makehoach"),
                                                    maNhanSu,
                                                    DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    iID.ToString("00000000"));
                clParam.makehoach = param["makehoach"].ToString();
                base.InsertData(clParam, strTableName);

                strResult = param["makehoach"].ToString();
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("InsertRow");
            return strResult;
        }
        #endregion

        #region update
        public string UpdateRow(M_KeHoach clParam)
        {
            logger.Start("UpdateRow");
            string bResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                sqlMap.Update("cv_kehoach.UpdateRow", param);
                sqlMap.CommitTransaction();
                bResult = clParam.makehoach;
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                bResult = "-1";
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
                sqlMap.Update("cv_kehoach.UpdateRow_OnCell", param);
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
        public bool DeleteRows(List<KeHoachModels> clParam)
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
                    sqlMap.Update("cv_kehoach.DeleteRow", param);
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