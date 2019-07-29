using NHG.Core.Functions;
using NHG.DataServices;
using NHG.DataServices.DataClass;
using NHG.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Data.Services 
{
    public class MuctieucongviecServices:BaseData
    {

        #region insert
        public string InsertRow(MucTieuCongViecModels clParam, string maNhanSu)
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
                base.InsertData(clParam, "cv_kehoach");

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

        public List<MucTieuCongViecModels> SelectRows()
        {
            logger.Start("SelectRows");
            List<MucTieuCongViecModels> lstResult = new List<MucTieuCongViecModels>();
            try
            {
                Hashtable param = new Hashtable();
                IList ilist = sqlMap.ExecuteQueryForList("cv_muctieucongviec.SelectRows10", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<MucTieuCongViecModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }
    }
}