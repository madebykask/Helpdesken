namespace dhHelpdesk_NG.Data.Dal.Mappers.Changes
{
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes;

    public class ContactToChangeContactEntity : IBusinessModelToEntityMapper<Contact, ChangeContactEntity>
    {
        public void Map(Contact businessModel, ChangeContactEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
