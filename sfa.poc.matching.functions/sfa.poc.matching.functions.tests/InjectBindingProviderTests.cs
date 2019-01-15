using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using sfa.poc.matching.functions.application.Services;
using sfa.poc.matching.functions.extensions;

namespace sfa.poc.matching.functions.tests
{
    public class InjectBindingProviderTests
    {
        [Test]
        public async Task InjectBindingProviderDoesSomething()
        {
            try
            {
                var mockService = new Mock<ITestService>();
                var mockServiceProvider = new Mock<IServiceProvider>();
                mockServiceProvider
                    .Setup(p => p.GetService(typeof(ITestService)))
                    .Returns(mockService.Object);

                //mockServiceProvider
                //    .Setup(p => p.GetService(typeof(IFunctionFilter)))
                //    .Returns<IFunctionFilter>(x =>
                //    {
                //        return new ScopeCleanupFilter();
                //    });

                var mockServiceScope = new Mock<IServiceScope>();
                mockServiceScope
                    .Setup(s => s.ServiceProvider)
                    .Returns(mockServiceProvider.Object);

                var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
                mockServiceScopeFactory
                    .Setup(f => f.CreateScope())
                    .Returns(mockServiceScope.Object);

                mockServiceProvider
                    .Setup(p => p.GetService(typeof(IServiceScopeFactory)))
                    .Returns(mockServiceScopeFactory.Object);

                var method = typeof(HttpTestFunction).GetMethod("Run");
                var parameters = method.GetParameters();
               
                var parameterInfo = parameters.First(x => x.CustomAttributes.Count() > 0 && x.CustomAttributes.Any(c => c.AttributeType == typeof(InjectAttribute)));

                var provider = new InjectBindingProvider(mockServiceProvider.Object);
                var cancellationToken = new CancellationToken();

                var context = new BindingProviderContext(
                    parameterInfo,
                    null,
                    cancellationToken);

                var binding = await provider.TryCreateAsync(context);

                Assert.IsNotNull(binding);

                var bindingContext = new BindingContext(
                    new ValueBindingContext(
                        new FunctionBindingContext(Guid.NewGuid(), cancellationToken),
                        cancellationToken), 
                    null);
                var valueProvider = await binding.BindAsync(bindingContext);

                Assert.IsNotNull(valueProvider);

                var result = await valueProvider.GetValueAsync();

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ITestService>(result);
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e);
                throw;
            }
        }
    }
}