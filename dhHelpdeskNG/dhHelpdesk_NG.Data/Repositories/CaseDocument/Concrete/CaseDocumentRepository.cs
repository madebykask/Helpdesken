using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace DH.Helpdesk.Dal.Repositories.CaseDocument.Concrete
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;
    using System.Web.Mvc;

    public sealed class CaseDocumentRepository : RepositoryBase<CaseDocumentEntity>, ICaseDocumentRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseDocumentEntity, CaseDocumentModel> _CaseDocumentToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseDocumentModel, CaseDocumentEntity> _CaseDocumentToEntityMapper;

        public CaseDocumentRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<CaseDocumentEntity, CaseDocumentModel> CaseDocumentToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseDocumentModel, CaseDocumentEntity> CaseDocumentToEntityMapper)
            : base(databaseFactory)
        {
            _CaseDocumentToBusinessModelMapper = CaseDocumentToBusinessModelMapper;
            _CaseDocumentToEntityMapper = CaseDocumentToEntityMapper;
        }

        public IEnumerable<CaseDocumentModel> GetCaseDocumentsByCustomer(int customerId)
        {
            var entities = this.Table
                  .Where(c => c.Customer_Id == customerId && c.Status > 0)
                   .Distinct()
                   .ToList();

            return entities.Select(this._CaseDocumentToBusinessModelMapper.Map);
        }

        public CaseDocumentModel GetCaseDocument(int id)
        {
            var entities = this.Table
                    .Where(c => c.Id == id && c.Status > 0)

                   .Distinct()
                   .ToList();

            return entities.Select(this._CaseDocumentToBusinessModelMapper.Map).FirstOrDefault();
        }

      

    }
}
