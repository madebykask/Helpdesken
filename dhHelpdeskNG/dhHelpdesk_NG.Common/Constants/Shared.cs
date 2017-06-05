﻿namespace DH.Helpdesk.Common.Constants
{
    public static class Text
    {
        public const string WorkflowStep = "Arbetsflödessteg";
        public const string SelectStep = "-- Välj --";
    }

    public static class Cache
    {
        public const int Duration = 60;
    }

        public static class CacheKey
    {
        public const string CaseSolutionCondition = "CaseSolutionCondition{0}";
        public const string DepartmentsByUserPermissions = "DepartmentsByUserPermissions{0}";
    }

    public static class CaseTabs
    {
        public const string CaseTab = "case-tab";
        public const string ChildCasesTab = "childcases-tab";
        public const string ExtendedCaseTab = "extended-case-tab{0}"; //replace {0} with id of extended case
    }

}