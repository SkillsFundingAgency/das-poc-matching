using System;
using sfa.poc.matching.functions.core.Interfaces;

namespace sfa.poc.matching.functions.application.Services
{
    public class TestService : ITestService
    {
        private readonly IMatchingConfiguration _configuration;
        private readonly ITestRepository _repository;

        public TestService(IMatchingConfiguration configuration, ITestRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public TestResponse GetMessage()
        {
            return new TestResponse
            {
                Message = $"Connection: {_configuration.SqlConnectionString}",
                EnvironmentName = Environment.GetEnvironmentVariable("EnvironmentName"),
                ServiceName = Environment.GetEnvironmentVariable("ServiceName"),
                Version = Environment.GetEnvironmentVariable("Version"),
                AdditionalData = GetData()
            };
        }

        private string GetData()
        {
            var data = _repository.GetData();
            return string.Join(",", data);
        }
    }
}
