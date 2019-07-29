using NHG.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace NHG.Core.Functions
{
    public class FunctionsImage
    {
        private static Log4Net logger = new Log4Net(typeof(FunctionsFile));

        /// <summary>
        /// Get image format with image name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormat(string file)
        {
            ImageFormat result = ImageFormat.Jpeg;
            string extension = file.Substring(file.LastIndexOf('.') + 1);
            switch (extension)
            {
                case "gif":
                    result = ImageFormat.Gif;
                    break;
                case "png":
                    result = ImageFormat.Png;
                    break;
                case "bmp":
                    result = ImageFormat.Bmp;
                    break;
                case "tiff":
                    result = ImageFormat.Tiff;
                    break;
                case "x-icon":
                    result = ImageFormat.Icon;
                    break;
                case "jpg":
                    result = ImageFormat.Jpeg;
                    break;
                default:
                    result = ImageFormat.Jpeg;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Resize hinh anh
        /// </summary>
        /// <param name="PathFileImage"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static Image ScaleByPercent(string PathFileImage, int iWidth, int iHeight)
        {
            Bitmap image = Image.FromFile(PathFileImage) as Bitmap;
            try
            {
                //tính kích thước cho ảnh mới theo tỷ lệ đưa vào 
                int resizedW = (int)(iWidth);
                int resizedH = (int)(iHeight);
                //tạo 1 ảnh Bitmap mới theo kích thước trên 
                Bitmap bmp = new Bitmap(resizedW, resizedH);
                //tạo 1 graphic mới từ 
                Graphics graphic = Graphics.FromImage((Image)bmp);
                //vẽ lại ảnh ban đầu lên bmp theo kích thước mới 
                graphic.DrawImage(image, 0, 0, resizedW, resizedH);
                //giải phóng tài nguyên mà graphic đang giữ 
                graphic.Dispose();
                image.Dispose();
                image = null;
                //return the image 
                return (Image)bmp;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return image = Image.FromFile(PathFileImage) as Bitmap;
            }
        }

        /// <summary>
        /// chuyen hinh thanh mang byte[]
        /// </summary>
        /// <param name="strFileName">ten file(duong dan file)</param>
        /// <returns></returns>
        public static byte[] ConvertImageToByteArray(string strFileName)
        {
            try
            {
                FileStream fInforr = new FileStream(strFileName, FileMode.Open);
                MemoryStream ms = new MemoryStream();
                Image imageIn = Image.FromStream(fInforr);
                imageIn.Save(ms, GetImageFormat(strFileName));
                byte[] bResult = ms.ToArray();
                imageIn.Dispose();
                ms.Dispose();
                fInforr.Dispose();
                return bResult;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Chuyen hinh anh sang chuoi base64
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static string ConvertImageToBase64String(string strFileName)
        {
            try
            {
                FileStream fInforr = new FileStream(strFileName, FileMode.Open);
                MemoryStream ms = new MemoryStream();
                Image imageIn = Image.FromStream(fInforr);
                imageIn.Save(ms, GetImageFormat(strFileName));
                byte[] bResult = ms.ToArray();
                imageIn.Dispose();
                ms.Dispose();
                fInforr.Dispose();
                return Convert.ToBase64String(bResult, Base64FormattingOptions.None); ;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// lay image icon theo duong dan file hinh
        /// </summary>
        /// <param name="strPath">ten</param>
        /// <returns>imag.png</returns>
        public static string ReturnImageIconWithPath(string strPath)
        {
            string fileExtension = Path.GetExtension(strPath);
            return GetExtensionFile(fileExtension);
        }

        /// <summary>
        /// lay image icon theo ten file hinh (ab.png)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>imag.png</returns>
        public static string ReturnImageIconWithName(string filename)
        {
            string fileExtension = filename.Substring(filename.LastIndexOf('.'));
            return GetExtensionFile(fileExtension);
        }

        /// <summary>
        /// lay hinh anh icon theo fileExtension
        /// </summary>
        /// <param name="fileExtension">fileExtension</param>
        /// <returns></returns>
        public static string GetExtensionFile(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".png":
                case ".tiff":
                case ".tif":
                case ".gif":
                case ".jpg":
                case "jpeg":
                case ".bmp":
                case ".dwg":
                    return "image.png";
                case ".htm":
                case ".html":
                case ".log":
                    return "html.png";
                case ".txt":
                    return "document.png";
                case ".doc":
                case ".docx":
                    return "word.png";
                case ".xls":
                case ".xlsx":
                    return "excel.png";
                case ".zip":
                case ".rar":
                case ".7zip":
                    return "zip.png";
                case ".pdf":
                    return "pdf.png";
                case ".ppt":
                case ".pptx":
                    return "powerpoint.png";
                default:
                    return "document.png";
            }
        }
    }
}
