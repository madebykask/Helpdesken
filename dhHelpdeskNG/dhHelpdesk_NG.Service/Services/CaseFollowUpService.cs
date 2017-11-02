using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Services.Services
{
    public class CaseFollowUpService : ICaseFollowUpService
    {
        private readonly ICaseFollowUpRepository _caseFollowUpRepository;

        public CaseFollowUpService(ICaseFollowUpRepository caseFollowUpRepository)
        {
            _caseFollowUpRepository = caseFollowUpRepository;
        }

        public void AddUpdateFollowUp(int userId, int caseId)
        {
            var existFlwUp = _caseFollowUpRepository.GetCaseFollowUp(userId, caseId);
            if (existFlwUp == null)
            {
                var newCaseFollowUp = new CaseFollowUp
                {
                    Case_Id = caseId,
                    FollowUpDate = DateTime.UtcNow,
                    User_Id = userId,
                    IsActive = true
                };
                _caseFollowUpRepository.AddCaseFollowUp(newCaseFollowUp);
            }
            else
            {
                existFlwUp.FollowUpDate = DateTime.UtcNow;
                existFlwUp.IsActive = true;
                _caseFollowUpRepository.UpdateCaseFollowUp(existFlwUp);
            }
        }

        public void RemoveFollowUp(int userId, int caseId)
        {
            var existFlwUp = _caseFollowUpRepository.GetCaseFollowUp(userId, caseId);
            if (existFlwUp != null)
            {
                existFlwUp.IsActive = false;
                _caseFollowUpRepository.UpdateCaseFollowUp(existFlwUp);
            }
        }

        public void DeleteFollowUp(int caseId)
        {
            var existFlwUp = _caseFollowUpRepository.GetCaseFollowUp(caseId);
            if (existFlwUp != null)
            {
                _caseFollowUpRepository.DeleteCaseFollowUp(existFlwUp);
            }
        }

        public bool IsCaseFollowUp(int userId, int caseId)
        {
            var existFlwUp = _caseFollowUpRepository.GetCaseFollowUp(userId, caseId);
            if (existFlwUp != null && existFlwUp.IsActive)
            {
                return true;
            }
            return false;
        }
    }

    public interface ICaseFollowUpService
    {
        void AddUpdateFollowUp(int userId, int caseId);
        void RemoveFollowUp(int userId, int caseId);
        bool IsCaseFollowUp(int userId, int caseId);

        void DeleteFollowUp(int caseId);
    }
}
