using AutoMapper;
using ExtendedCase.Common;
using ExtendedCase.Dal.Data;
using ExtendedCase.Models;

namespace ExtendedCase.Logic.Services.Mappers
{
    public interface IEntitiesToModelMapper
    {
        FieldValueModel MapToFieldValueModel(ExtendedCaseFieldValue entity);
        FormStateItem MapToFormStateItem(ExtendedCaseFormStateItem entity);
    }

    public class EntitiesToModelMapper : IEntitiesToModelMapper
    {
        private readonly IJsonSerializeService _serializationService;
        private readonly IMapper _mapper;

        #region ctor()

        public EntitiesToModelMapper(IJsonSerializeService serializationService, IMapper mapper)
        {
            _serializationService = serializationService;
            _mapper = mapper;
        }

        #endregion

        public FieldValueModel MapToFieldValueModel(ExtendedCaseFieldValue entity)
        {
            var properties = string.IsNullOrEmpty(entity.Properties)
                ? null
                : _serializationService.Deserialize<FieldProperties>(entity.Properties);

            return new FieldValueModel(entity.FieldId, entity.Value, entity.SecondaryValue, properties);
        }

        public FormStateItem MapToFormStateItem(ExtendedCaseFormStateItem entity)
        {
            return _mapper.Map<ExtendedCaseFormStateItem, FormStateItem>(entity);
        }
    }
}