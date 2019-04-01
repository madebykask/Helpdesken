using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
	public interface ICaseFollowUpRepository
	{
		void AddCaseFollowUp(CaseFollowUp newCaseFollowUp);
		CaseFollowUp GetCaseFollowUp(int userId, int caseId);
        CaseFollowUp GetCaseFollowUp( int caseId);
        void UpdateCaseFollowUp(CaseFollowUp existCase);
        void DeleteCaseFollowUp(CaseFollowUp caseFollowup);
    }

	public class CaseFollowUpRepository : RepositoryBase<CaseFollowUp>, ICaseFollowUpRepository
	{
		public CaseFollowUpRepository(IDatabaseFactory databaseFactory, IWorkContext workContext = null) : base(databaseFactory)
		{
		}

		public void AddCaseFollowUp(CaseFollowUp newCaseFollowUp)
		{
			Add(newCaseFollowUp);
			Commit();
		}

		public CaseFollowUp GetCaseFollowUp(int userId, int caseId)
		{
			return DataContext.CaseFollowUps.SingleOrDefault(x => x.User_Id == userId && x.Case_Id == caseId);
		}

        public CaseFollowUp GetCaseFollowUp(int caseId)
        {
            return DataContext.CaseFollowUps.SingleOrDefault(x =>  x.Case_Id == caseId);
        }

        public void UpdateCaseFollowUp(CaseFollowUp existCase)
		{
			Update(existCase);
			Commit();
		}

        public void DeleteCaseFollowUp(CaseFollowUp caseFollowup)
        {
            Delete(caseFollowup);
            Commit();
        }

    }
}
