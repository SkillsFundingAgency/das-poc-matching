using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Employer.Readers
{
    public class ExcelEmployerFileReader : IEmployerFileReader
    {
        public EmployerLoadResult Load(Stream stream)
        {
            var fileLoadResult = new EmployerLoadResult();
            var fileUploadEmployers = new List<FileUploadEmployer>();

            using (var document = SpreadsheetDocument.Open(stream, false))
            {
                var workbookPart = document.WorkbookPart;
                var sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                var relationshipId = sheets.First().Id.Value;
                var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
                var workSheet = worksheetPart.Worksheet;
                var sheetData = workSheet.GetFirstChild<SheetData>();
                var rows = sheetData.Descendants<Row>().ToList();
                rows.RemoveAt(0);

                foreach (var row in rows)
                {
                    var fileUploadEmployer = CreateEmployer(document, row);
                    fileUploadEmployers.Add(fileUploadEmployer);
                }

                fileLoadResult.Data = fileUploadEmployers;

                return fileLoadResult;
            }
        }

        #region Private Methods
        private static FileUploadEmployer CreateEmployer(SpreadsheetDocument document, OpenXmlElement row)
        {
            var account = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Account));
            var checksum = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Checksum));
            var modifiedOn = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.ModifiedOn));
            var companyName = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyName));
            var companyAka = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyAka));
            var aupa = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Aupa));
            var companyType = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyType));
            var primaryContact = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.PrimaryContact));
            var phone = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Phone));
            var email = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Email));
            var website = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Website));
            var address1 = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Address1));
            var city = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.City));
            var countryRegion = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CountryRegion));
            var postCode = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.PostCode));
            var createdBy = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CreatedBy));
            var created = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Created));
            var modifiedBy = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.ModifiedBy));
            var owner = GetCellValue(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Owner));

            var fileUploadEmployer = new FileUploadEmployer();
            fileUploadEmployer.Account = new Guid(account);
            fileUploadEmployer.Checksum = checksum;
            fileUploadEmployer.ModifiedOn = GetDate(modifiedOn);
            fileUploadEmployer.CompanyName = companyName;
            fileUploadEmployer.CompanyAka = companyAka;
            fileUploadEmployer.Aupa = aupa;
            fileUploadEmployer.CompanyType = companyType;
            fileUploadEmployer.PrimaryContact = primaryContact;
            fileUploadEmployer.Phone = phone;
            fileUploadEmployer.Email = email;
            fileUploadEmployer.Website = website;
            fileUploadEmployer.Address1 = address1;
            fileUploadEmployer.City = city;
            fileUploadEmployer.CountryRegion = countryRegion;
            fileUploadEmployer.PostCode = postCode;
            fileUploadEmployer.CreatedBy = createdBy;
            fileUploadEmployer.Created = GetDate(created);
            fileUploadEmployer.ModifiedBy = modifiedBy;
            fileUploadEmployer.Owner = owner;

            return fileUploadEmployer;
        }

        private static DateTime GetDate(string dateAsString)
        {
            var date = DateTime.FromOADate(Convert.ToDouble(dateAsString));

            return date;
        }

        private static string GetCellValue(SpreadsheetDocument document, CellType cell)
        {
            var stringTablePart = document.WorkbookPart.SharedStringTablePart;
            var cellValue = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
            }

            return cellValue;
        }
        #endregion
    }
}