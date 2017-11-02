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

        public void DeleteByCaseSolutionId(int id)
        {

            // this.DataContext.CaseSolutionsConditions.ToList().RemoveAll(x => x.CaseSolution_Id == id);
            //query.RemoveAll(x => x.CaseSolution_Id == id);
            string sql = "DELETE FROM tblCaseSolutionCondition WHERE CaseSolution_Id = " + id + "";
            string ConnectionStringExt = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;

            using (var connectionExt = new SqlConnection(ConnectionStringExt))
            {
                if (connectionExt.State == ConnectionState.Closed)
                {
                    connectionExt.Open();
                }
                using (var commandExt = new SqlCommand { Connection = connectionExt, CommandType = CommandType.Text, CommandTimeout = 0 })
                {
                    commandExt.CommandType = CommandType.Text;
                    commandExt.CommandText = sql;
                    commandExt.ExecuteNonQuery();

                }
            }

            //query.ForEach(x => this.DataContext.Remove(x));
        }

        public IEnumerable<CaseSolutionSettingsField> GetSelectedCaseSolutionFieldSetting(int casesolutionid, int customerid)
        {

            List<CaseSolutionSettingsField> list = new List<CaseSolutionSettingsField>();



            string sqlExt = string.Empty;

            sqlExt = "SELECT ";
            sqlExt += "0, ";
            sqlExt += "[CaseSolution_Id] AS[CaseSolution_Id],  ";
            sqlExt += "[CaseSolutionConditionProperty] AS[CaseSolutionConditionProperty],  ";
            sqlExt += "[dbo].[tblCaseSolutionConditionProperties].[Id] AS[Id],  ";
            sqlExt += "[Text] AS[Text],  ";
            sqlExt += "Convert(nvarchar(50), [CaseSolutionConditionGUID]) AS[CaseSolutionConditionGUID],  ";
            sqlExt += "[Values] AS[Values],  ";
            sqlExt += "[Table] AS[Table],  ";
            sqlExt += "[TableFieldId] AS[TableFieldId],  ";
            sqlExt += "[TableFieldName] AS[TableFieldName],  ";
            sqlExt += "[TableFieldGuid] AS[TableFieldGuid], ";
            sqlExt += "[TableParentId] AS[TableParentId], ";
            sqlExt += "[TableFieldStatus] AS[TableFieldStatus], ";
            sqlExt += "[tblCaseSolutionConditionProperties].SortOrder AS SortOrder ";
            sqlExt += "FROM[dbo].[tblCaseSolutionCondition] ";
            sqlExt += "INNER JOIN[dbo].[tblCaseSolutionConditionProperties] ";
            sqlExt += "ON[Property_Name] = [CaseSolutionConditionProperty] ";
            sqlExt += "WHERE[CaseSolution_Id] = " + casesolutionid + " ";
            sqlExt += "UNION ";
            sqlExt += "SELECT   ";
            sqlExt += "1, ";
            sqlExt += "" + casesolutionid + " AS[CaseSolution_Id], ";
            sqlExt += "CaseSolutionConditionProperty,  ";
            sqlExt += "tblCaseSolutionConditionProperties.Id, ";
            sqlExt += "tblCaseSolutionConditionProperties.Text,  ";
            sqlExt += "'' AS[CaseSolutionConditionGUID], ";
            sqlExt += "'' AS[Values], ";
            sqlExt += "[Table],  ";
            sqlExt += "TableFieldId,  ";
            sqlExt += "TableFieldName,  ";
            sqlExt += "TableFieldGuid, ";
            sqlExt += "TableParentId, ";
            sqlExt += "TableFieldStatus, ";
            sqlExt += " [tblCaseSolutionConditionProperties].SortOrder AS SortOrder ";
            sqlExt += "FROM dbo.tblCaseSolutionConditionProperties ";
            sqlExt += "WHERE tblCaseSolutionConditionProperties.Id NOT IN(SELECT ";
            sqlExt += "[dbo].[tblCaseSolutionConditionProperties].[Id] ";
            sqlExt += "FROM  [dbo].[tblCaseSolutionCondition] ";
            sqlExt += "INNER JOIN [dbo].[tblCaseSolutionConditionProperties] ON[Property_Name] = [CaseSolutionConditionProperty] ";
            sqlExt += "WHERE[CaseSolution_Id] = " + casesolutionid + ") ";
            sqlExt += " ORDER BY SortOrder ";

            string ConnectionStringExt = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
            DataTable dtExt = null;

            using (var connectionExt = new SqlConnection(ConnectionStringExt))
            {
                if (connectionExt.State == ConnectionState.Closed)
                {
                    connectionExt.Open();
                }
                using (var commandExt = new SqlCommand { Connection = connectionExt, CommandType = CommandType.StoredProcedure, CommandTimeout = 0 })
                {
                    commandExt.CommandType = CommandType.Text;
                    commandExt.CommandText = sqlExt;
                    var reader = commandExt.ExecuteReader();
                    dtExt = new DataTable();
                    dtExt.Load(reader);

                }
            }


            foreach (DataRow rowExt in dtExt.Rows)
            {
                CaseSolutionSettingsField c = new CaseSolutionSettingsField();
                c.SelectedValues = new List<string>();

                if (rowExt["Id"].ToString() != null)
                {
                    c.CaseSolutionConditionId = rowExt["Id"].ToString();
                }

                if (rowExt["CaseSolution_Id"].ToString() != null)
                {
                    c.CaseSolutionId = Convert.ToInt32(rowExt["CaseSolution_Id"].ToString());
                }

                if (rowExt["CaseSolutionConditionProperty"].ToString() != null)
                {
                    c.PropertyName = rowExt["CaseSolutionConditionProperty"].ToString();
                }
                if (rowExt["Text"].ToString() != null)
                {
                    c.Text = rowExt["Text"].ToString();
                }
                char[] delimiters = new char[] { ',' };

                if (rowExt["Values"].ToString() != null)
                {
                    string[] parts = rowExt["Values"].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parts.Length; i++)
                    {
                        c.SelectedValues.Add(parts[i]);
                    }
                }

                string tablename = rowExt["Table"].ToString();
                string tablefieldid = rowExt["TableFieldId"].ToString();
                string tablefieldname = rowExt["TableFieldName"].ToString();
                string tablefieldguid = rowExt["TableFieldGuid"].ToString();
                string tableParentId = rowExt["TableParentId"].ToString();
                string tableFieldStatus = rowExt["TableFieldStatus"].ToString();

                string selvals = string.Empty;
                foreach (var it in c.SelectedValues)
                {
                    if (selvals == string.Empty)
                    {
                        selvals = "'" + it.ToString() + "'";
                    }
                    else
                    {
                        selvals = selvals + ", '" + it.ToString() + "'";
                    }
                }


                selvals = selvals.Replace("''", "'");
                string sql = string.Empty;

                if (c.SelectedValues.Count > 0)
                {
                    sql = "SELECT ";
                    sql += "[" + tablefieldid + "] AS Id, ";
                    if (string.IsNullOrEmpty(tableParentId))
                    {
                        sql += "[" + tablefieldname + "] AS Name, ";

                    }
                    else
                    {
                        sql += "isnull(dbo.GetHierarchy(" + tablefieldid + ", '" + tablename + "'), '') AS Name, ";
                    }
                    sql += "" + tablefieldguid + " AS Guid, ";
                    sql += "cast(0 as bit) AS [Selected], ";
                    if (!string.IsNullOrEmpty(tableFieldStatus))
                    {
                        sql += "cast([" + tableFieldStatus + "] as bit) AS Status ";
                    }
                    else
                    {
                        sql += "cast(1 as bit) AS Status ";
                    }
                    sql += "FROM " + tablename + " ";
                    sql += "AS [Extent1] ";
                    sql += "WHERE  NOT((LOWER( CAST( [Extent1]. " + tablefieldguid + " AS nvarchar(max)))  ";
                    sql += "IN(" + selvals + ")) ";
                    sql += "AND(LOWER( CAST( [Extent1]." + tablefieldguid + " AS nvarchar(max))) IS NOT NULL)) AND (Customer_Id is null or Customer_Id= " + customerid + ") ";

                    sql += " UNION ";

                    sql += "SELECT ";
                    sql += "[" + tablefieldid + "] AS Id, ";
                    if (string.IsNullOrEmpty(tableParentId))
                    {
                        sql += "[" + tablefieldname + "] AS Name, ";

                    }
                    else
                    {
                        sql += "isnull(dbo.GetHierarchy(" + tablefieldid + ", '" + tablename + "'), '') AS Name, ";
                    }
                    sql += "" + tablefieldguid + " AS Guid, ";
                    sql += "cast(1 as bit) AS [Selected], ";
                    if (!string.IsNullOrEmpty(tableFieldStatus))
                    {
                        sql += "cast([" + tableFieldStatus + "] as bit) AS Status ";
                    }
                    else
                    {
                        sql += "cast(1 as bit) AS Status ";
                    }
                    sql += "FROM " + tablename + " ";
                    sql += "AS[Extent1] ";
                    sql += "WHERE(LOWER( CAST( [Extent1]." + tablefieldguid + " AS nvarchar(max))) ";
                    sql += "IN(" + selvals + ")) ";
                    sql += "AND(LOWER( CAST( [Extent1]." + tablefieldguid + " AS nvarchar(max))) IS NOT NULL) AND (Customer_Id is null or Customer_Id= " + customerid + ") ";
                    sql += " ORDER BY Name";
                }
                else
                {
                    sql = "SELECT ";
                    sql += "[" + tablefieldid + "] AS Id, ";
                    if (string.IsNullOrEmpty(tableParentId))
                    {
                        sql += "[" + tablefieldname + "] AS Name, ";
                    }
                    else
                    {
                        sql += "isnull(dbo.GetHierarchy(" + tablefieldid + ", '" + tablename + "'), '') AS Name, ";
                    }
                    sql += "" + tablefieldguid + " AS Guid, ";
                    sql += "cast(0 as bit) AS [Selected], ";
                    if (!string.IsNullOrEmpty(tableFieldStatus))
                    {
                        sql += "cast([" + tableFieldStatus + "] as bit) AS Status ";
                    }
                    else
                    {
                        sql += "cast(1 as bit) AS Status ";
                    }
                    sql += "FROM " + tablename + " ";
                    sql += "AS [Extent1] WHERE (Customer_Id is null or Customer_Id= " + customerid + ")"; //
                    sql += " ORDER BY Name";
                }


                string ConnectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
                DataTable dt = null;

                using (var connection = new SqlConnection(ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.StoredProcedure, CommandTimeout = 0 })
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        var reader = command.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(reader);
                    }
                }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        //var result="";
                        //result.Add(1);
                        List<DataRow> result = dt.AsEnumerable().ToList();

                        List<SelectListItem> ls = null;
                        ls = result
                          .Select(x => new SelectListItem
                          {
                              Text = x[1].ToString(),
                              Value = x[2].ToString(),
                              Selected = Convert.ToBoolean(x[3].ToString()),
                              Disabled = !Convert.ToBoolean(x[4].ToString())
                          }).ToList();


                        c.SelectList = ls;

                    }
                }

                list.Add(c);

            }

            //foreach (var k in list)
            //{
            //    foreach (var l in k.SelectList)
            //    {
            //        l.Text = Translation.Get("Ja", Enums.TranslationSource.TextTranslation);
            //    }
            //}

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
                string sql = string.Empty;

                sql = "SELECT dbo.tblCaseSolutionConditionProperties.Id, dbo.tblCaseSolutionConditionProperties.CaseSolutionConditionProperty, ";
                sql += "dbo.tblCaseSolutionConditionProperties.Text ";
                sql += "FROM            dbo.tblCaseSolutionConditionProperties ";
                sql += "WHERE dbo.tblCaseSolutionConditionProperties.Id NOT IN(SELECT        dbo.tblCaseSolutionConditionProperties.Id ";
                sql += "FROM            dbo.tblCaseSolutionCondition INNER JOIN ";
                sql += "dbo.tblCaseSolutionConditionProperties ON ";
                sql += "dbo.tblCaseSolutionCondition.Property_Name = dbo.tblCaseSolutionConditionProperties.CaseSolutionConditionProperty ";
                sql += "WHERE(dbo.tblCaseSolutionCondition.CaseSolution_Id = " + casesolutionid + ")) ";

                string ConnectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
                DataTable dt = null;

                using (var connection = new SqlConnection(ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.StoredProcedure, CommandTimeout = 0 })
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        var reader = command.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(reader);

                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    CaseSolutionSettingsField c = new CaseSolutionSettingsField();
                    if (row["Id"].ToString() != null)
                    {
                        c.CaseSolutionConditionId = row["Id"].ToString();
                    }
                    if (row["CaseSolutionConditionProperty"].ToString() != null)
                    {
                        c.PropertyName = row["CaseSolutionConditionProperty"].ToString();
                    }
                    if (row["Text"].ToString() != null)
                    {
                        c.Text = row["Text"].ToString();
                    }
                    list.Add(c);
                }

                //var query = from user in this.DataContext.CaseSolutionConditionProperties
                //            where !this.DataContext.CaseSolutionsConditions.Any(f => f.Property_Name == user.CaseSolutionConditionProperty)
                //            select new { user.Id, user.CaseSolutionConditionProperty, user.Text };
                //foreach (var nameGroup in query)
                //{
                //    CaseSolutionSettingsField c = new CaseSolutionSettingsField();
                //    if (nameGroup.Id != null)
                //    {
                //        c.CaseSolutionConditionId = nameGroup.Id.ToString();
                //    }
                //    if (nameGroup.CaseSolutionConditionProperty != null)
                //    {
                //        c.PropertyName = nameGroup.CaseSolutionConditionProperty.ToString();
                //    }
                //    if (nameGroup.Text != null)
                //    {
                //        c.Text = nameGroup.Text.ToString();
                //    }

                //    list.Add(c);
                //}
            }



            return list;

        }

        public void Remove(string condition, int casesolutionid)
        {
            string sql = string.Empty;
            string empty = string.Empty;


            sql = "DELETE FROM tblCaseSolutionCondition WHERE Property_Name='" + condition + "' AND CaseSolution_Id = " + casesolutionid + "";

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
            sql += " VALUES ( " + model.CaseSolution_Id + ", '" + model.Property_Name + "', '" + model.Values + "', " + istatus + " ) END;";
            //TEMP
            sql += "delete from tblCaseSolutionCondition where[Values] = 'null' ";

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



        //public IList<CaseSolutionCondition> GetCaseSolutionConditionModel(int casesolutionid, int customerid, string constString)
        //{
        //    //string constString = "case_WorkingGroup.WorkingGroupGUID";

        //    List<CaseSolutionConditionEntity> clist = this.DataContext.CaseSolutionsConditions.Where(z => z.Property_Name == constString && z.CaseSolution_Id == casesolutionid && z.Status == 1).ToList();
        //    List<CaseSolutionCondition> selected = new List<CaseSolutionCondition>();

        //    int i = -1;
        //    if (clist != null)
        //    {
        //        if (clist.Count > 0)
        //        {
        //            int count = 0;

        //            //Count characters
        //            foreach (CaseSolutionConditionEntity ccount in clist)
        //            {
        //                count = count + ccount.Values.Count(f => f == ',') + 1;
        //            }


        //            string[] wordsFinal = new string[count];
        //            foreach (CaseSolutionConditionEntity c in clist)
        //            {
        //                string[] words = null;
        //                words = c.Values.Split(',');


        //                foreach (string s in words)
        //                {

        //                    i = i + 1;

        //                    wordsFinal[i] = s;

        //                }
        //            }


        //            //Not guids                  
        //            List<string> uids = new List<string>(wordsFinal);

        //            foreach (string s in uids)
        //            {
        //                Guid guidOutput;

        //                bool isValid = Guid.TryParse(s.ToString(), out guidOutput);

        //                if (isValid == false)
        //                {
        //                    CaseSolutionCondition ss = new CaseSolutionCondition
        //                    {

        //                        Customer_Id = customerid,
        //                        Id = 0,
        //                        IsSelected = 1,
        //                        Name = "[" + s + "]",
        //                        StateSecondaryGUID = s.ToString()

        //                    };
        //                    selected.Add(ss);

        //                }
        //            }

        //            if (constString == "case_WorkingGroup.WorkingGroupGUID")
        //            {
        //                var tlist = from xx in this.DataContext.WorkingGroups
        //                            where xx.Customer_Id == customerid && uids.Contains(xx.WorkingGroupGUID.ToString())
        //                            select (xx);

        //                foreach (var c in tlist)
        //                {
        //                    CaseSolutionCondition ss = new CaseSolutionCondition
        //                    {

        //                        Customer_Id = c.Customer_Id,
        //                        Id = c.Id,
        //                        IsSelected = 1,
        //                        Name = c.WorkingGroupName,
        //                        StateSecondaryGUID = c.WorkingGroupGUID.ToString()

        //                    };

        //                    selected.Add(ss);
        //                }
        //            }
        //            else if (constString == "case_StateSecondary.StateSecondaryGUID")
        //            {
        //                var tlist = from xx in this.DataContext.StateSecondaries
        //                            where xx.Customer_Id == customerid && uids.Contains(xx.StateSecondaryGUID.ToString())
        //                            select (xx);

        //                foreach (var c in tlist)
        //                {
        //                    CaseSolutionCondition ss = new CaseSolutionCondition
        //                    {

        //                        Customer_Id = c.Customer_Id,
        //                        Id = c.Id,
        //                        IsSelected = 1,
        //                        Name = c.Name,
        //                        StateSecondaryGUID = c.StateSecondaryGUID.ToString()

        //                    };

        //                    selected.Add(ss);
        //                }

        //            }
        //            else if (constString == "case_Priority.PriorityGUID")
        //            {
        //                var tlist = from xx in this.DataContext.Priorities
        //                            where xx.Customer_Id == customerid && uids.Contains(xx.PriorityGUID.ToString())
        //                            select (xx);

        //                foreach (var c in tlist)
        //                {
        //                    CaseSolutionCondition ss = new CaseSolutionCondition
        //                    {

        //                        Customer_Id = c.Customer_Id,
        //                        Id = c.Id,
        //                        IsSelected = 1,
        //                        Name = c.Name,
        //                        StateSecondaryGUID = c.PriorityGUID.ToString()

        //                    };

        //                    selected.Add(ss);
        //                }

        //            }

        //            else if (constString == "case_Status.StatusGUID")
        //            {
        //                var tlist = from xx in this.DataContext.Statuses
        //                            where xx.Customer_Id == customerid && uids.Contains(xx.StatusGUID.ToString())
        //                            select (xx);

        //                foreach (var c in tlist)
        //                {
        //                    CaseSolutionCondition ss = new CaseSolutionCondition
        //                    {

        //                        Customer_Id = c.Customer_Id,
        //                        Id = c.Id,
        //                        IsSelected = 1,
        //                        Name = c.Name,
        //                        StateSecondaryGUID = c.StatusGUID.ToString()

        //                    };

        //                    selected.Add(ss);
        //                }

        //            }
        //            else if (constString == "user_WorkingGroup.WorkingGroupGUID")
        //            {
        //                var tlist = from xx in this.DataContext.WorkingGroups
        //                            where xx.Customer_Id == customerid && uids.Contains(xx.WorkingGroupGUID.ToString())
        //                            select (xx);

        //                foreach (var c in tlist)
        //                {
        //                    CaseSolutionCondition ss = new CaseSolutionCondition
        //                    {

        //                        Customer_Id = c.Customer_Id,
        //                        Id = c.Id,
        //                        IsSelected = 1,
        //                        Name = c.WorkingGroupName,
        //                        StateSecondaryGUID = c.WorkingGroupGUID.ToString()

        //                    };

        //                    selected.Add(ss);
        //                }
        //            }
        //            else if (constString == "case_ProductArea.ProductAreaGUID")
        //            {
        //                var tlist = from xx in this.DataContext.ProductAreas
        //                            where xx.Customer_Id == customerid && uids.Contains(xx.ProductAreaGUID.ToString())
        //                            select (xx);

        //                foreach (var c in tlist)
        //                {
        //                    CaseSolutionCondition ss = new CaseSolutionCondition
        //                    {

        //                        Customer_Id = c.Customer_Id,
        //                        Id = c.Id,
        //                        IsSelected = 1,
        //                        Name = c.Name,
        //                        StateSecondaryGUID = c.ProductAreaGUID.ToString()

        //                    };

        //                    selected.Add(ss);
        //                }
        //            }
        //        }
        //    }



        //    if (constString == "case_WorkingGroup.WorkingGroupGUID")
        //    {
        //        List<WorkingGroupEntity> stfinaList = this.DataContext.WorkingGroups.Where(z => z.Customer_Id == customerid).ToList();
        //        foreach (WorkingGroupEntity k in stfinaList)
        //        {
        //            bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.WorkingGroupGUID.ToString());
        //            if (has == false)
        //            {
        //                CaseSolutionCondition ss = new CaseSolutionCondition
        //                {
        //                    Customer_Id = k.Customer_Id,
        //                    Id = k.Id,
        //                    IsSelected = 0,
        //                    Name = k.WorkingGroupName,
        //                    StateSecondaryGUID = k.WorkingGroupGUID.ToString()

        //                };


        //                selected.Add(ss);
        //            }
        //        }
        //    }
        //    else if (constString == "case_StateSecondary.StateSecondaryGUID")
        //    {
        //        List<StateSecondary> stfinaList = this.DataContext.StateSecondaries.Where(z => z.Customer_Id == customerid).ToList();
        //        foreach (StateSecondary k in stfinaList)
        //        {
        //            bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.StateSecondaryGUID.ToString());
        //            if (has == false)
        //            {
        //                CaseSolutionCondition ss = new CaseSolutionCondition
        //                {
        //                    Customer_Id = k.Customer_Id,
        //                    Id = k.Id,
        //                    IsSelected = 0,
        //                    Name = k.Name,
        //                    StateSecondaryGUID = k.StateSecondaryGUID.ToString()

        //                };


        //                selected.Add(ss);
        //            }
        //        }
        //    }
        //    else if (constString == "case_Priority.PriorityGUID")
        //    {
        //        List<Priority> stfinaList = this.DataContext.Priorities.Where(z => z.Customer_Id == customerid).ToList();
        //        foreach (Priority k in stfinaList)
        //        {
        //            bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.PriorityGUID.ToString());
        //            if (has == false)
        //            {
        //                CaseSolutionCondition ss = new CaseSolutionCondition
        //                {
        //                    Customer_Id = k.Customer_Id,
        //                    Id = k.Id,
        //                    IsSelected = 0,
        //                    Name = k.Name,
        //                    StateSecondaryGUID = k.PriorityGUID.ToString()

        //                };


        //                selected.Add(ss);
        //            }
        //        }
        //    }
        //    else if (constString == "case_Status.StatusGUID")
        //    {
        //        List<Status> stfinaList = this.DataContext.Statuses.Where(z => z.Customer_Id == customerid).ToList();
        //        foreach (Status k in stfinaList)
        //        {
        //            bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.StatusGUID.ToString());
        //            if (has == false)
        //            {
        //                CaseSolutionCondition ss = new CaseSolutionCondition
        //                {
        //                    Customer_Id = k.Customer_Id,
        //                    Id = k.Id,
        //                    IsSelected = 0,
        //                    Name = k.Name,
        //                    StateSecondaryGUID = k.StatusGUID.ToString()

        //                };


        //                selected.Add(ss);
        //            }
        //        }
        //    }
        //    else if (constString == "user_WorkingGroup.WorkingGroupGUID")
        //    {
        //        List<WorkingGroupEntity> stfinaList = this.DataContext.WorkingGroups.Where(z => z.Customer_Id == customerid).ToList();
        //        foreach (WorkingGroupEntity k in stfinaList)
        //        {
        //            bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.WorkingGroupGUID.ToString());
        //            if (has == false)
        //            {
        //                CaseSolutionCondition ss = new CaseSolutionCondition
        //                {
        //                    Customer_Id = k.Customer_Id,
        //                    Id = k.Id,
        //                    IsSelected = 0,
        //                    Name = k.WorkingGroupName,
        //                    StateSecondaryGUID = k.WorkingGroupGUID.ToString()

        //                };


        //                selected.Add(ss);
        //            }
        //        }
        //    }
        //    else if (constString == "case_ProductArea.ProductAreaGUID")
        //    {
        //        List<ProductArea> stfinaList = this.DataContext.ProductAreas.Where(z => z.Customer_Id == customerid).ToList();
        //        foreach (ProductArea k in stfinaList)
        //        {
        //            bool has = selected.Any(cus => cus.StateSecondaryGUID.ToString() == k.ProductAreaGUID.ToString());
        //            if (has == false)
        //            {
        //                CaseSolutionCondition ss = new CaseSolutionCondition
        //                {
        //                    Customer_Id = k.Customer_Id,
        //                    Id = k.Id,
        //                    IsSelected = 0,
        //                    Name = k.Name,
        //                    StateSecondaryGUID = k.ProductAreaGUID.ToString()

        //                };


        //                selected.Add(ss);
        //            }
        //        }
        //    }
        //    var result = selected.OrderBy(x => x.Id).ThenBy(x => x.Name).ToList();

        //    return result;
        //}



    }
}
