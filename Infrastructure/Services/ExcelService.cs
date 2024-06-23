using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.Services
{
    public class ExcelService
    {
        public Dictionary<string, List<T>> ReadFromExcel<T>(string filePath, Func<ExcelWorksheet, int, T> map)
        {
            var sheetData = new Dictionary<string, List<T>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    var list = new List<T>();
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                    {
                        var item = map(worksheet, row);
                        list.Add(item);
                    }

                    sheetData[worksheet.Name] = list;
                }
            }

            return sheetData;
        }
    }
}
