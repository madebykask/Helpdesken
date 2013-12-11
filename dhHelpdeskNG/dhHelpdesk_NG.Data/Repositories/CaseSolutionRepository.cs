using System.Configuration;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.Utils;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region CASESOLUTION

    public interface ICaseSolutionRepository : IRepository<CaseSolution>
    {
        //int GetAntal(int customerid, int userid);
    }

    public class CaseSolutionRepository : RepositoryBase<CaseSolution>, ICaseSolutionRepository
    {
        public CaseSolutionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        //public int GetAntal(int customerid, int userid)
        //{
        //    string sSQL = "";
        //    int antal = 0;

        //    sSQL = "SELECT Count(*) AS Antal ";
        //    sSQL += "FROM tblCaseSolution ";
        //    sSQL += "WHERE tblCaseSolution.Customer_Id=" + customerid;
        //    sSQL += " AND (WorkingGroup_Id IN (SELECT workinggroup_id FROM tblUserWorkingGroup where user_id=" + userid + ") OR WorkingGroup_Id is null)";

        //    string con = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;

        //    using (var connection = new SqlConnection(con))
        //    {
        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = sSQL;
        //            connection.Open();

        //            using (var dr = command.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    antal = dr.SafeGetInteger("Antal");
        //                    break;

        //                }

        //            }

        //        }

        //    }
        //    return antal;
        //}
    }

    #endregion

    #region CASESOLUTIONCATEGORY

    public interface ICaseSolutionCategoryRepository : IRepository<CaseSolutionCategory>
    {
    }

    public class CaseSolutionCategoryRepository : RepositoryBase<CaseSolutionCategory>, ICaseSolutionCategoryRepository
    {
        public CaseSolutionCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CASESOLUTIONSCHEDULE

    public interface ICaseSolutionScheduleRepository : IRepository<CaseSolutionSchedule>
    {
    }

    public class CaseSolutionScheduleRepository : RepositoryBase<CaseSolutionSchedule>, ICaseSolutionScheduleRepository
    {
        public CaseSolutionScheduleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
