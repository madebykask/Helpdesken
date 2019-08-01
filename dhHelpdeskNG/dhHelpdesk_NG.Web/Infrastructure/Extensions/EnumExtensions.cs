using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.BusinessData.Models.Shared;

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
        public static int ToInt(this Enum enumeration)
        {            
            return Convert.ToInt32(enumeration);
        }

        public static IList<ItemOverview> ToItemOverviewList(this Enum enumeration)
        {
            var list = (from Enum d in Enum.GetValues(enumeration.GetType())
                        select new ItemOverview(d.ToString(), Convert.ToInt32(d).ToString())).ToList();
            return list;
        }

        public static SelectList ToSelectList(this Enum enumeration, Func<string, string> translateFn)
        {
            var list = (from Enum d in Enum.GetValues(enumeration.GetType())
                        select new { ID = Convert.ToInt32(d), Name = Translation.GetCoreTextTranslation(d.ToString()) }).ToList();
            return new SelectList(list, "ID", "Name");
        }

        /// <summary>
        /// Takes text from enum and translates it.
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static SelectList ToSelectList(this Enum enumeration, string selected)
        {
            var list =
                (from Enum d in Enum.GetValues(enumeration.GetType())
                 select new { ID = Convert.ToInt32(d), Name = Translation.GetCoreTextTranslation(d.ToString()) }).ToList();
            return new SelectList(list, "ID", "Name", selected);
        }

        /// <summary>
        /// Takes text from DisplayAttribute and translates it.
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static SelectList ToSelectListDipslay(this Enum enumeration, string selected)
        {
            var type = enumeration.GetType();
            var list =
                (from Enum d in Enum.GetValues(enumeration.GetType())
                 select new { ID = Convert.ToInt32(d), Name = Translation.GetCoreTextTranslation(GetDisplayName(type, d)) }).ToList();
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

        public static string GetName<TEnum>(this TEnum value) where TEnum: struct
        {
            return Enum.GetName(typeof(TEnum), value);
        }

        public static string GetDisplayName(this Enum e)
		{
			var type = e.GetType();
			return GetDisplayName(type, e);
		}

		public static string GetTranslation(this Enum e, bool escapeQuotes = false)
		{
			var displyName = e.GetDisplayName();
			var translation = Translation.GetCoreTextTranslation(displyName);

			var res = !String.IsNullOrWhiteSpace(translation)
				? translation
				: !String.IsNullOrWhiteSpace(displyName) ? displyName : e.ToString();

			return escapeQuotes ? res.Replace("'", "\\'").Replace("\"", "\\'") : res;
		}

		public static List<SelectListItem> ToSelectListItems(this Enum e)
		{
			var type = e.GetType();

			return Enum.GetValues(type).Cast<Enum>()
				.Select(x => new SelectListItem
				{
					Text = x.GetTranslation(),
					Value = x.ToInt().ToString()
				}).ToList();
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

		private static string GetDisplayName(Type type, object value)
		{
			var fieldInfo = type.GetField(Enum.GetName(type, value));
			var displayNameAttributes = fieldInfo.GetCustomAttributes(
				typeof(DisplayAttribute), false) as DisplayAttribute[];

			if (displayNameAttributes != null && displayNameAttributes.Length > 0)
				return displayNameAttributes[0].Name;

			var descriptionAttributes = fieldInfo.GetCustomAttributes(
						typeof(DescriptionAttribute), false) as DescriptionAttribute[];
			if (descriptionAttributes == null) return Enum.GetName(type, value);

			return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : Enum.GetName(type, value);
		}
	}
}