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
    public class AbsentServices : BaseData
    {
        #region khai bao ban dau
        private const string strTableName = "m_donvi_phongban";
        #endregion

        public AbsentServices()
        {
            logger = new Log4Net(typeof(DanhmucServices));
        }

        public int CountRows(AbsentModels clparam, string nghitu,string nghiden)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);

                if (param["manghiphep"].ToString() == "")
                    param["manghiphep"] = "";

                if (param["maphongban"].ToString() == "0")
                    param["maphongban"] = "";

                if (param["nguoitao"].ToString() == "0" || param["nguoitao"].ToString() == "1")
                    param["nguoitao"] = "";

                param["nghitu"] = nghitu;
                param["nghiden"] = nghiden;

                iResult = (int)sqlMap.ExecuteQueryForObject("Dangkynghiphep.CountRows", param);
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

        public List<AbsentModels> SelectRows_Danhsachnghiphep(AbsentModels clParam, int trangbd, int trangkt,string nghitu,string nghiden)
        {
            logger.Start("SelectRows");
            List<AbsentModels> lstResult = new List<AbsentModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                if (param["manghiphep"].ToString() == "")
                    param["manghiphep"] = "";

                if (param["maphongban"].ToString() == "0")
                    param["maphongban"] = "";

                if (param["nguoitao"].ToString() == "0" || param["nguoitao"].ToString() == "1")
                    param["nguoitao"] = "";
               
                param["nghitu"] = nghitu;
                param["nghiden"] = nghiden;

                param["trangbd"] = trangbd;
                param["trangkt"] = trangkt;


                IList ilist = sqlMap.ExecuteQueryForList("Dangkynghiphep.SelectRows_Danhsachnghiphep", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<AbsentModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public List<AbsentModels> SelectRows_Danhsachnghiphep_11111(AbsentModels clParam)
        {
            logger.Start("SelectRows");
            List<AbsentModels> lstResult = new List<AbsentModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);
                if (param["manghiphep"].ToString() == "")
                    param["manghiphep"] = "";

                if (param["nguoitao"].ToString() == "0")
                    param["nguoitao"] = "";

                IList ilist = sqlMap.ExecuteQueryForList("Dangkynghiphep.SelectRows_Danhsachnghiphep_11111", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<AbsentModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public List<AbsentModels> SelectRows_Danhsachnghiphep_songayphep(AbsentModels clParam)
        {
            logger.Start("SelectRows");
            List<AbsentModels> lstResult = new List<AbsentModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                if (param["nguoitao"].ToString() == "0")
                    param["nguoitao"] = "";

                param["nghitu"] = "21/12/" + (DateTime.Now.Year - 1).ToString();
                param["nghiden"] = "31/12/" + DateTime.Now.Year.ToString();

                IList ilist = sqlMap.ExecuteQueryForList("Dangkynghiphep.SelectRows_Danhsachnghiphep_songayphep", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<AbsentModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public string Insert_Donxinnghiphep(AbsentModels clParam)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                if (clParam.manghiphep.ToString().Trim() == "")
                {
                    Hashtable param = new Hashtable();
                    param = base.SetDataToHashtable(false, clParam);
                    strResult = GetSequence_All("dm_seq", "Dondangkynghiphep");
                    clParam.manghiphep = strResult;
                    bool kq= base.InsertData(clParam, "Dondangkynghiphep");
                    if (kq == false) strResult = "-1";
                    else strResult = clParam.manghiphep;
                }
                else
                {
                    Hashtable param = new Hashtable();
                    param = base.SetDataToHashtable(false, clParam);
                    int iResult = (int)sqlMap.ExecuteQueryForObject("Dangkynghiphep.CountRows_hieuchinh", param);
                    param["hieuchinh"] = iResult + 1;
                    sqlMap.Update("Dangkynghiphep.UpdateRow_Dondangkynghiphep", param);
                    strResult = (iResult + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("InsertRow");
            return strResult;
        }

        public string UpdateRow_capnhathotennguoiduyet(string manghiphep, string hotenduyetcap1, string hotenduyetcap2)
        {
            logger.Start("Insert_Donxinnghiphep");
            string strResult = "1";
            try
            {
                Hashtable param = new Hashtable();
                param["manghiphep"] = manghiphep;

                if (hotenduyetcap1.Trim() == "")
                    param["duyetcap1"] = "9";
                else param["duyetcap1"] = "0";

                if (hotenduyetcap2.Trim() == "")
                    param["duyetcap2"] = "9";
                else param["duyetcap2"] = "0";

                param["duyetcap1_ghichu"] = hotenduyetcap1;
                param["duyetcap2_ghichu"] = hotenduyetcap2;
                sqlMap.Update("Dangkynghiphep.UpdateRow_capnhathotennguoiduyet", param);
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("Insert_Donxinnghiphep");
            return strResult;
        }




        public string UpdateRow_Duyetnghiphep1(string manghiphep, string daduyet, string hieuchinh)
        {
            logger.Start("Insert_Donxinnghiphep");
            string strResult="1";
            try
            {
                Hashtable param = new Hashtable();
                param["manghiphep"] = manghiphep;
                param["duyetcap1"] = daduyet;
                param["hieuchinh"] = hieuchinh;
                int iResult = 0;
                if(daduyet=="2")
                    iResult = (int)sqlMap.ExecuteQueryForObject("Dangkynghiphep.CountRows_duyetdadongy1", param);
                if (iResult == 1)
                    strResult = "2";
                else
                {
                    iResult = (int)sqlMap.ExecuteQueryForObject("Dangkynghiphep.CountRows_hieuchinh", param);
                    if (hieuchinh == iResult.ToString())
                        sqlMap.Update("Dangkynghiphep.UpdateRow_Duyetnghiphep1", param);
                    else strResult = "3";
                }
                   
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("Insert_Donxinnghiphep");
            return strResult;
        }

        public string UpdateRow_Duyetnghiphep2(string manghiphep, string daduyet, string hieuchinh)
        {
            logger.Start("Insert_Donxinnghiphep");
            string strResult = "1";
            try
            {
                Hashtable param = new Hashtable();
                param["manghiphep"] = manghiphep;
                param["duyetcap2"] = daduyet;
                param["hieuchinh"] = hieuchinh;
                int iResult = 0;
                if (daduyet == "2")
                    iResult = (int)sqlMap.ExecuteQueryForObject("Dangkynghiphep.CountRows_duyetdadongy2", param);
                if (iResult == 1)
                    strResult = "2";
                else
                    sqlMap.Update("Dangkynghiphep.UpdateRow_Duyetnghiphep2", param);
                    
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("Insert_Donxinnghiphep");
            return strResult;
        }

        public bool DeletedRow_Dondangkynghiphep(string manghiphep,string userid)
        {
            logger.Start("Insert_Donxinnghiphep");
            bool strResult = true;
            try
            {
                Hashtable param = new Hashtable();
                param["manghiphep"] = manghiphep;
                param["nguoitao"] = userid;
                sqlMap.Update("Dangkynghiphep.DeletedRow_Dondangkynghiphep", param);
            }
            catch (Exception ex)
            {
                strResult = false;
                logger.Error(ex.Message);
            }
            logger.End("Insert_Donxinnghiphep");
            return strResult;
        }

        


       

    }
}