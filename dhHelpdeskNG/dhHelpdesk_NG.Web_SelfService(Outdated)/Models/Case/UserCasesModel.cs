using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Models.Case
{
    public class UserCasesModel
    {
        public int CustomerId { get; set; }

        public int LanguageId { get; set; }

        public string UserId { get; set; }

        public string PharasSearch { get; set; }

        public int MaxRecords { get; set; }

        public CaseSearchResultModel CaseSearchResult { get; set; }
    }

    public class CaseSearchResultModel
    {
        public IList<CaseSettings> CaseSettings { get; set; }

        public IList<CaseSearchResult> Cases { get; set; }

        public CaseColumnsSettingsModel ColumnSettingModel { get; set; }
    }

    public class CaseColumnsSettingsModel
    {
        public IList<CaseSettings> UserColumns { get; set; }

        public IList<SelectListItem> LineList { get; set; }

        public IList<CaseFieldSettingsWithLanguage> CaseFieldSettingLanguages { get; set; }

    }

}