using System;
using System.Collections.Generic;
using DH.Helpdesk.Web.Models;

namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using FieldModels;

    public sealed class OrderEditModel
    {
        public OrderEditModel()
        {            
        }

        public OrderEditModel(
            ConfigurableFieldModel<int?> property,
            ConfigurableFieldModel<string> orderRow1,
            ConfigurableFieldModel<string> orderRow2,
            ConfigurableFieldModel<string> orderRow3,
            ConfigurableFieldModel<string> orderRow4,
            ConfigurableFieldModel<string> orderRow5,
            ConfigurableFieldModel<string> orderRow6,
            ConfigurableFieldModel<string> orderRow7,
            ConfigurableFieldModel<string> orderRow8,
            ConfigurableFieldModel<string> configuration,
            ConfigurableFieldModel<string> orderInfo,
            ConfigurableFieldModel<int> orderInfo2,
            ConfigurableFieldModel<DateTime?> startedDate,
            ConfigurableFieldModel<DateTime?> finishDate,
            ConfigurableFieldModel<int?> eMailTypeId,
            ConfigurableFieldModel<bool> homeDirectory,
            ConfigurableFieldModel<bool> profile,
            ConfigurableFieldModel<int?> accountTypeId,
            ConfigurableFieldModel<List<CheckBoxListItem>> accountTypeId2,
            ConfigurableFieldModel<int?> accountTypeId3,
            ConfigurableFieldModel<int?> accountTypeId4,
            ConfigurableFieldModel<int?> accountTypeId5)
        {
            Property = property;
            OrderRow1 = orderRow1;
            OrderRow2 = orderRow2;
            OrderRow3 = orderRow3;
            OrderRow4 = orderRow4;
            OrderRow5 = orderRow5;
            OrderRow6 = orderRow6;
            OrderRow7 = orderRow7;
            OrderRow8 = orderRow8;
            Configuration = configuration;
            OrderInfo = orderInfo;
            OrderInfo2 = orderInfo2;
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = ConfigurableFieldModel<string>.CreateUnshowable();
            Info = ConfigurableFieldModel<string>.CreateUnshowable();
            AccountTypeId = accountTypeId;
            AccountTypeId2 = accountTypeId2;
            AccountTypeId3 = accountTypeId3;
            AccountTypeId4 = accountTypeId4;
            AccountTypeId5 = accountTypeId5;
        }

        public string Header { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> Property { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow1 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow2 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow3 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow4 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow5 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow6 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow7 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow8 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> Configuration { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderInfo { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<int> OrderInfo2 { get; set; }

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

        [NotNull]
        public SelectList Properties { get; set; }

        public static OrderEditModel CreateEmpty()
        {
            return new OrderEditModel(
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<int>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<bool>.CreateUnshowable(),
                ConfigurableFieldModel<bool>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<List<CheckBoxListItem>>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return Property.Show ||
                OrderRow1.Show ||
                OrderRow2.Show ||
                OrderRow3.Show ||
                OrderRow4.Show ||
                OrderRow5.Show ||
                OrderRow6.Show ||
                OrderRow7.Show ||
                OrderRow8.Show ||
                Configuration.Show ||
                OrderInfo.Show ||
                OrderInfo2.Show ||
                StartedDate.Show ||
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