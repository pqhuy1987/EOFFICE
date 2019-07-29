using Newtonsoft.Json.Linq;
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
    public class TimekeepingServices : BaseData
    {
        #region khai bao ban dau
        private const string strTableName = "Timekeeping";
        #endregion

        public TimekeepingServices()
        {
            logger = new Log4Net(typeof(TimekeepingServices));
        }

        public int CountRows(TimekeepingModels clparam, string ktchamcong, string thangnam, string manhanvien, string hovaten, string trangthaiduyet)
        {
            logger.Start("CountRows");
            int iResult = 0;
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clparam);

                if (trangthaiduyet.Trim() == "" || trangthaiduyet.Trim() == null || trangthaiduyet.Trim() == "undefined")
                    param["trangthaiduyet"] = "";
                else param["trangthaiduyet"] = trangthaiduyet;

                param["thangnam"] = thangnam;
                if (param["maphongban"].ToString() == "0")
                    param["maphongban"] = "";
                if (manhanvien.Trim() == "" || manhanvien.Trim() == null || manhanvien.Trim() == "undefined")
                    param["manhanvien"] = "";
                else param["manhanvien"] = manhanvien;
                if (hovaten.ToString().Trim() == "" || hovaten.Trim() == null || hovaten.Trim() == "undefined")
                    param["hovaten"] = "";
                else
                {
                    param["hovaten"] = hovaten;
                    if (ktchamcong == "0")
                        param["maphongban"] = "";
                    param["nguoitao"] = "";
                } 
                if (ktchamcong=="0")
                    iResult = (int)sqlMap.ExecuteQueryForObject("Timekeeping.CountRows", param);
                else
                    iResult = (int)sqlMap.ExecuteQueryForObject("Timekeeping.CountRows_ktchamcong", param);
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


        public List<TimekeepingModels> SelectRows(TimekeepingModels clParam, int trangbd, int trangkt, string thang, string manhanvien, string hovaten, string kiemtrachamcong, string trangthaiduyet)
        {
            logger.Start("SelectRows");
            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                if (param["maphongban"].ToString() == "0")
                    param["maphongban"] = "";

                if (param["nguoitao"].ToString() == "0" || param["nguoitao"].ToString() == "1")
                    param["nguoitao"] = "";

                if (manhanvien.Trim() == "" || manhanvien.Trim() == null || manhanvien.Trim() == "undefined")
                    param["manhanvien"] = "";
                else param["manhanvien"] = manhanvien;

                if (hovaten.ToString().Trim() == "" || hovaten.Trim() == null || hovaten.Trim() == "undefined")
                    param["hovaten"] = "";
                else
                {
                    param["hovaten"] = hovaten;
                    if (kiemtrachamcong=="0")
                        param["maphongban"] = "";
                    param["nguoitao"] = "";
                }

                if (trangthaiduyet.Trim() == "" || trangthaiduyet.Trim() == null || trangthaiduyet.Trim() == "undefined")
                    param["trangthaiduyet"] = "";
                else param["trangthaiduyet"] = trangthaiduyet;

                param["thang"] = thang;
                param["trangbd"] = trangbd;
                param["trangkt"] = trangkt;

                if (kiemtrachamcong == "0")
                {
                    IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows", param);
                    CastDataType cast = new CastDataType();
                    lstResult = cast.AdvanceCastDataToList<TimekeepingModels>(ilist);
                }
                else
                {
                    IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_manhanvien_thang_lllll", param);
                    CastDataType cast = new CastDataType();
                    lstResult = cast.AdvanceCastDataToList<TimekeepingModels>(ilist);
                }
                
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }


        public List<TimekeepingModels> SelectRows_xuatexcel(TimekeepingModels clParam, string thang)
        {
            logger.Start("SelectRows");
            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            try
            {
                Hashtable param = new Hashtable();
                param = base.SetDataToHashtable(false, clParam);

                if (param["maphongban"].ToString() == "0")
                    param["maphongban"] = "";
                param["thang"] = thang;

                IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_manhanvien_thang_xuatexcel", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TimekeepingModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }


        public List<AbsentModels> SelectRows_Danhsachnghiphep(string manhanvien, string tungay, string denngay)
        {
            logger.Start("SelectRows");
            List<AbsentModels> lstResult = new List<AbsentModels>();
            try
            {
                Hashtable param = new Hashtable();
                param["manhanvien"] = manhanvien;
                param["tungay"] = tungay;
                param["denngay"] = denngay;
                IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_Danhsachnghiphep", param);
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

        public List<AtHolidayModels> SelectRows_Danhsachngayle(string tungay, string denngay)
        {
            logger.Start("SelectRows_Danhsachngayle");
            List<AtHolidayModels> lstResult = new List<AtHolidayModels>();
            try
            {
                Hashtable param = new Hashtable();
                param["tungay"] = tungay;
                param["denngay"] = denngay;

                IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_Danhsachngayle", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<AtHolidayModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows_Danhsachngayle");
            return lstResult;
        }

        public string Insert_phongban(PhongBanModels clParam)
        {
            logger.Start("Insert_phongban");
            string strResult = "";
            try
            {
                if (clParam.maphongban.ToString().Trim() == "")
                {
                    strResult = GetSequence_All("dm_seq", "m_donvi_phongban");
                    //string[] chuoi = clParam.tenphongban.ToString().Trim().Split(' ');
                    //for (int i = 0; i < chuoi.Length; i++)
                    //{
                    //    maphongban += chuoi[i].Substring(0, 1);
                    //}
                    clParam.maphongban = int.Parse(strResult).ToString();
                    base.InsertData(clParam, strTableName);
                }
                else
                {
                    Hashtable param = new Hashtable();
                    param = base.SetDataToHashtable(false, clParam);
                    sqlMap.Update("Danhmuc.UpdateRow_phongban", param);
                }
                strResult = clParam.maphongban;
                
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("Insert_phongban");
            return strResult;
        }

        public string Deleted_phongban(string maphongban,string nguoitao)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                Hashtable param = new Hashtable();
                param["maphongban"] = maphongban;
                param["nguoitao"] = nguoitao;
                sqlMap.Update("Danhmuc.DeletedRow_phongban", param);
                strResult = maphongban;
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("InsertRow");
            return strResult;
        }

        public string Save_chamcong_New(string DataJson, string nguoitao, string machamcong)
        {
            logger.Start("Save_chamcong_New");
            string idmachamcong = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                JObject json = JObject.Parse(DataJson);
                param["machamcong"] = json["data1"]["machamcong"].ToString().Trim();
                machamcong = json["data1"]["machamcong"].ToString().Trim();
                param["manhanvien"] = json["data1"]["manhanvien"].ToString().Trim();
                param["hovaten"] = json["data1"]["hovaten"].ToString().Trim();
                param["email"] = json["data1"]["email"].ToString().Trim();
                param["ngaysinh"] = json["data1"]["ngaysinh"].ToString().Trim();
                param["ngaylamviectu"] = json["data1"]["ngaylamviectu"].ToString().Trim();
                param["ngaylamviecden"] = json["data1"]["ngaylamviecden"].ToString().Trim();
                param["maphongban"] = json["data1"]["maphongban"].ToString().Trim();
                param["thang"] = json["data1"]["thang"].ToString().Trim();
                param["ngaycong_le"] = json["data1"]["ngaycong_le"].ToString().Trim();
                param["nghiphep"] = json["data1"]["nghiphep"].ToString().Trim();
                param["nghihieu_hi"] = json["data1"]["nghihieu_hi"].ToString().Trim();
                param["nghiom_thaisan"] = json["data1"]["nghiom_thaisan"].ToString().Trim();
                param["nghikhongluong"] = json["data1"]["nghikhongluong"].ToString().Trim();
                param["songayduoc_traluong"] = json["data1"]["songayduoc_traluong"].ToString().Trim();
                param["songaypc_tiencom"] = json["data1"]["songaypc_tiencom"].ToString().Trim();
                param["songay_congchuan"] = json["data1"]["songay_congchuan"].ToString().Trim();
                param["chenhlech_ngaycong"] = json["data1"]["chenhlech_ngaycong"].ToString().Trim();
                param["tieuchi"] = json["data1"]["tieuchi"].ToString().Trim();
                param["hoanthanh_congviec"] = json["data1"]["hoanthanh_congviec"].ToString().Trim();
                param["tinhthan_trachnhiem"] = json["data1"]["tinhthan_trachnhiem"].ToString().Trim();
                param["cuxu_hoptac"] = json["data1"]["cuxu_hoptac"].ToString().Trim();
                param["tuanthu_giogiac"] = json["data1"]["tuanthu_giogiac"].ToString().Trim();
                param["xeploai"] = json["data1"]["xeploai"].ToString().Trim();
                param["daduyet"] = "0";
                param["nguoitao"] = nguoitao;

                JArray json_vpp_chitiet = (JArray)json["data2"];
                for (int i = 0; i < json_vpp_chitiet.Count(); i++)
                {
                    for (int j = 21; j < 32; j++)
                    {
                        try 
                        {
                            param["ngay" + j.ToString()] = json_vpp_chitiet[i]["ngay" + j.ToString()].ToString();
                        }
                        catch(Exception){}
                    }

                    for (int j = 1; j < 21; j++)
                    {
                        try
                        {
                            param["ngay" + j.ToString()] = json_vpp_chitiet[i]["ngay" + j.ToString()].ToString();
                        }
                        catch (Exception) { }
                    }
                }
                if (machamcong.Trim() == "" || machamcong.Trim() == "0")
                {
                    param["maphongban"] = json["data1"]["maphongban"].ToString().Trim();
                    param["manhanvien"] = json["data1"]["manhanvien"].ToString().Trim();
                    param["thang"] = json["data1"]["thang"].ToString().Trim();
                    param["email"] = json["data1"]["email"].ToString().Trim();
                    IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_manhanvien_thang", param);
                    if (ilist.Count == 0)
                    {
                        param["machamcong"] = GetSequence_All("dm_seq", "timekeeping");
                        sqlMap.Insert("Timekeeping.InsertRow_chamcong", param);
                    }
                    else
                    {
                        Hashtable paramcc = (Hashtable)ilist[0];
                        param["machamcong"] = paramcc["machamcong"].ToString().Trim();
                        sqlMap.Update("Timekeeping.UpdateRow_chamcong", param);
                    }
                }
                else
                {
                    param["machamcong"] = machamcong.Trim();
                    sqlMap.Update("Timekeeping.UpdateRow_chamcong", param);
                }
                idmachamcong = param["machamcong"].ToString();
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                logger.Error(ex.Message);
            }
            logger.End("Save_chamcong_New");
            return idmachamcong;
        }

        public string Save_chamcong(string DataJson, string nguoitao)
        {
            logger.Start("Save_chamcong");
            string idmachamcong = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                JObject json = JObject.Parse(DataJson);
                param["maphongban"] = json["data1"]["maphongban"].ToString().Trim();
                param["thang"] = json["data1"]["thang"].ToString().Trim();
                param["daduyet"] = "0";
                param["nguoitao"] = nguoitao;
                JArray json_vpp_chitiet = (JArray)json["data2"];
                for (int i = 0; i < json_vpp_chitiet.Count(); i++)
                {
                    for (int j = 21; j < 32; j++)
                    {
                        try
                        {
                            param["ngay" + j.ToString()] = json_vpp_chitiet[i]["ngay" + j.ToString()].ToString();
                        }
                        catch (Exception) { }
                    }

                    for (int j = 1; j < 21; j++)
                    {
                        try
                        {
                            param["ngay" + j.ToString()] = json_vpp_chitiet[i]["ngay" + j.ToString()].ToString();
                        }
                        catch (Exception) { }
                    }
                    string machamcong = json_vpp_chitiet[i]["machamcong"].ToString().Trim();

                    FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkeyEncodeLink.config"));
                    machamcong = AES.DecryptText(machamcong, function.ReadXMLGetKeyEncrypt());
                    param["machamcong"] = machamcong;
                    param["manhanvien"] = json_vpp_chitiet[i]["manhanvien"].ToString().Trim();
                    param["hovaten"] = json_vpp_chitiet[i]["hovaten"].ToString().Trim();


                    if (json_vpp_chitiet[i]["ngaylamviectu"].ToString().Trim() == "undefined" || json_vpp_chitiet[i]["ngaylamviectu"].ToString().Trim() == "")
                    {
                        try
                        {
                            param["ngaylamviectu"] = "21/" + (int.Parse(param["thang"].ToString().Split('/')[0].Trim()) - 1).ToString("00") + "/" + int.Parse(param["thang"].ToString().Split('/')[1]).ToString();
                        }
                        catch (Exception) { param["ngaylamviectu"] = json_vpp_chitiet[i]["ngaylamviectu"].ToString().Trim(); }
                    }
                    else param["ngaylamviectu"] = json_vpp_chitiet[i]["ngaylamviectu"].ToString().Trim();

                    if (json_vpp_chitiet[i]["ngaylamviecden"].ToString().Trim() == "undefined" || json_vpp_chitiet[i]["ngaylamviecden"].ToString().Trim() == "")
                        param["ngaylamviecden"] = "20/" + param["thang"].ToString().Trim();
                    else param["ngaylamviecden"] = json_vpp_chitiet[i]["ngaylamviecden"].ToString().Trim();


                    param["ngaycong_le"] = json_vpp_chitiet[i]["ngaycong_le"].ToString().Trim();
                    param["nghiphep"] = json_vpp_chitiet[i]["nghiphep"].ToString().Trim();
                    param["nghihieu_hi"] = json_vpp_chitiet[i]["nghihieu_hi"].ToString().Trim();
                    param["nghiom_thaisan"] = json_vpp_chitiet[i]["nghiom_thaisan"].ToString().Trim();
                    param["nghikhongluong"] = json_vpp_chitiet[i]["nghikhongluong"].ToString().Trim();
                    param["songayduoc_traluong"] = json_vpp_chitiet[i]["songayduoc_traluong"].ToString().Trim();
                    param["songaypc_tiencom"] = json_vpp_chitiet[i]["songaypc_tiencom"].ToString().Trim();
                    param["songay_congchuan"] = json_vpp_chitiet[i]["songay_congchuan"].ToString().Trim();
                    param["chenhlech_ngaycong"] = json_vpp_chitiet[i]["chenhlech_ngaycong"].ToString().Trim();
                    param["tieuchi"] = json_vpp_chitiet[i]["tieuchi"].ToString().Trim();
                    param["hoanthanh_congviec"] = json_vpp_chitiet[i]["hoanthanh_congviec"].ToString().Trim();
                    param["tinhthan_trachnhiem"] = json_vpp_chitiet[i]["tinhthan_trachnhiem"].ToString().Trim();
                    param["cuxu_hoptac"] = json_vpp_chitiet[i]["cuxu_hoptac"].ToString().Trim();
                    param["tuanthu_giogiac"] = json_vpp_chitiet[i]["tuanthu_giogiac"].ToString().Trim();
                    param["xeploai"] = json_vpp_chitiet[i]["xeploai"].ToString().Trim();
                    param["email"] = json_vpp_chitiet[i]["email"].ToString().Trim();
                    param["ngaysinh"] = json_vpp_chitiet[i]["ngaysinh"].ToString().Trim();


                    if (param["machamcong"].ToString().Trim() == "" || param["machamcong"].ToString().Trim() == "0")
                    {
                        param["email"] = json_vpp_chitiet[i]["email"].ToString().Trim();
                        param["maphongban"] = json["data1"]["maphongban"].ToString().Trim();
                        param["thang"] = json["data1"]["thang"].ToString().Trim();
                        param["machamcong"] = machamcong;
                        param["manhanvien"] = json_vpp_chitiet[i]["manhanvien"].ToString().Trim();
                        IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_manhanvien_thang", param);
                        if (ilist.Count == 0)
                        {
                            param["machamcong"] = GetSequence_All("dm_seq", "timekeeping");
                            sqlMap.Insert("Timekeeping.InsertRow_chamcong", param);
                        }
                        else
                        {
                            Hashtable paramcc=(Hashtable)ilist[0];
                            param["machamcong"] = paramcc["machamcong"].ToString().Trim();
                            sqlMap.Update("Timekeeping.UpdateRow_chamcong", param);
                        }
                        
                    }
                    else
                    {
                        param["machamcong"] = param["machamcong"].ToString().Trim();
                        sqlMap.Update("Timekeeping.UpdateRow_chamcong", param);
                    }
                }
                idmachamcong = param["machamcong"].ToString();
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                logger.Error(ex.Message);
            }
            logger.End("Save_chamcong");
            return idmachamcong;
        }

        public List<TimekeepingModels> SelectRows_hieuchinh_machamcong(string machamcong,string manhanvien)
        {
            logger.Start("SelectRows");
            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            try
            {
                Hashtable param = new Hashtable();
                if (machamcong.Trim() != "0")
                {
                    param["machamcong"] = machamcong;
                    IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_hieuchinh_machamcong", param);
                    CastDataType cast = new CastDataType();
                    lstResult = cast.AdvanceCastDataToList<TimekeepingModels>(ilist);
                }
                else
                {
                    param["manhanvien"] = manhanvien;
                    IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_hieuchinh_manhanvien", param);
                    CastDataType cast = new CastDataType();
                    lstResult = cast.AdvanceCastDataToList<TimekeepingModels>(ilist);
                }
               
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows");
            return lstResult;
        }

        public List<TimekeepingModels> SelectRows_hieuchinh_manhanvien_email(string manhanvien, string email)
        {
            logger.Start("SelectRows_hieuchinh_manhanvien_email");
            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            try
            {
                Hashtable param = new Hashtable();

                param["manhanvien"] = manhanvien;
                param["email"] = email;

                if (manhanvien.Trim() == "")
                {
                    param["manhanvien"] = "";
                }
                if (email.Trim() == "")
                {
                    param["email"] = "";
                }
                IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_hieuchinh_manhanvien_email", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TimekeepingModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows_hieuchinh_manhanvien_email");
            return lstResult;
        }

        public List<TimekeepingModels> SelectRows_hieuchinh_machamcong_themmoi(string machamcong)
        {
            logger.Start("SelectRows_hieuchinh_machamcong_themmoi");
            List<TimekeepingModels> lstResult = new List<TimekeepingModels>();
            try
            {
                Hashtable param = new Hashtable();
                param["machamcong"] = machamcong;
                IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRows_hieuchinh_machamcong", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<TimekeepingModels>(ilist);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
            }
            logger.End("SelectRows_hieuchinh_machamcong_themmoi");
            return lstResult;
        }

        public string Deleted_chamcongthang(string machamcong, string nguoitao)
        {
            logger.Start("InsertRow");
            string strResult = "";
            try
            {
                Hashtable param = new Hashtable();
                param["machamcong"] = machamcong;
                param["nguoitao"] = nguoitao;
                sqlMap.Update("Timekeeping.DeletedRow_chamcongthang", param);
                strResult = machamcong;
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("InsertRow");
            return strResult;
        }


        public string UpdateRow_Duyetpheponline(string maphongban,string thang,string dongy, string nguoitao)
        {
            logger.Start("UpdateRow_Duyetpheponline");
            string strResult = "1";
            try
            {
                Hashtable param = new Hashtable();
                param["maphongban"] = maphongban;
                param["thang"] = thang;
                param["dongy"] = dongy;
                param["nguoitao"] = nguoitao;
                if(dongy.Trim()=="2")
                {
                    IList ilist = sqlMap.ExecuteQueryForList("Timekeeping.SelectRow_Duyetpheponline", param);
                    if(ilist.Count==0)
                        sqlMap.Update("Timekeeping.UpdateRow_Duyetpheponline", param);
                    else strResult = "2";
                }
                else
                {
                    sqlMap.Update("Timekeeping.UpdateRow_Duyetpheponline", param);
                }
                
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("UpdateRow_Duyetpheponline");
            return strResult;
        }

        public string Update_trangthaiguiemail(string maphongban, string thang)
        {
            logger.Start("Update_trangthaiguiemail");
            string strResult = "";
            try
            {
                Hashtable param = new Hashtable();
                param["maphongban"] = maphongban;
                param["thang"] = thang;
                sqlMap.Update("Timekeeping.Update_trangthaiguiemail", param);
                strResult = "1";
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Error(ex.Message);
            }
            logger.End("Update_trangthaiguiemail");
            return strResult;
        }

    }
}