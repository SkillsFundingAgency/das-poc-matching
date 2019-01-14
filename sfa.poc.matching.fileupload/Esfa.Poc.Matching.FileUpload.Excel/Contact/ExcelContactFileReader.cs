using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Esfa.Poc.Matching.Application.Common.Interfaces;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.FileReader.Excel.Contact
{
    public class ExcelContactFileReader : IContactFileReader
    {
        public ContactLoadResult Load(Stream stream)
        {
            var fileLoadResult = new ContactLoadResult();
            var fileUploadContacts = new List<FileUploadContact>();

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
                    var fileUploadContact = CreateContact(document, row);
                    fileUploadContacts.Add(fileUploadContact);
                }

                fileLoadResult.Data = fileUploadContacts;

                return fileLoadResult;
            }
        }

        #region Private Methods
        private static FileUploadContact CreateContact(SpreadsheetDocument document, OpenXmlElement row)
        {
            var contact = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Contact));
            var checksum = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Checksum));
            var modifiedOn = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.ModifiedOn));
            var companyName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.CompanyName));
            var createdOn = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.CreatedOnCompany));
            var contactType = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.ContactType));
            var phoneHome = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PhoneHome));
            var jobTitle = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.JobTitle));
            var phoneMobile = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PhoneMobile));
            var modifiedBy = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.ModifiedBy));
            var modified = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Modified));
            var preferredContact = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PreferredContact));
            var fullName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.FullName));
            var firstName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.FirstName));
            var middleName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.MiddleName));
            var lastName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.LastName));
            var phoneBusiness = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PhoneBusiness));
            var createdBy = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.CreatedBy));
            var created = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Created));

            var fileUploadContact = new FileUploadContact
            {
                Contact = new Guid(contact),
                Checksum = checksum,
                ModifiedOn = modifiedOn.ToDate(),
                CompanyName = companyName,
                CreatedOnCompany = createdOn.ToDate(),
                ContactType = contactType,
                PhoneHome = phoneHome,
                JobTitle = jobTitle,
                PhoneMobile = phoneMobile,
                ModifiedBy = modifiedBy,
                Modified = modified.ToDate(),
                PreferredContact = preferredContact,
                FullName = fullName,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneBusiness = phoneBusiness,
                CreatedBy = createdBy,
                Created = created.ToDate()
            };

            return fileUploadContact;
        }
        #endregion
    }
}