using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using NHG.Core.Functions;
using NHG.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NHG.DataServices
{
    public class SQLMap
    {
        #region properties
        protected SqlMapper sqlMap;
        /// <summary>
        /// SQLConfig file
        /// </summary>
        protected string str_SqlMapConfig;
        protected Exception error;
        public bool ConvertParameter = true;
        /// <summary>
        /// logger
        /// </summary>
        public Log4Net logger = new Log4Net(typeof(SQLMap));
        #endregion properties

        #region Constructors
        /// <summary>
        /// Constructor without arguments
        /// </summary>
        public SQLMap()
        {
            //
            // TODO: 
            //
            logger.Start("Constructor SQLMap");
            try
            {
                sqlMap = (SqlMapper)Mapper.Instance();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            logger.End("Constructor SQLMap");
        }

        /// <summary>
        /// Constructor with sqlConfig file.
        /// </summary>
        /// <param name="sqlMapConfig">SQLConfig file</param>
        public SQLMap(string sqlMapConfig)
        {
            logger.Start("Constructor SQLMap - Map du lieu");
            try
            {
                logger.Param("Loi kết nối tới database", sqlMapConfig);
                //create SQLMapper from SQLConfig file
                this.str_SqlMapConfig = sqlMapConfig;
                DomSqlMapBuilder cDom = new DomSqlMapBuilder();
                sqlMap = (SqlMapper)cDom.Configure(sqlMapConfig);
                sqlMap.DataSource.ConnectionString = EncryptData(sqlMap.DataSource.ConnectionString);
                
                logger.Info("Creating SQLMapper sucessfull");
            }
            catch (SqlException ex)
            {
                logger.Error(ex.ToString());
            }
            logger.End("Constructor SQLMap");
        }

        private string EncryptData(string connectionString)
        {
            try
            {
                //duong dan file encryption key
                FunctionXML function = new FunctionXML(Functions.MapPath("~/Xml/Config/encryptionkey.config"));
                //lay key da ma hoa
                string strKeyEncrypt = EncDec.Decrypt(function.ReadXMLGetKeyEncrypt());
                //lay gia tri trong cau connection string
                string[] strSplit = connectionString.Split(';');
                //string strServer = AES.DecryptText(strSplit[0].Replace("server=", ""), strKeyEncrypt);
                //string strDatabaseName = AES.DecryptText(strSplit[1].Replace("database=", ""), strKeyEncrypt);
                //string strUserName = AES.DecryptText(strSplit[2].Replace("user id=", ""), strKeyEncrypt);
                //string strPassword = AES.DecryptText(strSplit[3].Replace("password=", ""), strKeyEncrypt);

                string strServer = strSplit[0].Replace("server=", "");
                string strDatabaseName = strSplit[1].Replace("database=", "");
                string strUserName =strSplit[2].Replace("userid=", "");
                string strPassword = strSplit[3].Replace("password=", "");


                string aaaa = string.Format("{0};{1};{2};{3}", strServer, strDatabaseName, strUserName, strPassword, strSplit[2].Split('=')[1]);


                //Data Source=RI-TONGHOP-01\SQLEXPRESS;Initial Catalog=nhg;Integrated Security=True

                return string.Format("{0};{1};{2};{3}", strServer, strDatabaseName, strUserName, strPassword, strSplit[2].Split('=')[1]);

                //return string.Format("server={0};database={1};user id={2};password={3};Port={4};", strServer, strDatabaseName, strUserName, strPassword, strSplit[4].Split('=')[1]);
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
            return string.Empty;
        }
        #endregion Constructor

        #region Connection
        /// <summary>
        /// Open connection.
        /// </summary>
        public bool OpenConnection()
        {
            logger.Start("OpenConnection.");
            logger.Info("OpenConnection.");
            try
            {
                sqlMap.OpenConnection();
                logger.Info("OpenConnection sucessful.");
                logger.End("OpenConnection.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                error = ex;
                logger.Info("OpenConnection fail.");
                logger.End("OpenConnection.");
                return false;
            }
        }
        /// <summary>
        /// Close connection.
        /// </summary>
        public bool CloseConnection()
        {
            logger.Start("CloseConnection");
            logger.Info("Close Connection");
            try
            {
                sqlMap.CloseConnection();
                logger.Info("CloseConnection successful");
                logger.End("CloseConnection");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                error = ex;
                logger.Info("CloseConnection fail.");
                logger.End("CloseConnection");
                throw (new Exception("DisConnectDB:ERR-134", new Exception("DisConnectDB:ERR-134")));
            }
        }
        #endregion Connection

        #region Transaction
        /// <summary>
        /// Begin transaction.
        /// </summary>
        public bool BeginTransaction()
        {
            logger.Start("BeginConnection");
            logger.Info("Begin transaction");
            try
            {
                sqlMap.BeginTransaction();
                logger.Info("BeginTransaction successful.");
                logger.End("BeginTransaction.");
                return true;
            }
            catch (Exception ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.Info("BeginTransaction fail.");
                logger.End("BeginTransaction.");
                throw (new Exception("DisConnectDB:ERR-134", new Exception("DisConnectDB:ERR-134")));
            }
        }

        /// <summary>
        /// Commit transaction.
        /// </summary>
        public bool CommitTransaction()
        {
            logger.Start("CommitTransaction");
            logger.Info("CommitTransaction");
            try
            {
                sqlMap.CommitTransaction();
                logger.Info("CommitTransaction successful.");
                logger.End("CommitTransaction.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                error = ex;
                logger.Info("CommitTransaction fail.");
                logger.End("CommitTransaction.");
                return false;
            }
        }

        /// <summary>
        /// Roolback transaction.
        /// </summary>
        public bool RollbackTransaction()
        {
            logger.Start("RollbackTransaction");
            bool sucess = true;
            try
            {
                sqlMap.RollBackTransaction();
                logger.Info("RollBackTransaction successful.");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                error = ex;
                logger.Info("RollBackTransaction fail.");
                sucess = false;
            }
            logger.End("RollBackTransaction.");
            return sucess;
        }
        #endregion Transaction

        #region ExcuteQuery

        /// <summary>
        /// Execute query return an Object.
        /// </summary>
        /// <param name="str_sqlID">id of sqlMapping</param>
        /// <returns> Data as an Object </returns>
        public object ExecuteQueryForObject(string str_sqlID, object parameter)
        {
            logger.Start("ExcuteQueryForObject");
            logger.Param("SQLMap_ID", str_sqlID);
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            if (ConvertParameter)
                ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            //(parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            object obj = null;
            try
            {
                obj = sqlMap.QueryForObject(str_sqlID, parameter);
                if (obj != null)
                    logger.Info("ExecuteQueryForObject successful.Object = " + obj.ToString());
                else
                    logger.Info("ExecuteQueryForObject successful.Object = null");
                logger.End("ExecuteQueryForObject.");

            }
            catch (SqlException ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.Info("ExecuteQueryForObject fail.");
                logger.End("ExecuteQueryForObject.");
                throw ex;

            }
            
            return obj;

        }

        /// <summary>
        /// Execute query and return a hastable.
        /// </summary>
        /// <param name="str_sqlID">id of sqlMapping</param>
        /// <returns>data as hashtable</returns>
        
        public Hashtable ExcuteQueryForHash(string str_sqlID, object parameter)
        {
            logger.Start("ExcuteQueryForHash");
            logger.Param("SQLMap_ID", str_sqlID);
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan DB.
            if (ConvertParameter)
                ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            // (parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            Hashtable hash = new Hashtable();
            Object obj;
            try
            {

                obj = sqlMap.QueryForObject(str_sqlID, parameter);
                if (obj != null)
                {
                    hash = (Hashtable)obj;
                    string id = hash["giatri"].ToString();
                }
                logger.Info("ExcuteQueryForHash successful.");
                logger.Param("Result count", hash.Count);


            }
            catch (Exception ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.Info("ExcuteQueryForHash fail.");
                logger.End("ExcuteQueryForHash.");
                throw ex;
            }
            logger.End("ExcuteQueryForHash.");
            return hash;
        }

        public string ExcuteQueryForString(string str_sqlID, object parameter)
        {
            logger.Start("ExcuteQueryForHash");
            logger.Param("SQLMap_ID", str_sqlID);
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan DB.
            if (ConvertParameter)
                ConvertUtilities.ConvertToDBValue(parameter);
            string id = "";
            try
            {
                Object obj = sqlMap.QueryForObject(str_sqlID, parameter);
                if (obj != null)
                {
                    Hashtable hash = (Hashtable)obj;
                    id = hash["giatri"].ToString();
                }
                logger.Info("ExcuteQueryForHash successful.");
            }
            catch (Exception ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.Info("ExcuteQueryForHash fail.");
                logger.End("ExcuteQueryForHash.");
                throw ex;
            }
            logger.End("ExcuteQueryForHash.");
            return id;
        }




        public IList ExecuteQueryForListNotLog(string str_sqlID, object parameter)
        {
            logger.Start("ExecuteQueryForList");
            logger.Param("SQLMap_ID", str_sqlID);
            //ghi log cua cac parameter
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            //  (parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            IList list = null;
            try
            {
                list = sqlMap.QueryForList(str_sqlID, parameter);
                logger.Info("Result ExecuteQueryForList : recordcount = " + list.Count.ToString());
                logger.Info("ExecuteQueryForList successful.");

            }
            catch (SqlException ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.Info("ExecuteQueryForList fail.");
                logger.End("ExecuteQueryForList");
                throw ex;
            }
            logger.End("ExecuteQueryForList");
            return list;

        }

        /// <summary>
        /// Execute query, return an list of data.
        /// </summary>
        /// <param name="str_sqlID"> id of sqlMapping</param>
        /// <returns></returns>
        public IList ExecuteQueryForList(string str_sqlID, object parameter)
        {
            logger.Start("ExecuteQueryForList");
            logger.Param("SQLMap_ID", str_sqlID);
            //ghi log cua cac parameter
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            // (parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            IList list = null;
            try
            {
                list = sqlMap.QueryForList(str_sqlID, parameter);
                logger.Info("Result ExecuteQueryForList : recordcount = " + list.Count.ToString());
                logger.Info("ExecuteQueryForList successful.");

            }
            catch (SqlException ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.Info("ExecuteQueryForList fail.");
                logger.End("ExecuteQueryForList");
                throw ex;
            }
            logger.End("ExecuteQueryForList");
            return list;

        }
        /// <summary>
        /// Execute query and return a datatable.
        /// </summary>
        /// <param name="str_sqlID"> id of sqlMapping</param>
        /// <returns>data as hash table</returns>
        public DataTable ExecuteQueryForDataTable(string str_sqlID, object parameter)
        {
            logger.Start("ExecuteQueryForDataTable");
            logger.Param("SQLMap_ID", str_sqlID);
            //ghi log cua cac parameter
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            //(parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            DataTable table = null;
            try
            {
                IList list = sqlMap.QueryForList(str_sqlID, parameter);
                table = ConvertUtilities.ToDataTable("table", list);
                if (table != null)
                    logger.Debug("So dong lay duoc" + table.Rows.Count.ToString());
            }
            catch (SqlException ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.End("ExecuteQueryForDataTable");
                throw ex;
            }
            logger.End("ExecuteQueryForDataTable");
            return table;

        }

        /// <summary>
        /// Execute query and return a datatable.
        /// </summary>
        /// <param name="str_sqlID"> id of sqlMapping</param>
        /// <param name="strTableName">Table name</param>
        /// <returns>data as hash table</returns>
        public DataTable ExecuteQueryForDataTable(string str_sqlID, string strTableName, object parameter)
        {
            logger.Start("ExecuteQueryForDataTable");
            logger.Param("SQLMap_ID", str_sqlID);
            //ghi log cua cac parameter
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            //  (parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            DataTable table = null;
            try
            {
                IList list = sqlMap.QueryForList(str_sqlID, parameter);
                table = ConvertUtilities.ToDataTable(strTableName, list);
                logger.Debug(table.Rows.Count + " selected!!!");
            }
            catch (SqlException ex)
            {
                error = ex;
                logger.Error(ex.ToString());
                logger.End("ExecuteQueryForDataTable");
                throw ex;
            }
            logger.End("ExecuteQueryForDataTable");
            return table;

        }


        #endregion ExcuteQuery

        #region Insert,Update, Delete
        /// <summary>
        /// Execute insert sql.
        /// </summary>
        /// <param name="str_SqlID">SQL id</param>
        /// <param name="parameter">Values inserted</param>
        /// <returns>
        /// Khoa chinh cua dong vua moi insert.
        /// </returns>
        public object Insert(string str_SqlID, object parameter)
        {
            logger.Start("Insert");
            logger.Param("SQL id", str_SqlID);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            if (ConvertParameter)
                ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            //  (parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            ShowParam(parameter);
            //BeginTransaction();
            object result = null;
            try
            {
                result = sqlMap.Insert(str_SqlID, parameter);
                //logger.Param("ID",result.ToString());
                //CommitTransaction();
                logger.Debug("Success!");
            }
            catch (Exception ex)
            {
                error = ex;
                //RollbackTransaction();
                logger.Debug("Insert Fail!");
                logger.Error(ex);
                logger.End("Insert");
                throw ex;
            }
            logger.End("Insert");
            return result;

        }
        /// <summary>
        /// Delete records.
        /// </summary>
        /// <param name="str_SqlID">SQL for delete</param>
        /// <param name="parameter">Conditions</param>
        /// <returns></returns>
        public object Delete(string str_SqlID, object parameter)
        {
            logger.Start("Delete");
            logger.Param("SQL id", str_SqlID);
            //ghi log cua cac parameter
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            if (ConvertParameter)
                ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            // (parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            //BeginTransaction();
            Object result = null;
            try
            {
                result = sqlMap.Delete(str_SqlID, parameter);
                //CommitTransaction();
                logger.Debug("Success!");
            }
            catch (Exception ex)
            {
                error = ex;
                //RollbackTransaction();
                logger.Debug("Delete Fail!");
                logger.Error(ex);
                logger.End("Delete");
                throw ex;
            }
            logger.End("Delete");
            return result;
        }
        /// <summary>
        /// Update records.
        /// </summary>
        /// <param name="str_SqlID">SQL for Update</param>
        /// <param name="parameter">Values inserted and conditions</param>
        /// <returns></returns>
        public object Update(string str_SqlID, object parameter)
        {
            logger.Start("Update");
            logger.Param("SQL id", str_SqlID);
            //ghi log cua cac parameter
            ShowParam(parameter);
            //Convert cac ki tu dac biet trong param dua vao cho dung chuan Ingres.
            if (ConvertParameter)
                ConvertUtilities.ConvertToDBValue(parameter);
            if (parameter == null)
                parameter = new Hashtable();
            //  (parameter as Hashtable)["DB"] = Global.CST_DATABSENAME;
            //BeginTransaction();
            Object result = null;
            try
            {
                result = sqlMap.Update(str_SqlID, parameter);
                //CommitTransaction();
                logger.Debug("Success!");
            }
            catch (Exception ex)
            {
                error = ex;
                //RollbackTransaction();
                logger.Debug("Fail!");
                logger.Error(ex);
                logger.End("Update");
                throw ex;
            }
            logger.End("Update");
            return result;
        }

        #endregion Insert,Update, Delete

        #region Ghi log cho param
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void ShowParam(object parameter)
        {
            try
            {
                if (parameter == null)
                {
                    logger.Debug("parameter null");
                    return;
                }
                if (parameter is Hashtable)
                {
                    foreach (DictionaryEntry dic in (Hashtable)parameter)
                        logger.Param(dic.Key.ToString(), dic.Value.ToString());
                }
                else
                    logger.Param("param", parameter);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
