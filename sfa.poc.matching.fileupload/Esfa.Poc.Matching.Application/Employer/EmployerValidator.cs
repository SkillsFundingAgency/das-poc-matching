using Esfa.Poc.Matching.Domain;
using FluentValidation;

namespace Esfa.Poc.Matching.Application.Employer
{
    public class EmployerValidator : AbstractValidator<FileUploadEmployer>
    {
        public EmployerValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty();
        }
    }
}