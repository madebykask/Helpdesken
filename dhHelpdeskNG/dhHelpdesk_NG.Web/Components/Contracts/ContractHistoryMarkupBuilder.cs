using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Contract;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Components.Contracts
{
    public static class ContractHistoryMarkupBuilder
    {
        public static IHtmlString Build(IList<FieldDifference> diffList, int customerId)
        {
            var strBld = new StringBuilder();
            foreach (var diff in diffList)
            {
                var s = string.Empty;
                if (diff.FieldName.Equals(EnumContractFieldSettings.Filename, StringComparison.OrdinalIgnoreCase))
                {
                    s = FormatFileChanges(diff, customerId);
                }
                else
                {
                    s = FormatChanges(diff);
                }

                if (!string.IsNullOrEmpty(s))
                    strBld.Append(s);
            }

            var markup = strBld.ToString();
            return new MvcHtmlString(markup);
        }

        private static string FormatFileChanges(FieldDifference diff, int customerId)
        {
            var output = string.Empty;
            var label = string.Empty;
            var displayList = new List<string>();
            var files = diff.NewValue.Split(new []{ ';'}, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (files.Any())
            {
                foreach (var file in files)
                {
                    var fileName = file;
                    var fileText = Translation.GetForCase(GlobalEnums.TranslationCaseFields.Filename.ToString(), customerId);
                    if (file.StartsWith(StringTags.Add, StringComparison.Ordinal))
                    {
                        label = $"{Translation.GetCoreTextTranslation("Lägg till")} {fileText}";
                        fileName = file.Substring(StringTags.Add.Length);
                    }
                    else if (file.StartsWith(StringTags.Delete, StringComparison.Ordinal))
                    {
                        label = $"{Translation.GetCoreTextTranslation("Ta bort")} {fileText}";
                        fileName = file.Substring(StringTags.Delete.Length);
                        
                    }
                    displayList.Add(fileName);
                }

                output = $@"<tr><th style=""width:40%;"">{label}</th><td style=""width:60%;"">{string.Join(", ", displayList)}</td></tr>";
            }
            return output;
        }

        private static string FormatChanges(FieldDifference diff)
        {
            var res = string.Empty;
            var prevVal = diff.PrevValue;
            var curVal = diff.NewValue;

            var label = string.IsNullOrEmpty(diff.LabelTranslation)
                ? Translation.GetCoreTextTranslation(diff.Label)
                : diff.LabelTranslation;
            
            if (string.IsNullOrEmpty(prevVal) && !string.IsNullOrEmpty(curVal))
            {
                res = $@"<tr><th style=""width:40%;"">{label}</th><td style=""width:60%;""> &rarr; {curVal}</td></tr>";
            }
            else
            {
                res = $@"<tr><th style=""width:40%;"">{label}</th><td style=""width:60%;"">{prevVal} &rarr; {curVal}</td></tr>";
            }

            return res;
        }
    }
}