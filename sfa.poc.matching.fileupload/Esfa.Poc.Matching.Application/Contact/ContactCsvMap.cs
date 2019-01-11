using CsvHelper.Configuration;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Contact
{
    internal sealed class ContactCsvMap : ClassMap<FileUploadContact>
    {
        internal ContactCsvMap()
        {
            Map(m => m.Contact).Name(ContactColumnConstants.Contact);
            Map(m => m.Checksum).Name(ContactColumnConstants.Checksum);
            Map(m => m.ModifiedOn).Name(ContactColumnConstants.ModifiedOn);
            Map(m => m.CompanyName).Name(ContactColumnConstants.CompanyName);
            Map(m => m.CreatedOnCompany).Name(ContactColumnConstants.CreatedOnCompany);
            Map(m => m.ContactType).Name(ContactColumnConstants.ContactType);
            Map(m => m.PhoneHome).Name(ContactColumnConstants.HomePhone);
            Map(m => m.JobTitle).Name(ContactColumnConstants.JobTitle);
            Map(m => m.PhoneMobile).Name(ContactColumnConstants.MobilePhone);
            Map(m => m.ModifiedBy).Name(ContactColumnConstants.ModifiedBy);
            Map(m => m.Modified).Name(ContactColumnConstants.Modified);
            Map(m => m.PreferredContact).Name(ContactColumnConstants.PreferredContact);
            Map(m => m.FullName).Name(ContactColumnConstants.FullName);
            Map(m => m.FirstName).Name(ContactColumnConstants.FirstName);
            Map(m => m.MiddleName).Name(ContactColumnConstants.MiddleName);
            Map(m => m.LastName).Name(ContactColumnConstants.LastName);
            Map(m => m.PhoneBusiness).Name(ContactColumnConstants.PhoneBusiness);
            Map(m => m.CreatedBy).Name(ContactColumnConstants.CreatedBy);
            Map(m => m.Created).Name(ContactColumnConstants.Created);
        }
    }
}