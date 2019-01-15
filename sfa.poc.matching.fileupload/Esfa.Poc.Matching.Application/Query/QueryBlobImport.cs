using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Entities;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Esfa.Poc.Matching.Application.Query
{
    public class QueryBlobImport : IBlobImport
    {
        private readonly QueryDataLoader _dataLoader;
        private readonly QueryDataValidator _dataValidator;

        public QueryBlobImport(QueryDataLoader dataLoader,
            QueryDataValidator dataValidator)
        {
            _dataLoader = dataLoader;
            _dataValidator = dataValidator;
        }

        public async Task<Result> Import(List<FileUpload> fileUploads, IAsyncCollector<string> output)
        {
            var returnResult = await _dataLoader.Load(fileUploads);
            var result = _dataValidator.Validate(returnResult.Object);

            if (!result.IsSuccess)
            {
                // TODO AU Error
            }

            foreach (var data in returnResult.Object)
                await output.AddAsync(JsonConvert.SerializeObject(data));

            return Result.Ok();
        }
    }
}