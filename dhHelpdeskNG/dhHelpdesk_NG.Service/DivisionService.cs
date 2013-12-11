using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IDivisionService
    {
        IList<Division> GetAllDivisions();
        IList<Division> GetDivisions(int customerId);

        Division GetDivision(int id);

        DeleteMessage DeleteDivision(int id);

        void SaveDivision(Division division, out IDictionary<string, string> errors);
        void Commit();
    }

    public class DivisionService : IDivisionService
    {
        private readonly IDivisionRepository _divisionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DivisionService(
            IDivisionRepository divisionRepository,
            IUnitOfWork unitOfWork)
        {
            _divisionRepository = divisionRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Division> GetAllDivisions()
        {
            return _divisionRepository.GetAll().OrderBy(x => x.Name).ToList(); 
        }

        public IList<Division> GetDivisions(int customerId)
        {
            return _divisionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Division GetDivision(int id)
        {
            return _divisionRepository.GetById(id);
        }

        public DeleteMessage DeleteDivision(int id)
        {
            var division = _divisionRepository.GetById(id);

            if (division != null)
            {
                try
                {
                    _divisionRepository.Delete(division);
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

        public void SaveDivision(Division division, out IDictionary<string, string> errors)
        {
            if (division == null)
                throw new ArgumentNullException("division");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(division.Name))
                errors.Add("Division.Name", "Du måste ange en division");

            if (division.Id == 0)
                _divisionRepository.Add(division);
            else
                _divisionRepository.Update(division);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
