namespace DH.Helpdesk.Dal.Dal.Mappers.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain.Changes;

    public class ContactToChangeContactEntity : IBusinessModelToEntityMapper<Contact, ChangeContactEntity>
    {
        public void Map(Contact businessModel, ChangeContactEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
