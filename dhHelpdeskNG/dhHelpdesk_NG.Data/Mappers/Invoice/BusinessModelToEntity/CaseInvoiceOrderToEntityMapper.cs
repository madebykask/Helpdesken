namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceOrderToEntityMapper : IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>
    {
        public void Map(CaseInvoiceOrder businessModel, CaseInvoiceOrderEntity entity)
        {
            entity.InvoiceId = businessModel.InvoiceId;
            entity.Number = businessModel.Number;
            entity.InvoicedByUserId = businessModel.InvoicedByUserId;
            entity.InvoiceDate = businessModel.InvoiceDate;
            entity.Date = businessModel.Date;

            entity.ReportedBy = businessModel.ReportedBy;
            entity.Persons_Name = businessModel.Persons_Name;
            entity.Persons_Phone = businessModel.Persons_Phone;
            entity.Persons_Cellphone = businessModel.Persons_Cellphone;
            entity.Persons_Email = businessModel.Persons_Email;

            if (businessModel.Region_Id.HasValue)
            {
                entity.Region_Id = businessModel.Region_Id.Value;
            }
            else
            {
                entity.Region_Id = 0;
            }
            if (businessModel.Department_Id.HasValue)
            {
                entity.Department_Id = businessModel.Department_Id.Value;
            }
            else
            {
                entity.Department_Id = 0;
            }

            if (businessModel.OU_Id.HasValue)
            {
                entity.OU_Id = businessModel.OU_Id.Value;
            }
            else
            {
                entity.OU_Id = 0;
            }
            entity.Place = businessModel.Place;
            entity.CostCentre = businessModel.CostCentre;
            entity.UserCode = businessModel.UserCode;
            entity.CreditForOrder_Id = businessModel.CreditForOrder_Id;
            entity.Project_Id = businessModel.Project_Id;
            entity.OrderState = businessModel.OrderState;
        }
    }
}