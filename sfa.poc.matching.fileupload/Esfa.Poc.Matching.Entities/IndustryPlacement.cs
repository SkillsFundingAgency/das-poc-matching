using System;
using System.ComponentModel.DataAnnotations;

namespace Esfa.Poc.Matching.Entities
{
    public class IndustryPlacement : IEquatable<IndustryPlacement>
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool Equals(IndustryPlacement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && CreatedOn.Equals(other.CreatedOn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IndustryPlacement)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ CreatedOn.GetHashCode();
            }
        }

        public static bool operator ==(IndustryPlacement industryPlacement1, IndustryPlacement industryPlacement2)
        {
            if (ReferenceEquals(industryPlacement1, null))
            {
                return ReferenceEquals(industryPlacement2, null);
            }

            return industryPlacement1.Equals(industryPlacement2);
        }

        public static bool operator !=(IndustryPlacement industryPlacement1, IndustryPlacement industryPlacement2) =>
            !(industryPlacement1 == industryPlacement2);
    }
}