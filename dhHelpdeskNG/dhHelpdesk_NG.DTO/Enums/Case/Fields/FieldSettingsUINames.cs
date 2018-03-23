using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.BusinessData.Enums.Case.Fields
{
    public static class FieldSettingsUiNames
    {
        public static Dictionary<string, string> Names = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { GlobalEnums.TranslationCaseFields.ReportedBy.ToString(), "Användar ID"},
            { GlobalEnums.TranslationCaseFields.Persons_Name.ToString(), "Anmälare"},
            { GlobalEnums.TranslationCaseFields.Persons_EMail.ToString(), "E-post"},
            { GlobalEnums.TranslationCaseFields.Persons_Phone.ToString(), "Telefon"},
            { GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString(), "Mobil"},
            { GlobalEnums.TranslationCaseFields.Region_Id.ToString(), "Område"},
            { GlobalEnums.TranslationCaseFields.Department_Id.ToString(), "Avdelning"},
            { GlobalEnums.TranslationCaseFields.OU_Id.ToString(), "Organisationsenhet"},
            { GlobalEnums.TranslationCaseFields.CostCentre.ToString(), "Kostnadsställe"},
            { GlobalEnums.TranslationCaseFields.Place.ToString(), "Placering"},
            { GlobalEnums.TranslationCaseFields.UserCode.ToString(), "Ansvarskod"},
            { GlobalEnums.TranslationCaseFields.AddUserBtn.ToString(), "Lägg till"},
            { GlobalEnums.TranslationCaseFields.UpdateNotifierInformation.ToString(), "Spara användaruppgifter"},
            { GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString(), "Välj följare"},
            { GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString(), "Användar ID"},
            { GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString(), "Anmälare"},
            { GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString(), "E-post"},
            { GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString(), "Telefon"},
            { GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString(), "Mobil"},
            { GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString(), "Område"},
            { GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString(), "Avdelning"},
            { GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString(), "Organisationsenhet"},
            { GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString(), "Kostnadsställe"},
            { GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString(), "Placering"},
            { GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString(), "Ansvarskod"},
            { GlobalEnums.TranslationCaseFields.InventoryNumber.ToString(), "PC Nummer"},
            { GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString(), "Datortyp"},
            { GlobalEnums.TranslationCaseFields.InventoryLocation.ToString(), "Placering"},
            { GlobalEnums.TranslationCaseFields.CaseNumber.ToString(), "Ärende"},
            { GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString(), "Källa"},
            { GlobalEnums.TranslationCaseFields.RegTime.ToString(), "Registreringsdatum"},
            { GlobalEnums.TranslationCaseFields.ChangeTime.ToString(), "Ändringsdatum"},
            { GlobalEnums.TranslationCaseFields.User_Id.ToString(), "Registrerad av"},
            { GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(), "Ärendetyp"},
            { GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(), "Produktområde"},
            { GlobalEnums.TranslationCaseFields.System_Id.ToString(), "System"},
            { GlobalEnums.TranslationCaseFields.Urgency_Id.ToString(), "Brådskandegrad"},
            { GlobalEnums.TranslationCaseFields.Impact_Id.ToString(), "Påverkan"},
            { GlobalEnums.TranslationCaseFields.Category_Id.ToString(), "Kategori"},
            { GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(), "Leverantör"},
            { GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString(), "Fakturanummer"},
            { GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString(), "Referensnummer"},
            { GlobalEnums.TranslationCaseFields.Caption.ToString(), "Rubrik"},
            { GlobalEnums.TranslationCaseFields.Description.ToString(), "Beskrivning"},
            { GlobalEnums.TranslationCaseFields.Miscellaneous.ToString(), "Övrigt"},
            { GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString(), "Telefonkontakt"},
            { GlobalEnums.TranslationCaseFields.SMS.ToString(), "SMS"},
            { GlobalEnums.TranslationCaseFields.AgreedDate.ToString(), "Överenskommet datum"},
            { GlobalEnums.TranslationCaseFields.Available.ToString(), "Anträffbar"},
            { GlobalEnums.TranslationCaseFields.Cost.ToString(), "Kostnad"},
            { GlobalEnums.TranslationCaseFields.Filename.ToString(), "Bifogad fil"},
            { GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(), "Driftgrupp"},
            { GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString(), "Ansvarig"},
            { GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(), "Handläggare"},
            { GlobalEnums.TranslationCaseFields.Priority_Id.ToString(), "Prioritet"},
            { GlobalEnums.TranslationCaseFields.Status_Id.ToString(), "Status"},
            { GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(), "Understatus"},
            { GlobalEnums.TranslationCaseFields.Project.ToString(), "Project"},
            { GlobalEnums.TranslationCaseFields.Problem.ToString(), "Problem"},
            { GlobalEnums.TranslationCaseFields.PlanDate.ToString(), "Planerad åtgärdsdatum"},
            { GlobalEnums.TranslationCaseFields.WatchDate.ToString(), "Bevakningsdatum"},
            { GlobalEnums.TranslationCaseFields.Verified.ToString(), "Verifierad"},
            { GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(), "Verifierad beskrivning"},
            { GlobalEnums.TranslationCaseFields.SolutionRate.ToString(), "Lösningsgrad"},
            { GlobalEnums.TranslationCaseFields.CausingPart.ToString(), "Rotorsak"},
            { GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString(), "Intern notering"},
            { GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(), "Extern notering"},
            { GlobalEnums.TranslationCaseFields.tblLog_Charge.ToString(), "Debitering"},
            { GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), "Bifogad fil"},
            { "tblLog.Text_Internal", "Intern notering"}, //Those names are different from GlobalEnums.TranslationCaseFields
            { "tblLog.Text_External", "Extern notering"},
            { "tblLog.Charge", "Debitering"},
            { "tblLog.Filename", "Bifogad fil"},
            { GlobalEnums.TranslationCaseFields.FinishingDescription.ToString(), "Avslutsbeskrivning"},
            { GlobalEnums.TranslationCaseFields.FinishingDate.ToString(), "Avslutsdatum"},
            { GlobalEnums.TranslationCaseFields.ClosingReason.ToString(), "Avslutsorsak"},


        };
    }
}
