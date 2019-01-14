using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Esfa.Poc.Matching.Application.Common;
using Esfa.Poc.Matching.Application.Common.Interfaces;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.FileReader.Excel.Query
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
            var companyTl = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.CompanyTl));
            var checksum = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Checksum));
            var modifiedOn = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.ModifiedOn));
            var company = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Company));
            var created = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Created));
            var dateContacted = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.DateContacted));
            var nextPlannedContact = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.NextPlannedContact));
            var resolution = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Resolution));
            var resolutionOther = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.ResolutionOther));
            var offeredPlacements = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.OfferedPlacements));
            var occupationalPath = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.OccupationalPath));
            var technicalRouteWay = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.TechnicalRouteWay));
            var pathwayOffered = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.PathwayOffered));
            var routeOffered = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.RouteOffered));
            var postCode = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.PostCode));
            var region = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Region));
            var query = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Query));
            var queryOther = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.QueryOther));
            var createdBy = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.CreatedBy));
            var placementOffered = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.PlacementOffered));
            var referToProvider = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.ReferToProvider));
            var provider1 = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Provider1));
            var provider2 = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Provider2));
            var provider3 = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(QueryColumnIndex.Provider3));

            var fileUploadQuery = new FileUploadQuery
            {
                CompanyTl = new Guid(companyTl),
                Checksum = checksum,
                ModifiedOn = modifiedOn.ToDate(),
                Company = company,
                Created = created.ToDate(),
                DateContacted = dateContacted.ToDate(),
                NextPlannedContact = nextPlannedContact.ToDate(),
                Resolution = resolution,
                ResolutionOther = resolutionOther,
                OfferedPlacements = int.Parse(offeredPlacements),
                OccupationalPath = occupationalPath,
                TechnicalRouteWay = technicalRouteWay,
                PathwayOffered = pathwayOffered,
                RouteOffered = routeOffered,
                PostCode = postCode,
                Region = region,
                Query = query,
                QueryOther = queryOther,
                CreatedBy = createdBy,
                PlacementOffered = placementOffered,
                ReferToProvider = GetNullableBool(referToProvider),
                Provider1 = provider1,
                Provider2 = provider2,
                Provider3 = provider3
            };

            return fileUploadQuery;
        }

        private static bool? GetNullableBool(string value)
        {
            switch (value)
            {
                case Constants.Yes:
                    return true;
                case Constants.No:
                    return false;
                default:
                    return default(bool?);
            }
        }
        #endregion
    }
}