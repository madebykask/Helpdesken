namespace DH.Helpdesk.Services.DisplayValues.Changes
{
    using System.Collections.Generic;
    using System.Text;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public sealed class ItemOverviewListDisplayValue : DisplayValue<List<ItemOverview>>
    {
        public ItemOverviewListDisplayValue(List<ItemOverview> value)
            : base(value)
        {
        }

        public override string GetDisplayValue()
        {
            if (this.Value == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var item in this.Value)
            {
                sb.AppendLine(string.Format("{0};", item.Name));
            }

            return sb.ToString();
        }
    }
}