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
            TextFieldSettingsModel info)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
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

        //[LocalizedDisplay("Vallista 1")]
        //public FieldSetting AccountTypeId { get; set; }

        //[LocalizedDisplay("Vallista 2")]
        //public FieldSetting AccountType2 { get; set; }

        //[LocalizedDisplay("Vallista 3")]
        //public FieldSetting AccountType3 { get; set; }

        //[LocalizedDisplay("Vallista 4")]
        //public FieldSetting AccountType4 { get; set; }

        //[LocalizedDisplay("Vallista 5")]
        //public FieldSetting AccountType5 { get; set; }

        [LocalizedDisplay("Övrigt")]
        public TextFieldSettingsModel Info { get; set; }
    }
}