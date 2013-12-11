﻿using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IOrderTypeService
    {
        IList<OrderType> GetOrderTypesForMailTemplate(int customerId);
        IList<OrderType> GetOrderTypes(int customerId);

        OrderType GetOrderType(int id);

        DeleteMessage DeleteOrderType(int id);

        void SaveOrderType(OrderType orderType, out IDictionary<string, string> errors);
        void Commit();
    }

    public class OrderTypeService : IOrderTypeService
    {
        private readonly IOrderTypeRepository _orderTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderTypeService(
            IOrderTypeRepository orderTypeRepository,
            IUnitOfWork unitOfWork)
        {
            _orderTypeRepository = orderTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<OrderType> GetOrderTypesForMailTemplate(int customerId)
        {
            return _orderTypeRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_OrderType_Id == null).OrderByDescending(x => x.ChangedDate).ToList();
        }

        public IList<OrderType> GetOrderTypes(int customerId)
        {
            return _orderTypeRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_OrderType_Id == null).OrderBy(x => x.Name).ToList();
        }

        public OrderType GetOrderType(int id)
        {
            return _orderTypeRepository.GetById(id);
        }

        public DeleteMessage DeleteOrderType(int id)
        {
            var orderType = _orderTypeRepository.GetById(id);

            if (orderType != null)
            {
                try
                {
                    _orderTypeRepository.Delete(orderType);
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

        public void SaveOrderType(OrderType orderType, out IDictionary<string, string> errors)
        {
            if (orderType == null)
                throw new ArgumentNullException("ordertype");

            errors = new Dictionary<string, string>();

            //if (string.IsNullOrEmpty(orderType.Name))
            //    errors.Add("OrderType.Name", "Du måste ange en beställningstyp");

            orderType.ChangedDate = DateTime.UtcNow;

            if (orderType.Id == 0)
                _orderTypeRepository.Add(orderType);
            else
                _orderTypeRepository.Update(orderType);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
