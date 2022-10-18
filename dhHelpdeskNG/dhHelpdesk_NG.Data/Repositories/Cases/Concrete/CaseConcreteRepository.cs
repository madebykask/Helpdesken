using DbExtensions;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DH.Helpdesk.BusinessData.Models.Case.Output;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    public class CaseConcreteRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
        
        public bool DeleteCases(int[] caseIds)
        {
            bool ret = false;

            DataTable casesTable = new DataTable();
            casesTable.Columns.Add(new DataColumn("Id", typeof(int)));
            
            // populate DataTable from your List here
            foreach (var id in caseIds)
                casesTable.Rows.Add(id);

            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.StoredProcedure, CommandTimeout = 0 })
                {
                    SqlParameter param = command.Parameters.AddWithValue("@Cases", casesTable);
                    param.SqlDbType = SqlDbType.Structured;
                    param.TypeName = "dbo.IdsList";
                    //param.Direction = ParameterDirection.Output;
                    command.CommandText = "sp_DeleteCases";
                    var rowsAffected =  command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        ret = true;
                    }
                }
            }            

            return ret;
            /*
            this.DeleteChildCasesFor(id);

            DeleteExtendedCase(id);

            //delete CaseIsAbout
            this.DeleteCaseIsAboutFor(id);

            // delete form field values
            var ffv = _formFieldValueRepository.GetFormFieldValuesByCaseId(id);
            if (ffv != null)
            {
                foreach (var v in ffv)
                {
                    _formFieldValueRepository.Delete(v);
                }
                _formFieldValueRepository.Commit();
            }

            // delete log files
            var logFiles = _logFileRepository.GetLogFilesByCaseId(id, true);

            if (logFiles != null)
            {
                foreach (var f in logFiles)
                {
                    _filesStorage.DeleteFile(f.GetFolderPrefix(), f.Log_Id, basePath, f.FileName);
                    _logFileRepository.Delete(f);
                }
                _logFileRepository.Commit();
            }

            // delete logs
            var logs = _logRepository.GetCaseLogs(id).ToList();

            // delete Mail2tickets with log
            foreach (var l in logs)
            {
                _mail2TicketRepository.DeleteByLogId(l.Id);
            }
            _mail2TicketRepository.Commit();

            //foreach (var l in logs)
            //{
            //    _emailLogAttemptRepository.DeleteLogAttempts(l.Id);
            //    _emailLogRepository.DeleteByLogId(l.Id);
            //}
            //_emailLogRepository.Commit();

            // delete email logs
            var elogs = _emailLogRepository.GetEmailLogsByCaseId(id);
            if (elogs != null)
            {
                foreach (var l in elogs)
                {
                    if (l.EmailLogAttempts != null && l.EmailLogAttempts.Any())
                        _emailLogAttemptRepository.DeleteLogAttempts(l.Id);

                    _emailLogRepository.Delete(l);
                }
                _emailLogRepository.Commit();
            }

            foreach (var l in logs)
            {
                _logRepository.Delete(l);
            }
            _logRepository.Commit();

            //Delete Mail2Tickets by caseId
            _mail2TicketRepository.DeleteByCaseId(id);
            _mail2TicketRepository.Commit();

            // delete caseHistory
            var caseHistories = _caseHistoryRepository.GetCaseHistoryByCaseId(id);
            if (caseHistories != null)
            {
                foreach (var h in caseHistories)
                {
                    _caseHistoryRepository.Delete(h);
                }
            }

            _caseHistoryRepository.Commit();

            //delete case lock
            _caseLockService.UnlockCaseByCaseId(id);

            // delete case files
            var caseFiles = _caseFileRepository.GetCaseFilesByCaseId(id);
            var c = _caseRepository.GetById(id);

            if (caseFiles != null)
            {
                foreach (var f in caseFiles)
                {
                    var intCaseNumber = decimal.ToInt32(c.CaseNumber);
                    _filesStorage.DeleteFile(ModuleName.Cases, intCaseNumber, basePath, f.FileName);
                    _caseFileRepository.Delete(f);
                }
                _caseFileRepository.Commit();
            }

            // delete File View Log
            _caseFileRepository.DeleteFileViewLogs(id);
            _caseFileRepository.Commit();

            // delete Invoice
            _invoiceArticleService.DeleteCaseInvoices(id);

            // delete contract log
            var contractLog = _contractLogRepository.getContractLogByCaseId(id);
            if (contractLog != null)
                _contractLogRepository.Delete(contractLog);

            //delete FollowUp
            _caseFollowUpService.DeleteFollowUp(id);
            _caseExtraFollowersService.DeleteByCase(id);

            if (c.CaseSectionExtendedCaseDatas != null && c.CaseSectionExtendedCaseDatas.Any())
            {
                c.CaseSectionExtendedCaseDatas.Clear();
                DeletetblCase_tblCaseSection_ExtendedCaseData(id);
            }

            // delete caseQuestionnaireCircular
            _circularService.DeleteConnectedCase(id);

            DeleteCaseById(id);
            */
        }
    }
}
