namespace DH.Helpdesk.Services.Services.Concrete.Users
{
    using System;

    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services.Users;

    public class UsersPasswordHistoryService : IUsersPasswordHistoryService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UsersPasswordHistoryService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public int SaveHistory(int userId, string password)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var historyRep = uow.GetRepository<UsersPasswordHistory>();

                var entity = new UsersPasswordHistory
                                 {
                                     User_Id = userId, 
                                     Password = password,
                                     CreatedDate = DateTime.Now
                                 };
                historyRep.Add(entity);
                uow.Save();

                return entity.Id;
            }
        }
    }
}