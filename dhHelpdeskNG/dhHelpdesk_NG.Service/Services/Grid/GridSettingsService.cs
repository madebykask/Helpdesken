namespace DH.Helpdesk.Services.Services.Grid
{
    using System;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain.Grid;

    using LinqLib.Operators;

    public class GridSettingsService 
    {
        #region Fields

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        #endregion

        #region Constructors and Destructors

        public GridSettingsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns grid settings business model
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="gridId"></param>
        /// <returns></returns>
        public GridSettingsModel GetForCustomerUserGrid(int customerId, int userId, string gridId)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<GridSettingsEntity>();
                var res = new GridSettingsModel(
                    gridId,
                    repository.GetAll()
                        .Where(item => item.CustomerId == customerId && item.UserId == userId && item.GridId == gridId)
                        .ToDictionary(it => it.Parameter.Trim(), it => it.Value.Trim()));
                return res;
            }
        }
        
        /// <summary>
        /// Saves settings in DB
        /// </summary>
        /// <param name="inputModel"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        public void Save(GridSettingsModel inputModel, int customerId, int userId)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
            {
                IRepository<GridSettingsEntity> repository = uow.GetRepository<GridSettingsEntity>();
                repository.DeleteWhere(it => it.CustomerId == customerId && it.UserId == userId && it.GridId == inputModel.GridId);
                inputModel.Parameters.ForEach(
                    param =>
                    repository.Add(
                        new GridSettingsEntity()
                            {
                                CustomerId = customerId,
                                UserId = userId,
                                GridId = inputModel.GridId,
                                Parameter = param.Key,
                                Value = param.Value
                            }));
                uow.Save();
            }
        }

        #endregion
    }
}