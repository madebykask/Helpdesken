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
        IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid, bool workGroupRestriction);
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




        public IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customersIdAll, int? count, bool forStartPage, int userid, bool workGroupRestriction)
        {


            var sql = string.Empty;
            DataTable dt = null;


            var custidstring = string.Empty;

            foreach (var t in customersIdAll)
            {
                if (custidstring == string.Empty)
                {
                    custidstring = t.ToString();
                }
                else
                {
                    custidstring = custidstring  + ", " + t;
                }
            }

            var startPage = 0;
            if (forStartPage == true)
            {
                startPage = 1;
            }


            //var links =
            //    from links1 in this.DataContext.Links
            //    join casesolution in this.DataContext.CaseSolutions on links1.CaseSolution_Id equals casesolution.Id into linkssolution
            //    join documents in this.DataContext.Documents on links1.Document_Id equals documents.Id into linksdocument
            //    join linkgroup in this.DataContext.LinkGroups on links1.LinkGroup_Id equals linkgroup.Id into linkslinkgroup
            //    join customer in this.DataContext.Customers on links1.Customer_Id equals customer.Id into linkscustomer
            //    where customersIdAll.Contains(links1.Customer_Id.Value)
            //    select new { links1.Id, links1.CaseSolution_Id, links1.CaseSolution, links1.Us, links1.Wg, links1.Customer_Id, links1.Document_Id, links1.LinkGroup_Id, links1.NewWindowHeight, links1.NewWindowWidth, links1.OpenInNewWindow, links1.SortOrder, links1.URLAddress, links1.URLName, links1.ShowOnStartPage, links1.Document, links1.LinkGroup, links1.Customer };




            //var userGroups = this.DataContext.UserWorkingGroups.Where(z => z.User_Id == userid).Select(u => u.WorkingGroup_Id);

            //links = links.Where(x => !x.Us.Any() || x.Us.Any(u => u.Id == userid));

            //links = links.Where(x => !x.Wg.Any() || x.Wg.Any(g => userGroups.Contains(g.Id)));



            //List<LinkOverview> llist1 = new List<LinkOverview>();
            //foreach (var r in links)
            //{



            //    LinkOverview ltemp = new LinkOverview();
            //    if (r.CaseSolution_Id != null)
            //    {
            //        ltemp.CaseSolutionId = r.CaseSolution_Id.ToString() != null ? Convert.ToInt32(r.CaseSolution_Id.ToString()) : 0;
            //    }
            //    else
            //    {
            //        ltemp.CaseSolutionId = 0;
            //    }
            //    if (r.CaseSolution != null)
            //    {
            //        ltemp.CaseSolutionName = r.CaseSolution.Name.ToString() != null ? Convert.ToString(r.CaseSolution.Name.ToString()) : null;
            //    }
            //    else
            //    {
            //        ltemp.CaseSolutionName = null;
            //    }
            //    if (r.Customer_Id != null)
            //    {
            //        ltemp.CustomerId = r.Customer_Id.ToString() != null ? Convert.ToInt32(r.Customer_Id.ToString()) : 0;
            //    }
            //    else
            //    {
            //        ltemp.CustomerId = 0;
            //    }
            //    if (r.Customer != null)
            //    {
            //        ltemp.CustomerName = r.Customer.Name.ToString() != null ? Convert.ToString(r.Customer.Name.ToString()) : null;
            //    }
            //    else
            //    {
            //        ltemp.CustomerName = null
            //            ;
            //    }
            //    if (r.Document_Id != null)
            //    {
            //        ltemp.DocumentId = r.Document_Id.ToString() != null ? Convert.ToInt32(r.Document_Id.ToString()) : 0;
            //    }
            //    else
            //    {
            //        ltemp.DocumentId = 0;
            //    }
            //    if (r.Document != null)
            //    {
            //        ltemp.DocumentName = r.Document.Name.ToString() != null ? Convert.ToString(r.Document.Name.ToString()) : null;
            //    }
            //    else
            //    {
            //        ltemp.DocumentName = null;
            //    }
            //    if (r.LinkGroup_Id != null)
            //    {
            //        ltemp.LinkGroupId = r.LinkGroup_Id.ToString() != null ? Convert.ToInt32(r.LinkGroup_Id.ToString()) : 0;
            //    }
            //    else
            //    {
            //        ltemp.LinkGroupId = 0;
            //    }
            //    if (r.LinkGroup != null)
            //    {
            //        ltemp.LinkGroupName = r.LinkGroup.LinkGroupName.ToString() != null ? Convert.ToString(r.LinkGroup.LinkGroupName.ToString()) : null;
            //    }
            //    else
            //    {
            //        ltemp.LinkGroupName = null;
            //    }

            //    if (r.NewWindowHeight != null)
            //    {
            //        ltemp.NewWindowHeight = r.NewWindowHeight.ToString() != null ? Convert.ToInt32(r.NewWindowHeight.ToString()) : 0;
            //    }
            //    else
            //    {
            //        ltemp.NewWindowHeight = 0;
            //    }

            //    if (r.NewWindowWidth != null)
            //    {
            //        ltemp.NewWindowWidth = r.NewWindowWidth.ToString() != null ? Convert.ToInt32(r.NewWindowWidth.ToString()) : 0;
            //    }
            //    else
            //    {
            //        ltemp.NewWindowWidth = 0;
            //    }

            //    if (r.OpenInNewWindow != null)
            //    {
            //        ltemp.OpenInNewWindow = Convert.ToBoolean(Convert.ToInt32(r.OpenInNewWindow.ToString()));
            //    }
            //    else
            //    {
            //        ltemp.OpenInNewWindow = false;
            //    }

            //    if (r.ShowOnStartPage != null)
            //    {
            //        ltemp.ShowOnStartPage = Convert.ToBoolean(Convert.ToInt32(r.ShowOnStartPage.ToString()));
            //    }
            //    else
            //    {
            //        ltemp.ShowOnStartPage = false;
            //    }

            //    if (r.SortOrder != null)
            //    {
            //        ltemp.SortOrder = r.SortOrder.ToString() != null ? Convert.ToString(r.SortOrder.ToString()) : string.Empty;
            //    }
            //    else
            //    {
            //        ltemp.SortOrder = string.Empty;
            //    }

            //    if (r.URLAddress != null)
            //    {
            //        ltemp.UrlAddress = r.URLAddress.ToString() != null ? Convert.ToString(r.URLAddress.ToString()) : string.Empty;
            //    }
            //    else
            //    {
            //        ltemp.UrlAddress = string.Empty;
            //    }

            //    if (r.URLName != null)
            //    {
            //        ltemp.UrlName = r.URLName.ToString() != null ? Convert.ToString(r.URLName.ToString()) : string.Empty;
            //    }
            //    else
            //    {
            //        ltemp.UrlName = string.Empty;
            //    }
            //    llist1.Add(ltemp);
            //}


            sql = "SELECT ";
            sql += "ISNULL([LinksOverview].[Id], 0) AS [Id], ";
            sql += "ISNULL([LinksOverview].[Customer_Id], 0) AS [Customer_Id], ";
            sql += "ISNULL([LinksOverview].[Document_Id], 0) AS [Document_Id], ";
            sql += "ISNULL([LinksOverview].[OpenInNewWindow], 0) AS [OpenInNewWindow], ";
            sql += "ISNULL([LinksOverview].[NewWindowHeight], 0) AS [NewWindowHeight], ";
            sql += "ISNULL([LinksOverview].[NewWindowWidth], 0) AS [NewWindowWidth], ";
            sql += "ISNULL([LinksOverview].[ShowOnStartPage], 0) AS [ShowOnStartPage], ";
            sql += "ISNULL([LinksOverview].[LinkGroup_Id], 0) AS [LinkGroup_Id], ";
            sql += "ISNULL([LinksOverview].[URLAddress], '') AS [URLAddress], ";
            sql += "ISNULL([LinksOverview].[URLName], '') AS [URLName], ";
            sql += "ISNULL([LinksOverview].[SortOrder], '') AS [SortOrder], ";
            sql += "ISNULL([LinksOverview].[ChangedDate], '') AS [ChangedDate], ";
            sql += "ISNULL([LinksOverview].[CreatedDate], '') AS [CreatedDate], ";
            sql += "ISNULL([LinksOverview].[CaseFilterFavorite_Id], '') AS [CaseFilterFavorite_Id], ";
            sql += "ISNULL([LinksOverview].[CaseSolution_Id], 0) AS [CaseSolution_Id], ";
            sql += " ISNULL((SELECT CaseSolutionName FROM dbo.tblCaseSolution WHERE(Id = [LinksOverview].[CaseSolution_Id])), '') AS CaseSolutionName, ";
            sql += " ISNULL((SELECT Name FROM dbo.tblCaseFilterFavorite WHERE(Id = [LinksOverview].[CaseFilterFavorite_Id])), '') AS CaseFilterFavoriteName, ";
            sql += " ISNULL((SELECT Name FROM dbo.tblCustomer WHERE(Id = [LinksOverview].[Customer_Id])), '') AS CustomerName, ";
            sql += " (SELECT DocumentName FROM dbo.tblDocument WHERE(Id = [LinksOverview].[Document_Id])) AS DocumentName,  ";
            sql += " (SELECT LinkGroup FROM dbo.tblLinkGroup WHERE(Id = [LinksOverview].[LinkGroup_Id])) AS LinkGroupName ";
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
            sql += "[Links].[CaseFilterFavorite_Id] AS [CaseFilterFavorite_Id], ";
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
            sql += $"WHERE([Links].[Id] = [LinkWG1].[Link_Id]) AND([LinkWG1].[WorkingGroup_Id] in (select WorkingGroup_Id from tblUserWorkingGroup where User_Id =  {userid}";
            if (workGroupRestriction) sql += $" AND UserRole = {(int)WorkingGroupUserPermission.ADMINSTRATOR}";
            sql += ")) )) ";
            sql += ") ";
            sql += $"AND([Links].[Customer_Id] IS NOT NULL) AND([Links].[Customer_Id] IN({custidstring})) AND([Links].[Customer_Id] IS NOT NULL) AND([Links].[ShowOnStartPage] = {startPage}) ";
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


                ltemp.CaseSolutionId = row["CaseSolution_Id"] != null ? Convert.ToInt32(row["CaseSolution_Id"].ToString()) : 0;
                ltemp.CaseSolutionName = row["CaseSolutionName"] != null ? Convert.ToString(row["CaseSolutionName"].ToString()) : string.Empty;
                ltemp.CaseFilterFavoriteId = row["CaseFilterFavorite_Id"] != null ? Convert.ToInt32(row["CaseFilterFavorite_Id"].ToString()) : 0;
                ltemp.CaseFilterFavoriteName = row["CaseFilterFavoriteName"] != null ? Convert.ToString(row["CaseFilterFavoriteName"].ToString()) : string.Empty;
                ltemp.CustomerId = row["Customer_Id"] != null ? Convert.ToInt32(row["Customer_Id"].ToString()) : 0;
                ltemp.CustomerName = row["CustomerName"] != null ? Convert.ToString(row["CustomerName"].ToString()) : string.Empty;
                ltemp.DocumentId = row["Document_Id"] != null ? Convert.ToInt32(row["Document_Id"].ToString()) : 0;
                ltemp.DocumentName = row["DocumentName"] != null ? Convert.ToString(row["DocumentName"].ToString()) : string.Empty;
                ltemp.LinkGroupId = row["LinkGroup_Id"] != null ? Convert.ToInt32(row["LinkGroup_Id"].ToString()) : 0;
                ltemp.LinkGroupName = row["LinkGroupName"] != null ? Convert.ToString(row["LinkGroupName"].ToString()) : string.Empty;
                ltemp.NewWindowHeight = row["NewWindowHeight"] != null ? Convert.ToInt32(row["NewWindowHeight"].ToString()) : 0;
                ltemp.NewWindowWidth = row["NewWindowWidth"] != null ? Convert.ToInt32(row["NewWindowWidth"].ToString()) : 0;
                if (row["OpenInNewWindow"] != null)
                {
                    ltemp.OpenInNewWindow = Convert.ToBoolean(Convert.ToInt32(row["OpenInNewWindow"].ToString()));
                }
                if (row["ShowOnStartPage"] != null)
                {
                    ltemp.ShowOnStartPage = Convert.ToBoolean(Convert.ToInt32(row["ShowOnStartPage"].ToString()));
                }

                ltemp.SortOrder = row["SortOrder"] != null ? Convert.ToString(row["SortOrder"].ToString()) : string.Empty;
                ltemp.UrlAddress = row["UrlAddress"] != null ? Convert.ToString(row["UrlAddress"].ToString()) : string.Empty;
                ltemp.UrlName = row["URLName"] != null ? Convert.ToString(row["URLName"].ToString()) : string.Empty;

                llist.Add(ltemp);
            }


            return llist;
            // return llist1;
        }
    }
}
