namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IOrderTypeService
    {
        IList<OrderType> GetOrderTypesForMailTemplate(int customerId);
        IList<OrderType> GetParentOrderTypesForMailTemplate(int customerId);
        IList<OrderType> GetParentOrderTypesForMailTemplateIndexPage(int customerId);
        IList<OrderType> GetOrderTypes(int customerId);
        IList<OrderType> GetSubOrderTypes(int id);
        OrderType GetOrderType(int id);

        DeleteMessage DeleteOrderType(int id);

        void SaveOrderType(OrderType orderType, out IDictionary<string, string> errors);
        void Commit();
        bool IsUserHasOrderTypes(int customerId, int userId);
    }

    public class OrderTypeService : IOrderTypeService
    {
        private readonly IOrderTypeRepository _orderTypeRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly IOrderFieldSettingsRepository _orderFieldSettingsRepository;

#pragma warning disable 0618
        public OrderTypeService(
            IOrderTypeRepository orderTypeRepository,
            IOrderFieldSettingsRepository orderFieldSettingsRepository,
            IUnitOfWork unitOfWork)
        {
            this._orderTypeRepository = orderTypeRepository;
            this._unitOfWork = unitOfWork;
            this._orderFieldSettingsRepository = orderFieldSettingsRepository;
        }
#pragma warning restore 0618

        public IList<OrderType> GetOrderTypesForMailTemplate(int customerId)
        {
            return this._orderTypeRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<OrderType> GetParentOrderTypesForMailTemplate(int customerId)
        {
            return this._orderTypeRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_OrderType_Id != null).OrderByDescending(x => x.ChangedDate).ToList();
        }

        public IList<OrderType> GetParentOrderTypesForMailTemplateIndexPage(int customerId)
        {
            return this._orderTypeRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_OrderType_Id != null).OrderBy(x => x.Name).ToList();
        }

        public IList<OrderType> GetOrderTypes(int customerId)
        {
            return this._orderTypeRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_OrderType_Id == null).OrderBy(x => x.Name).ToList();
        }

        public IList<OrderType> GetSubOrderTypes(int id)
        {
            return this._orderTypeRepository.GetMany(x => x.Parent_OrderType_Id == id).ToList();
        }

        public OrderType GetOrderType(int id)
        {
            return this._orderTypeRepository.GetById(id);
        }

        public DeleteMessage DeleteOrderType(int id)
        {
            var orderType = this._orderTypeRepository.GetById(id);

            if (orderType != null)
            {
                try
                {
                    var orderFieldsSettings = _orderFieldSettingsRepository.GetMany(x => x.Customer_Id == orderType.Customer_Id && x.OrderType_Id == id);
                    foreach (var ofs in orderFieldsSettings)
                    {
                        _orderFieldSettingsRepository.Delete(ofs);
                    }
                    this._orderTypeRepository.Delete(orderType);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch (Exception)
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveOrderType(OrderType orderType, out IDictionary<string, string> errors)
        {
            if (orderType == null)
                throw new ArgumentNullException("ordertype");

            errors = new Dictionary<string, string>();

            //if (string.IsNullOrEmpty(orderType.Name))
            //    errors.Add("OrderType.Name", "Du måste ange en beställningstyp");

            orderType.ChangedDate = DateTime.UtcNow;

            if (orderType.Id == 0)
                this._orderTypeRepository.Add(orderType);
            else
                this._orderTypeRepository.Update(orderType);

            if (orderType.IsDefault == 1)
            {
                this._orderTypeRepository.ResetDefault(orderType.Id, orderType.Customer_Id);
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public bool IsUserHasOrderTypes(int customerId, int userId)
        {
            return this._orderTypeRepository.IsUserHasOrderTypes(customerId, userId);
        }
    }
}
