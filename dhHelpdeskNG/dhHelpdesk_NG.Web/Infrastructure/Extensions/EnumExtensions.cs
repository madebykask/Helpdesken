namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class EnumExtensions
    {
        public static SelectList ToSelectList(this Enum enumeration)
        {
            var list =
                (from Enum d in Enum.GetValues(enumeration.GetType())
                 select new { ID = Convert.ToInt32(d), Name = d.ToString() }).ToList();
            return new SelectList(list, "ID", "Name");
        }

        public static SelectList ToSelectList(this Enum enumeration, string selected)
        {
            var list =
                (from Enum d in Enum.GetValues(enumeration.GetType())
                 select new { ID = Convert.ToInt32(d), Name = d.ToString() }).ToList();
            return new SelectList(list, "ID", "Name", selected);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(
                   this HtmlHelper<TModel> htmlHelper,
                   Expression<Func<TModel, TProperty>> expression,
                   TEnum selectedValue)
        {
            var items = GetSelectListItems(selectedValue);
            return htmlHelper.DropDownListFor(expression, items);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            TEnum selectedValue,
            string optionLabel)
        {
            var items = GetSelectListItems(selectedValue);
            return htmlHelper.DropDownListFor(expression, items, optionLabel);
        }

        private static IEnumerable<SelectListItem> GetSelectListItems<TEnum>(TEnum selectedValue)
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var items = from value in values
                        select
                            new SelectListItem()
                                {
                                    Text = value.ToString(),
                                    Value = Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture),
                                    Selected = value.Equals(selectedValue)
                                };
            return items;
        }
    }
}