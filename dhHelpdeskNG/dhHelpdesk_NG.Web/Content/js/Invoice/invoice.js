        IsProductAreaChanged: function () {
            var lastProductArea = $('#LastProductAreaId').val();
            if (this.ProductAreaElement.val() != lastProductArea)
                return true;
            else
                return false;
        },

            if (this.CaseId == undefined || this.CaseId == null || this.IsNewCase()) {
                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
            }
            

                default:
                    dhHelpdesk.Common.ShowErrorMessage("State is not implemented!");
                if (this.allVailableOrders[i].Id > 0 && this.allVailableOrders[i].OrderState != dhHelpdesk.CaseArticles.OrderStates.Deleted) {
                    cleanOrders.push(this.allVailableOrders[i]);
                } else {
                    cleanOrders.push(this.allVailableOrders[i]);
                if (this.allVailableOrders[i].Id == orderId && this.allVailableOrders[i].OrderState != dhHelpdesk.CaseArticles.OrderStates.Deleted) {
                width: "90%",
                overflow: "auto",
                zIndex: 1100,
                        click: function () {
                            dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_SAVING);
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                            if (!dhHelpdesk.CaseArticles.ValidateOpenInvoiceWindow()) {
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                            }
                            dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_SAVING);
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                            if (!dhHelpdesk.CaseArticles.ValidateOpenInvoiceWindow()) {
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                            }
                        //TODO: need to enable order tabs and others that we need to active them in Finished Case
            //// set order actions             
            
            
                /* Deleted orders will be null */ 
                if (order != null && !order.InvoiceValidate())
                if (th.IsNewCase() || th.IsProductAreaChanged()) {
                tabs.find("li").removeClass("ui-state-default");                               
                if (order.OrderState == dhHelpdesk.CaseArticles.OrderStates.Deleted) {
                    return
                }
                var tabIcon = (order.IsOrderInvoiced() ? "<i class='icon-ok icon-green'></i>&nbsp;&nbsp;" : "");                
                var newTab = $("<li class='case-invoice-order-tab active' ><a href='#case-invoice-order" + order.Id + "'>" + tabIcon + order.Caption + "</a><li>");
                newTab.find("a").click();                
                        $("#doInvoiceButton_" + currentOrder.Id).hide();                        
                        addArticleEl.hide();

                    if (currentOrder.IsOrderInvoiced() || currentOrder.CreditForOrder_Id != null) {
                        var allInitiatorFields = $(".InitiatorFields_" + currentOrder.Id);
                        $.each(allInitiatorFields, function (i) {
                            $(allInitiatorFields[i]).attr("disabled", true);
                        });
                        $("#invoiceSelectFile_" + currentOrder.Id).attr("disabled", true);
                    }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.CostCentre)) {
            var ordersCount = 0;
            for (i = 0; i < dhHelpdesk.CaseArticles.allVailableOrders.length; i++) {
                if (dhHelpdesk.CaseArticles.allVailableOrders[i].OrderState != this.OrderStates.Deleted)
                    ordersCount++;
            }

            var sentCount = 0;
            if (invoicedOrders != null)             
            
                $('#InvoiceModuleBtnOpen').val(dhHelpdesk.CaseArticles.invoiceButtonPureCaption + " (" + (sentCount) + "/" + (ordersCount) + ")");
                    var emptyOrders = false;
                    var invoice = new dhHelpdesk.CaseArticles.CaseInvoice();
                    var inv = null;

                    /* TODO: Now there is only one Invoice per case, if needs more we should change this line */
                        invoice.Id = dhHelpdesk.Common.GenerateId();
                        invoice.Initialize();
                        dhHelpdesk.CaseArticles.AddInvoice(invoice);
                        invoice.CaseId = dhHelpdesk.CaseArticles.CaseId;
                        emptyOrders = true;
                    } else {
                        inv = data.Invoices[0];
                        invoice.Id = inv.Id;
                        invoice.CaseId = inv.CaseId;
                        invoice.Initialize();
                        dhHelpdesk.CaseArticles._invoices = [];
                        dhHelpdesk.CaseArticles.AddInvoice(invoice);
                        dhHelpdesk.CaseArticles.Initialize($this);
                    }

                    if (emptyOrders || (data.Invoices.length != 0 && (data.Invoices[0].Orders == null || data.Invoices[0].Orders.length == 0))) {
                        
                        dhHelpdesk.CaseArticles.allVailableOrders = [];
                        var blankOrder = new dhHelpdesk.CaseArticles.CreateBlankOrder();
                        invoice.AddOrder(blankOrder);
                        dhHelpdesk.CaseArticles.ApplyChanges();                        
                        
                        if (callBack != undefined && callBack != null) {
                            callBack(obj);
                        }

                        dhHelpdesk.CaseArticles.ChangeTab(dhHelpdesk.CaseArticles.LastSelectedTab);
                        dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);

                        return;
                    }
               
