using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public interface IBaseCaseSolutionService
    {
        CaseSolution GetCaseSolution(int id);
        Task<List<CaseSolutionOverview>> GetCustomerCaseSolutionsAsync(int customerId);
        Task<List<CaseSolutionOverview>> GetCustomerMobileCaseSolutionsAsync(int customerId);
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
       
        public Task<List<CaseSolutionOverview>> GetCustomerCaseSolutionsAsync(int customerId)
        {
            var caseSolutions = GetCustomerCaseSolutionsQuery(customerId);
            return caseSolutions.ToListAsync();
        }

        public Task<List<CaseSolutionOverview>> GetCustomerMobileCaseSolutionsAsync(int customerId)
        {
            var caseSolutions = GetCustomerCaseSolutionsQuery(customerId, cs => cs.ShowOnMobile == 1);
            return caseSolutions.ToListAsync();
        }

        public CaseSolutionCategory GetCaseSolutionCategory(int id)
        {
            return CaseSolutionCategoryRepository.GetById(id);
        }

        protected IQueryable<CaseSolutionOverview> GetCustomerCaseSolutionsQuery(int customerId, Expression<Func<CaseSolution, bool>> filterExp = null)
        {
            var queryable = CaseSolutionRepository.GetCustomerCaseSolutions(customerId);

            if (filterExp != null)
            {
                queryable = queryable.Where(filterExp);
            }

            return
                queryable.Select(cs => new CaseSolutionOverview()
                    {
                        CaseSolutionId = cs.Id,
                        Name = cs.Name,
                        CategoryId = cs.CaseSolutionCategory_Id,
                        CategoryName = cs.CaseSolutionCategory.Name,
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
                    }).AsQueryable();

        }

    }
}