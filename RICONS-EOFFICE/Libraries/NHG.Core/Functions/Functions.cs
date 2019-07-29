
using NHG.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace NHG.Core.Functions
{
    public class Functions
    {
        private static Log4Net logger = new Log4Net(typeof(Functions));

        /// <summary>
        /// Cat chuoi theo so luong ky trong chuoi
        /// </summary>
        /// <param name="strStringSplit">Chuoi can cat</param>
        /// <param name="iLength">do dai bao nhieu ky tu</param>
        /// <returns></returns>
        public static string SplitString(string strStringSplit, int iLength)
        {
            string[] strCatTin = strStringSplit.Split(' ');
            string strTemp = string.Empty;
            if (strStringSplit.Length > iLength)
                for (int i = 0; i < strCatTin.Length; i++)
                {
                    if ((strTemp.Length + strCatTin[i].Length) > iLength)
                        break;
                    if (((strTemp.Length + strCatTin[i].Length + strCatTin[i + 1].Length > iLength) && strCatTin[i].Contains(":")) ||
                        ((strTemp.Length + strCatTin[i].Length + strCatTin[i + 1].Length > iLength) && strCatTin[i].Contains("-")) ||
                        ((strTemp.Length + strCatTin[i].Length + strCatTin[i + 1].Length > iLength) && strCatTin[i].Contains("\"")))
                    {
                        strTemp += "";
                        break;
                    }
                    else
                        strTemp += strCatTin[i] + " ";
                }
            return strTemp.Length <= 0 ? strStringSplit : strTemp + "...";
        }

        /// <summary>
        /// Chuyen chu tu Dinh dang abc sang Danh dang Abc
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UppercaseWords(string strInput)
        {
            strInput = strInput.ToLower();
            return Regex.Replace(strInput, @"\b[a-z]\w+", delegate(Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }

        public static string GetFirstLetter(string strInput)
        {
            return Regex.Replace(strInput, @"\b[a-zA-Z]\w+", delegate(Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]).ToString();
            });
        }

        public static string NotRowInTable()
        {
            string strResult = @"<div class='not_rowH'></div><div class='not_rowT'>Không có văn bản!!!</div>";
            return System.Text.RegularExpressions.Regex.Replace(strResult, @"\t|\n|\r", "");
        }

        // danh sách nhan viec va thu hồi. được phân cách với nhau bởi dấu |
        public string GiaoViec(string[] usernew, string[] userold)
        {
            string str1 = "";
            for (int i = 0; i < usernew.Length; i++)
            {
                for (int j = 0; j < userold.Length; j++)
                {
                    if (userold[j].Trim() == usernew[i].Trim())
                    {
                        usernew[i] = "0";
                        break;
                    }

                }
            }
            for (int i = 0; i < usernew.Length; i++)
            {
                str1 += usernew[i] + ",";
            }
            return str1;
        }
        // công việc thu hồi
        public string ThuHoi(string[] usernew, string[] userold)
        {
            string str2 = "";
            for (int i = 0; i < userold.Length; i++)
            {
                for (int j = 0; j < usernew.Length; j++)
                {
                    if (usernew[j].Trim() == userold[i].Trim())
                    {
                        userold[i] = "0";
                        break;
                    }

                }
            }
            for (int i = 0; i < userold.Length; i++)
            {
                str2 += userold[i] + ",";
            }
            return str2;
        }

        /// <summary>
        /// Tinh phan tram cong viec theo ngay
        /// </summary>
        /// <param name="ngayBatDau">ngay bat dau</param>
        /// <param name="ngayKetThuc">ngay ket thuc</param>
        /// <returns>ket qua</returns>
        public static int TinhPhanTramCongViec(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            int kq = 0;
            try
            {
                DateTime now = DateTime.Now;

                // lay TimeSpan cua hai ngay khac nhau
                TimeSpan elapsed = ngayKetThuc.Subtract(ngayBatDau);
                TimeSpan elapsedNow = now.Subtract(ngayBatDau);

                // lay so ngay da qua (kieu int)
                double daysAgo = elapsed.TotalDays == 0.0 ? 1 : elapsed.TotalDays;
                double daysNowAgo = elapsedNow.TotalDays + 1;

                //tinh phan tram
                kq = ((int)daysNowAgo * 100 * 60) / (60);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                kq = 0;
            }
            return kq;
        }

        /// <summary>
        /// Thay the cac ky tu dac biet
        /// </summary>
        /// <param name="strInput">\t|\n|\r</param>
        /// <returns>\\t|\\n|\\r</returns>
        public static string ReplaceExpressions(string strInput)
        {
            string strResult = strInput;
            strResult = System.Text.RegularExpressions.Regex.Replace(strResult, @"\t", "\\t");
            strResult = System.Text.RegularExpressions.Regex.Replace(strResult, @"\n", "\\n");
            strResult = System.Text.RegularExpressions.Regex.Replace(strResult, @"\r", "\\r");
            strResult = System.Text.RegularExpressions.Regex.Replace(strResult, @"""", @"\""");
            return strResult;
        }

        /// <summary>
        /// Kiem tra chuoi Null, "", whitespace, Undefined
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool CheckStringWebUndefinedOrNull(string strInput)
        {
            bool bResult = false;
            if (string.IsNullOrEmpty(strInput))
                bResult = true;
            else
                if (string.IsNullOrWhiteSpace(strInput))
                    bResult = true;
                else
                    if (strInput.Trim().ToLower() == "undefined")
                        bResult = true;
            return bResult;
        }

        /// <summary>
        /// kiem tra du lieu bien session
        /// </summary>
        /// <param name="obj">gia tri bien session</param>
        /// <param name="typeCheck">kieu du lieu</param>
        /// <returns>
        /// return true => gia tri bien session dung dinh dang
        /// return false => gia tri bien session khong dung dinh dang
        /// </returns>
        public static bool CheckSession(object obj, string typeCheck)
        {
            if (obj == null)
            {
                return false;
            }
            switch (typeCheck)
            {
                case "String":
                case "string":
                    if (string.IsNullOrWhiteSpace(obj.ToString()))
                        return false;
                    else
                        return true;
                case "Int64":
                    Int64 outValue_64 = 0;
                    return Int64.TryParse(obj.ToString(), out outValue_64);
                case "int":
                case "Int32":
                    Int32 outValue_32 = 0;
                    return Int32.TryParse(obj.ToString(), out outValue_32);
                case "float":
                    float outValue_float = 0;
                    return float.TryParse(obj.ToString(), out outValue_float);
                case "double":
                case "Double":
                    double outValue_double = 0;
                    return double.TryParse(obj.ToString(), out outValue_double);
                case "Boolean":
                    return Convert.ToBoolean(obj);
                default:
                    return false;
            }
        }

        public static object ConvertValue(object obj, string typeToConvert)
        {
            if (obj == null)
            {
                switch (typeToConvert)
                {
                    case "String":
                        return DBNull.Value.ToString();
                    case "Int64":
                    case "Boolean":
                    case "Decimal":
                    case "Int32":
                    case "List`1":
                        return null;
                    case "byte[]":
                    case "Byte[]":
                        return new byte[0];
                    default:
                        return DBNull.Value;
                }
            }
            switch (typeToConvert)
            {
                case "String":
                    return obj.ToString();

                case "Int64":
                    return Convert.ToInt64(obj);

                case "Boolean":
                    return Convert.ToBoolean(obj);

                case "Decimal":
                    return Convert.ToDecimal(obj);

                case "Int32":
                    return Convert.ToInt32(obj);
                default:
                    return obj;
            }
        }

        public static bool ValidateLinkURL(string strText)
        {
            bool bResult = false;
            string strRegex = @"(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?";
            Regex ge = new Regex(strRegex);
            if (ge.IsMatch(strText))
                return true;
            return bResult;
        }

        /// <summary>
        /// Định dạng lại tên thường trực, thường vụ
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string FormatName(string fullName)
        {
            string result = "";
            string strName = "";
            string[] arrayList = fullName.Split(' ');
            result += arrayList[arrayList.Length - 2].Substring(0, 1).ToUpper() + ".";
            strName = (arrayList[arrayList.Length - 1]).ToLower();
            result += strName.Substring(0, 1).ToUpper() + strName.Substring(1, strName.Length - 1);
            return result;
        }

        public static string MapPath(string path)
        {
            try
            {
                if (HostingEnvironment.IsHosted)
                {
                    //hosted
                    return HostingEnvironment.MapPath(path);
                }

                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return string.Empty;
        }


        public static String UserDomain = "";
        public static String PasswordDomain = "";
        public static String DomainName = "";

        public static string GetTemplateFileName(string fileTemplate, string filepacth)
        {
            string folder = Constants.FolderTempate;

            string fileName = folder + fileTemplate; //"C:\\DBDC\\TEMPLATE\\bhyt0100_maumoi2016.xls"

            string fileName1 = filepacth + fileTemplate;

            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            if (!File.Exists(fileName))   // duong dan toi thu muc "http://localhost:1778/WEB.SOAP/TEMPLATE/"
                DownLoadFile(Constants.FOLDER_TEMPLATECER_URL, fileTemplate, folder);  //folde: "C:\\DBDC\\TEMPLATE\\"   //filetempale :"bhyt0100_maumoi2016.xls"  // 

            if (!File.Exists(fileName1))   // duong dan toi thu muc "http://localhost:1778/WEB.SOAP/TEMPLATE/"
                DownLoadFile(Constants.FOLDER_TEMPLATECER_URL, fileTemplate, filepacth);  //folde: "C:\\DBDC\\TEMPLATE\\"   //filetempale :"bhyt0100_maumoi2016.xls"  // 

            return fileName;
        }


        public static String DownLoadFile(String URL, String filename, string folderlocal)
        {
            string localimage = folderlocal;
            if (!Directory.Exists(localimage))
                Directory.CreateDirectory(localimage);
            localimage = localimage + filename;
            try
            {
                WebClient client = new WebClient();
                NetworkCredential credential = new NetworkCredential(UserDomain, PasswordDomain, DomainName);
                client.Credentials = credential;
                URL = URL.Trim();
                if (URL[URL.Length - 1].ToString() != "/")
                    URL = URL + "/";
                if (!File.Exists(localimage))
                    client.DownloadFile(URL + filename, localimage);

                //string remoteUri = "http://www.contoso.com/library/homepage/images/";
                //string fileName = "ms-banner.gif", myStringWebResource = null;
                //// Create a new WebClient instance.
                //WebClient myWebClient = new WebClient();
                //// Concatenate the domain with the Web resource filename.
                //myStringWebResource = remoteUri + fileName;
                //Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
                //// Download the Web resource and save it into the current filesystem folder.
                //myWebClient.DownloadFile(myStringWebResource, fileName);
                //Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
                ////Console.WriteLine("\nDownloaded file saved in the following file system folder:\n\t" + Application.StartupPath);


            }
            catch (Exception)
            {

            }
            return localimage;
        }


    }
}
