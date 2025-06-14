﻿namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;

    public interface IConfigurableFieldModelBuilder
    {
        SelectList CreateSelectList(
            ModelEditFieldSetting setting,
            List<ItemOverview> items,
            string selectedValue);

        SelectList CreateSelectList(
            ModelEditFieldSetting setting,
            Enum items,
            string selectedValue);

        ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
            ModelEditFieldSetting setting,
            DateTime? value);

        ConfigurableFieldModel<DateTime> CreateDateTimeField(ModelEditFieldSetting setting, DateTime value);

        ConfigurableFieldModel<string> CreateStringField(ModelEditFieldSetting setting, string value);

        ConfigurableFieldModel<bool> CreateBooleanField(ModelEditFieldSetting setting, bool value);

        ConfigurableFieldModel<int> CreateIntegerField(ModelEditFieldSetting setting, int value);

        ConfigurableFieldModel<int?> CreateNullableIntegerField(ModelEditFieldSetting setting, int? value);
    }
}