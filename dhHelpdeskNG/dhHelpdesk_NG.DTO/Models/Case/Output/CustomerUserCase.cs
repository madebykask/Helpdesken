namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
	using OldComponents;
	using System;

	public sealed class CustomerUserCase
	{
		//public CustomerUserCase(
		//		int id,
		//		decimal caseNumber,
		//		DateTime registrationDate,
		//		DateTime changedDate,
		//		string subject,
		//		string initiatorName,
		//		string description,
		//		string customerName,
		//		string priorityName,
		//		string workingGroupName,
		//		string performerName,
		//		GlobalEnums.CaseIcon caseIcon)
		//{
		//	this.InitiatorName = initiatorName;
		//	this.Description = description;
		//	this.Subject = subject;
		//	this.RegistrationDate = registrationDate;
		//	this.ChangedDate = changedDate;
		//	this.Id = id;
		//	this.CaseNumber = caseNumber;
		//	this.CustomerName = customerName;
		//	this.PriorityName = priorityName;
		//	this.WorkingGroupName = workingGroupName;
		//	this.PerformerName = performerName;
		//	this.CaseIcon = caseIcon;
		
		//}

		public int Id { get; set; }

		public decimal CaseNumber { get; set; }

		public DateTime RegistrationDate { get; set; }

		public DateTime ChangedDate { get; set; }

		public string Subject { get; set; }

		public string CustomerName { get; set; }

		public string InitiatorName { get; set; }

		public string Description { get; set; }
		public string PriorityName { get; set; }
		public string WorkingGroupName { get; set; }
		public string PerformerName { get; set; }

		public string StateSecondaryName { get; set; }

		public DateTime? WatchDate { get; set; }
		public GlobalEnums.CaseIcon CaseIcon { get; set; }

		public bool Unread { get; set; }

		public bool IncludeInCaseStatistics { get; set; }

		public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }

		public int CustomerID { get; set; }

		public int? SolutionTime { get; set; }
		public int ExternalTime { get; internal set; }

        public string PersonsPhone { get; set; }

    }
}