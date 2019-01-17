using System;
using Esfa.Poc.Matching.Application.Enums;
using Esfa.Poc.Matching.Domain;
using Humanizer;

namespace Esfa.Poc.Matching.Application.Mappers
{
    public class EmployerMapper
    {
        public static Entities.Employer Map(FileUploadEmployer fileEmployer,
            Entities.Employer employer)
        {
            if (employer == null)
            {
                employer = new Entities.Employer
                {
                    Id = fileEmployer.Account
                };
            }

            Enum.TryParse(fileEmployer.Aupa, out AupaStatus aupa);
            var companyType = GetCompanyType(fileEmployer.CompanyType);

            employer.AlsoKnownAs = fileEmployer.CompanyAka;
            employer.AupaStatus = (int)aupa;
            employer.CompanyName = fileEmployer.CompanyName;
            employer.CompanyType = (int)companyType;
            employer.CreatedOn = fileEmployer.Created;

            return employer;
        }

        #region Private Methods
        private static CompanyType GetCompanyType(string companyTypeFromFile)
        {
            var companyTypeDehumanised = companyTypeFromFile.Dehumanize();
            Enum.TryParse(companyTypeDehumanised, out CompanyType companyType);

            return companyType;
        }
        #endregion
    }
}