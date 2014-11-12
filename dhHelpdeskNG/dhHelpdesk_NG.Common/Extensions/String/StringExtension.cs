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
                    case "AgreedDate": str = "Överenskommet datum"; break;
                    case "Available": str = "Anträffbar"; break;
                    case "caseCaption": str = "Rubrik"; break;
                    case "CaseNumber": str = "Ärende"; break;
                    case "CaseResponsibleUser_Id": str = "Ansvarig"; break;
                    case "CaseType_Id": str = "Ärendetyp"; break;
                    case "Category_Id": str = "Kategori"; break;
                    case "ChangeTime": str = "Ändringsdatum"; break;
                    case "ComputerType_Id": str = "Datortyp"; break;
                    case "ContactBeforeAction": str = "Telefonkontakt"; break;
                    case "Cost": str = "Kostnad"; break;
                    case "Customer_Id": str = "Kund"; break;
                    case "Department_Id": str = "Avdelning"; break;
                    case "Description": str = "Beskrivning"; break;
                    case "Filename": str = "Bifogad fil"; break;
                    case "FinishingDate": str = "Avslutsdatum"; break;
                    case "FinishingDescription": str = "Avslutsbeskrivning"; break;
                    case "Impact_Id": str = "Påverkan"; break;
                    case "InventoryLocation": str = "Placering"; break;
                    case "InventoryNumber": str = "PC Nummer"; break;
                    case "InvoiceNumber": str = "Fakturanummer"; break;
                    case "Miscellaneous": str = "Övrigt"; break;
                    case "OU_Id": str = "Enhet"; break;
                    case "Performer_User_Id": str = "Handläggare"; break;
                    case "Persons_CellPhone": str = "Mobilnr."; break;
                    case "Persons_EMail": str = "E-post"; break;
                    case "Persons_Name": str = "Namn"; break;
                    case "Persons_Phone": str = "Telefon"; break;
                    case "Place": str = "Plats"; break;
                    case "PlanDate": str = "Planerat datum"; break;
                    case "Priority_Id": str = "Prioritet"; break;
                    case "ProductArea_Id": str = "Produktområde"; break;
                    case "ReferenceNumber": str = "Referensnummer"; break;
                    case "Region_Id": str = "Område"; break;
                    case "RegTime": str = "Registreringsdatum"; break;
                    case "ReportedBy": str = "Användar ID"; break;
                    case "SMS": str = "SMS"; break;
                    case "SolutionRate": str = "Lösningsgrad"; break;
                    case "StateSecondary_Id": str = "Understatus"; break;
                    case "Status_Id": str = "Status"; break;
                    case "Supplier_Id": str = "Leverantör"; break;
                    case "System_Id": str = "System"; break;
                    case "tblLog_Charge": str = "Debitering"; break;
                    case "tblLog_Filename": str = "Bifogad fil"; break;
                    case "tblLog_Text_External": str = "Extern notering"; break;
                    case "tblLog_Text_Internal": str = "Intern notering"; break;
                    case "Urgency_Id": str = "Brådskandegrad"; break;
                    case "User_Id": str = "Registrerad av"; break;
                    case "UserCode": str = "Ansvarskod"; break;
                    case "WatchDate": str = "Bevakningsdatum"; break;
                    case "Verified": str = "Verifierad"; break;
                    case "VerifiedDescription": str = "Verifierad beskrivning"; break;
                    case "WorkingGroup_Id": str = "Driftgrupp"; break;
                    case "CausingPart": str = "Rotorsak"; break;
                    case "ClosingReason": str = "Avslutsorsak"; break;
                    case "_temporary_.LeadTime": str = "Tid kvar"; break;
                    case "tblProblem.ResponsibleUser_Id": str = "Problem"; break;
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
                    case "System_Id": str = ""; break;
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