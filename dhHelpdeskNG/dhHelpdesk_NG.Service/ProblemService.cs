using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IProblemService
    {
        IList<Problem> GetProblems(int customerId);

        Problem GetProblem(int id);

        DeleteMessage DeleteProblem(int id);

        void SaveProblem(Problem problem, out IDictionary<string, string> errors);
        void Commit();
    }

    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProblemService(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Problem> GetProblems(int customerId)
        {
            return _problemRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Problem GetProblem(int id)
        {
            return _problemRepository.GetById(id);
        }

        public DeleteMessage DeleteProblem(int id)
        {
            var problem = _problemRepository.GetById(id);

            if (problem != null)
            {
                try
                {
                    _problemRepository.Delete(problem);
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

        public void SaveProblem(Problem problem, out IDictionary<string, string> errors)
        {
            if (problem == null)
                throw new ArgumentNullException("problem");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(problem.Name))
                errors.Add("Problem.Name", "Du måste ange ett problem");

            if (string.IsNullOrEmpty(problem.InventoryNumber))
                errors.Add("Problem.InventoryNumber", "Du måste ange ett inventeringsnummer");

            if (problem.Id == 0)
                _problemRepository.Add(problem);
            else
                _problemRepository.Update(problem);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
