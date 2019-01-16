using System.Collections.Generic;

namespace Esfa.Poc.Matching.Application.Comparers
{
    public class ContactComparer : IEqualityComparer<Entities.Contact>
    {
        public bool Equals(Entities.Contact source, Entities.Contact target)
        {
            if (source == null && target == null)
                return true;

            if (source == null || target == null)
                return false;

            var isEqual = source.BusinessPhone.Equals(target.BusinessPhone)
                          && source.HomePhone.Equals(target.HomePhone)
                          && source.MobilePhone.Equals(target.MobilePhone);

            return isEqual;
        }

        public int GetHashCode(Entities.Contact contact)
        {
            return contact.BusinessPhone.GetHashCode()
                   ^ contact.HomePhone.GetHashCode()
                   ^ contact.MobilePhone.GetHashCode();
        }
    }
}