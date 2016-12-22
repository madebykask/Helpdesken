using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Mail;
using DH.Helpdesk.EForm.Core;
using DH.Helpdesk.EForm.Core.Cache;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using System.Web;
using System.Reflection;
using System.IO;

namespace DH.Helpdesk.EForm.Model.Contrete
{
    public class DocumentTextRepository : IDocumentTextRepository
    {
        private readonly string _connectionString;

        public DocumentTextRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        //GetText (string value1, CustomerId) = Default?

        public string GetText(string value1, int? customerId)
        {
            return GetText("", value1, "", "", "", "", customerId, null);
        }

        public string GetText(string value1, string replaceValue1, int? customerId)
        {
            return GetText("", value1, "", "", "", replaceValue1, customerId, null);
        }

        public string GetText(string textType, string value1, string replaceValue1, int? customerId)
        {
            return GetText(textType, value1, "", "", "", replaceValue1, customerId, null);
        }



        public string GetText(string textType, string value1, string operator1, string value2)
        {
            return GetText(textType, value1, operator1, value2, "", "", null, null);
        }

        public string GetText(string textType, string value1, string operator1, string value2, string operator2)
        {
            return GetText(textType, value1, operator1, value2, operator2, "", null, null);
        }

        public string GetText(string textType, string value1, string operator1, string value2, string operator2, string replaceValue1)
        {
            return GetText(textType, value1, operator1, value2, operator2, replaceValue1, null, null);
        }

        public string GetText(string textType, string value1, string operator1, string value2, string operator2, string replaceValue1, int? customerId)
        {
            return GetText(textType, value1, operator1, value2, operator2, replaceValue1, customerId, null);
        }

        public string GetText(string textType, string value1, string operator1, string value2, string operator2, string replaceValue1, int? customerId, Guid? formGuid)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    if (!string.IsNullOrEmpty(textType)) command.Parameters.Add(new SqlParameter("@TextType", SqlDbType.NVarChar)).Value = textType;
                    if (!string.IsNullOrEmpty(value1)) command.Parameters.Add(new SqlParameter("@Value1", SqlDbType.NVarChar)).Value = "\"" + value1 + "\"" ;
                    if (!string.IsNullOrEmpty(operator1)) command.Parameters.Add(new SqlParameter("@Operator1", SqlDbType.NVarChar)).Value = operator1;
                    if (!string.IsNullOrEmpty(replaceValue1)) command.Parameters.Add(new SqlParameter("@ReplaceValue", SqlDbType.NVarChar)).Value = replaceValue1;
                    if (!string.IsNullOrEmpty(value2)) command.Parameters.Add(new SqlParameter("@Value2", SqlDbType.NVarChar)).Value = "\"" + value2 + "\"";
                    if (!string.IsNullOrEmpty(operator2)) command.Parameters.Add(new SqlParameter("@Operator2", SqlDbType.NVarChar)).Value = operator2;
                    if (customerId != null) command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    if (formGuid != null) command.Parameters.Add(new SqlParameter("@FormGuid", SqlDbType.UniqueIdentifier)).Value = formGuid;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_DocumentDataText";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return dr.SafeGetString("Text");
                        }
                    }

                    //return empty text
                    return "";

                }


            }
        }

    }

}