namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    using System;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders.FieldNames;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;

    public static class OrderSpecifications
    {
        public static IQueryable<Order> GetByType(this IQueryable<Order> query, int? orderTypeId)
        {
            if (!orderTypeId.HasValue)
            {
                return query;
            }

            query = query.Where(o => o.OrderType_Id == orderTypeId);

            return query;
        }

        public static IQueryable<Order> GetByAdministrators(this IQueryable<Order> query, int[] administratorIds)
        {
            if (administratorIds == null || !administratorIds.Any())
            {
                return query;
            }

            query = query.Where(o => o.User_Id.HasValue && administratorIds.Contains(o.User_Id.Value));

            return query;
        }

        public static IQueryable<Order> GetByPeriod(
                        this IQueryable<Order> query,
                        DateTime? startDate,
                        DateTime? endDate)
        {
            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= endDate);
            }

            return query;
        } 

        public static IQueryable<Order> GetByStatuses(this IQueryable<Order> query, int[] statusIds)
        {
            if (statusIds == null || !statusIds.Any())
            {
                return query;
            }

            query = query.Where(o => o.OrderState_Id.HasValue && statusIds.Contains(o.OrderState_Id.Value));

            return query;
        }

        public static IQueryable<Order> GetByText(this IQueryable<Order> query, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return query;
            }

            var search = text.Trim().ToLower();

            query = query.Where(o =>
                                    o.Id.ToString().Trim().ToLower().Contains(search) ||                                          

                                    // Delivery
                                    o.DeliveryDepartment.DepartmentName.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryOu.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryAddress.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryPostalCode.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryPostalAddress.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryLocation.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryInfo.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryInfo.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryInfo2.Trim().ToLower().Contains(search) ||                    
                                    o.DeliveryOuEntity.Name.Trim().ToLower().Contains(search) ||       
             
                                    // General
                                    o.Domain.Name.Trim().ToLower().Contains(search) ||      

                                    // Log
                                    o.Logs.Any(l => l.LogNote.Trim().ToLower().Contains(search)) ||

                                    // Orderer
                                    o.OrdererID.Trim().ToLower().Contains(search) ||      
                                    o.Orderer.Trim().ToLower().Contains(search) ||      
                                    o.OrdererLocation.Trim().ToLower().Contains(search) ||      
                                    o.OrdererEMail.Trim().ToLower().Contains(search) ||      
                                    o.OrdererPhone.Trim().ToLower().Contains(search) ||      
                                    o.OrdererCode.Trim().ToLower().Contains(search) ||      
                                    o.Ou.Name.Trim().ToLower().Contains(search) ||      
                                    o.OrdererAddress.Trim().ToLower().Contains(search) ||
                                    o.OrdererInvoiceAddress.Trim().ToLower().Contains(search) ||
                                    o.OrdererReferenceNumber.Trim().ToLower().Contains(search) ||
                                    o.AccountingDimension1.Trim().ToLower().Contains(search) ||      
                                    o.AccountingDimension2.Trim().ToLower().Contains(search) ||      
                                    o.AccountingDimension3.Trim().ToLower().Contains(search) ||      
                                    o.AccountingDimension4.Trim().ToLower().Contains(search) ||      
                                    o.AccountingDimension5.Trim().ToLower().Contains(search) ||      

                                    // Order
                                    o.OrderProperty.OrderProperty.Trim().ToLower().Contains(search) ||
                                    o.OrderRow.Trim().ToLower().Contains(search) ||
                                    o.OrderRow2.Trim().ToLower().Contains(search) ||
                                    o.OrderRow3.Trim().ToLower().Contains(search) ||
                                    o.OrderRow4.Trim().ToLower().Contains(search) ||
                                    o.OrderRow5.Trim().ToLower().Contains(search) ||
                                    o.OrderRow6.Trim().ToLower().Contains(search) ||
                                    o.OrderRow7.Trim().ToLower().Contains(search) ||
                                    o.OrderRow8.Trim().ToLower().Contains(search) ||
                                    o.Configuration.Trim().ToLower().Contains(search) ||
                                    o.OrderInfo.Trim().ToLower().Contains(search) ||
                                    
                                    // Other
                                    o.Filename.Trim().ToLower().Contains(search) ||
                                    o.Info.Trim().ToLower().Contains(search) ||
                                    o.OrderState.Name.Trim().ToLower().Contains(search) ||

                                    // Receiver
                                    o.ReceiverId.Trim().ToLower().Contains(search) ||
                                    o.ReceiverName.Trim().ToLower().Contains(search) ||
                                    o.ReceiverEMail.Trim().ToLower().Contains(search) ||
                                    o.ReceiverPhone.Trim().ToLower().Contains(search) ||
                                    o.ReceiverLocation.Trim().ToLower().Contains(search) ||
                                    o.MarkOfGoods.Trim().ToLower().Contains(search) ||

                                    // Supplier
                                    o.SupplierOrderNumber.Trim().ToLower().Contains(search) ||
                                    o.SupplierOrderInfo.Trim().ToLower().Contains(search) ||

                                    // User
                                    o.UserId.Trim().ToLower().Contains(search) ||
                                    o.UserFirstName.Trim().ToLower().Contains(search) ||
                                    o.UserLastName.Trim().ToLower().Contains(search));

            return query;
        } 

        public static IQueryable<Order> Search(
                        this IQueryable<Order> query,
                        int customerId,
                        int? orderTypeId,
                        int[] administratorIds,
                        DateTime? startDate,
                        DateTime? endDate,
                        int[] statusIds,
                        string text,
                        SortField sort,
                        int selectCount)
        {
            query = query
                        .GetByCustomer(customerId)
                        .GetByType(orderTypeId)
                        .GetByAdministrators(administratorIds)
                        .GetByPeriod(startDate, endDate)
                        .GetByStatuses(statusIds)
                        .GetByText(text)
                        .Sort(sort);

            if (selectCount > 0)
            {
                query = query.Take(selectCount);
            }

            return query;
        }

        public static IQueryable<OrderType> GetOrderTypes(this IQueryable<OrderType> query, int customerId)
        {
            query = query.GetByCustomer(customerId)
                    .Where(t => t.IsActive == 1);

            return query;
        }

        public static IQueryable<OrderState> GetOrderStatuses(this IQueryable<OrderState> query, int customerId)
        {
            query = query.GetByCustomer(customerId)
                    .Where(s => s.IsActive == 1)
                    .OrderBy(s => s.SortOrder);

            return query;
        }

        public static IQueryable<Order> Sort(this IQueryable<Order> query, SortField sort)
        {
            if (sort == null)
            {
                return query;
            }

            switch (sort.SortBy)
            {
                case SortBy.Ascending:
                    // Delivery
                    if (sort.Name == DeliveryFieldNames.DeliveryDate)
                    {
                        query = query.OrderBy(o => o.Deliverydate);
                    }
                    else if (sort.Name == DeliveryFieldNames.InstallDate)
                    {
                        query = query.OrderBy(o => o.InstallDate);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryDepartment)
                    {
                        query = query.OrderBy(o => o.DeliveryDepartment.DepartmentName);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryOu)
                    {
                        query = query.OrderBy(o => o.DeliveryOu);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryAddress)
                    {
                        query = query.OrderBy(o => o.DeliveryAddress);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryPostalCode)
                    {
                        query = query.OrderBy(o => o.DeliveryPostalCode);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryPostalAddress)
                    {
                        query = query.OrderBy(o => o.DeliveryPostalAddress);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryLocation)
                    {
                        query = query.OrderBy(o => o.DeliveryLocation);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryInfo1)
                    {
                        query = query.OrderBy(o => o.DeliveryInfo);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryInfo2)
                    {
                        query = query.OrderBy(o => o.DeliveryInfo2);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryInfo3)
                    {
                        query = query.OrderBy(o => o.DeliveryInfo3);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryOuId)
                    {
                        query = query.OrderBy(o => o.DeliveryOuEntity.Name);
                    }

                    // General
                    else if (sort.Name == GeneralFieldNames.OrderNumber)
                    {
                        query = query.OrderBy(o => o.Id);
                    }
                    else if (sort.Name == GeneralFieldNames.Customer)
                    {
                        query = query.OrderBy(o => o.Customer.Name);
                    }
                    else if (sort.Name == GeneralFieldNames.Administrator)
                    {
                        query = query.OrderBy(o => o.User_Id);
                    }
                    else if (sort.Name == GeneralFieldNames.Domain)
                    {
                        query = query.OrderBy(o => o.Domain.Name);
                    }
                    else if (sort.Name == GeneralFieldNames.OrderDate)
                    {
                        query = query.OrderBy(o => o.OrderDate);
                    }

                    // Log
                    else if (sort.Name == LogFieldNames.Log)
                    {
                        query = query.OrderBy(l => l.Logs.Count);
                    }

                    // Orderer
                    else if (sort.Name == OrdererFieldNames.OrdererId)
                    {
                        query = query.OrderBy(o => o.OrdererID);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererName)
                    {
                        query = query.OrderBy(o => o.Orderer);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererLocation)
                    {
                        query = query.OrderBy(o => o.OrdererLocation);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererEmail)
                    {
                        query = query.OrderBy(o => o.OrdererEMail);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererPhone)
                    {
                        query = query.OrderBy(o => o.OrdererPhone);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererCode)
                    {
                        query = query.OrderBy(o => o.OrdererCode);
                    }
                    else if (sort.Name == OrdererFieldNames.Department)
                    {
                        query = query.OrderBy(o => o.Department_Id);
                    }
                    else if (sort.Name == OrdererFieldNames.Unit)
                    {
                        query = query.OrderBy(o => o.Ou.Name);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererAddress)
                    {
                        query = query.OrderBy(o => o.OrdererAddress);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererInvoiceAddress)
                    {
                        query = query.OrderBy(o => o.OrdererInvoiceAddress);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererReferenceNumber)
                    {
                        query = query.OrderBy(o => o.OrdererReferenceNumber);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension1)
                    {
                        query = query.OrderBy(o => o.AccountingDimension1);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension2)
                    {
                        query = query.OrderBy(o => o.AccountingDimension2);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension3)
                    {
                        query = query.OrderBy(o => o.AccountingDimension3);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension4)
                    {
                        query = query.OrderBy(o => o.AccountingDimension4);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension5)
                    {
                        query = query.OrderBy(o => o.AccountingDimension5);
                    }

                    // Order
                    else if (sort.Name == OrderFieldNames.Property)
                    {
                        query = query.OrderBy(o => o.OrderProperty.OrderProperty);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow1)
                    {
                        query = query.OrderBy(o => o.OrderRow);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow2)
                    {
                        query = query.OrderBy(o => o.OrderRow2);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow3)
                    {
                        query = query.OrderBy(o => o.OrderRow3);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow4)
                    {
                        query = query.OrderBy(o => o.OrderRow4);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow5)
                    {
                        query = query.OrderBy(o => o.OrderRow5);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow6)
                    {
                        query = query.OrderBy(o => o.OrderRow6);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow7)
                    {
                        query = query.OrderBy(o => o.OrderRow7);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow8)
                    {
                        query = query.OrderBy(o => o.OrderRow8);
                    }
                    else if (sort.Name == OrderFieldNames.Configuration)
                    {
                        query = query.OrderBy(o => o.Configuration);
                    }
                    else if (sort.Name == OrderFieldNames.OrderInfo)
                    {
                        query = query.OrderBy(o => o.OrderInfo);
                    }
                    else if (sort.Name == OrderFieldNames.OrderInfo2)
                    {
                        query = query.OrderBy(o => o.OrderInfo2);
                    }

                    // Other
                    else if (sort.Name == OtherFieldNames.FileName)
                    {
                        query = query.OrderBy(o => o.Filename);
                    }
                    else if (sort.Name == OtherFieldNames.CaseNumber)
                    {
                        query = query.OrderBy(o => o.CaseNumber);
                    }
                    else if (sort.Name == OtherFieldNames.Info)
                    {
                        query = query.OrderBy(o => o.Info);
                    }
                    else if (sort.Name == OtherFieldNames.Status)
                    {
                        query = query.OrderBy(o => o.OrderState.Name);
                    }

                    // Program
                    else if (sort.Name == ProgramFieldNames.Program)
                    {
                        query = query.OrderBy(o => o.Programs.Count);
                    }

                    // Receiver
                    else if (sort.Name == ReceiverFieldNames.ReceiverId)
                    {
                        query = query.OrderBy(o => o.ReceiverId);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverName)
                    {
                        query = query.OrderBy(o => o.ReceiverName);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverEmail)
                    {
                        query = query.OrderBy(o => o.ReceiverEMail);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverPhone)
                    {
                        query = query.OrderBy(o => o.ReceiverPhone);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverLocation)
                    {
                        query = query.OrderBy(o => o.ReceiverLocation);
                    }
                    else if (sort.Name == ReceiverFieldNames.MarkOfGoods)
                    {
                        query = query.OrderBy(o => o.MarkOfGoods);
                    }

                    // Supplier
                    else if (sort.Name == SupplierFieldNames.SupplierOrderNumber)
                    {
                        query = query.OrderBy(o => o.SupplierOrderNumber);
                    }
                    else if (sort.Name == SupplierFieldNames.SupplierOrderDate)
                    {
                        query = query.OrderBy(o => o.SupplierOrderDate);
                    }
                    else if (sort.Name == SupplierFieldNames.SupplierOrderInfo)
                    {
                        query = query.OrderBy(o => o.SupplierOrderInfo);
                    }

                    // User
                    else if (sort.Name == UserFieldNames.UserId)
                    {
                        query = query.OrderBy(o => o.UserId);
                    }
                    else if (sort.Name == UserFieldNames.UserFirstName)
                    {
                        query = query.OrderBy(o => o.UserFirstName);
                    }
                    else if (sort.Name == UserFieldNames.UserLastName)
                    {
                        query = query.OrderBy(o => o.UserLastName);
                    }

                    break;

                case SortBy.Descending:
                    // Delivery
                    if (sort.Name == DeliveryFieldNames.DeliveryDate)
                    {
                        query = query.OrderByDescending(o => o.Deliverydate);
                    }
                    else if (sort.Name == DeliveryFieldNames.InstallDate)
                    {
                        query = query.OrderByDescending(o => o.InstallDate);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryDepartment)
                    {
                        query = query.OrderByDescending(o => o.DeliveryDepartment.DepartmentName);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryOu)
                    {
                        query = query.OrderByDescending(o => o.DeliveryOu);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryAddress)
                    {
                        query = query.OrderByDescending(o => o.DeliveryAddress);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryPostalCode)
                    {
                        query = query.OrderByDescending(o => o.DeliveryPostalCode);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryPostalAddress)
                    {
                        query = query.OrderByDescending(o => o.DeliveryPostalAddress);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryLocation)
                    {
                        query = query.OrderByDescending(o => o.DeliveryLocation);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryInfo1)
                    {
                        query = query.OrderByDescending(o => o.DeliveryInfo);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryInfo2)
                    {
                        query = query.OrderByDescending(o => o.DeliveryInfo2);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryInfo3)
                    {
                        query = query.OrderByDescending(o => o.DeliveryInfo3);
                    }
                    else if (sort.Name == DeliveryFieldNames.DeliveryOuId)
                    {
                        query = query.OrderByDescending(o => o.DeliveryOuEntity.Name);
                    }

                    // General
                    else if (sort.Name == GeneralFieldNames.OrderNumber)
                    {
                        query = query.OrderByDescending(o => o.Id);
                    }
                    else if (sort.Name == GeneralFieldNames.Customer)
                    {
                        query = query.OrderByDescending(o => o.Customer.Name);
                    }
                    else if (sort.Name == GeneralFieldNames.Administrator)
                    {
                        query = query.OrderByDescending(o => o.User_Id);
                    }
                    else if (sort.Name == GeneralFieldNames.Domain)
                    {
                        query = query.OrderByDescending(o => o.Domain.Name);
                    }
                    else if (sort.Name == GeneralFieldNames.OrderDate)
                    {
                        query = query.OrderByDescending(o => o.OrderDate);
                    }

                    // Log
                    else if (sort.Name == LogFieldNames.Log)
                    {
                        query = query.OrderByDescending(l => l.Logs.Count);
                    }

                    // Orderer
                    else if (sort.Name == OrdererFieldNames.OrdererId)
                    {
                        query = query.OrderByDescending(o => o.OrdererID);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererName)
                    {
                        query = query.OrderByDescending(o => o.Orderer);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererLocation)
                    {
                        query = query.OrderByDescending(o => o.OrdererLocation);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererEmail)
                    {
                        query = query.OrderByDescending(o => o.OrdererEMail);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererPhone)
                    {
                        query = query.OrderByDescending(o => o.OrdererPhone);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererCode)
                    {
                        query = query.OrderByDescending(o => o.OrdererCode);
                    }
                    else if (sort.Name == OrdererFieldNames.Department)
                    {
                        query = query.OrderByDescending(o => o.Department_Id);
                    }
                    else if (sort.Name == OrdererFieldNames.Unit)
                    {
                        query = query.OrderByDescending(o => o.Ou.Name);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererAddress)
                    {
                        query = query.OrderByDescending(o => o.OrdererAddress);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererInvoiceAddress)
                    {
                        query = query.OrderByDescending(o => o.OrdererInvoiceAddress);
                    }
                    else if (sort.Name == OrdererFieldNames.OrdererReferenceNumber)
                    {
                        query = query.OrderByDescending(o => o.OrdererReferenceNumber);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension1)
                    {
                        query = query.OrderByDescending(o => o.AccountingDimension1);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension2)
                    {
                        query = query.OrderByDescending(o => o.AccountingDimension2);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension3)
                    {
                        query = query.OrderByDescending(o => o.AccountingDimension3);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension4)
                    {
                        query = query.OrderByDescending(o => o.AccountingDimension4);
                    }
                    else if (sort.Name == OrdererFieldNames.AccountingDimension5)
                    {
                        query = query.OrderByDescending(o => o.AccountingDimension5);
                    }

                    // Order
                    else if (sort.Name == OrderFieldNames.Property)
                    {
                        query = query.OrderByDescending(o => o.OrderProperty.OrderProperty);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow1)
                    {
                        query = query.OrderByDescending(o => o.OrderRow);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow2)
                    {
                        query = query.OrderByDescending(o => o.OrderRow2);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow3)
                    {
                        query = query.OrderByDescending(o => o.OrderRow3);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow4)
                    {
                        query = query.OrderByDescending(o => o.OrderRow4);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow5)
                    {
                        query = query.OrderByDescending(o => o.OrderRow5);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow6)
                    {
                        query = query.OrderByDescending(o => o.OrderRow6);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow7)
                    {
                        query = query.OrderByDescending(o => o.OrderRow7);
                    }
                    else if (sort.Name == OrderFieldNames.OrderRow8)
                    {
                        query = query.OrderByDescending(o => o.OrderRow8);
                    }
                    else if (sort.Name == OrderFieldNames.Configuration)
                    {
                        query = query.OrderByDescending(o => o.Configuration);
                    }
                    else if (sort.Name == OrderFieldNames.OrderInfo)
                    {
                        query = query.OrderByDescending(o => o.OrderInfo);
                    }
                    else if (sort.Name == OrderFieldNames.OrderInfo2)
                    {
                        query = query.OrderByDescending(o => o.OrderInfo2);
                    }

                    // Other
                    else if (sort.Name == OtherFieldNames.FileName)
                    {
                        query = query.OrderByDescending(o => o.Filename);
                    }
                    else if (sort.Name == OtherFieldNames.CaseNumber)
                    {
                        query = query.OrderByDescending(o => o.CaseNumber);
                    }
                    else if (sort.Name == OtherFieldNames.Info)
                    {
                        query = query.OrderByDescending(o => o.Info);
                    }
                    else if (sort.Name == OtherFieldNames.Status)
                    {
                        query = query.OrderByDescending(o => o.OrderState.Name);
                    }

                    // Program
                    else if (sort.Name == ProgramFieldNames.Program)
                    {
                        query = query.OrderByDescending(o => o.Programs.Count);
                    }

                    // Receiver
                    else if (sort.Name == ReceiverFieldNames.ReceiverId)
                    {
                        query = query.OrderByDescending(o => o.ReceiverId);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverName)
                    {
                        query = query.OrderByDescending(o => o.ReceiverName);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverEmail)
                    {
                        query = query.OrderByDescending(o => o.ReceiverEMail);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverPhone)
                    {
                        query = query.OrderByDescending(o => o.ReceiverPhone);
                    }
                    else if (sort.Name == ReceiverFieldNames.ReceiverLocation)
                    {
                        query = query.OrderByDescending(o => o.ReceiverLocation);
                    }
                    else if (sort.Name == ReceiverFieldNames.MarkOfGoods)
                    {
                        query = query.OrderByDescending(o => o.MarkOfGoods);
                    }

                    // Supplier
                    else if (sort.Name == SupplierFieldNames.SupplierOrderNumber)
                    {
                        query = query.OrderByDescending(o => o.SupplierOrderNumber);
                    }
                    else if (sort.Name == SupplierFieldNames.SupplierOrderDate)
                    {
                        query = query.OrderByDescending(o => o.SupplierOrderDate);
                    }
                    else if (sort.Name == SupplierFieldNames.SupplierOrderInfo)
                    {
                        query = query.OrderByDescending(o => o.SupplierOrderInfo);
                    }

                    // User
                    else if (sort.Name == UserFieldNames.UserId)
                    {
                        query = query.OrderByDescending(o => o.UserId);
                    }
                    else if (sort.Name == UserFieldNames.UserFirstName)
                    {
                        query = query.OrderByDescending(o => o.UserFirstName);
                    }
                    else if (sort.Name == UserFieldNames.UserLastName)
                    {
                        query = query.OrderByDescending(o => o.UserLastName);
                    }

                    break;
            }

            return query;
        }
    }
}