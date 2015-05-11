namespace DH.Helpdesk.Tests.Web.Infrastructure
{
    using System;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Web.Infrastructure;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class WorkingTimeCalculatorFactoryTest
    {
        [Test]
        public void CreateFromWorkContextTest()
        {
            var context0_0Mock = new Mock<IWorkContext>();
            var context0_24Mock = new Mock<IWorkContext>();
            var cacheMock = new Mock<ICacheContext>();

            context0_0Mock.Setup(it => it.Customer.WorkingDayStart).Returns(0);
            context0_0Mock.Setup(it => it.Customer.WorkingDayEnd).Returns(0);
            context0_0Mock.Setup(it => it.Cache).Returns(cacheMock.Object);
            context0_24Mock.Setup(it => it.Customer.WorkingDayStart).Returns(0);
            context0_24Mock.Setup(it => it.Customer.WorkingDayEnd).Returns(24);
            context0_24Mock.Setup(it => it.Cache).Returns(cacheMock.Object);
            
            const int ExpectedWorkingHours = 2;
            var dateFrom = new DateTime(2015, 1, 1, 23, 0, 0);
            var dateTo = dateFrom.AddHours(ExpectedWorkingHours);
            var ins = WorkingTimeCalculatorFactory.CreateFromWorkContext(context0_0Mock.Object);
            Assert.AreEqual(ExpectedWorkingHours * 60, ins.CalcWorkTimeMinutes(null, dateFrom, dateTo), "Calc created from factory calcs fine for 0-0 working hours");

            ins = WorkingTimeCalculatorFactory.CreateFromWorkContext(context0_24Mock.Object);
            Assert.AreEqual(ExpectedWorkingHours * 60, ins.CalcWorkTimeMinutes(null, dateFrom, dateTo), "Calc created from factory calcs fine for 0-24 working hours");
        }
    }

}