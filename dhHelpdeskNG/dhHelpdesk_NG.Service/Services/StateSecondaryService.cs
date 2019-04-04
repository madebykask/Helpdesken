using System.Data.Entity;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IStateSecondaryService
    {
        IList<StateSecondary> GetStateSecondaries(int customerId);
        Task<List<StateSecondary>> GetStateSecondariesAsync(int customerId);
        IList<StateSecondary> GetActiveStateSecondaries(int customerId);

        Task<List<StateSecondary>> GetActiveStateSecondariesAsync(int customerId);
        //IList<StateSecondary> GetStateSecondariesSelected(int customerId, string[] reg);
        //IList<StateSecondary> GetStateSecondariesAvailable(int customerId, string[] reg);

        StateSecondary GetStateSecondary(int id);
        Task<StateSecondary> GetStateSecondaryAsync(int id);
        
        DeleteMessage DeleteStateSecondary(int id);

        void SaveStateSecondary(StateSecondary stateSecondary, out IDictionary<string, string> errors);
        void Commit();

        ItemOverview GetDefaultOverview(int customerId);
    }

    public class StateSecondaryService : IStateSecondaryService
    {
        private readonly IStateSecondaryRepository _stateSecondaryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StateSecondaryService(
            IStateSecondaryRepository stateSecondaryRepository,
            IUnitOfWork unitOfWork)
        {
            this._stateSecondaryRepository = stateSecondaryRepository;
            this._unitOfWork = unitOfWork;
        }
        
        //public IList<StateSecondary> GetStateSecondariesSelected(int customerId, string[] reg)
        //{
        //    return _stateSecondaryRepository.GetStateSecondariesSelected(customerId, reg).ToList();
        //}

        //public IList<StateSecondary> GetStateSecondariesAvailable(int customerId, string[] reg)
        //{
        //    return _stateSecondaryRepository.GetStateSecondariesAvailable(customerId, reg).ToList();
        //}
        
        public IList<StateSecondary> GetStateSecondaries(int customerId)
        {
            return _stateSecondaryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Task<List<StateSecondary>> GetStateSecondariesAsync(int customerId)
        {
            return _stateSecondaryRepository.GetMany(x => x.Customer_Id == customerId)
                .AsQueryable()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public IList<StateSecondary> GetActiveStateSecondaries(int customerId)
        {
            return this._stateSecondaryRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public Task<List<StateSecondary>> GetActiveStateSecondariesAsync(int customerId)
        {
            return _stateSecondaryRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1)
                .AsQueryable()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public StateSecondary GetStateSecondary(int id)
        {
            return this._stateSecondaryRepository.Get(x => x.Id == id);
        }

        public Task<StateSecondary> GetStateSecondaryAsync(int id)
        {
            return _stateSecondaryRepository.GetAsync(x => x.Id == id);
        }

        public DeleteMessage DeleteStateSecondary(int id)
        {
            var stateSecondary = this._stateSecondaryRepository.GetById(id);

            if (stateSecondary != null)
            {
                try
                {
                    this._stateSecondaryRepository.Delete(stateSecondary);
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

        public void SaveStateSecondary(StateSecondary stateSecondary, out IDictionary<string, string> errors)
        {
            if (stateSecondary == null)
                throw new ArgumentNullException("statesecondary");

            errors = new Dictionary<string, string>();
            stateSecondary.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(stateSecondary.Name))
                errors.Add("StateSecondary.Name", "Du måste ange en understatus");

            if (stateSecondary.Id == 0)
                this._stateSecondaryRepository.Add(stateSecondary);
            else
                this._stateSecondaryRepository.Update(stateSecondary);

            if (stateSecondary.IsDefault == 1)
                this._stateSecondaryRepository.ResetDefault(stateSecondary.Id, stateSecondary.Customer_Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public ItemOverview GetDefaultOverview(int customerId)
        {
            return this._stateSecondaryRepository.GetDefaultOverview(customerId);
        }
    }
}
