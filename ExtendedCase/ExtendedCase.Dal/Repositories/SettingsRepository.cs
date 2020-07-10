using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Dal.Connection;

namespace ExtendedCase.Dal.Repositories
{
    public interface ISettingsRepository
    {
        string GetFilePath(int customerId);
    }

    public class SettingsRepository: HelpdeskRespositoryBase, ISettingsRepository
    {
        public SettingsRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public string GetFilePath(int customerId)
        {
            const string sql = @"SELECT TOP(1) PhysicalFilePath FROM [dbo].[tblSettings] WHERE Customer_Id = @cid";
            var result = QueryScalar<string>(sql, new { cid = customerId });

            return result;
        }
    }
}
