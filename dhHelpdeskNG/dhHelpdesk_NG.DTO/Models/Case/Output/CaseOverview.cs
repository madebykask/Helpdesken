// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaseOverview.cs" company="">
//   
// </copyright>
// <summary>
//   The case overview.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    /// <summary>
    /// The case overview.
    /// </summary>
    public sealed class CaseOverview
    {
        /// <summary>
        /// Gets or sets the customer_ id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the finishing date.
        /// </summary>
        public DateTime? FinishingDate { get; set; }

        /// <summary>
        /// Gets or sets the deleted.
        /// </summary>
        public int Deleted { get; set; }

        /// <summary>
        /// Gets or sets the user_ id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the status_ id.
        /// </summary>
        public int? StatusId { get; set; }

        /// <summary>
        /// Gets or sets the case number.
        /// </summary>
        public decimal CaseNumber { get; set; }

        /// <summary>
        /// Gets or sets the reported by.
        /// </summary>
        public string ReportedBy { get; set; }

        /// <summary>
        /// Gets or sets the persons name.
        /// </summary>
        public string PersonsName { get; set; }

        /// <summary>
        /// Gets or sets the persons phone.
        /// </summary>
        public string PersonsPhone { get; set; }

        /// <summary>
        /// Gets or sets the persons cellphone.
        /// </summary>
        public string PersonsCellphone { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the logs.
        /// </summary>
        public ICollection<Log> Logs { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int? OuId { get; set; }

        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// Gets or sets the user code.
        /// </summary>
        public string UserCode { get; set; }
    }
}