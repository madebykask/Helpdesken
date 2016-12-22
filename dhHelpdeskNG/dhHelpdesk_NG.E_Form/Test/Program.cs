namespace ECTReport
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Program
    {
        public static void Main(string[] args)
        {
            int customerId = 0;
            string countryName = string.Empty;
            string folder = string.Empty;
            string customDate = string.Empty;
            DateTime? customD = null;


            if(args.Length < 3)
            {
                return;
            }

            if(!int.TryParse(args[0], out customerId))
            {
                return;
            }

            countryName = args[1];
            folder = args[2];
            customDate = args[3];

            if (!string.IsNullOrEmpty(customDate))
                //customD = DateTime.ParseExact(customDate, "YYYY-MM-DD", CultureInfo.InvariantCulture);               
                customD = Convert.ToDateTime(customDate);
            try
            {
                var date = DateTime.Now;
                var dateWithFormat = date.Year + "-" + date.Month + "-" + date.Day;
                var reportName = dateWithFormat + "-" + countryName;
                var path = ConfigurationManager.AppSettings["LocationPath"];
                path = Path.Combine(AddSlash(path), AddSlash(folder));

                var extension = ConfigurationManager.AppSettings["FileExtension"];

                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using(var fs = new FileStream(path + reportName + extension, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using(var writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        var rep = new ReportRepository(ConfigurationManager.ConnectionStrings["DSN"].ConnectionString);
                        var arr = new List<List<string>>();

                        var headerCaptions = rep.GetColumnNames().GetRange(0, 22).ToList();
                        var cache = rep.GetCaseFormData(customerId, customD).ToList();
                        var formFieldName = rep.GetFormColumnNames(customerId).ToList();
                        //var formFieldName = cache.Select(x => x.FormFieldName).Distinct().ToList();

                        headerCaptions.AddRange(formFieldName);
                        arr.Add(headerCaptions);

                        var caseValue = rep.GetCaseHelpDeskData(customerId, customD).ToList();
                        var caseIds = cache.Select(x => new { x.CaseId, x.CaseNumber }).Distinct().ToArray();

                        for(var i = 0; i < caseIds.Length; i++)
                        {
                            var val = caseValue.FirstOrDefault(c => c.CaseNumber == caseIds[i].CaseNumber);
                            if(val != null)
                            {
                                var answer = new List<string>(headerCaptions.Count);
                                answer.Add(val.CaseNumber);
                                answer.Add(val.EmployeeNumber);
                                answer.Add(val.FirstName);
                                answer.Add(val.LastName);
                                answer.Add(val.DateOfBirth);
                                answer.Add(val.SubProcess);
                                answer.Add(val.MainProcess);
                                answer.Add(val.Company);
                                answer.Add(val.Unit);
                                answer.Add(val.Department);
                                answer.Add(val.Function);
                                answer.Add(val.Status);
                                answer.Add(val.NumberOFRecords);
                                answer.Add(val.WorkingGroup);
                                answer.Add(val.AdminName);
                                answer.Add(val.AdminLastName);
                                answer.Add(val.RegisteredByUser);
                                answer.Add(val.SLA);
                                answer.Add(val.Eyequality);
                                answer.Add(val.RegisterTime.ToShortDateString().Replace("-", "."));
                                answer.Add(val.FinishingDate.ToShortDateString().Replace("-", "."));
                                answer.Add(val.EffectiveDate.ToShortDateString().Replace("-", "."));

                                for(int j = 0; j < formFieldName.Count; j++)
                                {
                                    var tmpAnwer = cache.FirstOrDefault(x => x.CaseId == caseIds[i].CaseId && x.FormFieldName == formFieldName[j]);
                                    answer.Add(tmpAnwer == null ? string.Empty : tmpAnwer.FormFieldValue.Replace(";", string.Empty).Replace(Environment.NewLine, string.Empty));
                                }

                                arr.Add(answer);
                            }
                        }

                        for(int i = 0; i < arr.Count; i++)
                        {
                            for(int j = 0; j < arr[i].Count; j++)
                            {
                                writer.Write(arr[i][j] + ";");
                            }

                            writer.Write(Environment.NewLine);
                        }

                        writer.Close();
                    }
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static string AddSlash(string s)
        {
            if(string.IsNullOrEmpty(s))
                throw new ArgumentNullException(s);

            if(!s.EndsWith("\\"))
                s = s + "\\";

            return s;
        }

        static List<string> PolandSortOrder()
        {
            return new List<string> { "EmployeeNumber", "EmployeeFirstName", "EmployeeLastName", "Company", "Unit", "EffectiveDate", "EndDate", "InternalLogNote", "LastDayAtWork", "TerminationInitiatedBy", "ReasonForTermination", "MedicoverContinuation", "RemainingOvertime", "RemainingVacation", "OpenDeduction", "RemainingAmount", "MailingAddress", "Street", "StreetNumber", "HouseNumber", "City", "PostalCode", "Country", "CreateAnnualIncomeStatement", "CreateStatementRP7ForEmploymentPeriod", "Region", "District", "Community", "SubmittedToExternalBody", "lstWhoMadeMistake", "lstErrorType", "lstErrorConcerns", "lstCorrection", "DataChangeSelection", "TaxOfficeLocation", "Prefix", "PensionerStatus", "ContractType", "FunctionAllowance", "FunctionAllowanceAmount", "FunctionAllowanceGrossNett", "AdditionalPayment", "AdditionalPaymentAmount", "AdditionalPaymentGrossNett", "RecurringPaymentsAndDeductions", "RecurringPaymentsAndDeductionsAmount", "RecurringPaymentsAndDeductionsGrossNett", "OneIKEABonusType", "FunctionAllowance2", "FunctionAllowanceAmount2", "FunctionAllowanceGrossNett2", "FunctionAllowance3", "FunctionAllowanceAmount3", "FunctionAllowanceGrossNett3", "RecurringPaymentsAndDeductions2", "RecurringPaymentsAndDeductionsAmount2", "RecurringPaymentsAndDeductionsGrossNett2", "RecurringPaymentsAndDeductions3", "RecurringPaymentsAndDeductionsAmount3", "RecurringPaymentsAndDeductionsGrossNett3", "AdditionalPayment2", "AdditionalPaymentAmount2", "AdditionalPaymentGrossNett2", "AdditionalPayment3", "AdditionalPaymentAmount3", "AdditionalPaymentGrossNett3", "AccountNo", "AccountHolder", "NameOfBank", "AccountNo2", "AccountHolder2", "NameOfBank2", "AmountToBeTransferred", "AmountToBeTransferred2", "AccountNo3", "AccountHolder3", "NameOfBank3", "AmountToBeTransferred3", "ECMCompensationProgram", "PSArea", "PSGroup", "CollectiveScaleSalary", "MonthlyHourly", "BasicPay", "BasicPayInWords", "BasicPayInWordsPolish", "PostCode", "JobTitle", "PolishJobTitle", "Extent", "Workload", "PersonReplaced", "FirstDayAtWork", "TwoWeekNotice", "AdditionalPlaceOfWork", "WorkingTimeSchedule", "Department", "HomeCostCenter", "HomeCostCenterAllocation", "AdditionalCostCenter", "AdditionalCostCenterAllocation", "AdditionalCostCenter2", "AdditionalCostCenterAllocation2", "LineManager", "IdentityCardNo", "IdentityCardIssueAuthority", "IdentityCardIssueDate", "ContractConcludedOn", "ContractPolishConcludedOn", "AppendixConcludedOn", "AppendixPolishConcludedOn", "ContractPolish", "AppendixPolish", "EmployeeSource", "TypeOfHiring", "CountryCode", "SecondName", "FathersFirstName", "MothersFirstName", "MothersBirthName", "DateOfBirth", "PlaceOfBirth", "Nationality", "CommunicationLanguage", "Gender", "PESEL", "NIP", "Phone", "Email", "HomeStreet", "HomeStreetNumber", "HomeHouseNumber", "HomeCity", "HomePostCode", "HomeCountryCode", "HomeRegion", "MailingStreet", "MailingStreetNumber", "MailingHouseNumber", "MailingCity", "MailingPostCode", "MailingCountryCode", "MailingRegion", "EmergencyFirstName", "EmergencyLastName", "EmergencyStreet", "EmergencyStreetNumber", "EmergencyHouseNumber", "EmergencyCity", "EmergencyPostCode", "EmergencyCountryCode", "EmergencyRegion", "EmergencyPhone", "TaxStreet", "TaxStreetNumber", "TaxHouseNumber", "TaxCity", "TaxPostCode", "TaxCountryCode", "TaxRegion", "RecidencePermitNo", "RecidencePermitIssueDate", "NoOfWorkPermit", "NoOfWorkIssueDate", "HealthInsuranceLocation", "WaitingPeriod", "MobilityStatus", "EmployeeNumberOld", "IdentityDocument", "TaxDistrict", "TaxCommunity", "EmergencyDistrict", "EmergencyCommunity", "MailingDistrict", "MailingCommunity", "HomeDistrict", "HomeCommunity", "FamilyMemberRelationship", "FamilyMemberLastName", "FamilyMemberFirstName", "FamilyMemberDateOfBirth", "FamilyMemberPESEL", "FamilyMemberDisabilityScale", "FamilyMemberHealthInsurance", "FamilyMemberJoinedHousehold", "FamilyMemberExclusivelyDependent", "FamilyMembers", "FamilyMemberRelationship2", "FamilyMemberLastName2", "FamilyMemberFirstName2", "FamilyMemberDateOfBirth2", "FamilyMemberPESEL2", "FamilyMemberDisabilityScale2", "FamilyMemberHealthInsurance2", "FamilyMemberJoinedHousehold2", "FamilyMemberExclusivelyDependent2", "FamilyMemberRelationship3", "FamilyMemberLastName3", "FamilyMemberFirstName3", "FamilyMemberDateOfBirth3", "FamilyMemberPESEL3", "FamilyMemberDisabilityScale3", "FamilyMemberHealthInsurance3", "FamilyMemberJoinedHousehold3", "FamilyMemberExclusivelyDependent3", "FamilyMemberRelationship4", "FamilyMemberLastName4", "FamilyMemberFirstName4", "FamilyMemberDateOfBirth4", "FamilyMemberPESEL4", "FamilyMemberDisabilityScale4", "FamilyMemberHealthInsurance4", "FamilyMemberJoinedHousehold4", "FamilyMemberExclusivelyDependent4", "FamilyMemberRelationship5", "FamilyMemberLastName5", "FamilyMemberFirstName5", "FamilyMemberDateOfBirth5", "FamilyMemberPESEL5", "FamilyMemberDisabilityScale5", "FamilyMemberHealthInsurance5", "FamilyMemberJoinedHousehold5", "FamilyMemberExclusivelyDependent5", "ReasonForHiring", "BirthName", "Allowances" };
        }
    }
}
