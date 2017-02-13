using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Common.Extensions.Boolean;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Mappers.ExternalInvoice.BusinessModelToEntity
{
	public class ExternalInvoiceToEntityMapper : IBusinessModelToEntityMapper<BusinessData.Models.ExternalInvoice.ExternalInvoice, CaseInvoiceRow>
	{
		public void Map(BusinessData.Models.ExternalInvoice.ExternalInvoice businessModel, CaseInvoiceRow entity)
		{
			if (entity == null)
			{
				return;
			}

			entity.Id = businessModel.Id;
			entity.Case_Id = businessModel.CaseId;
			entity.InvoiceNumber = businessModel.InvoiceNumber;
			entity.InvoicePrice = businessModel.InvoicePrice;
			entity.Charge = businessModel.Charge.ToInt();
			entity.CreatedDate = businessModel.CreatedDate;
			entity.CreatedByUser_Id = businessModel.CreatedByUserId;
		}
	}
}
