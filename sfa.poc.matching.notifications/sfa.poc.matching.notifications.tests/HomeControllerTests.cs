using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using sfa.poc.matching.notifications.Application.Interfaces;
using sfa.poc.matching.notifications.Controllers;
using sfa.poc.matching.notifications.ViewModels;
using System.Threading.Tasks;

namespace sfa.poc.matching.notifications.tests
{
    public class HomeControllerTests
    {
        [Test]
        public void HomeControllerIndexReturnsView()
        {
            var mockEmailService = new Mock<IEmailService>();
            var controller = new HomeController(mockEmailService.Object);
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EmailViewModel>(result.Model);
        }

        [Test]
        public async Task HomeControllerIndexPostCallsEmailServiceAndReturnsView()
        {
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService
                .Setup(x => x.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(default(object)));

            var controller = new HomeController(mockEmailService.Object);

            var email = new EmailViewModel()
            {
                EmailTo = "test@test.com",
                ReplyTo = "reply@test.com",
                Template = "TEST_TEMPLATE",
                //Tokens = (dynamic)new { contactname = "name" }
                Tokens = "name"
            };

            var result = await controller.Index(email) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EmailViewModel>(result.Model);

            mockEmailService.VerifyAll();
        }

    }
}