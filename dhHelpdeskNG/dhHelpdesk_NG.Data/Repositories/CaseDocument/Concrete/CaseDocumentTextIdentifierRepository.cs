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


    public sealed class CaseDocumentTextIdentifierRepository : RepositoryBase<CaseDocumentTextIdentifierEntity>, ICaseDocumentTextIdentifierRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseDocumentTextIdentifierEntity, CaseDocumentTextIdentifierModel> _CaseDocumentTextIdentifierToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseDocumentTextIdentifierModel, CaseDocumentTextIdentifierEntity> _CaseDocumentTextIdentifierToEntityMapper;

        public CaseDocumentTextIdentifierRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<CaseDocumentTextIdentifierEntity, CaseDocumentTextIdentifierModel> CaseDocumentTextIdentifierToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseDocumentTextIdentifierModel, CaseDocumentTextIdentifierEntity> CaseDocumentTextIdentifierToEntityMapper)
            : base(databaseFactory)
        {
            _CaseDocumentTextIdentifierToBusinessModelMapper = CaseDocumentTextIdentifierToBusinessModelMapper;
            _CaseDocumentTextIdentifierToEntityMapper = CaseDocumentTextIdentifierToEntityMapper;
        }

        public IEnumerable<CaseDocumentTextIdentifierModel> GetCaseDocumentTextIdentifiers(int extendedCaseFormId)
        {
            var entities = this.Table
                   .Where(c => c.ExtendedCaseFormId == extendedCaseFormId || c.ExtendedCaseFormId == 0)

                   .Distinct()
                   .ToList();

            return entities
                .Select(this._CaseDocumentTextIdentifierToBusinessModelMapper.Map);
        }


    }
}
