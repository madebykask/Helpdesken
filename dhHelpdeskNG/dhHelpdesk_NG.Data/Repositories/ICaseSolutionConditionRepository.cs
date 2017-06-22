namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain.Cases;

    public interface ICaseSolutionConditionRepository : IRepository<CaseSolutionConditionEntity>
    {
        /// <summary>
        /// Returns all active conditions for a CaseSolution and cache them on server for 60 minutes
        /// </summary>
        /// <param name="caseSolution_Id">
        /// The id of CaseSolution.
        /// </param>
        IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int casesolutionid);

        IEnumerable<CaseSolutionSettingsField> GetCaseSolutionFieldSetting(int casesolutionid);

        IEnumerable<CaseSolutionSettingsField> GetSelectedCaseSolutionFieldSetting(int casesolutionid);

        void Save(CaseSolutionConditionEntity model);


        void Add(int casesolutionid, int conditionid);

        void Remove(string condition, int casesolutionid);

        IList<CaseSolutionCondition> GetCaseSolutionConditionModel(int casesolutionid, int customerid, string constString);

        void DeleteByCaseSolutionId(int id);
    }
}