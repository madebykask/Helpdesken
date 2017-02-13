using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
using DH.Helpdesk.Web.Models;

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
            ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<int?> accountTypeId,
            ConfigurableFieldModel<List<CheckBoxListItem>> accountTypeId2,
            ConfigurableFieldModel<int?> accountTypeId3,
            ConfigurableFieldModel<int?> accountTypeId4,
            ConfigurableFieldModel<int?> accountTypeId5)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
            AccountTypeId = accountTypeId;
            AccountTypeId2 = accountTypeId2;
            AccountTypeId3 = accountTypeId3;
            AccountTypeId4 = accountTypeId4;
            AccountTypeId5 = accountTypeId5;
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

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }

        public ConfigurableFieldModel<int?> AccountTypeId { get; set; }

        public ConfigurableFieldModel<List<CheckBoxListItem>> AccountTypeId2 { get; set; }

        public ConfigurableFieldModel<int?> AccountTypeId3 { get; set; }

        public ConfigurableFieldModel<int?> AccountTypeId4 { get; set; }

        public ConfigurableFieldModel<int?> AccountTypeId5 { get; set; }

        [NotNull]
        public SelectList EmailTypes { get; set; }

        [NotNull]
        public SelectList AccountTypes { get; set; }

        [NotNull]
        public MultiSelectList AccountTypes2 { get; set; }

        [NotNull]
        public SelectList AccountTypes3 { get; set; }

        [NotNull]
        public SelectList AccountTypes4 { get; set; }

        [NotNull]
        public SelectList AccountTypes5 { get; set; }

        public static AccountInfoEditModel CreateEmpty()
        {
            return new AccountInfoEditModel(
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<bool>.CreateUnshowable(),
                ConfigurableFieldModel<bool>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<List<CheckBoxListItem>>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return StartedDate.Show ||
                   FinishDate.Show ||
                   EMailTypeId.Show ||
                   HomeDirectory.Show ||
                   Profile.Show ||
                   InventoryNumber.Show ||
                   Info.Show ||
                   AccountTypeId.Show ||
                   AccountTypeId2.Show ||
                   AccountTypeId3.Show ||
                   AccountTypeId4.Show ||
                   AccountTypeId5.Show;
        }

    }


}