using System;
using System.ComponentModel.DataAnnotations;

namespace Esfa.Poc.Matching.Entities
{
    public class Contact : IEquatable<Contact>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EntityRefId { get; set; }
        public Employer Employer { get; set; }
        public int ContactTypeId { get; set; }
        public int PreferredContactMethodType { get; set; }
        public bool IsPrimary { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string BusinessPhone { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public bool Equals(Contact other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id)
                   && EntityRefId.Equals(other.EntityRefId)
                   && Equals(Employer, other.Employer)
                   && ContactTypeId == other.ContactTypeId
                   && PreferredContactMethodType == other.PreferredContactMethodType
                   && IsPrimary == other.IsPrimary
                   && string.Equals(FirstName, other.FirstName)
                   && string.Equals(MiddleName, other.MiddleName)
                   && string.Equals(LastName, other.LastName)
                   && string.Equals(JobTitle, other.JobTitle)
                   && string.Equals(BusinessPhone, other.BusinessPhone)
                   && string.Equals(MobilePhone, other.MobilePhone)
                   && string.Equals(HomePhone, other.HomePhone)
                   && string.Equals(Email, other.Email)
                   && CreatedOn.Equals(other.CreatedOn)
                   && ModifiedOn.Equals(other.ModifiedOn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((Contact)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ EntityRefId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Employer != null ? Employer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ContactTypeId;
                hashCode = (hashCode * 397) ^ PreferredContactMethodType;
                hashCode = (hashCode * 397) ^ IsPrimary.GetHashCode();
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MiddleName != null ? MiddleName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (JobTitle != null ? JobTitle.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (BusinessPhone != null ? BusinessPhone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MobilePhone != null ? MobilePhone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HomePhone != null ? HomePhone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Email != null ? Email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CreatedOn.GetHashCode();
                hashCode = (hashCode * 397) ^ ModifiedOn.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(Contact contact1, Contact contact2)
        {
            if (ReferenceEquals(contact1, null))
            {
                return ReferenceEquals(contact2, null);
            }

            return contact1.Equals(contact2);
        }

        public static bool operator !=(Contact contact1, Contact contact2) =>
            !(contact1 == contact2);
    }
}