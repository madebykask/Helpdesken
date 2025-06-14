﻿namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldEditSetting
    {
        public FieldEditSetting(bool show, string caption, bool required, string bookmark)
        {
            this.Show = show;
            this.Caption = caption;
            this.Required = required;
            this.Bookmark = bookmark;
        }

        public bool Show { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string Bookmark { get; private set; }
    }
}
