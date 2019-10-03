using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Services.BusinessLogic.Orders.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Types;
    using System.Threading;
    using Domain.Orders;

    public sealed class HistoriesComparator : IHistoriesComparator
    {
        public HistoriesDifference Compare(
            HistoryOverview previousHistory,
            HistoryOverview currentHistory,
            //List<LogOverview> currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs,
            FullOrderEditSettings settings)
        {
            return previousHistory == null
                ? CreateDifferenceForFirstHistory(currentHistory, currentHistoryEmailLogs, settings)
                : CompareHistories(previousHistory, currentHistory, currentHistoryEmailLogs, settings);
        }

        private static HistoriesDifference CreateDifferenceForFirstHistory(
            HistoryOverview firstHistory,
            //List<LogOverview> log,
            List<EmailLogOverview> emailLogs,
            FullOrderEditSettings settings)
        {
            var history = new List<FieldDifference>();

            AddFirstDifference(history, settings.Delivery.DeliveryDate, firstHistory.Order.Delivery.DeliveryDate);
            AddFirstDifference(history, settings.Delivery.InstallDate, firstHistory.Order.Delivery.InstallDate);
            AddFirstDifference(history, settings.Delivery.DeliveryDepartment, firstHistory.Order.Delivery.DeliveryDepartmentId, firstHistory.Order.Delivery.DeliveryDepartment);
            AddFirstDifference(history, settings.Delivery.DeliveryOu, firstHistory.Order.Delivery.DeliveryOu);
            AddFirstDifference(history, settings.Delivery.DeliveryAddress, firstHistory.Order.Delivery.DeliveryAddress);
            AddFirstDifference(history, settings.Delivery.DeliveryPostalCode, firstHistory.Order.Delivery.DeliveryPostalCode);
            AddFirstDifference(history, settings.Delivery.DeliveryPostalAddress, firstHistory.Order.Delivery.DeliveryPostalAddress);
            AddFirstDifference(history, settings.Delivery.DeliveryLocation, firstHistory.Order.Delivery.DeliveryLocation);
            AddFirstDifference(history, settings.Delivery.DeliveryInfo1, firstHistory.Order.Delivery.DeliveryInfo1);
            AddFirstDifference(history, settings.Delivery.DeliveryInfo2, firstHistory.Order.Delivery.DeliveryInfo2);
            AddFirstDifference(history, settings.Delivery.DeliveryInfo3, firstHistory.Order.Delivery.DeliveryInfo3);
            AddFirstDifference(history, settings.Delivery.DeliveryName, firstHistory.Order.Delivery.DeliveryName);
            AddFirstDifference(history, settings.Delivery.DeliveryPhone, firstHistory.Order.Delivery.DeliveryPhone);

            AddFirstDifference(history, settings.General.Administrator, firstHistory.Order.General.AdministratorId, firstHistory.Order.General.Administrator);
            AddFirstDifference(history, settings.General.Domain, firstHistory.Order.General.DomainId, firstHistory.Order.General.Domain);
            AddFirstDifference(history, settings.General.OrderDate, firstHistory.Order.General.OrderDate);
            AddFirstDifference(history, settings.General.Status, firstHistory.Order.General.StatusId, firstHistory.Order.General.Status);

            AddFirstDifference(history, settings.Orderer.OrdererId, firstHistory.Order.Orderer.OrdererId);
            AddFirstDifference(history, settings.Orderer.OrdererName, firstHistory.Order.Orderer.OrdererName);
            AddFirstDifference(history, settings.Orderer.OrdererLocation, firstHistory.Order.Orderer.OrdererLocation);
            AddFirstDifference(history, settings.Orderer.OrdererEmail, firstHistory.Order.Orderer.OrdererEmail);
            AddFirstDifference(history, settings.Orderer.OrdererPhone, firstHistory.Order.Orderer.OrdererPhone);
            AddFirstDifference(history, settings.Orderer.OrdererCode, firstHistory.Order.Orderer.OrdererCode);
            AddFirstDifference(history, settings.Orderer.Department, firstHistory.Order.Orderer.DepartmentId, firstHistory.Order.Orderer.Department);
            AddFirstDifference(history, settings.Orderer.Unit, firstHistory.Order.Orderer.UnitId, firstHistory.Order.Orderer.Unit);
            AddFirstDifference(history, settings.Orderer.OrdererAddress, firstHistory.Order.Orderer.OrdererAddress);
            AddFirstDifference(history, settings.Orderer.OrdererInvoiceAddress, firstHistory.Order.Orderer.OrdererInvoiceAddress);
            AddFirstDifference(history, settings.Orderer.OrdererReferenceNumber, firstHistory.Order.Orderer.OrdererReferenceNumber);
            AddFirstDifference(history, settings.Orderer.AccountingDimension1, firstHistory.Order.Orderer.AccountingDimension1);
            AddFirstDifference(history, settings.Orderer.AccountingDimension2, firstHistory.Order.Orderer.AccountingDimension2);
            AddFirstDifference(history, settings.Orderer.AccountingDimension3, firstHistory.Order.Orderer.AccountingDimension3);
            AddFirstDifference(history, settings.Orderer.AccountingDimension4, firstHistory.Order.Orderer.AccountingDimension4);
            AddFirstDifference(history, settings.Orderer.AccountingDimension5, firstHistory.Order.Orderer.AccountingDimension5);

            AddFirstDifference(history, settings.Order.Property, firstHistory.Order.Order.PropertyId, firstHistory.Order.Order.Property);
            AddFirstDifference(history, settings.Order.OrderRow1, firstHistory.Order.Order.OrderRow1);
            AddFirstDifference(history, settings.Order.OrderRow2, firstHistory.Order.Order.OrderRow2);
            AddFirstDifference(history, settings.Order.OrderRow3, firstHistory.Order.Order.OrderRow3);
            AddFirstDifference(history, settings.Order.OrderRow4, firstHistory.Order.Order.OrderRow4);
            AddFirstDifference(history, settings.Order.OrderRow5, firstHistory.Order.Order.OrderRow5);
            AddFirstDifference(history, settings.Order.OrderRow6, firstHistory.Order.Order.OrderRow6);
            AddFirstDifference(history, settings.Order.OrderRow7, firstHistory.Order.Order.OrderRow7);
            AddFirstDifference(history, settings.Order.OrderRow8, firstHistory.Order.Order.OrderRow8);
            AddFirstDifference(history, settings.Order.Configuration, firstHistory.Order.Order.Configuration);
            AddFirstDifference(history, settings.Order.OrderInfo, firstHistory.Order.Order.OrderInfo);
            AddFirstDifference(history, settings.Order.OrderInfo2, firstHistory.Order.Order.OrderInfo2);

            AddFirstDifference(history, settings.Other.FileName, firstHistory.Order.Other.FileName);
            AddFirstDifference(history, settings.Other.CaseNumber, firstHistory.Order.Other.CaseNumber);
            AddFirstDifference(history, settings.Other.Info, firstHistory.Order.Other.Info);
            //AddFirstDifference(history, settings.Other.Status, firstHistory.Order.Other.StatusId, firstHistory.Order.Other.Status);

            AddFirstDifference(history, settings.Receiver.ReceiverId, firstHistory.Order.Receiver.ReceiverId);
            AddFirstDifference(history, settings.Receiver.ReceiverName, firstHistory.Order.Receiver.ReceiverName);
            AddFirstDifference(history, settings.Receiver.ReceiverEmail, firstHistory.Order.Receiver.ReceiverEmail);
            AddFirstDifference(history, settings.Receiver.ReceiverPhone, firstHistory.Order.Receiver.ReceiverPhone);
            AddFirstDifference(history, settings.Receiver.ReceiverLocation, firstHistory.Order.Receiver.ReceiverLocation);
            AddFirstDifference(history, settings.Receiver.MarkOfGoods, firstHistory.Order.Receiver.MarkOfGoods);

            AddFirstDifference(history, settings.Supplier.SupplierOrderNumber, firstHistory.Order.Supplier.SupplierOrderNumber);
            AddFirstDifference(history, settings.Supplier.SupplierOrderDate, firstHistory.Order.Supplier.SupplierOrderDate);
            AddFirstDifference(history, settings.Supplier.SupplierOrderInfo, firstHistory.Order.Supplier.SupplierOrderInfo);

            AddFirstDifference(history, settings.User.UserId, firstHistory.Order.User.UserId);            
            AddFirstDifference(history, settings.User.UserFirstName, firstHistory.Order.User.UserFirstName);
            AddFirstDifference(history, settings.User.UserLastName, firstHistory.Order.User.UserLastName);
            AddFirstDifference(history, settings.User.UserPhone, firstHistory.Order.User.UserPhone);
            AddFirstDifference(history, settings.User.UserEMail, firstHistory.Order.User.UserEMail);

            AddFirstDifference(history, settings.User.Initials, firstHistory.Order.User.UserInitials);
            AddFirstDifference(history, settings.User.PersonalIdentityNumber, firstHistory.Order.User.UserPersonalIdentityNumber);
            AddFirstDifference(history, settings.User.Extension, firstHistory.Order.User.UserExtension);
            AddFirstDifference(history, settings.User.Title, firstHistory.Order.User.UserTitle);
            AddFirstDifference(history, settings.User.Location, firstHistory.Order.User.UserLocation);
            AddFirstDifference(history, settings.User.RoomNumber, firstHistory.Order.User.UserRoomNumber);
            AddFirstDifference(history, settings.User.PostalAddress, firstHistory.Order.User.UserPostalAddress);
            AddFirstDifference(history, settings.User.Responsibility, firstHistory.Order.User.Responsibility);
            AddFirstDifference(history, settings.User.Activity, firstHistory.Order.User.Activity);
            AddFirstDifference(history, settings.User.Manager, firstHistory.Order.User.Manager);
            AddFirstDifference(history, settings.User.ReferenceNumber, firstHistory.Order.User.ReferenceNumber);
            AddFirstDifference(history, settings.User.Info, firstHistory.Order.User.InfoUser);
            AddFirstDifference(history, settings.User.UnitId, firstHistory.Order.User.UserOU_Id, firstHistory.Order.User.UserOUName);
            AddFirstDifference(history, settings.User.EmploymentType, firstHistory.Order.User.EmploymentType_Id, firstHistory.Order.User.EmploymentTypeName);
            AddFirstDifference(history, settings.User.DepartmentId1, firstHistory.Order.User.UserDepartment_Id1, firstHistory.Order.User.UserDepartmentName);
            AddFirstDifference(history, settings.User.DepartmentId2, firstHistory.Order.User.UserDepartment_Id2, firstHistory.Order.User.UserDepartmentName2);

            AddFirstDifference(history, settings.AccountInfo.StartedDate, firstHistory.Order.AccountInfo.StartedDate);
            AddFirstDifference(history, settings.AccountInfo.FinishDate, firstHistory.Order.AccountInfo.FinishDate);
            //AddFirstDifference(history, settings.AccountInfo.EMailTypeId, firstHistory.Order.AccountInfo.EMailTypeId);
            AddFirstDifference(history, settings.AccountInfo.HomeDirectory, firstHistory.Order.AccountInfo.HomeDirectory);
            AddFirstDifference(history, settings.AccountInfo.Profile, firstHistory.Order.AccountInfo.Profile);
            AddFirstDifference(history, settings.AccountInfo.InventoryNumber, firstHistory.Order.AccountInfo.InventoryNumber);
            AddFirstDifference(history, settings.AccountInfo.Info, firstHistory.Order.AccountInfo.Info);
            AddFirstDifference(history, settings.AccountInfo.AccountTypeId, firstHistory.Order.AccountInfo.AccountTypeId, firstHistory.Order.AccountInfo.AccountTypeIdName);
            //AddFirstDifference(history, settings.AccountInfo.AccountTypeId2, firstHistory.Order.AccountInfo.AccountTypeId2, firstHistory.Order.AccountInfo.AccountTypeId2Name);
            AddFirstDifference(history, settings.AccountInfo.AccountTypeId3, firstHistory.Order.AccountInfo.AccountTypeId3, firstHistory.Order.AccountInfo.AccountTypeId3Name);
            AddFirstDifference(history, settings.AccountInfo.AccountTypeId4, firstHistory.Order.AccountInfo.AccountTypeId4, firstHistory.Order.AccountInfo.AccountTypeId4Name);
            AddFirstDifference(history, settings.AccountInfo.AccountTypeId5, firstHistory.Order.AccountInfo.AccountTypeId5, firstHistory.Order.AccountInfo.AccountTypeId5Name);

            AddFirstDifference(history, settings.Contact.Id, firstHistory.Order.Contact.Id);
            AddFirstDifference(history, settings.Contact.Name, firstHistory.Order.Contact.Name);
            AddFirstDifference(history, settings.Contact.Phone, firstHistory.Order.Contact.Phone);
            AddFirstDifference(history, settings.Contact.EMail, firstHistory.Order.Contact.Email);

            AddFirstDifference(history, settings.Program.InfoProduct, firstHistory.Order.Program.InfoProduct);

            var emails = emailLogs.Select(l => l.Email).ToList();
            //var logs = log.Select(l => l.Text).ToList();

            return new HistoriesDifference(
                firstHistory.DateAndTime,
                firstHistory.RegisteredBy,
                history,
                emails);
        }

        private static HistoriesDifference CompareHistories(
            HistoryOverview previousHistory,
            HistoryOverview currentHistory,
            //List<LogOverview> currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs,
            FullOrderEditSettings settings)
        {
            var history = new List<FieldDifference>();

            AddDifference(history, settings.Delivery.DeliveryDate, previousHistory.Order.Delivery.DeliveryDate, currentHistory.Order.Delivery.DeliveryDate);
            AddDifference(history, settings.Delivery.InstallDate, previousHistory.Order.Delivery.InstallDate, currentHistory.Order.Delivery.InstallDate);
            AddDifference(history, settings.Delivery.DeliveryDepartment, 
                previousHistory.Order.Delivery.DeliveryDepartmentId, previousHistory.Order.Delivery.DeliveryDepartment, 
                currentHistory.Order.Delivery.DeliveryDepartmentId, currentHistory.Order.Delivery.DeliveryDepartment);
            AddDifference(history, settings.Delivery.DeliveryOu, previousHistory.Order.Delivery.DeliveryOu, currentHistory.Order.Delivery.DeliveryOu);
            AddDifference(history, settings.Delivery.DeliveryAddress, previousHistory.Order.Delivery.DeliveryAddress, currentHistory.Order.Delivery.DeliveryAddress);
            AddDifference(history, settings.Delivery.DeliveryPostalCode, previousHistory.Order.Delivery.DeliveryPostalCode, currentHistory.Order.Delivery.DeliveryPostalCode);
            AddDifference(history, settings.Delivery.DeliveryPostalAddress, previousHistory.Order.Delivery.DeliveryPostalAddress, currentHistory.Order.Delivery.DeliveryPostalAddress);
            AddDifference(history, settings.Delivery.DeliveryLocation, previousHistory.Order.Delivery.DeliveryLocation, currentHistory.Order.Delivery.DeliveryLocation);
            AddDifference(history, settings.Delivery.DeliveryInfo1, previousHistory.Order.Delivery.DeliveryInfo1, currentHistory.Order.Delivery.DeliveryInfo1);
            AddDifference(history, settings.Delivery.DeliveryInfo2, previousHistory.Order.Delivery.DeliveryInfo2, currentHistory.Order.Delivery.DeliveryInfo2);
            AddDifference(history, settings.Delivery.DeliveryInfo3, previousHistory.Order.Delivery.DeliveryInfo3, currentHistory.Order.Delivery.DeliveryInfo3);
            AddDifference(history, settings.Delivery.DeliveryName, previousHistory.Order.Delivery.DeliveryName, currentHistory.Order.Delivery.DeliveryName);
            AddDifference(history, settings.Delivery.DeliveryPhone, previousHistory.Order.Delivery.DeliveryPhone, currentHistory.Order.Delivery.DeliveryPhone);

            AddDifference(history, settings.General.Administrator, 
                previousHistory.Order.General.AdministratorId, previousHistory.Order.General.Administrator,
                currentHistory.Order.General.AdministratorId, currentHistory.Order.General.Administrator);
            AddDifference(history, settings.General.Domain, 
                previousHistory.Order.General.DomainId, previousHistory.Order.General.Domain,
                currentHistory.Order.General.DomainId, currentHistory.Order.General.Domain);
            AddDifference(history, settings.General.OrderDate, previousHistory.Order.General.OrderDate, currentHistory.Order.General.OrderDate);
            AddDifference(history, settings.General.Status,
                previousHistory.Order.General.StatusId, previousHistory.Order.General.Status,
                currentHistory.Order.General.StatusId, currentHistory.Order.General.Status);

            AddDifference(history, settings.Orderer.OrdererId, previousHistory.Order.Orderer.OrdererId, currentHistory.Order.Orderer.OrdererId);
            AddDifference(history, settings.Orderer.OrdererName, previousHistory.Order.Orderer.OrdererName, currentHistory.Order.Orderer.OrdererName);
            AddDifference(history, settings.Orderer.OrdererLocation, previousHistory.Order.Orderer.OrdererLocation, currentHistory.Order.Orderer.OrdererLocation);
            AddDifference(history, settings.Orderer.OrdererEmail, previousHistory.Order.Orderer.OrdererEmail, currentHistory.Order.Orderer.OrdererEmail);
            AddDifference(history, settings.Orderer.OrdererPhone, previousHistory.Order.Orderer.OrdererPhone, currentHistory.Order.Orderer.OrdererPhone);
            AddDifference(history, settings.Orderer.OrdererCode, previousHistory.Order.Orderer.OrdererCode, currentHistory.Order.Orderer.OrdererCode);
            AddDifference(history, settings.Orderer.Department, 
                previousHistory.Order.Orderer.DepartmentId, previousHistory.Order.Orderer.Department,
                currentHistory.Order.Orderer.DepartmentId, currentHistory.Order.Orderer.Department);
            AddDifference(history, settings.Orderer.Unit, 
                previousHistory.Order.Orderer.UnitId, previousHistory.Order.Orderer.Unit,
                currentHistory.Order.Orderer.UnitId, currentHistory.Order.Orderer.Unit);
            AddDifference(history, settings.Orderer.OrdererAddress, previousHistory.Order.Orderer.OrdererAddress, currentHistory.Order.Orderer.OrdererAddress);
            AddDifference(history, settings.Orderer.OrdererInvoiceAddress, previousHistory.Order.Orderer.OrdererInvoiceAddress, currentHistory.Order.Orderer.OrdererInvoiceAddress);
            AddDifference(history, settings.Orderer.OrdererReferenceNumber, previousHistory.Order.Orderer.OrdererReferenceNumber, currentHistory.Order.Orderer.OrdererReferenceNumber);
            AddDifference(history, settings.Orderer.AccountingDimension1, previousHistory.Order.Orderer.AccountingDimension1, currentHistory.Order.Orderer.AccountingDimension1);
            AddDifference(history, settings.Orderer.AccountingDimension2, previousHistory.Order.Orderer.AccountingDimension2, currentHistory.Order.Orderer.AccountingDimension2);
            AddDifference(history, settings.Orderer.AccountingDimension3, previousHistory.Order.Orderer.AccountingDimension3, currentHistory.Order.Orderer.AccountingDimension3);
            AddDifference(history, settings.Orderer.AccountingDimension4, previousHistory.Order.Orderer.AccountingDimension4, currentHistory.Order.Orderer.AccountingDimension4);
            AddDifference(history, settings.Orderer.AccountingDimension5, previousHistory.Order.Orderer.AccountingDimension5, currentHistory.Order.Orderer.AccountingDimension5);

            AddDifference(history, settings.Order.Property, 
                previousHistory.Order.Order.PropertyId, previousHistory.Order.Order.Property,
                currentHistory.Order.Order.PropertyId, currentHistory.Order.Order.Property);
            AddDifference(history, settings.Order.OrderRow1, previousHistory.Order.Order.OrderRow1, currentHistory.Order.Order.OrderRow1);
            AddDifference(history, settings.Order.OrderRow2, previousHistory.Order.Order.OrderRow2, currentHistory.Order.Order.OrderRow2);
            AddDifference(history, settings.Order.OrderRow3, previousHistory.Order.Order.OrderRow3, currentHistory.Order.Order.OrderRow3);
            AddDifference(history, settings.Order.OrderRow4, previousHistory.Order.Order.OrderRow4, currentHistory.Order.Order.OrderRow4);
            AddDifference(history, settings.Order.OrderRow5, previousHistory.Order.Order.OrderRow5, currentHistory.Order.Order.OrderRow5);
            AddDifference(history, settings.Order.OrderRow6, previousHistory.Order.Order.OrderRow6, currentHistory.Order.Order.OrderRow6);
            AddDifference(history, settings.Order.OrderRow7, previousHistory.Order.Order.OrderRow7, currentHistory.Order.Order.OrderRow7);
            AddDifference(history, settings.Order.OrderRow8, previousHistory.Order.Order.OrderRow8, currentHistory.Order.Order.OrderRow8);
            AddDifference(history, settings.Order.Configuration, previousHistory.Order.Order.Configuration, currentHistory.Order.Order.Configuration);
            AddDifference(history, settings.Order.OrderInfo, previousHistory.Order.Order.OrderInfo, currentHistory.Order.Order.OrderInfo);
            AddDifference(history, settings.Order.OrderInfo2, previousHistory.Order.Order.OrderInfo2, currentHistory.Order.Order.OrderInfo2);

            AddDifference(history, settings.Other.FileName, previousHistory.Order.Other.FileName, currentHistory.Order.Other.FileName);
            AddDifference(history, settings.Other.CaseNumber, previousHistory.Order.Other.CaseNumber, currentHistory.Order.Other.CaseNumber);
            AddDifference(history, settings.Other.Info, previousHistory.Order.Other.Info, currentHistory.Order.Other.Info);
            //AddDifference(history, settings.Other.Status, 
            //    previousHistory.Order.Other.StatusId, previousHistory.Order.Other.Status,
            //    currentHistory.Order.Other.StatusId, currentHistory.Order.Other.Status);

            AddDifference(history, settings.Receiver.ReceiverId, previousHistory.Order.Receiver.ReceiverId, currentHistory.Order.Receiver.ReceiverId);
            AddDifference(history, settings.Receiver.ReceiverName, previousHistory.Order.Receiver.ReceiverName, currentHistory.Order.Receiver.ReceiverName);
            AddDifference(history, settings.Receiver.ReceiverEmail, previousHistory.Order.Receiver.ReceiverEmail, currentHistory.Order.Receiver.ReceiverEmail);
            AddDifference(history, settings.Receiver.ReceiverPhone, previousHistory.Order.Receiver.ReceiverPhone, currentHistory.Order.Receiver.ReceiverPhone);
            AddDifference(history, settings.Receiver.ReceiverLocation, previousHistory.Order.Receiver.ReceiverLocation, currentHistory.Order.Receiver.ReceiverLocation);
            AddDifference(history, settings.Receiver.MarkOfGoods, previousHistory.Order.Receiver.MarkOfGoods, currentHistory.Order.Receiver.MarkOfGoods);

            AddDifference(history, settings.Supplier.SupplierOrderNumber, previousHistory.Order.Supplier.SupplierOrderNumber, currentHistory.Order.Supplier.SupplierOrderNumber);
            AddDifference(history, settings.Supplier.SupplierOrderDate, previousHistory.Order.Supplier.SupplierOrderDate, currentHistory.Order.Supplier.SupplierOrderDate);
            AddDifference(history, settings.Supplier.SupplierOrderInfo, previousHistory.Order.Supplier.SupplierOrderInfo, currentHistory.Order.Supplier.SupplierOrderInfo);

            AddDifference(history, settings.User.UserId, previousHistory.Order.User.UserId, currentHistory.Order.User.UserId);
            AddDifference(history, settings.User.UserFirstName, previousHistory.Order.User.UserFirstName, currentHistory.Order.User.UserFirstName);
            AddDifference(history, settings.User.UserLastName, previousHistory.Order.User.UserLastName, currentHistory.Order.User.UserLastName);
            AddDifference(history, settings.User.UserPhone, previousHistory.Order.User.UserPhone, currentHistory.Order.User.UserPhone);
            AddDifference(history, settings.User.UserEMail, previousHistory.Order.User.UserEMail, currentHistory.Order.User.UserEMail);

            AddDifference(history, settings.User.Initials, previousHistory.Order.User.UserInitials, currentHistory.Order.User.UserInitials);
            AddDifference(history, settings.User.PersonalIdentityNumber, previousHistory.Order.User.UserPersonalIdentityNumber, currentHistory.Order.User.UserPersonalIdentityNumber);
            AddDifference(history, settings.User.Extension, previousHistory.Order.User.UserExtension, currentHistory.Order.User.UserExtension);
            AddDifference(history, settings.User.Title, previousHistory.Order.User.UserTitle, currentHistory.Order.User.UserTitle);
            AddDifference(history, settings.User.Location, previousHistory.Order.User.UserLocation, currentHistory.Order.User.UserLocation);
            AddDifference(history, settings.User.RoomNumber, previousHistory.Order.User.UserRoomNumber, currentHistory.Order.User.UserRoomNumber);
            AddDifference(history, settings.User.PostalAddress, previousHistory.Order.User.UserPostalAddress, currentHistory.Order.User.UserPostalAddress);
            AddDifference(history, settings.User.Responsibility, previousHistory.Order.User.Responsibility, currentHistory.Order.User.Responsibility);
            AddDifference(history, settings.User.Activity, previousHistory.Order.User.Activity, currentHistory.Order.User.Activity);
            AddDifference(history, settings.User.Manager, previousHistory.Order.User.Manager, currentHistory.Order.User.Manager);
            AddDifference(history, settings.User.ReferenceNumber, previousHistory.Order.User.ReferenceNumber, currentHistory.Order.User.ReferenceNumber);
            AddDifference(history, settings.User.Info, previousHistory.Order.User.InfoUser, currentHistory.Order.User.InfoUser);
            AddDifference(history, settings.User.UnitId,
                                    previousHistory.Order.User.UserOU_Id, previousHistory.Order.User.UserOUName,
                                    currentHistory.Order.User.UserOU_Id, currentHistory.Order.User.UserOUName);
            AddDifference(history, settings.User.EmploymentType,
                                    previousHistory.Order.User.EmploymentType_Id, previousHistory.Order.User.EmploymentTypeName,
                                    currentHistory.Order.User.EmploymentType_Id, currentHistory.Order.User.EmploymentTypeName);
            AddDifference(history, settings.User.DepartmentId1,
                                    previousHistory.Order.User.UserDepartment_Id1, previousHistory.Order.User.UserDepartmentName,
                                    currentHistory.Order.User.UserDepartment_Id1, currentHistory.Order.User.UserDepartmentName);
            AddDifference(history, settings.User.DepartmentId2,
                                    previousHistory.Order.User.UserDepartment_Id2, previousHistory.Order.User.UserDepartmentName2,
                                    currentHistory.Order.User.UserDepartment_Id2, currentHistory.Order.User.UserDepartmentName2);

            AddDifference(history, settings.AccountInfo.StartedDate, previousHistory.Order.AccountInfo.StartedDate, currentHistory.Order.AccountInfo.StartedDate);
            AddDifference(history, settings.AccountInfo.FinishDate, previousHistory.Order.AccountInfo.FinishDate, currentHistory.Order.AccountInfo.FinishDate);
            //AddDifference(history, settings.AccountInfo.EMailTypeId, previousHistory.Order.AccountInfo.EMailTypeId);
            AddDifference(history, settings.AccountInfo.HomeDirectory, previousHistory.Order.AccountInfo.HomeDirectory, currentHistory.Order.AccountInfo.HomeDirectory);
            AddDifference(history, settings.AccountInfo.Profile, previousHistory.Order.AccountInfo.Profile, currentHistory.Order.AccountInfo.Profile);
            AddDifference(history, settings.AccountInfo.InventoryNumber, previousHistory.Order.AccountInfo.InventoryNumber, currentHistory.Order.AccountInfo.InventoryNumber);
            AddDifference(history, settings.AccountInfo.Info, previousHistory.Order.AccountInfo.Info, currentHistory.Order.AccountInfo.Info);
            AddDifference(history, settings.AccountInfo.AccountTypeId,
                                    previousHistory.Order.AccountInfo.AccountTypeId, previousHistory.Order.AccountInfo.AccountTypeIdName,
                                    currentHistory.Order.AccountInfo.AccountTypeId, currentHistory.Order.AccountInfo.AccountTypeIdName);
            //AddDifference(history, settings.AccountInfo.AccountTypeId2,
            //                        previousHistory.Order.AccountInfo.AccountTypeId2, previousHistory.Order.AccountInfo.AccountTypeId2Name,
            //                        currentHistory.Order.AccountInfo.AccountTypeId2, currentHistory.Order.AccountInfo.AccountTypeId2Name);
            AddDifference(history, settings.AccountInfo.AccountTypeId3,
                                    previousHistory.Order.AccountInfo.AccountTypeId3, previousHistory.Order.AccountInfo.AccountTypeId3Name,
                                    currentHistory.Order.AccountInfo.AccountTypeId3, currentHistory.Order.AccountInfo.AccountTypeId3Name);
            AddDifference(history, settings.AccountInfo.AccountTypeId4,
                                    previousHistory.Order.AccountInfo.AccountTypeId4, previousHistory.Order.AccountInfo.AccountTypeId4Name,
                                    currentHistory.Order.AccountInfo.AccountTypeId4, currentHistory.Order.AccountInfo.AccountTypeId4Name);
            AddDifference(history, settings.AccountInfo.AccountTypeId5,
                                    previousHistory.Order.AccountInfo.AccountTypeId5, previousHistory.Order.AccountInfo.AccountTypeId5Name,
                                    currentHistory.Order.AccountInfo.AccountTypeId5, currentHistory.Order.AccountInfo.AccountTypeId5Name);

            AddDifference(history, settings.Contact.Id, previousHistory.Order.Contact.Id, currentHistory.Order.Contact.Id);
            AddDifference(history, settings.Contact.Name, previousHistory.Order.Contact.Name, currentHistory.Order.Contact.Name);
            AddDifference(history, settings.Contact.Phone, previousHistory.Order.Contact.Phone, currentHistory.Order.Contact.Phone);
            AddDifference(history, settings.Contact.EMail, previousHistory.Order.Contact.Email, currentHistory.Order.Contact.Email);

            AddDifference(history, settings.Program.InfoProduct, previousHistory.Order.Program.InfoProduct, currentHistory.Order.Program.InfoProduct);

            var emails = currentHistoryEmailLogs.Select(l => l.Email).ToList();
            //var logs = currentHistoryLog.Select(l => l.Text).ToList();

            if (history.Any() || emails.Any())
            {
                return new HistoriesDifference(
                currentHistory.DateAndTime,
                currentHistory.RegisteredBy,
                //logs,
                history,
                emails);
            }

            return null;
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, int? firstValue, string firstText)
        {
            if (firstValue.HasValue)
            {
                var difference = new FieldDifference(settings.Caption, null, firstText);
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, string firstText)
        {
            if (!string.IsNullOrEmpty(firstText))
            {
                var difference = new FieldDifference(settings.Caption, null, firstText);
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, int? firstValue, UserName firstUserName)
        {
            if (firstValue.HasValue)
            {
                var difference = new FieldDifference(settings.Caption, null, firstUserName.GetReversedFullName());
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, DateTime? firstValue)
        {
            if (firstValue.HasValue)
            {
                var difference = new FieldDifference(settings.Caption, null, firstValue.Value.ToString(DateFormats.Date, Thread.CurrentThread.CurrentUICulture));
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, int firstValue)
        {
            if (firstValue != 0)
            {
                var difference = new FieldDifference(settings.Caption, null, firstValue.ToString(CultureInfo.InvariantCulture));
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, bool? firstValue)
        {
            if (firstValue.HasValue)
            {
                var difference = new FieldDifference(settings.Caption, null, firstValue.Value.ToString(CultureInfo.InvariantCulture));
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, decimal? firstValue)
        {
            if (firstValue.HasValue)
            {
                var difference = new FieldDifference(settings.Caption, null, firstValue.Value.ToString(CultureInfo.InvariantCulture));
                differencies.Add(difference);
            }
        }


        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, int? oldValue, string oldText, int? newValue, string newText)
        {
            if (oldValue != newValue)
            {
                var difference = new FieldDifference(settings.Caption, oldText, newText);
                differencies.Add(difference);
            }
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, string oldText, string newText)
        {
            if (!oldText.EqualWith(newText))
            {
                var difference = new FieldDifference(settings.Caption, oldText, newText);
                differencies.Add(difference);
            }
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, int? oldValue, UserName oldUserName, int? newValue, UserName newUserName)
        {
            var oldText = oldUserName != null ? oldUserName.GetReversedFullName() : string.Empty;
            var newText = newUserName != null ? newUserName.GetReversedFullName() : string.Empty;

            AddDifference(differencies, settings, oldValue, oldText, newValue, newText);
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, DateTime? oldValue, DateTime? newValue)
        {
            if (oldValue != newValue)
            {
                var difference = new FieldDifference(                            
                            settings.Caption,
                            oldValue.HasValue ? oldValue.Value.ToString(DateFormats.Date, Thread.CurrentThread.CurrentUICulture) : string.Empty,
                            newValue.HasValue ? newValue.Value.ToString(DateFormats.Date, Thread.CurrentThread.CurrentUICulture) : string.Empty);
                differencies.Add(difference);
            }
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, int oldValue, int newValue)
        {
            if (oldValue != newValue)
            {
                var difference = new FieldDifference(                            
                            settings.Caption, 
                            oldValue.ToString(CultureInfo.InvariantCulture),
                            newValue.ToString(CultureInfo.InvariantCulture));
                differencies.Add(difference);
            }
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, bool? oldValue, bool? newValue)
        {
            var noValueText = "null";
            if (oldValue != newValue)
            {
                var difference = new FieldDifference(
                            settings.Caption,
                            oldValue?.ToString(CultureInfo.InvariantCulture) ?? noValueText,
                            newValue?.ToString(CultureInfo.InvariantCulture) ?? noValueText);
                differencies.Add(difference);
            }
        }


        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, decimal? oldValue, decimal? newValue)
        {
            if (oldValue != newValue)
            {
                var difference = new FieldDifference(                            
                            settings.Caption, 
                            oldValue.HasValue ? oldValue.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
                            newValue.HasValue ? newValue.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
                differencies.Add(difference);
            }
        }
    }
}