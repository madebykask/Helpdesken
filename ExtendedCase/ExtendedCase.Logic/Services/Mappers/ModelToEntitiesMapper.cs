using AutoMapper;
using ExtendedCase.Common;
using ExtendedCase.Dal.Data;
using ExtendedCase.Models;

namespace ExtendedCase.Logic.Services.Mappers
{
    public interface IModelToEntitiesMapper
    {
        ExtendedCaseFieldValue MapToExtendedCaseFieldValue(FieldValueModel m);
        ExtendedCaseFormStateItem MapToExtendedCaseFormStateItem(FormStateItem m);
    }

    public class ModelToEntitiesMapper : IModelToEntitiesMapper
    {
        private readonly IJsonSerializeService _serializationService;
        private readonly IMapper _mapper;

        #region ctor()

        public ModelToEntitiesMapper(IJsonSerializeService serializationService, IMapper mapper)
        {
            _serializationService = serializationService;
            _mapper = mapper;
        }

        #endregion

        public ExtendedCaseFieldValue MapToExtendedCaseFieldValue(FieldValueModel m)
        {
            return new ExtendedCaseFieldValue
            {
                FieldId = m.FieldId,
                Value = m.Value,
                SecondaryValue = m.SecondaryValue,
                Properties = m.Properties != null ? _serializationService.Serialize(m.Properties) : null
            };
        }

        public ExtendedCaseFormStateItem MapToExtendedCaseFormStateItem(FormStateItem m)
        {
            var item = _mapper.Map<FormStateItem, ExtendedCaseFormStateItem>(m);
            return item;
        }
    }
}