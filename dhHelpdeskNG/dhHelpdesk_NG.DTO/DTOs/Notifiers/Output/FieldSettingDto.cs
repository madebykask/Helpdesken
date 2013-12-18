namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    public sealed class FieldSettingDto
    {
        public string Name { get; set; }

        public bool ShowInDetails { get; set; }

        public bool ShowInNotifiers { get; set; }

        public string LdapAttribute { get; set; }

        public string Caption { get; set; }

        public bool Required { get; set; }

        public int MinLength { get; set; }
    }
}
