namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Web.Models.Shared;

    public static class FieldSettingsHelper
    {
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
    }
}