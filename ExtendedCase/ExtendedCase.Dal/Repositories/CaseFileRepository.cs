using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Dal.Connection;
using ExtendedCase.Dal.Data;

namespace ExtendedCase.Dal.Repositories
{
    public interface ICaseFileRepository
    {
        bool FileExists(int caseId, string fileName);
        int SaveCaseFile(CaseFile caseFile);
        bool DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName);
    }

    public class CaseFileRepository:  HelpdeskRespositoryBase, ICaseFileRepository
    {
        public CaseFileRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public bool FileExists(int caseId, string fileName)
        {
            const string sql = @"SELECT COUNT(1) FROM [dbo].[tblcasefile] WHERE Case_Id = @caseId AND FileName = @fileName";
            var result = QueryScalar<bool>(sql, new { caseId = caseId, fileName = fileName.Trim()});

            return result;
        }

        public bool DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName)
        {
            fileName = (fileName ?? string.Empty).Trim();
            if (!FileExists(caseId, fileName)) return false;

            const string sql = "DELETE FROM [dbo].[tblcasefile] WHERE Case_Id = @caseId AND FileName = @fileName";

            var affectedRows = ExecQuery(sql, new { caseId = caseId, fileName = fileName});
            return affectedRows > 0;
        }

        public int SaveCaseFile(CaseFile caseFile)
        {
            const string sql = @"INSERT INTO [dbo].[tblCaseFile]
                                       ([Case_Id]
                                       ,[FileName]
                                       ,[UserId]
                                       ,[CreatedDate])
                                     OUTPUT INSERTED.[Id]
                                     VALUES
                                           (@caseId
                                           ,@fileName
                                           ,@userId
                                           ,@date)
                                ";

            var args = new { caseId = caseFile.Case_Id, fileName = caseFile.FileName.Trim(), userId = caseFile.UserId, date = caseFile.CreatedDate };
            var id = QuerySingle<int>(sql, args);

            return id;
        }
    }
}
