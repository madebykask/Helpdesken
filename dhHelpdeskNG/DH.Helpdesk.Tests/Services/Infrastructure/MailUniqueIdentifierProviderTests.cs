namespace DH.Helpdesk.Tests.Services.Infrastructure
{
    using System;
    using System.Net.Mail;

    using DH.Helpdesk.Services.Infrastructure;

    using NUnit.Framework;

    [TestFixture]
    public sealed class MailUniqueIdentifierProviderTests
    {
        [Test]
        public void Provide_DateCeedAndEmailCeed_ShouldMatchToThePattern()
        {
            // Arrange
            var dateCeed = DateTime.Now;
            var emailCeed = new MailAddress("rustam.singatov@fastdev.se");
            var mailUniqueIdentifierProvider = new MailUniqueIdentifierProvider();
            
            // Act
            var identifier = mailUniqueIdentifierProvider.Provide(dateCeed, emailCeed);

            // Assert
            Assert.That(identifier, Is.StringMatching(@"\[.*\]\[.*\]@fastdev.se"));
        }
    }
}
