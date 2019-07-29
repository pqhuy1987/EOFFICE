using NHG.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace NHG.Core.Functions
{
    public class FunctionsFile
    {
        private static Log4Net logger = new Log4Net(typeof(FunctionsFile));
        /// <summary>
        /// lay chuoi danh dang file
        /// </summary>
        /// <param name="file">ten file hinhanh.png</param>
        /// <returns>
        ///    kq: image/png
        /// </returns>
        public static string GetStringFileContentType(string strPath)
        {
            string extension = Path.GetExtension(strPath);
            return ReturnFileContentType(extension);
        }

        private static string ReturnFileContentType(string extension)
        {
            if (extension != null)
            {
                switch (extension)
                {
                    case ".png":
                        return "image/png";
                    case ".tiff":
                    case ".tif":
                        return "image/tiff";
                    case ".gif":
                        return "image/gif";
                    case ".jpg":
                    case "jpeg":
                        return "image/jpeg";
                    case ".bmp":
                        return "image/bmp";
                    case ".dwg":
                        return "image/vnd.dwg";
                    case ".htm":
                    case ".html":
                    case ".log":
                        return "text/HTML";
                    case ".txt":
                        return "text/plain";
                    case ".doc":
                        return "application/ms-word";
                    case ".docx":
                        return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    case ".asf":
                        return "video/x-ms-asf";
                    case ".avi":
                        return "video/avi";
                    case ".zip":
                        return "application/zip";
                    case ".xls":
                    case ".xlsx":
                    case ".csv":
                        return "application/vnd.ms-excel";
                    case ".wav":
                        return "audio/wav";
                    case ".mp3":
                        return "audio/mpeg3";
                    case ".mpg":
                    case "mpeg":
                        return "video/mpeg";
                    case ".rtf":
                        return "application/rtf";
                    case ".asp":
                        return "text/asp";
                    case ".pdf":
                        return "application/pdf";
                    case ".fdf":
                        return "application/vnd.fdf";
                    case ".ppt":
                        return "application/mspowerpoint";
                    case ".msg":
                        return "application/msoutlook";
                    case ".xml":
                    case ".sdxl":
                        return "application/xml";
                    case ".xdp":
                        return "application/vnd.adobe.xdp+xml";
                    default:
                        return "application/octet-stream";
                }
            }
            else
                return "application/octet-stream";
        }

        /// <summary>
        /// Tao file dang text: doc, txt, html
        /// </summary>
        /// <param name="path">duong dan url</param>
        /// <param name="value">noi dung file</param>
        public static void WriteFile(string path, string value)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Create);
                using (StreamWriter _sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    _sw.WriteLine(value);
                    stream = null;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            try
            {
                System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(path);

                // Get a DirectorySecurity object that represents the 
                // current security settings.
                DirectorySecurity dSecurity = dInfo.GetAccessControl();

                // Add the FileSystemAccessRule to the security settings. 
                dSecurity.AddAccessRule(
                    new FileSystemAccessRule(
                        new System.Security.Principal.NTAccount("Everyone"),
                        FileSystemRights.DeleteSubdirectoriesAndFiles,
                        AccessControlType.Allow
                    )
                );

                // Set the new access settings.
                dInfo.SetAccessControl(dSecurity);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Lay size tap tin
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns>10KB, 100MB</returns>
        public static string GetFileSizeInFolder(string pathFile)
        {
            FileInfo fi = new FileInfo(pathFile);
            if (fi.Exists)
            {
                return SetFileSize((double)fi.Length);
            }
            return "";
        }

        public static string GetFileSize(string pathFile)
        {
            FileInfo fi = new FileInfo(pathFile);
            if (fi.Exists)
            {
                return SetFileSize((double)fi.Length);
            }
            return "";
        }

        /// <summary>
        /// Chuyen 1000 sang dinh 1KB 
        /// </summary>
        /// <param name="fileLength"></param>
        /// <returns></returns>
        public static string SetFileSize(double fileLength)
        {
            if (fileLength > 1024 * 1024)
                return (Math.Round(fileLength * 100 / (1024 * 1024)) / 100).ToString() + "MB";
            else
                return (Math.Round(fileLength * 100 / 1024) / 100).ToString() + "KB";
        }

        public static string GetPathFile(string date, string userid)
        {
            string strPathResult = "";

            return strPathResult;
        }

        /// <summary>
        /// Chuyen file thanh mang byte
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadFileToByte(string path)
        {
            byte[] byteResult = null;
            try
            {
                //using (FileStream MyFileStream = new FileStream(path, FileMode.Open))
                //{
                //    // Total bytes to read: 
                //    long size;
                //    size = MyFileStream.Length;
                //    byteResult = new byte[size];
                //    MyFileStream.Read(byteResult, 0, int.Parse(MyFileStream.Length.ToString()));
                //}
                byteResult = File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return byteResult;
        }

        /// <summary>
        /// ReadFileFromByte
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="fileName"></param>
        public static void ReadFileFromByte(byte[] FileData, string fileName)
        {
            try
            {
                //Write file data to selected file.
                FileInfo fi = new FileInfo(fileName);
                if (!fi.Directory.Exists)
                    Directory.CreateDirectory(fi.Directory.ToString());
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    fs.Write(FileData, 0, FileData.Length);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public static void MoveFileBetween2Folder(string pathSource, string pathDest, string files)
        {
            if (Directory.Exists(pathSource))
            {
                if (!Directory.Exists(pathDest))
                {
                    Directory.CreateDirectory(pathDest);
                }
                DirectoryInfo di = new DirectoryInfo(pathSource);
                string[] fileNames = files.Split(',');
                for (int i = 0; i < fileNames.Length; i++)
                {
                    FileInfo fi = new FileInfo(di.FullName + fileNames[i]);
                    if (fi.Exists)
                    {
                        FileInfo fiExists = new FileInfo(pathDest + fileNames[i]);
                        if (fiExists.Exists)
                        {
                            fiExists.Delete();
                        }
                        fi.MoveTo(pathDest + fileNames[i]);
                    }
                }
            }
        }
    }
}
