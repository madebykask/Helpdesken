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

        public FileIndexingRepository()
        {

        }

        public static List<int> GetCaseNumbersBy(string serverName, string catalogName, string searchText)
        {
            var _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING = string.Empty;

            if (ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"] != null)
                if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"].ConnectionString))
                    _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"].ConnectionString;

            var ret = new List<int>();
            
            var query = string.Format("SELECT path, filename{0}scope() " +
                                      "WHERE FREETEXT(Contents,'%{1}%')",
                                       _FROM_CLAUSE, searchText);

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
                                int caseNumber = -1;

                                var fullPath = dr["path"].ToString();
                                var lastDirectoryName = new DirectoryInfo(fullPath).Parent.Name.ToLower();
                                if (lastDirectoryName.Contains(ModuleName.Log.ToLower()))
                                    lastDirectoryName = lastDirectoryName.Replace(ModuleName.Log.ToLower(), string.Empty);

                                int tempCaseNumber;
                                if (int.TryParse(lastDirectoryName, out tempCaseNumber))
                                    caseNumber = tempCaseNumber;

                                if (caseNumber != -1)
                                    ret.Add(caseNumber);
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

            return ret;
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