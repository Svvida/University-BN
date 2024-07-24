using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Infrastructure.Services
{
    public class ExcelService
    {
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(ILogger<ExcelService> logger)
        {
            _logger = logger;
        }

        public Dictionary<string, List<T>> ReadFromExcel<T>(string filePath, Func<ExcelWorksheet, int, T> map)
        {
            _logger.LogInformation("Reading from Excel file: " + filePath);
            var sheetData = new Dictionary<string, List<T>>();

            var package = new ExcelPackage(new FileInfo(filePath));
            try
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    var list = new List<T>();
                    var rowCount = worksheet.Dimension.Rows;
                    _logger.LogInformation($"Reading sheet: {worksheet.Name} with {rowCount} rows");

                    // Skip header row
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var item = map(worksheet, row);
                        list.Add(item);
                    }

                    sheetData[worksheet.Name] = list;
                }
            }
            finally
            {
                package.Dispose();
            }

            return sheetData;
        }
    }
}
