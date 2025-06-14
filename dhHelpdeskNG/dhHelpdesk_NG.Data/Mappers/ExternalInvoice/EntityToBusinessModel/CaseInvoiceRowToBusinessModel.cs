﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Mappers.ExternalInvoice.EntityToBusinessModel
{
	public class CaseInvoiceRowToBusinessModel : IEntityToBusinessModelMapper<CaseInvoiceRow, BusinessData.Models.ExternalInvoice.ExternalInvoice>
	{
		public BusinessData.Models.ExternalInvoice.ExternalInvoice Map(CaseInvoiceRow entity)
		{
			return new BusinessData.Models.ExternalInvoice.ExternalInvoice
			{
				Id = entity.Id,
				CaseId = entity.Case_Id,
				InvoiceNumber = entity.InvoiceNumber,
				InvoicePrice = entity.InvoicePrice,
				Charge = entity.Charge.ToBool(),
				CreatedDate = entity.CreatedDate,
				CreatedByUserId = entity.CreatedByUser_Id,
				InvoiceRow = entity.InvoiceRow == null ? null : new BusinessData.Models.Invoice.InvoiceRow { Status = entity.InvoiceRow.Status }
			};
		}
	}
}
