/****************************************************************************
 * 
 *  INTERFACE ĐỊNH NGHĨA CÁC HÀM EXPORT DỮ LIỆU RA FILE EXCEL, NẾU CÓ SỬA CÁCH EXPORT 
 *  DỮ LIỆU THÌ PHẢI TUÂN THEO CÁC HÀM ĐỊNH NGHĨA NÀY VÌ NÓ ĐANG ĐƯỢC SỬA DỤNG
 * 
 ****************************************************************************/

using System.Data;
using System.Collections.Generic;
using ExportHelper.Repository;

namespace ExportHelper.IRepository
{
  public interface IExportToExcel
  {

    /// <summary>
    /// Phương thức export dữ liệu từ một table ra file excel
    /// </summary>
    /// <param name="table">DataTable cần export</param>
    void TableExport(DataTable table);

    /// <summary>
    /// Phương thức export dữ liệu từ một table ra file excel, với tiêu đề
    /// </summary>
    /// <param name="dataSource">DataTable cần export</param>
    /// <param name="title">Tiêu đề cần export</param>
    void TableExport(DataTable dataSource, string title);

    /// <summary>
    /// Phương thức export nhiểu table ra file excel, mỗi table tương ứng với 1 sheet, tên sheet = tên table
    /// </summary>
    /// <param name="listTables">Danh sách các table cần export</param>
    void TableExport(List<DataTable> listTables);


    /// <summary>
    /// Phương thức export dữ liệu từ một DataSet ra file excel, mỗi table trong DataSet tương ứng với 1 sheet, tên sheet = tên table
    /// </summary>
    /// <param name="dataSet">DataSet cần export</param>
    void DataSetExport(DataSet dataSet);


    /// <summary>
    /// Phương thức export dữ liệu từ một DataGridView ra file excel
    /// </summary>
    /// <param name="gridView">DataGridView cần export, có thể dùng 1 trong 3 loại sau:
    /// System.Windows.Forms.DataGridView
    /// DT.CustomControls.DTDataGridView
    /// DT.CustomControls.DtDataGridViewChk
    /// </param>


    /// <summary>
    /// Phương thức export dữ liệu ra file excel dựa trên file template
    /// </summary>
    /// <param name="table">Bảng dữ liệu cần export</param>
    /// <param name="startRowIndex">Index của dòng bắt đầu xuất dữ liệu trong file excel</param>
    /// <param name="columnIndexPairs">Tên column trong GridView và index của column trong file excel</param>
    /// <param name="setCellValues">
    /// - Dùng để insert dữ liệu vào từng cell cụ thể trong file excel
    ///  *Chú ý: trước khi dùng thuộc tính SetCellValue thì phải kích hoạt các cell này trong file excel 
    ///          trước bằng cách bôi đen vùng template và chọn boder.
    /// (SetCellValues trước khi export GridView)
    /// </param>
    void TemplateExport(DataTable table, int startRowIndex, Dictionary<string, int> columnIndexPairs, List<SetCellValue> setCellValues);

    /// <summary>
    /// Phương thức export dữ liệu ra file excel dựa trên file template
    /// </summary>
    /// <param name="table">Bảng dữ liệu cần export</param>
    /// <param name="isExportHeader">TRUE = Export cột header; FALSE = Không export cột header
    /// *Chú ý: Nếu export header thì sẽ export table.Columns["columnName"].Caption  </param>
    /// <param name="startRowIndex">Row index trong file excel (dùng để export cell đầu tiên của table)</param>
    /// <param name="startColumnIndex">Column index trong file excel (dùng để export cell đầu tiên của table)</param>
    /// <param name="setCellValues">
    /// - Dùng để insert dữ liệu vào từng cell cụ thể trong file excel
    ///  *Chú ý: trước khi dùng thuộc tính SetCellValue thì phải kích hoạt các cell này trong file excel 
    ///          trước bằng cách bôi đen vùng template và chọn boder.
    /// </param>
    /// <param name="isSetValuesAfterExportData">
    /// TRUE = setCellValues sau khi export dữ liệu (mặc định)
    /// FALSE = setCellValues trước export dữ liệu
    /// </param>
    void TemplateExport(DataTable table, bool isExportHeader, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues, bool isSetValuesAfterExportData = true);


    /// <summary>
    /// Phương thức export dữ liệu ra file excel dựa trên file template
    /// </summary>
    /// <param name="table">Bảng dữ liệu cần export</param>
    /// <param name="isExportHeader">TRUE = Export cột header; FALSE = Không export cột header
    /// *Chú ý: Nếu export header thì sẽ export table.Columns["columnName"].Caption  </param>
    /// <param name="option">TRUE = (Export theo add row); FALSE = (Export theo insert row)</param>
    /// <param name="startRowIndex">Index của row bắt đầu xuất dữ liệu trong file excel</param>
    /// <param name="startColumnIndex">Index của column bắt đầu xuất dữ liệu trong file excel</param>
    /// <param name="setCellValues">
    /// - Dùng để insert dữ liệu vào từng cell cụ thể trong file excel
    ///  *Chú ý: trước khi dùng thuộc tính SetCellValue thì phải kích hoạt các cell này trong file excel 
    ///          trước bằng cách bôi đen vùng template và chọn boder.
    /// </param>
    /// <param name="isSetValuesAfterExportData">
    /// TRUE = setCellValues sau khi export dữ liệu (mặc định)
    /// FALSE = setCellValues trước export dữ liệu
    /// </param>
    void TemplateExport(DataTable table, bool isExportHeader, bool option, int startRowIndex, int startColumnIndex,
      List<SetCellValue> setCellValues, bool isSetValuesAfterExportData = true);

    /// <summary>
    /// Phương thức export dữ liệu ra file excel dựa trên file template
    /// </summary>
    /// <param name="view">GridView cần export, là 1 trong 3 loại gridview sau:
    /// System.Windows.Forms.DataGridView
    /// DT.CustomControls.DTDataGridView
    /// DT.CustomControls.DtDataGridViewChk
    /// </param>
    /// <param name="columns">Danh sách tên các cột của GridView sẽ export</param>
    /// <param name="startRowIndex">Index của dòng bắt đầu xuất dữ liệu trong file excel</param>
    /// <param name="startColumnIndex">Index của cột bắt đầu xuất dữ liệu trong file excel</param>
    /// <param name="isAddRow">TRUE = (Export theo add row); FALSE = (Export theo insert row)</param>
    /// <param name="setCellValues">
    /// - Dùng để insert dữ liệu vào từng cell cụ thể trong file excel
    ///  *Chú ý: trước khi dùng thuộc tính SetCellValue thì phải kích hoạt các cell này trong file excel 
    ///          trước bằng cách bôi đen vùng template và chọn boder.
    /// (SetCellValues trước khi export GridView)
    /// </param>
    
    
    /// <summary>
    /// Phương thức export dữ liệu ra file excel dựa trên file template
    /// </summary>
    /// <param name="view">GridView cần export, là 1 trong 3 loại gridview sau:
    /// System.Windows.Forms.DataGridView
    /// DT.CustomControls.DTDataGridView
    /// DT.CustomControls.DtDataGridViewChk
    /// </param>
    /// <param name="startRowIndex">Index của dòng bắt đầu xuất dữ liệu trong file excel</param>
    /// <param name="columnIndexPairs">Tên column trong GridView và index của column trong file excel</param>
    /// <param name="setCellValues">
    /// - Dùng để insert dữ liệu vào từng cell cụ thể trong file excel
    ///  *Chú ý: trước khi dùng thuộc tính SetCellValue thì phải kích hoạt các cell này trong file excel 
    ///          trước bằng cách bôi đen vùng template và chọn boder.
    /// (SetCellValues trước khi export GridView)
    /// </param>
  

  }

}
