using System;


namespace DH.Helpdesk.Common.Enums
{
    public static class ApplicationTypes
    {
        public static readonly string Helpdesk = "helpdesk";

        public static readonly string LineManager = "linemanager";

        public static readonly string SelfService = "selfservice";

        public static readonly string HelpdeskMobile = "helpdeskmobile";
    }


    /// <summary>
    /// Current Application
    /// </summary>
    /// 
    [Serializable]
    [Flags]
    public enum ApplicationType
    {
        //Do not use 0, it is used in CaseSolutionCondition as ALL.
        Helpdesk = 1,
        LineManager = 2,
        SelfService = 3,
        HelpdeskMobile = 4
    }
}