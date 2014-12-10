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
            /// code removed until http://redmine.fastdev.se/issues/9876 will be started
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
                    return Translation.Get("Display Field");
                case CaseSolutionModes.ReadOnly:
                    return Translation.Get("Read Only");
                default:
                    return Translation.Get(enumeration.ToString());
            }
        }
    }
}