namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.FinishingCause;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IFinishingCauseService
    {
        IList<FinishingCauseCategory> GetFinishingCauseCategories(int customerId);
        IList<FinishingCause> GetFinishingCauses(int customerId);
        IList<FinishingCauseOverview> GetFinishingCausesWithChilds(int customerId);

        FinishingCauseCategory GetFinishingCauseCategory(int id);
        FinishingCause GetFinishingCause(int id);
        IList<FinishingCause> GetSubFinishingCauses(int id);

        DeleteMessage DeleteFinishingCauseCategory(int id);
        DeleteMessage DeleteFinishingCause(int id);

        void SaveFinishingCauseCategory(FinishingCauseCategory finishingCauseCategory, out IDictionary<string, string> errors);
        void SaveFinishingCause(FinishingCause finishingCause, out IDictionary<string, string> errors);
        void Commit();
    }

    public class FinishingCauseService : IFinishingCauseService
    {
        private readonly IFinishingCauseCategoryRepository _finishingCauseCategoryRepository;
        private readonly IFinishingCauseRepository _finishingCauseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FinishingCauseService(
            IFinishingCauseCategoryRepository finishingCauseCategoryRepository,
            IFinishingCauseRepository finishingCauseRepository,
            IUnitOfWork unitOfWork)
        {
            this._finishingCauseCategoryRepository = finishingCauseCategoryRepository;
            this._finishingCauseRepository = finishingCauseRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<FinishingCauseCategory> GetFinishingCauseCategories(int customerId)
        {
            return this._finishingCauseCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<FinishingCause> GetFinishingCauses(int customerId)
        {
            return this._finishingCauseRepository.GetMany(x => x.Customer_Id == customerId  && x.Parent_FinishingCause_Id == null).OrderBy(x => x.Name).ToList();
        }

        public IList<FinishingCauseOverview> GetFinishingCausesWithChilds(int customerId)
        {
            return this._finishingCauseRepository.GetFinishingCauseOverviews(customerId);
        }

        public FinishingCauseCategory GetFinishingCauseCategory(int id)
        {
            return this._finishingCauseCategoryRepository.Get(x => x.Id == id);
        }

        public FinishingCause GetFinishingCause(int id)
        {
            return this._finishingCauseRepository.Get(x => x.Id == id);
        }

        public IList<FinishingCause> GetSubFinishingCauses(int id)
        {
            return this._finishingCauseRepository.GetMany(x => x.Parent_FinishingCause_Id == id).ToList();
        }

        public DeleteMessage DeleteFinishingCauseCategory(int id)
        {
            var finishingCauseCategory = this._finishingCauseCategoryRepository.GetById(id);

            if (finishingCauseCategory != null)
            {
                try
                {
                    this._finishingCauseCategoryRepository.Delete(finishingCauseCategory);
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

        public DeleteMessage DeleteFinishingCause(int id)
        {
            var finishingCause = this._finishingCauseRepository.GetById(id);
            
            if (finishingCause != null)
            {
                try
                {
                    this._finishingCauseRepository.Delete(finishingCause);
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

        public void SaveFinishingCauseCategory(FinishingCauseCategory finishingCauseCategory, out IDictionary<string, string> errors)
        {
            if (finishingCauseCategory == null)
                throw new ArgumentNullException("finishingcausecategory");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(finishingCauseCategory.Name))
                errors.Add("FinishingCauseCategory.Name", "Du måste ange en avslutningskategori");

            if (finishingCauseCategory.Id == 0)
                this._finishingCauseCategoryRepository.Add(finishingCauseCategory);
            else
                this._finishingCauseCategoryRepository.Update(finishingCauseCategory);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveFinishingCause(FinishingCause finishingCause, out IDictionary<string, string> errors)
        {
            if (finishingCause == null)
                throw new ArgumentNullException("finishingcause");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(finishingCause.Name))
                errors.Add("FinishingCause.Name", "Du måste ange en avslutsorsak");

            if (finishingCause.Id == 0)
                this._finishingCauseRepository.Add(finishingCause);
            else
                this._finishingCauseRepository.Update(finishingCause);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
