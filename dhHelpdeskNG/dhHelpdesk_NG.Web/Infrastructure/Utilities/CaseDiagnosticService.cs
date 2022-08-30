using System;
using System.Diagnostics;
using System.Web;
using DH.Helpdesk.BusinessData.Models.LogProgram;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Utilities
{
    public interface ICaseDiagnosticService
    {
        void MakeTestSnapShot(HttpContextBase ctx);
    }

    public class CaseDiagnosticService : ICaseDiagnosticService
    {
        private readonly ILogProgramService _logProgramService;
        private readonly ICaseService _caseService;
        private const string SessionStatusKey = "SessionStatus";

        #region SessionStatus

        private class SessionStatus
        {
            // Seconds between refreshes, suggestion each 30 minutes, add to some config
            public const int SessionRefreshTime = 1800;
            public DateTime LastSessionStatusUpdate { get; set; }
            public Guid Guid { get; internal set; }
        }

        #endregion

        #region ctor()

        public CaseDiagnosticService(ILogProgramService logProgramService, ICaseService caseService)
        {
            _logProgramService = logProgramService;
            _caseService = caseService;
        }

        #endregion

        #region MakeTestSnapShot

        public void MakeTestSnapShot(HttpContextBase ctx)
        {
            try
            {
                var session = ctx.Session;
                // Check can be null in some request (static)
                if (session != null)
                {
                    var sessionStatus = session[SessionStatusKey] as SessionStatus;

                    var currentUser = SessionFacade.CurrentUser;
                    var userId = currentUser?.Id;

                    // No user session status exist, create new one
                    if (sessionStatus == null)
                    {
                        var responseTime = CheckResponseTime();
                        sessionStatus = new SessionStatus
                        {
                            LastSessionStatusUpdate = DateTime.Now,
                            Guid = Guid.NewGuid()
                        };

                        session[SessionStatusKey] = sessionStatus;

                        using (var p = Process.GetCurrentProcess())
                        {
                            // Start log for session
                            Log(userId, responseTime, sessionStatus.Guid, p.WorkingSet64, ctx.Request);
                        }
                    }
                    else // A status session item does already exist
                    {
                        var now = DateTime.Now;

                        // Check if is time to make update
                        var expires = sessionStatus.LastSessionStatusUpdate.AddSeconds(SessionStatus.SessionRefreshTime);
                        if (now >= expires)
                        {
                            // Updates sesstion status with current status information
                            var responseTime = CheckResponseTime();
                            sessionStatus.LastSessionStatusUpdate = now;

                            using (var p = Process.GetCurrentProcess())
                            {
                                Log(userId, responseTime, sessionStatus.Guid, p.WorkingSet64, ctx.Request);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignore all errors here
            }
        }

        private void Log(int? userId, long responseTime, Guid guid, long memoryUsage, HttpRequestBase request)
        {
            try
            {
                var data = $"{{ \"Topic\": \"Top 100 tblCases\", \"ResponseTime\": {responseTime}, \"Memory\": {memoryUsage}, \"Guid\": \"{guid}\" }}";

                var logProgramModel = new LogProgram
                {
                    CaseId = 0,
                    CustomerId = SessionFacade.CurrentCustomer != null ? SessionFacade.CurrentCustomer.Id : 0,
                    LogType = 99, // New Type for test purpose 
                    LogText = data,
                    New_Performer_user_Id = 0,
                    Old_Performer_User_Id = "0",
                    RegTime = DateTime.UtcNow,
                    UserId = userId,
                    ServerNameIP = $"{Environment.MachineName} ({request.ServerVariables["LOCAL_ADDR"]})",
                    NumberOfUsers = null
                };

                _logProgramService.UpdateUserLogin(logProgramModel);
            }
            catch (Exception)
            {
            }
        }

        protected long CheckResponseTime()
        {
            long ellapsed;
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                _caseService.GetTop100CasesForTest();
                ellapsed = sw.ElapsedMilliseconds;
            }
            catch
            {
                ellapsed = -1;
            }
            finally
            {
                sw.Stop();
            }

            return ellapsed;
        }

        #endregion
    }
}