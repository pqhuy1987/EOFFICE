using NHG.Logger;
using System;
using System.Collections;
using System.Data;

namespace NHG.DataServices
{
    public class ConvertUtilities
    {
        protected static Log4Net logger = new Log4Net(typeof(ConvertUtilities));

        /// <summary>
        /// Are constructors.
        /// </summary>
        private ConvertUtilities()
        {

        }

        /// <summary>
        /// Sets the data rows.
        /// </summary>
        /// <param name="toTable">To table.</param>
        /// <param name="fromTable">From table.</param>
        public static void SetDataRows(DataTable toTable, DataTable fromTable)
        {

            if (toTable == null)
            {
                return;
            }

            toTable.Clear();

            if (fromTable == null)
            {
                return;
            }
            logger.Start("SetDataRows");
            foreach (DataRow dataRow in fromTable.Rows)
            {
                toTable.ImportRow(dataRow);
            }
            logger.End("SetDataRows");
            return;
        }

        /// <summary>
        /// Specified in the table and then assign a value of type IList.
        /// <br></br>
        /// </summary>
        /// <param name="dataTable">table</param>
        /// <param name="list">IList</param>
        public static void SetDataRows(DataTable dataTable, IList list)
        {

            if (dataTable == null)
            {
                return;
            }

            logger.Start("SetDataRows");
            dataTable.Rows.Clear();
            if (list != null)
            {
                string[] columnNameArray = new string[dataTable.Columns.Count];

                for (int count = 0; count < dataTable.Columns.Count; count++)
                {
                    columnNameArray[count] =
                            dataTable.Columns[count].ColumnName;
                }
                foreach (Hashtable row in list)
                {
                    DataRow dataRow = dataTable.NewRow();

                    foreach (string key in columnNameArray)
                    {
                        object value = row[key];
                        if (value == null)
                        {
                            value = DBNull.Value;
                        }

                        dataRow[key] = value;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            dataTable.AcceptChanges();
            logger.End("SetDataRows");
            return;
        }

        public static void CopyDataTable(DataTable toTable, DataTable fromTable)
        {
            for (int r = 0; r < fromTable.Rows.Count; r++)
            {
                DataRow row = toTable.NewRow();
                for (int c = 0; c < toTable.Columns.Count; c++)
                {
                    String colName = toTable.Columns[c].ColumnName;
                    if (fromTable.Columns.Contains(colName))
                        row[colName] = fromTable.Rows[r][colName];
                }
                toTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Specified in the table and then assign a value of type Hashtable
        /// </summary>
        /// <param name="dataTable">table
        /// <br></br></param>
        /// <param name="row">Hashtable</param>
        /// <br></br>
        public static void SetDataRows(DataTable dataTable, Hashtable row)
        {
            if (dataTable == null)
            {
                return;
            }
            logger.Start("SetDataRows");
            dataTable.Rows.Clear();

            if (row != null && 0 < row.Count)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    string key = dataColumn.ColumnName;

                    object value = row[key];
                    if (value == null)
                    {
                        value = DBNull.Value;
                    }

                    dataRow[key] = value;
                }

                dataTable.Rows.Add(dataRow);
            }

            dataTable.AcceptChanges();
            logger.End("SetDataRows");
            return;
        }

        /// <summary>
        /// IList value to convert into a DataSet type.
        /// <br></br>
        /// </summary>
        /// <br></br></returns>
        public static DataSet ToDataSet(string tableName, IList list)
        {
            DataSet dataSet = new DataSet();

            if (list != null && 0 < list.Count)
            {
                dataSet.Tables.Add(ToDataTable(tableName, list));
            }

            return dataSet;
        }

        /// <summary>
        /// 	A value of type Hashtable to convert into a DataSet.
        /// </summary>
        public static DataSet ToDataSet(string tableName, Hashtable row)
        {
            DataSet dataSet = new DataSet();

            if (row != null && 0 < row.Count)
            {
                dataSet.Tables.Add(ToDataTable(tableName, row));
            }

            return dataSet;
        }

        public static DataTable ToDataTable(string tableName, IList list)
        {
            DataTable dataTable = new DataTable(tableName);

            if (list != null)
            {
                foreach (Hashtable row in list)
                {
                    DataRow dataRow = dataTable.NewRow();

                    foreach (string key in row.Keys)
                    {
                        object value = row[key];
                        if (value == null)
                        {
                            value = DBNull.Value;
                        }

                        try
                        {
                            dataRow[key] = value;
                        }
                        catch (ArgumentException)
                        {
                            if (value != DBNull.Value)
                                dataTable.Columns.Add(key, value.GetType());
                            else
                                dataTable.Columns.Add(key);
                            dataRow[key] = value;
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            dataTable.AcceptChanges();

            return dataTable;
        }

        /// <summary>
        /// Fill data to DataTable.
        /// </summary>
        /// <param name="dataTable">The DataTable to fill.</param>
        /// <param name="list">The list.</param>
        public static void FillDataTable(DataTable dataTable, IList list)
        {
            if (dataTable == null) return;
            if (list != null)
            {
                foreach (Hashtable row in list)
                {
                    DataRow dataRow = dataTable.NewRow();

                    foreach (string key in row.Keys)
                    {
                        object value = row[key];
                        if (value == null)
                            value = DBNull.Value;
                        try
                        {
                            dataRow[key] = value;
                        }
                        catch (ArgumentException) { }
                    }
                    try
                    {
                        dataTable.Rows.Add(dataRow);
                    }
                    catch (Exception) { /*CONSTRAINTS ERROR*/}

                }
            }
            dataTable.AcceptChanges();
        }

        public static DataTable ToDataTable(string tableName, Hashtable row)
        {
            DataTable dataTable = new DataTable(tableName);

            if (row != null && 0 < row.Count)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (string key in row.Keys)
                {
                    object value = row[key];
                    if (value == null)
                    {
                        value = DBNull.Value;
                    }

                    try
                    {
                        dataRow[key] = value;
                    }
                    catch (ArgumentException)
                    {
                        if (value != DBNull.Value)
                        {
                            dataTable.Columns.Add(key, value.GetType());
                            dataRow[key] = value;
                        }
                    }
                }

                dataTable.Rows.Add(dataRow);
            }

            dataTable.AcceptChanges();

            return dataTable;
        }

        public static Hashtable ToHashtable(IList list, string keyName,
            string valueName)
        {
            if (list == null || keyName == null || valueName == null)
            {
                return new Hashtable();
            }

            //
            Hashtable hashtable = new Hashtable();

            foreach (Hashtable row in list)
            {
                object key = row[keyName];

                if (key != null && key != DBNull.Value)
                {
                    object value = row[valueName];
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }

                    hashtable[key] = value;
                }
            }

            return hashtable;
        }

        public static Hashtable ToHashtable(DataRow dataRow)
        {
            if (dataRow == null)
            {
                return new Hashtable();
            }

            //
            Hashtable hashtable = new Hashtable();

            foreach (DataColumn dataColumn in dataRow.Table.Columns)
            {
                string key = dataColumn.ColumnName;
                object value = dataRow[key];

                if (value == DBNull.Value)
                {
                    value = null;
                }

                hashtable[key] = value;
            }

            return hashtable;
        }

        /// <summary>
        /// Convert date string with format
        /// </summary>
        /// <param name="value">Date String</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(String value)
        {
            DateTime dt = DateTime.Today;
            value = value.Trim(' ', '@');
            String[] datePart = value.Split(new string[] { "y", "m", "d", "-", "/", " ", "@" }, StringSplitOptions.RemoveEmptyEntries);
            if (datePart.Length != 3)
                throw (new Exception(String.Format("Date String:{0} is invalid format yyyy[/,y,-,@, ]MM[/,m,-,@, ]dd[d]", value)));
            try
            {
                dt = new DateTime(int.Parse(datePart[0]), int.Parse(datePart[1]), int.Parse(datePart[2]));
            }
            catch (Exception ex)
            {
                throw (new Exception(String.Format("Date String:{0} is invalid format yyyy[/,y,-,@, ]MM[/,m,-,@, ]dd[d]", value), ex));
            }
            return dt;
        }

        public static string ToDateString(string value, string format)
        {
            string[] formatArray = new string[] {
					"yyyyMMdd",		"yyMMdd",
					"yyyy/MM/dd",	"yyyy/MM/d",	"yyyy/M/dd",	"yyyy/M/d",
					"yy/MM/dd",		"yy/MM/d",		"yy/M/dd",		"yy/M/d",
					"yyyy-MM-dd",	"yyyy-MM-d",	"yyyy-M-dd",	"yyyy-M-d",
					"yy-MM-dd",		"yy-MM-d",		"yy-M-dd",		"yy-M-d",
					"yyyy.MM.dd",	"yyyy.MM.d",	"yyyy.M.dd",	"yyyy.M.d",
					"yy.MM.dd",		"yy.MM.d",		"yy.M.dd",		"yy.M.d",
                    "dd/MM/yyyy",   "dd.MM.yyyy",   "dd-MM-yyyy"
					};

            DateTime dateTime;
            foreach (string str in formatArray)
            {
                try
                {
                    dateTime = DateTime.ParseExact(value, str, null);

                    if (format == null || format.Trim().Equals(""))
                    {
                        return dateTime.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        return dateTime.ToString(format);
                    }
                }
                catch (Exception)
                {
                }
            }
            return "";
        }


        public static string ToDateMonthString(string value, string format)
        {
            string[] formatArray = new string[] {
					"yyyyMM",	"yyMM",
					"yyyy/MM",	"yyyy/M",	"yy/MM",	"yy/M",
					"yyyy-MM",	"yyyy-M",	"yy-MM",	"yy-M",
					"yyyy.MM",	"yyyy.M",	"yy.MM",	"yy.M",
                     "dd/MM/yyyy",   "dd.MM.yyyy",   "dd-MM-yyyy"

					};

            DateTime dateTime;

            foreach (string str in formatArray)
            {
                try
                {
                    dateTime = DateTime.ParseExact(value, str, null);

                    if (format == null || format.Trim().Equals(""))
                    {
                        return dateTime.ToString();
                    }
                    else
                    {
                        return dateTime.ToString(format);
                    }
                }
                catch (Exception)
                {
                }
            }
            return "";
        }
        /// <summary>
        /// Kiem tra cac ki tu dac biet cua Ingres
        /// </summary>        
        /// <param name="param">param</param>
        /// <example>
        /// Insert into table values('A'') --> bi error trong Ingres.
        /// <br></br>
        /// Sua lai thanh : Insert into table values('A''')
        /// </example>
        /// <returns>
        /// Null neu param = null
        /// <br></br>
        /// Gia tri hop le cua ingres.
        /// </returns>
        public static object ConvertToDBValue(object param)
        {

            if (param == null)
                return null;

            if (param is Hashtable)
            {
                Hashtable parameter = (Hashtable)param;
                Hashtable tmp = (Hashtable)parameter.Clone();
                foreach (DictionaryEntry dic in tmp)
                {
                    if (parameter[dic.Key] is string)
                    {
                        parameter[dic.Key] = parameter[dic.Key].ToString().Replace("\\", "\\\\");
                        //  parameter[dic.Key] = parameter[dic.Key].ToString().Replace("?", "\\?");
                        //  parameter[dic.Key] = parameter[dic.Key].ToString().Replace("copyright ", "\\copyright");
                    }
                }
            }
            else if (param is string)
            {
                param = param.ToString().Replace("\\", "\\\\");
                //  param = param.ToString().Replace("?", "\\?");
                //  param = param.ToString().Replace("copyright", "\\copyright");

            }
            return param;
        }
    }
}
