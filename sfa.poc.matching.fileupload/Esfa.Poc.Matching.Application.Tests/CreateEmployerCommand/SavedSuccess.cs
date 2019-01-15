//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Esfa.Poc.Matching.Application.Interfaces;
//using Xunit;

//namespace Esfa.Poc.Matching.Application.Tests.CreateEmployerCommand
//{
//    [Trait("CreateEmployerCommand", "Employer saved async successfully")]
//    public class SavedSuccess
//    {
//        private readonly IFileUploadContext _dbContextService;
//
//        public SavedSuccess()
//        {
//            _dbContextService = Substitute.For<IDbContextService>();
//            _dbContextService.SaveAsync().Returns(1);
//        }
//    }
//}