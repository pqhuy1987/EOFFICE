using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;
using System.Collections;

namespace ExportHelper.Repository
{
  public class BaseExcel
  {
    
    public HSSFWorkbook Workbook { get; set; }
    public Dictionary<String, ICellStyle> CellStyles { get; set; }

    //File excel template dùng để export theo template
    public string ExcelTemplate { get; set; }
    public short RowHeaderHeight { get; set; }
    public short RowHeight { get; set; }
    public short ColumnWidth { get; set; }

    // public string DialogFilter = "Excel|*.xls|Excel 2010|*.xlsx";
    public string DialogFilter = "Excel|*.xls";
    /// <summary>
    /// Khởi tạo các thuộc tính cho file excel
    /// </summary>
    private void InitProperties()
    {
      var property = PropertySetFactory.CreateDocumentSummaryInformation();
      property.Company = "iSoftware";
      Workbook.DocumentSummaryInformation = property;
      SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
      si.Subject = "iSoftware";
      Workbook.SummaryInformation = si;

      RowHeaderHeight = 400;
      RowHeight = 350;
      ColumnWidth = 25 * 256;
    }


    /// <summary>
    /// Constructor dùng cho xuất excel bình thường
    /// </summary>
    public BaseExcel()
    {
      
      Workbook = new HSSFWorkbook();
      InitProperties();
      CellStyles = CreateCellStyles(Workbook);
    }


    /// <summary>
    /// Constructor dùng cho xuất file excel theo template
    /// </summary>
    /// <param name="excelTemplate">File excel template</param>
    public BaseExcel(string excelTemplate)
    {
    
      ExcelTemplate = excelTemplate;
      var file = new FileStream(ExcelTemplate, FileMode.Open, FileAccess.Read);
      Workbook = new HSSFWorkbook(file);
      InitProperties();
      CellStyles = CreateCellStyles(Workbook);
    }


    /// <summary>
    /// Phương thức create sheet mới trong excel và export dữ liệu trong bảng vào sheet luôn
    /// </summary>
    /// <param name="source"></param>
    public void CreateSheet(DataTable source)
    {
      ISheet sheet = Workbook.CreateSheet(source.TableName);

      // Khởi tạo row header
      IRow rowHeader = sheet.CreateRow(0);
      rowHeader.Height = RowHeaderHeight;
      for (int i = 0; i < source.Columns.Count; i++)
      {
        sheet.SetColumnWidth(i, ColumnWidth);
        rowHeader.CreateCell(i).SetCellValue(source.Columns[i].ColumnName);
        rowHeader.GetCell(i).CellStyle = CellStyles[CELL_HEADER_CENTER];
      }

      // Xuất dữ liệu
      int x = 1;
      foreach (DataRow currentRow in source.Rows)
      {
        IRow row = sheet.CreateRow(x);
        row.Height = RowHeight;
        for (int j = 0; j < source.Columns.Count; j++)
        {
          row.CreateCell(j).SetCellValue(currentRow[j].ToString());
          row.GetCell(j).CellStyle = CellStyles[CELL_NOMAL_LEFT];
        }
        x = x + 1;
      }
    }


    /// <summary>
    /// Phương thức export header của bảng ra file excel
    /// </summary>
    /// <param name="sheet">Sheet dùng để export</param>
    /// <param name="columns">Các column cần export header</param>
    /// <param name="startRowIndex">Export header tại vị trí row index</param>
    /// <param name="startColumnIndex">Export header tại vị trí column index</param>
  

    public void CreateHeader(ref ISheet sheet, DataColumnCollection columns, int startRowIndex, int startColumnIndex)
    {
      IRow header = sheet.CreateRow(startRowIndex);
      header.Height = RowHeaderHeight;
      for (int i = 0; i < columns.Count; i++)
      {
        header.CreateCell(startColumnIndex + i).SetCellValue(columns[i].Caption);
        header.GetCell(startColumnIndex + i).CellStyle = CellStyles[CELL_HEADER_CENTER];
      }
    }
    /// <summary>
    /// Phương thức export nội dung của bảng ra file excel theo kiểu CreateRow
    /// </summary>
    /// <param name="sheet">Sheet dùng để export</param>
    /// <param name="table">Bảng dữ liệu cần export</param>
    /// <param name="startRowIndex">Export row đầu tiên của bảng tại vị trí row index</param>
    /// <param name="startColumnIndex">Export cột đầu tiên của tại vị trí column index</param>
    public void CreateContent(ref ISheet sheet, DataTable table, int startRowIndex, int startColumnIndex)
    {
      int x = 0;
      foreach (DataRow currentRow in table.Rows)
      {
        IRow row = sheet.CreateRow(startRowIndex + x);
        row.Height = RowHeight;

        for (int j = 0; j < table.Columns.Count; j++)
        {
          int index = startColumnIndex + j;
          object value = currentRow[j] ?? " ";
          row.CreateCell(index).SetCellValue(value.ToString());
          row.GetCell(index).CellStyle = CellStyles[CELL_NOMAL_LEFT];
        }
        x = x + 1;
      }
    }
    

    private ICellStyle CellHeaderStyle(IWorkbook wb, HorizontalAlignment alignment)
    {
      ICellStyle cell = CreateBorderedCellStyle(wb);
      cell.VerticalAlignment = VerticalAlignment.CENTER;
      cell.Alignment = alignment;
      cell.WrapText = true;
      return cell;
    }
   


    public void CreateContent(ref ISheet sheet, ArrayList arrcolumn, DataTable table, int startRowIndex, int startColumnIndex)
    {
      for (int i = 0; i < table.Rows.Count; i++)
      {
        IRow currentRow = sheet.CreateRow(startRowIndex + i);
        currentRow.Height = RowHeight;

        for (int j = 0; j < arrcolumn.Count; j++)
        {
          //  currentRow.GetCell(startColumnIndex + j).CellStyle = CellStyles[CELL_NOMAL_LEFT];
          if (table.Columns.Contains(arrcolumn[j].ToString()))
          {
            var value = table.Rows[i][arrcolumn[j].ToString()].ToString();
            currentRow.CreateCell(startColumnIndex + j).SetCellValue(value.ToString());

          }
          else
            currentRow.CreateCell(startColumnIndex + j).SetCellValue("");
        }
      }
    }

    public void CreateContentphunu(ref ISheet sheet, DataSet table, int startRowIndex, int startColumnIndex, string tablename, Dictionary<String, ICellStyle> styles)
    {
        for (int i = 0; i < table.Tables[tablename].Rows.Count; i++)
        {
            IRow currentRow = sheet.CreateRow(startRowIndex + i);
            currentRow.Height = RowHeight;
            for (int j = 0; j < table.Tables[tablename].Columns.Count; j++)
            {
                if (table.Tables[tablename].Columns.Contains(table.Tables[tablename].Columns[j].ToString()))
                {
                    var value = table.Tables[tablename].Rows[i][table.Tables[tablename].Columns[j].ToString()].ToString();
                    currentRow.CreateCell(startColumnIndex + j).SetCellValue(value.ToString());
                }
                else
                    currentRow.CreateCell(startColumnIndex + j).SetCellValue("");

                if (true)
                {
                    // currentRow.GetCell(i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                    currentRow.GetCell(startColumnIndex + j).CellStyle = styles["mystyle"];
                }
            }
        }
    }
    
    /// <summary>
    /// Phương thức export nội dung của bảng ra file excel theo kiểu InsertRow
    /// </summary>
    /// <param name="sheet">Sheet dùng để export</param>
    /// <param name="table">Bảng dữ liệu cần export</param>
    /// <param name="startRowIndex">Export row đầu tiên của bảng tại vị trí row index</param>
    /// <param name="startColumnIndex">Export cột đầu tiên của tại vị trí column index</param>
    public void InsertContent(ref ISheet sheet, DataTable table, int startRowIndex, int startColumnIndex)
    {
      IRow row = sheet.CreateRow(startRowIndex);
      row.Height = RowHeight;
      for (int j = 0; j < table.Columns.Count; j++)
      {
        row.CreateCell(startColumnIndex + j);
        row.GetCell(startColumnIndex + j).CellStyle = CellStyles[CELL_NOMAL_LEFT];
      }
      for (int i = 1; i < table.Rows.Count; i++)
      {
          sheet.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
      }
      int x = 0;
      foreach (DataRow currentRow in table.Rows)
      {
        sheet.GetRow(startRowIndex + x).Height = RowHeight;
        for (int j = 0; j < table.Columns.Count; j++)
        {
          sheet.GetRow(startRowIndex + x).GetCell(startColumnIndex + j).SetCellValue(currentRow[j].ToString());
        }
        x += 1;
      }
      // Remove Row: sheet1.RemoveRow(sheet1.GetRow(rowIndex));
    }


    /// <summary>
    /// Phương thức lưu file excel
    /// </summary>
    /// <param name="fileName">File excel cần lưu</param>
    public void SaveExport(string fileName)
    {
      try
      {
        using (var file = new FileStream(fileName, FileMode.Create))
        {
          Workbook.Write(file);
          file.Close();
        }
        //DialogResult result = MessageBox.Show("Xuất dữ liệu thành công, bạn có muốn mở file này không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //if (result == DialogResult.Yes)
          //FileDialogs.OpenFile(fileName);
        //MessageBox.Show("Xuất dữ liệu thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception)
      {
       // MessageBox.Show("Lỗi xuất dữ liệu!", "Thông Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }



    #region Create a library of cell styles

    public string CELL_HEADER_LEFT = "CELL_HEADER_LEFT";
    public string CELL_HEADER_CENTER = "CELL_HEADER_CENTER";
    public string CELL_HEADER_RIGHT = "CELL_HEADER_RIGHT";

    public string CELL_NOMAL_LEFT = "CELL_NOMAL_LEFT";
    public string CELL_NOMAL_CENTER = "CELL_NOMAL_CENTER";
    public string CELL_NOMAL_RIGHT = "CELL_NOMAL_RIGHT";

    public string CELL_NOMAL_BOLD_LEFT = "CELL_NOMAL_BOLD_LEFT";
    public string CELL_NOMAL_BOLD_CENTER = "CELL_NOMAL_BOLD_CENTER";
    public string CELL_NOMAL_BOLD_RIGHT = "CELL_NOMAL_BOLD_RIGHT";




    private Dictionary<String, ICellStyle> CreateCellStyles(IWorkbook wb)
    {
      var styles = new Dictionary<String, ICellStyle>();
      ICellStyle headerLeft = HeaderCellStyle(wb, HorizontalAlignment.LEFT);
      ICellStyle headerCenter = HeaderCellStyle(wb, HorizontalAlignment.CENTER);
      ICellStyle headerRight = HeaderCellStyle(wb, HorizontalAlignment.RIGHT);

      ICellStyle nomalLeft = NomalCellStyle(wb, HorizontalAlignment.LEFT);
      ICellStyle nomalCenter = NomalCellStyle(wb, HorizontalAlignment.CENTER);
      ICellStyle nomalRight = NomalCellStyle(wb, HorizontalAlignment.RIGHT);

      ICellStyle nomalBoldLeft = NomalBoldCellStyle(wb, HorizontalAlignment.LEFT);
      ICellStyle nomalBoldCenter = NomalBoldCellStyle(wb, HorizontalAlignment.CENTER);
      ICellStyle nomalBoldRight = NomalBoldCellStyle(wb, HorizontalAlignment.RIGHT);

      styles.Add(CELL_HEADER_LEFT, headerLeft);
      styles.Add(CELL_HEADER_CENTER, headerCenter);
      styles.Add(CELL_HEADER_RIGHT, headerRight);

      styles.Add(CELL_NOMAL_LEFT, nomalLeft);
      styles.Add(CELL_NOMAL_CENTER, nomalCenter);
      styles.Add(CELL_NOMAL_RIGHT, nomalRight);

      styles.Add(CELL_NOMAL_BOLD_LEFT, nomalBoldLeft);
      styles.Add(CELL_NOMAL_BOLD_CENTER, nomalBoldCenter);
      styles.Add(CELL_NOMAL_BOLD_RIGHT, nomalBoldRight);

      return styles;
    }


    private ICellStyle HeaderCellStyle(IWorkbook wb, HorizontalAlignment horizontalAlignment)
    {
      ICellStyle cell = CreateBorderedCellStyle(wb);
      IFont font = wb.CreateFont();
      font.FontName = "Times New Roman";
      font.FontHeightInPoints = 12;
      font.Boldweight = (short)FontBoldWeight.BOLD;
      font.Color = HSSFColor.BLACK.index;
      cell.Alignment = horizontalAlignment;
      cell.VerticalAlignment = VerticalAlignment.CENTER;
      cell.FillForegroundColor = IndexedColors.LIGHT_CORNFLOWER_BLUE.Index;
      cell.FillPattern = FillPatternType.SOLID_FOREGROUND;
      //cell.ShrinkToFit = true;
      cell.SetFont(font);
      return cell;
    }


    private ICellStyle NomalCellStyle(IWorkbook wb, HorizontalAlignment horizontalAlignment)
    {
      ICellStyle cell = CreateBorderedCellStyle(wb);
      cell.VerticalAlignment = VerticalAlignment.CENTER;
      cell.Alignment = horizontalAlignment;
      cell.WrapText = true;
      cell.ShrinkToFit = true;

      IFont font = wb.CreateFont();
      font.FontName = "Times New Roman";
      font.FontHeightInPoints = 11;
      font.Boldweight = (short)FontBoldWeight.NORMAL;
      font.Color = HSSFColor.BLACK.index;
      cell.SetFont(font);

      return cell;
    }

    private ICellStyle NomalBoldCellStyle(IWorkbook wb, HorizontalAlignment horizontalAlignment)
    {
      ICellStyle cell = CreateBorderedCellStyle(wb);
      cell.VerticalAlignment = VerticalAlignment.CENTER;
      cell.Alignment = horizontalAlignment;
      cell.WrapText = true;
      cell.ShrinkToFit = true;

      IFont font = wb.CreateFont();
      font.FontName = "Times New Roman";
      font.FontHeightInPoints = 11;
      font.Boldweight = (short)FontBoldWeight.BOLD;
      font.Color = HSSFColor.BLACK.index;
      cell.SetFont(font);

      return cell;
    }

    public ICellStyle MergeCellStyle(IWorkbook wb, HorizontalAlignment horizontalAlignment)
    {
        ICellStyle cell = CreateBorderedCellStyle(wb);
        IFont font = wb.CreateFont();
        font.FontName = "Times New Roman";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;
        font.Color = HSSFColor.BLACK.index;
        cell.Alignment = horizontalAlignment;
        cell.VerticalAlignment = VerticalAlignment.CENTER;
        cell.FillForegroundColor = IndexedColors.LIGHT_CORNFLOWER_BLUE.Index;
        cell.FillPattern = FillPatternType.SOLID_FOREGROUND;
        //cell.ShrinkToFit = true;
        cell.SetFont(font);
        return cell;
    }


    public ICellStyle CreateBorderedCellStyle(IWorkbook wb)
    {
      ICellStyle cell = wb.CreateCellStyle();
      cell.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
      cell.RightBorderColor = (IndexedColors.BLACK.Index);
      cell.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
      cell.BottomBorderColor = (IndexedColors.BLACK.Index);
      cell.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
      cell.LeftBorderColor = (IndexedColors.BLACK.Index);
      cell.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
      cell.TopBorderColor = (IndexedColors.BLACK.Index);
      return cell;
    }

    #endregion


    #region # Examples #

    ///// <summary>
    ///// Phương thức test các thuộc tính excel -> OK!
    ///// </summary>
    //public void TestExport001(List<SetCellValue> setCellValues)
    //{
    //  string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
    //  if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
    //  ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
    //  try
    //  {
    //    foreach (var item in setCellValues)
    //    {
    //      IRow row = sheet1.CreateRow(item.RowIndex);
    //      row.HeightInPoints = 45; // Set height của row trong file excel

    //      ICell cell = row.CreateCell(item.ColumnIndex);
    //      cell.SetCellValue(item.Value);

    //      ICellStyle style = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
    //      style.WrapText = true;
    //      cell.CellStyle = style;

    //      sheet1.AutoSizeColumn(item.ColumnIndex); // Set AutoSizeColumn của column trong file excel
    //    }
    //    _baseExcel.SaveExport(fileName);
    //  }
    //  catch (Exception ex)
    //  {
    //    _baseExcel.Logger.Error(ex);
    //    MessageBox.Show("Lỗi xuất dữ liệu!", "Thông Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //  }
    //}


    #endregion

  }
}
