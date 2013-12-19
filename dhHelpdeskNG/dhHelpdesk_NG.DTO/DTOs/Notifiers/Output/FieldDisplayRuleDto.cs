namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    public class FieldDisplayRuleDto
    {
        public FieldDisplayRuleDto(bool show, bool required)
        {
            this.Show = show;
            this.Required = required;
        }

        public bool Required { get; private set; }

        public bool Show { get; private set; }
    }
}
