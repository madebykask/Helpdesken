using System;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
    public class ConditionResult
    {
        #region ctors()

        public ConditionResult()
        {
        }

        public ConditionResult(bool show)
        {
            Show = show;
        }

        #endregion

        public bool Show { get; set; }
        public Exception FieldException { get; set; }

        #region Static Methods

        public static ConditionResult Success()
        {
            return new ConditionResult()
            {
                Show = true
            };
        }

        public static ConditionResult Error(Exception ex)
        {
            return new ConditionResult()
            {
                Show = false,
                FieldException = ex
            };
        }

        #endregion
    }
}
