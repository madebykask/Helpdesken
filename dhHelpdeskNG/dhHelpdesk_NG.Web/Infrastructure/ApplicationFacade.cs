namespace DH.Helpdesk.Web.Infrastructure
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Web;

	public class UserCaseInfo
	{
		public int UserId { get; set; }
		public int CaseId { get; set; }
		public DateTime Looked { get; set; }
	}

	public class LoggedInUsers
	{
		public string SessionId { get; set; }
		public string CaseNumber { get; set; }
		public int CaseId { get; set; }
		public int Customer_Id { get; set; }
		public int User_Id { get; set; }
		public string CustomerName { get; set; }
		public string UserFirstName { get; set; }
		public string UserLastName { get; set; }
		public DateTime LatestActivity { get; set; }
		public DateTime LoggedOnLastTime { get; set; }

	}

	public static class ApplicationFacade
	{
		private const string _USER_CASE_INFO = "USER_CASE_INFO";
		public const string _USER_LOGGED_IN = "USER_LOGGED_IN";

		private const string _PERFORMANCE_LOG_ACTIVE = "PERFORMANCE_LOG_ACTIVE";
		private const string _PERFORMANCE_LOG_FREQUENCY = "PERFORMANCE_LOG_FREQUENCY";
		private const string _PERFORMANCE_LOG_SETTINGS_CACHE = "PERFORMANCE_LOG_SETTINGS_CACHE";

		static ApplicationFacade()
		{
			Version = DH.Helpdesk.Version.FULL_VERSION;
		}

		public static string Version { get; private set; }

		public static IList<UserCaseInfo> UserCaseInfo
		{
			get
			{
				return (IList<UserCaseInfo>)HttpContext.Current.Application[_USER_CASE_INFO];
			}
			private set { }
		}

		public static UserCaseInfo GetUserCaseInfo(int caseId)
		{
			if (UserCaseInfo == null) return null;

			return UserCaseInfo.Where(x => x.CaseId == caseId).FirstOrDefault();
		}

		public static void AddCaseUserInfo(int userId, int caseId)
		{
			if (UserCaseInfo == null)
			{
				HttpContext.Current.Application[_USER_CASE_INFO] = new List<UserCaseInfo>();
			}

			if (UserCaseInfo.Where(x => x.CaseId == caseId).ToList().Count == 0)
				UserCaseInfo.Add(new UserCaseInfo { UserId = userId, CaseId = caseId, Looked = DateTime.UtcNow });
		}

		public static void RemoveCaseUserInfo(int userId)
		{
			if (UserCaseInfo == null) return;

			var userCaseInfos = UserCaseInfo.Where(x => x.UserId == userId).ToList();
			foreach (var userCaseInfo in userCaseInfos)
			{
				UserCaseInfo.Remove(userCaseInfo);
			}
		}

		internal static void SetPerformanceLogSettingsCache(int performanceLogSettingsCache)
		{
			HttpContext.Current.Application[_PERFORMANCE_LOG_SETTINGS_CACHE] = DateTime.UtcNow.AddSeconds(performanceLogSettingsCache);
		}

		public static void RemoveUserFromCase(int userId, int caseId, string sessionId)
		{
			if (UserCaseInfo == null) return;

			var userCaseInfo = UserCaseInfo.FirstOrDefault(x => x.UserId == userId && x.CaseId == caseId);

			if (userCaseInfo != null)
				UserCaseInfo.Remove(userCaseInfo);

			UpdateLoggedInUser(sessionId, "");
		}

		public static ConcurrentDictionary<string, LoggedInUsers> LoggedInUsers
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return (ConcurrentDictionary<string, LoggedInUsers>)HttpContext.Current.Application[_USER_LOGGED_IN];
			}
			private set { }
		}

		public static IList<LoggedInUsers> GetLoggedInUsers(int customerId)
		{
			var loggedInUsers = LoggedInUsers?.Where(x => x.Value.Customer_Id == customerId).Select(x => x.Value).ToList();

			return loggedInUsers;
		}

		public static IList<LoggedInUsers> GetAllLoggedInUsers()
		{
			var loggedInUsers = LoggedInUsers?.Select(x => x.Value).ToList();

			return loggedInUsers;
		}

		public static void AddLoggedInUser(LoggedInUsers loggedInUsers)
		{
			if (LoggedInUsers == null)
			{
				HttpContext.Current.Application[_USER_LOGGED_IN] = new ConcurrentDictionary<string, LoggedInUsers>();
			}
			LoggedInUsers.GetOrAdd(loggedInUsers.SessionId, loggedInUsers);
		}

		public static void UpdateLoggedInUserActivity(string sessionId)
		{
			if (LoggedInUsers != null)
			{
				if (LoggedInUsers.ContainsKey(sessionId))
				{
					LoggedInUsers[sessionId].LatestActivity = DateTime.UtcNow;
				}
			}
		}

		public static void UpdateLoggedInUser(string sessionId)
		{
			UpdateLoggedInUser(sessionId, null, 0);
		}

		public static void UpdateLoggedInUser(string sessionId, string caseNumber)
		{
			UpdateLoggedInUser(sessionId, caseNumber, 0);
		}

		public static void UpdateLoggedInUser(string sessionId, string caseNumber, int caseId)
		{
			if (LoggedInUsers != null)
			{
				if (LoggedInUsers.ContainsKey(sessionId))
				{
					LoggedInUsers[sessionId].LatestActivity = DateTime.UtcNow;
					if (caseNumber != null)
					{
						LoggedInUsers[sessionId].CaseNumber = caseNumber;
						LoggedInUsers[sessionId].CaseId = caseId;
					}
				}
			}
		}

		public static void RemoveLoggedInUser(string sessionId)
		{
			if (LoggedInUsers == null) return;

			if (LoggedInUsers.ContainsKey(sessionId))
			{
				LoggedInUsers user;
				var maxCount = 3;
				for (var i = 0; i < maxCount; i++)
				{
					if (LoggedInUsers.TryRemove(sessionId, out user)) break;
				}
			}
		}




		public static bool IsPerformanceLogActive()
		{
			var active = false;
			if (HttpContext.Current.Application[_PERFORMANCE_LOG_ACTIVE] != null)
				active = (bool)HttpContext.Current.Application[_PERFORMANCE_LOG_ACTIVE];
			return active;
		}

		public static void SetPerformanceLogActive(bool state)
		{
			HttpContext.Current.Application[_PERFORMANCE_LOG_ACTIVE] = state;
		}

		public static int GetPerformanceLogFrequency()
		{
			return (int)HttpContext.Current.Application[_PERFORMANCE_LOG_FREQUENCY];
		}

		public static void SetPerformanceLogFrequency(int frequency)
		{
			HttpContext.Current.Application[_PERFORMANCE_LOG_FREQUENCY] = frequency;
		}

		internal static bool HasPerformanceLogSettingsCacheExpired()
		{
			var expires = (DateTime)HttpContext.Current.Application[_PERFORMANCE_LOG_SETTINGS_CACHE];

			var expired = false;
			if ((expires <= DateTime.UtcNow))
				expired = true;

			return expired;
		}
	}
}