﻿namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class LogEditModel
    {
        public LogEditModel()
        {            
        }

        public LogEditModel(
            ConfigurableFieldModel<LogsModel> log)
        {
            this.Log = log;
        }

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Log { get; set; }

        public static LogEditModel CreateEmpty()
        {
            var empty = new LogEditModel(ConfigurableFieldModel<LogsModel>.CreateUnshowable());
            empty.Log.Value = new LogsModel();

            return empty;
        }

        public bool HasShowableFields()
        {
            return this.Log.Show;
        }
    }
}