using DH.Helpdesk.BusinessData.Enums.Orders.FieldNames;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System;
    using System.Collections.Generic;
    using BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Web.Models.Shared;

    public static class FieldSettingsHelper
    {
        private static string SOLVED_IN_TIME = "Rätt tid";
        private static string NOT_SOLVED_IN_TIME = "Ej rätt tid";
        private static string SOLVED_TIME_IS_NOT_CALCULATED = " - ";
        private static string TOTAL_HOUR = "Tim";
        private static string TOTAL_MINUTE = "Min";

        public static void CreateHeaderIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            List<GridColumnHeaderModel> headers)
        {
            if (!setting.Show)
            {
                return;
            }

            var header = new GridColumnHeaderModel(fieldName, setting.Caption);
            headers.Add(header);
        }

        public static void ForceCreateHeader(string fieldName, List<GridColumnHeaderModel> headers)
        {
            var header = new GridColumnHeaderModel(fieldName, string.Empty);
            headers.Add(header);
        }

        public static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            DateTime? value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new DateTimeDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        public static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            string value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new StringDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        public static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            decimal? value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new DecimalDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        public static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            bool value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new BooleanDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        public static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            string[] value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new HtmlStringsDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        public static void ForceCreateValue(            
            string fieldName,
            string value,
            List<NewGridRowCellValueModel> values)
        {           
            var fieldValue = new NewGridRowCellValueModel(fieldName,  new StringDisplayValue(value));
            values.Add(fieldValue);
        }

        public static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            DisplayValue value,
            List<NewGridRowCellValueModel> values)
        {
            if (!setting.Show)
            {
                return;
            }

            var fieldValue = new NewGridRowCellValueModel(fieldName, value);
            values.Add(fieldValue);
        }

        public static void CreateValueIfNeeded(FieldOverviewSetting setting, string fieldName,
            Enum value, List<NewGridRowCellValueModel> values)
        {
            if (!setting.Show)
            {
                return;
            }

            var fieldValue = new NewGridRowCellValueModel(fieldName, new StringDisplayValue(value != null ? value.GetDisplayName() : string.Empty));
            values.Add(fieldValue);
        }

        public static void CreateSolvedTimeValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            int? value,
            List<NewGridRowCellValueModel> values)
        {
            var tempValue = string.Empty;             

            if (value == null)
                tempValue = SOLVED_TIME_IS_NOT_CALCULATED;
            else
            {
                if (value == 0)
                    tempValue = Translation.Get(NOT_SOLVED_IN_TIME);            
                else 
                    tempValue = Translation.Get(SOLVED_IN_TIME);
            }
            var displayValue = new StringDisplayValue(tempValue);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        public static void CreateTotalValueIfNeeded(FieldOverviewSetting setting, string fieldName, decimal? value, List<NewGridRowCellValueModel> values)
        {
            if (!setting.Show)
            {
                return;
            }

            var tempValue = string.Empty;

            if (value.HasValue && value.Value > 0)
                tempValue = value.Value.ToString("0.00");

            var fieldValue = new NewGridRowCellValueModel(fieldName, new StringDisplayValue(tempValue));
            values.Add(fieldValue);
        }

        public static void CreateTotalTimeValueIfNeeded(FieldOverviewSetting setting, string fieldName, int? value, List<NewGridRowCellValueModel> values)
        {
            if (!setting.Show)
            {
                return;
            }

            var tempValue = string.Empty;

            if (value.HasValue && value.Value > 0)
                tempValue = string.Format("{0} {1} {2} {3}", value.Value / 60, Translation.GetCoreTextTranslation(TOTAL_HOUR), value.Value % 60, Translation.GetCoreTextTranslation(TOTAL_MINUTE));

            var fieldValue = new NewGridRowCellValueModel(fieldName, new StringDisplayValue(tempValue));
            values.Add(fieldValue);
        }
    }
}