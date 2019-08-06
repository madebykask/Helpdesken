using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Models.Invoice;

namespace DH.Helpdesk.Web.Models.Case
{
    public class CaseLogViewModel
    {
        public int ShowInvoiceFields { get; set; }
        public int CustomerId { get; set; }
        public int? CaseCustomerId { get; set; }
        public int CaseNumber { get; set; }
        public int CurrentCaseLanguageId { get; set; }
        public TimeZoneInfo UserTimeZone { get; set; }
        public bool IsCaseReopened { get; set; }
        public Department Department { get; set; }
        public CaseFilesModel CaseFiles { get; set; }
        public string HelpdeskEmail { get; set; }
        public bool CaseInternalLogAccess { get; set; }
        public IEnumerable<LogOverview> Logs { get; set; }
        public Setting Setting { get; set; }
        public CustomerSettings CustomerSettings { get; set; }
        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }
        public IList<CaseSolutionSettingModel> CaseSolutionSettingModels { get; set; }
        
        public int ShowExternalInvoiceFields { get; set; }
        public List<ExternalInvoiceModel> ExternalInvoices { get; set; }
        public bool IsTwoAttachmentsMode { get; set; }
        public CaseFilesUrlBuilder FilesUrlBuilder { get; set; }

        public LogNoteFilesViewModel CreateFilesViewModel(LogOverview log)
        {
            return new LogNoteFilesViewModel(CaseNumber, log, CaseFiles, FilesUrlBuilder, IsTwoAttachmentsMode);
        }

        public LogNoteEmailsViewModel CreateEmailsViewModel(LogOverview log)
        {
            return new LogNoteEmailsViewModel(log, HelpdeskEmail);
        }
    }

    public class LogNoteFilesViewModel
    {
        public int CaseNumber { get; set; }
        public LogOverview CurrentLog { get; }
        public CaseFilesModel CaseFiles { get; set; }
        public CaseFilesUrlBuilder FilesUrlBuilder { get; }
        public bool IsTwoAttachmentsMode { get; set; }

        public LogNoteFilesViewModel(int caseNumber, LogOverview log, CaseFilesModel caseFiles, CaseFilesUrlBuilder filesUrlBuilder, bool isTwoAttachmentsMode)
        {
            CaseNumber = caseNumber;
            CurrentLog = log;
            CaseFiles = caseFiles;
            FilesUrlBuilder = filesUrlBuilder;
            IsTwoAttachmentsMode = isTwoAttachmentsMode;
        }
    }

    public class LogNoteEmailsViewModel
    {
        public LogNoteEmailsViewModel()
        {
        }

        public LogNoteEmailsViewModel(LogOverview log, string helpdeskEmail)
        {
            CurrentLog = log;
            HelpdeskEmail = helpdeskEmail;
        }
        
        public string HelpdeskEmail { get; set; }
        public LogOverview CurrentLog { get; set; }
    }
}