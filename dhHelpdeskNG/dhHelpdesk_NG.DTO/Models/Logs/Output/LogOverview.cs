// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LogOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.BusinessData.Models.User.Interfaces;

namespace DH.Helpdesk.BusinessData.Models.Logs.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    /// <summary>
    /// The log overview.
    /// </summary>
    public sealed class LogOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the equipment price.
        /// </summary>
        public decimal EquipmentPrice { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Gets or sets the case_ id.
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// Gets or sets the case history id.
        /// </summary>
        public int? CaseHistoryId { get; set; }

        /// <summary>
        /// Gets or sets the charge.
        /// </summary>
        public int Charge { get; set; }

        /// <summary>
        /// Gets or sets the export.
        /// </summary>
        public int Export { get; set; }

        /// <summary>
        /// Gets or sets the finishing type.
        /// </summary>
        public int? FinishingType { get; set; }

        /// <summary>
        /// Gets or sets the inform customer.
        /// </summary>
        public int InformCustomer { get; set; }

        /// <summary>
        /// Gets or sets the log type.
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the working time.
        /// </summary>
        public int WorkingTime { get; set; }

        /// <summary>
        /// Gets or sets the over time.
        /// </summary>
        public int OverTime { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public string RegUser { get; set; }

        /// <summary>
        /// Gets or sets the text external.
        /// </summary>
        public string TextExternal { get; set; }

        /// <summary>
        /// Gets or sets the text internal.
        /// </summary>
        public string TextInternal { get; set; }

        /// <summary>
        /// Gets or sets the change time.
        /// </summary>
        public DateTime ChangeTime { get; set; }

        /// <summary>
        /// Gets or sets the export date.
        /// </summary>
        public DateTime? ExportDate { get; set; }

        /// <summary>
        /// Gets or sets the finishing date.
        /// </summary>
        public DateTime? FinishingDate { get; set; }

        /// <summary>
        /// Gets or sets the log date.
        /// </summary>
        public DateTime LogDate { get; set; }

        /// <summary>
        /// Gets or sets the registration time.
        /// </summary>
        public DateTime RegTime { get; set; }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        public Guid LogGuid { get; set; }

        /// <summary>
        /// Gets or sets the case history.
        /// </summary>
        //public CaseHistory CaseHistory { get; set; }
        public LogCaseHistoryOverview CaseHistory { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        //public User User { get; set; }
        public LogUserOverview User { get; set; }

        /// <summary>
        /// Gets or sets the log files.
        /// </summary>
        public IList<LogFileOverview> LogFiles { get; set; }

        /// <summary>
        /// Gets or sets the problem id.
        /// </summary>
        public int? ProblemId { get; set; }

        /// <summary>
        /// The is problem log.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsProblemLog()
        {
            return this.ProblemId.HasValue;
        }
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
        public string FileName { get;  }
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
}