using System;
using System.Globalization;
using System.Threading;

namespace DH.Helpdesk.Web.Models.Invoice
{
    public class ExternalInvoiceModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
        
        public decimal Amount { get; set; }

	    public string AmountValue
	    {
	        get
	        {
	            var culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
	            culture.NumberFormat.NumberDecimalSeparator = culture.NumberFormat.CurrencyDecimalSeparator = culture.NumberFormat.PercentDecimalSeparator = ".";
	            culture.NumberFormat.NumberGroupSeparator = culture.NumberFormat.CurrencyGroupSeparator = culture.NumberFormat.PercentGroupSeparator = " ";
                var val = Amount.ToString("#.##", culture.NumberFormat);
	            return val;
	        }
	        set
	        {
	            var culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
	            culture.NumberFormat.NumberDecimalSeparator = culture.NumberFormat.CurrencyDecimalSeparator = culture.NumberFormat.PercentDecimalSeparator = ".";
	            culture.NumberFormat.NumberGroupSeparator = culture.NumberFormat.CurrencyGroupSeparator = culture.NumberFormat.PercentGroupSeparator = " ";
	            Amount = 
                    decimal.Parse(value, 
                        NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | 
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, culture);
	        }
	    }

        public bool Charge { get; set; }

		public InvoiceRowViewModel InvoiceRow { get; set; }
	}
}