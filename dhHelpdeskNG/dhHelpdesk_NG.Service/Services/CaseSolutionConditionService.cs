using DH.Helpdesk.BusinessData.Models.CaseSolution;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Enums.CaseSolution;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Domain;
    using System.Reflection;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.BusinessData.Models.User.Input;

    public interface ICaseSolutionConditionService
    {
        IList<CaseSolutionConditionEntity> GetCaseSolutionCondition(int customerId);

        IList<StateSecondary> GetStateSecondaries(int casesolutionid, int customerid);

    }

    public class CaseSolutionConditionService : ICaseSolutionConditionService
    {
        private readonly ICaseSolutionConditionRepository _caseSolutionConditionRepository;

        public CaseSolutionConditionService(
            ICaseSolutionConditionRepository caseSolutionConditionRepository
            )
        {
            this._caseSolutionConditionRepository = caseSolutionConditionRepository;

        }

        //public int GetAntal(int customerId, int userid)
        //{
        //    return _caseSolutionRepository.GetAntal(customerId, userid);
        //}

        public IList<CaseSolutionConditionEntity> GetCaseSolutionCondition(int casesolutionid)
        {
            return this._caseSolutionConditionRepository.GetCaseSolutionCondition(casesolutionid).ToList();
        }

        public IList<StateSecondary> GetStateSecondaries(int casesolutionid)
        {
            return this._caseSolutionConditionRepository.GetStateSecondaries(casesolutionid, customerid).ToList();
        }

    }
}