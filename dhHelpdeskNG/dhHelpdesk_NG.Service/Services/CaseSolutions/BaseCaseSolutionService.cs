using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public interface IBaseCaseSolutionService
    {
        CaseSolution GetCaseSolution(int id);
        CaseSolutionCategory GetCaseSolutionCategory(int id);
    }

    public class BaseCaseSolutionService : IBaseCaseSolutionService
    {
        protected readonly ICaseSolutionRepository _caseSolutionRepository;
        protected readonly ICaseSolutionCategoryRepository _caseSolutionCategoryRepository;

        #region ctor()

        public BaseCaseSolutionService(ICaseSolutionRepository caseSolutionRepository, 
            ICaseSolutionCategoryRepository categoryRepository)
        {
            _caseSolutionCategoryRepository = categoryRepository;
            _caseSolutionRepository = caseSolutionRepository;
        }

        #endregion

        public CaseSolution GetCaseSolution(int id)
        {
            return _caseSolutionRepository.GetById(id);
        }

        public CaseSolutionCategory GetCaseSolutionCategory(int id)
        {
            return _caseSolutionCategoryRepository.GetById(id);
        }
    }
}