using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedCase.Dal.Data;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.Services.Mappers;
using ExtendedCase.Models;

namespace ExtendedCase.Logic.Services
{
    public interface IFormService
    {
        //todo: check if method is required?
        ExtendedCaseFormModel GetFormById(int id);
        ExtendedCaseDataModel GetExtendedCaseDataByUniqueId(Guid uniqueId);

        ExtendedCaseDataModel Save(FormDataSaveInputModel data);
        ExtendedCaseFormModel[] GetFormsList();

        FormMetaDataModel GetMetaDataById(int id, int? languageId = null);
        FormMetaDataModel GetMetaDataByAssignment(int? userRole, int? caseStatus, int? customerId, int? languageId = null);
    }

    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;
        private readonly ITranslationRepository _translationRepository;
        private readonly IModelToEntitiesMapper _modelToEntitiesMapper;
        private readonly IEntitiesToModelMapper _entitiesToModelMapper;

        #region ctor

        public FormService(IFormRepository formRepository, ITranslationRepository translationRepository,
            IModelToEntitiesMapper modelToEntitiesMapper, 
            IEntitiesToModelMapper entitiesToModelMapper)
        {
            _formRepository = formRepository;
            _translationRepository = translationRepository;
            _modelToEntitiesMapper = modelToEntitiesMapper;
            _entitiesToModelMapper = entitiesToModelMapper;
        }

        #endregion

        public ExtendedCaseFormModel GetFormById(int id)
        {
            var dbModel = _formRepository.GetFormById(id);
            return new ExtendedCaseFormModel();
        }

        #region ExtendedCase Data

        public ExtendedCaseDataModel GetExtendedCaseDataByUniqueId(Guid uniqueId)
        {
            var caseData = _formRepository.GetExtendedCaseDataByUniqueId(uniqueId);
            var data = new CaseData
            {
                ExtendedCaseFieldsValues =
                    caseData.FieldsValues.Select(o => _entitiesToModelMapper.MapToFieldValueModel(o))
                        .ToList()
            };

            var exCaseFormStateItems = _formRepository.GetFormStateItems(uniqueId);
            var formStateItems = exCaseFormStateItems.Select(_entitiesToModelMapper.MapToFormStateItem).ToList();
            return new ExtendedCaseDataModel(caseData.Id, caseData.ExtendedCaseGuid, caseData.ExtendedCaseFormId, data, formStateItems);
        }

        #endregion

        #region MetaData

        public FormMetaDataModel GetMetaDataById(int id, int? languageId = null)
        {
            var data = _formRepository.GetFormById(id);
            var metaData = languageId.HasValue ? TranslateMetaData(data.MetaData, languageId.Value, data.DefaultLanguageId) : data.MetaData;
            return new FormMetaDataModel(data.Id, metaData);
        }

        public FormMetaDataModel GetMetaDataByAssignment(int? userRole, int? caseStatus, int? customerId,
            int? languageId = null)
        {
            var data = _formRepository.GetMetaDataByAssignment(userRole, caseStatus, customerId);
            var metData = languageId.HasValue ? TranslateMetaData(data.MetaData, languageId.Value, data.DefaultLanguageId) : data.MetaData;
            return new FormMetaDataModel(data.Id, metData);
        }

        private string TranslateMetaData(string metaData, int languageId, int? defaultLanguageId)
        {
            if (string.IsNullOrWhiteSpace(metaData))
                return metaData;
            
            metaData = Translate(languageId, metaData);

            if (defaultLanguageId.HasValue)
            {
                metaData = Translate(defaultLanguageId.Value, metaData);
            }

            return metaData;
        }

        private string Translate(int langId, string data)
        {
            var regex = new Regex("[\"'](?:@Translation\\.)([^\"']+)[\"']");
            var propertiesValues = regex.Matches(data).Cast<Match>()
                .SelectMany(m => m.Groups.Cast<Group>().Skip(1).Select(g => g.Value))
                .ToArray();
            if (propertiesValues.Any())
            {
                var translations = _translationRepository.GetTranslations(langId, propertiesValues);
                foreach (var translation in translations)
                {
                    data = Regex.Replace(data,$"[\"']{Regex.Escape($"@Translation.{translation.Key}")}[\"']", $"\"{translation.Value}\"");
                }
            }
            return data;
        }

        #endregion

        public ExtendedCaseFormModel[] GetFormsList()
        {
            var data = _formRepository.GetForms();
            return data.Select(d => new ExtendedCaseFormModel
            {
                Id = d.Id,
                Name = d.Name
            }).ToArray();
        }

        public ExtendedCaseDataModel Save(FormDataSaveInputModel data)
        {
            var uniqueId = data.UniqueId ?? Guid.Empty;
            var fieldsValues = data.FieldsValues.Select(_modelToEntitiesMapper.MapToExtendedCaseFieldValue).ToList();
            
            //todo: use correct user once authection is implemented
            var caseDataId = _formRepository.SaveFormData(uniqueId, data.FormId, fieldsValues, "test");

            var formStateItems = data.FormState.Select(_modelToEntitiesMapper.MapToExtendedCaseFormStateItem).ToList();
            _formRepository.SaveFormState(uniqueId, caseDataId, formStateItems);

            var caseData = _formRepository.GetExtendedCaseDataById(caseDataId, false);
            return new ExtendedCaseDataModel(caseData.Id, caseData.ExtendedCaseGuid, caseData.ExtendedCaseFormId);
        }
    }
}
