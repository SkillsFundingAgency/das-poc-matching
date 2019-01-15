using System.Collections.Generic;
using System.Linq;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Query
{
    public class QueryDataValidator
    {
        public Result Validate(List<FileUploadQuery> fileData)
        {
            var validator = new QueryValidator();

            var errors = new List<string>();
            foreach (var v in fileData)
            {
                var results = validator.Validate(v);
                if (results.IsValid)
                    continue;

                var errorMessage = $"{v.CompanyTl} failed because {string.Join(", ", results.Errors.Select(e => e.ErrorMessage))}";
                errors.Add(errorMessage);
            }

            if (errors.Count > 0)
            {
                var error = string.Join("\r", errors.Select(e => e));
                return Result.Fail(error);
            }

            return Result.Ok();
        }
    }
}