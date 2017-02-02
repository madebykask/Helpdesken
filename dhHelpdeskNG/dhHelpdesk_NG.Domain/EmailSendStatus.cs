using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain
{
	public enum EmailSendStatus
	{
		DoNotSend = 0,
		Pending = 1,
		Sent = 2
	}
}
