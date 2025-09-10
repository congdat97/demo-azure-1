using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Model.Excel
{
    public class ExcelColumnCustom
    {
        public string? Header { get; set; }
        public double? Width { get; set; } // Chiều rộng cột
        public Color? BackgroundColor { get; set; } // Màu nền
        public ExcelHorizontalAlignment HorizontalAlignment { get; set; } = ExcelHorizontalAlignment.Left; // Căn ngang
        public ExcelVerticalAlignment VerticalAlignment { get; set; } = ExcelVerticalAlignment.Center; // Căn dọc
    }
}
