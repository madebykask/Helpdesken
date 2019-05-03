using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Models.Invoice;

namespace DH.Helpdesk.Web.Models.Case
{
    public class CaseLogViewModel
    {
        public int ShowInvoiceFields { get; set; }
        public int CustomerId { get; set; }
        public int? CaseCustomerId { get; set; }
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
        public IList<Mail2Ticket> Mail2Tickets { get; set; }

        public int ShowExternalInvoiceFields { get; set; }
        public List<ExternalInvoiceModel> ExternalInvoices { get; set; }

        public LogNoteFilesViewModel CreateFilesViewModel(LogOverview log)
        {
            return new LogNoteFilesViewModel(log, CaseFiles);
        }

        public LogNoteEmailsViewModel CreateEmailsViewModel(LogOverview log)
        {
            var logMail2Tickets = Mail2Tickets != null && Mail2Tickets.Any()
                ? Mail2Tickets.Where(m => m.Log_Id == log.Id).ToList()
                : new List<Mail2Ticket>();

            return new LogNoteEmailsViewModel(log, HelpdeskEmail, logMail2Tickets);
        }
    }

    public class LogNoteFilesViewModel
    {
        public LogOverview CurrentLog { get; }
        public CaseFilesModel CaseFiles { get; set; }

        public LogNoteFilesViewModel()
        {
        }

        public LogNoteFilesViewModel(LogOverview log, CaseFilesModel caseFiles)
        {
            CurrentLog = log;
            CaseFiles = caseFiles;
        }
    }

    public class LogNoteEmailsViewModel
    {
        public LogNoteEmailsViewModel()
        {
        }

        public LogNoteEmailsViewModel(LogOverview log, string helpdeskEmail, IList<Mail2Ticket> logMail2Tickets)
        {
            CurrentLog = log;
            HelpdeskEmail = helpdeskEmail;
            Mail2Tickets = logMail2Tickets;
        }
        
        public string HelpdeskEmail { get; set; }
        public LogOverview CurrentLog { get; set; }
        public IList<Mail2Ticket> Mail2Tickets { get; set; }
    }
}