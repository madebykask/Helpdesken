namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums.Settings;
    using DH.Helpdesk.Web.Models;

    public static class CaseSolutionSettingExtension
    {
        public static MvcHtmlString CaseSolutionSettingsFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, IList<CaseSolutionSettingModel>>> expression,
            CaseSolutionFields caseSolutionField)
        {
            string prefix = ExpressionHelper.GetExpressionText(expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var models = (IList<CaseSolutionSettingModel>)metadata.Model;

            for (int i = 0; i < models.Count; i++)
            {
                CaseSolutionSettingModel model = models[i];
                if (model.CaseSolutionField == caseSolutionField)
                {
                    string idPropertyName =
                        Common.Tools.ReflectionHelper.GetPropertyName<CaseSolutionSettingOverview>(x => x.Id);
                    string modePropertyName =
                        Common.Tools.ReflectionHelper.GetPropertyName<CaseSolutionSettingOverview>(
                            x => x.CaseSolutionMode);
                    string fieldNamePropertyName =
                        Common.Tools.ReflectionHelper.GetPropertyName<CaseSolutionSettingOverview>(
                            x => x.CaseSolutionField);

                    string hiddenName = GetInputName(prefix, i, idPropertyName);
                    string dropDownName = GetInputName(prefix, i, modePropertyName);
                    string hiddenFieldName = GetInputName(prefix, i, fieldNamePropertyName);

                    MvcHtmlString hidden = htmlHelper.Hidden(hiddenName, model.Id);
                    MvcHtmlString checkBox = htmlHelper.DropDownList(
                        dropDownName,
                        model.CaseSolutionMode.ToSelectList(
                            ((int)model.CaseSolutionMode).ToString(CultureInfo.InvariantCulture)));
                    MvcHtmlString hiddenForFieldName = htmlHelper.Hidden(hiddenFieldName, model.CaseSolutionField);

                    return MvcHtmlString.Create(hidden + checkBox.ToString() + hiddenForFieldName);
                }
            }

            return new MvcHtmlString(string.Empty);
        }

        private static string GetInputName(string name, int i, string idPropertyName)
        {
            var inputName = string.Format("{0}[{1}].{2}", name, i, idPropertyName);
            return inputName;
        }
    }
}