using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface IStateSecondaryRepository : IRepository<StateSecondary>
    {
        //IList<StateSecondary> GetStateSecondariesAvailable(int customerId, string[] reg);
        //IList<StateSecondary> GetStateSecondariesSelected(int customerId, string[] reg);
    }

    public class StateSecondaryRepository : RepositoryBase<StateSecondary>, IStateSecondaryRepository
    {
        public StateSecondaryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        //public IList<StateSecondary> GetStateSecondariesSelected(int customerId, string[] reg)
        //{
        //    List<StateSecondary> rlist = new List<StateSecondary>();

        //    var query = (from cat in this.DataContext.Set<StateSecondary>()
        //                 where cat.Customer_Id == customerId
        //                 orderby cat.Name
        //                 select cat);

        //    bool add = false;
        //    if (reg != null)
        //    {
        //        foreach (StateSecondary fg in query) // rr)
        //        {
        //            add = false;
        //            foreach (string i in reg)
        //            {
        //                if (i == fg.Id.ToString())
        //                {
        //                    add = true;
        //                }
        //            }

        //            if (add == true)
        //            {
        //                rlist.Add(fg);
        //            }
        //        }
        //    }

        //    return rlist;
        //}

        //public IList<StateSecondary> GetStateSecondariesAvailable(int customerId, string[] reg)
        //{
        //    List<StateSecondary> rlist = new List<StateSecondary>();

        //    var query = (from cat in this.DataContext.Set<StateSecondary>()
        //                 where cat.Customer_Id == customerId
        //                 orderby cat.Name
        //                 select cat);

        //    bool add = false;
        //    if (reg != null)
        //    {
        //        foreach (StateSecondary fg in query)// query)
        //        {
        //            add = true;
        //            foreach (string i in reg)
        //            {
        //                if (i == fg.Id.ToString())
        //                {
        //                    add = false;
        //                }
        //            }

        //            if (add == true)
        //            {
        //                rlist.Add(fg);
        //            }
        //        }
        //    }
        //    if (rlist.Count == 0)
        //    {
        //        return query.ToList();
        //    }
        //    else
        //    {
        //        return rlist.ToList();
        //    }
        //}

    }
}
