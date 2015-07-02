namespace DH.Helpdesk.BusinessData.Models.WorktimeCalculator
{
    using System;

    public interface IWorkTimeCalculatorFactory
    {
        WorkTimeCalculator Build(DateTime rangeBeginUTC, DateTime rangeEndUTC, int[] departmentsIds);
    }
}
