using System;

namespace Esfa.Poc.Matching.Domain
{
    public class FileUploadQuery
    {
        public Guid CompanyTl { get; set; }
        public string Checksum { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Company { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DateContacted { get; set; }
        public DateTime? NextPlannedContact { get; set; }
        public string Resolution { get; set; }
        public string ResolutionOther { get; set; }
        public int? OfferedPlacements { get; set; }
        public string OccupationalPath { get; set; }
        public string TechnicalRouteWay { get; set; }
        public string PathwayOffered { get; set; }
        public string RouteOffered { get; set; }
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string Query { get; set; }
        public string QueryOther { get; set; }
        public string CreatedBy { get; set; }
        public string PlacementOffered { get; set; }
        public bool? ReferToProvider { get; set; }
        public string Provider1 { get; set; }
        public string Provider2 { get; set; }
        public string Provider3 { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}