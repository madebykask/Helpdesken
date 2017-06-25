using System;
using System.Linq;
using System.Text;
using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

namespace DH.Helpdesk.Web.Infrastructure.Order
{
    internal class OrderExportHelper
    {
        private const string LineFormat = "{0}: {1}{2}";

        public byte[] GetOrderExportText(FullOrderEditModel model)
        {
            var builder = new StringBuilder();

            //general
            if (model.General.OrderNumber.Value > 0)
            {
                builder.AppendFormat("{0}: O-{1}{2}", Translation.GetCoreTextTranslation(model.General.OrderNumber.Caption), model.General.OrderNumber.Value, Environment.NewLine);
            }
            if (model.General.Status.Show)
            {
                var field = model.General.Status.Value.FirstOrDefault(x => x.Selected);
                var value = field != null ? field.Text : string.Empty;
                if (!string.IsNullOrEmpty(value))
                    builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.General.Status.Caption), value, Environment.NewLine);
            }
            if (model.General.Administrator.Show && model.General.Administrator.Value != null)
            {
                var field = model.General.Administrators.FirstOrDefault(x => x.Selected);
                var value = field != null ? field.Text : string.Empty;
                if (!string.IsNullOrEmpty(value))
                    builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.General.Administrator.Caption), value, Environment.NewLine);
            }
            if (model.General.OrderDate.Show && model.General.OrderDate.Value.HasValue)
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.General.OrderDate.Caption), model.General.OrderDate.Value.Value.ToShortDateString(), Environment.NewLine);
            }

            //user
            if (model.User.UserId.Show && !string.IsNullOrEmpty(model.User.UserId.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.User.UserId.Caption), model.User.UserId.Value, Environment.NewLine);
            }
            if (model.User.UserFirstName.Show && !string.IsNullOrEmpty(model.User.UserFirstName.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.User.UserFirstName.Caption), model.User.UserFirstName.Value, Environment.NewLine);
            }
            if (model.User.UserLastName.Show && !string.IsNullOrEmpty(model.User.UserLastName.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.User.UserLastName.Caption), model.User.UserLastName.Value, Environment.NewLine);
            }

            //userInfo
            if (model.UserInfo.OrdererId.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererId.Value))
            {
                var valuesSplitter = "&,";
                var pairSplitter = "&;";
                var multiTextValues = model.UserInfo.OrdererId.Value
                    .Split(new[] { pairSplitter }, StringSplitOptions.None)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s =>
                    {
                        var values = s.Split(new[] { valuesSplitter }, StringSplitOptions.None);
                        return values.Length == 2 ? values[0] ?? "" : "";
                    });
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererId.Caption), string.Join(", ", multiTextValues), Environment.NewLine);
            }
            if (model.UserInfo.OrdererName.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererName.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererName.Caption), model.UserInfo.OrdererName.Value, Environment.NewLine);
            }
            if (model.UserInfo.OrdererLocation.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererLocation.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererLocation.Caption), model.UserInfo.OrdererLocation.Value, Environment.NewLine);
            }
            if (model.UserInfo.OrdererEmail.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererEmail.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererEmail.Caption), model.UserInfo.OrdererEmail.Value, Environment.NewLine);
            }
            if (model.UserInfo.OrdererPhone.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererPhone.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererPhone.Caption), model.UserInfo.OrdererPhone.Value, Environment.NewLine);
            }
            if (model.UserInfo.OrdererCode.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererCode.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererCode.Caption), model.UserInfo.OrdererCode.Value, Environment.NewLine);
            }
            if (model.UserInfo.OrdererAddress.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererAddress.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererAddress.Caption), model.UserInfo.OrdererAddress.Value, Environment.NewLine);
            }
            if (model.UserInfo.OrdererInvoiceAddress.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererInvoiceAddress.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererInvoiceAddress.Caption), model.UserInfo.OrdererInvoiceAddress.Value, Environment.NewLine);
            }
            if (model.UserInfo.OrdererReferenceNumber.Show && !string.IsNullOrEmpty(model.UserInfo.OrdererReferenceNumber.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.OrdererReferenceNumber.Caption), model.UserInfo.OrdererReferenceNumber.Value, Environment.NewLine);
            }
            if (model.UserInfo.DepartmentId1.Show && model.UserInfo.DepartmentId1.Value != null)
            {
                var field = model.UserInfo.Departments.FirstOrDefault(x => x.Selected);
                var value = field != null ? field.Text : string.Empty;
                if (!string.IsNullOrEmpty(value))
                    builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.DepartmentId1.Caption), value, Environment.NewLine);
            }
            if (model.UserInfo.DepartmentId2.Show && model.UserInfo.DepartmentId2.Value.HasValue)
            {
                var field = model.UserInfo.Departments2.FirstOrDefault(x => x.Selected);
                var value = field != null ? field.Text : string.Empty;
                if (!string.IsNullOrEmpty(value))
                    builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.DepartmentId2.Caption), value, Environment.NewLine);
            }
            if (model.UserInfo.Unit.Show && model.UserInfo.Unit.Value.HasValue)
            {
                var field = model.UserInfo.Units.FirstOrDefault(x => x.Selected);
                var value = field != null ? field.Text : string.Empty;
                if (!string.IsNullOrEmpty(value))
                    builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.Unit.Caption), model.UserInfo.Unit.Value, Environment.NewLine);
            }
            if (model.UserInfo.AccountingDimension1.Show && !string.IsNullOrEmpty(model.UserInfo.AccountingDimension1.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.AccountingDimension1.Caption), model.UserInfo.AccountingDimension1.Value, Environment.NewLine);
            }
            if (model.UserInfo.AccountingDimension2.Show && !string.IsNullOrEmpty(model.UserInfo.AccountingDimension2.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.AccountingDimension2.Caption), model.UserInfo.AccountingDimension2.Value, Environment.NewLine);
            }
            if (model.UserInfo.AccountingDimension3.Show && !string.IsNullOrEmpty(model.UserInfo.AccountingDimension3.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.AccountingDimension3.Caption), model.UserInfo.AccountingDimension3.Value, Environment.NewLine);
            }
            if (model.UserInfo.AccountingDimension4.Show && !string.IsNullOrEmpty(model.UserInfo.AccountingDimension4.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.AccountingDimension4.Caption), model.UserInfo.AccountingDimension4.Value, Environment.NewLine);
            }
            if (model.UserInfo.AccountingDimension5.Show && !string.IsNullOrEmpty(model.UserInfo.AccountingDimension5.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.UserInfo.AccountingDimension5.Caption), model.UserInfo.AccountingDimension5.Value, Environment.NewLine);
            }

            //Order
            if (model.Order.Property.Show && model.Order.Property.Value.HasValue)
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.Property.Caption), model.Order.Property.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow1.Show && !string.IsNullOrEmpty(model.Order.OrderRow1.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow1.Caption), model.Order.OrderRow1.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow2.Show && !string.IsNullOrEmpty(model.Order.OrderRow2.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow2.Caption), model.Order.OrderRow2.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow3.Show && !string.IsNullOrEmpty(model.Order.OrderRow3.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow3.Caption), model.Order.OrderRow3.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow4.Show && !string.IsNullOrEmpty(model.Order.OrderRow4.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow4.Caption), model.Order.OrderRow4.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow5.Show && !string.IsNullOrEmpty(model.Order.OrderRow5.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow5.Caption), model.Order.OrderRow5.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow6.Show && !string.IsNullOrEmpty(model.Order.OrderRow6.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow6.Caption), model.Order.OrderRow6.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow7.Show && !string.IsNullOrEmpty(model.Order.OrderRow7.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow7.Caption), model.Order.OrderRow7.Value, Environment.NewLine);
            }
            if (model.Order.OrderRow8.Show && !string.IsNullOrEmpty(model.Order.OrderRow8.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderRow8.Caption), model.Order.OrderRow8.Value, Environment.NewLine);
            }
            if (model.Order.Configuration.Show && !string.IsNullOrEmpty(model.Order.Configuration.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.Configuration.Caption), model.Order.Configuration.Value, Environment.NewLine);
            }
            if (model.Order.OrderInfo.Show && !string.IsNullOrEmpty(model.Order.OrderInfo.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Order.OrderInfo.Caption), model.Order.OrderInfo.Value, Environment.NewLine);
            }

            //Receiver
            if (model.Receiver.ReceiverId.Show && !string.IsNullOrEmpty(model.Receiver.ReceiverId.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Receiver.ReceiverId.Caption), model.Receiver.ReceiverId.Value, Environment.NewLine);
            }
            if (model.Receiver.ReceiverName.Show && !string.IsNullOrEmpty(model.Receiver.ReceiverName.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Receiver.ReceiverName.Caption), model.Receiver.ReceiverName.Value, Environment.NewLine);
            }
            if (model.Receiver.ReceiverEmail.Show && !string.IsNullOrEmpty(model.Receiver.ReceiverEmail.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Receiver.ReceiverEmail.Caption), model.Receiver.ReceiverEmail.Value, Environment.NewLine);
            }
            if (model.Receiver.ReceiverPhone.Show && !string.IsNullOrEmpty(model.Receiver.ReceiverPhone.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Receiver.ReceiverPhone.Caption), model.Receiver.ReceiverPhone.Value, Environment.NewLine);
            }
            if (model.Receiver.ReceiverLocation.Show && !string.IsNullOrEmpty(model.Receiver.ReceiverLocation.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Receiver.ReceiverLocation.Caption), model.Receiver.ReceiverLocation.Value, Environment.NewLine);
            }
            if (model.Receiver.MarkOfGoods.Show && !string.IsNullOrEmpty(model.Receiver.MarkOfGoods.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Receiver.MarkOfGoods.Caption), model.Receiver.MarkOfGoods.Value, Environment.NewLine);
            }

            //Supplier
            if (model.Supplier.SupplierOrderNumber.Show && !string.IsNullOrEmpty(model.Supplier.SupplierOrderNumber.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Supplier.SupplierOrderNumber.Caption), model.Supplier.SupplierOrderNumber.Value, Environment.NewLine);
            }
            if (model.Supplier.SupplierOrderDate.Show && model.Supplier.SupplierOrderDate.Value.HasValue)
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Supplier.SupplierOrderDate.Caption), model.Supplier.SupplierOrderDate.Value, Environment.NewLine);
            }
            if (model.Supplier.SupplierOrderInfo.Show && !string.IsNullOrEmpty(model.Supplier.SupplierOrderInfo.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Supplier.SupplierOrderInfo.Caption), model.Supplier.SupplierOrderInfo.Value, Environment.NewLine);
            }

            //Delivery
            if (model.Delivery.DeliveryDate.Show && model.Delivery.DeliveryDate.Value.HasValue)
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryDate.Caption), model.Delivery.DeliveryDate.Value.Value.ToShortDateString(), Environment.NewLine);
            }
            if (model.Delivery.InstallDate.Show && model.Delivery.InstallDate.Value.HasValue)
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.InstallDate.Caption), model.Delivery.InstallDate.Value.Value.ToShortDateString(), Environment.NewLine);
            }
            if (model.Delivery.DeliveryDepartment.Show && model.Delivery.DeliveryDepartment.Value.HasValue)
            {
                var field = model.Delivery.Departments.FirstOrDefault(x => x.Selected);
                var value = field != null ? field.Text : string.Empty;
                if (!string.IsNullOrEmpty(value))
                    builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryDepartment.Caption), value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryOu.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryOu.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryOu.Caption), model.Delivery.DeliveryOu.Value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryAddress.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryAddress.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryAddress.Caption), model.Delivery.DeliveryAddress.Value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryPostalCode.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryPostalCode.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryPostalCode.Caption), model.Delivery.DeliveryPostalCode.Value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryPostalAddress.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryPostalAddress.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryPostalAddress.Caption), model.Delivery.DeliveryPostalAddress.Value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryLocation.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryLocation.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryLocation.Caption), model.Delivery.DeliveryLocation.Value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryInfo1.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryInfo1.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryInfo1.Caption), model.Delivery.DeliveryInfo1.Value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryInfo2.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryInfo2.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryInfo2.Caption), model.Delivery.DeliveryInfo2.Value, Environment.NewLine);
            }
            if (model.Delivery.DeliveryInfo3.Show && !string.IsNullOrEmpty(model.Delivery.DeliveryInfo3.Value))
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Delivery.DeliveryInfo3.Caption), model.Delivery.DeliveryInfo3.Value, Environment.NewLine);
            }

            if (model.Other.CaseNumber.Show && model.Other.CaseNumber.Value.HasValue)
            {
                builder.AppendFormat(LineFormat, Translation.GetCoreTextTranslation(model.Other.CaseNumber.Caption), model.Other.CaseNumber.Value, Environment.NewLine);
            }

            return new UTF8Encoding().GetBytes(builder.ToString());
        }
    }
}