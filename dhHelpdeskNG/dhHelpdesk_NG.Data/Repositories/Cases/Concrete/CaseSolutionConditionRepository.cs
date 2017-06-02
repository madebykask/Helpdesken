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
            List<CaseSolutionConditionEntity> s = this.DataContext.CaseSolutionsConditions.Where(z => z.CaseSolution_Id == casesolutionid && z.Status > 0).ToList();
            var t = s.Where(z => z.Property_Name == "New");

            if (casesolutionid == 0)
            {
                if (t.Count() == 0)
                {
                    CaseSolutionConditionEntity newent = new CaseSolutionConditionEntity
                    {
                        Property_Name = "[New]",
                        Id = 0,
                        CaseSolutionConditionGUID = Guid.Empty
                    };
                    s.Add(newent);
                }
            }
            return s.ToList();// this.DataContext.CaseSolutionsConditions.Where(z => z.CaseSolution_Id == casesolutionid).ToList();

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
                            IsDefault = c.IsDefault,
                            MailTemplate = c.MailTemplate,
                            MailTemplate_Id = c.MailTemplate_Id,
                            Name = c.Name,
                            NoMailToNotifier = c.NoMailToNotifier,
                            RecalculateWatchDate = c.RecalculateWatchDate,
                            ReminderDays = c.ReminderDays,
                            ResetOnExternalUpdate = c.ResetOnExternalUpdate,
                            StateSecondaryGUID = c.StateSecondaryGUID,
                            WorkingGroup = c.WorkingGroup,
                            WorkingGroup_Id = c.WorkingGroup_Id,
                            Selected=true
                            


                        };
                        selected.Add(ss);
                    }
                }
            }

            List<StateSecondary> stfinaList = this.DataContext.StateSecondaries.Where(z => z.Customer_Id == customerid).ToList();

            var result = selected.Union(stfinaList).OrderBy(x => x.Id).ThenBy(x=>x.Name) .ToList();

            return result;
        }


    }
}
