﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain.Invoice;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceActionParam
	{
		public int Id { get; set; }
		public InvoiceStatus Status { get; set; }
	}
}