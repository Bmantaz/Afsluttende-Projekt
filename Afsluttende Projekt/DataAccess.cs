using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace Afsluttende_Projekt
{
    public class DataAccess
    {
        private string excelFilePath;

        public DataAccess()
        {
            // Angiv stien til Excel-filen
            excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Brugerdata.xlsx");
            // Aktivér ikke-kommerciel licens for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public List<Bruger> HentBrugere()
        {
            List<Bruger> brugere = new List<Bruger>();

            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Ark1"]; // Antager at data er i det første ark
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Starter fra række 2 for at springe overskrifter over
                {
                    Bruger bruger = new Bruger
                    {
                        Brugernavn = worksheet.Cells[row, 1].Text,
                        MedarbejderID = worksheet.Cells[row, 2].Text,
                        Adgangskode = worksheet.Cells[row, 3].Text
                    };
                    brugere.Add(bruger);
                }
            }

            return brugere;
        }
    }

    public class Bruger
    {
        public string Brugernavn { get; set; }
        public string MedarbejderID { get; set; }
        public string Adgangskode { get; set; }
    }
}
