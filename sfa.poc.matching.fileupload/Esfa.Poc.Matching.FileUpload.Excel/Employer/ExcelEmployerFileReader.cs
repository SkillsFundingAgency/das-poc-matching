using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Esfa.Poc.Matching.Application.Common.Interfaces;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.FileReader.Excel.Employer
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
            var account = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Account));
            var checksum = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Checksum));
            var modifiedOn = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.ModifiedOn));
            var companyName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyName));
            var companyAka = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyAka));
            var aupa = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Aupa));
            var companyType = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyType));
            var primaryContact = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.PrimaryContact));
            var phone = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Phone));
            var email = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Email));
            var website = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Website));
            var address1 = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Address1));
            var city = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.City));
            var countryRegion = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CountryRegion));
            var postCode = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.PostCode));
            var createdBy = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CreatedBy));
            var created = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Created));
            var modified = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Modified));
            var modifiedBy = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.ModifiedBy));
            var owner = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Owner));

            var fileUploadEmployer = new FileUploadEmployer
            {
                Account = new Guid(account),
                Checksum = checksum,
                ModifiedOn = modifiedOn.ToDate(),
                CompanyName = companyName,
                CompanyAka = companyAka,
                Aupa = aupa,
                CompanyType = companyType,
                PrimaryContact = primaryContact,
                Phone = phone,
                Email = email,
                Website = website,
                Address1 = address1,
                City = city,
                CountryRegion = countryRegion,
                PostCode = postCode,
                CreatedBy = createdBy,
                Created = created.ToDate(),
                ModifiedBy = modifiedBy,
                Modified = modified.ToDate(),
                Owner = owner
            };

            return fileUploadEmployer;
        }
        #endregion
    }
}