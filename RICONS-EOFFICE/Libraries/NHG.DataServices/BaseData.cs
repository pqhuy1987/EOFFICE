using NHG.Core.Functions;
using NHG.DataServices.Settings;
using NHG.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NHG.DataServices
{
    public class BaseData
    {
        #region Fields and Properties
        public string ErrorCode = "";

        /// <summary>
        /// Logger
        /// </summary>
        protected Log4Net _logger;
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public Log4Net logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Log4Net(this.GetType());

                }
                return _logger;
            }
            set
            {
                _logger = value;
            }

        }

        /// <summary>
        /// SQLMap
        /// </summary>
        protected SQLMap _sqlMap;

        /// <summary>
        /// Gets the SQL map.
        /// </summary>
        /// <value>The SQL map.</value>
        public SQLMap sqlMap
        {
            get
            {

                if (_sqlMap == null)
                {
                    //CREATE NEW SQLMAP WITH SETTINGS
                    try
                    {
                        DataSettingsManager dtm = new DataSettingsManager();
                        DataSettings dtSetting = dtm.LoadSettings();
                        String strSqlMap = dtSetting.DataConnectionString;
                        _sqlMap = new SQLMap(strSqlMap);
                    }
                    catch (ConfigurationErrorsException)
                    {
                        //IF ERROR,CREATE DEFAULT SQLMAP
                        _sqlMap = new SQLMap();
                    }
                }

                // THOW EXCEPTION
                //else
                //    throw new Exception("CAN NOT CREATE SQLMAP FOR REQUEST TO GET DATA FROM DATABASE.");
                return _sqlMap;
            }
        }
        #endregion

        #region Methods
        public BaseData()
        {
        }

        /// <summary>
        /// Chuyen tu List sang HashTable, doi thanh chu Hoa/Thuong cac param
        /// </summary>
        /// <param name="isToUpper"></param>
        /// <param name="objectList">list</param>
        /// <returns></returns>
        protected Hashtable SetDataToHashtable(bool isToUpper, Object objectList)
        {
            Hashtable p = new Hashtable();
            if (objectList.GetType().GetProperties().Length > 0)
                foreach (PropertyInfo propertyInfo in objectList.GetType().GetProperties())
                {
                    if (isToUpper)
                    {
                        try
                        {
                            p[propertyInfo.Name.ToUpper()] = "";
                            if (propertyInfo.GetValue(objectList, null) != null)
                                p[propertyInfo.Name.ToUpper()] = Functions.ConvertValue(propertyInfo.GetValue(objectList, null), propertyInfo.PropertyType.Name);
                        }
                        catch (Exception ex)
                        {
                            p[propertyInfo.Name.ToUpper()] = "";
                            logger.Error(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            p[propertyInfo.Name.ToLower()] = "";
                            if (propertyInfo.GetValue(objectList, null) != null)
                                p[propertyInfo.Name.ToLower()] = Functions.ConvertValue(propertyInfo.GetValue(objectList, null), propertyInfo.PropertyType.Name);
                        }
                        catch (Exception ex)
                        {
                            p[propertyInfo.Name.ToLower()] = "";
                            logger.Error(ex);
                        }
                    }
                }
            return p;
        }

        /// <summary>
        /// Tao danh sach column, value de them moi xuong database
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="columns"></param>
        /// <param name="columnsWhere"></param>
        private void SetDataToArrayListForInsert(Object objectList, ref ArrayList columns, ref ArrayList values)
        {
            if (objectList.GetType().GetProperties().Length > 0)
                foreach (PropertyInfo propertyInfo in objectList.GetType().GetProperties())
                {
                    try
                    {
                        if (propertyInfo.GetValue(objectList, null) != null)
                        {
                            columns.Add(propertyInfo.Name.ToLower());
                            if ((propertyInfo.PropertyType.ToString() == "System.String"
                                || propertyInfo.PropertyType.ToString() == "System.DateTime") && propertyInfo.Name.ToLower() != "ngaytao" && propertyInfo.Name.ToLower() != "ngayhieuchinh")
                                values.Add(string.Format("N\'{1}\'", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));

                            else
                            {
                                if (propertyInfo.PropertyType.ToString() == "int")
                                    values.Add(Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name));
                                else
                                {
                                    values.Add(string.Format("{1}", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));
                                }
                            }
                                

                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
        }

        /// <summary>
        /// Tao danh sach column, value, de cap nhat xuong data base
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        private void SetDataToArrayListForUpdate(Object objectList, ref ArrayList columns, ref ArrayList columnsWhere, ArrayList dieuKienWhere)
        {
            if (objectList.GetType().GetProperties().Length > 0)
                foreach (PropertyInfo propertyInfo in objectList.GetType().GetProperties())
                {
                    try
                    {
                        if (propertyInfo.GetValue(objectList, null) != null)
                        {
                            if (dieuKienWhere.IndexOf(propertyInfo.Name.ToLower()) >= 0)
                            {
                                if (propertyInfo.PropertyType.ToString() == "System.String"
                                || propertyInfo.PropertyType.ToString() == "System.DateTime")
                                    columnsWhere.Add(string.Format("{0} = \'{1}\'", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));

                                else
                                    columnsWhere.Add(string.Format("{0} = {1} ", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));
                            }
                            else
                            {
                                if (propertyInfo.PropertyType.ToString() == "System.String"
                                || propertyInfo.PropertyType.ToString() == "System.DateTime")
                                    columns.Add(string.Format("{0} = \'{1}\'", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));

                                else
                                    columns.Add(string.Format("{0} = {1} ", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));
                            }

                        }
                           
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
        }

        /// <summary>
        ///  Tao danh sach column, value, de select database
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="columns"></param>
        /// <param name="columnsWhere"></param>
        /// <param name="dieuKienWhere"></param>
        private void SetDataToArrayListForSelect(Object objectList, ref ArrayList columns, ref ArrayList columnsWhere, ArrayList dieuKienWhere)
        {
            if (objectList.GetType().GetProperties().Length > 0)
                foreach (PropertyInfo propertyInfo in objectList.GetType().GetProperties())
                {
                    try
                    {
                        if (propertyInfo.GetValue(objectList, null) != null)
                        {
                            if (dieuKienWhere.IndexOf(propertyInfo.Name.ToLower()) >= 0)
                            {
                                if (propertyInfo.PropertyType.ToString() == "System.String"
                                || propertyInfo.PropertyType.ToString() == "System.DateTime")
                                    columnsWhere.Add(string.Format("{0} = \'{1}\'", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));

                                else
                                    columnsWhere.Add(string.Format("{0} = {1} ", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));
                            }
                            else
                            {
                                if (propertyInfo.PropertyType.ToString() == "System.String"
                                || propertyInfo.PropertyType.ToString() == "System.DateTime")
                                    columns.Add(string.Format("{0} = \'{1}\'", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));

                                else
                                    columns.Add(string.Format("{0} = {1} ", propertyInfo.Name.ToLower(), Functions.ConvertValue(propertyInfo.GetValue(objectList, null).ToString().Trim(), propertyInfo.PropertyType.Name)));
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
        }
        #endregion

        #region INSERT/UPDATE/DELETE/SELECT
        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="dataInsert"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// 
        public bool InsertData(Object dataInsert, string tableName)
        {
            _logger.Start("InsertData");
            _logger.Param("TableName", tableName);
            bool result = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();

                #region Tao du lieu cho values de import
                ArrayList arrValues = new ArrayList();
                ArrayList arrColumns = new ArrayList();

                this.SetDataToArrayListForInsert(dataInsert, ref arrColumns, ref arrValues);
                                
                #endregion

                //ten table
                param.Add("tablename", tableName);
                //Cac column trong table
                param.Add("columns", arrColumns);
                //gia tri cua cac du lieu can insert
                param.Add("values", arrValues);
                sqlMap.Insert("Common.InsertRow", param);

                //commit du lieu
                sqlMap.CommitTransaction();
                //them moi du lieu thanh cong
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                sqlMap.RollbackTransaction();
                _logger.Error(ex);
            }

            _logger.End("InsertData");
            return result;
        }

        /// <summary>
        /// Cap nhat du lieu xuong database
        /// </summary>
        /// <param name="dataUpdate"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool UpdateData(Object dataUpdate, string tableName, ArrayList dieuKienWhere)
        {
            _logger.Start("UpdateData");
            _logger.Param("TableName", tableName);
            bool result = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();

                #region Tao du lieu cho values de import
                ArrayList arrwhere = new ArrayList();
                ArrayList arrColumns = new ArrayList();

                this.SetDataToArrayListForUpdate(dataUpdate, ref arrColumns, ref arrwhere, dieuKienWhere);

                #endregion

                //ten table
                param.Add("tablename", tableName);
                //gia tri can update
                param.Add("columns", arrColumns);
                //dieu kien update
                param.Add("arrwhere", arrwhere);
                sqlMap.Update("Common.UpdateRow", param);

                //commit du lieu
                sqlMap.CommitTransaction();
                //cap nhat du lieu thanh cong
                result = true;
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                _logger.Error(ex);
            }

            _logger.End("UpdateData");
            return result;
        }

        public IList SelectRows(Object clParam, string tableName, ArrayList dieuKienWhere)
        {
            logger.Start("SelectRows");
            IList lstResult = null;
            try
            {
                Hashtable param = new Hashtable();

                #region Tao du lieu cho values de import
                ArrayList arrwhere = new ArrayList();
                ArrayList arrColumns = new ArrayList();

                this.SetDataToArrayListForUpdate(clParam, ref arrColumns, ref arrwhere, dieuKienWhere);

                #endregion

                //ten table
                param.Add("tablename", tableName);
                //gia tri can update
                param.Add("columns", arrColumns);
                //dieu kien update
                param.Add("arrwhere", arrwhere);

                IList ilist = sqlMap.ExecuteQueryForList("Common.SimpleSelectRows", param);
            }
            catch (Exception ex)
            {
                logger.Error("Loi ---> " + ex.Message);
                lstResult = null;
            }
            logger.End("SelectRows");
            return lstResult;
        }

        #endregion

        #region Sequence
        /// <summary>
        /// Lay sequence cua column
        /// </summary>
        /// <param name="columnName">Ten column can lay sequence</param>
        /// <returns>
        /// Neu column nay duoc dinh nghia trong table columnlist thi lay sequence ra
        /// Neu khong co thi lay ""
        /// </returns>
        public string GetSequenceName(string tableName)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                try
                {
                    Hashtable param = new Hashtable();
                    param["seqname"] = tableName;
                    return sqlMap.ExecuteQueryForList("Common.GetSequences", param).ToString();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Lay sequence cua column
        /// </summary>
        /// <param name="columnName">Ten column can lay sequence</param>
        /// <returns>
        /// Neu column nay duoc dinh nghia trong table columnlist thi lay sequence ra
        /// Neu khong co thi lay ""
        /// </returns>
        public string GetSequenceValue(string tableName)
        {
            string result = string.Empty;
            try
            {
                Hashtable param = new Hashtable();
                param["seqname"] = "seq_" + tableName;
                string strid = sqlMap.ExecuteQueryForObject("Common.GetNextVal", param).ToString();
            }
            catch (Exception ex)
            {
                result = string.Empty;
                _logger.Error(ex);
            }
            return result;
        }

        public SequenceValuecs GetSequenceValue(string tableName, string maTaiKhoan)
        {
            SequenceValuecs result = new SequenceValuecs();
            try
            {
                Hashtable param = new Hashtable();
                param["sequencename"] = "seq_" + tableName;
                param["mataikhoan"] = maTaiKhoan;
                IList ilist = sqlMap.ExecuteQueryForList("Common.GetNextSeq", param);
                CastDataType cast = new CastDataType();
                if(ilist.Count > 0)
                    result = cast.AdvanceCastDataToList<SequenceValuecs>(ilist)[0];
            }
            catch (Exception ex)
            {
                result = new SequenceValuecs();
                _logger.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// Cap nhat tham so tang tu dong xuong duoi database
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="maTaiKhoan"></param>
        /// <param name="giaTri"></param>
        /// <returns></returns>
        public bool UpdateSequenceValue(string tableName, string maTaiKhoan, string giaTri)
        {
            bool result = false;
            try
            {
                sqlMap.BeginTransaction();
                Hashtable param = new Hashtable();
                param["sequencename"] = "seq_" + tableName;
                param["mataikhoan"] = maTaiKhoan;
                param["giatri"] = giaTri;
                sqlMap.Update("Common.UpdateNextSeq", param);
                sqlMap.CommitTransaction();
                result = false;
            }
            catch (Exception ex)
            {
                sqlMap.RollbackTransaction();
                result = false;
                _logger.Error(ex);
            }
            return result;
        }


        public string GetSequence_All(string tableName,string columname)
        {
            string id = "";
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                try
                {
                    Hashtable param = new Hashtable();
                    param["tablename"] = tableName;
                    param["columname"] = columname;
                    id = sqlMap.ExcuteQueryForString("Common.Get_seq_All", param).ToString();
                    if (id != "")
                    {
                        param["giatri"] = (int.Parse(id)+1);
                        sqlMap.Update("Common.Update_seq_All", param);
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            return id;
        }



        #endregion
    }
}
