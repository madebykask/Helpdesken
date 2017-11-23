using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Models.Case
{
    public class InvoiceTimeModel
    {
        private string workingTime;
        private string overTime;
        private string equipmentPrice;
        private string price;
        private string invoiceStatus;

        public string Date { get; set; }
        public string Text { get; set; }
        public string WorkingTime {
            get {
                return workingTime;
            }
            set {
                int wt;                
                if (int.TryParse(value, out wt))
                    workingTime = wt.MinToTimeSpan();
                else
                    workingTime = value;
            }
        }
        public string OverTime
        {
            get
            {
                return overTime;
            }
            set
            {
                int ot;
                if (int.TryParse(value, out ot))
                    overTime = ot.MinToTimeSpan();
                else
                    overTime = value;
            }
        }
        public string EquipmentPrice
        {
            get
            {
                return equipmentPrice;
            }
            set
            {
                double ep;
                if (double.TryParse(value, out ep))
                    equipmentPrice = ep.ToString("### ###.##");
                else
                    equipmentPrice = value;
            }
        }
        public string Price {
            get
            {
                return price;
            }
            set
            {
                int p;
                if (int.TryParse(value, out p))
                    price = p.ToString("### ###.##");
                else
                    price = value;
            }
        }
        public string InvoiceStatus
        {
            get
            {
                return invoiceStatus;
            }
            set
            {
                int i;
                invoiceStatus = int.TryParse(value, out i) ? InvoiceTimeHelper.GetTextForInvoiceStatus(i) : value;
            }
        }        
    }

    public class InvoiceRowModel
    {        
        private string invoicePrice;        
        private string invoiceStatus;

        public string Date { get; set; }
        public string InvoiceNumber { get; set; }      
        public string InvoicePrice
        {
            get
            {
                return invoicePrice;
            }
            set
            {
                decimal ip;
                if (decimal.TryParse(value, out ip))
                    invoicePrice = ip.ToString("### ###.##");
                else
                    invoicePrice = value;
            }
        }      
        public string InvoiceStatus
        {
            get
            {
                return invoiceStatus;
            }
            set
            {
                int i;
                invoiceStatus = int.TryParse(value, out i)? InvoiceTimeHelper.GetTextForInvoiceStatus(i) : value;
            }
        }
    }


    public static class InvoiceTimeHelper
    {
        public static string GetTextForInvoiceStatus(int status)
        {
            switch (status)
            {
                case (int)InvoiceStatus.Ready:
                    return InvoiceStatus.Ready.GetTranslation(true);

                case (int)InvoiceStatus.Invoiced:
                    return InvoiceStatus.Invoiced.GetTranslation(true);
                    
                case (int)InvoiceStatus.NotInvoiced:
                    return InvoiceStatus.NotInvoiced.GetTranslation(true);
            };
            return "";
        }
    }
}