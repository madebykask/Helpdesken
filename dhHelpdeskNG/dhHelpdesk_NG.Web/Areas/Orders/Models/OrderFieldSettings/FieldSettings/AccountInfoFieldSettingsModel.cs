using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [LocalizedDisplay("Startdatum")]
        public TextFieldSettingsModel StartedDate { get; set; }

        [LocalizedDisplay("Slutdatum")]
        public TextFieldSettingsModel FinishDate { get; set; }

        [LocalizedDisplay("E-posttyp")]
        public TextFieldSettingsModel EMailTypeId { get; set; }

        [LocalizedDisplay("Hemkatalog")]
        public TextFieldSettingsModel HomeDirectory { get; set; }

        [LocalizedDisplay("Profil")]
        public TextFieldSettingsModel Profile { get; set; }

        [LocalizedDisplay("Inventarienummer")]
        public TextFieldSettingsModel InventoryNumber { get; set; }

        [LocalizedDisplay("Vallista 1")]
        public OrderFieldTypeSettingsModel AccountType { get; set; }

        [LocalizedDisplay("Vallista 2")]
        public OrderFieldTypeSettingsModel AccountType2 { get; set; }

        [LocalizedDisplay("Vallista 3")]
        public OrderFieldTypeSettingsModel AccountType3 { get; set; }

        [LocalizedDisplay("Vallista 4")]
        public OrderFieldTypeSettingsModel AccountType4 { get; set; }

        [LocalizedDisplay("Vallista 5")]
        public OrderFieldTypeSettingsModel AccountType5 { get; set; }

        [LocalizedDisplay("Övrigt")]
        public TextFieldSettingsModel Info { get; set; }

    }
}