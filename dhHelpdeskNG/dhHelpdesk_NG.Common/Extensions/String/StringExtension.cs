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
                    case "AgreedDate": str = "MajidTest2"; break;
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
                    case "Persons_EMail": str = "MajidTest2"; break;
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

            return str.Trim();
        }
    }
}