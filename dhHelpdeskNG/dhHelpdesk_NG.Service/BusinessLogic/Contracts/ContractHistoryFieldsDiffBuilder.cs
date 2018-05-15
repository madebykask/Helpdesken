using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Contracts;
using DH.Helpdesk.BusinessData.Models.Contract;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Services.BusinessLogic.Contracts
{
    public class ContractHistoryFieldsDiffBuilder
    {
        private readonly IContractHistoryFormatter _formatter;
        private readonly IList<ContractsSettingRowModel> _fieldSettings;
        private readonly int _languageId;
     

        #region FieldDisplaySettings

        private class FieldDisplaySettings
        {
            public bool Show { get; set; }
            public string FieldName { get; set; }
            public string Label { get; set; }
            public string LabelEng { get; set; }
        }

        #endregion

        #region FieldLabels

        private readonly IDictionary<string, string> FieldLabels = new Dictionary<string, string>()
        {
            {EnumContractFieldSettings.Number, ContractFieldLabels.ContractNumber },
            {EnumContractFieldSettings.Category, ContractFieldLabels.ContractCategory },
            {EnumContractFieldSettings.Supplier, ContractFieldLabels.Supplier},
            {EnumContractFieldSettings.StartDate, ContractFieldLabels.ContractStartDate},
            {EnumContractFieldSettings.EndDate, ContractFieldLabels.ContractEndDate},
            {EnumContractFieldSettings.NoticeDate, ContractFieldLabels.Noticedate},
            {EnumContractFieldSettings.WatchDate, ContractFieldLabels.Watchdate},
            {EnumContractFieldSettings.Department, ContractFieldLabels.Department},
            {EnumContractFieldSettings.ResponsibleUser, ContractFieldLabels.Responsibleuser},
            {EnumContractFieldSettings.Finished, ContractFieldLabels.Finished},
            {EnumContractFieldSettings.Running, ContractFieldLabels.Running},
            {EnumContractFieldSettings.FollowUpField, ContractFieldLabels.FollowUpInterval},
            {EnumContractFieldSettings.ResponsibleFollowUpField, ContractFieldLabels.FollowUpResponsibleUser},
            {EnumContractFieldSettings.CaseNumber, ContractFieldLabels.CaseNumber},
            {EnumContractFieldSettings.Other, ContractFieldLabels.Info},
            //{EnumContractFieldSettings.Responsible, ContractFieldLabels.Responsible},
            
            //todo:
            //{EnumContractFieldSettings.Filename
        };

        #endregion

        #region ctor()

        public ContractHistoryFieldsDiffBuilder(IContractHistoryFormatter formatter,
                                                IList<ContractsSettingRowModel> fieldSettings,
                                                int languageId)
        {
            _formatter = formatter;
            _fieldSettings = fieldSettings;
            _languageId = languageId;
        }

        #endregion

        public List<FieldDifference> BuildHistoryDiff(ContractHistoryFull prev, ContractHistoryFull cur)
        {
            var list = new List<FieldDifference>();
            
            CheckAndCreateDiff(list, FindSettings(EnumContractFieldSettings.Number), prev?.ContractNumber, cur.ContractNumber);

            CheckAndCreateDiff(list,
                FindSettings(EnumContractFieldSettings.Category),
                prev?.ContractCategory?.Id,
                cur.ContractCategory?.Id,
                prev?.ContractCategory?.Name,
                cur.ContractCategory?.Name);

            CheckAndCreateDiff(list,
                FindSettings(EnumContractFieldSettings.Supplier),
                prev?.Supplier?.Id,
                cur.Supplier?.Id,
                prev?.Supplier?.Name,
                cur.Supplier?.Name);

            CheckAndCreateDiff(list,
                FindSettings(EnumContractFieldSettings.Department),
                prev?.Department?.Id,
                cur.Department?.Id,
                prev?.Department?.Name,
                cur.Department?.Name);

            CheckAndCreateDiff(list,
                FindSettings(EnumContractFieldSettings.ResponsibleUser),
                prev?.ResponsibleUser?.Id,
                cur.ResponsibleUser?.Id,
                _formatter.FormatUserName(prev?.ResponsibleUser),
                _formatter.FormatUserName(cur.ResponsibleUser));

            CheckAndCreateDiff(list, FindSettings(EnumContractFieldSettings.Other), prev?.Info, cur.Info);
            CheckAndCreateDiff(list, FindSettings(EnumContractFieldSettings.StartDate), prev?.StartDate, cur.StartDate);
            CheckAndCreateDiff(list, FindSettings(EnumContractFieldSettings.EndDate), prev?.EndDate, cur.EndDate);
            CheckAndCreateDiff(list, FindSettings(EnumContractFieldSettings.NoticeDate), prev?.NoticeDate, cur.NoticeDate);

            CheckAndCreateDiff(list,
                CreateNoticeTimeFieldSettings(),
                prev?.NoticeTime,
                cur.NoticeTime,
                prev?.NoticeTime > 0 ? _formatter.FormatNoticeTime(prev?.NoticeTime ?? 0) : string.Empty,
                _formatter.FormatNoticeTime(cur.NoticeTime));

            CheckAndCreateDiff(list,
                FindSettings(EnumContractFieldSettings.Running),
                prev?.Running,
                cur.Running,
                _formatter.FormatToBoolean(prev?.Running),
                _formatter.FormatToBoolean(cur.Running));

            CheckAndCreateDiff(list,
                FindSettings(EnumContractFieldSettings.Finished),
                prev?.Finished,
                cur.Finished,
                _formatter.FormatToBoolean(prev?.Finished),
                _formatter.FormatToBoolean(cur.Finished));

            CheckAndCreateDiff(list,
                               FindSettings(EnumContractFieldSettings.FollowUpField), 
                               prev?.FollowUpInterval, 
                               cur.FollowUpInterval,
                               _formatter.FormartFollowUpInterval(prev?.FollowUpInterval),
                               _formatter.FormartFollowUpInterval(cur.FollowUpInterval));

            CheckAndCreateDiff(list,
                               FindSettings(EnumContractFieldSettings.ResponsibleFollowUpField),
                               prev?.FollowUpResponsibleUser?.Id,
                               cur.FollowUpResponsibleUser?.Id,
                               _formatter.FormatUserName(prev?.FollowUpResponsibleUser),
                               _formatter.FormatUserName(cur.FollowUpResponsibleUser));

            CreateFileDiff(list, FindSettings(EnumContractFieldSettings.Filename), cur.Files);

            var changes = list.Where(x => x != null).ToList();
            return changes;
        }

        #region Diff Methods
        
        private void CheckAndCreateDiff(List<FieldDifference> list, FieldDisplaySettings fieldSettings, string prevVal, string curVal)
        {
            if ((!string.IsNullOrEmpty(prevVal) || !string.IsNullOrEmpty(curVal)) && string.Compare(prevVal, curVal, StringComparison.OrdinalIgnoreCase) !=0)
            {
                if (fieldSettings.Show)
                {
                    var diff = CreateFieldDifference(fieldSettings, prevVal, curVal);
                    list.Add(diff);
                }
            }
        }

        private void CheckAndCreateDiff(List<FieldDifference> list, FieldDisplaySettings fieldSettings, int? prevVal, int? curVal, string prevFormatted, string curFormatted)
        {
            if ((prevVal.HasValue || curVal.HasValue) && Nullable.Compare(prevVal, curVal) != 0)
            {
                if (fieldSettings.Show)
                {
                    var fieldDifference = CreateFieldDifference(fieldSettings, prevFormatted, curFormatted);
                    list.Add(fieldDifference);
                }
            }
        }

        private void CheckAndCreateDiff(List<FieldDifference> list, FieldDisplaySettings fieldSettings, DateTime? prevVal, DateTime? curVal)
        {
            if ((prevVal.HasValue || curVal.HasValue) && Nullable.Compare<DateTime>(prevVal, curVal) != 0)
            {
                if (fieldSettings.Show)
                {
                    var fieldDifference = CreateFieldDifference(fieldSettings, _formatter.FormatNullableDate(prevVal), _formatter.FormatNullableDate(curVal));
                    list.Add(fieldDifference);
                }
            }
        }

        private void CreateFileDiff(List<FieldDifference> list, FieldDisplaySettings fs, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && fs.Show)
            {
                var diff = CreateFieldDifference(fs, null, fileName);
                list.Add(diff);
            }
        }

        private FieldDifference CreateFieldDifference(FieldDisplaySettings fieldSettings, string prevValue, string newValue)
        {
            return new FieldDifference(fieldSettings.FieldName, prevValue, newValue)
            {
                Label = fieldSettings.Label,
                LabelTranslation = _languageId == LanguageIds.English ? fieldSettings.LabelEng : null
            };
        }

        #endregion

        #region Helpder Methods

        private FieldDisplaySettings CreateNoticeTimeFieldSettings()
        {
            // since there's no separate settings for notice date - use 'Notice date' settings
            var fs = FindSettings(EnumContractFieldSettings.NoticeDate);
            fs.FieldName = "noticetime";
            fs.Label = "Skapa Ärende";
            fs.LabelEng = "Create Case";
            return fs;
        }

        private FieldDisplaySettings FindSettings(string fieldName)
        {
            var fieldModel =
                _fieldSettings.FirstOrDefault(x => fieldName.Equals(x.ContractField, StringComparison.OrdinalIgnoreCase));

            if (fieldModel == null)
            {
                fieldModel = new ContractsSettingRowModel()
                {
                    show = true,
                    ContractField = fieldName
                };
            }

            var fs = new FieldDisplaySettings()
            {
                FieldName = fieldName,
                Show = fieldModel.show,
                Label = string.IsNullOrEmpty(fieldModel.ContractFieldLable) ? FieldLabels[fieldName] : fieldModel.ContractFieldLable,
                LabelEng = fieldModel.ContractFieldLable_Eng
            };

            return fs;
        }

        #endregion
    }
}