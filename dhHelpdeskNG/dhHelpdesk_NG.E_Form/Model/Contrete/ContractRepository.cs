using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Mail;
using ECT.Core;
using ECT.Core.Cache;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using System.Web;
using System.Reflection;
using System.IO;

namespace ECT.Model.Contrete
{
    public class ContractRepository : IContractRepository
    {
        private readonly string _connectionString;

        public ContractRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDictionary<string, string> GetFormDictionary(int caseId, Guid formGuid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                    command.Parameters.Add(new SqlParameter("@FormGUID", SqlDbType.UniqueIdentifier)).Value = formGuid;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_FormFieldValues";
                    connection.Open();

                    var dictionary = new Dictionary<string, string>();
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dictionary.ContainsKey(dr.SafeGetString("FormFieldName")))
                            {
                                var apa = dr.SafeGetString("FormFieldName");
                            }

                            dictionary.Add(dr.SafeGetString("FormFieldName"), dr.SafeGetString("FormFieldValue"));

                            if (!string.IsNullOrEmpty(dr.SafeGetString("InitialFormFieldValue")))
                                dictionary.Add("OLD_" + dr.SafeGetString("FormFieldName"), dr.SafeGetString("InitialFormFieldValue"));
                        }
                    }

                    return dictionary;
                }
            }
        }

        public Form GetFormByGuid(Guid guid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@formGuid", SqlDbType.UniqueIdentifier)).Value = guid;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_FormByGUID";

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        dr.Read();

                        Form f = new Form();
                        f.Id = dr.SafeGetInteger("Id");
                        f.FormGuid = dr.SafeGetGuid("FormGuid");
                        f.FormName = dr.SafeGetString("FormName");
                        f.FormPath = dr.SafeGetString("FormPath");
                        f.CustomerId = dr.SafeGetInteger("Customer_Id");
                        f.CategoryId = dr.SafeGetNullableInteger("Category_Id");
                        f.WorkingGroupId = dr.SafeGetNullableInteger("WorkingGroup_Id");

                        if (dr.SafeGetNullableInteger("Priority_Id2") != null)
                        {
                            f.PriorityId = dr.SafeGetNullableInteger("Priority_Id2");
                            f.PriorityName = dr.SafeGetString("PriorityName2");
                            f.SolutionTime = dr.SafeGetInteger("SolutionTime2");
                        }
                        else
                        {
                            f.PriorityId = dr.SafeGetNullableInteger("Priority_Id");
                            f.PriorityName = dr.SafeGetString("PriorityName");
                            f.SolutionTime = dr.SafeGetInteger("SolutionTime");
                        }

                        f.StateSecondaryId = dr.SafeGetNullableInteger("StateSecondary_Id");
                        f.ProductAreaId = dr.SafeGetNullableInteger("ProductArea_Id");

                        FormSettings fs = new FormSettings();
                        fs.TextTypeId = dr.SafeGetInteger("TextType_Id");
                        fs.LogTranslations = dr.SafeGetBoolean("LogTranslations");
                        fs.DateFormat = dr.SafeGetString("DateFormat");

                        f.FormSettings = fs;

                        return f;

                    }
                }
            }
        }

        public IEnumerable<FormField> GetFormFields(int caseId, Guid formGuid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                    command.Parameters.Add(new SqlParameter("@FormGUID", SqlDbType.UniqueIdentifier)).Value = formGuid;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_FormFieldValues";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return new FormField
                            {
                                Id = dr.SafeGetInteger("Id"),
                                FormId = dr.SafeGetInteger("Form_Id"),
                                FormFieldName = dr.SafeGetString("FormFieldName"),
                                FormFieldValue = dr.SafeGetString("FormFieldValue"),
                                InitialFormFieldValue = dr.SafeGetString("InitialFormFieldValue"),
                                HCMData = dr.SafeGetBoolean("HCMData"),
                                ParentGVFields = dr.SafeGetBoolean("ParentGVFields")
                            };
                        }
                    }
                }
            }
        }

        public Contract Get(int caseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.NVarChar)).Value = caseId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_Case";
                    connection.Open();

                    Contract contract = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            contract = new Contract
                            {
                                Id = dr.SafeGetInteger("Id"),
                                CaseNumber = dr.SafeGetString("Casenumber"),
                                FirstName = dr.SafeGetString("FirstName"),
                                Surname = dr.SafeGetString("Surname"),
                                HireDate = dr.SafeGetDateTime("HireDate"),
                                Initiator = dr.SafeGetString("Initiator"),
                                InitiatorEMail = dr.SafeGetString("InitiatorEmail"),
                                HelpdeskEMail = dr.SafeGetString("HelpdeskEMail"),
                                StateSecondaryId = dr.SafeGetInteger("StateSecondaryId"),
                                StateSecondary = dr.SafeGetString("StateSecondary"),
                                AlternativeStateSecondaryName = dr.SafeGetString("AlternativeStateSecondaryName"),
                                LanguageId = dr.SafeGetInteger("Language_Id"),
                                WorkingGroupId = dr.SafeGetInteger("WorkingGroup_Id"),
                                ProductAreaId = dr.SafeGetInteger("ProductArea_Id"),
                                PriorityName = dr.SafeGetString("PriorityName"),
                                Unit = new Option
                                {
                                    Id = dr.SafeGetInteger("Department_Id"),
                                    Key = dr.SafeGetString("DepartmentSearchKey"),
                                    Name = dr.SafeGetString("Department"),
                                },
                                Company = new Option
                                {
                                    Id = dr.SafeGetInteger("Region_Id"),
                                    Key = dr.SafeGetString("RegionSearchKey"),
                                    Name = dr.SafeGetString("Region"),
                                },
                                CustomerId = dr.SafeGetInteger("Customer_Id"),
                                CaseFiles = GetCaseFiles(caseId),
                                CaseHistory = GetCaseHistory(caseId),
                                EmployeeNumber = dr.SafeGetString("EmployeeNumber"),
                                ParentCaseNumber = dr.SafeGetInteger("ParentCaseNumber"),
                                ChildCaseNumbers = GetChildCaseNumbers(int.Parse(dr.SafeGetString("CaseNumber"))),
                                FinishingDate = dr.SafeGetNullableDateTime("FinishingDate"),
                                WatchDate = dr.SafeGetNullableDateTime("WatchDate"),
                                ChangeTime = dr.SafeGetDateTime("ChangeTime"),
                                RegTime = dr.SafeGetDateTime("RegTime"),
                                HolidayHeader_Id = dr.SafeGetInteger("HolidayHeader_Id"),
                                SolutionTime = dr.SafeGetInteger("SolutionTime"),
                                ExternalTime = dr.SafeGetInteger("ExternalTime"),
                                LatestSLACountDate = dr.SafeGetNullableDateTime("LatestSLACountDate")
                            };
                        }
                    }

                    return contract;
                }
            }
        }

        public DocumentData GetDocumentData(int caseId)
        {

            DocumentData documentData = new DocumentData();
            documentData.DocumentFields = GetDocumentFields(caseId);

            return documentData;
        }

        public List<DocumentField> GetDocumentFields(int caseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_DocumentDataValues";
                    connection.Open();

                    List<DocumentField> documentFields = new List<DocumentField>();


                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var documentField = new DocumentField
                             {
                                 DocumentFieldName = dr.SafeGetString("DocumentFieldName"),
                                 DocumentFieldValue = dr.SafeGetString("DocumentFieldValue"),
                                 IsMandatory = dr.SafeGetBoolean("IsMandatory")
                             };

                            documentFields.Add(documentField);
                        }
                    }

                    return documentFields;
                }
            }
        }

        public IEnumerable<Option> GetCompanies(int customerId, int? userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = userId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_Regions";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            yield return new Option
                            {
                                Id = dr.SafeGetInteger("Id"),
                                Key = dr.SafeGetString("SearchKey"),
                                Name = dr.SafeGetString("Region"),
                            };
                    }
                }
            }
        }

        public IEnumerable<Option> GetUnits(int customerId, int? userId, int? companyId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = userId;
                    command.Parameters.Add(new SqlParameter("@Region_Id", SqlDbType.Int)).Value = companyId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_Departments";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            yield return new Option
                            {
                                Id = dr.SafeGetInteger("Id"),
                                Key = dr.SafeGetString("SearchKey"),
                                Name = dr.SafeGetString("Department"),
                            };
                    }
                }
            }
        }

        public IEnumerable<OU> GetOUs(int? departmentId, int? parentOUId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Department_Id", SqlDbType.Int)).Value = departmentId;
                    command.Parameters.Add(new SqlParameter("@Parent_OU_Id", SqlDbType.Int)).Value = parentOUId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_OU";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return new OU
                            {
                                Id = dr.SafeGetInteger("Id"),
                                OUId = dr.SafeGetString("OUId"),
                                Name = dr.SafeGetString("OU"),
                                Status = dr.SafeGetInteger("Status"),
                                Code = dr.SafeGetString("Code") //Add by TAN 2015-11-17
                            };
                        }
                    }
                }
            }
        }

        public Company GetCompany(string searchKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Search_Key", SqlDbType.NVarChar)).Value = searchKey;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_Company";
                    connection.Open();

                    Company company = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            company = new Company
                            {
                                Searchkey = searchKey,
                                HeaderName = dr.SafeGetString("HeaderName"),
                                footerName = dr.SafeGetString("FooterName"),
                                EmployerName = dr.SafeGetString("EmployerName"),
                                CountryEmployer = dr.SafeGetString("PolishEmployer"),
                                Tel = dr.SafeGetString("Tel"),
                                Fax = dr.SafeGetString("Fax"),
                                NIP = dr.SafeGetString("NIP"),
                                Regon = dr.SafeGetString("Regon"),
                                KRSNo = dr.SafeGetString("KRSNo"),
                                KapitalNo = dr.SafeGetString("KapitalNo"),
                                INGBankNo = dr.SafeGetString("INGBankNo")

                            };
                        }
                    }

                    return company;
                }
            }

        }

        public Company GetCompanyByCode(int customerId, string code)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@Code", SqlDbType.NVarChar)).Value = code;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_RegionByCode";
                    connection.Open();

                    Company company = null;
                    
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            company = new Company
                            {
                                 Name = dr.SafeGetString("Region"),
                                 Id = dr.SafeGetInteger("Id")
                                
                            };
                        }
                    }

                    return company;
                }
            }

        }

        public Department GetDepartmentByCode(int customerId, string code)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.NVarChar)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@Code", SqlDbType.NVarChar)).Value = code;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_DepartmentByCode";
                    connection.Open();

                    Department department = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            department = new Department
                            {
                                Name = dr.SafeGetString("Department")
                            };
                        }
                    }

                    return department;
                }
            }
        }

        public Department GetDepartmentByKey(string searchKey, int customerId)
        {
            Department department = GetDepartmentBySearchKey(customerId, searchKey);

            if (department == null)
            {
                department = new Department();
                department.CloseDay = "";
                department.Extra = "";
                department.HeadOfDepartmentCity = "";
                department.HeadOfDepartmentName = "";
                department.HeadOfDepartmentSignature = "";
                department.HeadOfDepartmentTitle = "";
                department.HrManager = "";
                department.Name = "";
                department.Searchkey = "";
                department.StoreManager = "";
                department.StrAddr = "";
                department.TelNbr = "";
                department.Unit = "";

                return department;
            }

            return department;
        }

        public IDictionary<string, string> GetDepartment(string searchKey, int customerId)
        {
            Department department = GetDepartmentByKey(searchKey, customerId);

            var dictionary = new Dictionary<string, string>();

            PropertyInfo[] infos = department.GetType().GetProperties();

            foreach (PropertyInfo info in infos)
            {
                dictionary.Add(info.Name, info.GetValue(department, null).ToString());
            }

            return dictionary;
        }

        public Department GetDepartmentBySearchKey(int customerId, string searchKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@SearchKey", SqlDbType.NVarChar)).Value = searchKey;
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.CommandType = CommandType.StoredProcedure;

                    //Weird name on this stored procedure. Change this eventually. TODO
                    command.CommandText = "ECT_Get_DepartmentById";
                    connection.Open();

                    Department department = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            department = new Department
                            {
                                Searchkey = searchKey,
                                Name = dr.SafeGetString("Department"),
                                HeadOfDepartmentName = dr.SafeGetString("HeadOfDepartment"),
                                HeadOfDepartmentTitle = dr.SafeGetString("HeadOfDepartmentTitle"),
                                HeadOfDepartmentCity = dr.SafeGetString("HeadOfDepartmentCity"),
                                HeadOfDepartmentSignature = dr.SafeGetString("HeadOfDepartmentSignature"),
                                Unit = dr.SafeGetString("Unit"),
                                StrAddr = dr.SafeGetString("StrAddr"),
                                CloseDay = dr.SafeGetString("CloseDay"),
                                TelNbr = dr.SafeGetString("TelNbr"),
                                HrManager = dr.SafeGetString("HrManager"),
                                StoreManager = dr.SafeGetString("StoreManager"),
                                Extra = dr.SafeGetString("Extra")
                            };
                        }
                    }

                    return department;
                }
            }
        }

        

        public OU GetOuById(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_OU_By_Id";
                    connection.Open();

                    OU ou = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ou = new OU
                            {
                                Name = dr.SafeGetString("OU"),
                                Code = dr.SafeGetString("Code"),
                                Id = dr.SafeGetInteger("Id"),
                                OUId = dr.SafeGetString("OuId"),
                                Status = dr.SafeGetInteger("Status")
                            };
                        }
                    }

                    return ou;
                }
            }
        }



        public Department GetDepartmentById(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_DepartmentByActualId";
                    connection.Open();

                    Department department = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            department = new Department
                            {
                                Name = dr.SafeGetString("Department"),
                                HeadOfDepartmentName = dr.SafeGetString("HeadOfDepartment"),
                                HeadOfDepartmentTitle = dr.SafeGetString("HeadOfDepartmentTitle"),
                                HeadOfDepartmentCity = dr.SafeGetString("HeadOfDepartmentCity"),
                                HeadOfDepartmentSignature = dr.SafeGetString("HeadOfDepartmentSignature"),
                                Unit = dr.SafeGetString("Unit"),
                                StrAddr = dr.SafeGetString("StrAddr"),
                                CloseDay = dr.SafeGetString("CloseDay"),
                                TelNbr = dr.SafeGetString("TelNbr"),
                                HrManager = dr.SafeGetString("HrManager"),
                                StoreManager = dr.SafeGetString("StoreManager"),
                                Extra = dr.SafeGetString("Extra"),

                                StrAddr2 = dr.SafeGetString("StrAddr2"),
                                StrAddr3 = dr.SafeGetString("StrAddr3"),
                                StrAddr4 = dr.SafeGetString("StrAddr4"),
                                PostalCode = dr.SafeGetString("DepartmentPostalCode"),
                                City = dr.SafeGetString("DepartmentCity")
                            };
                        }
                    }

                    return department;
                }
            }
        }

        public int SaveNew(Guid formGuid, int caseId, int userId, string regUserId, int stateSecondaryId, int source, string languageId, string ipNumber, int? parentCaseId, IDictionary<string, string> formFields, string CreatedByUser)
        {
            int caseHistoryId = 0;
            Contract currentContract = null;
            StateSecondary currentStatesecondary = null;
            int externalTime = 0;
            int actionExternalTime = 0;
            int actionLeadTime = 0;
            int historyLeadTime = 0;


            DateTime? latestSLACountDate = null;

            Form f = GetFormByGuid(formGuid);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                int? oldSubStatus = null;
                int? newSubStatus = null;
                DateTime? oldSLADate = null;


                Contract oldContract = null;
                if (caseId != 0)
                {                    
                    oldContract = Get(caseId);
                    oldSubStatus = oldContract != null? (int?) oldContract.StateSecondaryId : null;
                    oldSLADate = oldContract != null? oldContract.LatestSLACountDate : null;
                }
                
                newSubStatus = stateSecondaryId;
                latestSLACountDate = CalculateLatestSLACountDate(oldSubStatus, newSubStatus, oldSLADate, f.CustomerId);

                Customer c = GetCustomer(f.CustomerId);
                var customerTimeOffset = c.Setting_TimeZone_offset;
                
                var customerOffsetInHour = Convert.ToInt32(customerTimeOffset/60);               

                if (caseId != 0)
                {
                    currentContract = oldContract;
                     TimeSpan offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);

                    if (currentContract.StateSecondaryId != 0)
                    {
                        currentStatesecondary = GetStateSecondary(currentContract.CustomerId, currentContract.StateSecondaryId);


                        if (currentStatesecondary.IncludeInCaseStatistics == 0)
                        {
                            Customer customer = GetCustomer(currentContract.CustomerId);
                           
                            externalTime = leadTimeMinutes(getLocalTime(currentContract.ChangeTime, offset.Hours), getLocalTime(DateTime.UtcNow, offset.Hours), customer.WorkingDayStart, customer.WorkingDayEnd, currentContract.HolidayHeader_Id);
                            actionExternalTime = leadTimeMinutes(getLocalTime(oldContract.ChangeTime, customerOffsetInHour), getLocalTime(DateTime.UtcNow, customerOffsetInHour), c.WorkingDayStart, c.WorkingDayEnd, oldContract.HolidayHeader_Id);
                        }
                        else
                        {
                            actionLeadTime = leadTimeMinutes(getLocalTime(oldContract.ChangeTime, customerOffsetInHour), getLocalTime(DateTime.UtcNow, customerOffsetInHour), c.WorkingDayStart, c.WorkingDayEnd, oldContract.HolidayHeader_Id);                           
                        }
                    }
                  // S.G : add this when calculation for Lead time has been done.
                    //historyLeadTime = leadTimeMinutes(getLocalTime(oldContract.RegTime, offset.Hours), getLocalTime(DateTime.UtcNow, offset.Hours), c.WorkingDayStart, c.WorkingDayEnd, oldContract.HolidayHeader_Id);                           
                                     
                }

                

                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@FormGUID", SqlDbType.UniqueIdentifier)).Value = formGuid;
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                    command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = userId;
                    command.Parameters.Add(new SqlParameter("@StateSecondaryId", SqlDbType.Int)).Value = stateSecondaryId;
                    command.Parameters.Add(new SqlParameter("@IPAddress", SqlDbType.NVarChar)).Value = ipNumber;
                    command.Parameters.Add(new SqlParameter("@RegUserId", SqlDbType.NVarChar)).Value = regUserId;
                    command.Parameters.Add(new SqlParameter("@RegLanguageId", SqlDbType.NVarChar)).Value = languageId;
                    command.Parameters.Add(new SqlParameter("@RegistrationSource", SqlDbType.Int)).Value = source;
                    command.Parameters.Add(new SqlParameter("@ExternalTime", SqlDbType.Int)).Value = externalTime;
                    command.Parameters.Add(new SqlParameter("@ParentCaseId", SqlDbType.Int)).Value = parentCaseId;
                    command.Parameters.Add(new SqlParameter("@RegUserName", SqlDbType.NVarChar)).Value = CreatedByUser;
                    command.Parameters.Add(new SqlParameter("@LatestSLACountDate", SqlDbType.DateTime)).Value = latestSLACountDate;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_CaseNew";

                    caseId = (int)command.ExecuteScalar();
                }

                if (caseId != 0)
                {
                    using (var command = connection.CreateCommand())
                    {
                        foreach (var dictonary in formFields)
                        {
                            if (dictonary.Key != "MyXMLElements")
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                                command.Parameters.Add(new SqlParameter("@FormGUID", SqlDbType.UniqueIdentifier)).Value = formGuid;
                                command.Parameters.Add(new SqlParameter("@FormFieldName", SqlDbType.NVarChar)).Value = dictonary.Key;
                                command.Parameters.Add(new SqlParameter("@FormFieldValue", SqlDbType.NVarChar)).Value = dictonary.Value;

                                command.Parameters.Add(new SqlParameter("@RegUserId", SqlDbType.NVarChar)).Value = regUserId;

                                if (userId != 0)
                                {
                                    command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = userId;
                                }


                                command.CommandText = "ECT_Save_FormFieldValue";
                                command.CommandType = CommandType.StoredProcedure;

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }

                var contract = Get(caseId);

                

                // SET WatchDate
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = contract.Id;

                    if (currentStatesecondary != null)
                    {
                        command.Parameters.Add(new SqlParameter("@Old_StateSecondary_Id", SqlDbType.Int)).Value = currentStatesecondary.Id;
                    }

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_Case_WatchDate";

                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                    command.Parameters.Add(new SqlParameter("@CreatedByUser", SqlDbType.NVarChar)).Value = CreatedByUser;
                    command.Parameters.Add(new SqlParameter("@ActionExternalTime", SqlDbType.Int)).Value = actionExternalTime;
                    command.Parameters.Add(new SqlParameter("@ActionLeadTime", SqlDbType.Int)).Value = actionLeadTime;
                    command.Parameters.Add(new SqlParameter("@LeadTime", SqlDbType.Int)).Value = historyLeadTime;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_CaseHistory";

                    caseHistoryId = (int)command.ExecuteScalar();
                }

                ////////
                // BY AC
                //FORMFIELDVALUE HISTORY
                if (caseId != 0)
                {
                    using (var command = connection.CreateCommand())
                    {
                        foreach (var dictonary in formFields)
                        {
                            if (dictonary.Key != "MyXMLElements")
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                                command.Parameters.Add(new SqlParameter("@FormGUID", SqlDbType.UniqueIdentifier)).Value = formGuid;
                                command.Parameters.Add(new SqlParameter("@FormFieldName", SqlDbType.NVarChar)).Value = dictonary.Key;
                                command.Parameters.Add(new SqlParameter("@FormFieldValue", SqlDbType.NVarChar)).Value = dictonary.Value;
                                command.Parameters.Add(new SqlParameter("@CaseHistory_Id", SqlDbType.Int)).Value = caseHistoryId;
                                command.CommandText = "ECT_Save_FormFieldValueHistory";
                                command.CommandType = CommandType.StoredProcedure;

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }

                

                if (caseId != 0)
                {
                        using (var command = connection.CreateCommand())
                        {
                            command.Parameters.Clear();
                            var LogNoteExist = formFields.Keys.Contains("InternalLogNote");
                            var lognote = "";
                            if (LogNoteExist)
                            {
                                lognote = formFields["InternalLogNote"];
                                if (String.IsNullOrWhiteSpace(lognote))
                                {
                                    lognote = "";
                                }
                            }
                            command.Parameters.Add(new SqlParameter("@LogNoteText", SqlDbType.NVarChar)).Value = lognote;
                            command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                            command.Parameters.Add(new SqlParameter("@CaseHistory_Id", SqlDbType.Int)).Value = caseHistoryId;
                            command.Parameters.Add(new SqlParameter("@RegUserId", SqlDbType.NVarChar)).Value = regUserId;
                            if (userId != 0)
                            {
                                command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = userId;
                            }
                            
                            command.CommandText = "ECT_Save_LogNote";
                            command.CommandType = CommandType.StoredProcedure;

                            command.ExecuteNonQuery();
                        }
                }
                contract = Get(caseId);
                var globalsettings = GetGlobalSettings();
                var statesecondary = GetStateSecondary(contract.CustomerId, contract.StateSecondaryId);

                // Check if case closed
                if (contract.FinishingDate != null)
                {
                    
                    // Save Leadtime
                    Customer customer = GetCustomer(contract.CustomerId);

                    DateTime? FinishingDate = contract.FinishingDate;
                    DateTime? WatchDate = contract.WatchDate;


                    int iLeadTime = getLeadTime(contract.SolutionTime, contract.RegTime, WatchDate, FinishingDate, customer.WorkingDayStart, customer.WorkingDayEnd, contract.ExternalTime, contract.HolidayHeader_Id);

                    SaveCaseLeadTime(contract.Id, iLeadTime);

                    // To save to the case statistic table
                    var SLA = contract.SolutionTime;

                    var baseCalculationTime = contract.FinishingDate.Value;
                    Customer cs = GetCustomer(contract.CustomerId);
                    if (cs != null && cs.Setting_CalcSolvedInTimeByLatestSLADate != 0 && contract.LatestSLACountDate.HasValue)
                        baseCalculationTime = contract.LatestSLACountDate.Value;

                    var wasSolvedInTime = ResolveIsSolvedInTime(WatchDate, baseCalculationTime, SLA, iLeadTime);
                    SaveCaseStatistics(contract.Id, wasSolvedInTime.Value);

                }

                if (contract.WorkingGroupId != 0 && statesecondary.MailId != 0 && (currentContract == null || currentContract.WorkingGroupId != contract.WorkingGroupId))
                {

                    var mailtemplate = GetMail(statesecondary.MailId, contract.CustomerId, contract.LanguageId, contract.Id);
                    var messageId = CreateMessageId(contract.InitiatorEMail);

                    IEnumerable<User> users = GetUsersInWorkingGroup(contract.WorkingGroupId);

                    if (users != null)
                    {
                        foreach (User user in users)
                        {
                            if (UserHasPermissionToCase(contract.Id, user.Id) == 1)
                            {
                                if (user.Email != "")
                                {
                                    var ret = SendEmail(globalsettings.SmtpServer, "25", contract.HelpdeskEMail, user.Email, mailtemplate.Subject, mailtemplate.Body, messageId);

                                    if (ret == string.Empty)
                                    {
                                        using (var command = connection.CreateCommand())
                                        {
                                            command.Parameters.Add(new SqlParameter("@EMailAddress", SqlDbType.NVarChar)).Value = user.Email;
                                            command.Parameters.Add(new SqlParameter("@MailId", SqlDbType.Int)).Value = mailtemplate.MailId;
                                            command.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.NVarChar)).Value = messageId;
                                            command.Parameters.Add(new SqlParameter("@CaseHistory_Id", SqlDbType.Int)).Value = caseHistoryId;
                                            command.CommandType = CommandType.StoredProcedure;
                                            command.CommandText = "ECT_Save_EMailLog";
                                            //command.Transaction = transaction;
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else if (contract.WorkingGroupId != 0 && (currentContract == null || currentContract.WorkingGroupId != contract.WorkingGroupId))
                {
                    var mailtemplate = GetMail(7, contract.CustomerId, contract.LanguageId, contract.Id);

                    IEnumerable<User> users = GetUsersInWorkingGroup(statesecondary.WorkingGroupId);

                    if (users != null)
                    {
                        foreach (User user in users)
                        {
                            if (UserHasPermissionToCase(contract.Id, user.Id) == 1)
                            {
                                if (user.Email != "")
                                {
                                    var messageId = CreateMessageId(user.Email);

                                    var ret = SendEmail(globalsettings.SmtpServer, "25", contract.HelpdeskEMail, user.Email, mailtemplate.Subject, mailtemplate.Body, messageId);

                                    if (ret == string.Empty)
                                    {
                                        using (var command = connection.CreateCommand())
                                        {
                                            command.Parameters.Add(new SqlParameter("@EMailAddress", SqlDbType.NVarChar)).Value = user.Email;
                                            command.Parameters.Add(new SqlParameter("@MailId", SqlDbType.Int)).Value = mailtemplate.MailId;
                                            command.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.NVarChar)).Value = messageId;
                                            command.Parameters.Add(new SqlParameter("@CaseHistory_Id", SqlDbType.Int)).Value = caseHistoryId;
                                            command.CommandType = CommandType.StoredProcedure;
                                            command.CommandText = "ECT_Save_EMailLog";
                                            //command.Transaction = transaction;
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }


                }

                return caseId;
            }
        }


        public DateTime? CalculateLatestSLACountDate(int? oldSubStateId, int? newSubStateId, DateTime? oldSLADate , int customerId)
        {
            DateTime? ret = null;
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            var oldSubStateMode = -1;
            var newSubStateMode = -1;

            if (oldSubStateId.HasValue)
            {
                var oldSubStatus = GetStateSecondary(customerId, oldSubStateId.Value);
                if (oldSubStatus != null)
                    oldSubStateMode = oldSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (newSubStateId.HasValue)
            {
                var newSubStatus = GetStateSecondary(customerId, newSubStateId.Value);
                if (newSubStatus != null)
                    newSubStateMode = newSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (oldSubStateMode == -1 && newSubStateMode == -1)
                ret = null;
            else if (oldSubStateMode == -1 && newSubStateMode == 1)
                ret = null;
            else if (oldSubStateMode == 0 && newSubStateMode == 1)
                ret = null;
            else if (oldSubStateMode == 0 && newSubStateMode == -1)
                ret = null;
            else if (oldSubStateMode == -1 && newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode == 1 && newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode == 1 && newSubStateMode == -1)
                ret = oldSLADate;
            else if (oldSubStateMode == 1 && newSubStateMode == 1)
                ret = oldSLADate;
            else if (oldSubStateMode == 0 && newSubStateMode == 0)
                ret = oldSLADate;

            return ret;
        }

        private int? ResolveIsSolvedInTime(DateTime? watchDate, DateTime? finishDate, int SLA, int leadTime)
        {
            int? res = null;

            if (watchDate.HasValue)
            {
                res = ConvertToInt(ConvertToDay(finishDate.Value) <= ConvertToDay(watchDate.Value));
            }
            else if (SLA != 0)
            {
                res = ConvertToInt(leadTime <= SLA);
            }

            return res;
        }

        private DateTime ConvertToDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        private int ConvertToInt(bool value)
        {
            return value ? 1 : 0;
        }
        private int UserHasPermissionToCase(int Case_Id, int User_Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = Case_Id;
                    command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = User_Id;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_UserHasPermissionToCase";
                    connection.Open();

                    var result = command.ExecuteScalar();

                    return (int)result;

                }
            }
        }

        public void SaveFileViewLog(FileViewLog fileViewLog)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@caseId", SqlDbType.Int)).Value = fileViewLog.CaseId;
                    command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int)).Value = fileViewLog.UserId;
                    command.Parameters.Add(new SqlParameter("@fileName", SqlDbType.NVarChar)).Value = fileViewLog.FileName;
                    command.Parameters.Add(new SqlParameter("@filePath", SqlDbType.NVarChar)).Value = fileViewLog.FilePath;
                    command.Parameters.Add(new SqlParameter("@fileSource", SqlDbType.Int)).Value = 3;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_FileViewLog";
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public void SaveCaseFile(CaseFile caseFile)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@caseId", SqlDbType.Int)).Value = caseFile.CaseId;
                    command.Parameters.Add(new SqlParameter("@fileName", SqlDbType.NVarChar)).Value = caseFile.FileName;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_CaseFile";
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        private void SaveCaseLeadTime(int case_Id, int LeadTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = case_Id;
                    command.Parameters.Add(new SqlParameter("@LeadTime", SqlDbType.Int)).Value = LeadTime;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_Case_LeadTime";
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        private void SaveCaseStatistics(int case_Id, int wasSolvedInTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {            

                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = case_Id;
                    command.Parameters.Add(new SqlParameter("@WasSolvedInTime", SqlDbType.Int)).Value = wasSolvedInTime;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Save_CaseStatistics";
                    connection.Open();

                    command.ExecuteNonQuery();
                }

            }
        }

        public void DeleteCaseFile(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Delete_CaseFile";
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public CaseFile GetCaseFile(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_CaseFile";

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return new CaseFile
                            {
                                Id = dr.SafeGetInteger("Id"),
                                CaseId = dr.SafeGetInteger("Case_Id"),
                                FileName = dr.SafeGetString("FileName"),
                                CreatedDate = dr.SafeGetDateTime("CreatedDate")
                            };
                        }
                    }

                    return null;
                }
            }
        }

        public IEnumerable<CaseFile> GetCaseFiles(int caseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@caseId", SqlDbType.Int)).Value = caseId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_CaseFile";

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return new CaseFile
                            {
                                Id = dr.SafeGetInteger("Id"),
                                CaseId = dr.SafeGetInteger("Case_Id"),
                                FileName = dr.SafeGetString("FileName"),
                                CreatedDate = dr.SafeGetDateTime("CreatedDate")
                            };
                        }
                    }
                }
            }
        }

        public GlobalSettings GetGlobalSettings()
        {
            var cache = new CacheProvider();
            var globalSettings = cache.Get(Constants.Cache.GlobalSettings) as GlobalSettings;

            if (globalSettings != null)
                return globalSettings;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_GlobalSettings";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            globalSettings = new GlobalSettings
                            {
                                SmtpServer = dr.SafeGetString("SMTPServer"),
                                AttachedFileFolder = dr.SafeGetString("AttachedFileFolder")
                            };
                        }
                    }

                    if (globalSettings != null)
                        cache.Set(Constants.Cache.GlobalSettings, globalSettings, Constants.Cache.CacheTime);

                    return globalSettings;
                }
            }
        }

        public string GetFolderPath(string caseNumber)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@CaseNumber", SqlDbType.Int)).Value = Convert.ToUInt32(caseNumber);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_FolderPath";
                    connection.Open();

                    var folder = "";

                    using (var dr = command.ExecuteReader())
                    {
                        dr.Read();
                        folder = dr.SafeGetString("Folder");
                    }

                    return folder;
                }
            }
        }


     

        public string GetSiteUrl(string caseNumber, string currentUrl)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@CaseNumber", SqlDbType.Int)).Value = Convert.ToUInt32(caseNumber);
                    command.Parameters.Add(new SqlParameter("@CurrentUrl", SqlDbType.NVarChar)).Value = currentUrl;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_BaseUrl";
                    connection.Open();

                    var siteUrl = "";

                    using (var dr = command.ExecuteReader())
                    {
                        dr.Read();
                        siteUrl = dr.SafeGetString("SiteUrl");
                    }

                    return siteUrl;
                }
            }
        }


        private IEnumerable<int> GetChildCaseNumbers(int parentCaseNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@parentCaseNumber", SqlDbType.Int)).Value = parentCaseNumber;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select CaseNumber from tblCase where RelatedCaseNumber = @parentCaseNumber";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return int.Parse(dr.SafeGetString("CaseNumber"));
                        }
                    }
                }
            }
        }

        private IEnumerable<CaseHistory> GetCaseHistory(int caseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_CaseHistory";

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return new CaseHistory
                            {
                                Id = dr.SafeGetInteger("Id"),
                                StateSecondaryId = dr.SafeGetInteger("StateSecondaryId")
                            };
                        }
                    }
                }
            }
        }

        private StateSecondary GetStateSecondary(int customerId, int status)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@StateSecondaryId", SqlDbType.Int)).Value = status;
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_StateSecondaryById";
                    connection.Open();

                    StateSecondary statesecondary = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            statesecondary = new StateSecondary
                            {
                                Id = dr.SafeGetInteger("Id"),
                                StateSecondaryId = dr.SafeGetInteger("StateSecondaryId"),
                                WorkingGroupId = dr.SafeGetInteger("WorkingGroup_Id"),
                                //InitiatorMailTemplateId = dr.SafeGetInteger("MailTemplate_Id"),
                                IncludeInCaseStatistics = dr.SafeGetInteger("IncludeInCaseStatistics"),
                                MailId = dr.SafeGetInteger("MailId"),
                                RecalculateWatchDate = dr.SafeGetInteger("RecalculateWatchDate")
                            };
                        }
                    }

                    return statesecondary;
                }
            }
        }

        private Customer GetCustomer(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_CustomerById";
                    connection.Open();

                    Customer customer = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            customer = new Customer
                            {
                                Id = dr.SafeGetInteger("Id"),
                                Name = dr.SafeGetString("Name"),
                                WorkingDayStart = dr.SafeGetInteger("WorkingDayStart"),
                                WorkingDayEnd = dr.SafeGetInteger("WorkingDayEnd"),
                                Setting_CalcSolvedInTimeByLatestSLADate = dr.SafeGetInteger("CalcSolvedInTimeByLatestSLADate"),
                                Setting_TimeZone_offset = dr.SafeGetInteger("TimeZone_offset")
                            };
                        }
                    }

                    return customer;
                }
            }
        }

        private ProductArea GetProductArea(int ProductAreaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@ProductArea_Id", SqlDbType.Int)).Value = ProductAreaId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_ProductAreaById";
                    connection.Open();

                    ProductArea productarea = null;

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            productarea = new ProductArea
                            {
                                Id = dr.SafeGetInteger("Id"),
                                Name = dr.SafeGetString("ProductArea"),
                                DocumentPath = dr.SafeGetString("DocumentPath")
                            };
                        }
                    }

                    return productarea;
                }
            }
        }



        private MailTemplate GetMail(int mailID, int customerId, int languageId, int caseId)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            MailTemplate mailtemplate = null;

            using (var command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@MailId", SqlDbType.Int)).Value = mailID;
                command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                command.Parameters.Add(new SqlParameter("@Language_Id", SqlDbType.Int)).Value = languageId;
                command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "ECT_Get_MailByMailId";

                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        mailtemplate = new MailTemplate
                        {
                            Id = dr.SafeGetInteger("Id"),
                            Customer_Id = dr.SafeGetInteger("Customer_Id"),
                            MailId = dr.SafeGetInteger("MailId"),
                            Subject = dr.SafeGetString("Subject"),
                            Body = dr.SafeGetString("Body")
                        };
                    }
                }
            }

            if (connection != null)
                connection.Close();

            return mailtemplate;
        }

        private string SendEmail(string smtpServer, string smtpPort, string from, string to, string subject, string body, string messageId)
        {
            var strRet = string.Empty;

            try
            {
                int port;
                var smtpClient = int.TryParse(smtpPort, out port) ? new SmtpClient(smtpServer, port) : new SmtpClient(smtpServer);

                var msg = new MailMessage();

                msg.To.Add(new MailAddress(to));

                msg.Subject = subject;
                msg.From = new MailAddress(from);
                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.Body = body;
                msg.Headers.Add("Message-ID", messageId);

                smtpClient.UseDefaultCredentials = true;

                smtpClient.Send(msg);

            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }

            return strRet;

        }

        private string CreateMessageId(string email)
        {
            var sTemp0 = "<" + DateTime.Now.Year;
            sTemp0 = sTemp0 + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            sTemp0 = sTemp0 + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            sTemp0 = sTemp0 + DateTime.Now.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            sTemp0 = sTemp0 + DateTime.Now.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            sTemp0 = sTemp0 + DateTime.Now.Second.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            var sTemp1 = Guid.NewGuid().ToString();

            var sTemp2 = email.Replace("@", ".") + ">";

            return sTemp0 + sTemp1 + "@" + sTemp2;
        }

        private IEnumerable<User> GetUsersInWorkingGroup(int workinggroupId)
        {
            List<User> users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@WorkingGroup_Id", SqlDbType.Int)).Value = workinggroupId;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_UsersInWorkingGroup";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            User user = new User();

                            user.Id = dr.SafeGetInteger("Id");
                            user.UserId = dr.SafeGetString("UserId");
                            user.FirstName = dr.SafeGetString("FirstName");
                            user.Surname = dr.SafeGetString("SurName");
                            user.Email = dr.SafeGetString("Email");

                            users.Add(user);

                        }
                    }
                }
            }

            return users;
        }

        private int getLeadTime(int iSolutionTime, DateTime RegTime, DateTime? WatchDate, DateTime? FinishingDate, int WorkingDayStart, int WorkingDayEnd, int ExternalTime, int HolidayHeader_Id)
        {
            int iLeadTime = 0;

            if (iSolutionTime > 0 || WatchDate != null)
            {
                if (WatchDate != null)
                {

                    iLeadTime = leadTimeMinutes(WatchDate ?? DateTime.Now, FinishingDate ?? DateTime.Now, WorkingDayStart, WorkingDayEnd, HolidayHeader_Id);
                }
                else
                {
                    iLeadTime = leadTimeMinutes(RegTime, FinishingDate ?? DateTime.Now, WorkingDayStart, WorkingDayEnd, HolidayHeader_Id);

                }

                iLeadTime = iLeadTime - ExternalTime;

                // Timmar
                iLeadTime = iLeadTime / 60;

            }

            return iLeadTime;

        }

        private int leadTimeMinutes(DateTime Startdate, DateTime Enddate, int WorkingDayStart, int WorkingDayEnd, int HolidayHeader_Id)
        {
            int iLeadTime = 0;
            int iLeadTimeMinutes = 0;
            DateTime StartdateTemp;
            DateTime EndDateTemp;

            IList<Holiday> holidays = GetHolidays(HolidayHeader_Id);

            int days = ((TimeSpan)(Enddate.Date - Startdate.Date)).Days;

            // Räkna ut antalet hela dagar
            if (days > 1)
            {
                StartdateTemp = Startdate;

                //    Räkna ut antalet hela dagar
                for (int i = 1; i < days; i++)
                {
                    StartdateTemp = StartdateTemp.AddDays(1);

                    if (!isHoliday(StartdateTemp, holidays))
                    {

                        if ((int)StartdateTemp.DayOfWeek > 0 && (int)StartdateTemp.DayOfWeek < 6)
                        {
                            iLeadTime = iLeadTime + 1;
                        }

                    }

                }

                iLeadTime = (iLeadTime * (WorkingDayEnd - WorkingDayStart)) * 60;
            }


            // Räkna antalet minuter dag 1
            if (!isHoliday(Startdate, holidays))
            {
                if (Startdate.TimeOfDay.Hours >= WorkingDayStart)
                {
                    StartdateTemp = Startdate;
                }
                else
                {
                    StartdateTemp = Startdate.Date.AddHours(WorkingDayStart);
                }

                if (Startdate.Date != Enddate.Date)
                {
                    if (WorkingDayEnd == 24)
                    {
                        EndDateTemp = Startdate.Date.AddDays(1);
                    }
                    else
                    {
                        EndDateTemp = Startdate.Date.AddHours(WorkingDayEnd);
                    }
                }
                else
                {
                    if (Enddate < Enddate.Date.AddHours(WorkingDayEnd))
                    {
                        EndDateTemp = Enddate;
                    }
                    else
                    {
                        EndDateTemp = Enddate.Date.AddHours(WorkingDayEnd);
                    }
                }

                if ((int)StartdateTemp.DayOfWeek > 0 && (int)StartdateTemp.DayOfWeek < 6)
                {
                    iLeadTimeMinutes = ((TimeSpan)(EndDateTemp - StartdateTemp)).Minutes + (((TimeSpan)(EndDateTemp - StartdateTemp)).Hours * 60);
                }

            }

            if (iLeadTimeMinutes < 0)
                iLeadTimeMinutes = 0;

            iLeadTime = iLeadTime + iLeadTimeMinutes;

            // Räkna antalet minuter avslutsdagen
            if (Startdate.Date != Enddate.Date)
            {
                iLeadTimeMinutes = 0;

                if (!isHoliday(Enddate, holidays))
                {
                    StartdateTemp = Enddate.Date.AddHours(WorkingDayStart);

                    if (Enddate < Enddate.Date.AddHours(WorkingDayEnd))
                    {
                        EndDateTemp = Enddate;
                    }
                    else
                    {
                        EndDateTemp = Enddate.Date.AddHours(WorkingDayEnd);
                    }

                    if ((int)StartdateTemp.DayOfWeek > 0 && (int)StartdateTemp.DayOfWeek < 6)
                    {
                        iLeadTimeMinutes = ((TimeSpan)(EndDateTemp - StartdateTemp)).Minutes + (((TimeSpan)(EndDateTemp - StartdateTemp)).Hours * 60);
                    }

                }

                if (iLeadTimeMinutes < 0)
                    iLeadTimeMinutes = 0;

                iLeadTime = iLeadTime + iLeadTimeMinutes;
            }

            return iLeadTime;

        }

        private bool isHoliday(DateTime dtDate, IList<Holiday> holidays)
        {
            if (holidays != null)
            {
                foreach (Holiday h in holidays)
                {
                    if (h.HolidayDate.ToShortDateString() == dtDate.ToShortDateString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private IList<Holiday> GetHolidays(int holidayHeader_Id)
        {
            IList<Holiday> holidays = new List<Holiday>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@HolidayHeader_Id", SqlDbType.Int)).Value = holidayHeader_Id;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_Holidays";

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Holiday holiday = new Holiday
                            {
                                Id = dr.SafeGetInteger("Id"),
                                HolidayHeader_Id = dr.SafeGetInteger("HolidayHeader_Id"),
                                HolidayDate = dr.SafeGetDateTime("Holiday")
                            };

                            holidays.Add(holiday);
                        }
                    }
                }
            }

            return holidays;
        }

        private DateTime getLocalTime(DateTime time, int timeZoneOffset)
        {
            return time.AddHours(timeZoneOffset);
        }

        public IList<StaticFile> GetStaticDocuments(int productAreaId)
        {
            IList<StaticFile> files = new List<StaticFile>();

            try
            {
                ProductArea productarea = GetProductArea(productAreaId);

                if (productarea != null)
                {
                    if (productarea.DocumentPath != "")
                    {
                        DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/" + productarea.DocumentPath));

                        var fs = dir.GetFiles("*.*");

                        foreach (FileInfo f in fs)
                        {
                            StaticFile staticfile = new StaticFile
                            {
                                Name = f.Name,
                                Extension = f.Extension,
                                URL = "~/" + productarea.DocumentPath + "//" + f.Name
                            };

                            files.Add(staticfile);
                        }
                    }
                }

            }
            catch
            {
                return null;
            }

            return files;
        }
    }
}