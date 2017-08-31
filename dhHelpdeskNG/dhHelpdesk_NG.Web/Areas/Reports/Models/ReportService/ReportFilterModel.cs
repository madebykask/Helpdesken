using DH.Helpdesk.BusinessData.Models.ReportService;
using DH.Helpdesk.BusinessData.Models.Shared;
using System;
using System.Collections.Generic;
namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{        
    public class ReportFilterJSModel
    {
        public ReportFilterJSModel()
        {
            
        }

        public string Customers { get; set; }

        public string Deps_OUs { get; set; }        

        public string WorkingGroups { get; set; }

        public string Administrators { get; set; }

        public string CaseTypes { get; set; }

        public string ProductAreas { get; set; }

        public string CaseStatus { get; set; }

        public DateTime? RegisterFrom { get; set; }

        public DateTime? RegisterTo { get; set; }

        public DateTime? CloseFrom { get; set; }

        public DateTime? CloseTo { get; set; }

        public string ReportCategory { get; set; }

        public string Fields { get; set; }
    }

    public static class ReportFilterMapper
    {
        private static string _SEPARATOR = ",";
        public static ReportSelectedFilter MapToSelectedFilter(this ReportFilterJSModel reportFilter)
        {
            var ret = new ReportSelectedFilter();
            ret.SelectedCustomers.AddItems(reportFilter.Customers);
            ret.SeletcedDepartments.AddItems(RetrieveDepartments(reportFilter.Deps_OUs));
            ret.SeletcedOUs.AddItems(RetrieveOUs(reportFilter.Deps_OUs));
            ret.SelectedWorkingGroups.AddItems(reportFilter.WorkingGroups);
            ret.SelectedAdministrator.AddItems(reportFilter.Administrators);
            ret.SelectedCaseTypes.AddItems(reportFilter.CaseTypes);
            ret.SelectedProductAreas.AddItems(reportFilter.ProductAreas);
            ret.SelectedCaseStatus.AddItems(reportFilter.CaseStatus);
            ret.CaseCreationDate = new DateToDate(reportFilter.RegisterFrom, reportFilter.RegisterTo);
            ret.CaseClosingDate = new DateToDate(reportFilter.CloseFrom, reportFilter.CloseTo);
            ret.SelectedReportCategory.AddItems(reportFilter.ReportCategory);
            return ret;
        }

        private static string RetrieveDepartments(string dep_OU)
        {
            string ret = string.Empty;
            if (string.IsNullOrEmpty(dep_OU))
                return ret;

            var aryNums = dep_OU.Split(_SEPARATOR.ToCharArray());
            if (aryNums.Length > 0)
            {
                var retIds = new List<string>();
                var index = 0;
                foreach (var num in aryNums)
                {
                    var curId = int.MinValue;
                    if (int.TryParse(num, out curId))
                        if (curId > 0 && curId != int.MinValue)                        
                            retIds.Add(curId.ToString());                        
                }
                ret = string.Join(_SEPARATOR, retIds);
            }
            return ret;
        }

        private static string RetrieveOUs(string dep_OU)
        {
            string ret = string.Empty;
            if (string.IsNullOrEmpty(dep_OU))
                return ret;

            var aryNums = dep_OU.Split(_SEPARATOR.ToCharArray());
            if (aryNums.Length > 0)
            {
                var retIds = new List<string>();
                var index = 0;
                foreach (var num in aryNums)
                {
                    var curId = int.MinValue;
                    if (int.TryParse(num, out curId))
                        if (curId < 0 && curId != int.MinValue)
                            retIds.Add(num.Replace("-",string.Empty));
                }
                ret = string.Join(_SEPARATOR, retIds);
            }
            return ret;
        }

    }
}