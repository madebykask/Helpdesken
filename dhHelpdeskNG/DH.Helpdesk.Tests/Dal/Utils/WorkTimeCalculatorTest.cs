namespace DH.Helpdesk.Tests.Dal.Utils
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Dal.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class WorkTimeCalculatorTest
    {
        private const int WorkingHourBegin = 8;

        private const int WorkingHourEnd = 18;

        private readonly HolidayOverview[] holidays = new HolidayOverview[] 
        { 
            //// full redday on Tuesday 3-th March 2015
            new HolidayOverview() 
                {  
                    HolidayDate = new DateTime(2015, 03, 3),
                    HolidayHeader = null
                }, 
            //// Red day on Sunday 8-th March 2015
            new HolidayOverview()
                {  
                    HolidayDate = new DateTime(2015, 03, 08),
                    HolidayHeader = null
                }, 
            //// Partially red day on Wednesday 11-th March 2015
            new HolidayOverview()
                {  
                    HolidayDate = new DateTime(2015, 03, 11),
                    HolidayHeader = null,
                    TimeFrom = 9,
                    TimeUntil = 12
                }, 
        };

        private const int DefaultDepartmentId = 123;

        [Test]
        public void GetRange1CrossingTest()
        {
            var workDateBegin = new DateTime(2015, 03, 09, WorkingHourBegin, 0, 0);
            var workDateEnd = new DateTime(2015, 03, 09, WorkingHourEnd, 0, 0);
            var begin2h20min = new DateTime(2015, 03, 09, 8, 0, 0);
            var end2h20min = new DateTime(2015, 03, 09, 10, 20, 0);
            var check2h20min = 140;
            Assert.AreEqual(check2h20min, WorkTimeCalculator.GetRange1Crossing(begin2h20min, end2h20min, workDateBegin, workDateEnd));
        }


        [Test]
        public void OnlyWorkdayCountTest()
        {
            var inst = this.InstantiateDefault();
            var oneDayStart = new DateTime(2015, 03, 06, 16, 0, 0);
            var oneDayEnd = new DateTime(2015, 03, 06, 19, 48, 0);
            const int OneDayResult = 120;

            var twoDayStart = new DateTime(2015, 03, 05, 16, 0, 0);
            var twoDayEnd = new DateTime(2015, 03, 06, 11, 23, 0);
            const int TwoDayResult = 323; /// 5 h 23 min

            var threeDayStart = new DateTime(2015, 03, 04, 17, 0, 0);
            var threeDayEnd = new DateTime(2015, 03, 06, 8, 41, 0);
            const int ThreeDayResult = 701; /// 11h 41 min

            Assert.AreEqual(OneDayResult, inst.CalcWorkTimeMinutes(DefaultDepartmentId, oneDayStart, oneDayEnd), "Calculation for one working day");
            Assert.AreEqual(TwoDayResult, inst.CalcWorkTimeMinutes(DefaultDepartmentId, twoDayStart, twoDayEnd), "Calculation for two working days");
            Assert.AreEqual(ThreeDayResult, inst.CalcWorkTimeMinutes(DefaultDepartmentId, threeDayStart, threeDayEnd), "Calculation for tree working days");
        }


        [Test]
        public void WorkTimeWithWeekendsTest()
        {
            var inst = this.InstantiateDefault();
            var friday = new DateTime(2015, 03, 13, 14, 05, 0);
            var monday = new DateTime(2015, 03, 16, 7, 40, 0);
            const int FridayToMondayResult = 235; // 3h 55 min
            Assert.AreEqual(FridayToMondayResult, inst.CalcWorkTimeMinutes(DefaultDepartmentId, friday, monday), "Include working hours only when weekends in range");

            var saturdayTwoWeekends = new DateTime(2015, 03, 14, 11, 00, 0);
            var sundayTwoWeekends = new DateTime(2015, 03, 22, 14, 05, 0);
            var satrudayToSundayTwoWeekends = 3000; // 5 days 10 hours 0 min
            Assert.AreEqual(satrudayToSundayTwoWeekends, inst.CalcWorkTimeMinutes(DefaultDepartmentId, saturdayTwoWeekends, sundayTwoWeekends), "Include working hours only when weekends in range with two weekends");

            var sundayStartBegin = new DateTime(2015, 03, 15, 11, 00, 0);
            var sundayStartEnd = new DateTime(2015, 03, 21, 14, 05, 0);
            var sundayStartResult = 3000; // 5 days 10 hours 0 min
            Assert.AreEqual(sundayStartResult, inst.CalcWorkTimeMinutes(DefaultDepartmentId, sundayStartBegin, sundayStartEnd), "Include working hours only when weekends in range with two weekends when range starts in Sunday");
        }


        [Test]
        public void WorkTimeWithRedDayTest()
        {
            var item = this.InstantiateDefault();
            var monday = new DateTime(2015, 03, 2, 7, 23, 58);
            var wednesday = new DateTime(2015, 03, 4, 12, 1, 0);
            var fullReddayRslt = 841; /// 14 hrs 1 min
            Assert.AreEqual(fullReddayRslt, item.CalcWorkTimeMinutes(DefaultDepartmentId, monday, wednesday), "Only working time should be counted within interal with redays");

            var tuesday = new DateTime(2015, 03, 10, 16, 30, 0);
            var thurstday = new DateTime(2015, 03, 12, 8, 30, 0);
            var partReddayRslt = 300; /// 3 hrs 0 min
            Assert.AreEqual(partReddayRslt, item.CalcWorkTimeMinutes(DefaultDepartmentId, tuesday, thurstday), "Only working time should be counted within interal with partial redays");

            var sunday = new DateTime(2015, 03, 8, 16, 30, 0);
            monday = new DateTime(2015, 03, 9, 9, 25, 0);
            var weekendReddayRslt = 85; /// 25 min
            Assert.AreEqual(weekendReddayRslt, item.CalcWorkTimeMinutes(DefaultDepartmentId, sunday, monday), "Only working time should be counted within interal with partial redays that crossings with weekends");
        }

        [Test]
        public void UnusualShiftsTest()
        {
            var item0_24 = new WorkTimeCalculator(0, 24, null, null);
            var mondayNoon = 
                new DateTime(2015, 03, 09, 12, 0, 0);
            var wednesdayMidnight = 
                new DateTime(2015, 03, 11, 0, 0, 0);
            const int Shift0_24Rslt = 2160; //// 36 hrs
            Assert.AreEqual(Shift0_24Rslt, item0_24.CalcWorkTimeMinutes(DefaultDepartmentId, mondayNoon, wednesdayMidnight));

            var item20_8 = new WorkTimeCalculator(20, 8, null, null);
            const int Shift20_8Rslt = 960; //// 16 hours
            Assert.AreEqual(Shift20_8Rslt, item20_8.CalcWorkTimeMinutes(DefaultDepartmentId, mondayNoon, wednesdayMidnight));
        }

        private WorkTimeCalculator InstantiateDefault()
        {
            var deptHolidays = new Dictionary<int, IList<HolidayOverview>> { { DefaultDepartmentId, this.holidays } };
            var item = new WorkTimeCalculator(WorkingHourBegin, WorkingHourEnd, deptHolidays, null);
            return item;
        }
    }
}