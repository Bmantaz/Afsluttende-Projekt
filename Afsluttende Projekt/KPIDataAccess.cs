using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace Afsluttende_Projekt
{
    public class KPIData
    {
        public int OrdrePåDagen { get; set; }
        public double Præcision { get; set; }
        public int Uheld { get; set; }
    }

    public class KPIDataAccess
    {
        private string excelFilePath;

        public KPIDataAccess()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDir).FullName).FullName).FullName;
            excelFilePath = Path.Combine(projectDir, "Assets/KPI - Data.xlsx");

            // Debugging-linje for at tjekke om filen findes
            if (!File.Exists(excelFilePath))
            {
                throw new Exception($"Excel-filen blev ikke fundet på stien: {excelFilePath}");
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public List<KPIData> HentKPIData()
        {
            var kpiList = new List<KPIData>();
            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                var worksheet = package.Workbook.Worksheets["Data"];

                // Fejlhåndtering: Hvis worksheet er null
                if (worksheet == null)
                {
                    throw new Exception("Arket 'Data' blev ikke fundet i Excel-filen. Kontroller filen.");
                }

                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    var data = new KPIData
                    {
                        OrdrePåDagen = int.Parse(worksheet.Cells[row, 1].Text),
                        Præcision = double.Parse(worksheet.Cells[row, 2].Text),
                        Uheld = int.Parse(worksheet.Cells[row, 3].Text)
                    };
                    kpiList.Add(data);
                }
            }
            return kpiList;
        }
    }
}