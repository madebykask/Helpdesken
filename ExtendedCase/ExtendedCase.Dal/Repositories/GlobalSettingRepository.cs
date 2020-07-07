using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Dal.Connection;

namespace ExtendedCase.Dal.Repositories
{
    public interface IGlobalSettingRepository
    {
        string GetFileUploadExtensionWhitelist();
    }

    public class GlobalSettingRepository: HelpdeskRespositoryBase, IGlobalSettingRepository
    {
        public GlobalSettingRepository(IDbConnectionFactory connectionFactory)
            : base(connectionFactory)
        {

        }
        
        public string GetFileUploadExtensionWhitelist()
        {
            const string sql = @"SELECT TOP (1) FileUploadExtensionWhitelist FROM tblGlobalSettings";
            var result = QueryList<string>(sql, new Dictionary<string, string>());

            return result[0];
        }
    }
}
