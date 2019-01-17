using System;
using System.ComponentModel.DataAnnotations;

namespace Esfa.Poc.Matching.Entities
{
    public class Employer : IEquatable<Employer>
    {
        [Key]
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CompanyType { get; set; }
        public int AupaStatus { get; set; }

        public bool Equals(Employer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id) 
                   && string.Equals(CompanyName, other.CompanyName) 
                   && string.Equals(AlsoKnownAs, other.AlsoKnownAs) 
                   && CreatedOn.Equals(other.CreatedOn) 
                   && CompanyType == other.CompanyType 
                   && AupaStatus == other.AupaStatus;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((Employer) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (CompanyName != null ? CompanyName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AlsoKnownAs != null ? AlsoKnownAs.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CreatedOn.GetHashCode();
                hashCode = (hashCode * 397) ^ CompanyType;
                hashCode = (hashCode * 397) ^ AupaStatus;

                return hashCode;
            }
        }

        public static bool operator ==(Employer employer1, Employer employer2)
        {
            if (ReferenceEquals(employer1, null))
            {
                return ReferenceEquals(employer2, null);
            }

            return employer1.Equals(employer2);
        }

        public static bool operator !=(Employer employer1, Employer employer2) =>
            !(employer1 == employer2);
    }
}