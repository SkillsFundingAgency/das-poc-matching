using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Query.Readers
{
    public class ExcelQueryFileReader : IQueryFileReader
    {
        public QueryLoadResult Load(Stream stream)
        {
            var fileLoadResult = new QueryLoadResult();
            var fileUploadQueries = new List<FileUploadQuery>();

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
                    var fileUploadQuery = CreateQuery(document, row);
                    fileUploadQueries.Add(fileUploadQuery);
                }

                fileLoadResult.Data = fileUploadQueries;

                return fileLoadResult;
            }
        }

        #region Private Methods
        private static FileUploadQuery CreateQuery(SpreadsheetDocument document, OpenXmlElement row)
        {
            var companyTl = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.CompanyTl));
            var checksum = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Checksum));
            var modifiedOn = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.ModifiedOn));
            var company = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Company));
            var created = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Created));
            var dateContacted = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.DateContacted));
            var nextPlannedContact = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.NextPlannedContact));
            var resolution = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Resolution));
            var resolutionOther = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.ResolutionOther));
            var offeredPlacements = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.OfferedPlacements));
            var occupationalPath = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.OccupationalPath));
            var technicalRouteWay = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.TechnicalRouteWay));
            var pathwayOffered = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.PathwayOffered));
            var routeOffered = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.RouteOffered));
            var postCode = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.PostCode));
            var region = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Region));
            var query = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Query));
            var queryOther = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.QueryOther));
            var createdBy = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.CreatedBy));
            var placementOffered = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.PlacementOffered));
            var referToProvider = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.ReferToProvider));
            var provider1 = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Provider1));
            var provider2 = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Provider2));
            var provider3 = GetCellValue(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Provider3));

            var fileUploadQuery = new FileUploadQuery();
            fileUploadQuery.CompanyTl = new Guid(companyTl);
            fileUploadQuery.Checksum = checksum;
            fileUploadQuery.ModifiedOn = GetDate(modifiedOn);
            fileUploadQuery.Company = company;
            fileUploadQuery.Created = GetDate(created);
            fileUploadQuery.DateContacted = GetDate(dateContacted);
            fileUploadQuery.NextPlannedContact = GetDate(nextPlannedContact);
            fileUploadQuery.Resolution = resolution;
            fileUploadQuery.ResolutionOther = resolutionOther;
            fileUploadQuery.OfferedPlacements = int.Parse(offeredPlacements);
            fileUploadQuery.OccupationalPath = occupationalPath;
            fileUploadQuery.TechnicalRouteWay = technicalRouteWay;
            fileUploadQuery.PathwayOffered = pathwayOffered;
            fileUploadQuery.RouteOffered = routeOffered;
            fileUploadQuery.PostCode = postCode;
            fileUploadQuery.Region = region;
            fileUploadQuery.Query = query;
            fileUploadQuery.QueryOther = queryOther;
            fileUploadQuery.CreatedBy = createdBy;
            fileUploadQuery.PlacementOffered = placementOffered;
            fileUploadQuery.ReferToProvider = GetNullableBool(referToProvider);
            fileUploadQuery.Provider1 = provider1;
            fileUploadQuery.Provider2 = provider2;
            fileUploadQuery.Provider3 = provider3;

            return fileUploadQuery;
        }

        private static bool? GetNullableBool(string value)
        {
            switch (value)
            {
                case QueryConstants.Yes:
                    return true;
                case QueryConstants.No:
                    return false;
                default:
                    return default(bool?);
            }
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