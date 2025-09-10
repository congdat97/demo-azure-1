using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using Utility.Model.Excel;

namespace Utility.Helpers
{
    public static class ExcelHelpers
    {
        #region Contructor
        static ExcelHelpers()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        #endregion

        #region Method

        /// <summary>
        /// Export Excel từ header + data
        /// </summary>
        public static byte[] ExportExcel(List<ExcelColumnCustom> headers, List<List<object>> lstData, string sheetName = "Sheet1", List<string> lstColumnFormats = null)
        {
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add(sheetName);

                // Ghi header
                WriteHeader(ws, headers);

                // Ghi data
                WriteData(ws, lstData, lstColumnFormats);

                ws.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        #endregion

        #region Utility
        private static void WriteHeader(ExcelWorksheet ws, List<ExcelColumnCustom> columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                var col = columns[i];
                var cell = ws.Cells[1, i + 1];
                cell.Value = col.Header;
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;

                // Độ rộng cột
                if (col.Width.HasValue)
                    ws.Column(i + 1).Width = col.Width.Value;
                else
                    ws.Column(i + 1).AutoFit();

                // Màu nền
                if (col.BackgroundColor.HasValue)
                {
                    cell.Style.Fill.BackgroundColor.SetColor(col.BackgroundColor.Value);
                }
                else
                {
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                }

                // Căn lề ngang
                cell.Style.HorizontalAlignment = col.HorizontalAlignment;

                // Căn lề dọc
                cell.Style.VerticalAlignment = col.VerticalAlignment;
            }
        }

        private static void WriteData(ExcelWorksheet ws, List<List<object>> lstData, List<string> lstColumnFormats = null)
        {
            for (int row = 0; row < lstData.Count; row++)
            {
                for (int col = 0; col < lstData[row].Count; col++)
                {
                    var value = lstData[row][col];
                    var cell = ws.Cells[row + 2, col + 1];

                    // Gán value
                    cell.Value = value;

                    // Nếu có định nghĩa format cho cột này
                    if (lstColumnFormats != null && col < lstColumnFormats.Count && !string.IsNullOrEmpty(lstColumnFormats[col]))
                    {
                        cell.Style.Numberformat.Format = lstColumnFormats[col];
                    }
                    else
                    {
                        // Nếu không có định nghĩa thì auto detect
                        if (value is DateTime)
                        {
                            cell.Style.Numberformat.Format = "yyyy-MM-dd";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else if (value is int || value is long || value is short || value is decimal)
                        {
                            cell.Style.Numberformat.Format = "#,##0";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }  
                        else if (value is double || value is float)
                        {
                            cell.Style.Numberformat.Format = "#,##0.00";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        else
                        {
                            cell.Style.Numberformat.Format = "@"; // text
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                    }
                }
            }
        }
        #endregion

    }
}
