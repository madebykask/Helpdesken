using System.Collections.Generic;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.ExtendedCase;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Cases;

    public sealed class WorkFlowRepository : RepositoryBase<WorkFlowEntity>, IWorkFlowRepository
    {        
        private readonly IEntityToBusinessModelMapper<WorkFlowEntity, WorkFlowModel> _workFlowToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<WorkFlowModel, WorkFlowEntity> _workFlowToEntityMapper;

        public WorkFlowRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<WorkFlowEntity, WorkFlowModel> workFlowToBusinessModelMapper,
            IBusinessModelToEntityMapper<WorkFlowModel, WorkFlowEntity> wokFlowToEntityMapper)
            : base(databaseFactory)
        {
            _workFlowToBusinessModelMapper = workFlowToBusinessModelMapper;
            _workFlowToEntityMapper = wokFlowToEntityMapper;
        }


        public IList<WorkFlowModel> GetWorkFlowItems(int caseId)
        {
            return new List<WorkFlowModel>();
        }
    }
}
