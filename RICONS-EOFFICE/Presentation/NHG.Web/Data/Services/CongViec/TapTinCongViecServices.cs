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
    public class TapTinCongViecServices : BaseData
    {
        public TapTinCongViecServices()
        {
            logger = new Log4Net(typeof(TapTinCongViecServices));
        }

        #region select
        /// <summary>
        /// lay danh sach tap tin trong quan ly cong viec
        /// </summary>
        /// <param name="clParam"></param>
        /// <returns></returns>
        public List<TapTinCongViecModels> SelectRows(TapTinCongViecModels clParam)
        {
            logger.Start("SelectRows");
            List<TapTinCongViecModels> lstResult = new List<TapTinCongViecModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                IList ilist = sqlMap.ExecuteQueryForList("cv_congviec_taptin.SelectRows", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TapTinCongViecModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }
        
        #endregion

        #region insert
        /// <summary>
        /// them moi tap tin dinh kem
        /// </summary>
        /// <param name="clParam"></param>
        /// <returns></returns>
        public bool InsertRow(TapTinCongViecModels clParam)
        {
            logger.Start("InsertRow");
            bool result = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                param["mataptin"] = DateTime.Now.ToLongTimeString().Replace(" ", "").Replace(":", "");
                sqlMap.Insert("cv_congviec_taptin.InsertRow", param);
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

        #region update
        /// <summary>
        /// cap nhat thong tin tap tin dinh kem
        /// </summary>
        /// <param name="clParam"></param>
        /// <returns></returns>
        public bool UpdateRow(TapTinCongViecModels clParam)
        {
            logger.Start("UpdateRow");
            bool bResult = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                sqlMap.Update("cv_congviec_taptin.UpdateRow", param);
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

        #endregion

        #region Delete
        /// <summary>
        /// Xoa tap tin dinh kem
        /// </summary>
        /// <param name="clParam"></param>
        /// <returns></returns>
        public bool DeleteRows(List<TapTinCongViecModels> clParam)
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
                    sqlMap.Update("cv_congviec_taptin.DeleteRow", param);
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