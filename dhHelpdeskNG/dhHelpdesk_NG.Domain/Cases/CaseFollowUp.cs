using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Cases
{
	public class CaseFollowUp : Entity
	{
		public int UserId { get; set; }
		public int CaseId { get; set; }
		public DateTime FollowUpDate { get; set; }

		public bool IsActive { get; set; }

		public virtual User User { get; set; }
		public virtual Case Case { get; set; }
	}
}
