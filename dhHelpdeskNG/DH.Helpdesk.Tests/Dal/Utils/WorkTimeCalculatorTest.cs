namespace DH.Helpdesk.Tests.Dal.Utils
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Utils;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class WorkTimeCalculatorTest
    {
        private const int DEFAULT_CALENDAR_DEPT_ID = 2;

        private readonly DateTime Apr27 = new DateTime(2015, 4, 27);
        private readonly DateTime May5 = new DateTime(2015, 5, 5);
        private readonly DateTime Apr30 = new DateTime(2015, 4, 30);
        private readonly DateTime May1 = new DateTime(2015, 5, 1);

        private HolidayOverview[] GetDeptCalendar()
        {
            return new[]
                       {
                           new HolidayOverview() { HolidayDate = Apr30, DepartmentId = 1, TimeFrom = 20, TimeUntil = 24 },
                           new HolidayOverview() { HolidayDate = May1, DepartmentId = 1, TimeFrom = 0, TimeUntil = 0 },
                           new HolidayOverview() { HolidayDate = Apr30, DepartmentId = 3, TimeFrom = 0, TimeUntil = 0 },
                       };
        }

        private readonly IEnumerable<HolidayOverview> defaultCalendar = new[]
                                                                            {
                                                                                new HolidayOverview()
                                                                                    {
                                                                                        HolidayDate = new DateTime(2015, 5, 1),
                                                                                        TimeFrom = 0,
                                                                                        TimeUntil = 0
                                                                                    }
                                                                            };
        
        [Test]
        public void NormalWorkingHoursTest()
        {
            var calc = this.MakeTestCalc(8, 17);
            var mondayNoon = new DateTime(2015, 5, 4, 12, 0, 0);

            Assert.AreEqual(15 * 60, calc.CalculateWorkTime(Apr30, mondayNoon, DEFAULT_CALENDAR_DEPT_ID), "take in account holiday calendar");
        }

        [Test]
        public void _24HoursTest()
        {
            var calc = this.MakeTestCalc(0, 24);
            var Apr30 = new DateTime(2015, 4, 30);
            Assert.AreEqual(2 * 24 * 60, calc.CalculateWorkTime(Apr30, May5, DEFAULT_CALENDAR_DEPT_ID), "take in account holiday calendar");

            calc = this.MakeTestCalc(22, 22);
            Assert.AreEqual(48 * 60, calc.CalculateWorkTime(Apr30, May5, DEFAULT_CALENDAR_DEPT_ID), "take in account holiday calendar");
        }
        
        [Test]
        public void ShiftDevidedInDayTest()
        {
            var calc = this.MakeTestCalc(23, 4);
            var mondayNoon = new DateTime(2015, 5, 4, 12, 0, 0);
            
            Assert.AreEqual(2 * 60, calc.CalculateWorkTime(May1, mondayNoon, DEFAULT_CALENDAR_DEPT_ID));
            Assert.AreEqual(7 * 60, calc.CalculateWorkTime(Apr30, mondayNoon, DEFAULT_CALENDAR_DEPT_ID));
            Assert.AreEqual(6 * 60, calc.CalculateWorkTime(Apr30, mondayNoon, 1), "should take calendar for deptID = 1");
        }
        
        #region private methods

        private WorkTimeCalculator MakeTestCalc(int workdayBegin, int workdayEnd)
        {
            var departmentsIds = new[] { 1, DEFAULT_CALENDAR_DEPT_ID, 3 };
            var holidaysService = new Mock<IHolidayService>();
            holidaysService.Setup(it => it.GetDefaultCalendar()).Returns(this.defaultCalendar);
            holidaysService.Setup(it => it.GetHolidayBetweenDatesForDepartments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int[]>()))
                .Returns(this.GetDeptCalendar());
            var usersTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var calcFactory = new WorkTimeCalculatorFactory(holidaysService.Object, workdayBegin, workdayEnd, usersTimeZone);
            var res = calcFactory.Build(
                TimeZoneInfo.ConvertTimeToUtc(Apr27, usersTimeZone),
                TimeZoneInfo.ConvertTimeToUtc(this.May5, usersTimeZone),
                departmentsIds);
            return res;
        }

        #endregion
    }
}