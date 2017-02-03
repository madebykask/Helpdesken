using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Services.BusinessLogic.Specifications;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.Services.Concrete.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Attributes.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.Services.Orders;

    using OrderFieldSettings = DH.Helpdesk.Domain.OrderFieldSettings;

    public sealed class OrderFieldSettingsService : IOrderFieldSettingsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public OrderFieldSettingsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        [CreateMissingOrderFieldSettings("customerId", "orderTypeId")]
        public FullFieldSettingsOverview GetOrdersFieldSettingsOverview(int customerId, int? orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();

                return fieldSettingsRep.GetAll()
                        .GetByType(customerId, orderTypeId)
                        .MapToOrdersFieldSettingsOverview();
            }
        }

        public OrderFieldSettingsFilterData GetFilterData(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var orderTypeRep = uow.GetRepository<OrderType>();

                var orderTypes = orderTypeRep.GetAll()
                                    .GetRootOrderTypes(customerId);

                return OrderFieldSettingsMapper.MapToFilterData(orderTypes);
            }
        }

        [CreateMissingOrderFieldSettings("customerId", "orderTypeId")]
        public GetSettingsResponse GetOrderFieldSettings(int customerId, int? orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();
                var orderFieldTypeRep = uow.GetRepository<OrderFieldType>();
                var orderTypeRep = uow.GetRepository<OrderType>();

                var orderFieldTypes = orderFieldTypeRep.GetAll()
                    .GetByType(orderTypeId).ActiveOnly().ToList();

                var orderTypeSettings = orderTypeRep.GetAll()
                    .GetById(orderTypeId).ToList();

                return fieldSettingsRep.GetAll()
                        .GetByType(customerId, orderTypeId)
                        .MapToFullFieldSettings(orderFieldTypes, orderTypeSettings);
            }
        }

        public void UpdateSettings(FullFieldSettings settings)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();
                var orderFieldTypeRep = uow.GetRepository<OrderFieldType>();
                var orderTypeRep = uow.GetRepository<OrderType>();

                var orderFieldTypes = orderFieldTypeRep.GetAll()
                    .GetByType(settings.OrderTypeId).ActiveOnly().ToList();
                if (settings.OrderTypeId.HasValue)
                {
                    var orderTypeSettings = orderTypeRep.GetById(settings.OrderTypeId.Value);
                    if (orderTypeSettings != null)
                    {
                        orderTypeSettings.CaptionDeliveryInfo = settings.Delivery.Header ?? "";
                        orderTypeSettings.CaptionGeneral = settings.General.Header ?? "";
                        orderTypeSettings.CaptionOrder = settings.Order.Header ?? "";
                        orderTypeSettings.CaptionOrderInfo = settings.Supplier.Header ?? "";
                        orderTypeSettings.CaptionOrdererInfo = settings.Orderer.Header ?? "";
                        orderTypeSettings.CaptionOther = settings.Other.Header ?? "";
                        orderTypeSettings.CaptionProgram = settings.Program.Header ?? "";
                        orderTypeSettings.CaptionReceiverInfo = settings.Receiver.Header ?? "";
                        orderTypeSettings.CaptionUserInfo = settings.User.Header ?? "";
                    }
                }

                var allAccountTypeValues = new List<OrderFieldTypeValueSetting>()
                    .Concat(settings.AccountInfo.AccountType.Values)
                    .Concat(settings.AccountInfo.AccountType2.Values)
                    .Concat(settings.AccountInfo.AccountType3.Values)
                    .Concat(settings.AccountInfo.AccountType4.Values)
                    .Concat(settings.AccountInfo.AccountType5.Values)
                    .ToArray();

                var toDelete = orderFieldTypes.Where(e => !allAccountTypeValues.Any(t => t.Id == e.Id && !e.Deleted));
                var toAdd = allAccountTypeValues.Where(a => !a.Id.HasValue)
                    .Select(e => new OrderFieldType
                    {
                        ChangedDate = settings.ChangedDate,
                        CreatedDate = settings.ChangedDate,
                        Name = e.Value,
                        OrderField = e.Type,
                        OrderType_Id = settings.OrderTypeId
                    });
                //Update
                orderFieldTypes.Where(e => allAccountTypeValues.Any(t => t.Id == e.Id))
                    .ForEach(e =>
                    {
                        var updateDto = allAccountTypeValues.Single(t => t.Id == e.Id);
                        e.Name = updateDto.Value;
                        e.ChangedDate = settings.ChangedDate;
                    });
                toDelete.ForEach(d => d.Deleted = true );
                toAdd.ForEach(a => orderFieldTypeRep.Add(a));

                fieldSettingsRep.GetAll()
                        .GetByType(settings.CustomerId, settings.OrderTypeId)
                        .MapToEntitiesForUpdate(settings);

                uow.Save();
            }
        }

        [CreateMissingOrderFieldSettings("customerId", "orderTypeId")]
        public FullOrderEditSettings GetOrderEditSettings(int customerId, int? orderTypeId, IUnitOfWork uow)
        {
            var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();
            var orderTypeRep = uow.GetRepository<OrderType>();

            var orderTypeSettings = orderTypeRep.GetAll()
                .GetById(orderTypeId).FirstOrDefault();

            return fieldSettingsRep.GetAll()
                        .GetByType(customerId, orderTypeId)
                        .MapToFullOrderEditSettings(orderTypeSettings);
        }
    }
}