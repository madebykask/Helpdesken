// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ILinkRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid);
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

        public IEnumerable<LinkOverview> GetLinkOverviewsToStartPage(int[] customers, int? count, bool forStartPage, int userid)
        {


            string sql = string.Empty;
            DataTable dt = null;

            int custid = customers[0];

            int StartPage = 0;
            if (forStartPage == true)
            {
                StartPage = 1;
            }

            //var links = this.DataContext.Links.Where(z => z.Customer_Id == custid && z.ShowOnStartPage == StartPage);

            //links = links.Where(x => !x.Us.Any() || x.Us.Any(u => u.Id == userid));


            //var userGroups = this.DataContext.UserWorkingGroups.Where(u => u.User_Id == userid).Select(u => u.WorkingGroup_Id);

            //links = links.Where(x => !x.Wg.Any() || x.Wg.Any(g => userGroups.Contains(g.Id)));



            //List<LinkOverview> llist = new List<LinkOverview>();
            //foreach (var r in links)
            //{

            //    LinkOverview ltemp = new LinkOverview();
            //    ltemp.CaseSolutionId = r.CaseSolution_Id.ToString() != null ? Convert.ToInt32(r.CaseSolution_Id.ToString()) : 0;
            //    ltemp.CaseSolutionName = r.CaseSolution.Name.ToString() != null ? Convert.ToString(r.CaseSolution.Name.ToString()) : string.Empty;
            //    ltemp.CustomerId = r.Customer_Id.ToString() != null ? Convert.ToInt32(r.Customer_Id.ToString()) : 0;
            //    ltemp.CustomerName = r.Customer.CustomerID.ToString() != null ? Convert.ToString(r.Customer.CustomerID.ToString()) : string.Empty;
            //    ltemp.DocumentId = r.Document_Id.ToString() != null ? Convert.ToInt32(r.Document_Id.ToString()) : 0;
            //    ltemp.DocumentName = r.Document.Name.ToString() != null ? Convert.ToString(r.Document.Name.ToString()) : string.Empty;
            //    ltemp.LinkGroupId = r.LinkGroup_Id.ToString() != null ? Convert.ToInt32(r.LinkGroup_Id.ToString()) : 0;
            //    ltemp.LinkGroupName = r.LinkGroup.LinkGroupName.ToString() != null ? Convert.ToString(r.LinkGroup.LinkGroupName.ToString()) : string.Empty;
            //    ltemp.NewWindowHeight = r.NewWindowHeight.ToString() != null ? Convert.ToInt32(r.NewWindowHeight.ToString()) : 0;
            //    ltemp.NewWindowWidth = r.NewWindowWidth.ToString() != null ? Convert.ToInt32(r.NewWindowWidth.ToString()) : 0;
            //    if (r.OpenInNewWindow != null)
            //    {
            //        ltemp.OpenInNewWindow = Convert.ToBoolean(Convert.ToInt32(r.OpenInNewWindow.ToString()));
            //    }
            //    if (r.ShowOnStartPage != null)
            //    {
            //        ltemp.ShowOnStartPage = Convert.ToBoolean(Convert.ToInt32(r.ShowOnStartPage.ToString()));
            //    }

            //    ltemp.SortOrder = r.SortOrder.ToString() != null ? Convert.ToString(r.SortOrder.ToString()) : string.Empty;
            //    ltemp.UrlAddress = r.URLAddress.ToString() != null ? Convert.ToString(r.URLAddress.ToString()) : string.Empty;
            //    ltemp.UrlName = r.URLName.ToString() != null ? Convert.ToString(r.URLName.ToString()) : string.Empty;

            //    llist.Add(ltemp);
            //}


            sql = "SELECT TOP (100) ";
            sql += "ISNULL([Project3].[Id], 0) AS [Id], ";
            sql += "ISNULL([Project3].[Customer_Id], 0) AS [Customer_Id], ";
            sql += "ISNULL([Project3].[Document_Id], 0) AS [Document_Id], ";
            sql += "ISNULL([Project3].[OpenInNewWindow], 0) AS [OpenInNewWindow], ";
            sql += "ISNULL([Project3].[NewWindowHeight], 0) AS [NewWindowHeight], ";
            sql += "ISNULL([Project3].[NewWindowWidth], 0) AS [NewWindowWidth], ";
            sql += "ISNULL([Project3].[ShowOnStartPage], 0) AS [ShowOnStartPage], ";
            sql += "ISNULL([Project3].[LinkGroup_Id], 0) AS [LinkGroup_Id], ";
            sql += "ISNULL([Project3].[URLAddress], '') AS [URLAddress], ";
            sql += "ISNULL([Project3].[URLName], '') AS [URLName], ";
            sql += "ISNULL([Project3].[SortOrder], '') AS [SortOrder], ";
            sql += "ISNULL([Project3].[ChangedDate], '') AS [ChangedDate], ";
            sql += "ISNULL([Project3].[CreatedDate], '') AS [CreatedDate], ";
            sql += "ISNULL([Project3].[CaseSolution_Id], 0) AS [CaseSolution_Id], ";
            sql += " ISNULL((SELECT CaseSolutionName FROM dbo.tblCaseSolution WHERE(Id = [Project3].[CaseSolution_Id])), '') AS CaseSolutionName, ";
            sql += " ISNULL((SELECT Name FROM dbo.tblCustomer WHERE(Id = [Project3].[Customer_Id])), '') AS CustomerName, ";
            sql += " (SELECT DocumentName FROM dbo.tblDocument WHERE(Id = [Project3].[Document_Id])) AS DocumentName,  ";
            sql += " (SELECT LinkGroup FROM dbo.tblLinkGroup WHERE(Id = [Project3].[LinkGroup_Id])) AS LinkGroupName ";
            sql += "FROM(SELECT ";
            sql += "[Extent1].[Id] AS [Id], ";
            sql += "[Extent1].[Customer_Id] AS [Customer_Id], ";
            sql += "[Extent1].[Document_Id] AS [Document_Id], ";
            sql += "[Extent1].[OpenInNewWindow] AS [OpenInNewWindow], ";
            sql += "[Extent1].[NewWindowHeight] AS [NewWindowHeight], ";
            sql += "[Extent1].[NewWindowWidth] AS [NewWindowWidth], ";
            sql += "[Extent1].[ShowOnStartPage] AS [ShowOnStartPage], ";
            sql += "[Extent1].[LinkGroup_Id] AS [LinkGroup_Id], ";
            sql += "[Extent1].[URLAddress] AS [URLAddress], ";
            sql += "[Extent1].[URLName] AS [URLName], ";
            sql += "[Extent1].[SortOrder] AS [SortOrder], ";
            sql += "[Extent1].[ChangedDate] AS [ChangedDate], ";
            sql += "[Extent1].[CreatedDate] AS [CreatedDate], ";
            sql += "[Extent1].[CaseSolution_Id] AS [CaseSolution_Id], ";
            sql += "[Extent2].[Name] AS [Name] ";
            sql += "FROM[dbo].[tbllink] AS [Extent1] ";
            sql += "LEFT OUTER JOIN[dbo].[tblcustomer] AS [Extent2] ON[Extent1].[Customer_Id] = [Extent2].[Id] ";
            sql += "WHERE(   ";
            sql += "(NOT EXISTS(SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblUsers] AS [Extent3] ";
            sql += "WHERE[Extent1].[Id] = [Extent3].[Link_Id] ";
            sql += "Union ";
            sql += " SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblWorkingGroup] AS [Extent31] ";
            sql += "WHERE[Extent1].[Id] = [Extent31].[Link_Id] ";
            sql += ") ";
            sql += ") ";
            sql += "OR ";
            sql += "(EXISTS(SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblUsers] AS [Extent4] ";
            sql += "WHERE([Extent1].[Id] = [Extent4].[Link_Id]) AND([Extent4].[User_Id] = " + userid + ") ";
            sql += ")) ";
            sql += "OR ";
            sql += "(EXISTS(SELECT ";
            sql += "1 AS [C1] ";
            sql += "FROM[dbo].[tblLink_tblWorkingGroup] AS [Extent41] ";
            sql += "WHERE([Extent1].[Id] = [Extent41].[Link_Id]) AND([Extent41].[WorkingGroup_Id] in (select WorkingGroup_Id from tblUserWorkingGroup where User_Id =  " + userid + ")) ";
            sql += ")) ";
            sql += ") ";
            sql += "AND([Extent1].[Customer_Id] IS NOT NULL) AND([Extent1].[Customer_Id] IN(" + custid + ")) AND([Extent1].[Customer_Id] IS NOT NULL) AND([Extent1].[ShowOnStartPage] = " + StartPage + ") ";
            sql += ")  AS [Project3] ";
            sql += "ORDER BY[Project3].[Name] ";
            sql += "ASC, [Project3].[SortOrder] ";
            sql += "ASC ";

            string ConnectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;


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


            List<LinkOverview> llist = new List<LinkOverview>();
            foreach (DataRow row in dt.Rows)
            {
                LinkOverview ltemp = new LinkOverview();


                ltemp.CaseSolutionId = row["CaseSolution_Id"].ToString() != null ? Convert.ToInt32(row["CaseSolution_Id"].ToString()) : 0;
                ltemp.CaseSolutionName = row["CaseSolutionName"].ToString() != null ? Convert.ToString(row["CaseSolutionName"].ToString()) : string.Empty;
                ltemp.CustomerId = row["Customer_Id"].ToString() != null ? Convert.ToInt32(row["Customer_Id"].ToString()) : 0;
                ltemp.CustomerName = row["CustomerName"].ToString() != null ? Convert.ToString(row["CustomerName"].ToString()) : string.Empty;
                ltemp.DocumentId = row["Document_Id"].ToString() != null ? Convert.ToInt32(row["Document_Id"].ToString()) : 0;
                ltemp.DocumentName = row["DocumentName"].ToString() != null ? Convert.ToString(row["DocumentName"].ToString()) : string.Empty;
                ltemp.LinkGroupId = row["LinkGroup_Id"].ToString() != null ? Convert.ToInt32(row["LinkGroup_Id"].ToString()) : 0;
                ltemp.LinkGroupName = row["LinkGroupName"].ToString() != null ? Convert.ToString(row["LinkGroupName"].ToString()) : string.Empty;
                ltemp.NewWindowHeight = row["NewWindowHeight"].ToString() != null ? Convert.ToInt32(row["NewWindowHeight"].ToString()) : 0;
                ltemp.NewWindowWidth = row["NewWindowWidth"].ToString() != null ? Convert.ToInt32(row["NewWindowWidth"].ToString()) : 0;
                if (row["OpenInNewWindow"] != null)
                {
                    ltemp.OpenInNewWindow = Convert.ToBoolean(Convert.ToInt32(row["OpenInNewWindow"].ToString()));
                }
                if (row["ShowOnStartPage"] != null)
                {
                    ltemp.ShowOnStartPage = Convert.ToBoolean(Convert.ToInt32(row["ShowOnStartPage"].ToString()));
                }

                ltemp.SortOrder = row["SortOrder"].ToString() != null ? Convert.ToString(row["SortOrder"].ToString()) : string.Empty;
                ltemp.UrlAddress = row["UrlAddress"].ToString() != null ? Convert.ToString(row["UrlAddress"].ToString()) : string.Empty;
                ltemp.UrlName = row["URLName"].ToString() != null ? Convert.ToString(row["URLName"].ToString()) : string.Empty;

                llist.Add(ltemp);
            }


            return llist;
            // return links;
        }
    }
}
