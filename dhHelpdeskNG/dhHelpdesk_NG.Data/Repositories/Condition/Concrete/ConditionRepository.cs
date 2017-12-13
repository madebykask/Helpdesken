using System.Collections.Generic;
using System.Linq;
using System;
using DH.Helpdesk.Domain.Cases;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Globalization;


namespace DH.Helpdesk.Dal.Repositories.Condition.Concrete
{
    using BusinessData.Models.Condition;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;
    using System.Web.Mvc;

    public sealed class ConditionRepository : RepositoryBase<ConditionEntity>, IConditionRepository
    {
        private readonly IEntityToBusinessModelMapper<ConditionEntity, ConditionModel> _ConditionToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<ConditionModel, ConditionEntity> _ConditionToEntityMapper;

        public ConditionRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ConditionEntity, ConditionModel> ConditionToBusinessModelMapper,
            IBusinessModelToEntityMapper<ConditionModel, ConditionEntity> ConditionToEntityMapper)
            : base(databaseFactory)
        {
            _ConditionToBusinessModelMapper = ConditionToBusinessModelMapper;
            _ConditionToEntityMapper = ConditionToEntityMapper;
        }


        public IEnumerable<ConditionModel> GetConditions(int parent_Id, int conditionType_Id)
        {
            var entities = this.Table
                   .Where(c => c.Parent_Id == parent_Id && c.Status != 0 && c.ConditionType_Id == conditionType_Id)
                   .ToList();

            return entities
                .Select(this._ConditionToBusinessModelMapper.Map);
        }

    }
}
