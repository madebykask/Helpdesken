using System.Collections.Generic;
using System.Linq;
using System;
using DH.Helpdesk.Domain.Cases;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Globalization;


namespace DH.Helpdesk.Dal.Repositories.CaseDocument.Concrete
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;


    public sealed class CaseDocumentTextConditionRepository : RepositoryBase<CaseDocumentTextConditionEntity>, ICaseDocumentTextConditionRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseDocumentTextConditionEntity, CaseDocumentTextConditionModel> _CaseDocumentTextConditionToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseDocumentTextConditionModel, CaseDocumentTextConditionEntity> _CaseDocumentTextConditionToEntityMapper;

        public CaseDocumentTextConditionRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<CaseDocumentTextConditionEntity, CaseDocumentTextConditionModel> CaseDocumentTextConditionToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseDocumentTextConditionModel, CaseDocumentTextConditionEntity> CaseDocumentTextConditionToEntityMapper)
            : base(databaseFactory)
        {
            _CaseDocumentTextConditionToBusinessModelMapper = CaseDocumentTextConditionToBusinessModelMapper;
            _CaseDocumentTextConditionToEntityMapper = CaseDocumentTextConditionToEntityMapper;
        }

        public IEnumerable<CaseDocumentTextConditionModel> GetCaseDocumentTextConditions(int caseDocumentText_Id)
        {
            var entities = this.Table
                   .Where(c => c.CaseDocumentText_Id == caseDocumentText_Id && c.Status != 0)

                   .Distinct()
                   .ToList();

            return entities
                .Select(this._CaseDocumentTextConditionToBusinessModelMapper.Map);
        }


    }
}
