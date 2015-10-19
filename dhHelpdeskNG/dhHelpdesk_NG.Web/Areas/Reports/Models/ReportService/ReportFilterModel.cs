using DH.Helpdesk.BusinessData.Models.ReportService;
using DH.Helpdesk.BusinessData.Models.Shared;
using System;
namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{        
    public class ReportFilterJSModel
    {
        public ReportFilterJSModel()
        {
            
        }

        public string Customers { get; set; }

        public string Departments { get; set; }

        public string WorkingGroups { get; set; }

        public string Administrators { get; set; }

        public string CaseTypes { get; set; }

        public DateTime? RegisterFrom { get; set; }

        public DateTime? RegisterTo { get; set; }        
    }

    public static class ReportFilterMapper
    {
        public static ReportSelectedFilter MapToSelectedFilter(this ReportFilterJSModel reportFilter)
        {
            var ret = new ReportSelectedFilter();
            ret.SelectedCustomers.AddItems(reportFilter.Customers);
            ret.SeletcedDepartments.AddItems(reportFilter.Departments);
            ret.SelectedWorkingGroups.AddItems(reportFilter.WorkingGroups);
            ret.SelectedAdministrator.AddItems(reportFilter.Administrators);
            ret.SelectedCaseTypes.AddItems(reportFilter.CaseTypes);
            ret.CaseCreationDate = new DateToDate(reportFilter.RegisterFrom, reportFilter.RegisterTo);
            return ret;
        }
    }
}