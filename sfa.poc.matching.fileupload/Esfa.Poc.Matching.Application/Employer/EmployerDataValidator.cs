using System.Collections.Generic;
using System.Linq;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Employer
{
    public class EmployerDataValidator
    {
        public Result Validate(List<FileUploadEmployer> fileData)
        {
            var validator = new EmployerValidator();

            var errors = new List<string>();
            foreach (var v in fileData)
            {
                var results = validator.Validate(v);
                if (results.IsValid)
                    continue;

                var errorMessage = $"{v.Account} failed because {string.Join(", ", results.Errors.Select(e => e.ErrorMessage))}";
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