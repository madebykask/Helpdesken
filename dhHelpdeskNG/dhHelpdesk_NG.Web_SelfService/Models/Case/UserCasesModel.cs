using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;
using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.SelfService.Infrastructure;

namespace DH.Helpdesk.SelfService.Models.Case
{
    public interface ICaseSearchFilterModel
    {
        string ProgressId { get; set; }
        string PharasSearch { get; set; }
        IEnumerable<SelectListItem> ProgressItems { get; set; }
    }

    public class CaseSearchFilterModel : ICaseSearchFilterModel
    {
        public string ProgressId { get; set; }
        public string PharasSearch { get; set; }
        public IEnumerable<SelectListItem> ProgressItems { get; set; }
    }

    public class MultiCustomerUserFilterModel : CaseSearchFilterModel
    {
        public IEnumerable<UserCustomerOverview> Customers { get; set; }
    }

    public class UserCasesModel : CaseSearchFilterModel
    {
        public int CustomerId { get; set; }
    }

    public class CaseSearchResultModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public Enums.SortOrder SortOrder { get; set; }

        public string SortBy { get; set; }

        public int MaxRecords { get; set; }

        public IList<CaseSettings> CaseSettings { get; set; }

        public IList<CaseSearchResult> Cases { get; set; }

        public List<DynamicCase> DynamicCases { get; set; }

        public CaseColumnsSettingsModel ColumnSettingModel { get; set; }
    }

    public class CaseColumnsSettingsModel
    {
        public IList<CaseSettings> UserColumns { get; set; }

        public IList<SelectListItem> LineList { get; set; }

        public IList<CaseFieldSettingsWithLanguage> CaseFieldSettingLanguages { get; set; }

    }
}