using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using sfa.poc.matching.notifications.Application.Configuration;
using sfa.poc.matching.notifications.Application.Interfaces;
using sfa.poc.matching.notifications.Application.Services;
using System.Threading.Tasks;

namespace sfa.poc.matching.notifications.tests
{
    public class GovNotifyEmailServiceTests
    {
        [Test]
        public async Task GovNotifyEmailServiceDoesSomething()
        {
            var configuration = new MatchingConfiguration
            {
                GovNotifyApiKey = "test_key"
            };

            //var mockEmailService = new Mock<IEmailService>();

            var templateName = "MY_TEMPLATE";
            var toAddress = "to.someone@test.com";
            dynamic tokens = new { contactname = "name" };
            var replyToAddress = "reply.to.me@test.com";

            //TODO: Add mocks for the rest of the parameters and fix test

            //var emailService = new GovNotifyEmailService(configuration);
            
            //emailService.SendEmail(templateName, toAddress, tokens, replyToAddress);

            //var mockEmailService = new Mock<IEmailService>();
            //var controller = new HomeController(mockEmailService.Object);
            //var result = controller.Index() as ViewResult;
            //Assert.IsNotNull(result);
            //Assert.IsInstanceOf<EmailViewModel>(result.Model);
        }

        //[Test]
        //public async Task HomeControllerIndexPostCallsEmailServiceAndReturnsView()
        //{
        //var mockEmailService = new Mock<IEmailService>();
        //mockEmailService
        //    .Setup(x => x.SendEmail(
        //        It.IsAny<string>(),
        //        It.IsAny<string>(),
        //        It.IsAny<object>(),
        //        It.IsAny<string>()))
        //    .Returns(Task.FromResult(default(object)));

        //var controller = new HomeController(mockEmailService.Object);

        //var email = new EmailViewModel()
        //{
        //    EmailTo = "test@test.com",
        //    ReplyTo = "reply@test.com",
        //    Template = "TEST_TEMPLATE",
        //    //Tokens = (dynamic)new { contactname = "name" }
        //    Tokens = "name"
        //};

        //var result = await controller.Index(email) as ViewResult;
        //Assert.IsNotNull(result);
        //Assert.IsInstanceOf<EmailViewModel>(result.Model);

        //mockEmailService.VerifyAll();
        //}

    }
}