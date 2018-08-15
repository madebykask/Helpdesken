namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Enums.Settings;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models;

    public static class CaseSolutionSettingExtension
    {
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
            GlobalEnums.TranslationCaseFields caseFields)
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
                        DH.Helpdesk.Common.Tools.ReflectionHelper.GetPropertyName<CaseSolutionSettingOverview>(x => x.Id);

                    string modePropertyName =
                        DH.Helpdesk.Common.Tools.ReflectionHelper.GetPropertyName<CaseSolutionSettingOverview>(
                            x => x.CaseSolutionMode);

                    string fieldNamePropertyName =
                        DH.Helpdesk.Common.Tools.ReflectionHelper.GetPropertyName<CaseSolutionSettingOverview>(
                            x => x.CaseSolutionField);

                    string hiddenName = GetInputName(prefix, i, idPropertyName);
                    string dropDownName = GetInputName(prefix, i, modePropertyName);
                    string hiddenFieldName = GetInputName(prefix, i, fieldNamePropertyName);

                    MvcHtmlString hidden = htmlHelper.Hidden(hiddenName, model.Id);

                    SelectList selectList;
                    selectList = ToSelectList(
                            model.CaseSolutionMode,
                            ((int)model.CaseSolutionMode).ToString(CultureInfo.InvariantCulture),
                            false);
                    //if (caseFieldSettings.CaseFieldSettingRequiredCheck(caseFields.ToString()) == 1
                    //    || caseSolutionField == CaseSolutionFields.Department)
                    //{
                    //    if (model.CaseSolutionMode != CaseSolutionModes.ReadOnly)
                    //    {
                    //        selectList = ToSelectList(
                    //            new CaseSolutionModes(),
                    //            ((int)model.CaseSolutionMode).ToString(CultureInfo.InvariantCulture),
                    //            true);
                    //    }
                    //    else
                    //    {
                    //        selectList = ToSelectList(new CaseSolutionModes(), true);
                    //    }
                    //}
                    //else
                    //{
                    //    selectList = ToSelectList(
                    //        model.CaseSolutionMode,
                    //        ((int)model.CaseSolutionMode).ToString(CultureInfo.InvariantCulture),
                    //        false);
                    //}

                    MvcHtmlString dropDown = htmlHelper.DropDownList(dropDownName, selectList, new
                                                        {
                                                            @class = "fieldStateChanger",
                                                            standardId = caseFields,
                                                            ElementClass = "OptionDropDown",
                                                            ElementName = model.CaseSolutionField
                                                        });

                    MvcHtmlString hiddenForFieldName = htmlHelper.Hidden(hiddenFieldName, model.CaseSolutionField);

                    //return MvcHtmlString.Create(hidden + dropDown.ToString() + hiddenForFieldName);
                    return MvcHtmlString.Create(dropDown.ToString());
                }
            }

            return new MvcHtmlString(string.Empty);
        }

        private static string GetInputName(string name, int i, string idPropertyName)
        {
            var inputName = string.Format("{0}[{1}].{2}", name, i, idPropertyName);
            return inputName;
        }

        private static SelectList ToSelectList(CaseSolutionModes enumeration, bool isRequired)
        {
            var list = CreateList(enumeration, isRequired);

            return new SelectList(list, "ID", "Name");
        }

        private static SelectList ToSelectList(CaseSolutionModes enumeration, string selected, bool isRequired)
        {
            var list = CreateList(enumeration, isRequired);

            return new SelectList(list, "ID", "Name", selected);
        }

        private static IEnumerable<dynamic> CreateList(CaseSolutionModes enumeration, bool isRequired)
        {
            IEnumerable<CaseSolutionModes> query = from CaseSolutionModes d in Enum.GetValues(enumeration.GetType())
                                                   select d;
            if (isRequired)
            {
                query = query.Where(x => x != CaseSolutionModes.ReadOnly);
            }

            var list = query.Select(x => new { ID = Convert.ToInt32(x), Name = CreateName(x) }).ToList();
            return list;
        }

        private static string CreateName(CaseSolutionModes enumeration)
        {
            switch (enumeration)
            {
                case CaseSolutionModes.DisplayField:
                    return Translation.Get("Visa fält - redigera");
                case CaseSolutionModes.ReadOnly:
                    return Translation.Get("Visa fält - skrivskyddat");
                case CaseSolutionModes.Hide:
                    return Translation.Get("Dölj fält");
                default:
                    return Translation.Get(enumeration.ToString());
            }
        }
    }
}