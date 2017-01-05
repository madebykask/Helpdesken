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

		public static IQueryable<Order> GetByTypes(this IQueryable<Order> query, int[] orderTypeIds)
		{
			if (orderTypeIds == null || !orderTypeIds.Any())
			{
				return query;
			}

			query = query.Where(o => o.OrderType_Id.HasValue && orderTypeIds.Contains(o.OrderType_Id.Value));

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
		        o.DeliveryName.Trim().ToLower().Contains(search) ||
		        o.DeliveryPhone.Trim().ToLower().Contains(search) ||

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
		        o.UserLastName.Trim().ToLower().Contains(search) ||
		        o.UserEMail.Trim().ToLower().Contains(search) ||
		        o.UserPhone.Trim().ToLower().Contains(search) ||
		        o.UserLocation.Trim().ToLower().Contains(search) ||
		        o.UserPersonalIdentityNumber.Trim().ToLower().Contains(search) ||
		        o.UserTitle.Trim().ToLower().Contains(search) ||
		        o.InfoUser.Trim().ToLower().Contains(search) ||
		        o.Responsibility.Trim().ToLower().Contains(search) ||
		        o.Activity.Trim().ToLower().Contains(search) ||
		        o.Manager.Trim().ToLower().Contains(search) ||
		        o.ReferenceNumber.Trim().ToLower().Contains(search) ||
		        o.UserDepartment1.DepartmentName.Trim().ToLower().Contains(search) ||
		        o.UserDepartment2.DepartmentName.Trim().ToLower().Contains(search) ||
		        o.UserOU.Name.Trim().ToLower().Contains(search) ||
                o.EmploymentType.Name.Trim().ToLower().Contains(search) ||

                //Account
                o.InventoryNumber.Trim().ToLower().Contains(search) ||
		        o.AccountInfo.Trim().ToLower().Contains(search) ||
		        o.OrderFieldType.Name.Trim().ToLower().Contains(search) ||
		        //o.OrderFieldType2.Name.Trim().ToLower().Contains(search) ||
		        o.OrderFieldType3.Name.Trim().ToLower().Contains(search) ||
		        o.OrderFieldType4.Name.Trim().ToLower().Contains(search) ||
		        o.OrderFieldType5.Name.Trim().ToLower().Contains(search) ||

		        //Contact
		        o.ContactName.Trim().ToLower().Contains(search) ||
		        o.ContactPhone.Trim().ToLower().Contains(search) ||
		        o.ContactEMail.Trim().ToLower().Contains(search) ||

		        //Program
		        o.InfoProduct.Trim().ToLower().Contains(search));

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

		public static IQueryable<Order> Search(
						this IQueryable<Order> query,
						int customerId,
						int[] orderTypeIds,
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
						.GetByTypes(orderTypeIds)
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

		public static IQueryable<OrderType> GetRootOrderTypes(this IQueryable<OrderType> query, int customerId)
		{
			query = query.GetByCustomer(customerId)
					.Where(t => t.IsActive == 1 && t.Parent_OrderType_Id == null);

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
			var querySortedByType = query.OrderBy(x => x.OrderType.Name);

			if (sort == null)
			{
				return querySortedByType;
			}

			switch (sort.SortBy)
			{
				case SortBy.Ascending:
					switch (sort.Name)
					{
                        // Delivery
                        case DeliveryFieldNames.DeliveryDate:
					        query = (querySortedByType).ThenBy(o => o.Deliverydate);
					        break;
					    case DeliveryFieldNames.InstallDate:
					        query = querySortedByType.ThenBy(o => o.InstallDate);
					        break;
					    case DeliveryFieldNames.DeliveryDepartment:
					        query = querySortedByType.ThenBy(o => o.DeliveryDepartment.DepartmentName);
					        break;
					    case DeliveryFieldNames.DeliveryOu:
					        query = querySortedByType.ThenBy(o => o.DeliveryOu);
					        break;
					    case DeliveryFieldNames.DeliveryAddress:
					        query = querySortedByType.ThenBy(o => o.DeliveryAddress);
					        break;
					    case DeliveryFieldNames.DeliveryPostalCode:
					        query = querySortedByType.ThenBy(o => o.DeliveryPostalCode);
					        break;
					    case DeliveryFieldNames.DeliveryPostalAddress:
					        query = querySortedByType.ThenBy(o => o.DeliveryPostalAddress);
					        break;
					    case DeliveryFieldNames.DeliveryLocation:
					        query = querySortedByType.ThenBy(o => o.DeliveryLocation);
					        break;
					    case DeliveryFieldNames.DeliveryInfo1:
					        query = querySortedByType.ThenBy(o => o.DeliveryInfo);
					        break;
					    case DeliveryFieldNames.DeliveryInfo2:
					        query = querySortedByType.ThenBy(o => o.DeliveryInfo2);
					        break;
					    case DeliveryFieldNames.DeliveryInfo3:
					        query = querySortedByType.ThenBy(o => o.DeliveryInfo3);
					        break;
					    case DeliveryFieldNames.DeliveryOuId:
					        query = querySortedByType.ThenBy(o => o.DeliveryOuEntity.Name);
					        break;
                        case DeliveryFieldNames.DeliveryName:
                            query = querySortedByType.ThenBy(o => o.DeliveryName);
                            break;
                        case DeliveryFieldNames.DeliveryPhone:
                            query = querySortedByType.ThenBy(o => o.DeliveryPhone);
                            break;

                        // General
                        case GeneralFieldNames.OrderNumber:
					        query = querySortedByType.ThenBy(o => o.Id);
					        break;
					    case GeneralFieldNames.Customer:
					        query = querySortedByType.ThenBy(o => o.Customer.Name);
					        break;
					    case GeneralFieldNames.Administrator:
					        query = querySortedByType.ThenBy(o => o.User_Id);
					        break;
					    case GeneralFieldNames.Domain:
					        query = querySortedByType.ThenBy(o => o.Domain.Name);
					        break;
					    case GeneralFieldNames.OrderDate:
					        query = querySortedByType.ThenBy(o => o.OrderDate);
					        break;
                        //Log
					    case LogFieldNames.Log:
					        query = querySortedByType.ThenBy(l => l.Logs.Count);
					        break;
                        //Orderer
					    case OrdererFieldNames.OrdererId:
					        query = querySortedByType.ThenBy(o => o.OrdererID);
					        break;
					    case OrdererFieldNames.OrdererName:
					        query = querySortedByType.ThenBy(o => o.Orderer);
					        break;
					    case OrdererFieldNames.OrdererLocation:
					        query = querySortedByType.ThenBy(o => o.OrdererLocation);
					        break;
					    case OrdererFieldNames.OrdererEmail:
					        query = querySortedByType.ThenBy(o => o.OrdererEMail);
					        break;
					    case OrdererFieldNames.OrdererPhone:
					        query = querySortedByType.ThenBy(o => o.OrdererPhone);
					        break;
					    case OrdererFieldNames.OrdererCode:
					        query = querySortedByType.ThenBy(o => o.OrdererCode);
					        break;
					    case OrdererFieldNames.Department:
					        query = querySortedByType.ThenBy(o => o.Department_Id);
					        break;
					    case OrdererFieldNames.Unit:
					        query = querySortedByType.ThenBy(o => o.Ou.Name);
					        break;
					    case OrdererFieldNames.OrdererAddress:
					        query = querySortedByType.ThenBy(o => o.OrdererAddress);
					        break;
					    case OrdererFieldNames.OrdererInvoiceAddress:
					        query = querySortedByType.ThenBy(o => o.OrdererInvoiceAddress);
					        break;
					    case OrdererFieldNames.OrdererReferenceNumber:
					        query = querySortedByType.ThenBy(o => o.OrdererReferenceNumber);
					        break;
					    case OrdererFieldNames.AccountingDimension1:
					        query = querySortedByType.ThenBy(o => o.AccountingDimension1);
					        break;
					    case OrdererFieldNames.AccountingDimension2:
					        query = querySortedByType.ThenBy(o => o.AccountingDimension2);
					        break;
					    case OrdererFieldNames.AccountingDimension3:
					        query = querySortedByType.ThenBy(o => o.AccountingDimension3);
					        break;
					    case OrdererFieldNames.AccountingDimension4:
					        query = querySortedByType.ThenBy(o => o.AccountingDimension4);
					        break;
					    case OrdererFieldNames.AccountingDimension5:
					        query = querySortedByType.ThenBy(o => o.AccountingDimension5);
					        break;
                        //Order
					    case OrderFieldNames.Property:
					        query = querySortedByType.ThenBy(o => o.OrderProperty.OrderProperty);
					        break;
					    case OrderFieldNames.OrderRow1:
					        query = querySortedByType.ThenBy(o => o.OrderRow);
					        break;
					    case OrderFieldNames.OrderRow2:
					        query = querySortedByType.ThenBy(o => o.OrderRow2);
					        break;
					    case OrderFieldNames.OrderRow3:
					        query = querySortedByType.ThenBy(o => o.OrderRow3);
					        break;
					    case OrderFieldNames.OrderRow4:
					        query = querySortedByType.ThenBy(o => o.OrderRow4);
					        break;
					    case OrderFieldNames.OrderRow5:
					        query = querySortedByType.ThenBy(o => o.OrderRow5);
					        break;
					    case OrderFieldNames.OrderRow6:
					        query = querySortedByType.ThenBy(o => o.OrderRow6);
					        break;
					    case OrderFieldNames.OrderRow7:
					        query = querySortedByType.ThenBy(o => o.OrderRow7);
					        break;
					    case OrderFieldNames.OrderRow8:
					        query = querySortedByType.ThenBy(o => o.OrderRow8);
					        break;
					    case OrderFieldNames.Configuration:
					        query = querySortedByType.ThenBy(o => o.Configuration);
					        break;
					    case OrderFieldNames.OrderInfo:
					        query = querySortedByType.ThenBy(o => o.OrderInfo);
					        break;
					    case OrderFieldNames.OrderInfo2:
					        query = querySortedByType.ThenBy(o => o.OrderInfo2);
					        break;
                        // Other
                        case OtherFieldNames.FileName:
					        query = querySortedByType.ThenBy(o => o.Filename);
					        break;
					    case OtherFieldNames.CaseNumber:
					        query = querySortedByType.ThenBy(o => o.CaseNumber);
					        break;
					    case OtherFieldNames.Info:
					        query = querySortedByType.ThenBy(o => o.Info);
					        break;
					    case OtherFieldNames.Status:
					        query = querySortedByType.ThenBy(o => o.OrderState.Name);
					        break;
                        //Programm
					    case ProgramFieldNames.Program:
					        query = querySortedByType.ThenBy(o => o.Programs.Count);
					        break;
                        case ProgramFieldNames.InfoProduct:
                            query = querySortedByType.ThenBy(o => o.InfoProduct);
                            break;
                        //Reciever
                        case ReceiverFieldNames.ReceiverId:
					        query = querySortedByType.ThenBy(o => o.ReceiverId);
					        break;
					    case ReceiverFieldNames.ReceiverName:
					        query = querySortedByType.ThenBy(o => o.ReceiverName);
					        break;
					    case ReceiverFieldNames.ReceiverEmail:
					        query = querySortedByType.ThenBy(o => o.ReceiverEMail);
					        break;
					    case ReceiverFieldNames.ReceiverPhone:
					        query = querySortedByType.ThenBy(o => o.ReceiverPhone);
					        break;
					    case ReceiverFieldNames.ReceiverLocation:
					        query = querySortedByType.ThenBy(o => o.ReceiverLocation);
					        break;
					    case ReceiverFieldNames.MarkOfGoods:
					        query = querySortedByType.ThenBy(o => o.MarkOfGoods);
					        break;
                        //Supplier
					    case SupplierFieldNames.SupplierOrderNumber:
					        query = querySortedByType.ThenBy(o => o.SupplierOrderNumber);
					        break;
					    case SupplierFieldNames.SupplierOrderDate:
					        query = querySortedByType.ThenBy(o => o.SupplierOrderDate);
					        break;
					    case SupplierFieldNames.SupplierOrderInfo:
					        query = querySortedByType.ThenBy(o => o.SupplierOrderInfo);
					        break;
                        //User
					    case UserFieldNames.UserId:
					        query = querySortedByType.ThenBy(o => o.UserId);
					        break;
					    case UserFieldNames.UserFirstName:
					        query = querySortedByType.ThenBy(o => o.UserFirstName);
					        break;
					    case UserFieldNames.UserLastName:
					        query = querySortedByType.ThenBy(o => o.UserLastName);
					        break;
					    case UserFieldNames.UserPhone:
					        query = querySortedByType.ThenBy(o => o.UserPhone);
					        break;
					    case UserFieldNames.UserEMail:
					        query = querySortedByType.ThenBy(o => o.UserEMail);
					        break;
					    case UserFieldNames.UserInitials:
					        query = querySortedByType.ThenBy(o => o.UserInitials);
					        break;
					    case UserFieldNames.UserPersonalIdentityNumber:
					        query = querySortedByType.ThenBy(o => o.UserPersonalIdentityNumber);
					        break;
					    case UserFieldNames.UserExtension:
					        query = querySortedByType.ThenBy(o => o.UserExtension);
					        break;
					    case UserFieldNames.UserTitle:
					        query = querySortedByType.ThenBy(o => o.UserTitle);
					        break;
					    case UserFieldNames.UserLocation:
					        query = querySortedByType.ThenBy(o => o.UserLocation);
					        break;
					    case UserFieldNames.UserRoomNumber:
					        query = querySortedByType.ThenBy(o => o.UserRoomNumber);
					        break;
					    case UserFieldNames.UserPostalAddress:
					        query = querySortedByType.ThenBy(o => o.UserPostalAddress);
					        break;
					    case UserFieldNames.Responsibility:
					        query = querySortedByType.ThenBy(o => o.Responsibility);
					        break;
					    case UserFieldNames.Activity:
					        query = querySortedByType.ThenBy(o => o.Activity);
					        break;
					    case UserFieldNames.Manager:
					        query = querySortedByType.ThenBy(o => o.Manager);
					        break;
					    case UserFieldNames.ReferenceNumber:
					        query = querySortedByType.ThenBy(o => o.ReferenceNumber);
					        break;
					    case UserFieldNames.InfoUser:
					        query = querySortedByType.ThenBy(o => o.InfoUser);
					        break;
					    case UserFieldNames.UserOU_Id:
					        query = querySortedByType.ThenBy(o => o.UserOU.Name);
					        break;
					    case UserFieldNames.EmploymentType:
					        query = querySortedByType.ThenBy(o => o.EmploymentType.Name);
					        break;
					    case UserFieldNames.UserDepartment_Id1:
					        query = querySortedByType.ThenBy(o => o.UserDepartment1.DepartmentName);
					        break;
                        case UserFieldNames.UserDepartment_Id2:
                            query = querySortedByType.ThenBy(o => o.UserDepartment2.DepartmentName);
                            break;

                        //Account Info
                        case AccountInfoFieldNames.StartedDate:
                            query = querySortedByType.ThenBy(o => o.AccountStartDate);
                            break;
                        case AccountInfoFieldNames.FinishDate:
                            query = querySortedByType.ThenBy(o => o.AccountEndDate);
                            break;
                        case AccountInfoFieldNames.HomeDirectory:
                            query = querySortedByType.ThenBy(o => o.HomeDirectory);
                            break;
                        case AccountInfoFieldNames.Profile:
                            query = querySortedByType.ThenBy(o => o.Profile);
                            break;
                        //case AccountInfoFieldNames.EMailTypeId:
                        //    query = querySortedByType.ThenBy(o => o.Profile);
                        //    break;
                        case AccountInfoFieldNames.InventoryNumber:
                            query = querySortedByType.ThenBy(o => o.InventoryNumber);
                            break;
                        case AccountInfoFieldNames.Info:
                            query = querySortedByType.ThenBy(o => o.AccountInfo);
                            break;
                        case AccountInfoFieldNames.AccountTypeId:
                            query = querySortedByType.ThenBy(o => o.OrderFieldType.Name);
                            break;
                        //case AccountInfoFieldNames.AccountTypeId2:
                        //    query = querySortedByType.ThenBy(o => o.OrderFieldType2.Name);
                        //    break;
                        case AccountInfoFieldNames.AccountTypeId3:
                            query = querySortedByType.ThenBy(o => o.OrderFieldType3.Name);
                            break;
                        case AccountInfoFieldNames.AccountTypeId4:
                            query = querySortedByType.ThenBy(o => o.OrderFieldType4.Name);
                            break;
                        case AccountInfoFieldNames.AccountTypeId5:
                            query = querySortedByType.ThenBy(o => o.OrderFieldType5.Name);
                            break;
                        //Contact
                        case ContactFieldNames.Id:
                            query = querySortedByType.ThenBy(o => o.ContactId);
                            break;
                        case ContactFieldNames.EMail:
                            query = querySortedByType.ThenBy(o => o.ContactEMail);
                            break;
                        case ContactFieldNames.Name:
                            query = querySortedByType.ThenBy(o => o.ContactName);
                            break;
                        case ContactFieldNames.Phone:
                            query = querySortedByType.ThenBy(o => o.ContactPhone);
                            break;
                    }
                    break;

				case SortBy.Descending:
					switch (sort.Name)
					{
                        // Delivery
                        case DeliveryFieldNames.DeliveryDate:
					        query = querySortedByType.ThenByDescending(o => o.Deliverydate);
					        break;
					    case DeliveryFieldNames.InstallDate:
					        query = querySortedByType.ThenByDescending(o => o.InstallDate);
					        break;
					    case DeliveryFieldNames.DeliveryDepartment:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryDepartment.DepartmentName);
					        break;
					    case DeliveryFieldNames.DeliveryOu:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryOu);
					        break;
					    case DeliveryFieldNames.DeliveryAddress:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryAddress);
					        break;
					    case DeliveryFieldNames.DeliveryPostalCode:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryPostalCode);
					        break;
					    case DeliveryFieldNames.DeliveryPostalAddress:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryPostalAddress);
					        break;
					    case DeliveryFieldNames.DeliveryLocation:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryLocation);
					        break;
					    case DeliveryFieldNames.DeliveryInfo1:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryInfo);
					        break;
					    case DeliveryFieldNames.DeliveryInfo2:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryInfo2);
					        break;
					    case DeliveryFieldNames.DeliveryInfo3:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryInfo3);
					        break;
					    case DeliveryFieldNames.DeliveryOuId:
					        query = querySortedByType.ThenByDescending(o => o.DeliveryOuEntity.Name);
					        break;
                        case DeliveryFieldNames.DeliveryName:
                            query = querySortedByType.ThenByDescending(o => o.DeliveryName);
                            break;
                        case DeliveryFieldNames.DeliveryPhone:
                            query = querySortedByType.ThenByDescending(o => o.DeliveryPhone);
                            break;
                        //General
                        case GeneralFieldNames.OrderNumber:
					        query = querySortedByType.ThenByDescending(o => o.Id);
					        break;
					    case GeneralFieldNames.Customer:
					        query = querySortedByType.ThenByDescending(o => o.Customer.Name);
					        break;
					    case GeneralFieldNames.Administrator:
					        query = querySortedByType.ThenByDescending(o => o.User_Id);
					        break;
					    case GeneralFieldNames.Domain:
					        query = querySortedByType.ThenByDescending(o => o.Domain.Name);
					        break;
					    case GeneralFieldNames.OrderDate:
					        query = querySortedByType.ThenByDescending(o => o.OrderDate);
					        break;
                        //Log
					    case LogFieldNames.Log:
					        query = querySortedByType.ThenByDescending(l => l.Logs.Count);
					        break;
                        //Orderer
					    case OrdererFieldNames.OrdererId:
					        query = querySortedByType.ThenByDescending(o => o.OrdererID);
					        break;
					    case OrdererFieldNames.OrdererName:
					        query = querySortedByType.ThenByDescending(o => o.Orderer);
					        break;
					    case OrdererFieldNames.OrdererLocation:
					        query = querySortedByType.ThenByDescending(o => o.OrdererLocation);
					        break;
					    case OrdererFieldNames.OrdererEmail:
					        query = querySortedByType.ThenByDescending(o => o.OrdererEMail);
					        break;
					    case OrdererFieldNames.OrdererPhone:
					        query = querySortedByType.ThenByDescending(o => o.OrdererPhone);
					        break;
					    case OrdererFieldNames.OrdererCode:
					        query = querySortedByType.ThenByDescending(o => o.OrdererCode);
					        break;
					    case OrdererFieldNames.Department:
					        query = querySortedByType.ThenByDescending(o => o.Department_Id);
					        break;
					    case OrdererFieldNames.Unit:
					        query = querySortedByType.ThenByDescending(o => o.Ou.Name);
					        break;
					    case OrdererFieldNames.OrdererAddress:
					        query = querySortedByType.ThenByDescending(o => o.OrdererAddress);
					        break;
					    case OrdererFieldNames.OrdererInvoiceAddress:
					        query = querySortedByType.ThenByDescending(o => o.OrdererInvoiceAddress);
					        break;
					    case OrdererFieldNames.OrdererReferenceNumber:
					        query = querySortedByType.ThenByDescending(o => o.OrdererReferenceNumber);
					        break;
					    case OrdererFieldNames.AccountingDimension1:
					        query = querySortedByType.ThenByDescending(o => o.AccountingDimension1);
					        break;
					    case OrdererFieldNames.AccountingDimension2:
					        query = querySortedByType.ThenByDescending(o => o.AccountingDimension2);
					        break;
					    case OrdererFieldNames.AccountingDimension3:
					        query = querySortedByType.ThenByDescending(o => o.AccountingDimension3);
					        break;
					    case OrdererFieldNames.AccountingDimension4:
					        query = querySortedByType.ThenByDescending(o => o.AccountingDimension4);
					        break;
					    case OrdererFieldNames.AccountingDimension5:
					        query = querySortedByType.ThenByDescending(o => o.AccountingDimension5);
					        break;
                        // Order
                        case OrderFieldNames.Property:
					        query = querySortedByType.ThenByDescending(o => o.OrderProperty.OrderProperty);
					        break;
					    case OrderFieldNames.OrderRow1:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow);
					        break;
					    case OrderFieldNames.OrderRow2:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow2);
					        break;
					    case OrderFieldNames.OrderRow3:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow3);
					        break;
					    case OrderFieldNames.OrderRow4:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow4);
					        break;
					    case OrderFieldNames.OrderRow5:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow5);
					        break;
					    case OrderFieldNames.OrderRow6:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow6);
					        break;
					    case OrderFieldNames.OrderRow7:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow7);
					        break;
					    case OrderFieldNames.OrderRow8:
					        query = querySortedByType.ThenByDescending(o => o.OrderRow8);
					        break;
					    case OrderFieldNames.Configuration:
					        query = querySortedByType.ThenByDescending(o => o.Configuration);
					        break;
					    case OrderFieldNames.OrderInfo:
					        query = querySortedByType.ThenByDescending(o => o.OrderInfo);
					        break;
					    case OrderFieldNames.OrderInfo2:
					        query = querySortedByType.ThenByDescending(o => o.OrderInfo2);
					        break;
                        // Other
                        case OtherFieldNames.FileName:
					        query = querySortedByType.ThenByDescending(o => o.Filename);
					        break;
					    case OtherFieldNames.CaseNumber:
					        query = querySortedByType.ThenByDescending(o => o.CaseNumber);
					        break;
					    case OtherFieldNames.Info:
					        query = querySortedByType.ThenByDescending(o => o.Info);
					        break;
					    case OtherFieldNames.Status:
					        query = querySortedByType.ThenByDescending(o => o.OrderState.Name);
					        break;
                        // Program
                        case ProgramFieldNames.Program:
					        query = querySortedByType.ThenByDescending(o => o.Programs.Count);
					        break;
                        case ProgramFieldNames.InfoProduct:
                            query = querySortedByType.ThenByDescending(o => o.InfoProduct);
                            break;
                        // Reciever
                        case ReceiverFieldNames.ReceiverId:
					        query = querySortedByType.ThenByDescending(o => o.ReceiverId);
					        break;
					    case ReceiverFieldNames.ReceiverName:
					        query = querySortedByType.ThenByDescending(o => o.ReceiverName);
					        break;
					    case ReceiverFieldNames.ReceiverEmail:
					        query = querySortedByType.ThenByDescending(o => o.ReceiverEMail);
					        break;
					    case ReceiverFieldNames.ReceiverPhone:
					        query = querySortedByType.ThenByDescending(o => o.ReceiverPhone);
					        break;
					    case ReceiverFieldNames.ReceiverLocation:
					        query = querySortedByType.ThenByDescending(o => o.ReceiverLocation);
					        break;
					    case ReceiverFieldNames.MarkOfGoods:
					        query = querySortedByType.ThenByDescending(o => o.MarkOfGoods);
					        break;
                        // Supplier
                        case SupplierFieldNames.SupplierOrderNumber:
					        query = querySortedByType.ThenByDescending(o => o.SupplierOrderNumber);
					        break;
					    case SupplierFieldNames.SupplierOrderDate:
					        query = querySortedByType.ThenByDescending(o => o.SupplierOrderDate);
					        break;
					    case SupplierFieldNames.SupplierOrderInfo:
					        query = querySortedByType.ThenByDescending(o => o.SupplierOrderInfo);
					        break;
                        //User
					    case UserFieldNames.UserId:
					        query = querySortedByType.ThenByDescending(o => o.UserId);
					        break;
					    case UserFieldNames.UserFirstName:
					        query = querySortedByType.ThenByDescending(o => o.UserFirstName);
					        break;
					    case UserFieldNames.UserLastName:
					        query = querySortedByType.ThenByDescending(o => o.UserLastName);
					        break;
					    case UserFieldNames.UserPhone:
					        query = querySortedByType.ThenByDescending(o => o.UserPhone);
					        break;
					    case UserFieldNames.UserEMail:
					        query = querySortedByType.ThenByDescending(o => o.UserEMail);
					        break;
					    case UserFieldNames.UserInitials:
					        query = querySortedByType.ThenByDescending(o => o.UserInitials);
					        break;
					    case UserFieldNames.UserPersonalIdentityNumber:
					        query = querySortedByType.ThenByDescending(o => o.UserPersonalIdentityNumber);
					        break;
					    case UserFieldNames.UserExtension:
					        query = querySortedByType.ThenByDescending(o => o.UserExtension);
					        break;
					    case UserFieldNames.UserTitle:
					        query = querySortedByType.ThenByDescending(o => o.UserTitle);
					        break;
					    case UserFieldNames.UserLocation:
					        query = querySortedByType.ThenByDescending(o => o.UserLocation);
					        break;
					    case UserFieldNames.UserRoomNumber:
					        query = querySortedByType.ThenByDescending(o => o.UserRoomNumber);
					        break;
					    case UserFieldNames.UserPostalAddress:
					        query = querySortedByType.ThenByDescending(o => o.UserPostalAddress);
					        break;
					    case UserFieldNames.Responsibility:
					        query = querySortedByType.ThenByDescending(o => o.Responsibility);
					        break;
					    case UserFieldNames.Activity:
					        query = querySortedByType.ThenByDescending(o => o.Activity);
					        break;
					    case UserFieldNames.Manager:
					        query = querySortedByType.ThenByDescending(o => o.Manager);
					        break;
					    case UserFieldNames.ReferenceNumber:
					        query = querySortedByType.ThenByDescending(o => o.ReferenceNumber);
					        break;
					    case UserFieldNames.InfoUser:
					        query = querySortedByType.ThenByDescending(o => o.InfoUser);
					        break;
					    case UserFieldNames.UserOU_Id:
					        query = querySortedByType.ThenByDescending(o => o.UserOU.Name);
					        break;
					    case UserFieldNames.EmploymentType:
					        query = querySortedByType.ThenByDescending(o => o.EmploymentType.Name);
					        break;
					    case UserFieldNames.UserDepartment_Id1:
					        query = querySortedByType.ThenByDescending(o => o.UserDepartment1.DepartmentName);
					        break;
                        case UserFieldNames.UserDepartment_Id2:
                            query = querySortedByType.ThenByDescending(o => o.UserDepartment2.DepartmentName);
                            break;
                        //Account Info
                        case AccountInfoFieldNames.StartedDate:
                            query = querySortedByType.ThenByDescending(o => o.AccountStartDate);
                            break;
                        case AccountInfoFieldNames.FinishDate:
                            query = querySortedByType.ThenByDescending(o => o.AccountEndDate);
                            break;
                        case AccountInfoFieldNames.HomeDirectory:
                            query = querySortedByType.ThenByDescending(o => o.HomeDirectory);
                            break;
                        case AccountInfoFieldNames.Profile:
                            query = querySortedByType.ThenByDescending(o => o.Profile);
                            break;
                        //case AccountInfoFieldNames.EMailTypeId:
                        //    query = querySortedByType.ThenByDescending(o => o.Profile);
                        //    break;
                        case AccountInfoFieldNames.InventoryNumber:
                            query = querySortedByType.ThenByDescending(o => o.InventoryNumber);
                            break;
                        case AccountInfoFieldNames.Info:
                            query = querySortedByType.ThenByDescending(o => o.AccountInfo);
                            break;
                        case AccountInfoFieldNames.AccountTypeId:
                            query = querySortedByType.ThenByDescending(o => o.OrderFieldType.Name);
                            break;
                        //case AccountInfoFieldNames.AccountTypeId2:
                        //    query = querySortedByType.ThenByDescending(o => o.OrderFieldType2.Name);
                        //    break;
                        case AccountInfoFieldNames.AccountTypeId3:
                            query = querySortedByType.ThenByDescending(o => o.OrderFieldType3.Name);
                            break;
                        case AccountInfoFieldNames.AccountTypeId4:
                            query = querySortedByType.ThenByDescending(o => o.OrderFieldType4.Name);
                            break;
                        case AccountInfoFieldNames.AccountTypeId5:
                            query = querySortedByType.ThenByDescending(o => o.OrderFieldType5.Name);
                            break;
                        //Contact
                        case ContactFieldNames.Id:
                            query = querySortedByType.ThenByDescending(o => o.ContactId);
                            break;
                        case ContactFieldNames.EMail:
                            query = querySortedByType.ThenByDescending(o => o.ContactEMail);
                            break;
                        case ContactFieldNames.Name:
                            query = querySortedByType.ThenByDescending(o => o.ContactName);
                            break;
                        case ContactFieldNames.Phone:
                            query = querySortedByType.ThenByDescending(o => o.ContactPhone);
                            break;

                    }



                    break;
			}

			return query;
		}
	}
}