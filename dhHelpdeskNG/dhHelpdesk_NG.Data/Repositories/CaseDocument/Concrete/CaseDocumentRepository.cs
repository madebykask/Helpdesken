using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories.CaseDocument.Concrete
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;
    using System;

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

        public IList<CaseDocumentOverview> GetCustomerCaseDocumentsWithConditions(int caseId, int customerId)
        {
            var entities = this.Table
                .Where(c => c.Customer_Id == customerId && c.Status > 0)
                .Select(c => new CaseDocumentOverview
                {
                    Id = c.Id,
                    Name = c.Name,
                    FileType = c.FileType,
                    SortOrder = c.SortOrder,
                    CaseId = caseId,
                    CaseDocumentGUID = c.CaseDocumentGUID,
                    CaseDocumentTemplate_Id = c.CaseDocumentTemplate_Id,
                    Conditions = c.Conditions.Select(cc => new DocumentConditionOverview()
                    {
                        Properpty = cc.Property_Name,
                        Values = cc.Values
                    }).ToList()
                })
                .ToList();

            return entities;
        }

        public CaseDocumentModel GetCaseDocumentFull(Guid caseDocumentGUID)
        {
            var query =
                from caseDoc in this.DataContext.CaseDocuments
                where caseDoc.CaseDocumentGUID == caseDocumentGUID && caseDoc.Status > 0
                select new CaseDocumentModel
                {
                    Id = caseDoc.Id,
                    Name = caseDoc.Name,
                    CaseDocumentTemplate_Id = caseDoc.CaseDocumentTemplate_Id,
                    CaseDocumentGUID = caseDoc.CaseDocumentGUID,
                    Customer_Id = caseDoc.Customer_Id,
                    Description = caseDoc.Description,
                    FileType = caseDoc.FileType,
                    SortOrder = caseDoc.SortOrder,
                    Status = caseDoc.Status,
                    CaseDocumentTemplate = caseDoc.CaseDocumentTemplate,
                    CaseDocumentParagraphs =
                        (from parKey in caseDoc.CaseDocumentParagraphsKeys
                         let par = parKey.CaseDocumentParagraph
                         select new CaseDocumentParagraphModel
                            {
                                Id = par.Id,
                                Name = par.Name,
                                Description = par.Description,
                                ParagraphType = par.ParagraphType,
                                SortOrder = parKey.SortOrder,
                                CaseDocumentTexts = par.CaseDocumentTexts.Select(t => new CaseDocumentTextModel()
                                    {
                                        Id = t.Id,
                                        Headline = t.Headline,
                                        Name = t.Name,
                                        Text = t.Text,
                                        Description = t.Description,
                                        SortOrder = t.SortOrder,
                                        Conditions =
                                            t.Conditions.Where(tc => tc.Status != 0)
                                                .Select(tc => new CaseDocumentTextConditionModel()
                                                {
                                                    Id = tc.Id,
                                                    Property_Name = tc.Property_Name,
                                                    Values = tc.Values,
                                                    CaseDocumentTextConditionGUID = tc.CaseDocumentTextConditionGUID,
                                                    CaseDocumentText_Id = tc.CaseDocumentText_Id,
                                                    Operator = tc.Operator
                                                }).ToList()
                                    }).OrderBy(x => x.SortOrder).ToList()
                            }).ToList()
                };

            var res = query.FirstOrDefault();
            return res;
        }
    }
}
