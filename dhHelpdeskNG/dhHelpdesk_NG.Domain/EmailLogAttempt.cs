using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain
{
	public class EmailLogAttempt : Entity
	{
		public DateTime Date { get; set; }
		public string Message { get; set; }
		public int EmailLog_Id { get; set; }

		public virtual EmailLog EmailLog { get; set; }
	}
}
