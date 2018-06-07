// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ILinkRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.BusinessData.Enums.Users;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.BusinessData.Models.Link.Output;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;

    /// <summary>
    /// The LinkRepository interface.
    /// </summary>
    public interface ILinkRepository : Infrastructure.IRepository<Link>
    {
        IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid, int[] customerIdRestrictions);
    }

    /// <summary>
    /// The link repository.
    /// </summary>
    public class LinkRepository : RepositoryBase<Link>, ILinkRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public LinkRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        
        public IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customersIdAll, int? count, bool forStartPage, int userid, int[] customerIdRestrictions)
        {
            //TODO: Canbe optimized
            DataTable dt;

            var custidstring = string.Join(",", customersIdAll);

            var startPage = forStartPage ? 1 : 0;

            var sql = "SELECT ";
            //sql += "ISNULL([LinksOverview].[Id], 0) AS [Id], ";
            sql += "[LinksOverview].[Customer_Id] AS [Customer_Id], ";
            sql += "[LinksOverview].[Document_Id] AS [Document_Id], ";
            sql += "[LinksOverview].[OpenInNewWindow] AS [OpenInNewWindow], ";
            sql += "[LinksOverview].[NewWindowHeight] AS [NewWindowHeight], ";
            sql += "[LinksOverview].[NewWindowWidth] AS [NewWindowWidth], ";
            sql += "[LinksOverview].[ShowOnStartPage] AS [ShowOnStartPage], ";
            sql += "[LinksOverview].[LinkGroup_Id] AS [LinkGroup_Id], ";
            sql += "[LinksOverview].[URLAddress] AS [URLAddress], ";
            sql += "[LinksOverview].[URLName] AS [URLName], ";
            sql += "[LinksOverview].[SortOrder] AS [SortOrder], ";
            //sql += "ISNULL([LinksOverview].[ChangedDate], '') AS [ChangedDate], ";
            //sql += "ISNULL([LinksOverview].[CreatedDate], '') AS [CreatedDate], ";
            sql += "[LinksOverview].[CaseSolution_Id] AS [CaseSolution_Id], ";
            sql += " (SELECT CaseSolutionName FROM dbo.tblCaseSolution WHERE Id = [LinksOverview].[CaseSolution_Id]) AS CaseSolutionName, ";            
            sql += " (SELECT Name FROM dbo.tblCustomer WHERE Id = [LinksOverview].[Customer_Id]) AS CustomerName, ";
            sql += " (SELECT DocumentName FROM dbo.tblDocument WHERE Id = [LinksOverview].[Document_Id]) AS DocumentName,  ";
            sql += " (SELECT LinkGroup FROM dbo.tblLinkGroup WHERE Id = [LinksOverview].[LinkGroup_Id]) AS LinkGroupName ";
            sql += "FROM(SELECT ";
            sql += "[Links].[Id] AS [Id], ";
            sql += "[Links].[Customer_Id] AS [Customer_Id], ";
            sql += "[Links].[Document_Id] AS [Document_Id], ";
            sql += "[Links].[OpenInNewWindow] AS [OpenInNewWindow], ";
            sql += "[Links].[NewWindowHeight] AS [NewWindowHeight], ";
            sql += "[Links].[NewWindowWidth] AS [NewWindowWidth], ";
            sql += "[Links].[ShowOnStartPage] AS [ShowOnStartPage], ";
            sql += "[Links].[LinkGroup_Id] AS [LinkGroup_Id], ";
            sql += "[Links].[URLAddress] AS [URLAddress], ";
            sql += "[Links].[URLName] AS [URLName], ";
            sql += "[Links].[SortOrder] AS [SortOrder], ";
            sql += "[Links].[ChangedDate] AS [ChangedDate], ";
            sql += "[Links].[CreatedDate] AS [CreatedDate], ";
            sql += "[Links].[CaseSolution_Id] AS [CaseSolution_Id], ";
            sql += "[Customer].[Name] AS [Name] ";
            sql += "FROM[dbo].[tbllink] AS [Links] ";
            sql += "LEFT OUTER JOIN[dbo].[tblcustomer] AS [Customer] ON[Links].[Customer_Id] = [Customer].[Id] ";
            sql += "WHERE(   ";
            sql += "(NOT EXISTS(SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblUsers] AS [Users] ";
            sql += "WHERE[Links].[Id] = [Users].[Link_Id] ";
            sql += "Union ";
            sql += " SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblWorkingGroup] AS [LinkWG] ";
            sql += "WHERE[Links].[Id] = [LinkWG].[Link_Id] ";
            sql += ") ";
            sql += ") ";
            sql += "OR ";
            sql += "(EXISTS(SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblUsers] AS [LinkUsers] ";
            sql += $"WHERE([Links].[Id] = [LinkUsers].[Link_Id]) AND([LinkUsers].[User_Id] = {userid}) ";
            sql += ")) ";
            sql += "OR ";
            sql += "(EXISTS(SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblWorkingGroup] AS [LinkWG1] ";
            sql += "WHERE([Links].[Id] = [LinkWG1].[Link_Id]) AND([LinkWG1].[WorkingGroup_Id] in (select WorkingGroup_Id from [dbo].[tblUserWorkingGroup] as [LinkUWG] "
                + "JOIN [dbo].[tblWorkingGroup] AS [LinkWG2] ON [LinkWG2].Id = [LinkUWG].WorkingGroup_Id "
                + $"where User_Id = {userid}";
            if(customerIdRestrictions.Any())
            {
                sql += $" AND (UserRole = {WorkingGroupUserPermission.ADMINSTRATOR} AND [LinkWG2].Customer_Id in ({string.Join(",", customerIdRestrictions)})) ";
                var customersNoRestriction = customersIdAll.Except(customerIdRestrictions).ToArray();
                if (customersNoRestriction.Any())
                {
                    sql += $"OR [LinkWG2].Customer_Id in ({string.Join(",", customersNoRestriction)})";
                }
            }
            sql += ")) )) ";
            sql += ") ";
            sql += $"AND([Links].[Customer_Id] IN({custidstring})) AND([Links].[Customer_Id] IS NOT NULL) AND([Links].[ShowOnStartPage] = {startPage}) ";
            sql += ")  AS [LinksOverview] ";
            sql += "ORDER BY[LinksOverview].[Name] ";
            sql += "ASC, [LinksOverview].[SortOrder] ";
            sql += "ASC ";

            var connectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;


            using (var connection = new SqlConnection(connectionString))
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


            var llist = new List<LinkOverview>();
            foreach (DataRow row in dt.Rows)
            {
                var ltemp = new LinkOverview();


                ltemp.CaseSolutionId = !row.IsNull("CaseSolution_Id") ? Convert.ToInt32(row["CaseSolution_Id"].ToString()) : 0;
                ltemp.CaseSolutionName = !row.IsNull("CaseSolutionName") ? Convert.ToString(row["CaseSolutionName"].ToString()) : string.Empty;
                ltemp.CustomerId = !row.IsNull("Customer_Id") ? Convert.ToInt32(row["Customer_Id"].ToString()) : 0;
                ltemp.CustomerName = !row.IsNull("CustomerName") ? Convert.ToString(row["CustomerName"].ToString()) : string.Empty;
                ltemp.DocumentId = !row.IsNull("Document_Id") ? Convert.ToInt32(row["Document_Id"].ToString()) : 0;
                ltemp.DocumentName = !row.IsNull("DocumentName") ? Convert.ToString(row["DocumentName"].ToString()) : string.Empty;
                ltemp.LinkGroupId = !row.IsNull("LinkGroup_Id") ? Convert.ToInt32(row["LinkGroup_Id"].ToString()) : 0;
                ltemp.LinkGroupName = !row.IsNull("LinkGroupName") ? Convert.ToString(row["LinkGroupName"].ToString()) : string.Empty;
                ltemp.NewWindowHeight = !row.IsNull("NewWindowHeight") ? Convert.ToInt32(row["NewWindowHeight"].ToString()) : 0;
                ltemp.NewWindowWidth = !row.IsNull("NewWindowWidth") ? Convert.ToInt32(row["NewWindowWidth"].ToString()) : 0;
                ltemp.OpenInNewWindow = !row.IsNull("OpenInNewWindow") && Convert.ToBoolean(Convert.ToInt32(row["OpenInNewWindow"].ToString()));
                ltemp.ShowOnStartPage = !row.IsNull("ShowOnStartPage") && Convert.ToBoolean(Convert.ToInt32(row["ShowOnStartPage"].ToString()));

                ltemp.SortOrder = !row.IsNull("SortOrder") ? Convert.ToString(row["SortOrder"].ToString()) : string.Empty;
                ltemp.UrlAddress = !row.IsNull("URLAddress") ? Convert.ToString(row["URLAddress"].ToString()) : string.Empty;
                ltemp.UrlName = !row.IsNull("URLName") ? Convert.ToString(row["URLName"].ToString()) : string.Empty;

                llist.Add(ltemp);
            }


            return llist;
            // return llist1;
        }
    }
}
