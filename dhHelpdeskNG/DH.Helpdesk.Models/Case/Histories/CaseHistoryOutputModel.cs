using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Web.Common.Constants.Case;

namespace DH.Helpdesk.Models.Case.Histories
{

    public class CaseHistoryOutputModel
    {
        public CaseHistoryOutputModel()
        {
            EmailLogs = new List<CustomKeyValue<int, CustomKeyValue<int, string>>>();
            Changes = new List<ICaseHistoryOutputModel>();
        }

        public List<CustomKeyValue<int, CustomKeyValue<int, string>>> EmailLogs { get; set; }
        public List<ICaseHistoryOutputModel> Changes { get; set; }
    }

    public class CaseHistoryItemOutputModel<T>: ICaseHistoryOutputModel
    {
        public CaseHistoryItemOutputModel()
        {
        }

        public int Id { get; set; }
        public string FieldName { get; set; } // CaseFieldsNamesApi
        public string FieldLabel { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public T PreviousValue { get; set; }
        public T CurrentValue { get; set; }
    }

    public interface ICaseHistoryOutputModel
    {
        int Id { get; set; }
        string FieldName { get; set; } // CaseFieldsNamesApi
        string FieldLabel { get; set; }
        DateTime CreatedAt { get; set; }
        string CreatedBy { get; set; }
    }
}