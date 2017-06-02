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
    using DH.Helpdesk.Domain.Cases;

    public interface ICaseSolutionConditionService
    {
      

        IList<CaseSolutionCondition> GetStateSecondaries(int casesolutionid, int customerid);
        IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int casesolutionid);

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
        public IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int casesolutionid)
        {
            return this._caseSolutionConditionRepository.GetCaseSolutionConditions(casesolutionid).ToList();
        }


        public IList<CaseSolutionCondition> GetStateSecondaries(int casesolutionid, int customerid)
        {
            return this._caseSolutionConditionRepository.GetStateSecondaries(casesolutionid, customerid).ToList();
        }

    }
}