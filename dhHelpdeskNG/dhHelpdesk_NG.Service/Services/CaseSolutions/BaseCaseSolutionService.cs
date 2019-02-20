using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public interface IBaseCaseSolutionService
    {
        CaseSolution GetCaseSolution(int id);
        IList<CaseSolutionOverview> GetCustomerCaseSolutions(int customerId);
        IList<CaseSolutionOverview> GetCustomerMobileCaseSolutions(int customerId);
        CaseSolutionCategory GetCaseSolutionCategory(int id);
    }

    public class BaseCaseSolutionService : IBaseCaseSolutionService
    {
        protected readonly ICaseSolutionRepository CaseSolutionRepository;
        protected readonly ICaseSolutionCategoryRepository CaseSolutionCategoryRepository;

        #region ctor()

        public BaseCaseSolutionService(
            ICaseSolutionRepository caseSolutionRepository, 
            ICaseSolutionCategoryRepository categoryRepository)
        {
            CaseSolutionCategoryRepository = categoryRepository;
            CaseSolutionRepository = caseSolutionRepository;
        }

        #endregion

        public CaseSolution GetCaseSolution(int id)
        {
            return CaseSolutionRepository.GetById(id);
        }
       
        public IList<CaseSolutionOverview> GetCustomerCaseSolutions(int customerId)
        {
            var caseSolutions = GetCustomerCaseSolutionsInner(customerId);
            return caseSolutions;
        }

        public IList<CaseSolutionOverview> GetCustomerMobileCaseSolutions(int customerId)
        {
            var caseSolutions = GetCustomerCaseSolutionsInner(customerId, cs => cs.ShowOnMobile == 1);
            return caseSolutions;
        }

        public CaseSolutionCategory GetCaseSolutionCategory(int id)
        {
            return CaseSolutionCategoryRepository.GetById(id);
        }

        protected IList<CaseSolutionOverview> GetCustomerCaseSolutionsInner(int customerId, Expression<Func<CaseSolution, bool>> filterExp = null)
        {
            var queryable = CaseSolutionRepository.GetCustomerCaseSolutions(customerId);

            if (filterExp != null)
            {
                queryable = queryable.Where(filterExp);
            }

            var caseSolutions =
                queryable.Select(cs => new CaseSolutionOverview()
                    {
                        CaseSolutionId = cs.Id,
                        Name = cs.Name,
                        CategoryId = cs.CaseSolutionCategory_Id,
                        StateSecondaryId = cs.StateSecondary_Id,
                        NextStepState = cs.NextStepState,
                        Status = cs.Status,
                        WorkingGroupId = cs.WorkingGroup_Id,
                        WorkingGroupName = cs.WorkingGroup.WorkingGroupName,
                        SortOrder = cs.SortOrder,
                        ConnectedButton = cs.ConnectedButton,
                        ShowInsideCase = cs.ShowInsideCase,
                        ShowOnCaseOverview = cs.ShowOnCaseOverview,
                        StateSecondary = cs.StateSecondary != null
                            ? new StateSecondaryOverview
                            {
                                Id = cs.StateSecondary.Id,
                                Name = cs.StateSecondary.Name,
                                StateSecondaryId = cs.StateSecondary.StateSecondaryId,
                            }
                            : null
                    })
                    .ToList();
            return caseSolutions;
        }

    }
}