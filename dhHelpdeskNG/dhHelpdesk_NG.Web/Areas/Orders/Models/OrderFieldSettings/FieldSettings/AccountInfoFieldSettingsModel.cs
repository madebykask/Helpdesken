using DH.Helpdesk.BusinessData.Enums.Orders.Fields;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    public class AccountInfoFieldSettingsModel
    {
        public AccountInfoFieldSettingsModel()
        {
        }

        public AccountInfoFieldSettingsModel(TextFieldSettingsModel startedDate,
            TextFieldSettingsModel finishDate,
            TextFieldSettingsModel eMailTypeId,
            TextFieldSettingsModel homeDirectory,
            TextFieldSettingsModel profile,
            TextFieldSettingsModel inventoryNumber,
            TextFieldSettingsModel info,
            OrderFieldTypeSettingsModel accountType,
            OrderFieldTypeSettingsModel accountType2,
            OrderFieldTypeSettingsModel accountType3,
            OrderFieldTypeSettingsModel accountType4,
            OrderFieldTypeSettingsModel accountType5)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
            AccountType = accountType;
            AccountType2 = accountType2;
            AccountType3 = accountType3;
            AccountType4 = accountType4;
            AccountType5 = accountType5;
        }

        [LocalizedDisplay(OrderLabels.AccountInfoStartedDate)]
        public TextFieldSettingsModel StartedDate { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoFinishDate)]
        public TextFieldSettingsModel FinishDate { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoEMailTypeId)]
        public TextFieldSettingsModel EMailTypeId { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoHomeDirectory)]
        public TextFieldSettingsModel HomeDirectory { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoProfile)]
        public TextFieldSettingsModel Profile { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoInventoryNumber)]
        public TextFieldSettingsModel InventoryNumber { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoAccountType)]
        public OrderFieldTypeSettingsModel AccountType { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoAccountType2)]
        public OrderFieldTypeSettingsModel AccountType2 { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoAccountType3)]
        public OrderFieldTypeSettingsModel AccountType3 { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoAccountType4)]
        public OrderFieldTypeSettingsModel AccountType4 { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoAccountType5)]
        public OrderFieldTypeSettingsModel AccountType5 { get; set; }

        [LocalizedDisplay(OrderLabels.AccountInfoInfo)]
        public TextFieldSettingsModel Info { get; set; }

    }
}