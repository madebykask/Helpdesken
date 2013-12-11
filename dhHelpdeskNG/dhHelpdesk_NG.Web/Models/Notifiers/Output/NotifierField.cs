namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifierFieldModel
    {
        public NotifierFieldModel(string name)
        {
            ArgumentsValidator.NotNullAndEmpty(name, "name");
         
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}