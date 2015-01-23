namespace DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;

    public sealed class OrderMailTemplateFormatter : MailTemplateFormatter<UpdateOrderRequest>
    {
        protected override EmailMarkValues GetMarkValues(MailTemplate template, UpdateOrderRequest businessModel, int customerId, int languageId)
        {
            var markValues = new EmailMarkValues();

            AddDeliveryMarkValues(markValues, businessModel.Order.Delivery);
            AddGeneralMarkValues(markValues, businessModel.Order.General);
            AddLogMarkValues(markValues, businessModel.Order.Log);
            AddOrdererMarkValues(markValues, businessModel.Order.Orderer);
            AddOrderMarkValues(markValues, businessModel.Order.Order);
            AddOtherMarkValues(markValues, businessModel.Order.Other);
            AddProgramMarkValues(markValues, businessModel.Order.Program);
            AddReceiverMarkValues(markValues, businessModel.Order.Receiver);
            AddSupplierMarkValues(markValues, businessModel.Order.Supplier);
            AddUserMarkValues(markValues, businessModel.Order.User);

            return markValues;
        }

        private static void AddDeliveryMarkValues(EmailMarkValues markValues, DeliveryEditFields fields)
        {            
        }

        private static void AddGeneralMarkValues(EmailMarkValues markValues, GeneralEditFields fields)
        {            
        }

        private static void AddLogMarkValues(EmailMarkValues markValues, LogEditFields fields)
        {            
        }

        private static void AddOrdererMarkValues(EmailMarkValues markValues, OrdererEditFields fields)
        {            
        }

        private static void AddOrderMarkValues(EmailMarkValues markValues, OrderEditFields fields)
        {            
        }

        private static void AddOtherMarkValues(EmailMarkValues markValues, OtherEditFields fields)
        {            
        }

        private static void AddProgramMarkValues(EmailMarkValues markValues, ProgramEditFields fields)
        {            
        }

        private static void AddReceiverMarkValues(EmailMarkValues markValues, ReceiverEditFields fields)
        {            
        }

        private static void AddSupplierMarkValues(EmailMarkValues markValues, SupplierEditFields fields)
        {            
        }

        private static void AddUserMarkValues(EmailMarkValues markValues, UserEditFields fields)
        {            
        }
    }
}