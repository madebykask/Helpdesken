using System;
using System.Collections.Generic;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.Model.Abstract
{
    public interface IContractRepository
    {
        IDictionary<string, string> GetFormDictionary(int caseId, Guid formGuid);
        Form GetFormByGuid(Guid guid);
        IEnumerable<FormField> GetFormFields(int caseId, Guid formGuid); 
        Contract Get(int caseId);

        /// <summary>Get DocumentData by caseId
        /// </summary> 
        DocumentData GetDocumentData(int caseId);
        IEnumerable<Option> GetCompanies(int customerId, int? userId);
        IEnumerable<Option> GetUnits(int customerId, int? userId, int? companyId);
        DateTime? CalculateLatestSLACountDate(int? oldSubStateId, int? newSubStateId, DateTime? oldSLADate, int customerId);

        int SaveNew(Guid formGuid, int caseId, int userId, string regUserId, int stateSecondaryId, int source, string languageId, string ipNumber, int? parentCaseId, IDictionary<string, string> formFields, string CreatedByUser);
       
        void SaveFileViewLog(FileViewLog fileViewLog);
        void SaveCaseFile(CaseFile caseFile);

        CaseFile GetCaseFile(int id);
        void DeleteCaseFile(int id);
        IEnumerable<CaseFile> GetCaseFiles(int caseId);
        
        GlobalSettings GetGlobalSettings();
        string GetFolderPath(string caseNumber);
        string GetSiteUrl(string caseNumber, string currentUrl);
        
        Company GetCompany(string searchKey);
        Company GetCompanyByCode(int customerId, string code);

        Department GetDepartmentByCode(int customerId, string code);
        Department GetDepartmentBySearchKey(int customerId, string searchKey);
        Department GetDepartmentById(int Id);
        Department GetDepartmentByKey(string searchKey, int customerId);
        
        IDictionary<string, string> GetDepartment(string searchKey, int customerId);
        IEnumerable<OU> GetOUs(int? departmentId, int? parentOUId);
        IList<StaticFile> GetStaticDocuments(int productAreaId);
        OU GetOuById(int Id);

        //string GetWatchDate(int customerId, string startdate, int Days, int HolidayHeader_Id);
    }
}
