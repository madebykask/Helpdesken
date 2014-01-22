namespace dhHelpdesk_NG.Data.Dal.Mappers.Changes
{
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Change;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public class Changer : IEntityChangerFromBusinessModel<Contact, ChangeContactEntity>
    {
        public void Map(Contact businessModel, ChangeContactEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
