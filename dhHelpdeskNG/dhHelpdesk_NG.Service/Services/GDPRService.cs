using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.BusinessRules;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Services.Services
{
    public enum GDPROperations
    {
        DataPrivacy
    }

    public interface IGDPROperationsService
    {
        IDictionary<int, string> GetOperationAuditCustomers();
        IDictionary<int, string> ListFavorites(GDPRDataPrivacyAccess privacyAccess);
        IList<GdprOperationsAuditOverview> ListGdprOperationsAuditItems(int? customerId, bool successOnly = true);
    }

    public interface IGDPRDataPrivacyAccessService
    {
        GDPRDataPrivacyAccess GetUserWithPrivacyPermissionsByUserId(int userId);
    }

    public interface IGDPRDataPrivacyCasesService
    {
        IList<int> GetCaseParents(int customerId, DataPrivacyParameters p, IUnitOfWork uow);
        int GetCasesCount(int customerId, DataPrivacyParameters p);

        IQueryable<Case> GetCasesQuery(int customerId, DataPrivacyParameters p, IUnitOfWork uow);
    }

    public interface IGDPRFavoritesService
    {
        IDictionary<int, string> ListFavorites(GDPRDataPrivacyAccess privacyAccess);
        GdprFavoriteModel GetFavorite(int id);
        int SaveFavorite(GdprFavoriteModel model, int? userId);
        void DeleteFavorite(int favoriteId);
        DataPrivacyParameters CreateParameters(int favoriteId);
    }

    public class GDPRService : IGDPROperationsService, IGDPRDataPrivacyAccessService, IGDPRFavoritesService, IGDPRDataPrivacyCasesService
    {
        private readonly IGDPRDataPrivacyAccessRepository _privacyAccessRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IGDPROperationsAuditRespository _gdprOperationsAuditRespository;
        private readonly IJsonSerializeService _jsonSerializeService;
        private readonly IGDPRDataPrivacyFavoriteRepository _gdprFavoritesRepository;

        private readonly IBusinessModelToEntityMapper<GdprFavoriteModel, GDPRDataPrivacyFavorite> _gdprFavoritesEntityMapper;
        private readonly IEntityToBusinessModelMapper<GDPRDataPrivacyFavorite, GdprFavoriteModel> _gdprFavoritesModelMapper;
        
        #region ctor()

        public GDPRService(IGDPRDataPrivacyAccessRepository privacyAccessRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IGDPROperationsAuditRespository operationsAuditRespository,
            IGDPRDataPrivacyFavoriteRepository favoritesRepository,
            IBusinessModelToEntityMapper<GdprFavoriteModel, GDPRDataPrivacyFavorite> gdprFavoritesEntityMapper,
            IEntityToBusinessModelMapper<GDPRDataPrivacyFavorite, GdprFavoriteModel> gdprFavoritesModelMapper,
            IJsonSerializeService jsonSerializeService)
        {
            _gdprFavoritesEntityMapper = gdprFavoritesEntityMapper;
            _gdprFavoritesModelMapper = gdprFavoritesModelMapper;
            _gdprFavoritesRepository = favoritesRepository;
            _jsonSerializeService = jsonSerializeService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _privacyAccessRepository = privacyAccessRepository;
            _gdprOperationsAuditRespository = operationsAuditRespository;
        }

        #endregion

        #region IGDPRDataPrivacyAccessService

        public GDPRDataPrivacyAccess GetUserWithPrivacyPermissionsByUserId(int userId)
        {
            return _privacyAccessRepository.GetUserWithPrivacyPermissionsByUserId(userId);
        }

        #endregion

        public IDictionary<int, string> ListFavorites(GDPRDataPrivacyAccess privacyAccess)
        {
            var favorites = _gdprFavoritesRepository.ListFavorites(privacyAccess);
            return favorites;
        }

        public GdprFavoriteModel GetFavorite(int id)
        {
            var entity = _gdprFavoritesRepository.GetById(id);
            var model = _gdprFavoritesModelMapper.Map(entity);
            return model;
        }

        public void DeleteFavorite(int favoriteId)
        {
            _gdprFavoritesRepository.Delete(f => f.Id == favoriteId);
            _gdprFavoritesRepository.Commit();
        }

        public int SaveFavorite(GdprFavoriteModel model, int? userId)
        {
            int res;
            using (var uow = _unitOfWorkFactory.Create())
            {
                var repo = uow.GetRepository<GDPRDataPrivacyFavorite>();
                var entity = repo.GetById(model.Id);
                if (entity == null)
                {
                    entity = new GDPRDataPrivacyFavorite
                    {
                        CreatedByUser_Id = userId
                    };

                    repo.Add(entity);
                }

                _gdprFavoritesEntityMapper.Map(model, entity);

                entity.ChangedByUser_Id = userId;
                entity.ChangedDate = DateTime.Now;

                uow.Save();

                res = entity.Id;
            }

            return res;
        }
        
        public DataPrivacyParameters CreateParameters(int favoriteId)
        {
            var favoriteData = _gdprFavoritesRepository.GetById(favoriteId);

            var dpp = new DataPrivacyParameters()
            {
                TaskId = 0,
                SelectedCustomerId = favoriteData.CustomerId,
                SelectedFavoriteId = favoriteId,
                RetentionPeriod = favoriteData.RetentionPeriod,
                RegisterDateTo = favoriteData.RegisterDateTo,
                RegisterDateFrom = favoriteData.RegisterDateFrom,
                FinishedDateTo = favoriteData.FinishedDateTo,
                FinishedDateFrom = favoriteData.FinishedDateFrom,
                ClosedOnly = favoriteData.ClosedOnly,
                ReplaceEmails = favoriteData.ReplaceEmails,
                FieldsNames = favoriteData.FieldsNames?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                ReplaceDataWith = favoriteData.ReplaceDataWith,
                ReplaceDatesWith = favoriteData.ReplaceDatesWith,
                RemoveCaseAttachments = favoriteData.RemoveCaseAttachments,
                RemoveLogAttachments = favoriteData.RemoveLogAttachments,
                RemoveFileViewLogs = favoriteData.RemoveFileViewLogs,
                CaseTypes = favoriteData.CaseTypes?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                ProductAreas = favoriteData.ProductAreas?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                GDPRType = favoriteData.GDPRType
            };

            return dpp;
        }

        private DataPrivacyParameters DeserializeOperationParameters(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var parameters = _jsonSerializeService.Deserialize<DataPrivacyParameters>(data);
            return parameters;
        }
        
        #region Audit Methods

        public IDictionary<int, string> GetOperationAuditCustomers()
        {
            var res = _gdprOperationsAuditRespository.GetOperationsAuditCustomers();
            return res;
        }

        public IList<GdprOperationsAuditOverview> ListGdprOperationsAuditItems(int? customerId, bool successOnly = true)
        {
            var res = new List<GdprOperationsAuditOverview>();

            var cusId = customerId ?? 0;
            var operationName = GDPROperations.DataPrivacy.ToString();
            var query = _gdprOperationsAuditRespository.GetMany(x =>  (cusId == 0 || x.Customer_Id == cusId) && x.Operation == operationName);
            

            if (successOnly)
            {
                query = query.Where(x => x.Success);
            }

            var items = query.Select(x => new
            {
                CustomerId = x.Customer_Id,
                Data = x.Parameters,
                x.CreatedDate
            }).OrderByDescending(x => x.CreatedDate).ToList();

            foreach (var item in items)
            {
                var operationParams = DeserializeOperationParameters(item.Data);
                if (operationParams != null)
                {
                    var auditData = new GdprOperationsAuditOverview
                    {
                        CustomerId = item.CustomerId ?? 0,
                        RegDateFrom = operationParams.RegisterDateFrom,
                        RegDateTo = operationParams.RegisterDateTo,
                        ClosedOnly = operationParams.ClosedOnly,
                        ReplaceEmails = operationParams.ReplaceEmails,
                        Fields = operationParams.FieldsNames,
                        //CaseTypes = operationParams.CaseTypes,
                        RemoveCaseAttachments = operationParams.RemoveCaseAttachments,
                        RemoveLogAttachments = operationParams.RemoveLogAttachments,
                        ExecutedDate = item.CreatedDate
                    };

                    res.Add(auditData);
                }
            }
            return res;
        }

        public void UpdateAuditOperationData(GDPROperationsAudit auditData)
        {
            _gdprOperationsAuditRespository.Update(auditData);
            _gdprOperationsAuditRespository.Commit();
        }

        public int GetCasesCount(int customerId, DataPrivacyParameters p)
        {
            var sqlTimeout = 300;
            using (var uow = _unitOfWorkFactory.Create(sqlTimeout))
            {
                uow.AutoDetectChangesEnabled = false;
                var casesQueryable = GetCasesQuery(customerId, p, uow);
                return casesQueryable.Count();
            }
        }

        public IQueryable<Case> GetCasesQuery(int customerId, DataPrivacyParameters p, IUnitOfWork uow)
        {
            var caseRepository = uow.GetRepository<Case>();
            var casesQueryable = caseRepository.GetAll()
                .IncludePath(c => c.CaseHistories)
                .IncludePath(c => c.CaseExtendedCaseDatas.Select(ec => ec.ExtendedCaseData.ExtendedCaseValues))
                .IncludePath(c => c.CaseExtendedCaseDatas.Select(ec => ec.ExtendedCaseForm))
                .IncludePath(c => c.CaseSectionExtendedCaseDatas.Select(esc => esc.ExtendedCaseData.ExtendedCaseValues))
                .Where(x => x.Customer_Id == customerId);

            if (p.CaseTypes != null)
            {
                if (p.CaseTypes.Any())
                {
                    casesQueryable = casesQueryable.Where(c => p.CaseTypes.Contains(c.CaseType_Id.ToString()));
                }
            }
            if (p.ProductAreas != null)
            {
                if (p.ProductAreas.Any())
                {
                    casesQueryable = casesQueryable.Where(c => p.ProductAreas.Contains(c.ProductArea_Id.ToString()));
                }
            }

            if (p.RemoveCaseAttachments)
            {
                casesQueryable = casesQueryable.IncludePath(x => x.CaseFiles);
            }

            if (p.RemoveLogAttachments)
            {
                casesQueryable = casesQueryable.IncludePath(x => x.Logs.Select(f => f.LogFiles));
            }
            
            if (p.FieldsNames != null)
            {
                if(p.FieldsNames.Contains("tblLog.Text_External") ||
                p.FieldsNames.Contains("tblLog.Text_Internal") ||
                p.FieldsNames.Contains(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()))
                {
                    casesQueryable = casesQueryable.IncludePath(x => x.Logs.Select(f => f.LogFiles));
                }
                if (p.FieldsNames.Contains(GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString()))
                {
                    casesQueryable = casesQueryable.IncludePath(x => x.CaseFollowers);
                }
            }
            
            if (p.ReplaceEmails && p.GDPRType != (int)GDPRType.Radering)
            {
                casesQueryable = casesQueryable.IncludePath(x => x.Mail2Tickets);
                casesQueryable = casesQueryable.IncludePath(x => x.CaseHistories.Select(y => y.Emaillogs));
            }

            if (p.ClosedOnly)
                casesQueryable = casesQueryable.Where(x => x.FinishingDate.HasValue);

            if (p.RegisterDateFrom.HasValue)
            {
                p.RegisterDateFrom = p.RegisterDateFrom.Value.Date;
                casesQueryable = casesQueryable.Where(x => x.RegTime >= p.RegisterDateFrom.Value);
            }

            if (p.RegisterDateTo.HasValue)
            {
                //fix date
                p.RegisterDateTo = p.RegisterDateTo.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                casesQueryable = casesQueryable.Where(x => x.RegTime <= p.RegisterDateTo.Value);
            }

            if (p.FinishedDateFrom.HasValue)
            {
                p.FinishedDateFrom = p.FinishedDateFrom.Value.Date;
                casesQueryable = casesQueryable.Where(x => x.FinishingDate >= p.FinishedDateFrom.Value);
            }

            if (p.FinishedDateTo.HasValue)
            {
                //fix date
                p.FinishedDateTo = p.FinishedDateTo.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                casesQueryable = casesQueryable.Where(x => x.FinishingDate <= p.FinishedDateTo.Value);
            }

            casesQueryable = casesQueryable.OrderBy(x => x.Id);
            return casesQueryable;
        }

        public IList<int> GetCaseParents(int customerId, DataPrivacyParameters p, IUnitOfWork uow)
        {

            var casesQuery = GetCasesQuery(customerId, p, uow);

            var parentChildRepository = uow.GetRepository<ParentChildRelation>();

            var res = from pc in parentChildRepository.GetAll()
                      join c in casesQuery
                      on pc.AncestorId equals c.Id
                      select pc;

            return res.Select(c=> c.AncestorId).ToList();
        }

        #endregion
    }
}
   