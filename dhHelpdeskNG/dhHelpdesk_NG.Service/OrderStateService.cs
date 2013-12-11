using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
        private readonly IUnitOfWork _unitOfWork;

        public OrderStateService(
            IOrderStateRepository orderStateRepository,
            IUnitOfWork unitOfWork)
        {
            _orderStateRepository = orderStateRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<OrderState> GetOrderStates(int customerId)
        {
            //return _orderStateRepository.GetAll().OrderBy(x => x.SortOrder).ToList();
            return _orderStateRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public OrderState GetOrderState(int id)
        {
            return _orderStateRepository.GetById(id);
        }

        public DeleteMessage DeleteOrderState(int id)
        {
            var orderState = _orderStateRepository.GetById(id);

            if (orderState != null)
            {
                try
                {
                    _orderStateRepository.Delete(orderState);
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
                _orderStateRepository.Add(orderState);
            else
                _orderStateRepository.Update(orderState);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
