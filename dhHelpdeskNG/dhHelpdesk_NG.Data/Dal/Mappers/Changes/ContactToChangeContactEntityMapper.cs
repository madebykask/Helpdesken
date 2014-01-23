namespace dhHelpdesk_NG.Data.Dal.Mappers.Changes
{
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public sealed class ContactToChangeContactEntityMapper : INewBusinessModelToEntityMapper<Contact, ChangeContactEntity>
    {
        public ChangeContactEntity Map(Contact businessModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
