namespace DH.Helpdesk.Tests.Services.Infrastructure
{
    using System;
    using System.Net.Mail;

    using DH.Helpdesk.Services.BusinessLogic;

    using NUnit.Framework;

    [TestFixture]
    public sealed class MailUniqueIdentifierProviderTests
    {
        [Test]
        public void ShouldReturnIdentifierMathesPatternByDateAndEmailCeed()
        {
            var dateCeed = DateTime.Now;
            var emailCeed = new MailAddress("rustam.singatov@fastdev.se");
            var mailUniqueIdentifierProvider = new MailUniqueIdentifierProvider();

            var identifier = mailUniqueIdentifierProvider.Provide(dateCeed, emailCeed);

            Assert.That(identifier, Is.StringMatching(@"\[.*\]\[.*\]@fastdev.se"));
        }
    }
}