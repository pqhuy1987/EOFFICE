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
    public class LichSuCongViecServices : BaseData
    {
        public LichSuCongViecServices()
        {
            logger = new Log4Net(typeof(LichSuCongViecServices));
        }

        #region select
        /// <summary>
        /// Lay danh sach lich su cong viec
        /// </summary>
        /// <param name="clParam"></param>
        /// <returns></returns>
        public List<CongViecLichSuModels> SelectRows(CongViecLichSuModels clParam)
        {
            logger.Start("SelectRows");
            List<CongViecLichSuModels> lstResult = new List<CongViecLichSuModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_congviec_lichsu.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<CongViecLichSuModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        /// <summary>
        /// Dem so luong danh sach cong viec
        /// </summary>
        /// <param name="clparam"></param>
        /// <returns></returns>
        public int CountRows(CongViecLichSuModels clparam)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);
                iResult = (int)sqlMap.ExecuteQueryForObject("cv_congviec_lichsu.CountRows", param);
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

        #endregion

        #region insert

        /// <summary>
        /// Them moi lich su
        /// </summary>
        /// <param name="clParam"></param>
        /// <returns></returns>
        public bool InsertRow(CongViecLichSuModels clParam)
        {
            logger.Start("InsertRow");
            bool result = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                sqlMap.Insert("cv_congviec_lichsu.InsertRow", param);
                sqlMap.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                result = false;
                logger.Error(ex.Message);
            }
            logger.End("InsertRow");
            return result;
        }
        #endregion

    }
}