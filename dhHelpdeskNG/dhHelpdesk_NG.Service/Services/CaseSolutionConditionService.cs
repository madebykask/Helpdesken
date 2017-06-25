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



        IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int casesolutionid);


        void Save(CaseSolutionConditionEntity model);

        void Add(int casesolutionid, int conditionid);
        void Remove(string condition, int casesolutionid);

        IList<CaseSolutionCondition> GetCaseSolutionConditionModel(int casesolutionid, int customerid, string constString);
        IEnumerable<CaseSolutionSettingsField> GetCaseSolutionFieldSetting(int casesolutionid);

        IEnumerable<CaseSolutionSettingsField> GetSelectedCaseSolutionFieldSetting(int casesolutionid);

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

        public IEnumerable<CaseSolutionSettingsField> GetCaseSolutionFieldSetting(int casesolutionid)
        {
            return this._caseSolutionConditionRepository.GetCaseSolutionFieldSetting(casesolutionid).ToList();
        }

        public IEnumerable<CaseSolutionSettingsField> GetSelectedCaseSolutionFieldSetting(int casesolutionid)
        {
            return this._caseSolutionConditionRepository.GetSelectedCaseSolutionFieldSetting(casesolutionid).ToList();
        }

        public IList<CaseSolutionCondition> GetCaseSolutionConditionModel(int casesolutionid, int customerid, string constString)
        {
            return this._caseSolutionConditionRepository.GetCaseSolutionConditionModel(casesolutionid, customerid, constString).ToList();
        }


        public void Save(CaseSolutionConditionEntity model)
        {
            this._caseSolutionConditionRepository.Save(model);
        }

        public void Add(int casesolutionid, int conditionidl)
        {
            this._caseSolutionConditionRepository.Add(casesolutionid, conditionidl);
        }

        public void Remove(string condition, int casesolutionid)
        {
            this._caseSolutionConditionRepository.Remove(condition, casesolutionid);
        }
    }

}
