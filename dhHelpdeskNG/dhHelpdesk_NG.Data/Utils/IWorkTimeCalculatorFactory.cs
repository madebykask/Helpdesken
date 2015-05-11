namespace DH.Helpdesk.Dal.Utils
{
    using System;

    public interface IWorkTimeCalculatorFactory
    {
        WorkTimeCalculator Build(DateTime rangeBeginUTC, DateTime rangeEndUTC, int[] departmentsIds);
    }
}
