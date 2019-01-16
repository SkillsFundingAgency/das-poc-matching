using System;
using Esfa.Poc.Matching.Application.Enums;
using Esfa.Poc.Matching.Domain;
using Humanizer;

namespace Esfa.Poc.Matching.Application.Mappers
{
    public class ContactMapper
    {
        public static Entities.Contact Map(FileUploadContact fileContact,
            Entities.Contact contact)
        {
            if (contact == null)
            {
                contact = new Entities.Contact();
                contact.Id = Guid.NewGuid();
            }

            var contactType = GetContactType(fileContact.ContactType);
            var preferredMethodOfContact = GetPreferredMethodOfContact(fileContact.PreferredContact);

            contact.ContactTypeId = (int)contactType;
            contact.PreferredContactMethodType = (int)preferredMethodOfContact;
            //contact.IsPrimary
            contact.FirstName = fileContact.FirstName;
            contact.MiddleName = fileContact.MiddleName;
            contact.LastName = fileContact.LastName;
            contact.JobTitle = fileContact.JobTitle;
            contact.BusinessPhone = fileContact.PhoneBusiness;
            contact.MobilePhone = fileContact.PhoneMobile;
            contact.HomePhone = fileContact.PhoneHome;
            //contact.Email = fileContact.Email;
            contact.CreatedOn = fileContact.Created;
            contact.ModifiedOn = fileContact.ModifiedOn;

            return contact;
        }

        #region Private Methods
        private static ContactType GetContactType(string contactTypeFromFile)
        {
            var contactTypeDehumanised = contactTypeFromFile.Dehumanize();
            Enum.TryParse(contactTypeDehumanised, out ContactType contactType);

            return contactType;
        }

        private static PreferredMethodOfContact GetPreferredMethodOfContact(string preferredMethodOfContactFromFile)
        {
            var preferredMethodOfContactDehumanised = preferredMethodOfContactFromFile.Dehumanize();
            Enum.TryParse(preferredMethodOfContactDehumanised, out PreferredMethodOfContact preferredMethodOfContact);

            return preferredMethodOfContact;
        }
        #endregion
    }
}