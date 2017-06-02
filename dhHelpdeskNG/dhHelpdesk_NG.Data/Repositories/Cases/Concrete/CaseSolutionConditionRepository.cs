using System.Collections.Generic;
using System.Linq;
using System;

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




        public IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int casesolutionid)
        {
            var entities = this.Table
                   .Where(c => c.CaseSolution_Id == casesolutionid && c.Status != 0)

                   .Distinct()
                   .ToList();

            return entities
                .Select(this._CaseSolutionConditionToBusinessModelMapper.Map);
        }

        public IList<StateSecondary> GetStateSecondaries(int casesolutionid, int customerid)
        {
            string constString = "case_StateSecondary.StateSecondaryGUID";

            List<CaseSolutionConditionEntity> clist = this.DataContext.CaseSolutionsConditions.Where(z => z.Property_Name == constString && z.CaseSolution_Id == casesolutionid && z.Status == 1).ToList();
            List<StateSecondary> selected = new List<StateSecondary>();

            int i = -1;
            if (clist != null)
            {
                if (clist.Count > 0)
                {
                    int count = 0;

                    //Count characters
                    foreach (CaseSolutionConditionEntity ccount in clist)
                    {
                        count = count + ccount.Values.Count(f => f == ',') + 1;
                    }


                    string[] wordsFinal = new string[count];
                    foreach (CaseSolutionConditionEntity c in clist)
                    {
                        string[] words = null;
                        words = c.Values.Split(',');


                        foreach (string s in words)
                        {

                            i = i + 1;

                            wordsFinal[i] = s;

                        }
                    }

                    List<string> uids = new List<string>(wordsFinal);
                    var tlist = from xx in this.DataContext.StateSecondaries
                                where xx.Customer_Id == customerid && uids.Contains(xx.StateSecondaryGUID.ToString())
                                select (xx);

                    foreach (var c in tlist)
                    {
                        StateSecondary ss = new StateSecondary
                        {
                            ChangedDate = c.ChangedDate,
                            CreatedDate = c.CreatedDate,
                            Customer = c.Customer,
                            Customer_Id = c.Customer_Id,
                            Id = c.Id,
                            IncludeInCaseStatistics = c.IncludeInCaseStatistics,
                            IsActive = c.IsActive,
                            IsDefault = 1,
                            MailTemplate = c.MailTemplate,
                            MailTemplate_Id = c.MailTemplate_Id,
                            Name = c.Name,
                            NoMailToNotifier = c.NoMailToNotifier,
                            RecalculateWatchDate = c.RecalculateWatchDate,
                            ReminderDays = c.ReminderDays,
                            ResetOnExternalUpdate = c.ResetOnExternalUpdate,
                            StateSecondaryGUID = c.StateSecondaryGUID,
                            WorkingGroup = c.WorkingGroup,
                            WorkingGroup_Id = c.WorkingGroup_Id



                        };
                        selected.Add(ss);
                    }
                }
            }




            List<StateSecondary> stfinaList = this.DataContext.StateSecondaries.Where(z => z.Customer_Id == customerid).ToList();
            foreach (StateSecondary k in stfinaList)
            {
                bool has = selected.Any(cus => cus.StateSecondaryGUID == k.StateSecondaryGUID);
                if (has == false)
                {
                    StateSecondary ss = new StateSecondary
                    {
                        ChangedDate = k.ChangedDate,
                        CreatedDate = k.CreatedDate,
                        Customer = k.Customer,
                        Customer_Id = k.Customer_Id,
                        Id = k.Id,
                        IncludeInCaseStatistics = k.IncludeInCaseStatistics,
                        IsActive = k.IsActive,
                        IsDefault = 0,
                        MailTemplate = k.MailTemplate,
                        MailTemplate_Id = k.MailTemplate_Id,
                        Name = k.Name,
                        NoMailToNotifier = k.NoMailToNotifier,
                        RecalculateWatchDate = k.RecalculateWatchDate,
                        ReminderDays = k.ReminderDays,
                        ResetOnExternalUpdate = k.ResetOnExternalUpdate,
                        StateSecondaryGUID = k.StateSecondaryGUID,
                        WorkingGroup = k.WorkingGroup,
                        WorkingGroup_Id = k.WorkingGroup_Id



                    };
                    selected.Add(ss);
                }
            }

            var result = selected.OrderBy(x => x.Id).ThenBy(x => x.Name).ToList();

            return result;
        }


    }
}
