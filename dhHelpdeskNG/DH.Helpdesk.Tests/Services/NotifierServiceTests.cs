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
        public void DeleteNotifier_CorrectId_ShouldDelete()
        {
            // Arrange
            var notifierRepositoryMock = new Mock<INotifierRepository>();
            var notifierService = new NotifierService(notifierRepositoryMock.Object, null, null, null);

            // Act
            notifierService.DeleteNotifier(7);

            // Assert
            notifierRepositoryMock.Verify(r => r.DeleteById(7));
            notifierRepositoryMock.Verify(r => r.Commit());
        }
    }
}