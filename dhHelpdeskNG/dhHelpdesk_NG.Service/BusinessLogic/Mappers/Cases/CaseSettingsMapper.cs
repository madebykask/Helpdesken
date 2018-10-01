using System.Collections.Generic;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Case.Fields;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases.Data;

    public static class CaseSettingsMapper
    {
        public static FullCaseSettings MapToCaseSettings(
                            this IQueryable<CaseFieldSetting> query,
                            int languageId, List<string> manualFields = null)
        {
            var exceptFields = new string[] { LogFields.AttachedFile };
            var entities = query.Select(f => new CaseSettingsMapData
                                                 {
                                                    FieldName = f.Name,                                                    
                                                    Show = f.ShowOnStartPage,
                                                    ShowInList = f.ShowExternal
                                                 })
                                                 .Where(f=> !exceptFields.ToList().Contains(f.FieldName))
                                                 .ToList();

            // Add LeadTime calculation field and log totals fields manually 
            if (manualFields != null && manualFields.Any())
            {
                foreach (var fieldName in manualFields)
                {
                    entities.Add(new CaseSettingsMapData
                    {
                        FieldName = fieldName,
                        Show = 1,
                        ShowInList = 1
                    });
                }
            }
            var fieldSettings = new NamedObjectCollection<CaseSettingsMapData>(entities);

            return CreateCaseSettings(fieldSettings);
        }

        private static FullCaseSettings CreateCaseSettings(NamedObjectCollection<CaseSettingsMapData> fieldSettings)
        {
            var user = CreateUserSettings(fieldSettings);
            var computer = CreateComputerSettings(fieldSettings);
            var caseInfo = CreateCaseInfoSettings(fieldSettings);
            var other = CreateOtherSettings(fieldSettings);
            var log = CreateLogSettings(fieldSettings);

            return new FullCaseSettings(
                            user,
                            computer,
                            caseInfo,
                            other,
                            log);
        }

        private static UserSettings CreateUserSettings(NamedObjectCollection<CaseSettingsMapData> fieldSettings)
        {
            var user = CreateFieldSetting(fieldSettings.FindByName(UserFields.User));
            var notifier = CreateFieldSetting(fieldSettings.FindByName(UserFields.Notifier));
            var email = CreateFieldSetting(fieldSettings.FindByName(UserFields.Email));
            var phone = CreateFieldSetting(fieldSettings.FindByName(UserFields.Phone));
            var cellPhone = CreateFieldSetting(fieldSettings.FindByName(UserFields.CellPhone));
            var customer = CreateFieldSetting(fieldSettings.FindByName(UserFields.Customer));
            var region = CreateFieldSetting(fieldSettings.FindByName(UserFields.Region));
            var department = CreateFieldSetting(fieldSettings.FindByName(UserFields.Department));
            var unit = CreateFieldSetting(fieldSettings.FindByName(UserFields.Unit));
            var place = CreateFieldSetting(fieldSettings.FindByName(UserFields.Place));
            var ordererCode = CreateFieldSetting(fieldSettings.FindByName(UserFields.OrdererCode));
            var costcentre = CreateFieldSetting(fieldSettings.FindByName(UserFields.CostCentre));
            var isaboutuser = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_User));
            var isaboutpersonsname = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_Persons_Name));
            var isaboutpersonsphone = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_Persons_Phone));
            var isaboutpersonscellphone = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_Persons_CellPhone));
            var isaboutpersonsemail = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_Persons_Email));
            var isaboutdepartment = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_Department));
            var isaboutregion = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_Region));
            var isaboutou = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_OU));
            var isaboutcostcentre = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_CostCentre));
            var isaboutplace = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_Place));
            var isaboutusercode = CreateFieldSetting(fieldSettings.FindByName(UserFields.IsAbout_UserCode));

            return new UserSettings(
                        user,
                        notifier,
                        email,
                        phone,
                        cellPhone,
                        customer,
                        region,
                        department,
                        unit,
                        place,
                        ordererCode,
                        costcentre,
                        isaboutuser,
                        isaboutpersonsname,
                        isaboutpersonsphone,
                        isaboutpersonscellphone,
                        isaboutpersonsemail,
                        isaboutdepartment,
                        isaboutregion,
                        isaboutou,
                        isaboutcostcentre,
                        isaboutplace,
                        isaboutusercode);
        }

        private static ComputerSettings CreateComputerSettings(NamedObjectCollection<CaseSettingsMapData> fieldSettings)
        {
            var number = CreateFieldSetting(fieldSettings.FindByName(ComputerFields.PcNumber));
            var computerType = CreateFieldSetting(fieldSettings.FindByName(ComputerFields.ComputerType));
            var place = CreateFieldSetting(fieldSettings.FindByName(ComputerFields.Place));

            return new ComputerSettings(
                        number,
                        computerType,
                        place);
        }

        private static CaseInfoSettings CreateCaseInfoSettings(NamedObjectCollection<CaseSettingsMapData> fieldSettings)
        {
            var caseNumber = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Case));
            var registrationDate = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.RegistrationDate));
            var changeDate = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.ChangeDate));
            var registratedBy = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.RegistratedBy));
            var caseType = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.CaseType));
            var productArea = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.ProductArea));
            var system = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.System));
            var urgentDegree = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.UrgentDegree));
            var impact = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Impact));
            var category = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Category));
            var supplier = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Supplier));
            var invoiceNumber = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.InvoiceNumber));
            var referenceNumber = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.ReferenceNumber));
            var caption = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Caption));
            var description = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Description));
            var other = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Other));
            var phoneContact = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.PhoneContact));
            var sms = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Sms));
            var agreedDate = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.AgreedDate));
            var available = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Available));
            var cost = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.Cost));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.AttachedFile));
            var registrationSource = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.RegistrationSourceCustomer));
            var leadTime = CreateFieldSetting(fieldSettings.FindByName(CaseInfoFields.LeadTime));

            return new CaseInfoSettings(
                        caseNumber,
                        registrationDate,
                        changeDate,
                        registratedBy,
                        caseType,
                        productArea,
                        system,
                        urgentDegree,
                        impact,
                        category,
                        supplier,
                        invoiceNumber,
                        referenceNumber,
                        caption,
                        description,
                        other,
                        phoneContact,
                        sms,
                        agreedDate,
                        available,
                        cost,
                        attachedFile,
                        registrationSource,
                        leadTime);
        }

        private static OtherSettings CreateOtherSettings(NamedObjectCollection<CaseSettingsMapData> fieldSettings)
        {
            var workingGroup = CreateFieldSetting(fieldSettings.FindByName(OtherFields.WorkingGroup));
            var responsible = CreateFieldSetting(fieldSettings.FindByName(OtherFields.Responsible));
            var administrator = CreateFieldSetting(fieldSettings.FindByName(OtherFields.Administrator));
            var priority = CreateFieldSetting(fieldSettings.FindByName(OtherFields.Priority));
            var state = CreateFieldSetting(fieldSettings.FindByName(OtherFields.State));
            var subState = CreateFieldSetting(fieldSettings.FindByName(OtherFields.SubState));
            var plannedActionDate = CreateFieldSetting(fieldSettings.FindByName(OtherFields.PlannedActionDate));
            var watchDate = CreateFieldSetting(fieldSettings.FindByName(OtherFields.WatchDate));
            var verified = CreateFieldSetting(fieldSettings.FindByName(OtherFields.Verified));
            var verifiedDescription = CreateFieldSetting(fieldSettings.FindByName(OtherFields.VerifiedDescription));
            var solutionRate = CreateFieldSetting(fieldSettings.FindByName(OtherFields.SolutionRate));
            var causingPart = CreateFieldSetting(fieldSettings.FindByName(OtherFields.CausingPart));

            return new OtherSettings(
                        workingGroup,
                        responsible,
                        administrator,
                        priority,
                        state,
                        subState,
                        plannedActionDate,
                        watchDate,
                        verified,
                        verifiedDescription,
                        solutionRate,
                        causingPart);
        }

        private static LogSettings CreateLogSettings(NamedObjectCollection<CaseSettingsMapData> fieldSettings)
        {
            var internalLogNote = CreateFieldSetting(fieldSettings.FindByName(LogFields.InternalLogNote));            
            var externalLogNote = CreateFieldSetting(fieldSettings.FindByName(LogFields.ExternalLogNote));            
            var debiting = CreateFieldSetting(fieldSettings.FindByName(LogFields.Debiting));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(LogFields.AttachedFile));            
            var finishingDescription = CreateFieldSetting(fieldSettings.FindByName(LogFields.FinishingDescription));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(LogFields.FinishingDate));
            var finishingCause = CreateFieldSetting(fieldSettings.FindByName(LogFields.FinishingCause));
            var totalMaterial = CreateFieldSetting(fieldSettings.FindByName(LogFields.TotalMaterial));
            var totalOverTime = CreateFieldSetting(fieldSettings.FindByName(LogFields.TotalOverTime));
            var totalPrice = CreateFieldSetting(fieldSettings.FindByName(LogFields.TotalPrice));
            var totalWork = CreateFieldSetting(fieldSettings.FindByName(LogFields.TotalWork));

            return new LogSettings(
                        internalLogNote,
                        externalLogNote,
                        debiting,
                        attachedFile,
                        finishingDescription,
                        finishingDate,
                        finishingCause,
                        totalMaterial,
                        totalOverTime,
                        totalPrice,
                        totalWork);
        }

        private static FieldOverviewSetting CreateFieldSetting(CaseSettingsMapData fieldSetting)
        {
            if (fieldSetting == null)
            {
                return FieldOverviewSetting.CreateUnshowable();
            }

            return new FieldOverviewSetting(fieldSetting.IsShow(), fieldSetting.GetFieldCaption());
        }
    }
}