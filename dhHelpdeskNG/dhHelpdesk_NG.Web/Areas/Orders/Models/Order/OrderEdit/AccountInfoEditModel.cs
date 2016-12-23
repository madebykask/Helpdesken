using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    public class AccountInfoEditModel
    {
        public AccountInfoEditModel()
        {
        }

        public AccountInfoEditModel(ConfigurableFieldModel<DateTime?> startedDate,
            ConfigurableFieldModel<DateTime?> finishDate,
            ConfigurableFieldModel<int?> eMailTypeId,
            ConfigurableFieldModel<bool> homeDirectory,
            ConfigurableFieldModel<bool> profile,
            ConfigurableFieldModel<string> inventoryNumber,
            ConfigurableFieldModel<string> info)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
        }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> StartedDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> EMailTypeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> HomeDirectory { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> Profile { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> InventoryNumber { get; set; }

        //public ConfigurableFieldModel<int?> AccountTypeId { get; set; }

        //public ConfigurableFieldModel<List<int>> AccountType2 { get; set; }

        //public ConfigurableFieldModel<int?> AccountType3 { get; set; }

        //public ConfigurableFieldModel<int?> AccountType4 { get; set; }

        //public ConfigurableFieldModel<int?> AccountType5 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }

        [NotNull]
        public SelectList EmailTypes { get; set; }

        //[NotNull]
        //public SelectList AccountTypes { get; set; }

        //[NotNull]
        //public MultiSelectList AccountTypes2 { get; set; }

        //[NotNull]
        //public SelectList AccountTypes3 { get; set; }

        //[NotNull]
        //public SelectList AccountTypes4 { get; set; }

        //[NotNull]
        //public SelectList AccountTypes5 { get; set; }

        public static AccountInfoEditModel CreateEmpty()
        {
            return new AccountInfoEditModel(
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<bool>.CreateUnshowable(),
                ConfigurableFieldModel<bool>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return StartedDate.Show ||
                   FinishDate.Show ||
                   EMailTypeId.Show ||
                   HomeDirectory.Show ||
                   Profile.Show ||
                   InventoryNumber.Show ||
                   Info.Show;
        }

    }


}