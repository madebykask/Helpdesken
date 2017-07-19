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

    public sealed class CaseDocumentTextConditionIdentifierRepository : RepositoryBase<CaseDocumentTextConditionIdentifierEntity>, ICaseDocumentTextConditionIdentifierRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseDocumentTextConditionIdentifierEntity, CaseDocumentTextConditionIdentifierModel> _CaseDocumentTextConditionIdentifierToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseDocumentTextConditionIdentifierModel, CaseDocumentTextConditionIdentifierEntity> _CaseDocumentTextConditionIdentifierToEntityMapper;

        public CaseDocumentTextConditionIdentifierRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<CaseDocumentTextConditionIdentifierEntity, CaseDocumentTextConditionIdentifierModel> CaseDocumentTextConditionIdentifierToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseDocumentTextConditionIdentifierModel, CaseDocumentTextConditionIdentifierEntity> CaseDocumentTextConditionIdentifierToEntityMapper)
            : base(databaseFactory)
        {
            _CaseDocumentTextConditionIdentifierToBusinessModelMapper = CaseDocumentTextConditionIdentifierToBusinessModelMapper;
            _CaseDocumentTextConditionIdentifierToEntityMapper = CaseDocumentTextConditionIdentifierToEntityMapper;
        }

        public IEnumerable<CaseDocumentTextConditionIdentifierModel> 
            GetCaseDocumentTextConditionIdentifiers(int extendedCaseFormId)
        {
            var entities = this.Table
                   .Where(c => c.ExtendedCaseFormId == extendedCaseFormId || c.ExtendedCaseFormId == 0)

                   .Distinct()
                   .ToList();

            return entities
                .Select(this._CaseDocumentTextConditionIdentifierToBusinessModelMapper.Map);
        }

        public CaseDocumentTextConditionIdentifierModel GetCaseDocumentTextConditionPropertyName(int extendedCaseFormId, string identifier)
        {
            var entity = this.Table
                   .Where(c => (c.ExtendedCaseFormId == extendedCaseFormId || c.ExtendedCaseFormId == 0) && c.Identifier.ToLower() == identifier.ToLower())

                   .Distinct()
                   .ToList();

            

            return entity.Select(this._CaseDocumentTextConditionIdentifierToBusinessModelMapper.Map).FirstOrDefault();
        }


    }
}