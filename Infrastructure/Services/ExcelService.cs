using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using Utilities;

namespace Infrastructure.Services
{
    public class ExcelService
    {
        public Dictionary<string, List<T>> ReadFromExcel<T>(string filePath, Func<ExcelWorksheet, int, T> map)
        {
            Logger.Instance.Log("Reading from Excel file: " + filePath);
            var sheetData = new Dictionary<string, List<T>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    var list = new List<T>();
                    var rowCount = worksheet.Dimension.Rows;
                    Logger.Instance.Log(rowCount.ToString());

                    for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                    {
                        var item = map(worksheet, row);
                        Logger.Instance.Log(item.ToString());
                        list.Add(item);
                    }

                    sheetData[worksheet.Name] = list;
                }
            }

            return sheetData;
        }
    }
}
