using System.Collections.Generic;
using System.Linq;
using System;
using DH.Helpdesk.Domain.Cases;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Globalization;


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



        public void Save(CaseSolutionConditionEntity model)
        {
            string sql = string.Empty;
            int istatus = 1;

            sql = "IF EXISTS (SELECT Id FROM tblCaseSolutionCondition WHERE Property_Name = '" + model.Property_Name + "' AND CaseSolution_Id= " + model.CaseSolution_Id + ") BEGIN ";
            sql += "UPDATE tblCaseSolutionCondition SET [Values]='" + model.Values + "' WHERE Property_Name = '" + model.Property_Name + "' AND CaseSolution_Id = " + model.CaseSolution_Id + " END ";
            sql += " ELSE BEGIN INSERT INTO tblCaseSolutionCondition (CaseSolution_Id, Property_Name, [Values], Status) ";
            sql += " VALUES ( " + model.CaseSolution_Id + ", '" + model.Property_Name + "', '" + model.Values + "', " + istatus + " ) END";



            string ConnectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    cmd.CommandText = sql;
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    cmd.ExecuteNonQuery();

                }
            }
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



        public IList<CaseSolutionCondition> GetCaseSolutionConditionModel(int casesolutionid, int customerid, string constString)
        {
            //string constString = "case_WorkingGroup.WorkingGroupGUID";

            List<CaseSolutionConditionEntity> clist = this.DataContext.CaseSolutionsConditions.Where(z => z.Property_Name == constString && z.CaseSolution_Id == casesolutionid && z.Status == 1).ToList();
            List<CaseSolutionCondition> selected = new List<CaseSolutionCondition>();

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


                    //Not guids                  
                    List<string> uids = new List<string>(wordsFinal);

                    foreach (string s in uids)
                    {
                        Guid guidOutput;

                        bool isValid = Guid.TryParse(s.ToString(), out guidOutput);

                        if (isValid == false)
                        {
                            CaseSolutionCondition ss = new CaseSolutionCondition
                            {

                                Customer_Id = customerid,
                                Id = 0,
                                IsSelected = 1,
                                Name = "[" + s + "]",
                                StateSecondaryGUID = s.ToString()

                            };
                            selected.Add(ss);

                        }
                    }

                    if (constString == "case_WorkingGroup.WorkingGroupGUID")
                    {
                        var tlist = from xx in this.DataContext.WorkingGroups
                                    where xx.Customer_Id == customerid && uids.Contains(xx.WorkingGroupGUID.ToString())
                                    select (xx);

                        foreach (var c in tlist)
                        {
                            CaseSolutionCondition ss = new CaseSolutionCondition
                            {

                                Customer_Id = c.Customer_Id,
                                Id = c.Id,
                                IsSelected = 1,
                                Name = c.WorkingGroupName,
                                StateSecondaryGUID = c.WorkingGroupGUID.ToString()

                            };

                            selected.Add(ss);
                        }
                    }
                    else if (constString == "case_StateSecondary.StateSecondaryGUID")
                    {
                        var tlist = from xx in this.DataContext.StateSecondaries
                                    where xx.Customer_Id == customerid && uids.Contains(xx.StateSecondaryGUID.ToString())
                                    select (xx);

                        foreach (var c in tlist)
                        {
                            CaseSolutionCondition ss = new CaseSolutionCondition
                            {

                                Customer_Id = c.Customer_Id,
                                Id = c.Id,
                                IsSelected = 1,
                                Name = c.Name,
                                StateSecondaryGUID = c.StateSecondaryGUID.ToString()

                            };

                            selected.Add(ss);
                        }

                    }
                    else if (constString == "case_Priority.PriorityGUID")
                    {
                        var tlist = from xx in this.DataContext.Priorities
                                    where xx.Customer_Id == customerid && uids.Contains(xx.PriorityGUID.ToString())
                                    select (xx);

                        foreach (var c in tlist)
                        {
                            CaseSolutionCondition ss = new CaseSolutionCondition
                            {

                                Customer_Id = c.Customer_Id,
                                Id = c.Id,
                                IsSelected = 1,
                                Name = c.Name,
                                StateSecondaryGUID = c.PriorityGUID.ToString()

                            };

                            selected.Add(ss);
                        }

                    }

                    else if (constString == "case_Status.StatusGUID")
                    {
                        var tlist = from xx in this.DataContext.Statuses
                                    where xx.Customer_Id == customerid && uids.Contains(xx.StatusGUID.ToString())
                                    select (xx);

                        foreach (var c in tlist)
                        {
                            CaseSolutionCondition ss = new CaseSolutionCondition
                            {

                                Customer_Id = c.Customer_Id,
                                Id = c.Id,
                                IsSelected = 1,
                                Name = c.Name,
                                StateSecondaryGUID = c.StatusGUID.ToString()

                            };

                            selected.Add(ss);
                        }

                    }
                    else if (constString == "user_WorkingGroup.WorkingGroupGUID")
                    {
                        var tlist = from xx in this.DataContext.WorkingGroups
                                    where xx.Customer_Id == customerid && uids.Contains(xx.WorkingGroupGUID.ToString())
                                    select (xx);

                        foreach (var c in tlist)
                        {
                            CaseSolutionCondition ss = new CaseSolutionCondition
                            {

                                Customer_Id = c.Customer_Id,
                                Id = c.Id,
                                IsSelected = 1,
                                Name = c.WorkingGroupName,
                                StateSecondaryGUID = c.WorkingGroupGUID.ToString()

                            };

                            selected.Add(ss);
                        }

                    }
                }
            }



            if (constString == "case_WorkingGroup.WorkingGroupGUID")
            {
                List<WorkingGroupEntity> stfinaList = this.DataContext.WorkingGroups.Where(z => z.Customer_Id == customerid).ToList();
                foreach (WorkingGroupEntity k in stfinaList)
                {
                    bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.WorkingGroupGUID.ToString());
                    if (has == false)
                    {
                        CaseSolutionCondition ss = new CaseSolutionCondition
                        {
                            Customer_Id = k.Customer_Id,
                            Id = k.Id,
                            IsSelected = 0,
                            Name = k.WorkingGroupName,
                            StateSecondaryGUID = k.WorkingGroupGUID.ToString()

                        };


                        selected.Add(ss);
                    }
                }
            }
            else if (constString == "case_StateSecondary.StateSecondaryGUID")
            {
                List<StateSecondary> stfinaList = this.DataContext.StateSecondaries.Where(z => z.Customer_Id == customerid).ToList();
                foreach (StateSecondary k in stfinaList)
                {
                    bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.StateSecondaryGUID.ToString());
                    if (has == false)
                    {
                        CaseSolutionCondition ss = new CaseSolutionCondition
                        {
                            Customer_Id = k.Customer_Id,
                            Id = k.Id,
                            IsSelected = 0,
                            Name = k.Name,
                            StateSecondaryGUID = k.StateSecondaryGUID.ToString()

                        };


                        selected.Add(ss);
                    }
                }
            }
            else if (constString == "case_Priority.PriorityGUID")
            {
                List<Priority> stfinaList = this.DataContext.Priorities.Where(z => z.Customer_Id == customerid).ToList();
                foreach (Priority k in stfinaList)
                {
                    bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.PriorityGUID.ToString());
                    if (has == false)
                    {
                        CaseSolutionCondition ss = new CaseSolutionCondition
                        {
                            Customer_Id = k.Customer_Id,
                            Id = k.Id,
                            IsSelected = 0,
                            Name = k.Name,
                            StateSecondaryGUID = k.PriorityGUID.ToString()

                        };


                        selected.Add(ss);
                    }
                }
            }
            else if (constString == "case_Status.StatusGUID")
            {
                List<Status> stfinaList = this.DataContext.Statuses.Where(z => z.Customer_Id == customerid).ToList();
                foreach (Status k in stfinaList)
                {
                    bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.StatusGUID.ToString());
                    if (has == false)
                    {
                        CaseSolutionCondition ss = new CaseSolutionCondition
                        {
                            Customer_Id = k.Customer_Id,
                            Id = k.Id,
                            IsSelected = 0,
                            Name = k.Name,
                            StateSecondaryGUID = k.StatusGUID.ToString()

                        };


                        selected.Add(ss);
                    }
                }
            }
            else if (constString == "user_WorkingGroup.WorkingGroupGUID")
            {
                List<WorkingGroupEntity> stfinaList = this.DataContext.WorkingGroups.Where(z => z.Customer_Id == customerid).ToList();
                foreach (WorkingGroupEntity k in stfinaList)
                {
                    bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.WorkingGroupGUID.ToString());
                    if (has == false)
                    {
                        CaseSolutionCondition ss = new CaseSolutionCondition
                        {
                            Customer_Id = k.Customer_Id,
                            Id = k.Id,
                            IsSelected = 0,
                            Name = k.WorkingGroupName,
                            StateSecondaryGUID = k.WorkingGroupGUID.ToString()

                        };


                        selected.Add(ss);
                    }
                }
            }
            var result = selected.OrderBy(x => x.Id).ThenBy(x => x.Name).ToList();

            return result;
        }



    }
}
