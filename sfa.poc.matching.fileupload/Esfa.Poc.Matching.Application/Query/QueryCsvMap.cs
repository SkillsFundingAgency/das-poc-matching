using CsvHelper.Configuration;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Query
{
    internal sealed class QueryCsvMap : ClassMap<FileUploadQuery>
    {
        internal QueryCsvMap()
        {
            Map(m => m.CompanyTl).Name(QueryColumnConstants.CompanyTl);
            Map(m => m.Checksum).Name(QueryColumnConstants.Checksum);
            Map(m => m.ModifiedOn).Name(QueryColumnConstants.ModifiedOn);
            Map(m => m.Company).Name(QueryColumnConstants.Company);
            Map(m => m.Created).Name(QueryColumnConstants.Created);
            Map(m => m.DateContacted).Name(QueryColumnConstants.DateContacted);
            Map(m => m.NextPlannedContact).Name(QueryColumnConstants.NextPlannedContact);
            Map(m => m.Resolution).Name(QueryColumnConstants.Resolution);
            Map(m => m.ResolutionOther).Name(QueryColumnConstants.ResolutionOther);
            Map(m => m.OfferedPlacements).Name(QueryColumnConstants.OfferedPlacements);
            Map(m => m.OccupationalPath).Name(QueryColumnConstants.OccupationalPath);
            Map(m => m.TechnicalRouteWay).Name(QueryColumnConstants.TechnicalRouteWay);
            Map(m => m.PathwayOffered).Name(QueryColumnConstants.PathwayOffered);
            Map(m => m.RouteOffered).Name(QueryColumnConstants.RouteOffered);
            Map(m => m.PostCode).Name(QueryColumnConstants.PostCode);
            Map(m => m.Region).Name(QueryColumnConstants.Region);
            Map(m => m.Query).Name(QueryColumnConstants.Query);
            Map(m => m.QueryOther).Name(QueryColumnConstants.QueryOther);
            Map(m => m.CreatedBy).Name(QueryColumnConstants.CreatedBy);
            Map(m => m.PlacementOffered).Name(QueryColumnConstants.PlacementOffered);
            Map(m => m.ReferToProvider).Name(QueryColumnConstants.ReferToProvider)
                .TypeConverterOption.BooleanValues(true, true, QueryConstants.Yes)
                .TypeConverterOption.BooleanValues(false, true, QueryConstants.No);
            Map(m => m.Provider1).Name(QueryColumnConstants.Provider1);
            Map(m => m.Provider2).Name(QueryColumnConstants.Provider2);
            Map(m => m.Provider3).Name(QueryColumnConstants.Provider3);
        }
    }
}