namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererProcessingSettings
    {
        public OrdererProcessingSettings(
            FieldProcessingSetting id,
            FieldProcessingSetting name,
            FieldProcessingSetting phone,
            FieldProcessingSetting cellPhone,
            FieldProcessingSetting email,
            FieldProcessingSetting department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public FieldProcessingSetting Id { get; private set; }

        [NotNull]
        public FieldProcessingSetting Name { get; private set; }

        [NotNull]
        public FieldProcessingSetting Phone { get; private set; }

        [NotNull]
        public FieldProcessingSetting CellPhone { get; private set; }

        [NotNull]
        public FieldProcessingSetting Email { get; private set; }

        [NotNull]
        public FieldProcessingSetting Department { get; private set; }
    }
}
