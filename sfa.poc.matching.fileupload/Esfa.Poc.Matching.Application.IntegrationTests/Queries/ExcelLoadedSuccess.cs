using System;
using System.IO;
using Esfa.Poc.Matching.Application.Query;
using Esfa.Poc.Matching.Application.Query.Readers;
using Esfa.Poc.Matching.Domain;
using Xunit;

namespace Esfa.Poc.Matching.Application.IntegrationTests.Queries
{
    [Trait("Queries Excel", "Successfully loaded file")]
    public class ExcelLoadedSuccess
    {
        private readonly QueryLoadResult _loadResult;

        private const string DataFilePath = "./Queries/Query-Simple.xlsx";
        private readonly FileUploadQuery _firstRecord;

        public ExcelLoadedSuccess()
        {
            var fileReader = new ExcelQueryFileReader();
            using (var stream = File.Open(DataFilePath, FileMode.Open))
            {
                _loadResult = fileReader.Load(stream);
                _firstRecord = _loadResult.Data[0];
            }
        }

        [Fact(DisplayName = "Correct number of records loaded")]
        public void CorrectNumberOfRecordsLoaded() =>
            Assert.Single(_loadResult.Data);

        #region 1st Record Tests
        [Fact(DisplayName = "1st record - Company T-Level is correct")]
        public void CompanyTlIsCorrectForRecord1() =>
            Assert.Equal(new Guid("7931c597-5443-e811-80d7-000d3a214f60"), _firstRecord.CompanyTl);

        [Fact(DisplayName = "1st record - Checksum is correct")]
        public void ChecksumIsCorrectForRecord1() =>
            Assert.Equal("Checksum", _firstRecord.Checksum);

        [Fact(DisplayName = "1st record - Modified On is correct")]
        public void ModifiedOnIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2018, 12, 18, 9, 32, 0), _firstRecord.ModifiedOn);

        [Fact(DisplayName = "1st record - Company is correct")]
        public void CompanyIsCorrectForRecord1() =>
            Assert.Equal("Company", _firstRecord.Company);

        [Fact(DisplayName = "1st record - Created is correct")]
        public void CreatedIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2018, 12, 18, 9, 32, 0), _firstRecord.Created);

        [Fact(DisplayName = "1st record - Date Contacted is correct")]
        public void DateContactedIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2018, 12, 5), _firstRecord.DateContacted);

        [Fact(DisplayName = "1st record - Next Planned Contact is correct")]
        public void NextPlannedContactIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2018, 12, 20), _firstRecord.NextPlannedContact);

        [Fact(DisplayName = "1st record - Resolution is correct")]
        public void ResolutionIsCorrectForRecord1() =>
            Assert.Equal("Resolution", _firstRecord.Resolution);

        [Fact(DisplayName = "1st record - Resolution Other is correct")]
        public void ResolutionOtherIsCorrectForRecord1() =>
            Assert.Equal("Other Resolution", _firstRecord.ResolutionOther);

        [Fact(DisplayName = "1st record - Offered Placements is correct")]
        public void OfferedPlacementsIsCorrectForRecord1() =>
            Assert.Equal(1, _firstRecord.OfferedPlacements);

        [Fact(DisplayName = "1st record - Occupational Path is correct")]
        public void OccupationalPathIsCorrectForRecord1() =>
            Assert.Equal("Occupational Path", _firstRecord.OccupationalPath);

        [Fact(DisplayName = "1st record - Technical Route Way is correct")]
        public void TechnicalRouteWayIsCorrectForRecord1() =>
            Assert.Equal("Technical Route Way", _firstRecord.TechnicalRouteWay);

        [Fact(DisplayName = "1st record - Pathway Offered is correct")]
        public void PathwayOfferedIsCorrectForRecord1() =>
            Assert.Equal("Pathway Offered", _firstRecord.PathwayOffered);

        [Fact(DisplayName = "1st record - Route Offered is correct")]
        public void RouteOfferedIsCorrectForRecord1() =>
            Assert.Equal("Route Offered", _firstRecord.RouteOffered);

        [Fact(DisplayName = "1st record - Post Code is correct")]
        public void PostCodeIsCorrectForRecord1() =>
            Assert.Equal("ST1 1AA", _firstRecord.PostCode);

        [Fact(DisplayName = "1st record - Region is correct")]
        public void RegionIsCorrectForRecord1() =>
            Assert.Equal("Region", _firstRecord.Region);

        [Fact(DisplayName = "1st record - Query is correct")]
        public void QueryIsCorrectForRecord1() =>
            Assert.Equal("Query", _firstRecord.Query);

        [Fact(DisplayName = "1st record - Query Other is correct")]
        public void QueryOtherIsCorrectForRecord1() =>
            Assert.Equal("Query Other", _firstRecord.QueryOther);

        [Fact(DisplayName = "1st record - Created By is correct")]
        public void CreatedByIsCorrectForRecord1() =>
            Assert.Equal("Created By", _firstRecord.CreatedBy);

        [Fact(DisplayName = "1st record - Placement Offered is correct")]
        public void PlacementOfferedIsCorrectForRecord1() =>
            Assert.Equal("", _firstRecord.PlacementOffered);

        [Fact(DisplayName = "1st record - Refer To Provider is correct")]
        public void ReferToProviderdIsCorrectForRecord1() =>
            Assert.Equal(false, _firstRecord.ReferToProvider);

        [Fact(DisplayName = "1st record - Provider 1 is correct")]
        public void Provider1IsCorrectForRecord1() =>
            Assert.Equal("Provider 1", _firstRecord.Provider1);

        [Fact(DisplayName = "1st record - Provider 2 is correct")]
        public void Provider2IsCorrectForRecord1() =>
            Assert.Equal("Provider 2", _firstRecord.Provider2);

        [Fact(DisplayName = "1st record - Provider 3 is correct")]
        public void Provider3IsCorrectForRecord1() =>
            Assert.Equal("Provider 3", _firstRecord.Provider3);
        #endregion
    }
}