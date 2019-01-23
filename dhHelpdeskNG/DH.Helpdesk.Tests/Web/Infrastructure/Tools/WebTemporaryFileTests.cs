using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Tests.Web.Infrastructure.Tools
{
    using System;

    using DH.Helpdesk.Web.Infrastructure.Tools;

    using NUnit.Framework;

    [TestFixture]
    public sealed class WebTemporaryFileTests
    {
        #region Public Methods and Operators

        [Test]
        public void ConstructorMustAcceptEmptyContent()
        {
            new WebTemporaryFile(new byte[0], "File.txt");
        }

        [Test]
        public void ConstructorMustAssignValuesCorrectly()
        {
            var content = new byte[0];
            var name = "File.txt";

            var temporaryFile = new WebTemporaryFile(content, name);

            Assert.AreSame(temporaryFile.Content, content);
            Assert.AreSame(temporaryFile.Name, name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorMustThrowArgumentNullExceptionOnEmptyName()
        {
            new WebTemporaryFile(new byte[0], string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorMustThrowArgumentNullExceptionOnNullContent()
        {
            new WebTemporaryFile(null, "File.txt");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorMustThrowArgumentNullExceptionOnNullName()
        {
            new WebTemporaryFile(new byte[0], null);
        }

        #endregion
    }
}