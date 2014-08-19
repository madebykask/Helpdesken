namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Operation;
    using DH.Helpdesk.Dal.Infrastructure;

    using OperationObject = DH.Helpdesk.Domain.OperationObject;

    public interface IOperationObjectRepository : IRepository<OperationObject>
    {
        void Add(OperationObjectForInsert businessModel);

        void Update(OperationObjectForUpdate businessModel);

        OperationObjectForView FindByName(string name);

        void DeleteById(int id);

        bool IsExist(string name);
    }

    public class OperationObjectRepository : RepositoryBase<OperationObject>, IOperationObjectRepository
    {
        public OperationObjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(OperationObjectForInsert businessModel)
        {
            var entity = new OperationObject();
            this.Map(businessModel, entity);

            entity.CreatedDate = businessModel.CreatedDate;
            entity.ChangedDate = businessModel.CreatedDate; // todo
            entity.Customer_Id = businessModel.CustomerId;

            this.Table.Add(entity);
        }

        public void Update(OperationObjectForUpdate businessModel)
        {
            var entity = this.Table.Single(x => x.Name.Equals(businessModel.Name));
            this.Map(businessModel, entity);
            entity.ChangedDate = businessModel.ChangeDate;
        }

        public OperationObjectForView FindByName(string name)
        {
            var anonymus =
                this.Table.Where(x => x.Name.Equals(name))
                    .Select(x => new { x.Id, x.Name, x.Description, x.CreatedDate, x.ChangedDate }).Single();

            var businessModel = new OperationObjectForView(
                anonymus.Name,
                anonymus.Description,
                anonymus.Id,
                anonymus.ChangedDate,
                anonymus.CreatedDate);

            return businessModel;
        }

        public void DeleteById(int id)
        {
            var entity = this.Table.Find(id);
            entity.Printers.Clear();
            this.Table.Remove(entity);
        }

        public bool IsExist(string name)
        {
            var anonymus = this.Table.FirstOrDefault(x => x.Name.Equals(name));
            bool isExist = anonymus != null;

            return isExist;
        }

        private void Map(BusinessData.Models.Operation.OperationObject businessModel, OperationObject entity)
        {
            if (string.IsNullOrWhiteSpace(businessModel.Name))
            {
                throw new ArgumentNullException("businessModel.Name");
            }

            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description ?? string.Empty;

            // todo
            entity.ShowPI = 0;
            entity.ShowOnStartPage = 0;
            entity.IsActive = 1;
            entity.WorkingGroup_Id = null;
        }
    }
}
