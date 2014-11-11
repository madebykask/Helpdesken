// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the StringExtension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.Common.Enums;
namespace DH.Helpdesk.Common.Extensions.String
{
    /// <summary>
    /// The string extension.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// The contain text.
        /// </summary>
        /// <param name="str">
        /// The string.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ContainsText(this string str, string text)
        {
            if (str == null || text == null)
            {
                return str == text;
            }

            return str.Trim().ToLower().Contains(text.Trim().ToLower());
        }

        /// <summary>
        /// The compare strings.
        /// </summary>
        /// <param name="str">
        /// The string.
        /// </param>
        /// <param name="forCompare">
        /// The for compare.
        /// </param>
        /// <param name="ignoreCase">
        /// The ignore case.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool EqualWith(this string str, string forCompare, bool ignoreCase = true)
        {
            if (str == null || forCompare == null)
            {
                return str == forCompare;
            }

            return string.Compare(str.Trim(), forCompare.Trim(), ignoreCase) == 0;
        }

        public static string ToTrimString(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var str = obj.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Trim();
        }

        public static string GetDefaultValue(this string value, int languageId)
        {
            if (value == null)
            {
                return null;
            }

            var str = value;
            
            #region SwedishValues 
            if (languageId == LanguageId.Swedish)
            {
                switch (value) 
                {
                    case "AgreedDate": str = "MajidTest1"; break;
                    case "Available": str = ""; break;
                    case "caseCaption": str = ""; break;
                    case "CaseNumber": str = ""; break;
                    case "CaseResponsibleUser_Id": str = ""; break;
                    case "CaseType_Id": str = ""; break;
                    case "Category_Id": str = ""; break;
                    case "ChangeTime": str = ""; break;
                    case "ComputerType_Id": str = ""; break;
                    case "ContactBeforeAction": str = ""; break;
                    case "Cost": str = ""; break;
                    case "Customer_Id": str = ""; break;
                    case "Department_Id": str = ""; break;
                    case "Description": str = ""; break;
                    case "Filename": str = ""; break;
                    case "FinishingDate": str = ""; break;
                    case "FinishingDescription": str = ""; break;
                    case "Impact_Id": str = ""; break;
                    case "InventoryLocation": str = ""; break;
                    case "InventoryNumber": str = ""; break;
                    case "InvoiceNumber": str = ""; break;
                    case "Miscellaneous": str = ""; break;
                    case "OU_Id": str = ""; break;
                    case "Performer_User_Id": str = ""; break;
                    case "Persons_CellPhone": str = ""; break;
                    case "Persons_EMail": str = "MajidTest1"; break;
                    case "Persons_Name": str = ""; break;
                    case "Persons_Phone": str = ""; break;
                    case "Place": str = ""; break;
                    case "PlanDate": str = ""; break;
                    case "Priority_Id": str = ""; break;
                    case "ProductArea_Id": str = ""; break;
                    case "ReferenceNumber": str = ""; break;
                    case "Region_Id": str = ""; break;
                    case "RegTime": str = ""; break;
                    case "ReportedBy": str = ""; break;
                    case "SMS": str = ""; break;
                    case "SolutionRate": str = ""; break;
                    case "StateSecondary_Id": str = ""; break;
                    case "Status_Id": str = ""; break;
                    case "Supplier_Id": str = ""; break;
                    case "System_Id": str = ""; break;
                    case "tblLog_Charge": str = ""; break;
                    case "tblLog_Filename": str = ""; break;
                    case "tblLog_Text_External": str = ""; break;
                    case "tblLog_Text_Internal": str = ""; break;
                    case "Urgency_Id": str = ""; break;
                    case "User_Id": str = ""; break;
                    case "UserCode": str = ""; break;
                    case "WatchDate": str = ""; break;
                    case "Verified": str = ""; break;
                    case "VerifiedDescription": str = ""; break;
                    case "WorkingGroup_Id": str = ""; break;
                    case "CausingPart": str = ""; break;
                    case "ClosingReason": str = ""; break;
                    case "_temporary_LeadTime": str = ""; break;
                    case "tblProblem.ResponsibleUser_Id": str = ""; break;
                }
            }
            #endregion 

            #region EnglishValues 
            if (languageId != LanguageId.Swedish)
            {
                switch (value)
                {
                    case "AgreedDate": str = "Agreed date"; break;
                    case "Available": str = "Available"; break;
                    case "caseCaption": str = "Subject"; break;
                    case "CaseNumber": str = "Case"; break;
                    case "CaseResponsibleUser_Id": str = "Responsible"; break;
                    case "CaseType_Id": str = "Case Type"; break;
                    case "Category_Id": str = "Category"; break;
                    case "ChangeTime": str = "Date of change"; break;
                    case "ComputerType_Id": str = "Computer type"; break;
                    case "ContactBeforeAction": str = "Phone contact"; break;
                    case "Cost": str = "Cost"; break;
                    case "Customer_Id": str = "Customer"; break;
                    case "Department_Id": str = "Department"; break;
                    case "Description": str = "Description"; break;
                    case "Filename": str = "Attached file"; break;
                    case "FinishingDate": str = "Closing date"; break;
                    case "FinishingDescription": str = "Closing date"; break;
                    case "Impact_Id": str = "Impact"; break;
                    case "InventoryLocation": str = "Placement"; break;
                    case "InventoryNumber": str = "PC Nummer"; break;
                    case "InvoiceNumber": str = "Invoice number"; break;
                    case "Miscellaneous": str = "Other"; break;
                    case "OU_Id": str = "Unit"; break;
                    case "Performer_User_Id": str = "Administrator"; break;
                    case "Persons_CellPhone": str = "Cell Phone"; break;
                    case "Persons_EMail": str = "E-mail"; break;
                    case "Persons_Name": str = "Initiator"; break;
                    case "Persons_Phone": str = "Phone"; break;
                    case "Place": str = "Placement"; break;
                    case "PlanDate": str = "Planned action date"; break;
                    case "Priority_Id": str = "Priority"; break;
                    case "ProductArea_Id": str = "Product area"; break;
                    case "ReferenceNumber": str = "Reference number"; break;
                    case "Region_Id": str = "Region"; break;
                    case "RegTime": str = "Registration date"; break;
                    case "ReportedBy": str = "User ID"; break;
                    case "SMS": str = "SMS"; break;
                    case "SolutionRate": str = "Resolution rate"; break;
                    case "StateSecondary_Id": str = "Sub status"; break;
                    case "Status_Id": str = "Status"; break;
                    case "Supplier_Id": str = "Supplier"; break;
                    case "tblLog_Charge": str = "System"; break;
                    case "tblLog_Filename": str = "Attached file"; break;
                    case "tblLog_Text_External": str = "External log note"; break;
                    case "tblLog_Text_Internal": str = "Internal log note"; break;
                    case "Urgency_Id": str = "Urgent degree"; break;
                    case "User_Id": str = "Registered by"; break;
                    case "UserCode": str = "Orderer Code"; break;
                    case "WatchDate": str = "Watch date"; break;
                    case "Verified": str = "Verified"; break;
                    case "VerifiedDescription": str = "Verified description"; break;
                    case "WorkingGroup_Id": str = "Working Group"; break;
                    case "CausingPart": str = "Causing part"; break;
                    case "ClosingReason": str = "Closing reason"; break;
                    case "_temporary_.LeadTime": str = "Time left"; break;
                    case "tblProblem.ResponsibleUser_Id": str = "Problem"; break;
                }
            }
            #endregion 

            return str.Trim();
        }
    }
}