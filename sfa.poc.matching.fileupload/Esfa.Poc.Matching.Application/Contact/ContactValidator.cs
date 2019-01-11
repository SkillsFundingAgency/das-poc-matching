using Esfa.Poc.Matching.Domain;
using FluentValidation;

namespace Esfa.Poc.Matching.Application.Contact
{
    public class ContactValidator : AbstractValidator<FileUploadContact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty();
        }
    }
}