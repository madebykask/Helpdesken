using ECT.Model.Entities;
using ECT.Model.Entities.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.Model.Abstract
{
    public interface IGlobalViewRepository
    {
        IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, int userId, string searchKey = null);
        IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, int userId, bool allCoWorkers, string searchKey = null);
        IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, int userId, bool allCoWorkers, string searchKey = null, string formFieldName = null);
        IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, string employeeNumbers, string searchKey = null);
        IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, string employeeNumbers, int? userId, bool allCoWorkers, string searchKey = null, string formFieldName = null);

        //IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, string searchKey = null);
        //IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, bool allCoWorkers, string searchKey = null);
        //IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, string employeeNumbers, string searchKey = null);
        //IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, string employeeNumbers, bool allCoWorkers, string searchKey = null);
        IEnumerable<GlobalViewExtendedInfo> GetEmployeeExtendedInfo(Guid formGuid, string employeenumber);
        
        IDictionary<string, string> GetGvDataDictionary(int caseId);
        
        void SaveGlobalViewFields(int Case_ID, int FormField_Id, string GWValue);

        //int SaveGlobalViewfile(string Description, DateTime LastUpdateDate ,DateTime ChangeDate);
        //int GETGlobalViewFile(DateTime UpdateTime,string Description);


        IEnumerable<GVMapFields> GetAllGVFieldsName();
    }
}
