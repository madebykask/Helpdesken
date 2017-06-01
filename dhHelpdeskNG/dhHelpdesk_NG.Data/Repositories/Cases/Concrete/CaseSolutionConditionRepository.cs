using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;
    

    public sealed class CaseSolutionConditionRepository : RepositoryBase<CaseSolutionConditionEntity>, ICaseSolutionConditionRepository
    {        
        private readonly IEntityToBusinessModelMapper<CaseSolutionConditionEntity, CaseSolutionConditionModel> _CaseSolutionConditionToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseSolutionConditionModel, CaseSolutionConditionEntity> _CaseSolutionConditionToEntityMapper;

        public CaseSolutionConditionRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<CaseSolutionConditionEntity, CaseSolutionConditionModel> CaseSolutionConditionToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseSolutionConditionModel, CaseSolutionConditionEntity> CaseSolutionConditionToEntityMapper)
            : base(databaseFactory)
        {
            _CaseSolutionConditionToBusinessModelMapper = CaseSolutionConditionToBusinessModelMapper;
            _CaseSolutionConditionToEntityMapper = CaseSolutionConditionToEntityMapper;
        }


        public IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int caseSolution_Id)
        {
        
            var entities = this.Table
                  .Where(c => c.CaseSolution_Id == caseSolution_Id && c.Status != 0)
                  
                  .Distinct()
                  .ToList();

            return entities
                .Select(this._CaseSolutionConditionToBusinessModelMapper.Map);
        }

        public IList<CaseSolutionConditionEntity> GetCaseSolutionCondition(int casesolutionid)
        {
            return this.DataContext.CaseSolutionsConditions.Where(z => z.CaseSolution_Id == casesolutionid).ToList();
         
        }




    }
}
