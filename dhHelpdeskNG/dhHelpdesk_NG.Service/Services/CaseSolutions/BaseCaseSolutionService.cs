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
        Task<CaseSolution> GetCaseSolutionAsync(int id);
        Task<List<CaseSolution>> GetCustomerCaseSolutionsAsync(int customerId);
        Task<List<CaseSolution>> GetCaseSolutionsWithExtendeCaseFormAsync(int customerId);
        Task<List<CaseSolutionOverview>> GetCustomerMobileCaseSolutionsAsync(int customerId);
        Task<List<CaseSolutionOverview>> GetCustomersMobileCaseSolutionsAsync(IList<int> customersIds);
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

        public Task<CaseSolution> GetCaseSolutionAsync(int id)
        {
            return CaseSolutionRepository.GetByIdAsync(id);
        }

        public Task<List<CaseSolution>> GetCustomerCaseSolutionsAsync(int customerId)
        {
            var caseSolutions = CaseSolutionRepository.GetCustomerCaseSolutions(new List<int> { customerId });
            return caseSolutions.ToListAsync();
        }

        public Task<List<CaseSolution>> GetCaseSolutionsWithExtendeCaseFormAsync(int customerId)
        {
            var caseSolutions = CaseSolutionRepository.GetCaseSolutionsWithExtendeCaseForm(customerId);
            return caseSolutions.ToListAsync();
        }

        public Task<List<CaseSolutionOverview>> GetCustomerMobileCaseSolutionsAsync(int customerId)
        {
            var caseSolutions = GetCustomerCaseSolutionsQuery(new List<int> { customerId }, cs => cs.ShowOnMobile == 1);
            return caseSolutions.ToListAsync();
        }

        public Task<List<CaseSolutionOverview>> GetCustomersMobileCaseSolutionsAsync(IList<int> customersIds)
        {
            var caseSolutions = GetCustomerCaseSolutionsQuery(customersIds, cs => cs.ShowOnMobile == 1);
            return caseSolutions.ToListAsync();
        }

        public CaseSolutionCategory GetCaseSolutionCategory(int id)
        {
            return CaseSolutionCategoryRepository.GetById(id);
        }

        protected IQueryable<CaseSolutionOverview> GetCustomerCaseSolutionsQuery(IList<int> customersIds, Expression<Func<CaseSolution, bool>> filterExp = null)
        {
            var queryable = customersIds.Count == 1
                ? CaseSolutionRepository.GetCustomerCaseSolutions(customersIds.Single())
                : CaseSolutionRepository.GetCustomerCaseSolutions(customersIds);

            if (filterExp != null)
            {
                queryable = queryable.Where(filterExp);
            }

            return
                queryable.Select(cs => new CaseSolutionOverview()
                {
                    CaseSolutionId = cs.Id,
                    Name = cs.Name,
                    CustomerId = cs.Customer_Id,
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