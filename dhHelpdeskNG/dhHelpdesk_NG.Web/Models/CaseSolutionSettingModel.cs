namespace DH.Helpdesk.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums.Settings;

    public class CaseSolutionSettingModel
    {
        public CaseSolutionSettingModel()
        {
        }

        public CaseSolutionSettingModel(CaseSolutionFields caseSolutionField, CaseSolutionModes caseSolutionMode)
        {
            this.CaseSolutionField = caseSolutionField;
            this.CaseSolutionMode = caseSolutionMode;
        }

        public int Id { get; set; }

        public CaseSolutionFields CaseSolutionField { get; set; }

        public CaseSolutionModes CaseSolutionMode { get; set; }

        public static List<CaseSolutionSettingModel> CreateModel(IEnumerable<CaseSolutionSettingOverview> settingOverviews)
        {
            var models =
                settingOverviews.Select(
                    x => new CaseSolutionSettingModel(x.CaseSolutionField, x.CaseSolutionMode) { Id = x.Id }).ToList();

            return models;
        }

        public static List<CaseSolutionSettingModel> CreateDefaultModel()
        {
            List<CaseSolutionSettingModel> settings =
                (from caseSolutionFields in (CaseSolutionFields[])Enum.GetValues(typeof(CaseSolutionFields))
                 select new CaseSolutionSettingModel(caseSolutionFields, CaseSolutionModes.DisplayField)).ToList();
            return settings;
        }
    }
}