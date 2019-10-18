using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
	public static class SelectExtensions
	{
	    public static List<SelectListItem> ToSelectList<K,V>(this IDictionary<K,V> dic, SelectListItem defaultItem = null)
	    {
	        if (dic == null)
	            return null;

            var listItems = new List<SelectListItem>();
	        if (defaultItem != null)
	        {
	            listItems.Add(defaultItem);
	        }

	        listItems.AddRange(
	            dic.Select(kv => new SelectListItem()
	            {
	                Value = kv.Key.ToString(),
	                Text = kv.Value?.ToString() ?? string.Empty,
	            }));

	        return listItems;
	    }

        public static SelectList Translate(this SelectList list)
        {
            foreach (var selectListItem in list)
                selectListItem.Text = Translation.GetCoreTextTranslation(selectListItem.Text);
            return list;
        }

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString ListBoxExtendedOptionsFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IEnumerable<SelectListItem> selectList,
			Expression<Func<SelectListItem, string>> optionClassExpression)
		{
			return ListBoxExtendedOptionsFor(htmlHelper, expression, selectList, null, optionClassExpression);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString ListBoxExtendedOptionsFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IEnumerable<SelectListItem> selectList,
			object htmlAttributes,
			Expression<Func<SelectListItem, string>> optionClassExpression)
		{
			return ListBoxExtendedOptionsFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), optionClassExpression);
		}

		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Users cannot use anonymous methods with the LambdaExpression type")]
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString ListBoxExtendedOptionsFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IEnumerable<SelectListItem> selectList,
			IDictionary<string, object> htmlAttributes,
			Expression<Func<SelectListItem, string>> optionClassExpression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

			return ListBoxExtendedOptionsHelper(htmlHelper,
								 metadata,
								 ExpressionHelper.GetExpressionText(expression),
								 selectList,
								 htmlAttributes,
								 optionClassExpression);
		}

		private static MvcHtmlString ListBoxExtendedOptionsHelper(HtmlHelper htmlHelper,
			ModelMetadata metadata,
			string name,
			IEnumerable<SelectListItem> selectList,
			IDictionary<string, object> htmlAttributes,
			Expression<Func<SelectListItem, string>> optionClassExpression)
		{
			return SelectInternal(htmlHelper, metadata, optionLabel: null, name: name, selectList: selectList, allowMultiple: true, htmlAttributes: htmlAttributes, optionClassExpression: optionClassExpression);
		}

		private static IEnumerable<SelectListItem> GetSelectData(this HtmlHelper htmlHelper, string name)
		{
			object o = null;
			if (htmlHelper.ViewData != null)
			{
				o = htmlHelper.ViewData.Eval(name);
			}
			if (o == null)
			{
				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Missing select data.",
						name,
						"IEnumerable<SelectListItem>"));
			}
			IEnumerable<SelectListItem> selectList = o as IEnumerable<SelectListItem>;
			if (selectList == null)
			{
				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Wrong select data type.",
						name,
						o.GetType().FullName,
						"IEnumerable<SelectListItem>"));
			}
			return selectList;
		}

		private static string ListItemToOption(SelectListItem item,
			Func<SelectListItem, string> optionClassExpression = null)
		{
			TagBuilder builder = new TagBuilder("option")
			{
				InnerHtml = HttpUtility.HtmlEncode(item.Text)
			};
			if (item.Value != null)
			{
				builder.Attributes["value"] = item.Value;
			}
			if (item.Selected)
			{
				builder.Attributes["selected"] = "selected";
			}
			if (optionClassExpression != null)
			{
				builder.AddCssClass(optionClassExpression(item));
			}
			return builder.ToString(TagRenderMode.Normal);
		}

		private static IEnumerable<SelectListItem> GetSelectListWithDefaultValue(IEnumerable<SelectListItem> selectList, object defaultValue, bool allowMultiple)
		{
			IEnumerable defaultValues;

			if (allowMultiple)
			{
				defaultValues = defaultValue as IEnumerable;
				if (defaultValues == null || defaultValues is string)
				{
					throw new InvalidOperationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Select expression not enumerable",
							"expression"));
				}
			}
			else
			{
				defaultValues = new[] { defaultValue };
			}

			IEnumerable<string> values = from object value in defaultValues
										 select Convert.ToString(value, CultureInfo.CurrentCulture);
			HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
			List<SelectListItem> newSelectList = new List<SelectListItem>();

			foreach (SelectListItem item in selectList)
			{
				item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
				newSelectList.Add(item);
			}
			return newSelectList;
		}

		private static object GetModelStateValue(string key, Type destinationType, HtmlHelper htmlHelper)
		{
			ModelState modelState;
			if (htmlHelper.ViewContext.ViewData.ModelState.TryGetValue(key, out modelState))
			{
				if (modelState.Value != null)
				{
					return modelState.Value.ConvertTo(destinationType, null /* culture */);
				}
			}
			return htmlHelper.ViewContext.ViewData.Eval(key);

		}

		private static MvcHtmlString SelectInternal(this HtmlHelper htmlHelper,
			ModelMetadata metadata,
			string optionLabel,
			string name,
			IEnumerable<SelectListItem> selectList,
			bool allowMultiple,
			IDictionary<string, object> htmlAttributes,
			Expression<Func<SelectListItem, string>> optionClassExpression)
		{
			string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			if (String.IsNullOrEmpty(fullName))
			{
				throw new ArgumentException("Null or empty.", "name");
			}

			bool usedViewData = false;

			// If we got a null selectList, try to use ViewData to get the list of items.
			if (selectList == null)
			{
				selectList = htmlHelper.GetSelectData(name);
				usedViewData = true;
			}

			object defaultValue = (allowMultiple) ? GetModelStateValue(fullName, typeof(string[]), htmlHelper) : GetModelStateValue(fullName, typeof(string), htmlHelper);

			// If we haven't already used ViewData to get the entire list of items then we need to
			// use the ViewData-supplied value before using the parameter-supplied value.
			if (!usedViewData && defaultValue == null && !String.IsNullOrEmpty(name))
			{
				defaultValue = htmlHelper.ViewData.Eval(name);
			}

			if (defaultValue != null)
			{
				selectList = GetSelectListWithDefaultValue(selectList, defaultValue, allowMultiple);
			}

			// Convert each ListItem to an <option> tag
			StringBuilder listItemBuilder = new StringBuilder();

			// Make optionLabel the first item that gets rendered.
			if (optionLabel != null)
			{
				listItemBuilder.AppendLine(ListItemToOption(new SelectListItem() { Text = optionLabel, Value = String.Empty, Selected = false }));
			}
			var compiledExpression = optionClassExpression != null ? optionClassExpression.Compile() : null;
			foreach (SelectListItem item in selectList)
			{
				listItemBuilder.AppendLine(ListItemToOption(item, compiledExpression));
			}

			TagBuilder tagBuilder = new TagBuilder("select")
			{
				InnerHtml = listItemBuilder.ToString()
			};
			tagBuilder.MergeAttributes(htmlAttributes);
			tagBuilder.MergeAttribute("name", fullName, true /* replaceExisting */);
			tagBuilder.GenerateId(fullName);
			if (allowMultiple)
			{
				tagBuilder.MergeAttribute("multiple", "multiple");
			}

			// If there are any errors for a named field, we add the css attribute.
			ModelState modelState;
			if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
			{
				if (modelState.Errors.Count > 0)
				{
					tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
				}
			}

			tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

			return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
		}
	}
}