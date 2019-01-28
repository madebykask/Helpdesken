using System.Collections.Generic;
using System.Linq;
using ExtendedCase.Dal.Connection;

namespace ExtendedCase.Dal.Repositories
{
    public class TranslationRepository : HelpdeskRespositoryBase, ITranslationRepository
    {
        #region ctor()

        public TranslationRepository(IDbConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion

        public IDictionary<string, string> GetTranslations(int languageId)
        {
            const string sql = @"
                SELECT Property, Text FROM ExtendedCaseTranslations
                WHERE LanguageId = @languageId
            ";

            var res = QueryList<dynamic>(sql, new { languageId }).ToDictionary(x => (string)x.Property, y => (string)y.Text);
            return res;
        }
    }

    public interface ITranslationRepository
    {
        IDictionary<string, string> GetTranslations(int languageId);
    }
}
