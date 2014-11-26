namespace DH.Helpdesk.Services.DisplayValues
{
    using System.Linq.Dynamic;
    using System.Text;

    public class StringsDisplayValue : DisplayValue<string[]>
    {
        public StringsDisplayValue(string[] value, string separator)
            : base(value)
        {
            this.Separator = separator;
        }

        private string Separator { get; set; }

        public static explicit operator StringsDisplayValue(string[] value)
        {
            var displayValue = new StringsDisplayValue(value, string.Empty);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            var sb = new StringBuilder();
            if (this.Value != null && this.Value.Any())
            {
                for (var i = 0; i < this.Value.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(this.Separator);
                    }

                    sb.Append(this.Value[i]);
                }
            }

            return sb.ToString();
        }
    }
}