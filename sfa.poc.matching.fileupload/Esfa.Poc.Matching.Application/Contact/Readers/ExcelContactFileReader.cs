using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Contact.Readers
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

        private static FileUploadContact CreateContact(SpreadsheetDocument document, OpenXmlElement row)
        {
            var contact = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Contact));
            var checksum = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Checksum));
            var modifiedOn = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.ModifiedOn));
            var companyName = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.CompanyName));
            var createdOn = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.CreatedOnCompany));
            var contactType = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.ContactType));
            var phoneHome = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PhoneHome));
            var jobTitle = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.JobTitle));
            var phoneMobile = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PhoneMobile));
            var modifiedBy = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.ModifiedBy));
            var modified = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Modified));
            var preferredContact = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PreferredContact));
            var fullName = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.FullName));
            var firstName = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.FirstName));
            var middleName = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.MiddleName));
            var lastName = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.LastName));
            var phoneBusiness = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.PhoneBusiness));
            var createdBy = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.CreatedBy));
            var created = GetCellValue(document, row.Descendants<Cell>().ElementAt(ContactColumnIndex.Created));

            var fileUploadContact = new FileUploadContact();
            fileUploadContact.Contact = new Guid(contact);
            fileUploadContact.Checksum = checksum;
            fileUploadContact.ModifiedOn = GetDate(modifiedOn);
            fileUploadContact.CompanyName = companyName;
            fileUploadContact.CreatedOnCompany = GetDate(createdOn);
            fileUploadContact.ContactType = contactType;
            fileUploadContact.PhoneHome = phoneHome;
            fileUploadContact.JobTitle = jobTitle;
            fileUploadContact.PhoneMobile = phoneMobile;
            fileUploadContact.ModifiedBy = modifiedBy;
            fileUploadContact.Modified = GetDate(modified);
            fileUploadContact.PreferredContact = preferredContact;
            fileUploadContact.FullName = fullName;
            fileUploadContact.FirstName = firstName;
            fileUploadContact.MiddleName = middleName;
            fileUploadContact.LastName = lastName;
            fileUploadContact.PhoneBusiness = phoneBusiness;
            fileUploadContact.CreatedBy = createdBy;
            fileUploadContact.Created = GetDate(created);

            return fileUploadContact;
        }

        #region Private Methods
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