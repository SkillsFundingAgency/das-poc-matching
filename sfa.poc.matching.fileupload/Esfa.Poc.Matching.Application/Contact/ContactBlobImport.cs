using Esfa.Poc.Matching.Application.Interfaces;
using Esfa.Poc.Matching.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Esfa.Poc.Matching.Common;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Esfa.Poc.Matching.Application.Contact
{
    public class ContactBlobImport : IBlobImport
    {
        public Result Import(Stream stream, FileUpload fileUpload,
            IAsyncCollector<string> output)
        {
            var fileExtension = Path.GetExtension(fileUpload.Path);
            var reader = ContactReaderFactory.Create(fileExtension);

            var loadResult = reader.Load(stream);

            if (!string.IsNullOrEmpty(loadResult.Error))
                return Result.Fail(loadResult.Error);

            var validator = new ContactValidator();

            var errors = new List<string>();
            foreach (var v in loadResult.Data)
            {
                var results = validator.Validate(v);
                if (results.IsValid)
                    continue;

                var errorMessage = $"{v.Contact} failed because {string.Join(", ", results.Errors.Select(e => e.ErrorMessage))}";
                errors.Add(errorMessage);
            }

            if (errors.Count > 0)
            {
                var error = string.Join("\r", errors.Select(e => e));
                return Result.Fail(error);
            }

            foreach (var data in loadResult.Data)
                output.AddAsync(JsonConvert.SerializeObject(data));

            return Result.Ok();
        }
    }
}