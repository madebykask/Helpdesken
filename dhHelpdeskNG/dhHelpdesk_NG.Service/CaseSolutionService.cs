using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseSolutionService
    {
        IList<CaseSolution> GetCaseSolutions(int customerId);
        IList<CaseSolutionCategory> GetCaseSolutionCategories(int customerId);
        IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch SearchCaseSolutions);

        //int GetAntal(int customerId, int userid);

        CaseSolution GetCaseSolution(int id);
        CaseSolutionCategory GetCaseSolutionCategory(int id);
        CaseSolutionSchedule GetCaseSolutionSchedule(int id);

        DeleteMessage DeleteCaseSolution(int id);
        DeleteMessage DeleteCaseSolutionCategory(int id);

        void SaveCaseSolution(CaseSolution caseSolution, CaseSolutionSchedule caseSolutionSchedule, out IDictionary<string, string> errors);
        void SaveCaseSolutionCategory(CaseSolutionCategory caseSolutionCategory, out IDictionary<string, string> errors);
        void Commit();
    }

    public class CaseSolutionService : ICaseSolutionService
    {
        private readonly ICaseSolutionRepository _caseSolutionRepository;
        private readonly ICaseSolutionCategoryRepository _caseSolutionCategoryRepository;
        private readonly ICaseSolutionScheduleRepository _caseSolutionScheduleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CaseSolutionService(
            ICaseSolutionRepository caseSolutionRepository,
            ICaseSolutionCategoryRepository caseSolutionCategoryRepository,
            ICaseSolutionScheduleRepository caseSolutionScheduleRepository,
            IUnitOfWork unitOfWork)
        {
            _caseSolutionRepository = caseSolutionRepository;
            _caseSolutionCategoryRepository = caseSolutionCategoryRepository;
            _caseSolutionScheduleRepository = caseSolutionScheduleRepository;
            _unitOfWork = unitOfWork;
        }

        //public int GetAntal(int customerId, int userid)
        //{
        //    return _caseSolutionRepository.GetAntal(customerId, userid);
        //}
        
        public IList<CaseSolution> GetCaseSolutions(int customerId)
        {
            return _caseSolutionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch SearchCaseSolutions)
        {
            var query = (from cs in _caseSolutionRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select cs);

            if (!string.IsNullOrEmpty(SearchCaseSolutions.SearchCss))
                query = query.Where(x => x.Caption.Contains(SearchCaseSolutions.SearchCss)
                    || x.Description.Contains(SearchCaseSolutions.SearchCss)
                    || x.Miscellaneous.Contains(SearchCaseSolutions.SearchCss)
                    || x.Name.Contains(SearchCaseSolutions.SearchCss)
                    || x.ReportedBy.Contains(SearchCaseSolutions.SearchCss)
                    || x.Text_External.Contains(SearchCaseSolutions.SearchCss)
                    || x.Text_Internal.Contains(SearchCaseSolutions.SearchCss));

            return query.OrderBy(x => x.ChangedDate).ToList();
        }

        public IList<CaseSolutionCategory> GetCaseSolutionCategories(int customerId)
        {
            return _caseSolutionCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public CaseSolution GetCaseSolution(int id)
        {
            return _caseSolutionRepository.GetById(id);
        }

        public CaseSolutionCategory GetCaseSolutionCategory(int id)
        {
            return _caseSolutionCategoryRepository.GetById(id);
        }

        public CaseSolutionSchedule GetCaseSolutionSchedule(int id)
        {
            return _caseSolutionScheduleRepository.GetById(id);
        }

        public DeleteMessage DeleteCaseSolution(int id)
        {
            var caseSolution = _caseSolutionRepository.GetById(id);

            if (caseSolution != null)
            {
                try
                {
                    var caseSolutionSchedule = _caseSolutionScheduleRepository.GetById(id);

                    if (caseSolutionSchedule != null)
                        _caseSolutionScheduleRepository.Delete(caseSolutionSchedule);

                    _caseSolutionRepository.Delete(caseSolution);

                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public DeleteMessage DeleteCaseSolutionCategory(int id)
        {
            var caseSolutionCategory = _caseSolutionCategoryRepository.GetById(id);

            if (caseSolutionCategory != null)
            {
                try
                {
                    _caseSolutionCategoryRepository.Delete(caseSolutionCategory);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveCaseSolution(CaseSolution caseSolution, CaseSolutionSchedule caseSolutionSchedule, out IDictionary<string, string> errors)
        {
            if (caseSolution == null)
                throw new ArgumentNullException("casesolution");

            errors = new Dictionary<string, string>();

            caseSolution.Caption = caseSolution.Caption ?? string.Empty;
            caseSolution.Description = caseSolution.Description ?? string.Empty;
            caseSolution.Miscellaneous = caseSolution.Miscellaneous ?? string.Empty;
            caseSolution.ReportedBy = caseSolution.ReportedBy ?? string.Empty;
            caseSolution.Text_External = caseSolution.Text_External ?? string.Empty;
            caseSolution.Text_Internal = caseSolution.Text_Internal ?? string.Empty;

            if (string.IsNullOrEmpty(caseSolution.Name))
                errors.Add("CaseSolution.Name", "Du måste ange en ärendemall");

            if (string.IsNullOrEmpty(caseSolution.Caption))
                errors.Add("CaseSolution.Caption", "Du måste ange en rubrik");

            if (string.IsNullOrEmpty(caseSolution.Miscellaneous))
                errors.Add("CaseSolution.Miscellaneous", "Du måste fylla i övrigt-fältet");

            if (string.IsNullOrEmpty(caseSolution.Text_External))
                errors.Add("CaseSolution.Text_External", "Du måste fylla i extern notering-fältet");

            if (string.IsNullOrEmpty(caseSolution.Text_Internal))
                errors.Add("CaseSolution.Text_Internal", "Du måste fylla i intern notering-fältet");

            if (caseSolution.Id == 0)
                _caseSolutionRepository.Add(caseSolution);
            else
            {
                _caseSolutionRepository.Update(caseSolution);
                _caseSolutionScheduleRepository.Delete(x => x.CaseSolution_Id == caseSolution.Id);
            }

            if (caseSolutionSchedule != null)
            {
                _caseSolutionScheduleRepository.Add(caseSolutionSchedule);
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveCaseSolutionCategory(CaseSolutionCategory caseSolutionCategory, out IDictionary<string, string> errors)
        {
            if (caseSolutionCategory == null)
                throw new ArgumentNullException("casesolutioncategory");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(caseSolutionCategory.Name))
                errors.Add("CaseSolutionCategory.Name", "Du måste ange en ärendemallskategori");


            if (caseSolutionCategory.Id == 0)
                _caseSolutionCategoryRepository.Add(caseSolutionCategory);
            else
            {
                _caseSolutionCategoryRepository.Update(caseSolutionCategory);
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}