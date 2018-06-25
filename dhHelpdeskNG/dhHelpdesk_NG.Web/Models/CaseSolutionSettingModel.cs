namespace DH.Helpdesk.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums.Settings;
    
    public class CaseSolutionSettingModel
    {
        #region ctor()

        public CaseSolutionSettingModel()
        {
        }

        public CaseSolutionSettingModel(CaseSolutionFields caseSolutionField, CaseSolutionModes caseSolutionMode)
        {
            this.CaseSolutionField = caseSolutionField;
            this.CaseSolutionMode = caseSolutionMode;
        }

        #endregion
     
        public int Id { get; set; }

        public CaseSolutionFields CaseSolutionField { get; set; }

        public CaseSolutionModes CaseSolutionMode { get; set; }

        #region Static Methods

        public static List<CaseSolutionSettingModel> CreateModel(IEnumerable<CaseSolutionSettingOverview> settingOverviews)
        {
            var models =
                settingOverviews.Select(
                    x => new CaseSolutionSettingModel(x.CaseSolutionField, x.CaseSolutionMode)
                    {
                        Id = x.Id
                    }).ToList();

            return models;
        }

        public static List<CaseSolutionSettingModel> CreateDefaultModel()
        {
            var caseSolutionFields = (CaseSolutionFields[]) Enum.GetValues(typeof(CaseSolutionFields));
            var settings = caseSolutionFields.Select(f => new CaseSolutionSettingModel(f, CaseSolutionModes.DisplayField)).ToList();
            return settings;
        }

        public static bool IsFieldAlwaysVisible(CaseSolutionFields fieldId)
        {
            return fieldId == CaseSolutionFields.CaseType || fieldId == CaseSolutionFields.Administrator
                   || fieldId == CaseSolutionFields.Priority || fieldId == CaseSolutionFields.InternalLogNote;
        }

        #endregion
    }
}