using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Enums;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    
    public class FileIndexingRepository
    {
        const string _FROM_CLAUSE = "@FROM";        
		const string _DATA_SOURCE_CONNECTION_STRING = " Data Source=\"{0}\";";
        private static string[] _ACCEPTED_SUB_DIRECTORIES = new string[] { "html" };

        public FileIndexingRepository()
        {

        }

        public static Tuple<List<int>, List<int>> GetCaseNumeralInfoBy(string serverName, string catalogName, string searchText)
        {
            var _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING = string.Empty;

            if (ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"] != null)
                if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"].ConnectionString))
                    _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"].ConnectionString;

            var caseNumbers = new List<int>();
            var logIds   = new List<int>();
            
            var query = string.Format("SELECT path, filename{0}scope() " +
                                      "WHERE FREETEXT(Contents,'%{1}%')",
                                       _FROM_CLAUSE, searchText.SafeForSqlInject());

            var indexQuery = GetIndexQueryText(serverName, catalogName, query);            
            using (var con = new OleDbConnection(_INDEXING_SERVICE_PROVIDER_CONNECTION_STRING))
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = indexQuery;                        
                        var dr = cmd.ExecuteReader();
                        if (dr != null)
                        {                            
                            while (dr.Read())
                            {                                                                    
                                var fullPath = dr["path"].ToString();
                                bool isLog  = false;
                                int number = -1; 

                                RetrieveNumber(fullPath, out number, out isLog);
                                if (number != -1)                                
                                    if (isLog)
                                        logIds.Add(number);
                                    else
                                        caseNumbers.Add(number);                                                                
                            }
                        }
                        dr.Close();
                    }
                    catch (Exception ex)
                    {
                        DataLogRepository.SaveLog("Error: " + ex.Message, DataLogTypes.GENERAL);
                    }            
                    finally
                    {
                        con.Close();
                    }
                }
            }

            var ret = new Tuple<List<int>, List<int>>(caseNumbers, logIds);
            return ret;
        }

        private static void RetrieveNumber(string fullPath, out int number, out bool isLog)
        {
            number = -1;
            isLog = false;

            var lastDirectoryName = new DirectoryInfo(fullPath).Parent.Name.ToLower();            
            if (_ACCEPTED_SUB_DIRECTORIES.Contains(lastDirectoryName))
            {
                fullPath = fullPath.Replace(string.Format("{0}\\", lastDirectoryName), string.Empty);
                lastDirectoryName = new DirectoryInfo(fullPath).Parent.Name.ToLower();
            }
            
            if (lastDirectoryName.Contains(ModuleName.Log.ToLower()))
            {
                lastDirectoryName = lastDirectoryName.Replace(ModuleName.Log.ToLower(), string.Empty);
                isLog = true;                
            }

            int tempCaseNumber;
            if (int.TryParse(lastDirectoryName, out tempCaseNumber))
                number = tempCaseNumber;            
        }
                           
        private static string GetIndexQueryText(string serverName, string catalogName, string query)
        {            
            if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(catalogName) || 
                string.IsNullOrEmpty(query) || !query.Contains(_FROM_CLAUSE))            
                return null;
            
            return query.Replace(_FROM_CLAUSE, string.Format(" FROM {0}.{1}..", serverName, catalogName));                                                                                 
        }               
              
    }
}