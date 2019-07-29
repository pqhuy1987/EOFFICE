using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExportHelper.IRepository;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections;
using NHG.Core.Functions;

namespace ExportHelper.Repository
{
  /// <summary>
  /// Phương thức chứa các hàm tiện ích export dữ liệu ra file excel
  /// </summary>
  public class ExportToExcel : IExportToExcel
  {
    private readonly BaseExcel _baseExcel;

    public ExportToExcel()
    {
      _baseExcel = new BaseExcel();
    }


    public ExportToExcel(string excelTemplate)
    {
      _baseExcel = new BaseExcel(excelTemplate);
    }


    public void TableExport(DataTable dataSource)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (fileName == null) return;
      _baseExcel.CreateSheet(dataSource);
      _baseExcel.SaveExport(fileName);
    }
    /// <summary>
    /// Xuat table , tieu de
    /// </summary>
    /// <param name="dataSource"></param>
    /// <param name="title"></param>
    public void TableExport(DataTable dataSource, string title)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
      ISheet sheet = _baseExcel.Workbook.GetSheetAt(0);

      IRow row0 = sheet.CreateRow(0);
      for (int i = 0; i < dataSource.Columns.Count; i++)
      {
        row0.CreateCell(i);
        sheet.SetColumnWidth(i, _baseExcel.ColumnWidth);
      }
      row0.GetCell(0).SetCellValue(title);
      row0.GetCell(0).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_HEADER_CENTER];

      sheet.AddMergedRegion(new CellRangeAddress(0, 2, 0, dataSource.Columns.Count - 1));

      _baseExcel.CreateHeader(ref sheet, dataSource.Columns, 3, 0);
      _baseExcel.CreateContent(ref sheet, dataSource, 4, 0);
      _baseExcel.SaveExport(fileName);
    }


    public void TableExport(List<DataTable> tables)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (fileName == null) return;
      foreach (DataTable table in tables)
      {
        _baseExcel.CreateSheet(table);
      }
      _baseExcel.SaveExport(fileName);
    }


    public void DataSetExport(DataSet source)
    {
      var tables = source.Tables.Cast<DataTable>().ToList();
      TableExport(tables);
    }

    ///// <summary>
    ///// Xuat gridView ra Excel
    ///// </summary>
    ///// <param name="gridView"></param>
    //public void DataGridViewExport(DataGridView gridView)
    //{
    //  string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
    //  if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
    //  ISheet sheet = _baseExcel.Workbook.GetSheetAt(0);

    //  List<string> columns = (from DataGridViewColumn column in gridView.Columns
    //                          where column.Visible && column.Name != "space"
    //                          select column.Name).ToList();

    //  IRow rowHeader = sheet.CreateRow(0);
    //  rowHeader.Height = _baseExcel.RowHeaderHeight;
    //  for (int j = 0; j < columns.Count; j++)
    //  {
    //    sheet.SetColumnWidth(j, _baseExcel.ColumnWidth);
    //    rowHeader.CreateCell(j).SetCellValue(gridView.Columns[columns[j]].HeaderText);
    //    rowHeader.GetCell(j).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_HEADER_CENTER];
    //  }

    //  // Xuất dữ liệu
    //  for (int i = 0; i < gridView.Rows.Count; i++)
    //  {
    //    IRow currentRow = sheet.CreateRow(i + 1);
    //    currentRow.Height = _baseExcel.RowHeight;
    //    for (int j = 0; j < columns.Count; j++)
    //    {
    //      var value = gridView.Rows[i].Cells[columns[j]].Value ?? "";
    //      currentRow.CreateCell(j).SetCellValue(value.ToString());
    //      // currentRow.GetCell(j).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
    //    }
    //  }
    //  _baseExcel.SaveExport(fileName);
    //}

  

    public void TemplateExport(DataTable table, int startRowIndex, Dictionary<string, int> columnIndexPairs, List<SetCellValue> setCellValues)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
      try
      {
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);

        // Set cell value
        if (setCellValues != null)
        {
          foreach (SetCellValue item in setCellValues)
          {
            sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
          }
        }

        for (int i = 1; i < table.Rows.Count; i++)
        {
          sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
        }

        int x = 0;
        foreach (DataRow row in table.Rows)
        {
          sheet1.GetRow(startRowIndex + x).Height = _baseExcel.RowHeight;
          foreach (string column in columnIndexPairs.Keys)
          {
            sheet1.GetRow(startRowIndex + x).GetCell(columnIndexPairs[column]).SetCellValue(row[column].ToString());
          }
          x++;
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
     
      }
    }

    /// <summary>
    /// Xuat GridView ra template có sẵn
    /// </summary>
    /// <param name="table">DataGridView</param>
    /// <param name="isExportHeader"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="startColumnIndex"></param>
    /// <param name="setCellValues"></param>
 
    
    /// <summary>
    /// Xuất DataTable ra Excel có sẵn
    /// </summary>
    public void TemplateExport(DataTable table, bool isExportHeader, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues, bool isSetValuesAfterExportData = true)
    {

      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;

      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
      try
      {
        // Set cell value
        if (isSetValuesAfterExportData == false && setCellValues != null)
        {
          foreach (var item in setCellValues)
          {
            sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
          }
        }

        // Export header
        if (isExportHeader)
        {
          _baseExcel.CreateHeader(ref sheet1, table.Columns, startRowIndex, startColumnIndex);
          startRowIndex = startRowIndex + 1;
        }

        // Export chi tiết
        _baseExcel.CreateContent(ref sheet1, table, startRowIndex, startColumnIndex);

        // Set cell value
        if (isSetValuesAfterExportData && setCellValues != null)
        {
          foreach (var item in setCellValues)
          {
            sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
          }
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
        
        
      }
    }

    public void TemplateExport(DataTable table, bool isExportHeader, bool option, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues, bool isSetValuesAfterExportData = true)
    {
      if (option)
        TemplateExport(table, isExportHeader, startRowIndex, startColumnIndex, setCellValues, isSetValuesAfterExportData);
      else
      {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;

        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        try
        {
          // Set cell value
          if (isSetValuesAfterExportData == false)
          {
            foreach (var item in setCellValues)
            {
              sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
          }

          // Export header
          if (isExportHeader)
          {
            _baseExcel.CreateHeader(ref sheet1, table.Columns, startRowIndex, startColumnIndex);
            startRowIndex = startRowIndex + 1;
          }

          // Export chi tiết
          _baseExcel.InsertContent(ref sheet1, table, startRowIndex, startColumnIndex);

          // Set cell value
          if (isSetValuesAfterExportData)
          {
            foreach (var item in setCellValues)
            {
              sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
          }
          _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
          
          
        }
      }
    }


   

    /// <summary>
    /// Chưa test: 2014.07.01
    /// </summary>
   
    #region DataTable
    /// <summary>
    /// Xuất DataTable với các cột định nghĩa sẵn
    /// </summary>
    /// <param name="sheetindex"></param>
    /// <param name="arrcolumn">Cột định nghĩa, theo thứ tự</param>
    /// <param name="table"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="startColumnIndex"></param>
    /// <param name="setCellValues"></param>
    public void TemplateExportDataTable(int sheetindex, ArrayList arrcolumn, DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;

      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(sheetindex);
      try
      {
        // Export chi tiết
        _baseExcel.CreateContent(ref sheet1, arrcolumn, table, startRowIndex, startColumnIndex);

        // Set cell value
        if (setCellValues != null)
        {
          foreach (var item in setCellValues)
          {
            sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
          }
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
        
        
      }
    }

    public void TemplateExportphunu(DataSet table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        try
        {
            IFont font = _baseExcel.Workbook.CreateFont();
            font.FontHeightInPoints = (short)10;
            font.FontName = "Times New Roman";
            ICellStyle style = _baseExcel.Workbook.CreateCellStyle();
            if (true)
            {
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            }
            style.SetFont(font);
            var styles = new Dictionary<String, ICellStyle>();
            styles.Add("mystyle", style);
            // Export chi tiết
            string tablename = "";
            for (int i = 0; i < table.Tables.Count; i++)
            {
                tablename = table.Tables[i].ToString();
                ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(i);

                _baseExcel.CreateContentphunu(ref sheet1, table, startRowIndex, startColumnIndex, tablename, styles);
                // Set cell value
                if (setCellValues != null)
                {
                    foreach (var item in setCellValues)
                    {
                        sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                    }
                }
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
        }
    }

   

      /// <summary>
      /// Thuong chinh
      /// </summary>
      /// 

      //Hận xuất taxonline bhyt tre em
    public void TemplateExportTaxOnline(DataTable table, int startRowIndex, int startColumnIndex)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);

        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 700;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j] ?? " ";
                    row.CreateCell(index).SetCellValue(value.ToString());
                    var cell = _baseExcel.CellStyles[(j==0||j == 3 || j == 5 || j == 6) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                    cell.WrapText = true;
                    cell.VerticalAlignment = VerticalAlignment.CENTER;
                    row.GetCell(index).CellStyle = cell;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
           
        }
    }
    #endregion

    #region Khoa

    public void ExportTemplateDT0200(List<SetCellValue> setCellValues, bool isSetValuesAfterExportData = true)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;

      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
      try
      {

        // Set cell value
        if (isSetValuesAfterExportData == false)
        {
          foreach (var item in setCellValues)
          {
            sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
          }
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
       // 
        //MessageBox.Show("Lỗi xuất dữ liệu: \r\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public void TemplateExport(DataTable table, List<string> columns, int startRowIndex, int startColumnIndex, bool isAddRow, List<SetCellValue> setCellValues, bool isBorderCell = true)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        try
        {
            int rowCount = table.Rows.Count;
            int columnCount = columns.Count;
            ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
            IFont font = _baseExcel.Workbook.CreateFont();
            font.FontHeightInPoints = (short)8;
            font.FontName = "Times New Roman";
            ICellStyle style = _baseExcel.Workbook.CreateCellStyle();
            if (isBorderCell)
            {
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            }
            style.SetFont(font);
            var styles = new Dictionary<String, ICellStyle>();
            styles.Add("mystyle", style);

            // Set cell value
            if (setCellValues != null)
            {
                foreach (SetCellValue item in setCellValues)
                {
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                }
            }
            // Nếu số cell > 4000 mà set cellStyle sẽ lỗi (Lỗi này chưa fix được)
            bool isSetCellStyle = (rowCount * columnCount) < 4000;
            if (!isAddRow)
            {
                foreach (DataRow Row in table.Rows)
                {
                    IRow currentRow = sheet1.CreateRow(startRowIndex);
                    currentRow.Height = 650; //_baseExcel.RowHeight;
                    int i = startColumnIndex;
                    foreach (string column in columns)
                    {
                        //var value = Row[column] ?? "";
                        var value = Row[column];
                        currentRow.CreateCell(i).SetCellValue(value.ToString());
                        if (isSetCellStyle)
                        {
                            // currentRow.GetCell(i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                            currentRow.GetCell(i).CellStyle = styles["mystyle"];
                        }
                        i++;
                    }
                    startRowIndex++;
                }
            }
            else
            {
                IRow row = sheet1.CreateRow(startRowIndex);
                row.Height = _baseExcel.RowHeight;
                for (int i = 0; i < columnCount; i++)
                {
                    row.CreateCell(startColumnIndex + i);
                    // row.GetCell(startColumnIndex + i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                    row.GetCell(startColumnIndex + i).CellStyle = styles["mystyle"];
                }

                for (int i = 1; i < rowCount; i++)
                {
                    sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
                }

                int x = 0;
                foreach (DataRow Row in table.Rows)
                {
                    sheet1.GetRow(startRowIndex + x).Height = _baseExcel.RowHeight;

                    for (int j = 0; j < columnCount; j++)
                    {
                        object value = Row[columns[j]];
                        sheet1.GetRow(startRowIndex + x).GetCell(startColumnIndex + j).SetCellValue(value.ToString());
                    }
                    x++;
                }
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            // Không xử lý TH người dùng truyền tham số sai 
           
        }
    }

    public void TemplateExport_Sobo(DataTable table, List<string> columns, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        try
        {
            int rowCount = table.Rows.Count;
            int columnCount = columns.Count;

            ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);

            // Set cell value
            if (setCellValues != null)
            {
                foreach (SetCellValue item in setCellValues)
                {
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                }
            }

            IRow row = sheet1.CreateRow(startRowIndex);
            row.Height = _baseExcel.RowHeight;

            for (int i = 0; i < columnCount; i++)
            {
                row.CreateCell(startColumnIndex + i);
            }

            for (int i = 1; i < rowCount; i++)
            {
                sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
            }

            int x = 0;
            foreach (DataRow Row in table.Rows)
            {
                sheet1.GetRow(startRowIndex + x).Height = _baseExcel.RowHeight;

                for (int j = 0; j < columnCount; j++)
                {
                    object value = Row[columns[j]] ?? "";
                    sheet1.GetRow(startRowIndex + x).GetCell(startColumnIndex + j).SetCellValue(value.ToString());
                }
                x++;
            }

            CellRangeAddress cellRange;
            for (int i = 0; i < table.Rows.Count; i = i + 2)
            {
                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i, 2, 3);
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 1, 1);
                string firstValue = sheet1.GetRow(startRowIndex + i).GetCell(1).StringCellValue;
                string lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(1).StringCellValue;
                string mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(1).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 4, 4);
                firstValue = sheet1.GetRow(startRowIndex + i).GetCell(4).StringCellValue;
                lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(4).StringCellValue;
                mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(4).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 5, 5);
                firstValue = sheet1.GetRow(startRowIndex + i).GetCell(5).StringCellValue;
                lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(5).StringCellValue;
                mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(5).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 6, 6);
                firstValue = sheet1.GetRow(startRowIndex + i).GetCell(6).StringCellValue;
                lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(6).StringCellValue;
                mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(6).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 7, 7);
                firstValue = sheet1.GetRow(startRowIndex + i).GetCell(7).StringCellValue;
                lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(7).StringCellValue;
                mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(7).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 8, 8);
                firstValue = sheet1.GetRow(startRowIndex + i).GetCell(8).StringCellValue;
                lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(8).StringCellValue;
                mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(8).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 9, 9);
                firstValue = sheet1.GetRow(startRowIndex + i).GetCell(9).StringCellValue;
                lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(9).StringCellValue;
                mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(9).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);

                cellRange = new CellRangeAddress(startRowIndex + i, startRowIndex + i + 1, 10, 10);
                firstValue = sheet1.GetRow(startRowIndex + i).GetCell(10).StringCellValue;
                lastValue = sheet1.GetRow(startRowIndex + i + 1).GetCell(10).StringCellValue;
                mergeValue = firstValue + Environment.NewLine + lastValue;
                sheet1.GetRow(startRowIndex + i).GetCell(10).SetCellValue(mergeValue.ToString());
                sheet1.AddMergedRegion(cellRange);
            }

            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            //
            // Không xử lý TH người dùng truyền tham số sai 
            //
        }
    }

    public void TemplateExport(DataTable table, List<string> columns, int startRowIndex, int startColumnIndex, bool isAddRow, List<SetCellValue> setCellValues, short fontSize, bool isMergeCell, List<string> listMergeCells, bool isBorderCell = true)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        try
        {
            int rowCount = table.Rows.Count;
            int columnCount = columns.Count;
            ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
            IFont font = _baseExcel.Workbook.CreateFont();
            font.FontHeightInPoints = fontSize;
            font.FontName = "Times New Roman";
            ICellStyle style = _baseExcel.Workbook.CreateCellStyle();
            if (isBorderCell)
            {
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            }
            style.VerticalAlignment = VerticalAlignment.CENTER;
            style.WrapText = true;
            style.ShrinkToFit = true;
            style.SetFont(font);
            var styles = new Dictionary<String, ICellStyle>();
            styles.Add("mystyle", style);

            // Set cell value
            if (setCellValues != null)
            {
                foreach (SetCellValue item in setCellValues)
                {
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                }
            }
            // Nếu số cell > 4000 mà set cellStyle sẽ lỗi (Lỗi này chưa fix được)
            bool isSetCellStyle = (rowCount * columnCount) < 4000;
            if (!isAddRow)
            {
                foreach (DataRow Row in table.Rows)
                {
                    IRow currentRow = sheet1.CreateRow(startRowIndex);
                    currentRow.Height = 650; //_baseExcel.RowHeight;
                    int i = startColumnIndex;
                    foreach (string column in columns)
                    {
                        var value = Row[column] ?? "";
                        currentRow.CreateCell(i).SetCellValue(value.ToString());
                        if (isSetCellStyle)
                        {
                            // currentRow.GetCell(i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                            currentRow.GetCell(i).CellStyle = styles["mystyle"];
                        }
                        i++;
                    }
                    startRowIndex++;
                }
            }
            else
            {
                IRow row = sheet1.CreateRow(startRowIndex);
                row.Height = _baseExcel.RowHeight;
                for (int i = 0; i < columnCount; i++)
                {
                    row.CreateCell(startColumnIndex + i);
                    // row.GetCell(startColumnIndex + i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                    row.GetCell(startColumnIndex + i).CellStyle = styles["mystyle"];
                }

                for (int i = 1; i < rowCount; i++)
                {
                    sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
                }

                int x = 0;
                foreach (DataRow Row in table.Rows)
                {
                    sheet1.GetRow(startRowIndex + x).Height = _baseExcel.RowHeight;

                    for (int j = 0; j < columnCount; j++)
                    {
                        object value = Row[columns[j]] ?? "";
                        sheet1.GetRow(startRowIndex + x).GetCell(startColumnIndex + j).SetCellValue(value.ToString());
                    }
                    x++;
                }

                if (isMergeCell == true)
                {
                    ICellStyle mergeStyle = _baseExcel.MergeCellStyle(_baseExcel.Workbook, NPOI.SS.UserModel.HorizontalAlignment.CENTER);
                    foreach (string item in listMergeCells)
                    {
                        string[] regionMerge = item.Split(';');
                        sheet1.AddMergedRegion(new CellRangeAddress(int.Parse(regionMerge[0]), int.Parse(regionMerge[1]),
                            int.Parse(regionMerge[2]), int.Parse(regionMerge[3])));
                        sheet1.GetRow(int.Parse(regionMerge[0])).GetCell(0).CellStyle = mergeStyle;
                    }
                }
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            // Không xử lý TH người dùng truyền tham số sai 
           
        }
    }

      // Danh sach ho ngheo
    public void TemplateExport_DanhsachHongheo(DataTable table, List<string> columns, int startRowIndex, 
        int startColumnIndex, bool isAddRow, List<SetCellValue> setCellValues, short fontSize, 
        bool isMergeCell, List<string> listMergeCells, bool isBorderCell = true)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        try
        {
            int rowCount = table.Rows.Count;
            int columnCount = columns.Count;
            ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);

            #region Get cellstyles
            ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
            IRow rowSheet2 = sheet2.GetRow(0);
            ICellStyle[] cellStyles = new ICellStyle[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
                cellStyles[i] = rowSheet2.GetCell(i).CellStyle;
            #endregion

            // Set cell value
            if (setCellValues != null)
            {
                foreach (SetCellValue item in setCellValues)
                {
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                }
            }
            // Nếu số cell > 4000 mà set cellStyle sẽ lỗi (Lỗi này chưa fix được)
            bool isSetCellStyle = (rowCount * columnCount) < 4000;
            if (!isAddRow)
            {
                foreach (DataRow Row in table.Rows)
                {
                    IRow currentRow = sheet1.CreateRow(startRowIndex);
                    currentRow.Height = 600; //_baseExcel.RowHeight;
                    int i = startColumnIndex;
                    foreach (string column in columns)
                    {
                        var value = Row[column] ?? "";
                        currentRow.CreateCell(i).SetCellValue(value.ToString());
                        if (isSetCellStyle)
                        {
                            // currentRow.GetCell(i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                            currentRow.GetCell(i).CellStyle = cellStyles[i];
                        }
                        i++;
                    }
                    startRowIndex++;
                }
            }
            else
            {
                IRow row = sheet1.CreateRow(startRowIndex);
                row.Height = 750;
                for (int i = 0; i < columnCount; i++)
                {
                    row.CreateCell(startColumnIndex + i);
                    // row.GetCell(startColumnIndex + i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                    row.GetCell(startColumnIndex + i).CellStyle = cellStyles[i];
                }

                for (int i = 1; i < rowCount; i++)
                {
                    sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
                }

                int x = 0;
                foreach (DataRow Row in table.Rows)
                {
                    sheet1.GetRow(startRowIndex + x).Height = 600;

                    for (int j = 0; j < columnCount; j++)
                    {
                        object value = Row[columns[j]] ?? "";
                        sheet1.GetRow(startRowIndex + x).GetCell(startColumnIndex + j).SetCellValue(value.ToString());
                    }
                    x++;
                }

                if (isMergeCell == true)
                {
                    foreach (string item in listMergeCells)
                    {
                        string[] regionMerge = item.Split(';');
                        sheet1.AddMergedRegion(new CellRangeAddress(int.Parse(regionMerge[0]), int.Parse(regionMerge[1]),
                            int.Parse(regionMerge[2]), int.Parse(regionMerge[3])));
                    }
                }
            }
            _baseExcel.Workbook.RemoveSheetAt(1);
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            // Không xử lý TH người dùng truyền tham số sai 
           
        }
    }

    #endregion

    #region //quanly

    public void ExportTemplateQuanLy010000(List<SetCellValue> setCellValues, bool isSetValuesAfterExportData = true)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;

      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
      try
      {
        // Set cell value
        if (isSetValuesAfterExportData == false)
        {
          foreach (var item in setCellValues)
          {
            sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
          }
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
        
      }
    }

    #endregion

    #region Trí Thương: Thống kê chung nhân sự

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view">DataGridView: chỉ export những cột hiển thị (visible = true)</param>
    /// <param name="startRowIndex"></param>
    /// <param name="startColumnIndex"></param>
    /// <param name="setCellValues"></param>
   

    /// <summary>
    /// Export charts
    /// </summary>
    /// <param name="setCellValues">Chú ý: Giá trị của "SetCellValue.Value = kiểu số"</param>
    public void TemplateExportCharts(List<SetCellValue> setCellValues)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
      try
      {
        foreach (var item in setCellValues)
        {
          double d;
          ICell cell = sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex);
          bool isNumber = double.TryParse(item.Value, out d);
          if (isNumber)
            cell.SetCellValue(d);
          else
            cell.SetCellValue(item.Value);
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
        
        
      }
    }

    #endregion

    // Xuất template GIA DINH VAN HOAS
    public void TemplateExportBDKGDVH(DataTable table, int startRowIndex, int startColumnIndex)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 400;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j] ?? " ";
                    row.CreateCell(index).SetCellValue(value.ToString());
                    var cell = _baseExcel.CellStyles[(j == 0 || j == 3 || j == 5 || j == 7) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                    cell.WrapText = true;
                    cell.VerticalAlignment = VerticalAlignment.CENTER;
                    cell.SetFont(font);
                    row.GetCell(index).CellStyle = cell;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            //
        }
    }

    // Xuất template GIA DINH VAN HOAS
    public void TemplateExportNguoiGay_Nannhan(DataTable table, int startRowIndex, int startColumnIndex)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 600;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j] ?? " ";
                    row.CreateCell(index).SetCellValue(value.ToString());
                    var cell = _baseExcel.CellStyles[(j == 0 || j == 3 || j == 5 || j == 7|| j == 6 || j == 8 || j == 9 || j == 4) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                    cell.WrapText = true;
                    cell.VerticalAlignment = VerticalAlignment.CENTER;
                    cell.SetFont(font);
                    row.GetCell(index).CellStyle = cell;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
        }
    }
    
    // Xuất template BHYT Tự nguyện 
    public void TemplateExportBHYTTN(DataTable table, int startRowIndex, int startColumnIndex)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)8;
        font.FontName = "Times New Roman";
        try
        {
            int x = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
            }
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 600;              
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j] ?? " ";
                    row.CreateCell(index).SetCellValue(value.ToString());
                    var cell = _baseExcel.CellStyles[(j==2||j==3||j == 4 || j == 6 || j == 7||j==8 || j == 10||j==15||j==17) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                    cell.WrapText = true;
                    cell.VerticalAlignment = VerticalAlignment.CENTER;
                    cell.SetFont(font);
                    row.GetCell(index).CellStyle = cell;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
        }
    }
    // Xuất template BHYT Tre em 
    public void TemplateExportMuasamthietbi(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = "C:\\RICONS\\TEMPLATE\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "Denghimuasamthietbi.xls";                 //FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);

        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh trai
        ICellStyle cellStyle3 = rowSheet2.GetCell(2).CellStyle; //border; canh trai, in dam
        ICellStyle cellStyle4 = rowSheet2.GetCell(3).CellStyle;
        ICellStyle cellStyle5 = rowSheet2.GetCell(4).CellStyle;
        ICellStyle cellStyle6 = rowSheet2.GetCell(5).CellStyle; // boder canh phai
        ICellStyle cellStyle7 = rowSheet2.GetCell(7).CellStyle; // boder canh phai

        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)10;
        font.FontName = "Times New Roman";
        try
        {
            foreach (var item in setCellValues)
            {
                try {
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                }
                catch (Exception)
                { 

                }
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
            }

            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                //row.Height = 600;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 0) row.GetCell(index).CellStyle = cellStyle1;
                    if (j == 1) row.GetCell(index).CellStyle = cellStyle2;
                    if (j == 2) row.GetCell(index).CellStyle = cellStyle2;
                    if (j == 3) row.GetCell(index).CellStyle = cellStyle4;
                    if (j == 4) row.GetCell(index).CellStyle = cellStyle5;
                    if (j == 5) row.GetCell(index).CellStyle = cellStyle6;
                    if (j == 6) row.GetCell(index).CellStyle = cellStyle6;
                    if (j == 7) row.GetCell(index).CellStyle = cellStyle6;
                    if (j == 8) row.GetCell(index).CellStyle = cellStyle6;
                    if (j == 9) row.GetCell(index).CellStyle = cellStyle6;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }


    public void TemplateExportDenghicungcapvattuthietbi(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues, string fileName)
    {
        //string fileName = "C:\\RICONS\\TEMPLATE\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "bangchamcongthang.xls";                 //FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);

        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //canh trai; 
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //canh giua
        ICellStyle cellStyle3 = rowSheet2.GetCell(2).CellStyle; //canh phai
        ICellStyle cellStyle4 = rowSheet2.GetCell(3).CellStyle; //canh trai in dam
        ICellStyle cellStyle5 = rowSheet2.GetCell(4).CellStyle; // canh giua in dam
        ICellStyle cellStyle6 = rowSheet2.GetCell(5).CellStyle; // canh phai in dam
        ICellStyle cellStyle7 = rowSheet2.GetCell(6).CellStyle; // canh phai in dam

        ICellStyle cellStyle8 = rowSheet2.GetCell(7).CellStyle; // canh phai in dam
       

        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)9;
        font.FontName = "Times New Roman";
        try
        {
            foreach (var item in setCellValues)
            {
                try
                {
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                }
                catch (Exception)
                {

                }
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
            }

            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                int iRowTong = startRowIndex + x;
                IRow row = sheet1.CreateRow(iRowTong);
                row.Height = 600;
                if (currentRow["stt"].ToString().Trim().Length > 4)
                {
                    sheet1.AddMergedRegion(new CellRangeAddress(iRowTong, iRowTong, 0, 2));
                }
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 0 && currentRow["stt"].ToString().Trim().Length > 4) row.GetCell(index).CellStyle = cellStyle4;
                    else if (j == 2) row.GetCell(index).CellStyle = cellStyle1;
                    else row.GetCell(index).CellStyle = cellStyle2;           
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {


        }
    }


    //chien
    // Xuất BHYT_BHXH
    public void TemplateExportBHYT_BaoTro(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        #region lay cellstyle
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh trai
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        #endregion
        try
        {
            int x = 0;
            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 600;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 0)
                        row.GetCell(index).CellStyle = cellStyle1;
                    else
                        row.GetCell(index).CellStyle = cellStyle2;
                }
                x = x + 1;
            }
            _baseExcel.Workbook.RemoveSheetAt(1);
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
           
        }
    }
    // Xuất template VĂN HOÁ THỐNG KÊ.
    public void TemplateExportVanhoathongke(List<SetCellValue> setCellValues2, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        try
        {
            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            foreach (var item in setCellValues2)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            //
        }
    }

    public void TemplateExportquanly(List<SetCellValue> setCellValues)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
      SetCellValue debugItem = new SetCellValue();

      //    IFont font = _baseExcel.Workbook.CreateFont();
      //    font.FontHeightInPoints = (short)11;
      //    font.FontName = "Times New Roman";
      try
      {
        foreach (var item in setCellValues)
        {
          debugItem = item;
          sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
        string s = string.Format("sheet1.GetRow({0}).GetCell({1}).SetCellValue({2})", debugItem.RowIndex,debugItem.ColumnIndex,debugItem.Value);
       
      }
    }

    // Xuất TBXH Chi Tra Luong 
    public void TemplateExportTBXH(List<SetCellValue> setCellValues2, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        try
        {

            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            foreach (var item in setCellValues2)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
           
        }
    }

    // Xuất template Phu nu phong trao thi dua
    public void TemplateExportPhuNuPhongTrao(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        #region lay cellstyle
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh trai
        #endregion
        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 600;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 1)
                        row.GetCell(index).CellStyle = cellStyle2;
                    else
                        row.GetCell(index).CellStyle = cellStyle1;
                }
                x = x + 1;
            }
            //dong tong
            int iRowTong = startRowIndex + x;
            IRow rowTong = sheet1.CreateRow(iRowTong);
            rowTong.Height = 600;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                int index = startColumnIndex + j;
                rowTong.CreateCell(index);
                if (j == 1)
                    rowTong.GetCell(index).CellStyle = cellStyle2;                    
                else
                    rowTong.GetCell(index).CellStyle = cellStyle1;
            }
            sheet1.AddMergedRegion(new CellRangeAddress(iRowTong, iRowTong, 0, 1));
            rowTong.GetCell(0).SetCellValue("Tổng số");
            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
        }
    }

    public void TemplateExportthongkedskhhgd(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        #region lay cellstyle
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh trai
        #endregion
        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 400;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 1)
                        row.GetCell(index).CellStyle = cellStyle2;
                    else
                        row.GetCell(index).CellStyle = cellStyle1;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }

    // Xuất template thong ke ket qua Phu nu phong trao thi dua
    public void TemplateExportTKPhuNuPhongTrao(DataTable table, int startRowIndex, int startColumnIndex,List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        #region lay cellstyle
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh trai
        #endregion
        try
        {
            int x = 0;
            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 400;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 1)
                        row.GetCell(index).CellStyle = cellStyle2;
                    else
                        row.GetCell(index).CellStyle = cellStyle1;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }
    // xuat danh sach ca nhan dat danh hieu phu nu xuat sac duoc khen thuong bieu duong
    public void TemplateExportTKPhuNuKhenThuong(DataTable table, int startRowIndex, int startColumnIndex,List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        #region lay cellstyle
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh trai
        #endregion
        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 600;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j==0 || j == 2 || j==3)
                        row.GetCell(index).CellStyle = cellStyle1;
                    else
                        row.GetCell(index).CellStyle = cellStyle2;
                }
                x = x + 1;
            }
            //dong tong
            int iRowTong = startRowIndex + x;
            IRow rowTong = sheet1.CreateRow(iRowTong);
            rowTong.Height = 600;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                int index = startColumnIndex + j;
                rowTong.CreateCell(index);
                if (j == 1)
                    rowTong.GetCell(index).CellStyle = cellStyle2;
                else
                    rowTong.GetCell(index).CellStyle = cellStyle1;
            }
            sheet1.AddMergedRegion(new CellRangeAddress(iRowTong, iRowTong, 0, 1));
            rowTong.GetCell(0).SetCellValue("Tổng số");
            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }
    // Xuất template GIA DINH VAN HOAS
    public void TemplatePhuNu_DanhSach(DataTable table, int startRowIndex, int startColumnIndex)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 400;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j] ?? " ";
                    row.CreateCell(index).SetCellValue(value.ToString());
                    var cell = _baseExcel.CellStyles[(j == 0) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                    cell.WrapText = true;
                    cell.VerticalAlignment = VerticalAlignment.CENTER;
                    cell.SetFont(font);
                    row.GetCell(index).CellStyle = cell;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }
    // Xuất template HỒ SƠ SAO Y
    public void TemplateExportSAOY(DataTable table, int startRowIndex, int startColumnIndex, string soluong, string tongtien)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";

        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(2);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh phai
        ICellStyle cellStyle3 = rowSheet2.GetCell(2).CellStyle;//canh trái

        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                //row.Height = 400;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 1 || j == 4 || j == 5)
                        row.GetCell(index).CellStyle = cellStyle1;
                    else if (j == 6)
                        row.GetCell(index).CellStyle = cellStyle3;
                    else
                        row.GetCell(index).CellStyle = cellStyle2;
                }
                x = x + 1;
            }
            //dong tong
            int iRowTong = startRowIndex + x;
            IRow rowTong = sheet1.CreateRow(iRowTong);
            rowTong.Height = 600;

            for (int j = 0; j < table.Columns.Count; j++)
            {
                int index = startColumnIndex + j;
                rowTong.CreateCell(index);
                if (j == 5)
                    rowTong.GetCell(index).CellStyle = cellStyle1;
                else
                    rowTong.GetCell(index).CellStyle = cellStyle3;
            }
            sheet1.AddMergedRegion(new CellRangeAddress(iRowTong, iRowTong, 0, 4));
            rowTong.GetCell(0).SetCellValue("Tổng số: ");

            rowTong.GetCell(5).SetCellValue(""+soluong+"");

            rowTong.GetCell(6).SetCellValue(""+tongtien+"");

           
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }

    // Xuat template SoCaiNghien
    public void TemplateSoCaiNghien(DataTable table, int startRowIndex, int startColumnIndex)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 4000;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j] ?? " ";
                    row.CreateCell(index).SetCellValue(value.ToString());
                    var cell = _baseExcel.CellStyles[(j == 0 || j == 12 || j == 13 || j == 14) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                    cell.WrapText = true;
                    cell.VerticalAlignment = VerticalAlignment.CENTER;
                    cell.SetFont(font);
                    row.GetCell(index).CellStyle = cell;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }

    // Xuất template HỒ SƠ chung thuc
    public void TemplateExportCHUNGTHUC(DataTable table, int startRowIndex, int startColumnIndex,string soluong,string tongtien)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";

        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(2);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh phai
        ICellStyle cellStyle3 = rowSheet2.GetCell(2).CellStyle;//canh trái

        try
        {
            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 600;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 1 || j == 4)
                        row.GetCell(index).CellStyle = cellStyle1;
                    else if (j == 5)
                        row.GetCell(index).CellStyle = cellStyle3;
                    else
                        row.GetCell(index).CellStyle = cellStyle2;
                }
                x = x + 1;
            }
            //dong tong
            int iRowTong = startRowIndex + x;
            IRow rowTong = sheet1.CreateRow(iRowTong);
            rowTong.Height = 600;

            for (int j = 0; j < table.Columns.Count; j++)
            {
                int index = startColumnIndex + j;
                rowTong.CreateCell(index);
                rowTong.GetCell(index).CellStyle = cellStyle3;
            }
            sheet1.AddMergedRegion(new CellRangeAddress(iRowTong, iRowTong, 0, 4));
            rowTong.GetCell(0).SetCellValue("Tổng số: ");
            rowTong.GetCell(5).SetCellValue("" + tongtien + "");
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            //
        }
    }

    //Xuất mẫu thống kê tổng số trẻ e đặc biệt
    public void TemplateExportTreEm(List<SetCellValue> setCellValues, bool isSetValuesAfterExportData = true)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
      try
      {

        // Set cell value
        if (isSetValuesAfterExportData == false)
        {
          foreach (var item in setCellValues)
          {
            sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
          }
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
        
       
      }
    }

    // Xuất template danh sach tho cung liet si
    public void DanhSachThoCungLietSi(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)12;
        font.FontName = "Times New Roman";
        try
        {
            int x = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
            }
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 600;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j] ?? " ";
                    row.CreateCell(index).SetCellValue(value.ToString());
                    var cell = _baseExcel.CellStyles[(j == 0) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                    cell.WrapText = true;
                    cell.VerticalAlignment = VerticalAlignment.CENTER;
                    cell.SetFont(font);
                    row.GetCell(index).CellStyle = cell;
                }
                x = x + 1;
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
           // 
        }
    }

    public void TemplateExport_TBXH_Dieuduong(DataTable table, List<string> columns, int startRowIndex, int startColumnIndex, bool isAddRow, List<SetCellValue> setCellValues, bool isBorderCell = true)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        try
        {
            int rowCount = table.Rows.Count;
            int columnCount = columns.Count;
            ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
            IFont font = _baseExcel.Workbook.CreateFont();
            font.FontHeightInPoints = (short)10;
            font.FontName = "Times New Roman";
            ICellStyle style = _baseExcel.Workbook.CreateCellStyle();
            if (isBorderCell)
            {
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            }
            style.SetFont(font);
            var styles = new Dictionary<String, ICellStyle>();
            styles.Add("mystyle", style);

            // Set cell value
            if (setCellValues != null)
            {
                foreach (SetCellValue item in setCellValues)
                {
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                }
            }
            // Nếu số cell > 4000 mà set cellStyle sẽ lỗi (Lỗi này chưa fix được)
            bool isSetCellStyle = (rowCount * columnCount) < 4000;
            if (!isAddRow)
            {
                foreach (DataRow Row in table.Rows)
                {
                    IRow currentRow = sheet1.CreateRow(startRowIndex);
                    currentRow.Height = 650; //_baseExcel.RowHeight;
                    int i = startColumnIndex;
                    foreach (string column in columns)
                    {
                        var value = Row[column] ?? "";
                        currentRow.CreateCell(i).SetCellValue(value.ToString());
                        if (isSetCellStyle)
                        {
                            // currentRow.GetCell(i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                            currentRow.GetCell(i).CellStyle = styles["mystyle"];
                        }
                        i++;
                    }
                    startRowIndex++;
                }
            }
            else
            {
                IRow row = sheet1.CreateRow(startRowIndex);
                row.Height = _baseExcel.RowHeight;
                for (int i = 0; i < columnCount; i++)
                {
                    row.CreateCell(startColumnIndex + i);
                    // row.GetCell(startColumnIndex + i).CellStyle = _baseExcel.CellStyles[_baseExcel.CELL_NOMAL_LEFT];
                    row.GetCell(startColumnIndex + i).CellStyle = styles["mystyle"];
                }

                for (int i = 1; i < rowCount; i++)
                {
                    sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
                }

                int x = 0;
                foreach (DataRow Row in table.Rows)
                {
                    sheet1.GetRow(startRowIndex + x).Height = _baseExcel.RowHeight;

                    for (int j = 0; j < columnCount; j++)
                    {
                        object value = Row[columns[j]] ?? "";
                        sheet1.GetRow(startRowIndex + x).GetCell(startColumnIndex + j).SetCellValue(value.ToString());
                    }
                    x++;
                }
            }
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            // Không xử lý TH người dùng truyền tham số sai 
            
        }
    }

    public void TemplateExportDansoKehoachHGD(DataSet dtsIn
        , string[] _startRowIndex, string[] _startColumnIndex
        ,string donvi, int r,int c)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        try
        {
            for (int i = 0; i < dtsIn.Tables.Count; i++)
            {
                int startRowIndex = int.Parse(_startRowIndex[i].ToString());
                int startColumnIndex = int.Parse(_startColumnIndex[i].ToString());
                DataTable table = dtsIn.Tables[i];
                int x = 0;
                foreach (DataRow currentRow in table.Rows)
                {
                    IRow row = sheet1.CreateRow(startRowIndex + x);
                    row.Height = 600;
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        int index = startColumnIndex + j;
                        object value = currentRow[j] ?? " ";
                        row.CreateCell(index).SetCellValue(value.ToString());
                        var cell = _baseExcel.CellStyles[(j == 0 || j == 3 || j == 5 || j == 7 || j == 6 || j == 8 || j == 9 || j == 4) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
                        cell.WrapText = true;
                        cell.VerticalAlignment = VerticalAlignment.CENTER;
                        cell.SetFont(font);
                        row.GetCell(index).CellStyle = cell;
                    }
                    x = x + 1;
                }
            }
            //param
            IRow row1 = sheet1.CreateRow(r);
            row1.CreateCell(c).SetCellValue(donvi);
            var cell1 = _baseExcel.CellStyles[(c == 2) ? _baseExcel.CELL_NOMAL_CENTER : _baseExcel.CELL_NOMAL_LEFT];
            cell1.WrapText = true;
            cell1.VerticalAlignment = VerticalAlignment.CENTER;
            cell1.SetFont(font);
            row1.GetCell(c).CellStyle = cell1;

            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }

    public void TemplateExport_DS_THONGKE_DVHC(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        #region lay cellstyle
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh trai
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh giua
        //ICellStyle cellStyle3 = rowSheet2.GetCell(2).CellStyle; //border; canh phai
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)12;
        font.FontName = "Times New Roman";
        #endregion
        try
        {
            int x = 0;
            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 500;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 1||j==2)
                        row.GetCell(index).CellStyle = cellStyle1;
                    else
                        row.GetCell(index).CellStyle = cellStyle2;
                }
                x = x + 1;
            }
            _baseExcel.Workbook.RemoveSheetAt(1);
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }

    public void TemplateExport_DS_THONGKE_BIENDONG_KHHGD(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        #region lay cellstyle
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(1);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh trai
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh giua
        //ICellStyle cellStyle3 = rowSheet2.GetCell(2).CellStyle; //border; canh phai
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)10;
        font.FontName = "Times New Roman";
        #endregion
        try
        {
            int x = 0;
            foreach (var item in setCellValues)
            {
                sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
            }
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                row.Height = 400;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if (j == 1)
                        row.GetCell(index).CellStyle = cellStyle1;
                    else
                        row.GetCell(index).CellStyle = cellStyle2;
                }
                x = x + 1;
            }
            _baseExcel.Workbook.RemoveSheetAt(1);
            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }


    //Hộ Nghèo phân nhom thu nhập hộ gia đình theo kết quả điều tra ,ra soát hộ nghèo
    public void PhanNhomThuNhap(DataTable table, int startRowIndex, int startColumnIndex, List<SetCellValue> setCellValues)
    {
      string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
      if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
      ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
      IFont font = _baseExcel.Workbook.CreateFont();
      font.FontHeightInPoints = (short)12;
      font.FontName = "Times New Roman";
      try
      {
        int x = 0;
        foreach (var item in setCellValues)
        {
          sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
        }
        for (int i = 0; i < table.Rows.Count; i++)
        {
          sheet1.GetRow(startRowIndex).CopyRowTo(startRowIndex + 1);
        }
        foreach (DataRow currentRow in table.Rows)
        {
          IRow row = sheet1.CreateRow(startRowIndex + x);
          row.Height = 600;
          for (int j = 0; j < table.Columns.Count; j++)
          {
            int index = startColumnIndex + j;
            object value = currentRow[j] ?? " ";
            row.CreateCell(index).SetCellValue(value.ToString());
            var cell = _baseExcel.CellStyles[(j == 0) ?  _baseExcel.CELL_NOMAL_LEFT : _baseExcel.CELL_NOMAL_CENTER];
            cell.WrapText = true;
            cell.VerticalAlignment = VerticalAlignment.CENTER;
            cell.SetFont(font);
            row.GetCell(index).CellStyle = cell;
          }
          x = x + 1;
        }
        _baseExcel.SaveExport(fileName);
      }
      catch (Exception ex)
      {
        
        
      }
    }

    public void TemplateExporvnho_chua(DataTable table, int startRowIndex, int startColumnIndex, int tongsotindo, int tongnam,int tongnu, List<SetCellValue> setCellValues)
    {
        string fileName = FileDialogs.SaveFiles(_baseExcel.DialogFilter, "xls");
        if (!File.Exists(_baseExcel.ExcelTemplate) || fileName == null) return;
        ISheet sheet1 = _baseExcel.Workbook.GetSheetAt(0);
        IFont font = _baseExcel.Workbook.CreateFont();
        font.FontHeightInPoints = (short)11;
        font.FontName = "Times New Roman";
        ISheet sheet2 = _baseExcel.Workbook.GetSheetAt(2);
        IRow rowSheet2 = sheet2.GetRow(0);
        ICellStyle cellStyle1 = rowSheet2.GetCell(0).CellStyle; //border; canh giua
        ICellStyle cellStyle2 = rowSheet2.GetCell(1).CellStyle; //border; canh giua
        try
        {
            
            foreach (var item in setCellValues)
            {
                try
                {
                    sheet1.AddMergedRegion(new CellRangeAddress(item.RowIndex, item.RowIndex, 0, 14));
                    rowSheet2.GetCell(item.RowIndex).CellStyle = cellStyle2;
                    sheet1.GetRow(item.RowIndex).GetCell(item.ColumnIndex).SetCellValue(item.Value);
                    
                }
                catch (Exception) { }
            }

            int x = 0;
            foreach (DataRow currentRow in table.Rows)
            {
                IRow row = sheet1.CreateRow(startRowIndex + x);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int index = startColumnIndex + j;
                    object value = currentRow[j];
                    row.CreateCell(index).SetCellValue(value.ToString());
                    if(j==1||j==2)
                        row.GetCell(index).CellStyle = cellStyle1;
                    else
                        row.GetCell(index).CellStyle = cellStyle2;
                }
                x = x + 1;
            }
            //dong tong
            int iRowTong = startRowIndex + x;
            IRow rowTong = sheet1.CreateRow(iRowTong);
            rowTong.Height = 600;

            for (int j = 0; j < table.Columns.Count; j++)
            {
                int index = startColumnIndex + j;
                rowTong.CreateCell(index);
                rowTong.GetCell(index).CellStyle = cellStyle2;
            }
            sheet1.AddMergedRegion(new CellRangeAddress(iRowTong, iRowTong, 0, 2));
            rowTong.GetCell(0).SetCellValue("Tổng số: ");

            rowTong.GetCell(3).SetCellValue("" + tongsotindo + "");

            rowTong.GetCell(4).SetCellValue("" + tongnam + "");
            rowTong.GetCell(5).SetCellValue("" + tongnu + "");

            _baseExcel.SaveExport(fileName);
        }
        catch (Exception ex)
        {
            
            
        }
    }

   

  }
}
