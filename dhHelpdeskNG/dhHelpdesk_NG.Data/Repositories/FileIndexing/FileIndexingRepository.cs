using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;

namespace DH.Helpdesk.Dal.Repositories
{
	using Common.Constants;
	using DH.Helpdesk.Common.Enums;
	using DH.Helpdesk.Dal.Enums;
	using FileIndexing;
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
	using System.Text.RegularExpressions;

	public interface IFileIndexingRepository
	{
		Tuple<List<int>, List<int>> GetCaseNumeralInfoBy(string serverName, string catalogName, string searchText, string[] excludePathPatterns);
	}

	public class FileIndexingRepository : IFileIndexingRepository
    {
        const string _FROM_CLAUSE = "@FROM";        
		const string _DATA_SOURCE_CONNECTION_STRING = " Data Source=\"{0}\";";
        private static string[] _ACCEPTED_SUB_DIRECTORIES = new string[] { "html" };
		private IFeatureToggleRepository _featureToggleRepository;

		public FileIndexingRepository(IFeatureToggleRepository featureToggleRepository)
        {
			_featureToggleRepository = featureToggleRepository;
        }

		public Tuple<List<int>, List<int>> GetCaseNumeralInfoBy(string serverName, string catalogName, string searchText, string[] excludePathPatterns)
		{
			var _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING = string.Empty;


			var fileSearchIdxToggle = _featureToggleRepository.GetByStrongName(FeatureToggleTypes.FILE_SEARCH_IDX_SERVICE.ToString());

			if (ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"] != null)
				if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"].ConnectionString))
					_INDEXING_SERVICE_PROVIDER_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["HelpdeskIndexingService"].ConnectionString;

			var caseNumbers = new List<int>();
			var logIds = new List<int>();

			// Check if legacy indexing service should be used
			if (fileSearchIdxToggle != null && fileSearchIdxToggle.Active)
			{
				try
				{
					// Deprecated: remove when sure not used any more
					GetFilesUsingIndexingService(serverName, catalogName, searchText, _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING, caseNumbers, logIds);
				}
				catch (Exception ex)
				{
					DataLogRepository.SaveLog("Error: " + ex.Message, DataLogTypes.GENERAL);

					throw new FileIndexingException(FileIndexingServiceType.IndexingService, ex);
				}
			}
			else
			{
				try
				{


					var serverPrefix = serverName == null ? "" : serverName + ".";
					var query = $"SELECT System.ItemName, System.ItemFolderPathDisplay FROM {serverPrefix}SystemIndex WHERE CONTAINS(*, '\"{searchText.SafeForSqlInject()}\"', 1033) RANK BY COERCION(Absolute, 1) AND SCOPE='file:{catalogName}'";
					var adapter = new OleDbDataAdapter(query, _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING);
					var dataSet = new DataSet();
					if (adapter.Fill(dataSet) > 0)
					{
						var table = dataSet.Tables[0];

						var number = -1;
						var isLog = false;

						foreach (var row in table.Rows.OfType<DataRow>())
						{
							var path = (string)row["System.ItemFolderPathDisplay"];
							var file = (string)row["System.ItemName"];
							RetrieveNumber($"{path}\\{file})", out number, out isLog);

							var excludePath = excludePathPatterns.Any(pattern => Regex.Match(path, pattern).Success);

							if (number != -1 && !excludePath)
								if (isLog)
									logIds.Add(number);
								else
									caseNumbers.Add(number);
						}
					}
				}
				catch (Exception ex)
				{
					DataLogRepository.SaveLog("Error: " + ex.Message, DataLogTypes.GENERAL);

					throw new FileIndexingException(FileIndexingServiceType.WindowsSearch, ex);
				}
			}


			var ret = new Tuple<List<int>, List<int>>(caseNumbers, logIds);
			return ret;
		}

		private static void GetFilesUsingIndexingService(string serverName, string catalogName, string searchText, string _INDEXING_SERVICE_PROVIDER_CONNECTION_STRING, List<int> caseNumbers, List<int> logIds)
		{
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
						using (var dr = cmd.ExecuteReader())
						{
							if (dr != null)
							{
								while (dr.Read())
								{
									var fullPath = dr["path"].ToString();
									bool isLog = false;
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