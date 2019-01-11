using System;
using System.IO;
using Esfa.Poc.Matching.Application.Contact;
using Esfa.Poc.Matching.Application.Contact.Readers;
using Esfa.Poc.Matching.Domain;
using Xunit;

namespace Esfa.Poc.Matching.Application.IntegrationTests.Contacts
{
    [Trait("Contacts Excel", "Successfully loaded file")]
    public class ExcelLoadedSuccess
    {
        private readonly ContactLoadResult _contactLoadResult;

        private const string DataFilePath = "./Contacts/Contact-Simple.xlsx";
        private readonly FileUploadContact _firstRecord;

        public ExcelLoadedSuccess()
        {
            var fileReader = new ExcelContactFileReader();
            using (var stream = File.Open(DataFilePath, FileMode.Open))
            {
                _contactLoadResult = fileReader.Load(stream);
                _firstRecord = _contactLoadResult.Data[0];
            }
        }

        [Fact(DisplayName = "Correct number of records loaded")]
        public void CorrectNumberOfRecordsLoaded() =>
            Assert.Single(_contactLoadResult.Data);


        #region 1st Record Tests
        [Fact(DisplayName = "1st record - Contact is correct")]
        public void ContactIsCorrectForRecord1() =>
            Assert.Equal(new Guid("86d1233c-22b6-e611-80ce-000d3a219609"), _firstRecord.Contact);

        [Fact(DisplayName = "1st record - Checksum is correct")]
        public void ChecksumIsCorrectForRecord1() =>
            Assert.Equal("Checksum", _firstRecord.Checksum);

        [Fact(DisplayName = "1st record - Modified On is correct")]
        public void ModifiedOnIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2018, 12, 5, 14, 49, 0), _firstRecord.ModifiedOn);

        [Fact(DisplayName = "1st record - Company Name is correct")]
        public void CompanyNameIsCorrectForRecord1() =>
            Assert.Equal("Company Name", _firstRecord.CompanyName);

        [Fact(DisplayName = "1st record - Created On Company is correct")]
        public void CreatedOnCompanyIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2018, 12, 5, 14, 47, 0), _firstRecord.CreatedOnCompany);

        [Fact(DisplayName = "1st record - Contact Type is correct")]
        public void ContactTypeIsCorrectForRecord1() =>
            Assert.Equal("Employer", _firstRecord.ContactType);

        [Fact(DisplayName = "1st record - Phone Home is correct")]
        public void PhoneHomeIsCorrectForRecord1() =>
            Assert.Equal("7777722227", _firstRecord.PhoneHome);

        [Fact(DisplayName = "1st record - Job Title is correct")]
        public void JobTitleIsCorrectForRecord1() =>
            Assert.Equal("Manager", _firstRecord.JobTitle);

        [Fact(DisplayName = "1st record - Phone Mobile is correct")]
        public void PhoneMobileIsCorrectForRecord1() =>
            Assert.Equal("7777722228", _firstRecord.PhoneMobile);

        [Fact(DisplayName = "1st record - Modified By is correct")]
        public void ModifiedByIsCorrectForRecord1() =>
            Assert.Equal("Modified By", _firstRecord.ModifiedBy);

        [Fact(DisplayName = "1st record - Modified is correct")]
        public void ModifiedIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2018, 3, 12, 16, 50, 28), _firstRecord.Modified);

        [Fact(DisplayName = "1st record - Preferred Contact is correct")]
        public void PreferredContactIsCorrectForRecord1() =>
            Assert.Equal("Any", _firstRecord.PreferredContact);

        [Fact(DisplayName = "1st record - Full Name is correct")]
        public void FullNameIsCorrectForRecord1() =>
            Assert.Equal("Full Name", _firstRecord.FullName);

        [Fact(DisplayName = "1st record - First Name is correct")]
        public void FirstNameIsCorrectForRecord1() =>
            Assert.Equal("First Name", _firstRecord.FirstName);

        [Fact(DisplayName = "1st record - Middle Name is correct")]
        public void MiddleNameIsCorrectForRecord1() =>
            Assert.Equal("Middle Name", _firstRecord.MiddleName);

        [Fact(DisplayName = "1st record - Last Name is correct")]
        public void LastNameIsCorrectForRecord1() =>
            Assert.Equal("Last Name", _firstRecord.LastName);

        [Fact(DisplayName = "1st record - Phone Business is correct")]
        public void PhoneBusinessIsCorrectForRecord1() =>
            Assert.Equal("7777722229", _firstRecord.PhoneBusiness);

        [Fact(DisplayName = "1st record - Created By is correct")]
        public void CreatedByIsCorrectForRecord1() =>
            Assert.Equal("Created By", _firstRecord.CreatedBy);

        [Fact(DisplayName = "1st record - Created On is correct")]
        public void CreatedIsCorrectForRecord1() =>
            Assert.Equal(new DateTime(2016, 11, 29, 10, 54, 44), _firstRecord.Created);
        #endregion
    }
}