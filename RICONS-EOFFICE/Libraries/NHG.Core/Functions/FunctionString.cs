using NHG.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHG.Core.Functions
{
    public static class Functiontring
    {
        #region Field
        private static Log4Net _logger = new Log4Net(typeof(Functiontring));
        #endregion

        /// <summary>
        /// Lay dinh dang format string
        /// vi du: string.Format("{0}", ab);
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string ReturnStringFormatID(string column)
        {
            try
            {
                FunctionXML fnc = new FunctionXML(Functions.MapPath("/Xml/Config/StringFormat.xml"));
                string strFormat = fnc.GetStringFormatIDForDataBase(column);
                return strFormat;
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return string.Empty;
        }

        public static string ReturnStringFormatEmail(string column)
        {
            try
            {
                FunctionXML fnc = new FunctionXML(Functions.MapPath("/Xml/Config/StringEmailTo.xml"));
                string strFormat = fnc.GetStringFormatIDForDataBase(column);
                return strFormat;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return string.Empty;
        }

        public static string ReturnStringFormatthongtincauhinhmail(string column)
        {
            try
            {
                FunctionXML fnc = new FunctionXML(Functions.MapPath("/Xml/Config/email.xml"));
                string strFormat = fnc.GetStringFormatIDForDataBase(column);
                return strFormat;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return string.Empty;
        }


        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
