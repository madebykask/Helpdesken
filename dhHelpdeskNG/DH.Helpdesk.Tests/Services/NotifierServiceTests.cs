namespace DH.Helpdesk.Tests.Services
{
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Services.Services.Concrete;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public sealed class NotifierServiceTests
    {
        [Test]
        public void DeleteNotifierShouldDeleteNotifierById()
        {
            const int NotifierId = 7;

            var notifierRepositoryMock = new Mock<INotifierRepository>();
            var notifierService = new NotifierService(notifierRepositoryMock.Object, null, null, null);

            notifierService.DeleteNotifier(NotifierId);

            notifierRepositoryMock.Verify(r => r.DeleteById(NotifierId));
            notifierRepositoryMock.Verify(r => r.Commit());
        }
    }
}