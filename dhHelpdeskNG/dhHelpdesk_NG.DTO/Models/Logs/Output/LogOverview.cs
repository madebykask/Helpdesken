// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LogOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.User.Interfaces;

namespace DH.Helpdesk.BusinessData.Models.Logs.Output
{
    public sealed class LogOverview
    {
        public int Id { get; set; }

        public decimal EquipmentPrice { get; set; }

        public int Price { get; set; }

        public int CaseId { get; set; }

        public int? CaseHistoryId { get; set; }
        
        public int Charge { get; set; }

        public int Export { get; set; }

        public int? FinishingType { get; set; }

        public int InformCustomer { get; set; }

        public int LogType { get; set; }

        public int? UserId { get; set; }

        public int? ProblemId { get; set; }

        public int WorkingTime { get; set; }

        public int OverTime { get; set; }

        public string RegUser { get; set; }

        public string TextExternal { get; set; }

        public string TextInternal { get; set; }

        public DateTime ChangeTime { get; set; }

        public DateTime? ExportDate { get; set; }

        public DateTime? FinishingDate { get; set; }

        public DateTime LogDate { get; set; }

        public DateTime RegTime { get; set; }

        public Guid LogGuid { get; set; }

        public LogCaseHistoryOverview CaseHistory { get; set; }

        public LogUserOverview User { get; set; }

        public IList<LogFileOverview> LogFiles { get; set; }

        public IList<Mail2TicketOverview> Mail2Tickets { get; set; }

        #region Public Readonly Properties

        public bool IsProblemLog()
        {
            return ProblemId.HasValue;
        }

        #endregion
    }

    public class LogCaseHistoryOverview
    {
        public LogCaseHistoryOverview(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public IList<EmailLogsOverview> Emaillogs { get; set; }
    }

    public class LogFileOverview
    {
        public LogFileOverview(int id, string fileName, int? caseId = null, int? logId = null)
        {
            Id = id;
            FileName = fileName;
            CaseId = caseId;
            LogId = logId;
        }

        public int Id { get; }
        public string FileName { get; }
        public int? CaseId { get; set; }
        public int? LogId { get; set; }
    }

    public class LogUserOverview : IUserInfo
    {
        public LogUserOverview(int id, string firstName, string surName)
        {
            Id = id;
            FirstName = firstName;
            SurName = surName;
        }

        public int Id { get; }
        public string FirstName { get; }
        public string SurName { get; }
    }

    public class Mail2TicketOverview
    {
        public Mail2TicketOverview(int id, string type, string email, string subject)
        {
            Id = id;
            Type = type;
            Email = email;
            Subject = subject;
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}