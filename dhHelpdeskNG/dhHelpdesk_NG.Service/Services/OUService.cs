﻿using System.Threading.Tasks;
using DH.Helpdesk.Common.Extensions.Boolean;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IOUService
    {
        IList<OU> GetAllOUs();
        IList<OU> GetThemAllOUs(int customerId);
        IList<OU> GetOuForDepartment(int departmentId);
        IList<OU> GetOUs(int customerId);
        IList<OU> GetRootOUs(int customerId, bool includeSubOu = false);

        IList<OU> GetOUs(int customerId, int departmentId, bool? isActive = null);
        Task<IList<OU>> GetOUsAsync(int customerId, int departmentId, bool? isActive = null);

        OU GetOU(int id);

        OU GetOUIdByName(string oUName);

        DeleteMessage DeleteOU(int id);

        void SaveOU(OU ou, out IDictionary<string, string> errors);
        void Commit();

        List<OU> GetActiveOuForDepartment(int? departmentId, int customerId);
        Task<List<OU>> GetActiveOuForDepartmentAsync(int? departmentId, int customerId);
    }

    public class OUService : IOUService
    {
        private readonly IOrganizationUnitRepository _ouRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public OUService(IOrganizationUnitRepository ouRepository,
            IUnitOfWork unitOfWork)
        {
            this._ouRepository = ouRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<OU> GetAllOUs()
        {
            return this._ouRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public IList<OU> GetThemAllOUs(int customerId)
        {
            return this._ouRepository.GetMany(x => x.Department.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<OU> GetOuForDepartment(int departmentId)
        {
            return this._ouRepository.GetMany(x => x.Department_Id == departmentId).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Returns all first level OUs 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IList<OU> GetOUs(int customerId)
        {
            return _ouRepository.GetRootOUs(customerId, false).ToList();;
        }

        public IList<OU> GetRootOUs(int customerId, bool includeSubOu = false)
        {
            return _ouRepository.GetRootOUs(customerId, includeSubOu).ToList();
        }

        public IList<OU> GetOUs(int customerId, int departmentId, bool? isActive = null)
        {
            return _ouRepository.GetOUs(customerId, departmentId, isActive)
                    .OrderBy(x => x.Name)
                    .ToList();
        }

        public async Task<IList<OU>> GetOUsAsync(int customerId, int departmentId, bool? isActive = null)
        {
            return (await _ouRepository.GetOUsAsync(customerId, departmentId, isActive))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public OU GetOU(int id)
        {
            return this._ouRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteOU(int id)
        {
            var ou = this._ouRepository.GetById(id);

            if (ou != null)
            {
                try
                {
                    this._ouRepository.Delete(ou);

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

        public void SaveOU(OU ou, out IDictionary<string, string> errors)
        {
            if (ou == null)
                throw new ArgumentNullException("ou");

           
            ou.Path = ou.Path ?? string.Empty;
            ou.OUId = ou.OUId ?? string.Empty;
            ou.HomeDirectory = ou.HomeDirectory ?? string.Empty;
            ou.ScriptPath = ou.ScriptPath ?? string.Empty;            

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(ou.Name))
                errors.Add("ou.Name", "Du måste ange en organisationsenhet");


            if (ou.Id == 0)
                this._ouRepository.Add(ou);
            else
                this._ouRepository.Update(ou);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public OU GetOUIdByName(string oUName)
        {
            return this._ouRepository.Get(x => x.Name.ToLower() == oUName.ToLower());
        }

        public List<OU> GetActiveOuForDepartment(int? departmentId, int customerId)
        {
            var oUs = departmentId.HasValue 
                ? GetOUs(customerId, departmentId.Value, true) 
                : GetOUs(customerId).Where(o => o.IsActive == 1).ToList();
            
            return GetOusFlatList(oUs);
        }

        public async Task<List<OU>> GetActiveOuForDepartmentAsync(int? departmentId, int customerId)
        {
            var oUs = departmentId.HasValue ? await GetOUsAsync(customerId, departmentId.Value, true) : null;

            return GetOusFlatList(oUs);
        }

        private List<OU> GetOusFlatList(IList<OU> oUsTree)
        {
            var unionList = new List<OU>();
            if (oUsTree != null && oUsTree.Any())
            {
                unionList.AddRange(oUsTree);
                unionList.AddRange(oUsTree.SelectMany(ou => ou.SubOUs.Where(e => e.IsActive == 1)));
            }

            return unionList;
        }
    }
   
}
