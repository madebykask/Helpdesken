﻿// --------------------------------------------------------------------------------------------------------------------
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
            if (languageId == LanguageIds.Swedish)
            {
                switch (value.ToLower()) 
                {
                    case "agreeddate": str = "Överenskommet datum"; break;
                    case "available": str = "Anträffbar"; break;
                    case "casecaption": str = "Rubrik"; break; //obsolete? Seems like it doesn't exist in database.
                    case "caption": str = "Rubrik"; break;
                    case "casenumber": str = "Ärende"; break;
                    case "caseresponsibleuser_id": str = "Ansvarig"; break;
                    case "casetype_id": str = "Ärendetyp"; break;
                    case "category_id": str = "Kategori"; break;
                    case "changetime": str = "Ändringsdatum"; break;
                    case "computertype_id": str = "Datortyp"; break;
                    case "contactbeforeaction": str = "Telefonkontakt"; break;
                    case "cost": str = "Kostnad"; break;
                    case "customer_id": str = "Kund"; break;
                    case "department_id": str = "Avdelning"; break;
                    case "description": str = "Beskrivning"; break;
                    case "filename": str = "Bifogad fil"; break;
                    case "finishingdate": str = "Avslutsdatum"; break;
                    case "finishingdescription": str = "Avslutsbeskrivning"; break;
                    case "impact_id": str = "Påverkan"; break;
                    case "inventorylocation": str = "Placering"; break;
                    case "inventorynumber": str = "PC Nummer"; break;
                    case "invoicenumber": str = "Fakturanummer"; break;
                    case "miscellaneous": str = "Övrigt"; break;
                    case "ou_id": str = "Enhet"; break;
                    case "performer_user_id": str = "Handläggare"; break;
                    case "persons_cellphone": str = "Mobilnr."; break;
                    case "persons_email": str = "E-post"; break;
                    case "persons_name": str = "Namn"; break;
                    case "persons_phone": str = "Telefon"; break;
                    case "place": str = "Plats"; break;
                    case "plandate": str = "Planerat datum"; break;
                    case "priority_id": str = "Prioritet"; break;
                    case "productarea_id": str = "Produktområde"; break;
                    case "referencenumber": str = "Referensnummer"; break;
                    case "region_id": str = "Område"; break;
                    case "regtime": str = "Registreringsdatum"; break;
                    case "reportedby": str = "Användar ID"; break;
                    case "sms": str = "SMS"; break;
                    case "solutionrate": str = "Lösningsgrad"; break;
                    case "statesecondary_id": str = "Understatus"; break;
                    case "status_id": str = "Status"; break;
                    case "supplier_id": str = "Leverantör"; break;
                    case "system_id": str = "System"; break;
                    case "tbllog_charge": str = "Debitering"; break;
                    case "tbllog_filename": str = "Log bifogad fil"; break;
                    case "tbllog_text_external": str = "Extern notering"; break;
                    case "tbllog_text_internal": str = "Intern notering"; break;
                    case "tbllog.charge": str = "Debitering"; break;
                    case "tbllog.filename": str = "Log bifogad fil"; break;
                    case "tbllog.text_external": str = "Extern notering"; break;
                    case "tbllog.text_internal": str = "Intern notering"; break;
                    case "urgency_id": str = "Brådskandegrad"; break;
                    case "user_id": str = "Registrerad av"; break;
                    case "usercode": str = "Ansvarskod"; break;
                    case "watchdate": str = "Bevakningsdatum"; break;
                    case "verified": str = "Verifierad"; break;
                    case "verifieddescription": str = "Verifierad beskrivning"; break;
                    case "workinggroup_id": str = "Driftgrupp"; break;
                    case "causingpart": str = "Rotorsak"; break;
                    case "closingreason": str = "Avslutsorsak"; break;
                    case "_temporary_.leadtime": str = "Tid kvar"; break;
                    case "tblproblem.responsibleuser_id": str = "Problem"; break;
                    case "change": str = "Ändringshantering"; break;
                    case "project": str = "Projekt"; break;
                    case "registrationsourcecustomer": str = "Källa"; break;
                    case "isabout_reportedby": str = "Användar ID"; break;
                    case "isabout_persons_name": str = "Namn"; break;
                    case "isabout_persons_email": str = "E-post"; break;
                    case "isabout_persons_phone": str = "Telefon"; break;
                    case "isabout_persons_cellphone": str = "Mobilnr."; break;
                    case "isabout_region_id": str = "Område"; break;
                    case "isabout_department_id": str = "Avdelning"; break;
                    case "isabout_ou_id": str = "Enhet"; break;
                    case "isabout_costcentre": str = "Kostnad"; break;
                    case "isabout_place": str = "Plats"; break;
                    case "isabout_usercode": str = "Ansvarskod"; break;
                }
            }
            #endregion 

            #region EnglishValues 
            if (languageId != LanguageIds.Swedish)
            {
                switch (value.ToLower())
                {
                    case "agreeddate": str = "Agreed date"; break;
                    case "available": str = "Available"; break;
                    case "casecaption": str = "Subject"; break; //obsolete? Seems like it doesn't exist in database.
                    case "caption": str = "Subject"; break;
                    case "casenumber": str = "Case"; break;
                    case "caseresponsibleuser_id": str = "Responsible"; break;
                    case "casetype_id": str = "Case Type"; break;
                    case "category_id": str = "Category"; break;
                    case "changetime": str = "Date of change"; break;
                    case "computertype_id": str = "Computer type"; break;
                    case "contactbeforeaction": str = "Phone contact"; break;
                    case "cost": str = "Cost"; break;
                    case "customer_id": str = "Customer"; break;
                    case "department_id": str = "Department"; break;
                    case "description": str = "Description"; break;
                    case "filename": str = "Attached file"; break;
                    case "finishingdate": str = "Closing date"; break;
                    case "finishingdescription": str = "Closing description"; break;
                    case "impact_id": str = "Impact"; break;
                    case "inventorylocation": str = "Placement"; break;
                    case "inventorynumber": str = "PC Nummer"; break;
                    case "invoicenumber": str = "Invoice number"; break;
                    case "miscellaneous": str = "Other"; break;
                    case "ou_id": str = "Unit"; break;
                    case "performer_user_id": str = "Administrator"; break;
                    case "persons_cellphone": str = "Cell Phone"; break;
                    case "persons_email": str = "E-mail"; break;
                    case "persons_name": str = "Initiator"; break;
                    case "persons_phone": str = "Phone"; break;
                    case "place": str = "Placement"; break;
                    case "plandate": str = "Planned action date"; break;
                    case "priority_id": str = "Priority"; break;
                    case "productarea_id": str = "Product area"; break;
                    case "referencenumber": str = "Reference number"; break;
                    case "region_id": str = "Region"; break;
                    case "regtime": str = "Registration date"; break;
                    case "reportedby": str = "User ID"; break;
                    case "sms": str = "SMS"; break;
                    case "solutionrate": str = "Resolution rate"; break;
                    case "statesecondary_id": str = "Sub status"; break;
                    case "status_id": str = "Status"; break;
                    case "supplier_id": str = "Supplier"; break;
                    case "system_id": str = "System"; break;
                    case "tbllog_charge": str = "Charge"; break;
                    case "tbllog_filename": str = "Log Attached file"; break;
                    case "tbllog_text_external": str = "External log note"; break;
                    case "tbllog_text_internal": str = "Internal log note"; break;
                    case "tbllog.charge": str = "Charge"; break;
                    case "tbllog.filename": str = "Log Attached file"; break;
                    case "tbllog.text_external": str = "External log note"; break;
                    case "tbllog.text_internal": str = "Internal log note"; break;
                    case "urgency_id": str = "Urgent degree"; break;
                    case "user_id": str = "Registered by"; break;
                    case "usercode": str = "Orderer Code"; break;
                    case "watchdate": str = "Watch date"; break;
                    case "verified": str = "Verified"; break;
                    case "verifieddescription": str = "Verified description"; break;
                    case "workinggroup_id": str = "Working Group"; break;
                    case "causingpart": str = "Causing part"; break;
                    case "closingreason": str = "Closing reason"; break;
                    case "_temporary_.leadtime": str = "Time left"; break;
                    case "tblproblem.responsibleuser_id": str = "Problem"; break;
                    case "change": str = "Change"; break;
                    case "project": str = "Project"; break;
                    case "registrationsourcecustomer": str = "Source"; break;
                    case "isabout_reportedby": str = "User ID"; break;
                    case "isabout_persons_name": str = "Initiator"; break;
                    case "isabout_persons_email": str = "E-mail"; break;
                    case "isabout_persons_phone": str = "Phone"; break;
                    case "isabout_persons_cellphone": str = "Cell Phone"; break;
                    case "isabout_region_id": str = "Region"; break;
                    case "isabout_department_id": str = "Department"; break;
                    case "isabout_ou_id": str = "Unit"; break;
                    case "isabout_costcentre": str = "Cost"; break;
                    case "isabout_place": str = "Placement"; break;
                    case "isabout_usercode": str = "Orderer Code"; break;
                }
            }
            #endregion 

            return str.Trim();
        }

        public static string showlimitedcharacters(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            return value.Length > 105 ? value.Substring(0, 105) : value;
        }
    }
}