namespace DH.Helpdesk.Tests.Services.BusinessLogic.OtherTools
{
    using System;

    using DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete;

    using NUnit.Framework;

    [TestFixture]
    public sealed class TemporaryIdProviderTests
    {
        #region Public Methods and Operators

        [Test]
        public void ProvideTemporaryIdMustReturnValidGuid()
        {
            var provider = new TemporaryIdProvider();
            var temporaryId = provider.ProvideTemporaryId();
            Guid result;

            Assert.IsTrue(Guid.TryParse(temporaryId, out result));
        }

        #endregion
    }
}