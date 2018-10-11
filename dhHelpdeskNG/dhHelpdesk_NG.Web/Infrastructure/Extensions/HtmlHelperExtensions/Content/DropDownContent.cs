using System.Linq;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    using System;
    using System.Collections.Generic;

    public sealed class DropDownContent
    {
        #region ctor()

        public DropDownContent(IList<SelectListItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), "Value cannot be null.");

            Items = items.Select(x => new DropDownItem(x.Text, x.Value)).ToList();
            SelectedValue = items.FirstOrDefault(x => x.Selected)?.Value;
        }

        public DropDownContent(List<DropDownItem> items)
            : this(items, null)
        {
        }

        public DropDownContent(List<DropDownItem> items, string selectedValue)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), "Value cannot be null.");

            this.Items = items;
            this.SelectedValue = selectedValue;
        }

        #endregion

        #region Public Properties

        public List<DropDownItem> Items { get; private set; }

        public string SelectedValue { get; private set; }

        #endregion
    }
}