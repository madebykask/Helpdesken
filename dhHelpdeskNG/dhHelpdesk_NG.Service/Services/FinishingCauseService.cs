using System.Threading.Tasks;

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
        Task<IList<FinishingCauseOverview>> GetFinishingCausesWithChildsAsync(int customerId);

        FinishingCauseCategory GetFinishingCauseCategory(int id);
        FinishingCause GetFinishingCause(int id);
        IList<FinishingCause> GetSubFinishingCauses(int id);

        DeleteMessage DeleteFinishingCauseCategory(int id);
        DeleteMessage DeleteFinishingCause(int id);

        string GetFinishingTypeName(int id);        

        void SaveFinishingCauseCategory(FinishingCauseCategory finishingCauseCategory, out IDictionary<string, string> errors);
        void SaveFinishingCause(FinishingCause finishingCause, out IDictionary<string, string> errors);
        void Commit();

        IEnumerable<FinishingCauseInfo> GetFinishingCauseInfos(int customerId);
        IEnumerable<FinishingCauseInfo> GetAllFinishingCauseInfos(int customerId);
        FinishingCause GetMergedFinishingCause(int customerId);
        IList<FinishingCause> GetAllFinishingCauses(int customerId);
        int SaveFinishingCauseAndGetId(FinishingCause finishingCause, out IDictionary<string, string> errors);
        int SaveFinishingCauseCategoryAndGetId(FinishingCauseCategory finishingCauseCategory, out IDictionary<string, string> errors);
    }

    public class FinishingCauseService : IFinishingCauseService
    {
        private readonly IFinishingCauseCategoryRepository _finishingCauseCategoryRepository;
        private readonly IFinishingCauseRepository _finishingCauseRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public FinishingCauseService(
            IFinishingCauseCategoryRepository finishingCauseCategoryRepository,
            IFinishingCauseRepository finishingCauseRepository,
            IUnitOfWork unitOfWork)
        {
            this._finishingCauseCategoryRepository = finishingCauseCategoryRepository;
            this._finishingCauseRepository = finishingCauseRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public string GetFinishingTypeName(int id)
        {
            var fc = GetFinishingCause(id);
            return GetFinishingCausePath(fc);
        }      

        private string GetFinishingCausePath(FinishingCause fc)
        {
            if (fc.Parent_FinishingCause_Id == null)
                return fc.Name;
            else
                return GetFinishingCausePath(fc.ParentFinishingCause) + "-" + fc.Name;
        }

        public IList<FinishingCauseCategory> GetFinishingCauseCategories(int customerId)
        {
            return this._finishingCauseCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<FinishingCause> GetFinishingCauses(int customerId)
        {
            return this._finishingCauseRepository
                .GetManyWithSubFinishingCauses(x => x.Customer_Id == customerId)
                .OrderBy(x => x.Name)
                .ToList()
                .Where(x => x.Parent_FinishingCause_Id == null)
                .ToList();
        }
        public IList<FinishingCause> GetAllFinishingCauses(int customerId)
        {
            return this._finishingCauseRepository
                .GetManyWithSubFinishingCauses(x => x.Customer_Id == customerId)
                .OrderBy(x => x.Name)
                .ToList();
        }

        public IList<FinishingCauseOverview> GetFinishingCausesWithChilds(int customerId)
        {
            var causeEntities = _finishingCauseRepository.GetFinishingCauseOverviews(customerId);

            var parentCauses = causeEntities.Where(c => c.ParentId == null).ToList();

            foreach (var parentCategory in parentCauses)
            {
                CreateFinishingCauseOverviewTree(parentCategory, causeEntities);
            }

            return parentCauses;
        }

        public async  Task<IList<FinishingCauseOverview>> GetFinishingCausesWithChildsAsync(int customerId)
        {
            var causeEntities = await _finishingCauseRepository.GetFinishingCauseOverviewsAsync(customerId);

            var parentCauses = causeEntities.Where(c => c.ParentId == null).ToList();

            foreach (var parentCategory in parentCauses)
            {
                CreateFinishingCauseOverviewTree(parentCategory, causeEntities);
            }

            return parentCauses;
        }

        public FinishingCauseCategory GetFinishingCauseCategory(int id)
        {
            return this._finishingCauseCategoryRepository.Get(x => x.Id == id);
        }

        public FinishingCause GetFinishingCause(int id)
        {
            return this._finishingCauseRepository.Get(x => x.Id == id);
        }
        public FinishingCause GetMergedFinishingCause(int customerId)
        {
            return this._finishingCauseRepository.Get(x => x.Customer_Id == customerId && x.Merged == true);
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
            finishingCauseCategory.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(finishingCauseCategory.Name))
                errors.Add("FinishingCauseCategory.Name", "Du måste ange en avslutningskategori");

            if (finishingCauseCategory.Id == 0)
            {
                finishingCauseCategory.FinishingCauseCategoryGUID = Guid.NewGuid();
                this._finishingCauseCategoryRepository.Add(finishingCauseCategory);
            }    
            else
                this._finishingCauseCategoryRepository.Update(finishingCauseCategory);

            if (errors.Count == 0)
                this.Commit();
        }
        public int SaveFinishingCauseCategoryAndGetId(FinishingCauseCategory finishingCauseCategory, out IDictionary<string, string> errors)
        {
            if (finishingCauseCategory == null)
                throw new ArgumentNullException("finishingcausecategory");

            errors = new Dictionary<string, string>();
            finishingCauseCategory.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(finishingCauseCategory.Name))
                errors.Add("FinishingCauseCategory.Name", "Du måste ange en avslutningskategori");

            if (finishingCauseCategory.Id == 0)
            {
                finishingCauseCategory.FinishingCauseCategoryGUID = Guid.NewGuid();
                this._finishingCauseCategoryRepository.Add(finishingCauseCategory);
            }   
            else
                this._finishingCauseCategoryRepository.Update(finishingCauseCategory);

            if (errors.Count == 0)
                this.Commit();
            return finishingCauseCategory.Id;
        }

        public void SaveFinishingCause(FinishingCause finishingCause, out IDictionary<string, string> errors)
        {
            if (finishingCause == null)
                throw new ArgumentNullException("finishingcause");

            errors = new Dictionary<string, string>();
            finishingCause.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(finishingCause.Name))
                errors.Add("FinishingCause.Name", "Du måste ange en avslutsorsak");

            if (finishingCause.Id == 0)
            {
                finishingCause.FinishingCauseGUID = Guid.NewGuid();
                this._finishingCauseRepository.Add(finishingCause);
            }
            else
                this._finishingCauseRepository.Update(finishingCause);

            if (errors.Count == 0)
                this.Commit();
        }
        public int SaveFinishingCauseAndGetId(FinishingCause finishingCause, out IDictionary<string, string> errors)
        {
            if (finishingCause == null)
                throw new ArgumentNullException("finishingcause");

            errors = new Dictionary<string, string>();
            finishingCause.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(finishingCause.Name))
                errors.Add("FinishingCause.Name", "Du måste ange en avslutsorsak");

            if (finishingCause.Id == 0)
            {
                finishingCause.FinishingCauseGUID = Guid.NewGuid();
                this._finishingCauseRepository.Add(finishingCause);
            }
            else
                this._finishingCauseRepository.Update(finishingCause);

            if (errors.Count == 0)
                this.Commit();
            return finishingCause.Id;
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IEnumerable<FinishingCauseInfo> GetFinishingCauseInfos(int customerId)
        {
            return this._finishingCauseRepository.GetFinishingCauseInfos(customerId);
        }

        public IEnumerable<FinishingCauseInfo> GetAllFinishingCauseInfos(int customerId)
        {
            return this._finishingCauseRepository.GetAllFinishingCauseInfos(customerId);
        }

        private void CreateFinishingCauseOverviewTree(FinishingCauseOverview parentCategory, IList<FinishingCauseOverview> allCategories)
        {
            var children = allCategories.Where(c => c.ParentId == parentCategory.Id).OrderBy(c => c.Name).ToList();
            if (children.Any())
            {
                parentCategory.ChildFinishingCauses.AddRange(children);
                foreach (var child in children)
                {
                    this.CreateFinishingCauseOverviewTree(child, allCategories);
                }
            }
        }
    }
}
