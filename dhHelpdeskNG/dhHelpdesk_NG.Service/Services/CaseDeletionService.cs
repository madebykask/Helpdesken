using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Dal.Infrastructure.Extensions;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Services.Services.Cache;

namespace DH.Helpdesk.Services.Services
{

    using DH.Helpdesk.BusinessData.Models.BusinessRules;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Constants;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Enums.BusinessRule;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Customers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Customers;
    using DH.Helpdesk.Services.Infrastructure.Email;
    using DH.Helpdesk.Services.Services.CaseStatistic;
    using DH.Helpdesk.Services.utils;
    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
    using DH.Helpdesk.Services.Infrastructure;
    using Feedback;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    using System.Linq.Expressions;
    using Common.Enums.Cases;
    using Common.Extensions.String;
    using Utils;
    using DH.Helpdesk.BusinessData.Models.User;
    using DH.Helpdesk.BusinessData.Models.Case.MergedCase;
    using DH.Helpdesk.Dal.Repositories.Cases.Concrete;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using LinqLib.Operators;

    public class CaseDeletionService : ICaseDeletionService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseFileRepository _caseFileRepository;
       
        private readonly ILogRepository _logRepository;
        private readonly ILogFileRepository _logFileRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IFilesStorage _filesStorage;
        private readonly IMasterDataService _masterDataService;
       

#pragma warning disable 0618       

        public CaseDeletionService(
            ICaseRepository caseRepository,
            ICaseFileRepository caseFileRepository,
            ILogRepository logRepository,
            ILogFileRepository logFileRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IFilesStorage filesStorage,
            IMasterDataService masterDataService)
        {
            _caseRepository = caseRepository;
            _caseFileRepository = caseFileRepository;
            _logRepository = logRepository;
            _logFileRepository = logFileRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _filesStorage = filesStorage;
            _masterDataService = masterDataService;
        }
        
        public void DeleteCases(int[] ids, int customerId, int? parentCaseId)
        {
            var caseList = _caseRepository.GetCasesByCaseIds(ids);
            var caseFiles = _caseFileRepository.GetCaseFilesByCaseList(caseList);
            var logFiles = _logFileRepository.GetLogFilesByCaseList(caseList, true);           

            var basePath = _masterDataService.GetFilePath(customerId);
            var caseConcreteRepository = new CaseConcreteRepository();
            var idsToExclude = ValidateParentChildDeletion(ids);
            
            if (idsToExclude != null)
            {
                caseList = caseList.Where(x => !idsToExclude.Contains(x.Id)).ToList();
                ids = ids.Where(x=> !idsToExclude.Contains(x)).ToArray();
            }
            if (caseConcreteRepository.DeleteCases(ids))
            {
                _filesStorage.DeleteFilesInFolders(caseList, caseFiles, logFiles, basePath);
            }
        }

        private int[] ValidateParentChildDeletion(int[] ids)
        {
            int[] idsToExclude = null;
            foreach (var id in ids)
            {
                int index = 0;
                var childrenCases = _caseRepository.GetChildCasesFor(id);

                if (childrenCases != null)
                {
                    var childrenNotIncludedForDeletion = childrenCases.Where(x => !ids.Contains(x.Id));
                    if (childrenNotIncludedForDeletion.Any())
                    {
                        idsToExclude[index] = id;
                        index++;
                        //string errorMsg = Translation.Get("Ärendet har underärende som ej är avslutade");
                        //throw new Exception(errorMsg);
                    }
                }
            }

            return idsToExclude;
        }

        public Guid Delete(int id, string basePath, int? parentCaseId)
        {

            Guid ret = Guid.Empty;

            if (parentCaseId.HasValue)
            {
                using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
                {
                    var relationsRepo = uow.GetRepository<ParentChildRelation>();
                    var relation = relationsRepo.GetAll().FirstOrDefault(it => it.DescendantId == id);
                    if (relation == null || relation.AncestorId != parentCaseId.Value)
                    {
                        throw new ArgumentException(string.Format("bad parentCaseId \"{0}\" for case id \"{1}\"", parentCaseId.Value, id));
                    }

                    relationsRepo.Delete(relation);
                    uow.Save();
                    //@TODO: make a record in parent history
                }
            }

            var c = _caseRepository.GetById(id);
            var caseList = new List<Case>() { c };
            var caseFiles = _caseFileRepository.GetCaseFilesByCaseList(caseList);
            var logFiles = _logFileRepository.GetLogFilesByCaseList(caseList, true);


            var caseConcreteRepository = new CaseConcreteRepository();
            int[] caseId = { id };

            if (caseConcreteRepository.DeleteCases(caseId))
            {
                _filesStorage.DeleteFilesInFolders(caseList, caseFiles, logFiles, basePath);
            }

            ret = c.CaseGUID;

            return ret;
        }
#pragma warning restore 0618


    }
}
