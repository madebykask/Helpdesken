using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public int SortOrder { get; set; }
        public int CaseId { get; set; }
        public Guid CaseDocumentGUID { get; set; }
        public int CaseDocumentTemplate_Id { get; set; }

        public List<DocumentConditionOverview> Conditions { get; set; }
    }
}