using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Models.Case
{
    public class InvoiceTimeModel
    {
        private string workingTime;
        private string overTime;
        private string equipmentPrice;
        private string price;
        private string invoiceRow_Id;

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
        public string InvoiceRow_Id {
            get
            {
                return invoiceRow_Id;
            }
            set
            {
                int i;
                if (int.TryParse(value, out i))
                    invoiceRow_Id = i > 0 ? Translation.GetCoreTextTranslation("Klara (fakturerade)") : "";
                else
                    invoiceRow_Id = value;
            }
        }        
    }

    public class InvoiceRowModel
    {        
        private string invoicePrice;        
        private string invoiceRow_Id;

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
        public string InvoiceRow_Id
        {
            get
            {
                return invoiceRow_Id;
            }
            set
            {
                int i;
                if (int.TryParse(value, out i))
                    invoiceRow_Id = i > 0 ? Translation.GetCoreTextTranslation("Klara (fakturerade)") : "";
                else
                    invoiceRow_Id = value;
            }
        }
    }
}