
if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {    
    _INVOICE_IDLE = 'invoice_idle';
    _INVOICE_SAVING = 'invoice_saving';
    var changeRegionUrl = '/Cases/ChangeRegion/';
    var changeDepartmentUrl = '/Cases/ChangeDepartment/';    

    //used for translating casefields
    var AllCaseFields = [];

    var AllTranslations = [];

    var CaseFieldSettings = [];

    var CurrentLanguageId = 1; //defaultlang - swedish    

    var caseButtonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' + 
                                 '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                                 '.btn.show-inventory, .btn.previous-case, .btn.next-case');

    var invoiceButtons = '.btn.close-invoice, .btn.save-invoice, .btn.save-close-invoice';

    var saveInvoiceIndicator = '.loading-msg.save-invoice';

    var invoiceButtonIndicator = $("#invoiceButtonIndicator");

    var projectElement = $("#case__Project_Id");

    dhHelpdesk.System = {
        RaiseEvent: function (eventType, extraParameters) {
            $(document).trigger(eventType, extraParameters);
        },

        OnEvent: function (event, handler) {
            $(document).on(event, handler);
        }        
    }

    dhHelpdesk.Math = {
        IsInteger: function (str) {
            str = str.replace(" ", "");
            return (str + "").match(/^\d+$/);
        },

        GetRandomInt: function (min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        },        

    },

    dhHelpdesk.Common = {

        GenerateId: function () {
            return -dhHelpdesk.Math.GetRandomInt(1, 10000);
        },

        IsNullOrEmpty: function (value) {
            return !(typeof value === "string" && value.length > 0);
        },

        ParseDate: function (date) {
            return new Date(parseInt(date.replace(/\/Date\((\d+)\)\//g, "$1")));
        },

        DateToJSONDateTime: function (date) {
            var jsonDate = "";
            if (date != null) {
                var _date = new Date(parseFloat(date.substr(6)));
                jsonDate = (_date.getMonth() + 1) + "/" +
                            _date.getDate() + "/" +
                            _date.getFullYear() + " " +
                            _date.getHours() + ":" +
                            _date.getMinutes() + ":" +
                            _date.getSeconds();
            }
            return jsonDate;
        },

        DateToDisplayDate: function (date) {
            var displayDate = "";
            if (date != null) {
                var _date = new Date(parseFloat(date.substr(6)));
                displayDate = _date.getFullYear() + "-" +
                               ((_date.getMonth() + 1).toString().length < 2 ? "0" + (_date.getMonth() + 1) : (_date.getMonth() + 1)) + "-" +
                                (_date.getDate().toString().length < 2 ? "0" + _date.getDate() : _date.getDate());
            }
            return displayDate;
        },

        DateToDisplayTime: function (date) {
            var displayTime = "";
            if (date != null) {
                var _date = new Date(parseFloat(date.substr(6)));
                displayTime = (_date.getHours().toString().length < 2 ? "0" + _date.getHours().toString() : _date.getHours().toString()) + ":" +
                                (_date.getMinutes().toString().length < 2 ? "0" + _date.getMinutes().toString() : _date.getMinutes().toString()) + ":" +
                                (_date.getSeconds().toString().length < 2 ? "0" + _date.getSeconds().toString() : _date.getSeconds().toString());
            }
            return displayTime;
        },

        MakeTextNumeric: function (elementName) {
            $(elementName).on("keypress keyup blur", function (event) {
                if (event.which == 37 || event.which == 38 || event.which == 39 || event.which == 40 ||
                    event.which == 9 || (event.shiftKey && event.which == 9) || (event.which >= 48 && event.which <= 57))
                    return;
                event.preventDefault();
            });
        },

        ShowSuccessMessage: function (message) {
            this.ShowMessage(message, 'success');
        },

        ShowWarningMessage: function (message) {
            this.ShowMessage(message, 'warning');
        },

        ShowErrorMessage: function (message) {
            this.ShowMessage(message, 'error');
        },

        ShowMessage: function (message, type) {
            $().toastmessage('showToast', {
                text: message,
                sticky: false,
                position: 'top-center',
                type: type,
                closeText: '',
                stayTime: 5000,
                inEffectDuration: 1000,
                close: function () {
                }
            });
        },

        MakeInvalid: function (e) {
            e.css("border-color", "red");
        },

        MakeValid: function (e) {
            e.css("border-color", "");
        },

        MarkTextInvalid: function (e) {
            e.css("color", "red");
        },

        MarkTextValid: function (e) {
            e.css("color", "");
        },

        Translate: function (text) {
            return text;
            //var AllTranslationsLength = AllTranslations.length;
            //for (var i = 0; i < AllTranslationsLength; i++) {
            //    if (AllTranslations[i].TextToTranslate.toLowerCase() === text.toLowerCase()) {
            //        var AllTranslationsTranslationsLength = AllTranslations[i].Translations.length;
            //        //for (var j = 0; j <= AllTranslationsTranslationsLength; j++) {
            //        //    if (AllTranslations[i].Translations[j].Language_Id)
            //        //    {
            //        //        if (AllTranslations[i].Translations[j].Language_Id == CurrentLanguageId) { //language_id is sometimes undefined...?? bug. todo fix
            //        //            return AllTranslations[i].Translations[j].TranslationName;
            //        //        }
            //        //    }
            //        //}
            //    }
            //}

            //return text;
        },

        TranslateCaseFields: function (text) {
            var AllCaseFieldsLength = AllCaseFields.length;
            for (var i = 0; i < AllCaseFieldsLength; i++) {
                if (AllCaseFields[i].Name.toLowerCase() == text.toLowerCase()) {
                    if (AllCaseFields[i].Label != null)
                        return AllCaseFields[i].Label;
                    else
                        return text;
                }
            }
            return text;
        },  
    }

    dhHelpdesk.CaseArticles = {
        MaxDescriptionCount: 50,

        DefaultAmount: 1,        

        DefaultPpu: 0,

        DateFormat: null,

        _invoices: [],

        _invoicesBackup: [],

        _container: null,

        _invoiceArticles: [],

        allVailableOrders: [],

        ProductAreaElement: null,

        CaseInvoicesElement: null,

        CaseInvoiceTemplate: null,

        CaseInvoiceOrderTemplate: null,

        CaseInvoiceArticleTemplate: null,

        CaseInvoiceOverviewTemplate: null,

        CaseInvoiceArticleOverviewTemplate: null,

        CaseInvoiceOrderActionsTemplate: null,

        CaseInvoiceCaseFilesTemplate: null,

        CaseInvoiceOrderFilesTemplate: null,

        CaseId: null,

        CaseKey: null,

        LogKey: null,

        CustomerId: null,

        OrderActionsInstance: null,

        ResetAddArticlePlace: function(){
            var articleEl = this._container.find(".chosen-select.articles-params-article");
            articleEl.find('option:selected').prop('selected', false);
            articleEl.trigger('chosen:updated');
            articleEl.trigger('chosen:activate');

            var units_El = this._container.find(".articles-params-units");
            units_El.val(dhHelpdesk.CaseArticles.DefaultAmount);

            var units_PriceEl = this._container.find(".articles-params-units-price");
            units_PriceEl.val('');
            units_PriceEl.prop("readonly", false);
            units_PriceEl.prop("disabled", false);            
        },
                        
        AddInvoiceArticle: function (article) {
            this._invoiceArticles.push(article);
        },

        GetInvoiceArticles: function () {
            return this._invoiceArticles;
        },

        GetInvoiceArticle: function (id) {
            var articles = this.GetInvoiceArticles();
            for (var i = 0; i < articles.length; i++) {
                var article = articles[i];
                if (article.Id == id) {
                    return article;
                }
            }
            return null;
        },

        ClearInvoiceArticles: function () {
            this._invoiceArticles = [];
        },

        IsNewCase: function () {
            return !(this.CaseId > 0);
        },

        IsFinishedCase: function () {
            var isFinished = $('#case__FinishingDate_HasValue').val();
            return isFinished;
        },

        AddInvoice: function (invoice) {
            this._invoices.push(invoice);
            invoice.Initialize();
        },

        GetInvoices: function () {
            return this._invoices;
        },

        GetInvoice: function (id) {
            var invoices = this.GetInvoices();

            if (typeof id === 'undefined' && invoices.length > 0) {
                return invoices[0];
            }

            for (var i = 0; i < invoices.length; i++) {
                var invoice = invoices[i];
                if (invoice.Id == id) {
                    return invoice;
                }
            }
            return null;
        },       

        GetOrder: function (id) {
            var invoices = this.GetInvoices();
            for (var i = 0; i < invoices.length; i++) {
                var order = invoices[i].GetOrder(id);
                if (order != null) {
                    return order;
                }
            }
            return null;
        },

        FillOrganisationDataForOtherReference: function () {
            this.FillRegions();
            $.each(this._invoices, function (i, invoice) {
                $.each(invoice._orders, function (j, order) {
                    dhHelpdesk.CaseArticles.FillDepartments(order.Id);
                    dhHelpdesk.CaseArticles.FillOUs(order.Id);
                });
            });
        },

        FillRegions: function (OrderId) {
            this.PopulateSelectBoxesFromSelect('RegionSelect', 'case__Region_Id');
        },        

        FillDepartments: function (OrderId) {
            var DepartmentFilterFormat = 0; 
            var RegionId = $('#RegionSelect-Order' + OrderId).parent().find('input:hidden').val()
            var CustomerId = $('#case__Customer_Id').val();            
            $.post(changeRegionUrl, { 'id': RegionId, 'customerId': CustomerId, 'departmentFilterFormat': DepartmentFilterFormat }, function (data) {
                if (data != undefined) {
                    var selector = '#DepartmentSelect-Order' + OrderId;
                    $(selector).empty();
                    $(selector).append("<option value=''></option>");
                    for (var i = 0; i < data.list.length; i++) {
                        var item = data.list[i];
                        var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                        if (option.val() == $(selector).parent().find('input:hidden').val()) {
                            $(selector).val($(selector).parent().find('input:hidden').val());
                            option.prop("selected", true);
                        }
                        $(selector).append(option);
                    }
                }
            }, 'json').always(function () {
                $('#DepartmentId_' + OrderId).val($('#DepartmentSelect-Order' + OrderId).val());                                
                dhHelpdesk.CaseArticles.FillOUs(OrderId);
            });            
        },

        PopulateSelectBoxesFromSelect: function (ClassSelector, IdSelectorForData) {
            var Selectors = $('.' + ClassSelector);
            var ValList = [];
            var TextList = [];
            $('#' + IdSelectorForData + ' option').each(function () {
                ValList.push($(this).val());
                TextList.push($(this).text());
            });
            $.each(Selectors, function (i, selector) {
                var thisSelector = $(selector);
                thisSelector.empty();
                var OptionsListToAppend = "";
                $.each(ValList, function (i, item) {
                    OptionsListToAppend = OptionsListToAppend + '<option value="' + item + '">' + TextList[i] + '</option>';
                });
                thisSelector.append(OptionsListToAppend);
                var IdInDB = thisSelector.parent().find('input:hidden').val();
                thisSelector.val(IdInDB);
            });
        },

        FillOUs: function (OrderId) {
            var DepartmentFilterFormat = 0;
            var DepartmentId = $('#DepartmentId_' + OrderId).val();
            var CustomerId = $('#case__Customer_Id').val();
            $.post(changeDepartmentUrl, { 'id': DepartmentId, 'CustomerId': CustomerId, 'departmentFilterFormat': DepartmentFilterFormat }, function (data) {
                if (data != undefined) {
                    var selector = '#OUSelect-Order' + OrderId;
                    $(selector).empty();
                    $(selector).append("<option value=''></option>");
                    for (var i = 0; i < data.list.length; i++) {
                        var item = data.list[i];
                        var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                        if (option.val() == $(selector).parent().find('input:hidden').val()) {
                            $(selector).val($(selector).parent().find('input:hidden').val());
                            option.prop("selected", true);
                        }
                        $(selector).append(option);
                    }
                }
            });
        },
        
        GetArticle: function (id) {
            var invoices = this.GetInvoices();
            for (var i = 0; i < invoices.length; i++) {
                var article = invoices[i].GetArticle(id);
                if (article != null) {
                    return article;
                }
            }
            return null;
        },

        ClearInvoices: function () {
            this._invoices = [];
        },

        SaveInvoices: function () {            
            this.CaseInvoicesElement.val(this.ToJson());            
        },

        GetSavedInvoices: function () {            
            return this.CaseInvoicesElement.val();
        },

        ApplyChanges: function () {
            this._invoicesBackup = [];           
            for (var i = 0; i < this._invoices.length; i++) {
                this._invoicesBackup.push(this._invoices[i].Clone());
            }
            this.SaveInvoices();
        },

        SaveToDatabase: function (callBack, that, orderIdToXML) {
            if (this.CaseId == undefined || this.CaseId == null || this.IsNewCase())
                return;
            
            dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_SAVING);
            var res = "";
            $.post('/cases/SaveCaseInvoice/', {
                'caseInvoiceArticle': this.GetSavedInvoices(),
                'customerId': this.CustomerId,
                'caseId': this.CaseId,
                'caseKey': this.CaseKey,
                'logKey': this.LogKey,
                'orderIdToXML': orderIdToXML
            },
                function (returnedData) {                    
                    if (returnedData != undefined && returnedData != null && returnedData.result == "Success") {                                                
                        $("[data-invoice]").attr("data-invoice-case-articles", returnedData.data);
                        $('#InvoiceModuleBtnOpen').remove();
                        if (returnedData.warningMessage != undefined && !dhHelpdesk.Common.IsNullOrEmpty(returnedData.warningMessage))
                            dhHelpdesk.Common.ShowWarningMessage(returnedData.warningMessage)
                        loadAllData(callBack, that);
                    }
                    else {                        
                        dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("An error has occurred during save Invoice! <br/> " + returnedData.data));
                        callBack(that);
                    }
                }
            , 'json').done(function () {
                
            });
        },

        SetInvoiceState: function (state) {
            switch (state) {
                case _INVOICE_IDLE:
                    $(saveInvoiceIndicator).hide();
                    $(invoiceButtons).removeClass("disabled");
                    $(invoiceButtons).css("pointer-events", "");                    
                    break;

                case _INVOICE_SAVING:
                    $(saveInvoiceIndicator).show();
                    $(invoiceButtons).addClass("disabled");
                    $(invoiceButtons).css("pointer-events", "none");                    
                    break;
            }
        },

        Restore: function () {
            this._invoices = [];
            for (var i = 0; i < this._invoicesBackup.length; i++) {
                this._invoices.push(this._invoicesBackup[i].Clone());
            }
        },

        CancelChanges: function () {
            this.Restore();
        },

        ToJson: function () {
            return this.GetInvoice().ToJson();
        },        

        ShowAlreadyInvoicedMessage: function () {
            dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Den här ordern är redan fakturerad. Du kan inte utföra den här aktiviteten."));
        },
       
        CreateBlankOrder: function () {
            var blankOrder = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
            blankOrder.Id = dhHelpdesk.Common.GenerateId();
            blankOrder.ReportedBy = $('#case__ReportedBy').val();
            blankOrder.Persons_Name = $('#case__PersonsName').val();
            blankOrder.Persons_Email = $('#case__PersonsEmail').val();
            blankOrder.Persons_Phone = $('#case__PersonsPhone').val();
            blankOrder.Persons_CellPhone = $('#case__PersonsCellPhone').val();
            blankOrder.Region_Id = $('#case__Region_Id').val();
            blankOrder.Department_Id = $('#case__Department_Id').val();
            blankOrder.OU_Id = $('#case__Ou_Id').val();
            blankOrder.Place = $('#case__Place').val();
            blankOrder.UserCode = $('#case__UserCode').val();
            blankOrder.CostCentre = $('#case__CostCentre').val();
            blankOrder.CreditForOrder_Id = null;
            blankOrder.Project_Id = projectElement.val();
            blankOrder.IsInvoiced = false;

            return blankOrder;
        },

        AddOrder: function (invoiceId) {
            if (this.IsFinishedCase()) {
                return
            }
            var invoice = this.GetInvoice(invoiceId);
            if (invoice != null) {                
                invoice.AddOrder(this.CreateBlankOrder());
            }
        },

        DeleteCurrentOrder: function (invoiceId) {
            var invoice = this.GetInvoice(invoiceId);
            if (invoice != null) {
                var currentOrder = this.GetCurrentOrder();
                if (currentOrder != null) {
                    invoice.DeleteOrder(currentOrder.Id);
                }
            }
        },

        CreateBlankArticle: function () {
            var blank = new dhHelpdesk.CaseArticles.InvoiceArticle();
            blank.Id = dhHelpdesk.Common.GenerateId();
            return blank;
        },

        AddBlankArticle: function (orderId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                if (!this.GetInvoiceStatusOfCurrentOrder()) {
                    var article = this.CreateBlankArticle();
                    var caseArticle = article.ToCaseArticle();
                    caseArticle.Article = article;
                    order.AddArticle(caseArticle);
                    return caseArticle;
                }
                else {
                    this.ShowAlreadyInvoicedMessage();
                    return null;
                }

            }
        },

        RemoveOrderFile: function (fileName) {
            var order = this.GetCurrentOrder();
            if (order != null) {
                order.RemoveOrderFile(fileName);
            }
        },

        DoOrderInvoice: function (orderId) {
            var th = this;
            $.getJSON("/Invoice/InvoiceSettingsValid/?CustomerId=" + th.CustomerId, function (data) {
                if (data != null && data.IsValid == true) {
                    if (!th.IsNewCase()) {
                        var order = th.GetOrder(orderId);
                        if (!order.IsOrderInvoiced()) {
                            var orderValidation = order.Validate();
                            if (orderValidation.IsValid) {                                
                                if (order.InvoiceValidate()) {
                                    order.DoInvoice();
                                }
                            }
                            else
                                dhHelpdesk.Common.ShowErrorMessage(order.Caption + " kunde inte sparas då det saknas data i ett eller flera obligatoriska fält. Var vänlig kontrollera i ordern." + orderValidation.Message);
                        }
                        else {
                            th.ShowAlreadyInvoicedMessage();
                        }
                    }
                    else {
                        dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Du måste spara ärendet en gång innan du kan fakturera") + ".");
                    }
                }
                else {
                    var _message = "";
                    if (data != null)
                        _message = "- " + data.LastMessage;
                    dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Det saknas inställningar för fakturering") + ". " +
                                        dhHelpdesk.Common.Translate("Var god kontakta systemadministratör") + "." + "<br/>" + _message);
                }
            });
        },

        DoOrderCredit: function (orderId) {
            if (orderId < 0) {
                dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate('First save the order.'));
            }
            else {
                var curOrd = dhHelpdesk.CaseArticles.GetOrder(orderId);                
                dhHelpdesk.CaseArticles.GetInvoice(curOrd.Invoice.Id).AddOrder(curOrd.GetCreditedOrder());
                dhHelpdesk.Common.MakeTextNumeric(".article-ppu");
                dhHelpdesk.Common.MakeTextNumeric(".article-amount");
            }
        },

        SelectCaseFiles: function (orderId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                if (!this.GetInvoiceStatusOfCurrentOrder()) {
                    order.SelectCaseFiles();
                }
                else {
                    this.ShowAlreadyInvoicedMessage();
                }
            }
        },

        ShowOtherReference: function (orderId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                $('.InitiatorFieldsOrder' + orderId).show();
            }
            
            $('.icon-plus-sign.showRefrence' + orderId).css("display", "none");
            $('.icon-minus-sign.hideRefrence' + orderId).css("display", "inline-block");
        },

        HideOtherReference: function (orderId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                $('.InitiatorFieldsOrder' + orderId).hide();
            }
            
            $('.icon-plus-sign.showRefrence' + orderId).css("display", "inline-block");
            $('.icon-minus-sign.hideRefrence' + orderId).css("display", "none");
        },

        ShowAttached: function (orderId) {            
            $('.attachedshow').show();            
            $('.icon-plus-sign.showAttached').css("display", "none");
            $('.icon-minus-sign.hideAttached').css("display", "inline-block");
        },

        HideAttached: function (orderId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                $('.attachedshow').hide();
            }

            $('.icon-plus-sign.showAttached').css("display", "inline-block");
            $('.icon-minus-sign.hideAttached').css("display", "none");
        },

        UpdateArticle: function (e) {            
            var id = e.attr("data-id");
            var obj = $("#article-amount_" + id);
            var curAmount = parseInt(obj.val());
            var article = this.GetArticle(id);
            if (article != null) {
                if (article.Amount != null && article.Order.CreditForOrder_Id != null) {
                    var validatedAmount = this.ValidateAmount(article.Order.Id, article.Article.Id, id, curAmount);                    
                    obj.val(validatedAmount);
                }                
                article.Update();
            }
        },
        
        UpdateCharacterCount: function (objectId) {
            var len = $("#Description_" + objectId).val().length;
            var newLen = dhHelpdesk.Common.Translate("Tecken kvar") + ": " + (this.MaxDescriptionCount - len).toString();
            $("#DescriptionCount_" + objectId).text(newLen);
        },

        ValidateAmount: function (curOrder_Id, curArticle_Id, excludeArticleId, newAmount) {           
            var curOrder = this.GetOrder(curOrder_Id);

            // Original Order
            if (curOrder == null || (curOrder != null && curOrder.CreditForOrder_Id == null))
                return newAmount;

            //Credit Order
            var originalOrderId = curOrder.CreditForOrder_Id;
            var originalOrder = this.GetOrder(originalOrderId);
            var originalArticles = [];
            if (originalOrder.Articles != undefined)
                originalArticles = originalOrder.Articles;
            else if (originalOrder._articles != undefined)
                originalArticles = originalOrder._articles;

            var totalOrderAmount = this.GetOrderTotalAmount(originalArticles, curArticle_Id);                                    
            var totalCreditUsed = this.GetCreditUsed(originalOrderId, curArticle_Id, excludeArticleId);

            var newUsedAmout = totalCreditUsed + parseInt(newAmount);
            if (totalOrderAmount < newUsedAmout) {
                
                if (totalOrderAmount - totalCreditUsed > 0) {
                    newAmount = totalOrderAmount - totalCreditUsed;
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Maximum amount for this article is") + ": " + newAmount);
                }
                else {
                    newAmount = 0;
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("All credits already used!"));
                }
            }
           
            return newAmount;
        },

        ValidateNewCreditAmount: function (originalOrderId, creditOrderId, curArticle_Id, newAmount, alreayUsedAmount) {                                   
            var originalOrder = this.GetOrder(originalOrderId);

            var originalArticles = [];
            if (originalOrder.Articles != undefined)
                originalArticles = originalOrder.Articles;
            else if (originalOrder._articles != undefined)
                originalArticles = originalOrder._articles;

            var totalOrderAmount = this.GetOrderTotalAmount(originalArticles, curArticle_Id);
            var totalCreditUsed = this.GetCreditUsed(originalOrderId, curArticle_Id, null) + parseInt(alreayUsedAmount);

            var newUsedAmout = totalCreditUsed + parseInt(newAmount);
            if (totalOrderAmount < newUsedAmout) {
                if (totalOrderAmount - totalCreditUsed > 0) {
                    newAmount = totalOrderAmount - totalCreditUsed;
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Maximum amount for this article is") + ": " + newAmount);
                }
                else {
                    newAmount = 0;
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("All credits already used!"));
                }
            }

            return newAmount;
        },

        GetOrderTotalAmount: function (articles, articleId) {
            var allOrderAmount = 0;
            
            for (var j = 0; j < articles.length; j++) {
                var curArticle = articles[j];
                var curArtId = 0;
                if (curArticle.Article != undefined)
                    curArtId = curArticle.Article.Id;
                else
                    curArtId = curArticle.Id;

                if (curArtId == articleId) {
                    allOrderAmount += parseInt(curArticle.Amount);
                }
            }           
            return allOrderAmount;
        },

        GetCreditUsed: function (originalOrderId, curArticle_Id, excludeArticleId) {
            var creditedAmount = 0;            
            var allCredits = this.GetCreditOrdersFor(this.allVailableOrders, originalOrderId);

            for (var i = 0; i < allCredits.length; i++) {
                var curCredit = allCredits[i];
                var articles = [];
                
                if (curCredit.Articles != undefined) 
                    articles = curCredit.Articles;
                else if (curCredit._articles != undefined) 
                    articles = curCredit._articles;

                for (var j = 0; j < articles.length; j++) {
                    var curArticle = articles[j];
                    var curArtId = 0;
                    if (curArticle.Article != undefined)
                        curArtId = curArticle.Article.Id;
                    else
                        curArtId = curArticle.Id;
                    if (curArtId == curArticle_Id && curArticle.Id != excludeArticleId) {
                        creditedAmount += parseInt(curArticle.Amount);
                    }
                }                
            }

            return creditedAmount;
        },

        DeleteArticle: function (e) {
            var id = e.attr("data-id");
            var article = this.GetArticle(id);
            if (article != null) {
                article.Order.DeleteArticle(id);
            }
        },

        GetCurrentOrder: function () {
            var orderId = this._container.find(".case-invoice-order:visible").attr("data-id");
            var currentOrder = this.GetOrder(orderId);
            if (currentOrder == null) {
                currentOrder = this.GetInvoice().GetLastOrder();
            }

            return currentOrder;
        },

        GetInvoiceStatusOfCurrentOrder: function () {
            var orderId = this._container.find(".case-invoice-order:visible").attr("data-id");
            var currentOrder = this.GetOrder(orderId);
            var OrderIsInvoiced = false;
            if (currentOrder != null) {
                if (!currentOrder.IsOrderInvoiced()) {
                    return false;
                }
                else {
                    OrderIsInvoiced = true;
                    return true;
                }
            }
            return OrderIsInvoiced;
        },

        GetUserSearchOptions: function () {
            var options = {
                items: 20,
                minLength: 2,
                source: function (query, process) {
                    return $.ajax({
                        url: '/cases/search_user',
                        type: 'post',
                        data: { query: query, customerId: $('#case__Customer_Id').val() },
                        dataType: 'json',
                        success: function (result) {
                            var resultList = jQuery.map(result, function (item) {
                                var aItem = {
                                    id: item.Id
                                            , num: item.UserId
                                            , name: item.FirstName + ' ' + item.SurName
                                            , email: item.Email
                                            , place: item.Location
                                            , phone: item.Phone
                                            , usercode: item.UserCode
                                            , cellphone: item.CellPhone
                                            , regionid: item.Region_Id
                                            , regionname: item.RegionName
                                            , departmentid: item.Department_Id
                                            , departmentname: item.DepartmentName
                                            , ouid: item.OU_Id
                                            , ouname: item.OUName
                                            , name_family: item.SurName + ' ' + item.FirstName
                                            , customername: item.CustomerName
                                            , costcentre: item.CostCentre
                                };
                                return JSON.stringify(aItem);

                            });

                            return process(resultList);
                        }
                    });
                },

                matcher: function (obj) {
                    var item = JSON.parse(obj);                    
                    return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.name_family.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.email.toLowerCase().indexOf(this.query.toLowerCase());
                },

                sorter: function (items) {
                    var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                    while (aItem = items.shift()) {
                        var item = JSON.parse(aItem);
                        if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                        else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                        else caseInsensitive.push(JSON.stringify(item));
                    }

                    return beginswith.concat(caseSensitive, caseInsensitive);
                },

                highlighter: function (obj) {
                    var item = JSON.parse(obj);
                    var orgQuery = this.query;
                    if (item.departmentname == null)
                        item.departmentname = ""
                    var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                    var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname;
                    var resultBy_NameFamily = item.name_family + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname;

                    if (result.toLowerCase().indexOf(orgQuery.toLowerCase()) > -1)
                        return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                            return '<strong>' + match + '</strong>';
                        });
                    else
                        return resultBy_NameFamily.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                            return '<strong>' + match + '</strong>';
                        });

                },


                updater: function (obj) {
                    var item = JSON.parse(obj);
                    var OrderId = this.$element.data("orderid")

                    $('#Persons_Name_' + OrderId).val(item.name);
                    $('#Persons_Email_' + OrderId).val(item.email);
                    $('#Persons_Phone_' + OrderId).val(item.phone);
                    $('#Persons_CellPhone_' + OrderId).val(item.CellPhone);

                    $('#RegionId_' + OrderId).val(item.regionid);
                    $('#RegionSelect-Order' + OrderId).val(item.regionid);
                    $('.RegionSelect').change();

                    $('#DepartmentId_' + OrderId).val(item.departmentid);
                    $('#DepartmentSelect-Order' + OrderId).val(item.departmentid);
                    $('.DepartmentSelect').change();

                    $('#OUId_' + OrderId).val(item.ouid);
                    $('#OUSelect-Order' + OrderId).val(item.ouid);
                    $('.OUSelect').change();

                    $('#Place_' + OrderId).val(item.place);
                    $('#UserCode_' + OrderId).val(item.usercode);
                    $('#CostCentre_' + OrderId).val(item.costcentre);
                    return item.num;
                }
            };
            return options;
        },
        
        OrderActions: function (spec, my) {
            var that = {};
            my = my || {};

            var actionsContainer = spec.actionsContainer || null;
            var creditOrderButton = null;

            var get_actionsContainer = function () {
                return actionsContainer;
            };

            var get_creditOrderButton = function (orderId) {                
                 return $(document).find("#actions-credit-order_" + orderId);
            };

            that.get_actionsContainer = get_actionsContainer;
            that.get_creditOrderButton = get_creditOrderButton;

            return that;
        },

        CloseContainerDialog: function () {
            var th = this;
            th.CloseContainer();
            return;

            
            var d = $('<div id="adialog">' + 'testmessage' + '<div>');
            return d.dialog({
                title: dhHelpdesk.Common.Translate('Meddelande'),
                buttons: [
                    {
                        text: 'Ok',
                        click: function () {
                            th.CloseContainer();
                            d.dialog("close");
                        }
                    },
                    {
                        text: 'Avbryt',
                        click: function () {
                            d.dialog("close");
                        }
                    }
                ],
                modal: true
            });
        },
       
        CloseContainer: function () {
            var th = this;
            var invoice = th.GetInvoice();
            th._container = invoice.Container;
            th.CancelChanges();            
            $('.case-invoice-container').remove();
        },        

        CleanUpAllOrders: function () {
            var cleanOrders = [];            
            for (var i = 0; i < this.allVailableOrders.length; i++) {
                if (this.allVailableOrders[i].Id > 0) {
                    var articles = [];
                    if (this.allVailableOrders[i].Articles != undefined)
                        articles = this.allVailableOrders[i].Articles;
                    else if (this.allVailableOrders[i]._articles != undefined)
                        articles = this.allVailableOrders[i]._articles;

                    var validArticles = [];
                    for (var j = 0; j < articles.length; j++) {
                        if (articles[j].Id > 0) {
                            validArticles.push(articles[j]);
                        }
                    }
                    this.allVailableOrders[i].Articles = validArticles;
                    cleanOrders.push(this.allVailableOrders[i]);                        
                }

            }
            this.allVailableOrders = cleanOrders;
        },

        UpdateAvailableArticles: function (orderId, articleId, isAdded, article) {            
            for (var i = 0; i < this.allVailableOrders.length; i++) {
                if (this.allVailableOrders[i].Id == orderId) {
                    var articles = [];
                    if (this.allVailableOrders[i].Articles != undefined)
                        articles = this.allVailableOrders[i].Articles;
                    else if (this.allVailableOrders[i]._articles != undefined)
                        articles = this.allVailableOrders[i]._articles;

                    for (var j = 0; j < articles.length; j++) {
                        if (articles[j].Id == articleId) {
                            if (isAdded)
                                articles[j] = article;
                            else
                                articles.splice(j, 1);
                            return true;
                        }
                    }

                    if (isAdded)
                        articles.push(article);
                    return true;
                }
            }
            return false;
        },

        CreateContainer: function (message) {
            
            var th = this;
            var invoice = th.GetInvoice();
            th._container = invoice.Container;

            var markInvoiceButton = false;
            if (typeof message !== "undefined") {
                th._container.find(".case-invoice-message").text(message);
                markInvoiceButton = true;
            }

            th._container.dialog({
                title: dhHelpdesk.Common.Translate("Artiklar att fakturera"),
                modal: true,
                width: 1100,                
                autoResize: true,
                overflow:"auto",
                resize: function (event, ui) {
                    var thisHeight = $(event.target).height(); //todo fix this - if you resize manually, the window wont automatically resize later on
                    var tabsheight = $('#case-invoice-orders-tabs').height();
                    event.preventDefault();
                },
                close: function () {                                            
                    th.CloseInvoiceWindow(th);                    
                },
                buttons: [
                    {
                        text: dhHelpdesk.Common.Translate("Spara"),
                        'data-doInvoiceOrder':'',
                        'class': 'btn save-invoice',
                        click: function () {                            
                            if (!th.Validate()) {
                                return;
                            }
                            var me = $(".btn.save-invoice");
                            var orderIdToXML = me.attr('data-doInvoiceOrder');
                            me.attr('data-doInvoiceOrder', '');
                            th.ApplyChanges();                            
                            th.SaveToDatabase(th.CloseReOpenWindow, th, orderIdToXML);
                        }
                    },
                    {
                        text: dhHelpdesk.Common.Translate("Spara och stäng"),
                        'class': 'btn save-close-invoice',
                        click: function () {
                            if (!th.Validate()) {
                                return;
                            }
                            th.ApplyChanges();
                            th.SaveToDatabase(th.CloseInvoiceWindow, th);
                        }
                    },
                    {
                        text: dhHelpdesk.Common.Translate("Stäng"),
                        'class': 'btn close-invoice',
                        click: function () {
                            th.CloseInvoiceWindow(th);
                        }
                    }
                ],
                autoOpen: false,
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                    dhHelpdesk.CaseArticles.CleanUpAllOrders();
                    dhHelpdesk.System.RaiseEvent("OnChangeOrder", [th.GetCurrentOrder()]);                    
                }
            });

            th._container.parent().addClass("overflow-visible");
            var curOrder = th.GetCurrentOrder();
                        
            //// set order actions 
            dhHelpdesk.CaseArticles.OrderActionsInstance = dhHelpdesk.CaseArticles.OrderActions({ actionsContainer: th._container.find("#orderButtonsArea") });
            var orderActionsViewModel = dhHelpdesk.CaseArticles.OrderActionsViewModel({
                creditOrderEnabled: curOrder.IsOrderInvoiced(),
                creditOrderTitle: dhHelpdesk.Common.Translate("Kreditera order"),
                creditOrderId: curOrder.Id
            });

            dhHelpdesk.CaseArticles.OrderActionsInstance.get_actionsContainer().prepend($(dhHelpdesk.CaseArticles.CaseInvoiceOrderActionsTemplate.render(orderActionsViewModel)));            
                    
            dhHelpdesk.Common.MakeTextNumeric(".articles-params-units");            
            dhHelpdesk.Common.MakeTextNumeric(".articles-params-units-price");
            dhHelpdesk.Common.MakeTextNumeric(".article-ppu");
            dhHelpdesk.Common.MakeTextNumeric(".article-amount");

            var addEl = th._container.find(".articles-params-add");
            addEl
                .unbind('click')
                .click(function () {                    
                    if (!th.GetInvoiceStatusOfCurrentOrder()) {
                        var unitsEl = th._container.find(".articles-params-units");
                        var units = unitsEl.val();
                        if (!dhHelpdesk.Math.IsInteger(units) || units <= 0) {
                            units = dhHelpdesk.CaseArticles.DefaultAmount;
                            unitsEl.val(units);
                        }
                        var units_PriceEl = th._container.find(".articles-params-units-price");
                        var units_Price = units_PriceEl.val();
                        
                        var articlesEl = th._container.find(".articles-params-article");
                        var articleId = articlesEl.val();
                        var article = th.GetInvoiceArticle(articleId);
                        var elementForFocus = null;
                        if (article != null) {
                            var currentOrder = th.GetCurrentOrder();
                            var curAmount = parseInt(units);
                            if (article.HasChildren()) {
                                for (var i = 0; i < article.Children.length; i++) {
                                    var child = article.Children[i].ToCaseArticle();
                                    child.Amount = units;

                                    // Validate Amount
                                    if (currentOrder.CreditForOrder_Id != null && units != null) {
                                        child.Order = currentOrder;
                                        var validatedAmount = dhHelpdesk.CaseArticles.ValidateAmount(child.Order.Id, child.Article.Id, null, units);
                                        child.Amount = validatedAmount;
                                        articlesEl.val(validatedAmount);
                                    }

                                    if (!child.HasPpu) {                                        
                                        child.Ppu = units_Price;                                                                                
                                        currentOrder.AddArticle(child);
                                        if (elementForFocus == null && units_Price == "") {
                                            elementForFocus = $('#ArtPrice_' + child.Id);
                                        }
                                    }
                                    else
                                        currentOrder.AddArticle(child);

                                    if (child.Article.TextDemand) {
                                        var addedElement = dhHelpdesk.CaseArticles.AddBlankArticle(currentOrder.Id);
                                        if (elementForFocus == null && addedElement != null)
                                            elementForFocus = $('#Description_' + addedElement.Id);
                                    }
                                }
                            } else {
                                var caseArticle = article.ToCaseArticle();
                                caseArticle.Amount = units;

                                // Validate Amount
                                if (currentOrder.CreditForOrder_Id != null) {
                                    caseArticle.Order = currentOrder;
                                    var validatedAmount = dhHelpdesk.CaseArticles.ValidateAmount(caseArticle.Order.Id, caseArticle.Article.Id, null, units);
                                    caseArticle.Amount = validatedAmount;
                                    articlesEl.val(validatedAmount);
                                }
                                
                                if (!caseArticle.HasPpu) {
                                    caseArticle.Ppu = units_Price;                                    
                                    currentOrder.AddArticle(caseArticle);
                                    if (elementForFocus == null && units_Price == "") {
                                        elementForFocus = $('#ArtPrice_' + caseArticle.Id);
                                    }                                        
                                }
                                else
                                    currentOrder.AddArticle(caseArticle);

                                if (caseArticle.Article.TextDemand) {
                                    var addedElement = dhHelpdesk.CaseArticles.AddBlankArticle(currentOrder.Id);
                                    if (elementForFocus == null && addedElement != null)
                                        elementForFocus = $('#Description_' + addedElement.Id);
                                }

                            }                            

                            
                            if (elementForFocus == null)                                
                                articlesEl.trigger('chosen:activate');
                            else
                                elementForFocus.focus();

                            articlesEl.val('').trigger('chosen:updated');
                            units_PriceEl.val('');
                            units_PriceEl.prop("readonly", false);
                            units_PriceEl.prop("disabled", false);
                            unitsEl.val(dhHelpdesk.CaseArticles.DefaultAmount);
                                                    
                        }
                    }
                    else {
                        th.ShowAlreadyInvoicedMessage();
                    }
                });
        },
         
        ValidateOpenInvoiceWindow: function(){
            var isValid = true;
            //if (dhHelpdesk.Common.IsNullOrEmpty($('#case__CostCentre').val())) {
            //    dhHelpdesk.CaseArticles.ShowErrorMessage(dhHelpdesk.Common.Translate('Kostnadsställe saknas på ärendet.'))
            //}
            return isValid;
        },

        CloseInvoiceWindow: function (that) {            
            caseButtonsToDisable.removeClass('disabled');
            caseButtonsToDisable.css("pointer-events", "");
            that.CloseContainerDialog();
        },

        CloseReOpenWindow: function (that) {
            caseButtonsToDisable.removeClass('disabled');
            caseButtonsToDisable.css("pointer-events", "");
            that.CloseContainer();
            $('#InvoiceModuleBtnOpen').click();                        
        },

        OpenInvoiceWindow: function (message) {            
            var th = dhHelpdesk.CaseArticles;
            th.CreateContainer(message);
            var addArticleEl = th._container.find(".articles-params");
            var articlesSelectContainer = addArticleEl.find(".articles-select-container");
            articlesSelectContainer.empty();
            var articlesEl = $("<select class='chosen-select articles-params-article'></select>");                        
            articlesSelectContainer.append(articlesEl);

            var articles = th.GetInvoiceArticles();
            if (articles == null || articles.length == 0) {
                addArticleEl.hide();                
            } else {
                addArticleEl.show();
            }

            articlesEl.append("<option value='0'> </option>");
            for (var i = 0; i < articles.length; i++) {
                var article = articles[i];
                articlesEl.append("<option value='" + article.Id + "'>" + article.GetFullName() + "</option>");
            }

            $('.InitiatorField').hide();
                       
            articlesEl.chosen({
                width: "500px",
                height: "100px",                
                placeholder_text_single: dhHelpdesk.Common.Translate("Välj artikel"),
                'no_results_text': '?',                                
            })
            .change(function () {
                var selectedArticle = dhHelpdesk.CaseArticles.GetInvoiceArticle(articlesEl.chosen().val());                
                var units_PriceEl = th._container.find(".articles-params-units-price");

                if (selectedArticle != null) {
                    if (selectedArticle.HasChildren()) {
                        units_PriceEl.val('');
                        units_PriceEl.prop("readonly", false);
                        units_PriceEl.prop("disabled", false);

                    }
                    else
                    {                                                
                        if (!selectedArticle.HasPpu) {
                            units_PriceEl.val('');
                            units_PriceEl.prop("readonly", false);
                            units_PriceEl.prop("disabled", false);
                        }
                        else
                        {
                            units_PriceEl.val(selectedArticle.Ppu);
                            units_PriceEl.prop("readonly", true);
                            units_PriceEl.prop("disabled", true);
                        }
                    }
                }                
            });
            
            articlesSelectContainer.find("select.chosen-select").addClass("min-width-500 max-width-500 case-invoice-multiselect");            
            
            caseButtonsToDisable.addClass("disabled");
            caseButtonsToDisable.css("pointer-events", "none");
            th._container.dialog("open");
            

            if (th.IsFinishedCase()) {
                $('.case-invoice-container *').attr("disabled", true);
                $('#actions-credit-order').attr("disabled", true);
            }
            //dhHelpdesk.CaseArticles.DoDelimit();
            th.FillOrganisationDataForOtherReference();
        },

        Initialize: function (e) {
            var th = this;
            th.ProductAreaElement = $(document).find("." + e.attr("data-invoice-product-area"));
            th.ModuleCaseInvoiceElement = $('#ModuleCaseInvoice');
            th.CaseId = e.attr("data-invoice-case-id");
            th.CustomerId = e.attr("data-invoice-customerId");
            th.CaseInvoicesElement = $(document).find("." + e.attr("data-invoice-articles-for-save"));
            th.DateFormat = e.attr("data-invoice-date-format");
            var ButtonCaption = e.attr("data-invoice-Caption");
            var ButtonHint = e.attr("data-invoice-Hint");
            
            var button = $(document.createElement("input"))
                .attr("id", "InvoiceModuleBtnOpen")
                .attr("type", "button")
                .attr("value", ButtonCaption)
                .attr("title", ButtonHint)
                .addClass("btn");

            button.click(function () {
                if (th.IsNewCase()) {
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Please save the case and then try again!"));
                    return;
                }
                if (th.ValidateOpenInvoiceWindow()) {
                    th.OpenInvoiceWindow();                    
                }
            });
            e.after(button);

            var onChangeProductArea = function () {
                
                button.hide();
                invoiceButtonIndicator.show();
                $.getJSON("/invoice/articles?caseId="+ th.CaseId + "&productAreaId=" + th.ProductAreaElement.val(), function (data) {
                    th.ClearInvoiceArticles();
                    if (data.Data == "" && data.Data.length == 0) {
                        if (data.CaseHasInvoice)
                            button.show();
                        invoiceButtonIndicator.hide();
                    }
                    else {                        
                        for (var i = 0; i < data.Data.length; i++) {
                            var a = data.Data[i];
                            var article = new dhHelpdesk.CaseArticles.InvoiceArticle();
                            article.Id = a.Id;
                            article.ParentId = a.ParentId;
                            article.Number = a.Number;
                            article.Name = a.Name;
                            article.NameEng = a.NameEng;
                            article.Description = a.Description;
                            article.TextDemand = a.TextDemand;
                            article.UnitId = a.UnitId;
                            if (a.Unit != null) {
                                article.Unit = new dhHelpdesk.CaseArticles.InvoiceArticleUnit();
                                article.Unit.Id = a.Unit.Id;
                                article.Unit.Name = a.Unit.Name;
                                article.Unit.CustomerId = a.Unit.CustomerId;
                            }
                            article.Ppu = a.Ppu;
                            article.HasPpu = (a.Ppu != null && a.Ppu != 0);
                            article.ProductAreaId = a.ProductAreaId;
                            article.CustomerId = a.CustomerId;
                            th.AddInvoiceArticle(article);
                        }

                        var articles = th.GetInvoiceArticles();
                        for (var j = 0; j < articles.length; j++) {
                            var parent = articles[j];
                            for (var k = 0; k < articles.length; k++) {
                                var child = articles[k];
                                if (child.ParentId == parent.Id) {
                                    parent.AddChild(child);
                                }
                            }
                        }
                        button.show();
                        invoiceButtonIndicator.hide();
                    }                    
                });
            };

            th.ProductAreaElement.change(function () {
                onChangeProductArea();
            });
            onChangeProductArea();
        },      
        
        Validate: function () {
            var isValid = true;
            var invoices = this.GetInvoices();
            for (var i = 0; i < invoices.length; i++) {
                var invoice = invoices[i];
                if (!invoice.Validate()) {
                    isValid = false;
                }
            }
            return isValid;
        },

        SortOrders: function (allOrders) {
            var sortedOrders = [];
            var originalOrders = this.GetOriginalOrders(allOrders);
            for (var i = 0; i < originalOrders.length; i++) {
                originalOrders[i].Caption = dhHelpdesk.Common.Translate("Order") + " " + (i + 1);
                sortedOrders.push(originalOrders[i]);
                var creditOrders = this.GetCreditOrdersFor(allOrders, originalOrders[i].Id);
                for (var j = 0; j < creditOrders.length; j++) {
                    creditOrders[j].Caption = dhHelpdesk.Common.Translate("Kredit") + " " + (j + 1) + " " + dhHelpdesk.Common.Translate("för") + " " + originalOrders[i].Caption;
                    sortedOrders.push(creditOrders[j]);
                }
            }
            return sortedOrders;
        },

        GetOriginalOrders: function (allOrders) {             
            var ret = [];
            for (var i = 0; i < allOrders.length; i++) {
                if (allOrders[i].CreditForOrder_Id == null)
                    ret.push(allOrders[i]);
            }
            return ret.sort(function (a1, a2) { return a1.Number - a2.Number; });
        },

        GetCreditOrdersFor: function (allOrders, orderId) {
            var ret = [];
            for (var i = 0; i < allOrders.length; i++) {
                if (allOrders[i].CreditForOrder_Id == orderId)
                    ret.push(allOrders[i]);
            }            
            return ret.sort(function (a1, a2) { return a1.Number - a2.Number; });
        },

        InvoiceArticle: function () {
            this.Id = null;
            this.ParentId = null;
            this.Number = null;
            this.Name = null;
            this.NameEng = null;
            this.Description = null;
            this.UnitId = null;
            this.Unit = null;
            this.Ppu = null;
            this.HasPpu = false;
            this.ProductAreaId = null;
            this.CustomerId = null;
            this.TextDemand = null;

            this.Parent = null;
            this.Children = [];

            this.IsBlank = function () {
                return this.Number == null;
            };

            this.AddChild = function (child) {
                this.Children.push(child);
                child.Parent = this;
            };

            this.HasChildren = function () {
                return this.Children.length > 0;
            };

            this.GetName = function () {
                var name = "";
                if (this.Name != null && this.Name != "") {
                    name = this.Name;
                }
                if (this.NameEng != null && this.NameEng != "") {
                    if (name != "") {
                        name += " - " + this.NameEng;
                    }
                    else {
                        name += this.NameEng;
                    }                    
                }
                return name;
            };

            this.GetFullName = function () {
                var fullName;
                if (this.Number != null) {
                    fullName = this.Number + " - " + this.GetName();
                } else {
                    fullName = this.GetName();
                }

                if (this.Unit != null && this.Ppu != null) {
                    fullName += " (" + this.Ppu + " - " + this.Unit.Name + ")";
                }
                else if (this.Unit != null) {
                    fullName += " (" + this.Unit.Name + ")";
                }

                return fullName;
            };

            this.ToCaseArticle = function () {
                var article = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                article.Id = dhHelpdesk.Common.GenerateId();
                article.Article = this;
                article.Name = this.Name;
                article.Amount = dhHelpdesk.CaseArticles.DefaultAmount;                
                article.Ppu = this.Ppu;
                article.HasPpu = this.HasPpu;
                return article;
            };
        },

        CaseInvoice: function () {
            this.Id = null;
            this.CaseId = null;
            this._orders = [];

            this.GetOrders = function () {
                return this._orders;
            },

            this.GetOrder = function (id) {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    if (order.Id == id) {
                        return order;
                    }
                }
                return null;
            },

            this.GetLastOrder = function () {
                if (this._orders.length == 0) {
                    return null;
                }

                return this._orders[this._orders.length - 1];
            },

            this.GetArticle = function (id) {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var article = orders[i].GetArticle(id);
                    if (article != null) {
                        return article;
                    }
                }
                return null;
            },

            this.HasArticles = function () {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    if (order.HasArticles()) {
                        return true;
                    }
                }
                return false;
            },            

            this._refreshTabs = function () {
                var tabs = this.Container.find("#case-invoice-orders-tabs");
                tabs.tabs("refresh");
                tabs.find("ul").removeClass("ui-widget-header");
                tabs.find("li").removeClass("ui-state-default");
            },

            this.AddOrder = function (order) {
                order.Invoice = this
                order.Initialize();
                var origOrders = dhHelpdesk.CaseArticles.GetOriginalOrders(dhHelpdesk.CaseArticles.allVailableOrders);
                if (order.Caption == null || order.Caption == "") {
                    order.Caption = dhHelpdesk.Common.Translate("Order") + " " + (origOrders.length + 1);
                    if (order.CreditForOrder_Id != null) {
                        var sourceOrder = this.GetOrder(order.CreditForOrder_Id);
                        var credits = dhHelpdesk.CaseArticles.GetCreditOrdersFor(dhHelpdesk.CaseArticles.allVailableOrders, sourceOrder.Id);
                        if (sourceOrder != null)
                            order.Caption = dhHelpdesk.Common.Translate("Kredit") + " " + (credits.length + 1) + " " +
                                            dhHelpdesk.Common.Translate("för") + " " + sourceOrder.Caption;
                    }
                    if (order.Id < 0) {
                        dhHelpdesk.CaseArticles.allVailableOrders.push(order);
                        order.Number = dhHelpdesk.CaseArticles.allVailableOrders.length;
                    }
                }                                   
                               
                this._orders.push(order);
                
                var container = this.Container.find(".orders-container");
                container.append(order.Container);
                
                var tabs = this.Container.find("#case-invoice-orders-tabs");
                var tabIcon = (order.IsOrderInvoiced() ? "<i class='icon-ok icon-green'></i>&nbsp;&nbsp;" : "");
                var newTab = $("<li class='case-invoice-order-tab active'><a href='#case-invoice-order" + order.Id + "'>" + tabIcon + order.Caption  + "</a><li>");
                if (this._orders.length == 1) {
                    tabs.find("ul").prepend(newTab);
                } else {
                    tabs.find("li.case-invoice-order-tab:last").after(newTab);
                }
                this._refreshTabs();
                //dhHelpdesk.CaseArticles.FillOrganisationDataForOtherReference();
                newTab.find("a").click();

                $('.articles-params-units').focus();
                $('.articles-params-units').click();                
                // Deselect Article drop down 
                
                var units_PriceEl = this.Container.find(".articles-params-units-price");
                units_PriceEl.val('');
                units_PriceEl.prop("readonly", false);
                units_PriceEl.prop("disabled", false);

                var units_El = this.Container.find(".articles-params-units");
                units_El.val(dhHelpdesk.CaseArticles.DefaultAmount);                

                var articleEl = this.Container.find(".chosen-select.articles-params-article");
                articleEl.find('option:selected').prop('selected', false);                
                articleEl.trigger('chosen:updated');
                articleEl.trigger('chosen:activate');
                dhHelpdesk.CaseArticles.HideOtherReference(order.Id);                
            },
            
            this.DeleteOrder = function (id) {
                if (this._orders.length <= 1) {
                    return;
                }
                
                for (var i = 0; i < this._orders.length; i++) {
                    var order = this._orders[i];
                    if (order.Id == id) {
                        this._orders.splice(i, 1);
                        var tabs = this.Container.find("#case-invoice-orders-tabs");
                        tabs.find("[href='#case-invoice-order" + order.Id + "']").parent().remove();
                        order.Container.remove();
                        this._refreshTabs();
                        this.UpdateTotal();
                        return;
                    }
                }
            },

            this.ToJson = function () {
                var ordersResult = "";
                var orders = this.GetOrders();
                
                var projectId = null;
                if (projectElement != undefined)
                    projectId = projectElement.val();

                for (var i = 0; i < orders.length; i++) {
                    var curOrder = orders[i];

                    if (!curOrder.IsInvoiced && curOrder.CreditForOrder_Id == null)
                        curOrder.Project_Id = projectId;

                    ordersResult += curOrder.ToJson();
                    if (i < orders.length - 1) {
                        ordersResult += ", ";
                    }
                }

                return '{' +
                        '"Id":"' + (this.Id >= 0 ? this.Id : 0) + '", ' +
                        '"CaseId":"' + this.CaseId + '", ' +
                        '"Orders": [' + ordersResult + ']' +
                        '}';
            },           

            this.Clone = function () {                
                var clone = new dhHelpdesk.CaseArticles.CaseInvoice();
                clone.Id = this.Id;
                clone.CaseId = this.CaseId;                
                clone.Initialize();
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    clone.AddOrder(orders[i].Clone());
                }
                return clone;
            },

            this.GetViewModel = function () {
                var model = new dhHelpdesk.CaseArticles.CaseInvoiceViewModel();
                model.Id = this.Id != null ? this.Id : dhHelpdesk.Common.GenerateId();
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    if (orders[i]._articles.length > 0)
                        model.AddOrder(orders[i].GetViewModel());
                }

                var totals = this.GetArticlesTotal();
                model.Total = dhHelpdesk.CaseArticles.DoDelimit(totals.Total.toString());
                model.TotalInvoiced = dhHelpdesk.CaseArticles.DoDelimit(totals.Invoiced.toString());
                model.TotalNotInvoiced = dhHelpdesk.CaseArticles.DoDelimit(totals.NotInvoiced.toString());                

                return model;
            },

            this.ShowSummary = function () {
                this.Container.find(".case-invoice-order-summary").show();
            },

            this.HideSummary = function () {
                this.Container.find(".case-invoice-order-summary").hide();
            },

            this.GetCurrentOrder = function () {
                var orderId = this.Container.find(".case-invoice-order:visible").attr("data-id");
                return this.GetOrder(orderId);
            },

            this.Initialize = function () {                
                this.Container = $(dhHelpdesk.CaseArticles.CaseInvoiceTemplate.render(this.GetViewModel()));                
                var tabs = this.Container.find("#case-invoice-orders-tabs");
                var th = this;
                tabs.tabs({
                    activate: function (event, ui) {
                        tabs.find(".case-invoice-order-tab").removeClass("active");
                        tabs.find(".case-invoice-order-summary").removeClass("active");
                        ui.newTab.addClass("active");
                        var isSummaryTab = ui.newTab.hasClass("case-invoice-order-summary");
                        if (isSummaryTab) {
                            th.Container.find(".articles-params").hide();
                            th.Container.find(".case-invoice-order-summary-container")
                                .html(dhHelpdesk.CaseArticles.CaseInvoiceOverviewTemplate.render(th.GetViewModel()));
                        } else {
                            th.Container.find(".articles-params").show();
                        }

                        // Raise OnChangeOrder event
                        dhHelpdesk.System.RaiseEvent("OnChangeOrder", [th.GetCurrentOrder()]);
                    }
                });

                dhHelpdesk.System.OnEvent("OnAddArticle", function (e, order, article) {                   
                    th.ShowSummary();
                    dhHelpdesk.Common.MakeTextNumeric(".article-ppu");
                    dhHelpdesk.Common.MakeTextNumeric(".article-amount");
                    dhHelpdesk.CaseArticles.UpdateAvailableArticles(order.Id, article.Id, true, article);                                                           
                });                

                dhHelpdesk.System.OnEvent("OnDeleteArticle", function (e, order, articleId) {
                    if (th.HasArticles()) {
                        th.ShowSummary();
                    } else {
                        th.HideSummary();
                    }
                    dhHelpdesk.CaseArticles.UpdateAvailableArticles(order.Id, articleId, false, null);
                });

                // Set handler for OnChangeOrder event
                dhHelpdesk.System.OnEvent("OnChangeOrder", function (e, currentOrder) {
                    if (currentOrder == null) {
                        return;
                    }                    
                   
                    var addArticleEl = dhHelpdesk.CaseArticles._container.find(".articles-params");
                    if (currentOrder.IsOrderInvoiced()) {
                        if (currentOrder.CreditForOrder_Id == null)
                            dhHelpdesk.CaseArticles.OrderActionsInstance.get_creditOrderButton(currentOrder.Id).show();
                        else
                            dhHelpdesk.CaseArticles.OrderActionsInstance.get_creditOrderButton(currentOrder.Id).hide();                        
                        $("#doInvoiceButton_" + currentOrder.Id).hide();

                        var allInitiatorFields = $(".InitiatorFields_" + currentOrder.Id);
                        $.each(allInitiatorFields, function (i) {
                            $(allInitiatorFields[i]).attr("disabled", true);
                        });
                        $("#invoiceSelectFile_" + currentOrder.Id).attr("disabled", true);
                        addArticleEl.hide();                        
                    } else {
                        dhHelpdesk.CaseArticles.OrderActionsInstance.get_creditOrderButton(currentOrder.Id).hide();                        
                        $("#doInvoiceButton_" + currentOrder.Id).show();
                        var articles = dhHelpdesk.CaseArticles.GetInvoiceArticles();
                        if (articles == null || articles.length == 0)
                            addArticleEl.hide();
                        else {
                            dhHelpdesk.CaseArticles.ResetAddArticlePlace();
                            addArticleEl.show();
                        }
                    }

                    if (currentOrder.Id < 0)
                        $("#doInvoiceButton_" + currentOrder.Id).hide();
                });
            },
        
            this.GetArticlesTotal = function () {                
                var total = { Invoiced: 0, NotInvoiced: 0, Total: 0 };
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var t = orders[i].GetArticlesTotal();
                    total.Invoiced += t.Invoiced;
                    total.NotInvoiced += t.NotInvoiced;
                    total.Total += t.Total;
                }
                return total;
            },

            this.UpdateTotal = function () {
                //this.Container.find("#case-invoice-order-summary-total").text(this.GetArticlesTotal());
            },

            this.Validate = function () {
                var isValid = true;
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    var orderValidate = order.Validate();
                    if (!orderValidate.IsValid) {
                        dhHelpdesk.Common.ShowErrorMessage(order.Caption + " kunde inte sparas då det saknas data i ett eller flera obligatoriska fält. Var vänlig kontrollera i ordern." + orderValidate.Message);
                        return false;                        
                    }
                }
                return isValid;
            }
        },

        CaseInvoiceOrder: function () {
            this.Id = null;
            this.Invoice = null;
            this.Number = null;
            this.InvoiceDate = null;
            this.InvoicedByUser = null;
            this.InvoicedByUserId = null;            
            this.Date = null;
            this.Caption = null;
            this.IsInvoiced = false;

            //////////////
            /// INITIATOR FIELDS
            this.ReportedBy = null;
            this.Persons_Name = null;
            this.Persons_Email = null;
            this.Persons_Phone = null;
            this.Persons_CellPhone = null;
            this.Region_Id = null;
            this.Department_Id = null;
            this.OU_Id = null;
            this.Place = null;
            this.UserCode = null;
            this.CostCentre = null;
            this.CreditForOrder_Id = null;
            this.Project_Id = null;
            /////

            this._articles = [];
            this.Container = null;
            this.CreditedFrom = null;
            this.files = dhHelpdesk.CaseArticles.OrderFilesCollection({});
            
            this.GetArticles = function () {
                return this._articles;
            },

            this.HasArticles = function () {
                return this._articles.length > 0;
            },

            this.GetArticle = function (id) {
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (article.Id == id) {
                        return article;
                    }
                }
                return null;
            },

            this.HasNotBlankArticles = function () {
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (!article.IsBlank()) {
                        return true;
                    }
                }
                return false;
            },

            this.IsOrderInvoiced = function () {
                return (this.InvoicedByUserId != undefined && this.InvoicedByUserId != null && this.InvoicedByUserId > 0) ||
                       (this.InvoiceDate != undefined && this.InvoiceDate != null);
            },
            
            this.EnableAddBlank = function (enable) {
                var addBlank = this.Container.find(".add-blank-article");
                if (enable) {
                    addBlank.css("display", "block");
                } else {
                    addBlank.css("display", "none");
                }
            },

            this.AddArticle = function (article) {
                this._articles.push(article);
                article.Order = this;

                if (article.Position == null) {
                    article.Position = this._articles.length - 1;
                }
                article.Initialize();

                var rows = this.Container.find(".articles-rows");
                rows.append(article.Container);                
                this.EnableAddBlank(this.HasNotBlankArticles());
                this.UpdateTotal();

                dhHelpdesk.System.RaiseEvent("OnAddArticle", [this, article]);
            },

            this.DeleteArticle = function (id) {
                for (var i = 0; i < this._articles.length; i++) {
                    var article = this._articles[i];
                    if (article.Id == id) {
                        this._articles.splice(i, 1);
                        this._deleteFromContainer(article.Id);
                        this.EnableAddBlank(this.HasNotBlankArticles());
                        break;
                    }
                }

                var articlesForDelete = [];
                for (var j = 0; j < this._articles.length; j++) {
                    var a = this._articles[j];
                    if (!a.IsBlank()) {
                        break;
                    }
                    articlesForDelete.push(a.Id);
                }

                for (var j = 0; j < articlesForDelete.length; j++) {
                    var articleForDelete = articlesForDelete[j];
                    this._articles.splice(articleForDelete, 1);
                    this._deleteFromContainer(articleForDelete);
                }

                this.UpdateTotal();

                dhHelpdesk.System.RaiseEvent("OnDeleteArticle", [this, id]);
            },

            this.GetSortedArticles = function () {
                return this._articles.sort(function (a1, a2) { return a1.Position - a2.Position; });
            },

            this.ToJson = function () {
                var articlesResult = "";
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    articlesResult += articles[i].ToJson();
                    if (i < articles.length - 1) {
                        articlesResult += ", ";
                    }
                }

                var filesResult = "";
                var files = this.files.getFiles();
                for (var j = 0; j < files.length; j++) {
                    filesResult += files[j].toJson();
                    if (j < files.length - 1) {
                        filesResult += ", ";
                    }
                }                

                return '{' +
                        '"Id":"' + (this.Id >= 0 ? this.Id : 0) + '", ' +
                        '"InvoiceId":"' + (this.Invoice.Id > 0 ? this.Invoice.Id : 0) + '", ' +
                        '"Number":"' + this.Number + '", ' +
                        '"InvoiceDate":"' + dhHelpdesk.Common.DateToJSONDateTime(this.InvoiceDate) + '", ' +
                        '"InvoicedByUser":"' + (this.InvoicedByUser != null ? this.InvoicedByUser : '') + '", ' +
                        '"InvoicedByUserId":"' + (this.InvoicedByUserId != null ? this.InvoicedByUserId : '') + '", ' +
                        '"ReportedBy":"' + (this.ReportedBy != null ? this.ReportedBy : '') + '", ' +
                        '"Persons_Name":"' + (this.Persons_Name != null ? this.Persons_Name : '') + '", ' +
                        '"Persons_Email":"' + (this.Persons_Email != null ? this.Persons_Email : '') + '", ' +
                        '"Persons_Phone":"' + (this.Persons_Phone != null ? this.Persons_Phone : '') + '", ' +
                        '"Persons_CellPhone":"' + (this.Persons_CellPhone != null ? this.Persons_CellPhone : '') + '", ' +
                        '"Region_Id":"' + (this.Region_Id != null ? this.Region_Id : '') + '", ' +
                        '"Department_Id":"' + (this.Department_Id != null ? this.Department_Id : '') + '", ' +
                        '"OU_Id":"' + (this.OU_Id != null ? this.OU_Id : '') + '", ' +
                        '"CostCentre":"' + (this.CostCentre != null ? this.CostCentre : '') + '", ' +
                        '"Place":"' + (this.Place != null ? this.Place : '') + '", ' +
                        '"UserCode":"' + (this.UserCode != null ? this.UserCode : '') + '", ' +
                        '"CreditForOrder_Id":"' + (this.CreditForOrder_Id != null ? this.CreditForOrder_Id : '') + '", ' +
                        '"Project_Id":"' + (this.Project_Id != null ? this.Project_Id : '') + '", ' +
                        '"Articles": [' + articlesResult + '],' +
                        '"Files": [' + filesResult + ']' +
                        '}';
            },

            this.GetArticlesTotal = function () {
                var total = {Invoiced:0, NotInvoiced:0, Total:0};
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    if (articles[i].Order.IsOrderInvoiced())
                        total.Invoiced += articles[i].GetTotal();
                    else
                        total.NotInvoiced += articles[i].GetTotal();

                    total.Total += articles[i].GetTotal();
                }
                return total;
            },

            this.IsArticlesEmpty = function () {
                return this._articles.length == 0;
            },

            this.DoInvoice = function () {                               
                var btnSave = $(".btn.save-invoice");
                //pass order_id to generate xml for order
                btnSave.attr("data-doInvoiceOrder", this.Id);
                btnSave.click();

                dhHelpdesk.System.RaiseEvent("OnChangeOrder", [this]);                
            },

            this.Clone = function () {
                var clone = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                clone.Id = this.Id;
                clone.Number = this.Number;
                clone.InvoiceDate = this.InvoiceDate;
                clone.InvoicedByUser = this.InvoicedByUser;
                clone.InvoicedByUserId = this.InvoicedByUserId;
                clone.Caption = this.Caption;
                clone.IsInvoiced = this.IsInvoiced;

                ////// INITIATOR FIELDS
                clone.ReportedBy = this.ReportedBy;
                clone.Persons_Name = this.Persons_Name;
                clone.Persons_Email = this.Persons_Email;
                clone.Persons_Phone = this.Persons_Phone;
                clone.Persons_CellPhone = this.Persons_CellPhone;
                clone.Region_Id = this.Region_Id;
                clone.Department_Id = this.Department_Id;
                clone.OU_Id = this.OU_Id;
                clone.Place = this.Place;
                clone.UserCode = this.UserCode;
                clone.CostCentre = this.CostCentre;
                clone.CreditForOrder_Id = this.CreditForOrder_Id;
                clone.Project_Id = this.Project_Id;

                clone.CreditedFrom = this.CreditedFrom;
                clone.Initialize();
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    clone.AddArticle(articles[i].Clone());
                }
                clone.files = this.files.clone(clone.IsInvoiced);

                return clone;
            },

            // Get credited from this order
            this.GetCreditedOrder = function () {
                var credited = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                credited.Id = dhHelpdesk.Common.GenerateId();
                credited.CreditedFrom = this;

                ///////////////
                credited.ReportedBy = this.ReportedBy;
                credited.Persons_Name = this.Persons_Name;
                credited.Persons_Email = this.Persons_Email;
                credited.Persons_Phone = this.Persons_Phone;
                credited.Persons_CellPhone = this.Persons_CellPhone;
                credited.Region_Id = this.Region_Id;
                credited.Department_Id = this.Department_Id;
                credited.OU_Id = this.OU_Id;
                credited.Place = this.Place;
                credited.UserCode = this.UserCode;
                credited.CostCentre = this.CostCentre;
                credited.CreditForOrder_Id = this.Id;
                credited.Project_Id = this.Project_Id;

                credited.Initialize();
                var articles = this.GetArticles();
                var alreadyUsedAmount = [];
                for (var i = 0; i < articles.length; i++) {
                    var a = articles[i];                    
                    var article = a.GetCreditedArticle(credited, alreadyUsedAmount);
                    if (a.Article != null)
                        alreadyUsedAmount.push({ Id: a.Article.Id, Amount: article.Amount });
                    if (a.Article == null || (a.Article != null && parseInt(article.Amount) > 0))
                        credited.AddArticle(article);
                }
                return credited;
            },

            this.GetViewModel = function () {
                var model = new dhHelpdesk.CaseArticles.CaseInvoiceOrderViewModel();
                model.Id = this.Id != null ? this.Id : dhHelpdesk.Common.GenerateId();
                model.Number = this.Number + 1;

                var totalPrices = this.GetArticlesTotal();
                model.Total = dhHelpdesk.CaseArticles.DoDelimit(totalPrices.Total.toString());
                
                model.InvoiceDate = this.InvoiceDate != null && this.InvoiceDate != "" ? dhHelpdesk.Common.DateToDisplayDate(this.InvoiceDate) : "";
                model.InvoiceTime = this.InvoiceDate != null && this.InvoiceDate != "" ? dhHelpdesk.Common.DateToDisplayTime(this.InvoiceDate) : "";
                model.InvoicedByUser = this.InvoicedByUser != null ? this.InvoicedByUser : "";
                model.InvoicedByUserId = this.InvoicedByUserId != null ? this.InvoicedByUserId : "";                
                model.IsInvoiced = this.IsOrderInvoiced();                

                ////////initiator field
                model.ReportedBy = this.ReportedBy != null ? this.ReportedBy : "";
                model.ShowReportedBy = dhHelpdesk.CaseArticles.ShowCaseField('ReportedBy')

                model.Persons_Name = this.Persons_Name != null ? this.Persons_Name : "";
                model.ShowPersons_Name = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Name');

                model.Persons_Email = this.Persons_Email != null ? this.Persons_Email : "";
                model.ShowPersons_Email = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Email');

                model.Persons_Phone = this.Persons_Phone != null ? this.Persons_Phone : "";
                model.ShowPersons_Phone = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Phone');

                model.Persons_CellPhone = this.Persons_CellPhone != null ? this.Persons_CellPhone : "";
                model.ShowPersons_CellPhone = dhHelpdesk.CaseArticles.ShowCaseField('Persons_CellPhone');

                model.Region_Id = this.Region_Id != null ? this.Region_Id : "";
                model.ShowRegion_Id = dhHelpdesk.CaseArticles.ShowCaseField('Region_Id');

                model.Department_Id = this.Department_Id != null ? this.Department_Id : "";
                model.ShowDepartment_Id = dhHelpdesk.CaseArticles.ShowCaseField('Department_Id');

                model.OU_Id = this.OU_Id != null ? this.OU_Id : "";
                model.ShowOU_Id = dhHelpdesk.CaseArticles.ShowCaseField('OU_Id');

                model.Place = this.Place != null ? this.Place : "";
                model.ShowPlace = dhHelpdesk.CaseArticles.ShowCaseField('Place');

                model.UserCode = this.UserCode != null ? this.UserCode : "";
                model.ShowUserCode = dhHelpdesk.CaseArticles.ShowCaseField('UserCode');

                model.CostCentre = this.CostCentre != null ? this.CostCentre : "";
                model.ShowCostCentre = dhHelpdesk.CaseArticles.ShowCaseField('CostCentre');
                                
                model.TitleCaption = this.Caption;                

                var articles = this.GetSortedArticles();
                for (var i = 0; i < articles.length; i++) {
                    model.AddArticle(articles[i].GetViewModel());
                }
                model.files = this.files.getFiles(model.IsInvoiced);
                return model;
            },
            
            this.Initialize = function () {
                if (this.Container != null) {
                    this.UpdateFiles();
                    return;
                }

                this.Container = $(dhHelpdesk.CaseArticles.CaseInvoiceOrderTemplate.render(this.GetViewModel()));
                this.UpdateFiles();

                var rows = this.Container.find(".articles-rows");                
                var th = this;
                rows.sortable({
                    stop: function () {
                        var newFirstArticle = th.GetArticle(rows.find("tr").first().attr("data-id"));
                        if (newFirstArticle.IsBlank()) {
                            rows.sortable("cancel");
                            return;
                        }
                        var i = 0;
                        rows.find("tr").each(function () {
                            var article = th.GetArticle($(this).attr("data-id"));
                            if (article != null) {
                                article.Position = i;
                            }
                            i++;
                        });
                        th.GetArticles().sort(function (a, b) { return a.Position - b.Position; });
                    }
                });
                
                var publicClassName = ".InitiatorFields_" + th.Id;
                var ReportedBy = this.Container.find(publicClassName + ".reportedby");
                ReportedBy.typeahead(dhHelpdesk.CaseArticles.GetUserSearchOptions());

                ReportedBy.blur(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        th.ReportedBy = $(this).val();
                        email.blur();
                        personsName.blur();
                        phone.blur();
                        CellPhone.blur();
                        region.blur();
                        department.blur();
                        ou.blur();
                        place.blur();
                        costcentre.blur();
                        usercode.blur();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.ReportedBy);
                    }
                });                

                var personsName = this.Container.find(publicClassName + ".name");
                personsName.blur(function () {                    
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();                    
                    if (!curOrd.IsOrderInvoiced()) {
                        th.Persons_Name = $(this).val();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.Persons_Name);
                    }
                });              

                var email = this.Container.find(publicClassName + ".email");
                email.blur(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        th.Persons_Email = $(this).val();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                       
                        $(this).val(curOrd.Persons_Email);
                    }
                });

                var phone = this.Container.find(publicClassName + ".phone");
                phone.blur(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        th.Persons_Phone = $(this).val();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.Persons_Phone);
                    }
                });

                var CellPhone = this.Container.find(publicClassName + ".cellphone");
                CellPhone.blur(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        th.Persons_CellPhone = $(this).val();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                                                
                        $(this).val(th.Persons_CellPhone);
                    }
                });

                var region = this.Container.find(publicClassName + ".region");
                region.change(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        $(this).parent().find('input:hidden').val($(this).val());
                        th.Region_Id = $(this).val();
                        var OrderId = $(this).data("orderid");
                        dhHelpdesk.CaseArticles.FillDepartments(OrderId);
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.Region_Id);
                    }

                });

                var department = this.Container.find(publicClassName + ".department");
                department.change(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        $(this).parent().find('input:hidden').val($(this).val());
                        th.Department_Id = $(this).val();
                        var OrderId = $(this).data("orderid");
                        $('#CostCentre_' + OrderId).val('');
                        dhHelpdesk.CaseArticles.FillOUs(OrderId);
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.Department_Id);
                    }
                });

                var ou = this.Container.find(publicClassName + ".ou");
                ou.change(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        $(this).parent().find('input:hidden').val($(this).val());
                        th.OU_Id = $(this).val();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.OU_Id);
                    }
                });

                var place = this.Container.find(publicClassName + ".place");
                place.blur(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        th.Place = $(this).val();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.Place);
                    }
                });

                var costcentre = this.Container.find(publicClassName + ".costcentre");
                costcentre.blur(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        th.CostCentre = $(this).val();                        
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.CostCentre);
                    }
                });
              
                var usercode = this.Container.find(publicClassName + ".usercode");
                usercode.blur(function () {
                    var curOrd = dhHelpdesk.CaseArticles.GetCurrentOrder();
                    if (!curOrd.IsOrderInvoiced()) {
                        th.UserCode = $(this).val();
                    }
                    else {
                        dhHelpdesk.CaseArticles.ShowAlreadyInvoicedMessage();                        
                        $(this).val(curOrd.UserCode);
                    }
                });
            },

            this.UpdateTotal = function () {
                this.Container.find(".articles-total").text(dhHelpdesk.CaseArticles.DoDelimit(this.GetArticlesTotal().Total.toString()));                
                if (this.Invoice != null) {
                    this.Invoice.UpdateTotal();
                }
            },

            this._deleteFromContainer = function (id) {
                this.Container.find("[data-id='" + id + "']").remove();
            },

            this.Validate = function () {
                var ret = { IsValid: true, Message: "" };
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    var articleValidation = article.Validate();
                    if (!articleValidation.IsValid) {
                        ret.IsValid = false;
                        ret.Message += "<br/>  - " + articleValidation.Message;
                    }
                }
                return ret;
            },

            this.InvoiceValidate = function () {
                var isValid = true;
                if (dhHelpdesk.Common.IsNullOrEmpty(this.CostCentre)) {
                    dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Kostnadställe saknas.") + " " + this.Caption);
                    isValid = false;
                }
                return isValid;
            },

            this.UpdateFiles = function () {
                var model = dhHelpdesk.CaseArticles.CaseFilesViewModel({
                    caseId: dhHelpdesk.CaseArticles.CaseFiles.getCaseId(),
                    files: this.files.getFiles(),
                    isInvoiced: this.IsOrderInvoiced()
                });
                this.Container.find("#files-container").html(dhHelpdesk.CaseArticles.CaseInvoiceOrderFilesTemplate.render(model));
            },

            this.RemoveOrderFile = function (fileName) {
                this.files.deleteFile(fileName);                
                this.UpdateFiles();
            },

            this.AddFileByName = function (fileName) {
                var alreadyAdded = this.files.getFiles();
                if (this.Container != null && alreadyAdded != null && alreadyAdded.length > 0) {                    
                    this.RemoveOrderFile(fileName);                    
                };

                var caseFile = dhHelpdesk.CaseArticles.CaseFiles.getFile(fileName);
                if (caseFile != null) {
                    this.files.addFile(caseFile.clone(this.IsInvoiced));
                }
            },

            this.SelectCaseFiles = function () {
                var model = dhHelpdesk.CaseArticles.CaseFilesViewModel({
                    caseId: dhHelpdesk.CaseArticles.CaseFiles.getCaseId(),
                    files: dhHelpdesk.CaseArticles.CaseFiles.getFiles()
                });

                var that = this;
                var d = $(dhHelpdesk.CaseArticles.CaseInvoiceCaseFilesTemplate.render(model));
                var allOpenedDialogs = $(document).find(".selectFileDialog");
                $.each(allOpenedDialogs, function () {
                    $(this).dialog("close");
                });

                if (model.getFiles().length > 0) {
                    d.dialog({
                        width: 450,
                        buttons: [
                            {
                                text: dhHelpdesk.Common.Translate("Bifoga"),
                                click: function () {
                                    d.find("input:checked").each(function () {
                                        that.AddFileByName($(this).val());
                                    });
                                    that.UpdateFiles();                                    
                                    d.dialog("close");
                                },
                                class: "btn"
                            },
                            {
                                text: dhHelpdesk.Common.Translate("Stäng"),
                                click: function () {
                                    d.dialog("close");
                                },
                                class: "btn"
                            }
                        ]
                    });
                    d.dialog("open");
                } else {
                    dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Det finns inga filer att lägga till") + ". " + dhHelpdesk.Common.Translate("Endast PDF-filer som är bifogade på ärendet går att bifoga."));
                }
            }
        },

        ShowCaseField: function (CaseFieldName) {
            var CaseFieldSettingsLength = CaseFieldSettings.Result.length;
            for (var i = 0; i < CaseFieldSettingsLength; i++) {
                if (CaseFieldSettings.Result[i].Name.toLowerCase() === CaseFieldName.toLowerCase()) {
                    if (CaseFieldSettings.Result[i].Show == 1) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            return false;
        },

        CaseInvoiceArticle: function () {
            this.Id = null;
            this.Order = null;
            this.Article = null;
            this.Name = null;
            this.Amount = null;
            this.Position = null;            
            this.Ppu = null;
            this.HasPpu = false;
            
            this.Container = null;
            this.CreditedFrom = null;

            this.ToJson = function () {
                return '{' +
                        '"Id":"' + (this.Id >= 0 ? this.Id : 0) + '", ' +
                        '"OrderId":"' + (this.Order.Id > 0 ? this.Order.Id : 0) + '", ' +
                        '"ArticleId":"' + (this.Article != null && this.Article.Id > 0 ? this.Article.Id : '') + '", ' +
                        '"Number":"' + this.GetNumber() + '", ' +
                        '"Name":"' + (this.Name != null ? this.Name : '') + '", ' +
                        '"Amount":"' + (this.Amount != null && !this.IsBlank() ? this.Amount : '') + '", ' +
                        '"Ppu":"' + (this.Ppu != null ? this.Ppu : '') + '", ' +
                        '"Position":"' + this.Position + '"' +                        
                    '}';
            };

            this.Clone = function () {
                var clone = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                clone.Id = this.Id;
                clone.Article = this.Article;
                clone.Name = this.Name;
                clone.Amount = this.Amount;
                clone.Position = this.Position;                
                clone.Ppu = this.Ppu;
                clone.HasPpu = this.HasPpu;
                clone.CreditedFrom = this.CreditedFrom;
                return clone;
            };

            this.GetCreditedArticle = function (newCreditedOrder, alreadyUsedAmount) {
                var article = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                article.Id = dhHelpdesk.Common.GenerateId();
                article.CreditedFrom = this;
                article.Article = this.Article;
                article.Name = this.Name;
                article.Amount = this.Amount;

                if (article.Article != null) {
                    usedAmounts = 0;
                    for (var i = 0; i < alreadyUsedAmount.length; i++) {
                        if (alreadyUsedAmount[i].Id == article.Article.Id)
                            usedAmounts = parseInt(alreadyUsedAmount[i].Amount);
                    }

                    if (newCreditedOrder.CreditForOrder_Id != null) {
                        var validatedAmount = dhHelpdesk.CaseArticles.ValidateNewCreditAmount(newCreditedOrder.CreditForOrder_Id, newCreditedOrder.Id, article.Article.Id, this.Amount, usedAmounts);
                        article.Amount = validatedAmount;
                    }
                }
                
                article.Position = this.Position;                
                article.Ppu = this.Ppu;
                article.HasPpu = this.HasPpu;
                
                return article;
            };

            this.IsNew = function () {
                return this.Id <= 0;
            };

            this.IsBlank = function () {
                return this.Article == null || this.Article.IsBlank();
            };

            this.GetTotal = function () {                
                if (this.Article != null) {                    
                    return (this.Amount * this.GetPpu()) ;
                }
                return 0;
            };

            this.GetUnitName = function () {
                if (this.Article != null &&
                    this.Article.Unit != null) {
                    return this.Article.Unit.Name;
                }
                return "";
            };

            this.GetNumber = function () {
                if (this.Article != null) {
                    return this.Article.Number;
                }
                return "";
            }

            this.GetPpu = function () {

                if (this.Ppu != null) {
                    return this.Ppu;
                }

                if (this.Article != null &&
                    this.Article.Ppu != null) {
                    return this.Article.Ppu;
                }

                return dhHelpdesk.CaseArticles.DefaultPpu;
            };

            this.GetViewModel = function () {
                var model = new dhHelpdesk.CaseArticles.CaseArticleViewModel();
                model.IsBlank = this.IsBlank();                
                model.Name = this.Name != null ? this.Name : "";
                model.NameEng = this.Article != null && this.Article.NameEng != null ? this.Article.NameEng : "";
                model.Description = this.Article != null && this.Article.Description != null ? this.Article.Description : "";
                model.Id = this.Id;
                model.Number = this.GetNumber();
                model.Amount = this.Amount;
                model.UnitName = this.GetUnitName();
                model.Ppu = this.GetPpu();
                model.Total = dhHelpdesk.CaseArticles.DoDelimit(this.GetTotal().toString());
                model.IsArticlePpuExists = this.IsArticlePpuExists();
                model.IsCredited = this.Order.CreditForOrder_Id != null;
                model.IsInvoiced = this.Order.IsInvoiced;
                model.DescriptionCount = dhHelpdesk.Common.Translate("Tecken kvar") + ": " + (dhHelpdesk.CaseArticles.MaxDescriptionCount - model.Name.length);
                return model;
            };

            this.IsArticlePpuExists = function () {
                return this.HasPpu;                
            },

            this.Initialize = function () {
                this.Container = $(dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate.render(this.GetViewModel()));
            };

            this.Update = function () {                
                if (this.IsBlank()) {
                    this.Name = this.Container.find(".article-name").val();
                    return;
                }

                var eAmount = this.Container.find(".article-amount");
                var amount = eAmount.val();
                if (!dhHelpdesk.Math.IsInteger(amount)) {
                    amount = dhHelpdesk.CaseArticles.DefaultAmount;
                    eAmount.val(amount);
                }
                this.Amount = amount;                
            
                var ePpu = this.Container.find(".article-ppu");                
                if (ePpu.length > 0) {
                    var ppu = ePpu.val();
                    if (!dhHelpdesk.Math.IsInteger(ppu) || ppu <= 0) {
                        ppu = dhHelpdesk.CaseArticles.DefaultPpu;
                        ePpu.val(ppu);
                    }
                    this.Ppu = ppu; 
                }               

                for (var i = 0; i < dhHelpdesk.CaseArticles.allVailableOrders.length; i++) {
                    var ord = dhHelpdesk.CaseArticles.allVailableOrders[i];
                    if (ord.Articles != undefined) {
                        for (var j = 0; j < ord.Articles.length; j++) {
                            if (this.Id == ord.Articles[j].Id) {
                                ord.Articles[j].Amount = parseInt(this.Amount);
                                ord.Articles[j].Ppu = parseInt(this.Ppu);
                            }
                        }
                    }
                    else
                        if (ord._articles != undefined) {
                            for (var j = 0; j < ord._articles.length; j++) {
                                if (this.Id == ord._articles[j].Id) {
                                    ord._articles[j].Amount = parseInt(this.Amount);
                                    ord._articles[j].Ppu = parseInt(this.Ppu);
                                }
                            }
                        }
                }

                var totalPrice = this.GetTotal().toString();
                this.Container.find(".article-total").text(dhHelpdesk.CaseArticles.DoDelimit(totalPrice));
                this.Order.UpdateTotal();                
            },

            this.Refresh = function () {
                var refreshed = $(dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate.render(this.GetViewModel()));
                this.Container.replaceWith(refreshed);
            },

            this.DoInvoice = function () {
                //this.IsInvoiced = true;
                this.CreditedFrom = null;
                this.Refresh();
            },

            this.Validate = function () {
                var ret = { IsValid: true, Message: "" };
                var isValid = true;
                if (!this.IsArticlePpuExists()) {
                    var ePpu = this.Container.find(".article-ppu");
                    if (ePpu.length > 0) {
                        var ppu = ePpu.val();
                        if (!dhHelpdesk.Math.IsInteger(ppu)) {
                            dhHelpdesk.Common.MakeInvalid(ePpu);
                            if (ret.Message != "")
                                ret.Message += " ,";
                            ret.Message += "PPE";
                            ret.IsValid = false;
                        }
                        else {
                            dhHelpdesk.Common.MakeValid(ePpu);
                        }                        
                    }
                }

                if (this.IsBlank()) {
                    var eName = this.Container.find(".article-name");
                    if (eName.length > 0) {
                        var name = eName.val();
                        if (dhHelpdesk.Common.IsNullOrEmpty(name)) {
                            dhHelpdesk.Common.MakeInvalid(eName);
                            if (ret.Message != "")
                                ret.Message += " ,";
                            if (eName.attr("Id").indexOf("Description_") == 0)
                                ret.Message += "Beskrivning";
                            else
                                ret.Message += "Namn";
                            ret.IsValid = false;
                        } else {
                            dhHelpdesk.Common.MakeValid(eName);
                        }
                    }
                }

                // Check if units number more then units of the credited article
                var eAmount = this.Container.find(".article-amount");
                if (eAmount.length > 0) {
                    var amount = eAmount.val();
                    if (!dhHelpdesk.Math.IsInteger(amount) ||
                        amount <= 0 ||
                        (this.CreditedFrom != null && amount > this.CreditedFrom.Amount)) {
                        dhHelpdesk.Common.MakeInvalid(eAmount);
                        if (ret.Message != "")
                            ret.Message += " ,";
                        ret.Message += "Enheter";
                        ret.IsValid = false;
                    } else {
                        dhHelpdesk.Common.MakeValid(eAmount);
                    }
                }
                
                //check if Textdemand has blank article?
                if (!(this.IsBlank()) && this.Article.TextDemand) {
                    var TextIsValid = false;
                    var thisArticle = this;
                    var thisPosition = thisArticle.Position + 1;
                    var allArticles = thisArticle.Order._articles;
                    $.each(allArticles, function () {
                        if (this.Position == thisPosition && this.IsBlank())
                        {
                            TextIsValid = true;
                            return false;
                        }
                    });
                    var eArticleRow = this.Container.find(".article-amount").parent().parent();
                    if (!TextIsValid) {
                        ret.IsValid = false;
                        dhHelpdesk.Common.MarkTextInvalid(eArticleRow);
                        if (ret.Message != "")
                            ret.Message += " ,";
                        ret.Message += "Textrad";                        
                    }
                    else {
                        dhHelpdesk.Common.MarkTextValid(eArticleRow);
                    }
                }
                return ret;
            }
        },

        DoDelimit: function(val)
        {
            var decimalDelimiter = ',';
            var thousandDelimiter = ' ';            
            x = val.split(this.decimalDelimiter);
            intVal = x[0];
            DecimalVal = x.length > 1 ? decimalDelimiter + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(intVal)) {
                intVal = intVal.replace(rgx, '$1' + thousandDelimiter + '$2');
            }
            return intVal + DecimalVal;            
        },
        
        InvoiceArticleUnit: function () {
            this.Id = null;
            this.Name = null;
            this.CustomerId = null;

            this.Clone = function () {
                var clone = new dhHelpdesk.CaseArticles.InvoiceArticleUnit();
                clone.Id = this.Id;
                clone.Name = this.Name;
                clone.CustomerId = this.CustomerId;
                return clone;
            };
        },

        CaseInvoiceViewModel: function () {
            this.Id = null;
            this.SelectItemHeader = dhHelpdesk.Common.Translate("Artikel");
            this.UnitsHeader = dhHelpdesk.Common.Translate("Enheter");
            this.UnitsPriceHeader = dhHelpdesk.Common.Translate("PPE SEK");
            this.DefaultAmount = dhHelpdesk.CaseArticles.DefaultAmount;
            this.AddButtonLabel = dhHelpdesk.Common.Translate("Lägg till");
            this.OrderTitle = dhHelpdesk.Common.Translate("Order");
            this.SummaryTitle = dhHelpdesk.Common.Translate("Översikt");
            this.TotalLabel = dhHelpdesk.Common.Translate("Total");            
            this.TotalAllLabel = dhHelpdesk.Common.Translate("Totalt alla ordrar");
            this.TotalInvoicedLabel = dhHelpdesk.Common.Translate("Totalt fakturerade ordrar");
            this.TotalNotInvoicedLabel = dhHelpdesk.Common.Translate("Totalt ej fakturerade ordrar");
            this.Total = null;
            this.TotalInvoiced = null;
            this.TotalNotInvoiced = null;
            this.Orders = [];

            this.AddOrder = function (order) {
                this.Orders.push(order);
            }
        },

        CaseInvoiceOrderViewModel: function () {
            this.Id = null;
            this.ArtNoHeader = dhHelpdesk.Common.Translate("Art nr");
            this.NameHeader = dhHelpdesk.Common.Translate("Artikelnamn SWE");
            this.NameEngHeader = dhHelpdesk.Common.Translate("Artikelnamn ENG");
            this.UnitsHeader = dhHelpdesk.Common.Translate("Enheter");
            this.UnitsPriceHeader = dhHelpdesk.Common.Translate("PPE SEK");
            this.TypeHeader = dhHelpdesk.Common.Translate("Typ");
            this.PpuHeader = dhHelpdesk.Common.Translate("PPE");
            this.TotalHeader = dhHelpdesk.Common.Translate("Total");
            this.TotalLabel = dhHelpdesk.Common.Translate("Total");
            this.TotalAllLabel = dhHelpdesk.Common.Translate("Totalt alla ordrar");
            this.ReferenceTitle = dhHelpdesk.Common.Translate("Orderreferens");
            this.InvoicedByTitle = dhHelpdesk.Common.Translate("Fakturerad av");

            //LABELS FOR initiator data
            this.ReportedByTitle = dhHelpdesk.Common.TranslateCaseFields("ReportedBy");
            this.Persons_NameTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_Name");
            this.Persons_EmailTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_Email");
            this.Persons_PhoneTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_Phone");
            this.Persons_CellPhoneTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_CellPhone");
            this.Region_IdTitle = dhHelpdesk.Common.TranslateCaseFields("Region_Id");
            this.Department_IdTitle = dhHelpdesk.Common.TranslateCaseFields("Department_Id");
            this.OU_IdTitle = dhHelpdesk.Common.TranslateCaseFields("OU_Id");
            this.PlaceTitle = dhHelpdesk.Common.TranslateCaseFields("Place");
            this.UserCodeTitle = dhHelpdesk.Common.TranslateCaseFields("UserCode");
            this.CostCentreTitle = dhHelpdesk.Common.TranslateCaseFields("CostCentre");

            this.Total = null;            
            this.Articles = [];
            this.InvoiceDate = "";
            this.InvoiceTime = "";
            this.InvoicedByUser = "";
            this.InvoicedByUserId = "";
            this.IsInvoiced = false;
            this.TitleCaption = "";

            this.ShowReportedBy = false;
            this.ReportedBy = "";

            this.Persons_Name = "";
            this.ShowPersons_Name = false;

            this.Persons_Email = "";
            this.ShowPersons_Email = false;

            this.Persons_Phone = "";
            this.ShowPersons_phone = false;

            this.Persons_CellPhone = "";
            this.ShowPersons_CellPhone = false;

            this.Region_Id = "";
            this.ShowRegion_Id = false;

            this.Department_Id = "";
            this.ShowDepartment_Id = false;

            this.OU_Id = "";
            this.ShowOU_Id = false;

            this.Place = "";
            this.ShowPlace = false;

            this.UserCode = "";
            this.ShowUserCode = false;

            this.CostCentre = "";
            this.ShowCostCentre = false;
            
            this.CreditForOrder_Id = null;
            this.Project_Id = null;

            this.CreditOrderEnabled = false;
            this.CreditOrderTitle = "";

            this.CreditOrderId = "";

            this.attachedFilesTitle = dhHelpdesk.Common.Translate("Bifogade Filer");
            this.attachFilesTitle = dhHelpdesk.Common.Translate("Lägg till");
            this.doInvoiceTitle = dhHelpdesk.Common.Translate("Fakturera");
            this.doCreditTitle = dhHelpdesk.Common.Translate("Kreditera");
            this.SavingMessage = dhHelpdesk.Common.Translate("Saving...");            

            this.AddArticle = function (article) {
                this.Articles.push(article);
            }
        },

        CaseArticleViewModel: function () {
            this.IsBlank = null;            
            this.Id = null;
            this.Name = null;
            this.NameEng = null;
            this.Description = null;
            this.Number = null;
            this.Amount = null;
            this.UnitName = null;
            this.Ppu = null;
            this.Total = null;
            this.IsArticlePpuExists = null;
            this.IsCredited = false;
            this.IsInvoiced = false;
            this.DescriptionCount = '';
        },
        
        OrderActionsViewModel: function (spec) {
            var that = {};

            var creditOrderEnabled = spec.creditOrderEnabled || false;
            var creditOrderTitle = spec.creditOrderTitle || dhHelpdesk.Common.Translate("Kreditera order");
            var creditOrderId = spec.creditOrderId || 0;

            var get_creditOrderEnabled = function () {
                return creditOrderEnabled;
            }

            var get_creditOrderTitle = function () {
                return creditOrderTitle;
            }

            var get_creditOrderId = function () {
                return creditOrderId;
            }

            that.get_creditOrderEnabled = get_creditOrderEnabled;
            that.get_creditOrderTitle = get_creditOrderTitle;
            that.get_creditOrderId = get_creditOrderId;

            return that;
        },

        CaseFilesViewModel: function (spec, my) {
            var that = {};
            my = my || {};

            var caseId = spec.caseId || '';
            var files = spec.files || [];
            var title = spec.title || dhHelpdesk.Common.Translate("Ärendefiler");

            var getCaseId = function () {
                return caseId;
            };

            var getFiles = function () {
                return files;
            };

            var getTitle = function () {
                return title;
            };            
                        
            that.getCaseId = getCaseId;
            that.getFiles = getFiles;
            that.getTitle = getTitle;
            
            return that;
        },        

        OrderFilesCollection: function (spec, my) {
            var that = {};
            my = my || {};

            var files = spec.files || [];

            var getFiles = function () {                
                return files;
            };

            var getFile = function (fileName) {
                var fs = getFiles();
                for (var i = 0; i < fs.length; i++) {
                    var f = fs[i];
                    if (f.getFileName() == fileName) {
                        return f;
                    }
                }

                return null;
            };

            var addFile = function (file) {
                if (file == null) {
                    return;
                }
                getFiles().push(file);
            };

            var deleteFile = function (fileName) {
                var fs = getFiles();
                var found = false;
                do {
                    found = false;
                    for (var i = 0; i < fs.length; i++) {
                        if (fs[i].getEncodedFileName() == fileName || fs[i].getFileName() == fileName) {
                            fs.splice(i, 1);
                            found = true;
                            break;
                        }
                    }
                } while (found);
            };

            var clone = function (isInvoiced) {
                var c = dhHelpdesk.CaseArticles.OrderFilesCollection({});
                var fs = getFiles();
                for (var i = 0; i < fs.length; i++) {
                    c.addFile(fs[i].clone(isInvoiced));
                }

                return c;
            };

            that.getFiles = getFiles;
            that.getFile = getFile;
            that.addFile = addFile;
            that.deleteFile = deleteFile;
            that.clone = clone;

            return that;
        },

        CaseFile: function (spec, my) {
            var that = {};
            my = my || {};

            var fileName = spec.fileName || '';
            var category = spec.category || '';
            var size = spec.size || 0;
            var type = spec.type || '';            
            var createdDate = spec.createdDate || '';
            var user = spec.user || '';
            var isOrderInvoiced = spec.isOrderInvoiced || false;

            var getFileCategory = function () {
                return category;
            };

            var getFileName = function () {
                return fileName;
            };

            var getSize = function () {
                return size;
            };

            var getType = function () {
                return type;
            };

            var getCreatedDate = function () {
                return createdDate;
            };

            var getUser = function () {
                return user;
            };

            var getEncodedFileName = function () {
                return encodeURIComponent(getFileName());
            };

            var getIsOrderInvoiced = function () {
                return isOrderInvoiced;
            };

            var clone = function (isOrderInvoiced) {                
                var f = dhHelpdesk.CaseArticles.CaseFile({
                    fileName: getFileName(),
                    size: getSize(),
                    type: getType(),
                    category: getFileCategory(),
                    createdDate: getCreatedDate(),
                    user: getUser(),
                    isOrderInvoiced: (isOrderInvoiced != undefined? isOrderInvoiced : false)   
                });                
                return f;
            };

            var toJson = function () {
                return '{' +
                            '"FileName":"' + getEncodedFileName() + '"' +
                        '}';
            };
            
            that.getFileName = getFileName;
            that.getSize = getSize;
            that.getType = getType;
            that.getFileCategory = getFileCategory;
            that.getCreatedDate = getCreatedDate;
            that.getUser = getUser;
            that.getEncodedFileName = getEncodedFileName;
            that.getIsOrderInvoiced = getIsOrderInvoiced;
            that.clone = clone;
            that.toJson = toJson;

            return that;
        },

        CaseFilesCollection: function (spec, my) {
            var that = {};
            my = my || {};

            var maxFileSizeMb = spec.maxFileSizeMb || 10;
            var allowedFileTypes = spec.allowedFileTypes || ["application/pdf"];
            var caseId = spec.caseId || '';
            var files = spec.files || [];

            var getMaxFileSizeMb = function () {
                return maxFileSizeMb;
            };

            var getAllowedFileTypes = function () {
                return allowedFileTypes;
            };

            var getCaseId = function () {
                return caseId;
            };

            var getFiles = function () {            
                return files;
            };            

            var getFile = function (fileName) { //this has a problem if there is a weird filename, example "Ä(4).pdf" todo: fix this
                var fs = getFiles();
                for (var i = 0; i < fs.length; i++) {
                    var f = fs[i];                    
                    if (f.getEncodedFileName() == fileName || f.getFileName() == fileName) {
                        return f;
                    }
                }

                return null;
            };

            var addFile = function (file) {
                getFiles().push(file);
            };

            var deleteFile = function (fileName) {
                var fs = getFiles();
                for (var i = 0; i < fs.length; i++) {
                    if (fs[i].getFileName() == fileName) {
                        fs.splice(i, 1);
                        break;
                    }
                }
            };

            var isFileValid = function (file) {
                if (file.getSize() > getMaxFileSizeMb() * 1024 * 1024) {
                    return false;
                }

                var isAllowedType = false;
                var allowedTypes = getAllowedFileTypes();
                for (var i = 0; i < allowedTypes.length; i++) {
                    if (allowedTypes[i] == file.getType()) {
                        isAllowedType = true;
                        break;
                    }
                }
                if (!isAllowedType) {
                    return false;
                }

                return true;
            };

            that.getMaxFileSizeMb = getMaxFileSizeMb;
            that.getAllowedFileTypes = getAllowedFileTypes;
            that.getCaseId = getCaseId;
            that.getFiles = getFiles;            
            that.getFile = getFile;
            that.addFile = addFile;
            that.deleteFile = deleteFile;
            that.isFileValid = isFileValid;

            return that;
        },

        CaseFiles: null
    }

    dhHelpdesk.System.OnEvent("OnUploadCaseFile", function (e, uploader, files) {
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var f = dhHelpdesk.CaseArticles.CaseFile({
                fileName: file.name,
                size: file.size,
                type: file.type,                
                category: "Case"
            });

            if (dhHelpdesk.CaseArticles.CaseFiles.isFileValid(f)) {
                dhHelpdesk.CaseArticles.CaseFiles.addFile(f);
            }
        }
    });

    dhHelpdesk.System.OnEvent("OnUploadLogFile", function (e, uploader, files) {        
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var f = dhHelpdesk.CaseArticles.CaseFile({
                fileName: file.name,
                size: file.size,
                type: file.type,
                category: "Log"
            });

            if (dhHelpdesk.CaseArticles.CaseFiles.isFileValid(f)) {
                dhHelpdesk.CaseArticles.CaseFiles.addFile(f);
            }
        }
    });

    dhHelpdesk.System.OnEvent("OnDeleteCaseFile", function (e, caseId, fileName) {
        var f = fileName.trim();
        dhHelpdesk.CaseArticles.CaseFiles.deleteFile(f);

        var orders = dhHelpdesk.CaseArticles.GetInvoice().GetOrders();
        for (var i = 0; i < orders.length; i++) {
            orders[i].files.deleteFile(f);
            orders[i].UpdateFiles();
        }
    });

    var loadCaseFiles = function () {        
        dhHelpdesk.CaseArticles.CaseKey = $(document).find("[data-invoice]").attr("data-invoice-case-key");
        dhHelpdesk.CaseArticles.LogKey = $(document).find("[data-invoice]").attr("data-invoice-log-key");
        dhHelpdesk.CaseArticles.CaseFiles = dhHelpdesk.CaseArticles.CaseFilesCollection({
            caseId: dhHelpdesk.CaseArticles.CaseKey
        });

        return $.getJSON("/invoice/casefiles?id=" + dhHelpdesk.CaseArticles.CaseKey +
                                        "&logKey=" + dhHelpdesk.CaseArticles.LogKey, function (data) {
            for (var i = 0; i < data.length; i++) {
                var file = data[i];
                var f = dhHelpdesk.CaseArticles.CaseFile({
                    fileName: file.FileName,
                    size: file.Size,
                    type: file.Type,
                    category: file.Category == 'L'? "Log" : "Case"
                });
                if (dhHelpdesk.CaseArticles.CaseFiles.isFileValid(f)) {
                    dhHelpdesk.CaseArticles.CaseFiles.addFile(f);
                }
            }
        });
    };

    var loadTranslationList = function () {
        $.getJSON("/Translation/CurrentLanguageId", function (data) {
            CurrentLanguageId = data;
        });

        return $.getJSON("/Translation/GetAllCoreTextTranslations", function (data) {
            AllTranslations = data;
        });
    };

    var loadCaseFieldTranslations = function () {
        return $.getJSON("/Translation/GetCaseFieldsForTranslation", function (data) {
            AllCaseFields = data;
        });
    };

    var loadCaseFieldSettings = function () {
        return $.getJSON("/Cases/GetCaseFields", function (data) {
            CaseFieldSettings = data;
        });
    };

    var loadCaseInvoiceTemplate = function () {
        return $.get("/content/templates/case-invoice.tmpl.html", function (caseInvoiceTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceTemplate = $.templates("caseInvoice", caseInvoiceTemplate);
        });
    };

    var loadCaseInvoiceOrderTemplate = function () {
        return $.get("/content/templates/case-invoice-order.tmpl.html", function (caseInvoiceOrderTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceOrderTemplate = $.templates("caseInvoiceOrder", caseInvoiceOrderTemplate);
        });
    };

    var loadCaseInvoiceArticleTemplate = function () {
        return $.get("/content/templates/case-invoice-article.tmpl.html", function (caseInvoiceArticleTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate = $.templates("caseInvoiceArticle", caseInvoiceArticleTemplate);
        });
    };

    var loadCaseInvoiceOverviewTemplate = function () {
        return $.get("/content/templates/case-invoice-overview.tmpl.html", function (caseInvoiceOverviewTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceOverviewTemplate = $.templates("caseInvoiceOverview", caseInvoiceOverviewTemplate);
        });
    };

    var loadCaseInvoiceArticleOverviewTemplate = function () {
        return $.get("/content/templates/case-invoice-article-overview.tmpl.html", function (caseInvoiceArticleOverviewTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceArticleOverviewTemplate = $.templates("caseInvoiceArticleOverview", caseInvoiceArticleOverviewTemplate);
        });
    };

    var loadCaseInvoiceOrderActionsTemplate = function () {
        return $.get("/content/templates/case-invoice-order-actions.tmpl.html", function (caseInvoiceOrderActionsTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceOrderActionsTemplate = $.templates("caseInvoiceOrderActions", caseInvoiceOrderActionsTemplate);
        });
    };

    var loadCaseInvoiceCaseFilesTemplate = function () {
        return $.get("/content/templates/case-invoice-case-files.tmpl.html", function (caseInvoiceCaseFilesTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceCaseFilesTemplate = $.templates("CaseInvoiceCaseFiles", caseInvoiceCaseFilesTemplate);
        });
    };

    var loadCaseInvoiceOrderFilesTemplate = function () {
        return $.get("/content/templates/case-invoice-order-files.tmpl.html", function (caseInvoiceOrderFilesTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceOrderFilesTemplate = $.templates("CaseInvoiceOrderFiles", caseInvoiceOrderFilesTemplate);
        });
    };

    var loadAllData = function (callBack, obj) {
        loadTranslationList()
            .then(loadCaseFieldTranslations)
            .then(loadCaseFieldSettings)
            .then(loadCaseInvoiceTemplate)
            .then(loadCaseInvoiceOrderTemplate)
            .then(loadCaseInvoiceArticleTemplate)
            .then(loadCaseInvoiceOverviewTemplate)
            .then(loadCaseInvoiceArticleOverviewTemplate)
            .then(loadCaseInvoiceOrderActionsTemplate)
            .then(loadCaseInvoiceCaseFilesTemplate)
            .then(loadCaseInvoiceOrderFilesTemplate)
            .then(loadCaseFiles)
            .then(function () {                
                $("[data-invoice]").each(function () {
                    var $this = $(this);
                    var data = $.parseJSON($this.attr("data-invoice-case-articles"));
                    if (data == null || data.Invoices.length == 0) {
                        var blankInvoice = new dhHelpdesk.CaseArticles.CaseInvoice();
                        blankInvoice.Id = dhHelpdesk.Common.GenerateId();
                        blankInvoice.Initialize();
                        dhHelpdesk.CaseArticles.AddInvoice(blankInvoice);
                        dhHelpdesk.CaseArticles.Initialize($this);
                        blankInvoice.CaseId = dhHelpdesk.CaseArticles.CaseId;
                        var blankOrder = new dhHelpdesk.CaseArticles.CreateBlankOrder();
                        blankInvoice.AddOrder(blankOrder);
                        dhHelpdesk.CaseArticles.ApplyChanges();
                        return;
                    }

                    var inv = data.Invoices[0];
                    var invoice = new dhHelpdesk.CaseArticles.CaseInvoice();
                    invoice.Id = inv.Id;
                    invoice.CaseId = inv.CaseId;
                    invoice.Initialize();
                    dhHelpdesk.CaseArticles._invoices = [];
                    dhHelpdesk.CaseArticles.AddInvoice(invoice);
                    dhHelpdesk.CaseArticles.Initialize($this);
                    if (inv.Orders != null) {                        
                        inv.Orders = dhHelpdesk.CaseArticles.SortOrders(inv.Orders);
                        dhHelpdesk.CaseArticles.allVailableOrders = inv.Orders;
                        for (var j = 0; j < inv.Orders.length; j++) {
                            var ord = inv.Orders[j];
                            var order = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                            order.Id = ord.Id;
                            order.Number = ord.Number;                            
                            order.InvoiceDate = ord.InvoiceDate;
                            order.InvoicedByUser = ord.InvoicedByUser;
                            order.InvoicedByUserId = ord.InvoicedByUserId;                                                                                    
                            order.Caption = ord.Caption;
                            if ((ord.InvoicedByUserId != undefined && ord.InvoicedByUserId != null && ord.InvoicedByUserId > 0) ||
                                (ord.InvoiceDate != undefined && ord.InvoiceDate != null))
                                order.IsInvoiced = true;
                            else
                                order.IsInvoiced = false;

                            /////////////////////////
                            ///////Initiator labels
                            order.ReportedBy = ord.ReportedBy;
                            order.Persons_Name = ord.Persons_Name;
                            order.Persons_Email = ord.Persons_Email;
                            order.Persons_Phone = ord.Persons_Phone;
                            order.Persons_CellPhone = ord.Persons_Cellphone;
                            order.Region_Id = ord.Region_Id;
                            order.Department_Id = ord.Department_Id;
                            order.OU_Id = ord.OU_Id;
                            order.Place = ord.Place;
                            order.UserCode = ord.UserCode;
                            order.CostCentre = ord.CostCentre;
                            order.CreditForOrder_Id = ord.CreditForOrder_Id;
                            order.Project_Id = ord.Project_Id;


                            order.Date = ord.Date;

                            if (ord.Files != null) {
                                for (var m = 0; m < ord.Files.length; m++) {
                                    order.AddFileByName(ord.Files[m].FileName);
                                }
                            }

                            invoice.AddOrder(order);
                            if (ord.Articles != null) {
                                for (var k = 0; k < ord.Articles.length; k++) {
                                    var article = ord.Articles[k];
                                    var caseArticle = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                                    caseArticle.Id = article.Id;
                                    if (article.Article != null) {
                                        caseArticle.Article = new dhHelpdesk.CaseArticles.InvoiceArticle();
                                        caseArticle.Article.Id = article.Article.Id;
                                        caseArticle.Article.ParentId = article.Article.ParentId;
                                        caseArticle.Article.Number = article.Article.Number;
                                        caseArticle.Article.Name = article.Article.Name;
                                        caseArticle.Article.NameEng = article.Article.NameEng;
                                        caseArticle.Article.Description = article.Article.Description;
                                        caseArticle.Article.TextDemand = article.Article.TextDemand;
                                        caseArticle.Article.UnitId = article.Article.UnitId;
                                        if (article.Article.Unit != null) {
                                            var unit = new dhHelpdesk.CaseArticles.InvoiceArticleUnit();
                                            unit.Id = article.Article.Unit.Id;
                                            unit.Name = article.Article.Unit.Name;
                                            unit.CustomerId = article.Article.Unit.CustomerId;
                                            caseArticle.Article.Unit = unit;
                                        }
                                        caseArticle.Article.Ppu = article.Article.Ppu;
                                        caseArticle.Article.ProductAreaId = article.Article.ProductAreaId;
                                        caseArticle.Article.CustomerId = article.Article.CustomerId;
                                    }
                                    caseArticle.Name = article.Name;
                                    caseArticle.Amount = article.Amount;
                                    caseArticle.Ppu = article.Ppu;
                                    caseArticle.UnitId = article.UnitId;
                                    caseArticle.Position = article.Position;                                    
                                    order.AddArticle(caseArticle);
                                }
                            }
                        }
                    }
                 
                    dhHelpdesk.CaseArticles.ApplyChanges();
                    if (callBack != undefined && callBack != null) {                        
                        callBack(obj);
                    }
                    dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);                    

                });
            });
    };

    loadAllData();
});


