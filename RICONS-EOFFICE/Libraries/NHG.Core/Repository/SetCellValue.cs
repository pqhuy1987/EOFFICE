namespace ExportHelper.Repository
{
  /// <summary>
  /// Class template dùng để SetCellValue dùng trong excel export
  /// </summary>
  public class SetCellValue
  {
    public int ColumnIndex { get; set; }
    public int RowIndex { get; set; }
    public string Value { get; set; }
  }
}