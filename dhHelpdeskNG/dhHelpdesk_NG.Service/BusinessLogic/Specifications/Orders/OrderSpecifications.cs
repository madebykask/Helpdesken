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
					// Delivery
					if (sort.Name == DeliveryFieldNames.DeliveryDate)
					{
						query = (querySortedByType).ThenBy(o => o.Deliverydate);
					}
					else if (sort.Name == DeliveryFieldNames.InstallDate)
					{
						query = querySortedByType.ThenBy(o => o.InstallDate);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryDepartment)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryDepartment.DepartmentName);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryOu)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryOu);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryAddress)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryAddress);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryPostalCode)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryPostalCode);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryPostalAddress)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryPostalAddress);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryLocation)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryLocation);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryInfo1)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryInfo);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryInfo2)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryInfo2);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryInfo3)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryInfo3);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryOuId)
					{
						query = querySortedByType.ThenBy(o => o.DeliveryOuEntity.Name);
					}

					// General
					else if (sort.Name == GeneralFieldNames.OrderNumber)
					{
						query = querySortedByType.ThenBy(o => o.Id);
					}
					else if (sort.Name == GeneralFieldNames.Customer)
					{
						query = querySortedByType.ThenBy(o => o.Customer.Name);
					}
					else if (sort.Name == GeneralFieldNames.Administrator)
					{
						query = querySortedByType.ThenBy(o => o.User_Id);
					}
					else if (sort.Name == GeneralFieldNames.Domain)
					{
						query = querySortedByType.ThenBy(o => o.Domain.Name);
					}
					else if (sort.Name == GeneralFieldNames.OrderDate)
					{
						query = querySortedByType.ThenBy(o => o.OrderDate);
					}

					// Log
					else if (sort.Name == LogFieldNames.Log)
					{
						query = querySortedByType.ThenBy(l => l.Logs.Count);
					}

					// Orderer
					else if (sort.Name == OrdererFieldNames.OrdererId)
					{
						query = querySortedByType.ThenBy(o => o.OrdererID);
					}
					else if (sort.Name == OrdererFieldNames.OrdererName)
					{
						query = querySortedByType.ThenBy(o => o.Orderer);
					}
					else if (sort.Name == OrdererFieldNames.OrdererLocation)
					{
						query = querySortedByType.ThenBy(o => o.OrdererLocation);
					}
					else if (sort.Name == OrdererFieldNames.OrdererEmail)
					{
						query = querySortedByType.ThenBy(o => o.OrdererEMail);
					}
					else if (sort.Name == OrdererFieldNames.OrdererPhone)
					{
						query = querySortedByType.ThenBy(o => o.OrdererPhone);
					}
					else if (sort.Name == OrdererFieldNames.OrdererCode)
					{
						query = querySortedByType.ThenBy(o => o.OrdererCode);
					}
					else if (sort.Name == OrdererFieldNames.Department)
					{
						query = querySortedByType.ThenBy(o => o.Department_Id);
					}
					else if (sort.Name == OrdererFieldNames.Unit)
					{
						query = querySortedByType.ThenBy(o => o.Ou.Name);
					}
					else if (sort.Name == OrdererFieldNames.OrdererAddress)
					{
						query = querySortedByType.ThenBy(o => o.OrdererAddress);
					}
					else if (sort.Name == OrdererFieldNames.OrdererInvoiceAddress)
					{
						query = querySortedByType.ThenBy(o => o.OrdererInvoiceAddress);
					}
					else if (sort.Name == OrdererFieldNames.OrdererReferenceNumber)
					{
						query = querySortedByType.ThenBy(o => o.OrdererReferenceNumber);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension1)
					{
						query = querySortedByType.ThenBy(o => o.AccountingDimension1);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension2)
					{
						query = querySortedByType.ThenBy(o => o.AccountingDimension2);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension3)
					{
						query = querySortedByType.ThenBy(o => o.AccountingDimension3);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension4)
					{
						query = querySortedByType.ThenBy(o => o.AccountingDimension4);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension5)
					{
						query = querySortedByType.ThenBy(o => o.AccountingDimension5);
					}

					// Order
					else if (sort.Name == OrderFieldNames.Property)
					{
						query = querySortedByType.ThenBy(o => o.OrderProperty.OrderProperty);
					}
					else if (sort.Name == OrderFieldNames.OrderRow1)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow);
					}
					else if (sort.Name == OrderFieldNames.OrderRow2)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow2);
					}
					else if (sort.Name == OrderFieldNames.OrderRow3)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow3);
					}
					else if (sort.Name == OrderFieldNames.OrderRow4)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow4);
					}
					else if (sort.Name == OrderFieldNames.OrderRow5)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow5);
					}
					else if (sort.Name == OrderFieldNames.OrderRow6)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow6);
					}
					else if (sort.Name == OrderFieldNames.OrderRow7)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow7);
					}
					else if (sort.Name == OrderFieldNames.OrderRow8)
					{
						query = querySortedByType.ThenBy(o => o.OrderRow8);
					}
					else if (sort.Name == OrderFieldNames.Configuration)
					{
						query = querySortedByType.ThenBy(o => o.Configuration);
					}
					else if (sort.Name == OrderFieldNames.OrderInfo)
					{
						query = querySortedByType.ThenBy(o => o.OrderInfo);
					}
					else if (sort.Name == OrderFieldNames.OrderInfo2)
					{
						query = querySortedByType.ThenBy(o => o.OrderInfo2);
					}

					// Other
					else if (sort.Name == OtherFieldNames.FileName)
					{
						query = querySortedByType.ThenBy(o => o.Filename);
					}
					else if (sort.Name == OtherFieldNames.CaseNumber)
					{
						query = querySortedByType.ThenBy(o => o.CaseNumber);
					}
					else if (sort.Name == OtherFieldNames.Info)
					{
						query = querySortedByType.ThenBy(o => o.Info);
					}
					else if (sort.Name == OtherFieldNames.Status)
					{
						query = querySortedByType.ThenBy(o => o.OrderState.Name);
					}

					// Program
					else if (sort.Name == ProgramFieldNames.Program)
					{
						query = querySortedByType.ThenBy(o => o.Programs.Count);
					}

					// Receiver
					else if (sort.Name == ReceiverFieldNames.ReceiverId)
					{
						query = querySortedByType.ThenBy(o => o.ReceiverId);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverName)
					{
						query = querySortedByType.ThenBy(o => o.ReceiverName);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverEmail)
					{
						query = querySortedByType.ThenBy(o => o.ReceiverEMail);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverPhone)
					{
						query = querySortedByType.ThenBy(o => o.ReceiverPhone);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverLocation)
					{
						query = querySortedByType.ThenBy(o => o.ReceiverLocation);
					}
					else if (sort.Name == ReceiverFieldNames.MarkOfGoods)
					{
						query = querySortedByType.ThenBy(o => o.MarkOfGoods);
					}

					// Supplier
					else if (sort.Name == SupplierFieldNames.SupplierOrderNumber)
					{
						query = querySortedByType.ThenBy(o => o.SupplierOrderNumber);
					}
					else if (sort.Name == SupplierFieldNames.SupplierOrderDate)
					{
						query = querySortedByType.ThenBy(o => o.SupplierOrderDate);
					}
					else if (sort.Name == SupplierFieldNames.SupplierOrderInfo)
					{
						query = querySortedByType.ThenBy(o => o.SupplierOrderInfo);
					}

					// User
					else if (sort.Name == UserFieldNames.UserId)
					{
						query = querySortedByType.ThenBy(o => o.UserId);
					}
					else if (sort.Name == UserFieldNames.UserFirstName)
					{
						query = querySortedByType.ThenBy(o => o.UserFirstName);
					}
					else if (sort.Name == UserFieldNames.UserLastName)
					{
						query = querySortedByType.ThenBy(o => o.UserLastName);
					}

					break;

				case SortBy.Descending:
					// Delivery
					if (sort.Name == DeliveryFieldNames.DeliveryDate)
					{
						query = querySortedByType.ThenByDescending(o => o.Deliverydate);
					}
					else if (sort.Name == DeliveryFieldNames.InstallDate)
					{
						query = querySortedByType.ThenByDescending(o => o.InstallDate);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryDepartment)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryDepartment.DepartmentName);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryOu)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryOu);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryAddress)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryAddress);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryPostalCode)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryPostalCode);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryPostalAddress)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryPostalAddress);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryLocation)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryLocation);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryInfo1)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryInfo);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryInfo2)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryInfo2);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryInfo3)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryInfo3);
					}
					else if (sort.Name == DeliveryFieldNames.DeliveryOuId)
					{
						query = querySortedByType.ThenByDescending(o => o.DeliveryOuEntity.Name);
					}

					// General
					else if (sort.Name == GeneralFieldNames.OrderNumber)
					{
						query = querySortedByType.ThenByDescending(o => o.Id);
					}
					else if (sort.Name == GeneralFieldNames.Customer)
					{
						query = querySortedByType.ThenByDescending(o => o.Customer.Name);
					}
					else if (sort.Name == GeneralFieldNames.Administrator)
					{
						query = querySortedByType.ThenByDescending(o => o.User_Id);
					}
					else if (sort.Name == GeneralFieldNames.Domain)
					{
						query = querySortedByType.ThenByDescending(o => o.Domain.Name);
					}
					else if (sort.Name == GeneralFieldNames.OrderDate)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderDate);
					}

					// Log
					else if (sort.Name == LogFieldNames.Log)
					{
						query = querySortedByType.ThenByDescending(l => l.Logs.Count);
					}

					// Orderer
					else if (sort.Name == OrdererFieldNames.OrdererId)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererID);
					}
					else if (sort.Name == OrdererFieldNames.OrdererName)
					{
						query = querySortedByType.ThenByDescending(o => o.Orderer);
					}
					else if (sort.Name == OrdererFieldNames.OrdererLocation)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererLocation);
					}
					else if (sort.Name == OrdererFieldNames.OrdererEmail)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererEMail);
					}
					else if (sort.Name == OrdererFieldNames.OrdererPhone)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererPhone);
					}
					else if (sort.Name == OrdererFieldNames.OrdererCode)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererCode);
					}
					else if (sort.Name == OrdererFieldNames.Department)
					{
						query = querySortedByType.ThenByDescending(o => o.Department_Id);
					}
					else if (sort.Name == OrdererFieldNames.Unit)
					{
						query = querySortedByType.ThenByDescending(o => o.Ou.Name);
					}
					else if (sort.Name == OrdererFieldNames.OrdererAddress)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererAddress);
					}
					else if (sort.Name == OrdererFieldNames.OrdererInvoiceAddress)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererInvoiceAddress);
					}
					else if (sort.Name == OrdererFieldNames.OrdererReferenceNumber)
					{
						query = querySortedByType.ThenByDescending(o => o.OrdererReferenceNumber);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension1)
					{
						query = querySortedByType.ThenByDescending(o => o.AccountingDimension1);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension2)
					{
						query = querySortedByType.ThenByDescending(o => o.AccountingDimension2);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension3)
					{
						query = querySortedByType.ThenByDescending(o => o.AccountingDimension3);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension4)
					{
						query = querySortedByType.ThenByDescending(o => o.AccountingDimension4);
					}
					else if (sort.Name == OrdererFieldNames.AccountingDimension5)
					{
						query = querySortedByType.ThenByDescending(o => o.AccountingDimension5);
					}

					// Order
					else if (sort.Name == OrderFieldNames.Property)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderProperty.OrderProperty);
					}
					else if (sort.Name == OrderFieldNames.OrderRow1)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow);
					}
					else if (sort.Name == OrderFieldNames.OrderRow2)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow2);
					}
					else if (sort.Name == OrderFieldNames.OrderRow3)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow3);
					}
					else if (sort.Name == OrderFieldNames.OrderRow4)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow4);
					}
					else if (sort.Name == OrderFieldNames.OrderRow5)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow5);
					}
					else if (sort.Name == OrderFieldNames.OrderRow6)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow6);
					}
					else if (sort.Name == OrderFieldNames.OrderRow7)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow7);
					}
					else if (sort.Name == OrderFieldNames.OrderRow8)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderRow8);
					}
					else if (sort.Name == OrderFieldNames.Configuration)
					{
						query = querySortedByType.ThenByDescending(o => o.Configuration);
					}
					else if (sort.Name == OrderFieldNames.OrderInfo)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderInfo);
					}
					else if (sort.Name == OrderFieldNames.OrderInfo2)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderInfo2);
					}

					// Other
					else if (sort.Name == OtherFieldNames.FileName)
					{
						query = querySortedByType.ThenByDescending(o => o.Filename);
					}
					else if (sort.Name == OtherFieldNames.CaseNumber)
					{
						query = querySortedByType.ThenByDescending(o => o.CaseNumber);
					}
					else if (sort.Name == OtherFieldNames.Info)
					{
						query = querySortedByType.ThenByDescending(o => o.Info);
					}
					else if (sort.Name == OtherFieldNames.Status)
					{
						query = querySortedByType.ThenByDescending(o => o.OrderState.Name);
					}

					// Program
					else if (sort.Name == ProgramFieldNames.Program)
					{
						query = querySortedByType.ThenByDescending(o => o.Programs.Count);
					}

					// Receiver
					else if (sort.Name == ReceiverFieldNames.ReceiverId)
					{
						query = querySortedByType.ThenByDescending(o => o.ReceiverId);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverName)
					{
						query = querySortedByType.ThenByDescending(o => o.ReceiverName);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverEmail)
					{
						query = querySortedByType.ThenByDescending(o => o.ReceiverEMail);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverPhone)
					{
						query = querySortedByType.ThenByDescending(o => o.ReceiverPhone);
					}
					else if (sort.Name == ReceiverFieldNames.ReceiverLocation)
					{
						query = querySortedByType.ThenByDescending(o => o.ReceiverLocation);
					}
					else if (sort.Name == ReceiverFieldNames.MarkOfGoods)
					{
						query = querySortedByType.ThenByDescending(o => o.MarkOfGoods);
					}

					// Supplier
					else if (sort.Name == SupplierFieldNames.SupplierOrderNumber)
					{
						query = querySortedByType.ThenByDescending(o => o.SupplierOrderNumber);
					}
					else if (sort.Name == SupplierFieldNames.SupplierOrderDate)
					{
						query = querySortedByType.ThenByDescending(o => o.SupplierOrderDate);
					}
					else if (sort.Name == SupplierFieldNames.SupplierOrderInfo)
					{
						query = querySortedByType.ThenByDescending(o => o.SupplierOrderInfo);
					}

					// User
					else if (sort.Name == UserFieldNames.UserId)
					{
						query = querySortedByType.ThenByDescending(o => o.UserId);
					}
					else if (sort.Name == UserFieldNames.UserFirstName)
					{
						query = querySortedByType.ThenByDescending(o => o.UserFirstName);
					}
					else if (sort.Name == UserFieldNames.UserLastName)
					{
						query = querySortedByType.ThenByDescending(o => o.UserLastName);
					}

					break;
			}

			return query;
		}
	}
}