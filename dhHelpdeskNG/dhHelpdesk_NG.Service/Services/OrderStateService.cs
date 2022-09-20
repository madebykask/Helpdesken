namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IOrderStateService
    {
        IList<OrderState> GetOrderStates(int customerId);

        OrderState GetOrderState(int id);

        DeleteMessage DeleteOrderState(int id);

        void SaveOrderState(OrderState orderState, out IDictionary<string, string> errors);
        void Commit();
    }

    public class OrderStateService : IOrderStateService
    {
        private readonly IOrderStateRepository _orderStateRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public OrderStateService(
            IOrderStateRepository orderStateRepository,
            IUnitOfWork unitOfWork)
        {
            this._orderStateRepository = orderStateRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<OrderState> GetOrderStates(int customerId)
        {
            //return _orderStateRepository.GetAll().OrderBy(x => x.SortOrder).ToList();
            return this._orderStateRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.SortOrder).ToList();
        }

        public OrderState GetOrderState(int id)
        {
            return this._orderStateRepository.GetById(id);
        }

        public DeleteMessage DeleteOrderState(int id)
        {
            var orderState = this._orderStateRepository.GetById(id);

            if (orderState != null)
            {
                try
                {
                    this._orderStateRepository.Delete(orderState);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveOrderState(OrderState orderState, out IDictionary<string, string> errors)
        {
            if (orderState == null)
                throw new ArgumentNullException("orderstate");

            orderState.EMailList = orderState.EMailList ?? "";

            errors = new Dictionary<string, string>();

            //if (string.IsNullOrEmpty(orderState.Name))
            //    errors.Add("OrderState.Name", "Du måste ange en beställningsstatus");

            //if (string.IsNullOrEmpty(orderState.EMailList))
            //    errors.Add("OrderState.EMailList", "Du måste fylla i e-postadressfältet");

            orderState.ChangedDate = DateTime.UtcNow;

            if (orderState.Id == 0)
                this._orderStateRepository.Add(orderState);
            else
                this._orderStateRepository.Update(orderState);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
