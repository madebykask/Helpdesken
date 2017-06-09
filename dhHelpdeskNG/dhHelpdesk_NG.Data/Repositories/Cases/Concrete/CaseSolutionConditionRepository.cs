﻿using System.Collections.Generic;
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
    using System.Web.Mvc;

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


        public IEnumerable<CaseSolutionSettingsField> GetSelectedCaseSolutionFieldSetting(int casesolutionid)
        {

            List<CaseSolutionSettingsField> list = new List<CaseSolutionSettingsField>();

            var query = from contact in this.DataContext.CaseSolutionsConditions
                        join dealer in this.DataContext.CaseSolutionConditionProperties on contact.Property_Name equals dealer.CaseSolutionConditionProperty
                        where contact.CaseSolution_Id == casesolutionid
                        select new { dealer.CaseSolutionConditionProperty, dealer.Id, dealer.Text, contact.CaseSolutionConditionGUID, contact.Values, contact.CaseSolution_Id };

            foreach (var nameGroup in query)
            {
                CaseSolutionSettingsField c = new CaseSolutionSettingsField();
                c.SelectedValues = new List<string>();

                if (nameGroup.Id != null)
                {
                    c.CaseSolutionConditionId = nameGroup.Id.ToString();
                }
                if (nameGroup.CaseSolution_Id != null)
                {
                    c.CaseSolutionId = nameGroup.CaseSolution_Id;
                }

                if (nameGroup.CaseSolutionConditionProperty != null)
                {
                    c.PropertyName = nameGroup.CaseSolutionConditionProperty.ToString();
                }
                if (nameGroup.Text != null)
                {
                    c.Text = nameGroup.Text.ToString();
                }

                char[] delimiters = new char[] { ',' };
                if (nameGroup.Values != null)
                {
                    string[] parts = nameGroup.Values.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parts.Length; i++)
                    {
                        c.SelectedValues.Add(parts[i]);
                    }
                }

                switch (nameGroup.CaseSolutionConditionProperty.ToString())
                {
                    case "case_StateSecondary.StateSecondaryGUID":

                        var temp = from st in this.DataContext.StateSecondaries
                                   where !c.SelectedValues.Contains(st.StateSecondaryGUID.ToString())
                                   select new { st.Name, st.Id, st.StateSecondaryGUID, Selected = false };

                        var temp1 = from st in this.DataContext.StateSecondaries
                                    where c.SelectedValues.Contains(st.StateSecondaryGUID.ToString())
                                    select new { st.Name, st.Id, st.StateSecondaryGUID, Selected = true };

                        var result = temp.Concat(temp1).OrderBy(x => x.Name).ToList();

                        List<SelectListItem> ls = null;
                        ls = result
                          .Select(x => new SelectListItem
                          {
                              Text = x.Name,
                              Value = x.StateSecondaryGUID.ToString(),
                              Selected = x.Selected
                          }).ToList();


                        c.SelectList = ls;
                        break;
                    case "case_WorkingGroup.WorkingGroupGUID":
                        var temp11 = from st in this.DataContext.WorkingGroups
                                     where !c.SelectedValues.Contains(st.WorkingGroupGUID.ToString())
                                     select new { st.WorkingGroupName, st.Id, st.WorkingGroupGUID, Selected = false };

                        var temp12 = from st in this.DataContext.WorkingGroups
                                     where c.SelectedValues.Contains(st.WorkingGroupGUID.ToString())
                                     select new { st.WorkingGroupName, st.Id, st.WorkingGroupGUID, Selected = true };

                        var result11 = temp11.Concat(temp12).OrderBy(x => x.WorkingGroupName).ToList();

                        List<SelectListItem> ls11 = null;
                        ls11 = result11
                          .Select(x => new SelectListItem
                          {
                              Text = x.WorkingGroupName,
                              Value = x.WorkingGroupGUID.ToString(),
                              Selected = x.Selected
                          }).ToList();


                        c.SelectList = ls11;
                        break;
                    case "case_Priority.PriorityGUID":
                        var temp21 = from st in this.DataContext.Priorities
                                     where !c.SelectedValues.Contains(st.PriorityGUID.ToString())
                                     select new { st.Name, st.Id, st.PriorityGUID, Selected = false };

                        var temp22 = from st in this.DataContext.Priorities
                                     where c.SelectedValues.Contains(st.PriorityGUID.ToString())
                                     select new { st.Name, st.Id, st.PriorityGUID, Selected = true };

                        var result21 = temp21.Concat(temp22).OrderBy(x => x.Name).ToList();

                        List<SelectListItem> ls21 = null;
                        ls21 = result21
                          .Select(x => new SelectListItem
                          {
                              Text = x.Name,
                              Value = x.PriorityGUID.ToString(),
                              Selected = x.Selected
                          }).ToList();


                        c.SelectList = ls21;
                        break;
                    case "case_Status.StatusGUID":
                        var temp31 = from st in this.DataContext.Statuses
                                     where !c.SelectedValues.Contains(st.StatusGUID.ToString())
                                     select new { st.Name, st.Id, st.StatusGUID, Selected = false };

                        var temp32 = from st in this.DataContext.Statuses
                                     where c.SelectedValues.Contains(st.StatusGUID.ToString())
                                     select new { st.Name, st.Id, st.StatusGUID, Selected = true };

                        var result31 = temp31.Concat(temp32).OrderBy(x => x.Name).ToList();

                        List<SelectListItem> ls31 = null;
                        ls31 = result31
                          .Select(x => new SelectListItem
                          {
                              Text = x.Name,
                              Value = x.StatusGUID.ToString(),
                              Selected = x.Selected
                          }).ToList();


                        c.SelectList = ls31;
                        break;
                    case "user_WorkingGroup.WorkingGroupGUID":
                        var temp41 = from st in this.DataContext.WorkingGroups
                                     where !c.SelectedValues.Contains(st.WorkingGroupGUID.ToString())
                                     select new { st.WorkingGroupName, st.Id, st.WorkingGroupGUID, Selected = false };

                        var temp42 = from st in this.DataContext.WorkingGroups
                                     where c.SelectedValues.Contains(st.WorkingGroupGUID.ToString())
                                     select new { st.WorkingGroupName, st.Id, st.WorkingGroupGUID, Selected = true };

                        var result41 = temp41.Concat(temp42).OrderBy(x => x.WorkingGroupName).ToList();

                        List<SelectListItem> ls41 = null;
                        ls41 = result41
                          .Select(x => new SelectListItem
                          {
                              Text = x.WorkingGroupName,
                              Value = x.WorkingGroupGUID.ToString(),
                              Selected = x.Selected
                          }).ToList();


                        c.SelectList = ls41;
                        break;
                    case "case_ProductArea.ProductAreaGUID":
                        var temp51 = from st in this.DataContext.ProductAreas
                                     where !c.SelectedValues.Contains(st.ProductAreaGUID.ToString())
                                     select new { st.Name, st.Id, st.ProductAreaGUID, Selected = false };

                        var temp52 = from st in this.DataContext.ProductAreas
                                     where c.SelectedValues.Contains(st.ProductAreaGUID.ToString())
                                     select new { st.Name, st.Id, st.ProductAreaGUID, Selected = true };

                        var result51 = temp51.Concat(temp52).OrderBy(x => x.Name).ToList();

                        List<SelectListItem> ls51 = null;
                        ls51 = result51
                          .Select(x => new SelectListItem
                          {
                              Text = x.Name,
                              Value = x.ProductAreaGUID.ToString(),
                              Selected = x.Selected
                          }).ToList();


                        c.SelectList = ls51;
                        break;
                }

                list.Add(c);
            }

            return list;

        }

        public IEnumerable<CaseSolutionSettingsField> GetCaseSolutionFieldSetting(int casesolutionid)
        {

            List<CaseSolutionSettingsField> list = new List<CaseSolutionSettingsField>();


            if (casesolutionid == 0)
            {
                var query =
                  from post in this.DataContext.CaseSolutionConditionProperties
                  select new { post.CaseSolutionConditionProperty, post.Id, post.Text };

                foreach (var nameGroup in query)
                {
                    CaseSolutionSettingsField c = new CaseSolutionSettingsField();
                    if (nameGroup.Id != null)
                    {
                        c.CaseSolutionConditionId = nameGroup.Id.ToString();
                    }
                    if (nameGroup.CaseSolutionConditionProperty != null)
                    {
                        c.PropertyName = nameGroup.CaseSolutionConditionProperty.ToString();
                    }
                    if (nameGroup.Text != null)
                    {
                        c.Text = nameGroup.Text.ToString();
                    }

                    list.Add(c);
                }

            }
            else
            {

                var query = from user in this.DataContext.CaseSolutionConditionProperties
                            where !this.DataContext.CaseSolutionsConditions.Any(f => f.Property_Name == user.CaseSolutionConditionProperty)
                            select new { user.Id, user.CaseSolutionConditionProperty, user.Text };
                foreach (var nameGroup in query)
                {
                    CaseSolutionSettingsField c = new CaseSolutionSettingsField();
                    if (nameGroup.Id != null)
                    {
                        c.CaseSolutionConditionId = nameGroup.Id.ToString();
                    }
                    if (nameGroup.CaseSolutionConditionProperty != null)
                    {
                        c.PropertyName = nameGroup.CaseSolutionConditionProperty.ToString();
                    }
                    if (nameGroup.Text != null)
                    {
                        c.Text = nameGroup.Text.ToString();
                    }

                    list.Add(c);
                }
            }



            return list;

        }

        public void Add(int casesolutionid, int conditionid)
        {
            string sql = string.Empty;
            string empty = string.Empty;


            sql = "IF NOT EXISTS (SELECT Id FROM tblCaseSolutionCondition WHERE CaseSolution_Id= " + casesolutionid + " AND Property_Name = (SELECT CaseSolutionConditionProperty FROM dbo.tblCaseSolutionConditionProperties WHERE(Id = " + conditionid + "))) BEGIN ";
            sql += "INSERT INTO tblCaseSolutionCondition (CaseSolution_Id, Property_Name, [Values], [Status]) ";
            sql += "VALUES (" + casesolutionid + ", (SELECT CaseSolutionConditionProperty FROM dbo.tblCaseSolutionConditionProperties WHERE(Id = " + conditionid + ")), '" + empty + "', 1) END ";

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
                    else if (constString == "case_ProductArea.ProductAreaGUID")
                    {
                        var tlist = from xx in this.DataContext.ProductAreas
                                    where xx.Customer_Id == customerid && uids.Contains(xx.ProductAreaGUID.ToString())
                                    select (xx);

                        foreach (var c in tlist)
                        {
                            CaseSolutionCondition ss = new CaseSolutionCondition
                            {

                                Customer_Id = c.Customer_Id,
                                Id = c.Id,
                                IsSelected = 1,
                                Name = c.Name,
                                StateSecondaryGUID = c.ProductAreaGUID.ToString()

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
            else if (constString == "case_ProductArea.ProductAreaGUID")
            {
                List<ProductArea> stfinaList = this.DataContext.ProductAreas.Where(z => z.Customer_Id == customerid).ToList();
                foreach (ProductArea k in stfinaList)
                {
                    bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.ProductAreaGUID.ToString());
                    if (has == false)
                    {
                        CaseSolutionCondition ss = new CaseSolutionCondition
                        {
                            Customer_Id = k.Customer_Id,
                            Id = k.Id,
                            IsSelected = 0,
                            Name = k.Name,
                            StateSecondaryGUID = k.ProductAreaGUID.ToString()

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
