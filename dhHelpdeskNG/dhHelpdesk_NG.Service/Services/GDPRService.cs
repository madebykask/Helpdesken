using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories.GDPR;
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
        IList<GdprOperationsAuditOverview> ListGdprOperationsAuditItems(int? customerId, bool successOnly = true);
    }

    public interface IGDPRDataPrivacyAccessService
    {
        GDPRDataPrivacyAccess GetByUserId(int userId);
    }

    public interface IGDPRFavoritesService
    {
        IDictionary<int, string> ListFavorites();
        GdprFavoriteModel GetFavorite(int id);
        int SaveFavorite(GdprFavoriteModel model, int? userId);
        void DeleteFavorite(int favoriteId);
    }

    public class GDPRService : IGDPROperationsService, IGDPRDataPrivacyAccessService, IGDPRFavoritesService
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

        public GDPRDataPrivacyAccess GetByUserId(int userId)
        {
            return _privacyAccessRepository.GetByUserId(userId);
        }

        #endregion

        public IDictionary<int, string> ListFavorites()
        {
            var favorites = _gdprFavoritesRepository.ListFavorites();
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

        #endregion
    }
}
   