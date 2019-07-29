using System;
using System.Diagnostics;


namespace ExportHelper.Repository
{
  /// <summary>
  /// Class tiện ích làm việc với các hộp thoại
  /// </summary>
  public static class FileDialogs
  {

    /// <summary>
    /// Phương thức mở file và trả về file name
    /// </summary>
    /// <param name="filter">VD: filter = "File image|*.jpg; *.jpeg; *.png; *.bmp;*.JPG; *.JPEG; *.PNG; *.BMP"</param>
    /// <returns>NULL || FileName</returns>
    public static string OpenFiles(string filter)
    {
        return "1";
    }

    /// <summary>
    /// Hộp thoại lưu file và trả về file name
    /// </summary>
    /// <param name="filter">VD: filter = "File image|*.jpg; *.jpeg; *.png; *.bmp;*.JPG; *.JPEG; *.PNG; *.BMP"</param>
    /// <param name="defaultExt">VD: txt</param>
    /// <returns>NULL || FileName</returns>
    public static string SaveFiles(string filter, string defaultExt)
    {
        //var sfd = new SaveFileDialog
        //{
        //    Title = "Save as...",
        //    InitialDirectory = @"D:\",
        //    CheckFileExists = false,
        //    CheckPathExists = true,
        //    DefaultExt = defaultExt,
        //    Filter = filter,
        //    FilterIndex = 2,
        //    RestoreDirectory = true
        //};
        //DialogResult result = sfd.ShowDialog();
        //return (result == DialogResult.OK) ? sfd.FileName : null;
        return "1";
    }

    /// <summary>
    /// Hộp thoại chọn folder
    /// </summary>
    /// <returns>Folder được chọn || null</returns>
    public static string SelectFolder()
    {
        return "1";
    }

    /// <summary>
    /// Phương thức mở một file tồn tại
    /// </summary>
    /// <param name="fileName"></param>
    public static void OpenFile(string fileName)
    {
      var process = new Process
                    {
                      StartInfo = {FileName = fileName, Verb = "Open", WindowStyle = ProcessWindowStyle.Maximized}
                    };
      try
      {
        process.Start();
      }
      catch (Exception)
      {
      }
    }

  }
}
