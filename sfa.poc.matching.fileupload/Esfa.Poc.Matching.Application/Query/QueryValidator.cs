using Esfa.Poc.Matching.Domain;
using FluentValidation;

namespace Esfa.Poc.Matching.Application.Query
{
    public class QueryValidator : AbstractValidator<FileUploadQuery>
    {
        public QueryValidator()
        {
            RuleFor(x => x.Company).NotEmpty();
        }
    }
}