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
    using System.Web.Mvc;

    public sealed class CaseDocumentConditionRepository : RepositoryBase<CaseDocumentConditionEntity>, ICaseDocumentConditionRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseDocumentConditionEntity, CaseDocumentConditionModel> _CaseDocumentConditionToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseDocumentConditionModel, CaseDocumentConditionEntity> _CaseDocumentConditionToEntityMapper;

        public CaseDocumentConditionRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<CaseDocumentConditionEntity, CaseDocumentConditionModel> CaseDocumentConditionToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseDocumentConditionModel, CaseDocumentConditionEntity> CaseDocumentConditionToEntityMapper)
            : base(databaseFactory)
        {
            _CaseDocumentConditionToBusinessModelMapper = CaseDocumentConditionToBusinessModelMapper;
            _CaseDocumentConditionToEntityMapper = CaseDocumentConditionToEntityMapper;
        }

        public IEnumerable<CaseDocumentConditionModel> GetCaseDocumentConditions(int caseDocument_Id)
        {
            var entities = this.Table
                   .Where(c => c.CaseDocument_Id == caseDocument_Id && c.Status != 0)

                   .Distinct()
                   .ToList();

            return entities
                .Select(this._CaseDocumentConditionToBusinessModelMapper.Map);
        }


    }
}
