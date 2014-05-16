namespace DH.Helpdesk.Tests.Services.Exceptions
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Services.Exceptions;

    using NUnit.Framework;

    [TestFixture]
    public sealed class MarksNotFoundExceptionTests
    {
        #region Public Methods and Operators

        [Test]
        public void ConstructorMustAssignValuesCorrectly()
        {
            var message = "Exception message.";
            var marks = new List<string> { "Mark 1" };

            var exception = new MarksNotFoundException(message, marks);

            Assert.AreSame(exception.Message, message);
            Assert.AreSame(exception.Marks, marks);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorMustThrowArgumentNullExceptionOnEmptyMarks()
        {
            new MarksNotFoundException("Exception message.", new List<string>(0));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorMustThrowArgumentNullExceptionOnNullMarks()
        {
            new MarksNotFoundException("Exception message.", null);
        }

        #endregion
    }
}