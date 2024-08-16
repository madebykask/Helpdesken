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

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Impact.Output;
    using DH.Helpdesk.BusinessData.Models.Logs.Output;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.BusinessData.Models.Status.Output;
    using DH.Helpdesk.BusinessData.Models.Supplier.Output;
    using DH.Helpdesk.BusinessData.Models.Systems.Output;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The case overview.
    /// </summary>
    public sealed class CaseOverview
    {
        public int Id { get; set; }

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
        public int? UserId { get; set; }

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
        /// Gets or sets the persons email.
        /// </summary>
        public string PersonsEmail { get; set; }

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
        public IEnumerable<LogOverview> Logs { get; set; }

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

        /// <summary>
        /// Gets or sets this field.
        /// </summary>
        public OU Ou { get; set; }

        /// <summary>
        /// Gets or sets the inventory number.
        /// </summary>
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Gets or sets the inventory type.
        /// </summary>
        public string InventoryType { get; set; }

        /// <summary>
        /// Gets or sets the inventory location.
        /// </summary>
        public string InventoryLocation { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserOverview User { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the parent path case type.
        /// </summary>
        public string ParentPathCaseType { get; set; }

        /// <summary>
        /// Gets or sets the case type id.
        /// </summary>
        public int CaseTypeId { get; set; }

        /// <summary>
        /// Gets or sets the system id.
        /// </summary>
        public int? SystemId { get; set; }

        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        public SystemOverview System { get; set; }

        /// <summary>
        /// Gets or sets the urgency.
        /// </summary>
        public Urgency Urgency { get; set; }

        /// <summary>
        /// Gets or sets the impact id.
        /// </summary>
        public int? ImpactId { get; set; }

        /// <summary>
        /// Gets or sets the impact.
        /// </summary>
        public ImpactOverview Impact { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public CategoryOverview Category { get; set; }

        /// <summary>
        /// Gets or sets the supplier id.
        /// </summary>
        public int? SupplierId { get; set; }

        /// <summary>
        /// Gets or sets the supplier.
        /// </summary>
        public SupplierOverview Supplier { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the reference number.
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the miscellaneous.
        /// </summary>
        public string Miscellaneous { get; set; }

        /// <summary>
        /// Gets or sets the product area id.
        /// </summary>
        public int? ProductAreaId { get; set; }

        /// <summary>
        /// Gets or sets the product area.
        /// </summary>
        public ProductAreaOverview ProductArea { get; set; }

        /// <summary>
        /// Gets or sets the contact before action.
        /// </summary>
        public int ContactBeforeAction { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        public int Sms { get; set; }

        /// <summary>
        /// Gets or sets the agreed date.
        /// </summary>
        public DateTime? AgreedDate { get; set; }

        /// <summary>
        /// Gets or sets the available.
        /// </summary>
        public string Available { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the other cost.
        /// </summary>
        public int OtherCost { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the working group id.
        /// </summary>
        public int? WorkingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the working group.
        /// </summary>
        public WorkingGroupEntity WorkingGroup { get; set; }

        /// <summary>
        /// Gets or sets the case responsible user id.
        /// </summary>
        public int? CaseResponsibleUserId { get; set; }

        /// <summary>
        /// Gets or sets the case responsible user.
        /// </summary>
        public UserOverview CaseResponsibleUser { get; set; }

        /// <summary>
        /// Gets or sets the performer user id.
        /// </summary>
        public int? PerformerUserId { get; set; }

        /// <summary>
        /// Gets or sets the performer user.
        /// </summary>
        public UserOverview PerformerUser { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// Gets or sets the state secondary id.
        /// </summary>
        public int? StateSecondaryId { get; set; }

        /// <summary>
        /// Gets or sets the state secondary.
        /// </summary>
        public StateSecondary StateSecondary { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public StatusOverview Status { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        public ProjectOverview Project { get; set; }

        /// <summary>
        /// Gets or sets the problem id.
        /// </summary>
        public int? ProblemId { get; set; }

        /// <summary>
        /// Gets or sets the problem.
        /// </summary>
        public ProblemOverview Problem { get; set; }

        /// <summary>
        /// Gets or sets the change id.
        /// </summary>
        public int? ChangeId { get; set; }

        public int? MovedFromCustomer_Id { get; set; }

        /// <summary>
        /// Gets or sets the change.
        /// </summary>
        public ChangeOverview Change { get; set; }

        /// <summary>
        /// Gets or sets the watch date.
        /// </summary>
        public DateTime? WatchDate { get; set; }

        /// <summary>
        /// Gets or sets the verified.
        /// </summary>
        public int Verified { get; set; }

        /// <summary>
        /// Gets or sets the verified description.
        /// </summary>
        public string VerifiedDescription { get; set; }

        /// <summary>
        /// Gets or sets the solution rate.
        /// </summary>
        public string SolutionRate { get; set; }

        /// <summary>
        /// Gets or sets the finishing description.
        /// </summary>
        public string FinishingDescription { get; set; }

        /// <summary>
        /// Gets or sets the case histories.
        /// </summary>
        public ICollection<CaseHistory> CaseHistories { get; set; }

        /// <summary>
        /// Gets or sets the causing type id.
        /// </summary>
        public int? CausingTypeId { get; set; }

        /// <summary>
        /// Gets or sets the causing type.
        /// </summary>
        public CausingPartOverview CausingPart { get; set; }
    }
}