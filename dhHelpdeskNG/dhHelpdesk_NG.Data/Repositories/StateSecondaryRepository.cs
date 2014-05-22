namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IStateSecondaryRepository : IRepository<StateSecondary>
    {
        //IList<StateSecondary> GetStateSecondariesAvailable(int customerId, string[] reg);
        //IList<StateSecondary> GetStateSecondariesSelected(int customerId, string[] reg);

        void ResetDefault(int exclude);
    }

    public class StateSecondaryRepository : RepositoryBase<StateSecondary>, IStateSecondaryRepository
    {
        public StateSecondaryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude)
        {
            foreach (StateSecondary obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
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
