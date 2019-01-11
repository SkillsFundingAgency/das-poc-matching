using System.Collections.Generic;

namespace Esfa.Poc.Matching.Application.Comparers
{
    public class EmployerComparer : IEqualityComparer<Entities.Employer>
    {
        public bool Equals(Entities.Employer source, Entities.Employer target)
        {
            if (source == null && target == null)
                return true;

            if (source == null || target == null)
                return false;

            var isEqual = source.AlsoKnownAs.Equals(target.AlsoKnownAs) 
                          && source.CompanyType.Equals(target.CompanyType);

            return isEqual;
        }

        public int GetHashCode(Entities.Employer obj)
        {
            return obj.AlsoKnownAs.GetHashCode() 
                   ^ obj.CompanyType.GetHashCode();
        }
    }
}