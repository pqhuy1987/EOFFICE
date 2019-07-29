 using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using NHG.DataServices;
using NHG.Logger;
using NHG.Web.Models;
using NHG.Core.Constants;

namespace NHG.Web.Data.Services
{
    public class SuKien010000Service : BaseData
    {
        public SuKien010000Service()
        {
            logger = new Log4Net(typeof(SuKien010000Service));
        }

        #region
        /// <summary>
        /// Load lên các sự kiện theo mã tài khoản
        /// </summary>
        /// <param name="id">mã tài khoản</param>
        /// <returns></returns>
        public List<LichLamViecModels> selectEventForId(string id)
        {
            logger.Start("selectEventForId");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable parram = new Hashtable();
                parram["mataikhoan"] = id;
                //parram["maphongban"] = (int)sqlMap.ExecuteQueryForObject("SuKien010000.selectPhongBan", parram);
                parram["xoa"] = "0";
                parram["hienthitatca"] = "1";
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.loadSuKienTheoId", parram);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("selectEventForId");
            return lstResult;
        }

        /// <summary>
        /// Load lên các sự kiện tổng hợp để duyệt lịch
        /// </summary>
        /// <param name="id">mã tài khoản</param>
        /// <returns></returns>
        public List<LichLamViecModels> loadSuKienTongHop(string id)
        {
            logger.Start("loadSuKienTongHop");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable parram = new Hashtable();
                parram["mataikhoan"] = id;
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                parram["hienthitatca"] = CST_Common.CST_PUBLIC_EVENT;
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.loadSuKienTongHop", parram);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("loadSuKienTongHop");
            return lstResult;
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
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.loadSuKienTheoIdEvent", parram);
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
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.loadSuKien", param);
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

        public List<LichLamViecModels> getListCompare()
        {
            logger.Start("getListCompare");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable param = new Hashtable();
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.getListCompare", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                lstResult = null;
            }
            logger.End("getListCompare");
            return lstResult;
        }

        public List<LichLamViecModels> getListCompareUpdate()
        {
            logger.Start("getListCompareUpdate");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable param = new Hashtable();
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.getListCompareUpdate", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                lstResult = null;
            }
            logger.End("getListCompareUpdate");
            return lstResult;
        }

        public void insertEventWaitAccept(string manguoidung, string masukien, string trangthai)
        {
            logger.Start("insertEventWaitAccept");
            try
            {
                sqlMap.BeginTransaction();
                Hashtable parram = new Hashtable();
                parram["nguoitao"] = manguoidung;
                parram["maghichu"] = masukien;
                parram["trangthai"] = trangthai;
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                sqlMap.Insert("SuKien010000.insertEventWaitAccept", parram);
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                sqlMap.RollbackTransaction();
            }
            logger.End("insertEventWaitAccept");
        }

        public string insertEvent(LichLamViecModels clParam)
        {
            logger.Start("insertEvent");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable parram = SetDataToHashtable(false, clParam);
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                parram["seqname"] = "seq_cal_ghichucongviec";
                string id = sqlMap.ExecuteQueryForObject("Common.GetNextVal", parram).ToString();
                parram["maghichu"] = id;
                sqlMap.Insert("SuKien010000.insertSuKien", parram);
                //sqlMap.CommitTransaction();

                ////insert cal_ghichunguoidung
                //Hashtable parram1 = new Hashtable();
                //parram1["maghichu"] = id;
                //parram1["seqname"] = "seq_cal_ghichucongviecnguoidung";
                //string id1 = sqlMap.ExecuteQueryForObject("Common.GetNextVal", parram1).ToString();
                //parram1["maghichunguoidung"] = id1;
                //parram1["mataikhoan"] = clParam.nguoitaoghichucongviec;
                //parram1["loainhacnho"] = clParam.loainhacnho;
                //parram1["thoigiannhacnho"] = clParam.thoigiannhacnho;
                //parram1["songaynhacnho"] = clParam.songaynhacnho;
                //parram1["danhacnho"] = CST_Common.CST_CHUANHACNHO_FLG;
                //sqlMap.Insert("SuKien010000.insertSuKienNguoiDung", parram1);
                sqlMap.CommitTransaction();
                strResult = id;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                sqlMap.RollbackTransaction();
                strResult = "-1";
            }
            logger.End("insertEvent");
            return strResult;
        }

        public string updateEvent(LichLamViecModels clParam)
        {
            logger.Start("updateEvent");
            string strResult = "";
            try
            {
                //if (clParam.loainhacnho == "1")
                //{
                //    //Khoang cach: datetime - int
                //    DateTime dtime = DateTime.Parse(clParam.ngaybatdau);
                //    int temp = int.Parse(clParam.songaynhacnho);
                //    DateTime dtNhacNho = dtime.AddDays(-temp);
                //    if (dtNhacNho.DayOfWeek == DayOfWeek.Sunday)
                //    {
                //        dtNhacNho = dtNhacNho.AddDays(-2);
                //    }
                //    else if (dtNhacNho.DayOfWeek == DayOfWeek.Saturday)
                //    {
                //        dtNhacNho = dtNhacNho.AddDays(-1);
                //    }
                //    clParam.thoigiannhacnho = dtNhacNho.ToString("yyyy-MM-dd");
                //}
                
                sqlMap.BeginTransaction();
                Hashtable parram = SetDataToHashtable(false, clParam);
                sqlMap.Update("SuKien010000.updateSuKien", parram);

                //sqlMap.Update("SuKien010000.updateSuKienNguoiDung", parram);
                sqlMap.CommitTransaction();
                strResult = "1";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                sqlMap.RollbackTransaction();
                strResult = "-1";
            }
            logger.End("updateEvent");
            return strResult;
        }

        public string updateSuKienRegist(LichLamViecModels clParam)
        {
            logger.Start("updateSuKienRegist");
            string strResult = "";
            try
            {
                Hashtable param = SetDataToHashtable(false, clParam);
                sqlMap.Update("SuKien010000.updateSuKienRegist", param);
                sqlMap.CommitTransaction();
                strResult = "1";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                strResult = "-1";
            }
            logger.End("updateSuKienRegist");
            return strResult;
        }

        public string deleteEvent(int id)
        {
            logger.Start("deleteEvent");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable parram = new Hashtable();
                parram["maghichu"] = id;
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                sqlMap.Delete("SuKien010000.deleteSuKien", parram);
                sqlMap.CommitTransaction();
                strResult = "1";
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                logger.Error(ex.Message);
                strResult = "-1";
            }
            logger.End("deleteEvent");
            return strResult;
        }

        /// <summary>
        /// select sự kiện đến hẹn nhắc nhở
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns>DataSet</returns>

        public List<LichLamViecModels> selectSuKienNhacNho(string id)
        {
            logger.Start("selectSuKienNhacNho");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable parram = new Hashtable();
                parram["mataikhoan"] = id;
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.selectSuKienNhacNho", parram);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("selectSuKienNhacNho");
            return lstResult;
        }

        /// <summary>
        /// Đếm số lượng sự kiện sắp diễn ra
        /// </summary>
        /// <param name="manguoidung">mã người dùng/ Mã tài khoản</param>
        /// <returns></returns>

        public int countEvent(int manguoidung)
        {
            int iReSult = 0;
            logger.Start("countEvent");
            try
            {
                Hashtable parram = new Hashtable();
                parram["mataikhoan"] = manguoidung;
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                iReSult = (int)sqlMap.ExecuteQueryForObject("SuKien010000.countEvent", parram);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                iReSult = -1;
            }
            logger.End("countEvent");
            return iReSult;            
        }

        public string selectNguoiTaoEvent(int manguoidung)
        {
            logger.Start("selectNguoiTaoEvent");
            string strResult = "";
            try
            {
                Hashtable parram = new Hashtable();
                parram["manguoidung"] = manguoidung;
                strResult = (string)sqlMap.ExecuteQueryForObject("SuKien010000.selectNguoiTaoEvent", parram);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                strResult = "";
            }
            logger.End("selectNguoiTaoEvent");
            return strResult;
        }

        /// <summary>
        /// Load lên các sự kiện theo mã tài khoản phân trang SuKien.aspx
        /// </summary>
        /// <param name="id">mã tài khoản</param>
        /// <returns></returns>
        public List<LichLamViecModels> selectNhacNho(int id, int limit)
        {
            logger.Start("selectNhacNho");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable parram = new Hashtable();
                parram["mataikhoan"] = id;
                parram["xoa"] = CST_Common.CST_NOT_DELETE;
                parram["limit"] = limit;
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.selectNhacNho", parram);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.End("selectNhacNho");
            return lstResult;
        }

        #endregion

        #region Week Calendar
        
        /// <summary>
        /// get calendar in a week
        /// </summary>
        /// <param name="dateStart">date to start</param>
        /// <param name="dateEnd">date to end</param>
        /// <returns></returns>
        public List<LichLamViecModels> getCalInWeek(string dateStart, string dateEnd)
        {
            logger.Start("getcalInWeek");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable param = new Hashtable();
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                param["ngaybatdau"] = dateStart;
                param["ngaykethuc"] = dateEnd;
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.getCalInWeek", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                lstResult = null;
            }
            logger.End("getcalInWeek");
            return lstResult;
        }

        public string editCal(LichLamViecModels clParam)
        {
            logger.Start("editCal");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = SetDataToHashtable(false, clParam);
                sqlMap.Update("SuKien010000.editCal", param);
                strResult = "1";
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                logger.Error(ex);
                strResult = "0";
            }
            logger.End("editCal");
            return strResult;
        }

        public string insertCal(LichLamViecModels clParam)
        {
            logger.Start("insertCal");
            string strResult = "";
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = SetDataToHashtable(false, clParam);
                param["seqname"] = "seq_cal_ghichucongviec";
                param["maghichu"] = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
                sqlMap.Insert("SuKien010000.insertCal", param);
                strResult = "Lưu thành công";
                sqlMap.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                logger.Error(ex);
                strResult = "Lỗi " + ex.Message;
            }
            logger.End("insertCal");
            return strResult;
        }

        public List<LichLamViecModels> getListCompareCal()
        {
            logger.Start("getListCompareCal");
            List<LichLamViecModels> lstResult = new List<LichLamViecModels>();
            try
            {
                Hashtable param = new Hashtable();
                param["xoa"] = CST_Common.CST_NOT_DELETE;
                IList iList = sqlMap.ExecuteQueryForList("SuKien010000.getListCompareCal", param);
                CastDataType cast = new CastDataType();
                lstResult = cast.AdvanceCastDataToList<LichLamViecModels>(iList);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                lstResult = null;
            }
            logger.End("getListCompareCal");
            return lstResult;
        }
        #endregion

        #region lich co quan

        #endregion
    }
}