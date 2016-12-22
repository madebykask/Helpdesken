using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ECT.Model.Abstract;
using ECT.Model.Entities;

namespace ECT.Model.Contrete
{
    public class TextRepository : ITextRepository
    {
        private readonly string _connectionString;

        public TextRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TextTranslation> GetTextTranslations(int? textType)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@TextType", SqlDbType.Int)).Value = textType;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_TextTranslations";
                    connection.Open();

                    using(var dr = command.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            yield return new TextTranslation
                            {
                                LanguageId = dr.SafeGetString("LanguageID"),
                                TextType = dr.SafeGetInteger("TextType"),
                                Text = dr.SafeGetString("TextString"),
                                Translation = dr.SafeGetString("TextTranslation")
                            };
                        }
                    }
                }
            }
        }

        public void LogTextTranslation(string textString, int customerId, string language, Guid formGuid, string source)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@TextString", SqlDbType.NVarChar)).Value = textString;
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@LanguageId", SqlDbType.NVarChar)).Value = language;
                    command.Parameters.Add(new SqlParameter("@FormGuid", SqlDbType.UniqueIdentifier)).Value = formGuid;
                    command.Parameters.Add(new SqlParameter("@Source", SqlDbType.NVarChar)).Value = source;
                    

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_LogTextTranslation";
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}

