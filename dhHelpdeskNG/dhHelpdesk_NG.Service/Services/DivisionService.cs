namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public DivisionService(
            IDivisionRepository divisionRepository,
            IUnitOfWork unitOfWork)
        {
            this._divisionRepository = divisionRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Division> GetAllDivisions()
        {
            return this._divisionRepository.GetAll().OrderBy(x => x.Name).ToList(); 
        }

        public IList<Division> GetDivisions(int customerId)
        {
            return this._divisionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Division GetDivision(int id)
        {
            return this._divisionRepository.GetById(id);
        }

        public DeleteMessage DeleteDivision(int id)
        {
            var division = this._divisionRepository.GetById(id);

            if (division != null)
            {
                try
                {
                    this._divisionRepository.Delete(division);
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
                this._divisionRepository.Add(division);
            else
                this._divisionRepository.Update(division);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
