namespace DH.Helpdesk.Tests.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DH.Helpdesk.Dal.DbContext;
    using DH.Helpdesk.Domain;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public sealed class LanguageRepositoryTests
    {
        [Test]
        public void m()
        {
            var languages = (DbSet<Language>)new List<Language>().AsEnumerable();
            new Mock<HelpdeskDbContext>().Setup(c => c.Languages).Returns(languages);
        }
    }
}
