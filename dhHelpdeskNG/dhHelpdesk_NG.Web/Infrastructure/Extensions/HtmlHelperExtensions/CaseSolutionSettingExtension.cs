using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Settings;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Models;

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    public static class CaseSolutionSettingExtension
    {
        private static readonly IList<ItemOverview> _caseSolutionModes = CaseSolutionModes.DisplayField.ToItemOverviewList();

        public static MvcHtmlString CaseSolutionSettingsFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, IList<CaseSolutionSettingModel>>> expression,
            CaseSolutionFields caseSolutionField)
        {
            return CaseSolutionSettingsFor(
                htmlHelper,
                expression,
                caseSolutionField,
                null,
                GlobalEnums.TranslationCaseFields.None);
        }

        public static MvcHtmlString CaseSolutionSettingsFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, IList<CaseSolutionSettingModel>>> expression,
            CaseSolutionFields caseSolutionField,
            IList<CaseFieldSetting> caseFieldSettings,
            GlobalEnums.TranslationCaseFields caseField) 
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var models = (IList<CaseSolutionSettingModel>)metadata.Model;

            var model = models.FirstOrDefault(m => m.CaseSolutionField == caseSolutionField);
            if (model != null)
            {
                var index = models.IndexOf(model);
                var modelName = ExpressionHelper.GetExpressionText(expression);
                var modelPropertyName = nameof(CaseSolutionSettingOverview.CaseSolutionMode);
                var selectedModeValue = ((int)model.CaseSolutionMode).ToString(CultureInfo.InvariantCulture);
                
                var dropDownName = GetInputName(modelName, index, modelPropertyName);
                
                // this was done for performance optimisation - to reuse existing list instead of readin enum values each time 
                var modeSelectOptions = _caseSolutionModes.Select(x => new SelectListItem()
                {
                    Value = x.Value,
                    Text = TranslateModeName(x.Name),
                    Selected = selectedModeValue == x.Value
                }).ToList();

                var fieldSettings = caseFieldSettings.getCaseSettingsValue(caseField.ToString());
                
                var htmlAttributes = new
                {
                    @class = "fieldStateChanger",
                    standardId = caseField,
                    ElementClass = "OptionDropDown",
                    ElementName = model.CaseSolutionField
                };

                var htmlAttributesAsDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                
                // check if should be disabled 
                if (fieldSettings?.Hide ?? false)
                {
                    htmlAttributesAsDict.Add("disabled", "disabled");
                }

                var dropDown = htmlHelper.DropDownList(dropDownName, modeSelectOptions, htmlAttributesAsDict);
                return MvcHtmlString.Create(dropDown.ToString());
            }

            return new MvcHtmlString(string.Empty);
        }

        private static string GetInputName(string name, int i, string idPropertyName)
        {
            var inputName = $"{name}[{i}].{idPropertyName}";
            return inputName;
        }

        private static string TranslateModeName(string mode)
        {
            if (mode.Equals(CaseSolutionModes.DisplayField.ToString(), StringComparison.OrdinalIgnoreCase))
                return Translation.GetCoreTextTranslation("Visa fält - redigera");

            if (mode.Equals(CaseSolutionModes.ReadOnly.ToString(), StringComparison.OrdinalIgnoreCase))
                return Translation.GetCoreTextTranslation("Visa fält - skrivskyddat");

            if (mode.Equals(CaseSolutionModes.Hide.ToString(), StringComparison.OrdinalIgnoreCase))
                return Translation.GetCoreTextTranslation("Dölj fält");

            return Translation.GetCoreTextTranslation(mode);
        }
    }
}