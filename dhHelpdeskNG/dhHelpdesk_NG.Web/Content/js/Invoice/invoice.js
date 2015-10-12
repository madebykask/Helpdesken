
if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

    //used for translating casefields
    var AllCaseFields = [];

    var AllTranslations = [];

    var CaseFieldSettings = [];

    dhHelpdesk.CaseArticles = {
        DefaultAmount: 1,

        DefaultPpu: 0,

        DateFormat: null,

        _invoices: [],

        _invoicesBackup: [],

        _container: null,

        _invoiceArticles: [],

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

        CustomerId: null,

        OrderActionsInstance: null,

        translate: function (text) {
            var AllTranslationsLength = AllTranslations.length;
            for (var i = 0; i < AllTranslationsLength; i++) {
                if (AllTranslations[i].TextToTranslate.toLowerCase() === text.toLowerCase())
                {
                    var AllTranslationsTranslationsLength = AllTranslations[i].Translations.length;
                    for (var j = 0; j < AllTranslationsTranslationsLength; j++) {
                        if (AllTranslations[i].Translations[j].Language_Id == 1)
                        {
                            return AllTranslations[i].Translations[j].TranslationName;
                        }
                    }
                }
            }

            return text;
        },
        translateCaseFields: function(text)
        {
            var AllCaseFieldsLength = AllCaseFields.length;
            for (var i = 0; i < AllCaseFieldsLength; i++) {
                if (AllCaseFields[i].Name.toLowerCase() == text.toLowerCase()) {
                    return AllCaseFields[i].Label;
                }
            }
            return text;
        },

        RaiseEvent: function(eventType, extraParameters) {
            $(document).trigger(eventType, extraParameters);
        },

        OnEvent: function(event, handler) {
            $(document).on(event, handler);
        },

        MakeInvalid: function(e) {
            e.css("border-color", "red");
        },

        MakeValid: function(e) {
            e.css("border-color", "");
        },

        IsNullOrEmpty: function(value) {
            return !(typeof value === "string" && value.length > 0);
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

        IsNewCase: function() {
            return !(this.CaseId > 0);
        },

        AddInvoice: function(invoice) {
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

        HasNotInvoicedArticles: function() {
            var invoices = this.GetInvoices();
            for (var i = 0; i < invoices.length; i++) {
                var invoice = invoices[i];
                if (invoice.HasNotInvoicedArticles()) {
                    return true;
                }
            }

            return false;
        },

        HasInvoicedArticles: function() {
            var invoices = this.GetInvoices();
            for (var i = 0; i < invoices.length; i++) {
                var invoice = invoices[i]
                if (invoice.HasInvoicedArticles()) {
                    return true;
                }
            }
            return false;
        },

        GetOrder: function(id) {
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
                var DepartmentFilterFormat = 0; //majid didn't know what this was. Doesn't seem to be working.
                var RegionId = $('#RegionSelect-Order' + OrderId).parent().find('input:hidden').val()
                var CustomerId = $('#case__Customer_Id').val();
            $.post(publicChangeRegion, { 'id': RegionId, 'customerId': CustomerId, 'departmentFilterFormat': DepartmentFilterFormat }, function (data) {
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
            //}
        },

        FillOUs: function (OrderId) {
                var DepartmentFilterFormat = 0;
                var DepartmentId = $('#DepartmentId_' + OrderId).val();
                var CustomerId = $('#case__Customer_Id').val();
                $.post(publicChangeDepartment, { 'id': DepartmentId, 'CustomerId': CustomerId, 'departmentFilterFormat': DepartmentFilterFormat}, function (data) {
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
        
        PopulateSelectBoxesFromSelect: function(ClassSelector, IdSelectorForData)
        {
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

        GetArticle: function(id) {
            var invoices = this.GetInvoices();
            for (var i = 0; i < invoices.length; i++) {
                var article = invoices[i].GetArticle(id);
                if (article != null) {
                    return article;
                }
            }
            return null;
        },

        ClearInvoices: function() {
            this._invoices = [];
        },

        SaveInvoices: function () {
            this.CaseInvoicesElement.val(this.ToJson());
        },

        GetSavedInvoices: function() {
            return this.CaseInvoicesElement.val();
        },

        ApplyChanges: function() {
            this._invoicesBackup = [];
            for (var i = 0; i < this._invoices.length; i++) {
                this._invoicesBackup.push(this._invoices[i].Clone());
            }
            this.SaveInvoices();
        },

        Restore: function() {
            this._invoices = [];
            for (var i = 0; i < this._invoicesBackup.length; i++) {
                this._invoices.push(this._invoicesBackup[i].Clone());
            }

            
        },

        CancelChanges: function () {
            this.Restore();
        },

        InvoiceArticles: function() {
            var th = this;

            $.post("/Invoice/DoInvoice", {
                customerId: this.CustomerId,
                caseId: this.CaseId,
                invoices: this.ToJson()
            },
            function () {
                th._invoicesBackup = [];
                for (var i = 0; i < th._invoices.length; i++) {
                    var invoice = th._invoices[i];
                    invoice.DoInvoice();
                    th._invoicesBackup.push(invoice.Clone());
                }
                th.SaveInvoices();
                th.ShowMessage(dhHelpdesk.CaseArticles.translate('Order fakturerad.'));
            })
            .fail(function(result) {
                th.ShowErrorMessage(result.statusText);
            });
        },

        ToJson: function () {
            return this.GetInvoice().ToJson();
        },

        ShowSuccessMessage: function(message) {
            this.ShowMessage(message, 'success');
        },

        ShowErrorMessage: function(message) {
            this.ShowMessage(message, 'error');
        },
        ShowAlreadyInvoicedMessage: function()
        {
            this.ShowErrorMessage("Den här ordern är redan fakturerad. Du kan inte utföra den här aktiviteten.");
        },

        ShowMessage: function(message, type) {
            $().toastmessage('showToast', {
                text: message,
                sticky: false,
                position: 'top-center',
                type: type,
                closeText: '',
                stayTime: 3000,
                inEffectDuration: 1000,
                close: function () {
                }
            });
        },

        CreateBlankOrder: function() {
            var blankOrder = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
            blankOrder.Id = dhHelpdesk.CaseArticles.GenerateId();
            blankOrder.ReportedBy = $('#case__ReportedBy').val();
            blankOrder.Persons_Name = $('#case__PersonsName').val();
            blankOrder.Persons_Phone = $('#case__PersonsPhone').val();
            blankOrder.Persons_Cellphone = $('#case__PersonsCellphone').val();
            blankOrder.Region_Id = $('#case__Region_Id').val();
            blankOrder.Department_Id = $('#case__Department_Id').val();
            blankOrder.OU_Id = $('#case__Ou_Id').val();
            blankOrder.Place = $('#case__Place').val();
            blankOrder.UserCode = $('#case__UserCode').val();
            blankOrder.CostCentre = $('#case__CostCentre').val();
            blankOrder.IsInvoiced = false;

            return blankOrder;
        },

        AddOrder: function(invoiceId) {
            var invoice = this.GetInvoice(invoiceId);
            if (invoice != null) {
                invoice.AddOrder(this.CreateBlankOrder());
            }
        },

        DeleteCurrentOrder: function(invoiceId) {
            var invoice = this.GetInvoice(invoiceId);
            if (invoice != null) {
                var currentOrder = this.GetCurrentOrder();
                if (currentOrder != null) {
                    invoice.DeleteOrder(currentOrder.Id);
                }
            }
        },

        CreateBlankArticle: function() {
            var blank = new dhHelpdesk.CaseArticles.InvoiceArticle();
            blank.Id = dhHelpdesk.CaseArticles.GenerateId();
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
                }
                else {
                    this.ShowAlreadyInvoicedMessage();
                }
                
            }            
        },

        RemoveOrderFile: function (fileName) {
            var order = this.GetCurrentOrder();
            if (order != null) {                
                order.RemoveOrderFile(fileName);
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

        OtherReference: function(orderId){
            var order = this.GetOrder(orderId);
            if (order != null)
            {
                $('.InitiatorFieldsOrder' + orderId).toggle();

            }
        },

        UpdateArticle: function(e) {
            var id = e.attr("data-id");
            var article = this.GetArticle(id);
            if (article != null) {
                article.Update();
            }
        },

        DeleteArticle: function(e) {
            var id = e.attr("data-id");
            var article = this.GetArticle(id);
            if (article != null) {
                article.Order.DeleteArticle(id);
            }
        },

        GetCurrentOrder: function() {
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
                if (currentOrder.InvoicedByUser == null || currentOrder.InvoicedByUser == "") {
                    return false;
                }
                else {
                    OrderIsInvoiced = true;
                    return true;
                }

                //Comented out because we had only invoiced status on articles. This was old and it is changed
                //var ArticlesInCurrentOrder = currentOrder.GetArticles();
                //$.each(ArticlesInCurrentOrder, function (i, article) {
                //    if (article.IsInvoiced) {
                //        OrderIsInvoiced = true;
                //        return false;
                //    }
                //});
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
                                            , name: item.SurName + ' ' + item.FirstName
                                            , email: item.Email
                                            , place: item.Location
                                            , phone: item.Phone
                                            , usercode: item.UserCode
                                            , cellphone: item.CellPhone
                                            , regionid: item.Region_Id
                                            , departmentid: item.Department_Id
                                            , ouid: item.OU_Id
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
                        || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.email.toLowerCase().indexOf(this.query.toLowerCase());
                },

                sorter: function (items) {
                    var beginswith = [], caseSensitive = [], caseInsensitive = [], aItem;
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
                    var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                    var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email;

                    return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });
                },

                updater: function (obj) {
                    var item = JSON.parse(obj);
                    var OrderId = this.$element.data("orderid")

                    $('#Persons_Name_' + OrderId).val(item.name);
                    $('#Persons_Phone_' + OrderId).val(item.phone);
                    $('#Persons_CellPhone_' + OrderId).val(item.cellphone);

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

                    //email

                    return item.num;
                }
            };
            return options;
        },

        // Class for working with order actions
        orderActions: function(spec, my) {
            var that = {};
            my = my || {};
            
            var actionsContainer = spec.actionsContainer || null;
            var creditOrderButton = null;

            var get_actionsContainer = function() {
                return actionsContainer;
            };

            var get_creditOrderButton = function () {
                if (creditOrderButton == null) {
                    creditOrderButton = get_actionsContainer().find("#actions-credit-order");
                }
                return creditOrderButton;
            };

            that.get_actionsContainer = get_actionsContainer;
            that.get_creditOrderButton = get_creditOrderButton;

            return that;
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
                title: dhHelpdesk.CaseArticles.translate("Artiklar att fakturera"),
                width: 1000,
                height: 800,
                close: function () {
                    th._container.dialog("destroy");
                },
                buttons: [
                    {
                        text: dhHelpdesk.CaseArticles.translate("Spara"),
                        click: function () {
                            if (!th.Validate()) {
                                return;
                            }
                            th.ApplyChanges();
                        }
                    },
                    {
                        text: dhHelpdesk.CaseArticles.translate("Stäng"),
                        click: function () {
                            th.CancelChanges();
                            th._container.dialog("close");
                            $('.case-invoice-container').remove();
                        }
                    }
                ],
                autoOpen: false
            });

            th._container.parent().addClass("overflow-visible");
            dhHelpdesk.CaseArticles.OrderActionsInstance = dhHelpdesk.CaseArticles.orderActions({ actionsContainer: th._container.parent().find(".ui-dialog-buttonset") });

            if (!th.IsNewCase()) {
                var invoiceBtn = $("<button>" + dhHelpdesk.CaseArticles.translate("Fakturera") + "</button>");
                if (markInvoiceButton) {
                    invoiceBtn.css("color", "red");
                }
                invoiceBtn.click(function () {

                    if (!th.Validate()) {
                        return;
                    }

                    th.InvoiceArticles();
                    th._container.dialog("close");
                });
                dhHelpdesk.CaseArticles.OrderActionsInstance.get_actionsContainer().prepend(invoiceBtn);
            }

            // set order actions 
            var orderActionsViewModel = dhHelpdesk.CaseArticles.orderActionsViewModel({
                creditOrderEnabled: th.GetCurrentOrder().HasInvoicedArticles(),
                creditOrderTitle: dhHelpdesk.CaseArticles.translate("Kreditera order")
            });
            dhHelpdesk.CaseArticles.OrderActionsInstance.get_actionsContainer().prepend($(dhHelpdesk.CaseArticles.CaseInvoiceOrderActionsTemplate.render(orderActionsViewModel)));
            dhHelpdesk.CaseArticles.OrderActionsInstance.get_creditOrderButton().click(function () {
                th.GetInvoice().AddOrder(th.GetCurrentOrder().GetCreditedOrder());
            });

            var addEl = th._container.find(".articles-params-add");
            addEl
                .unbind('click')
                .click(function () {                   
                    if (!th.GetInvoiceStatusOfCurrentOrder()) {
                        var unitsEl = th._container.find(".articles-params-units");
                        var units = unitsEl.val();
                        if (!th.IsInteger(units) || units <= 0) {
                            units = dhHelpdesk.CaseArticles.DefaultAmount;
                            unitsEl.val(units);
                        }
                        var articlesEl = th._container.find(".articles-params-article");
                        var articleId = articlesEl.val();
                        var article = th.GetInvoiceArticle(articleId);
                        if (article != null) {
                            var currentOrder = th.GetCurrentOrder();
                            if (article.HasChildren()) {
                                for (var i = 0; i < article.Children.length; i++) {
                                    var child = article.Children[i].ToCaseArticle();
                                    child.Amount = units;
                                    currentOrder.AddArticle(child);
                                }
                            } else {
                                var caseArticle = article.ToCaseArticle();
                                caseArticle.Amount = units;
                                currentOrder.AddArticle(caseArticle);
                            }
                            // Reset number of units to default
                            unitsEl.val(dhHelpdesk.CaseArticles.DefaultAmount);
                        }
                    }
                    else {
                        th.ShowAlreadyInvoicedMessage();
                    }
            });
        },

        OpenInvoiceWindow: function (message) {

            var th = dhHelpdesk.CaseArticles;
            th.CreateContainer(message);
            var addArticleEl = th._container.find(".articles-params");
            var articlesSelectContainer = addArticleEl.find(".articles-select-container");
            articlesSelectContainer.empty();
            var articlesEl = $("<select class='multiselect articles-params-article'></select>");
            articlesSelectContainer.append(articlesEl);

            var articles = th.GetInvoiceArticles();
            if (articles == null || articles.length == 0) {
                addArticleEl.hide();
                return;
            } else {
                addArticleEl.show();
            }

            for (var i = 0; i < articles.length; i++) {
                var article = articles[i];
                articlesEl.append("<option value='" + article.Id + "'>" + article.GetFullName() + "</option>");
            }
            $('.InitiatorField').hide();

            articlesEl.multiselect();
            articlesSelectContainer.find("button.multiselect").addClass("min-width-500 max-width-500");
            th._container.dialog("open");
            
            th.FillOrganisationDataForOtherReference();
        },

        Initialize: function (e) {
            var th = this;
            th.ProductAreaElement = $(document).find("." + e.attr("data-invoice-product-area"));
            th.CaseId = e.attr("data-invoice-case-id");
            th.CustomerId = e.attr("data-invoice-customerId");
            th.CaseInvoicesElement = $(document).find("." + e.attr("data-invoice-articles-for-save"));
            th.DateFormat = e.attr("data-invoice-date-format");
            var ButtonCaption = e.attr("data-invoice-Caption");
            var ButtonHint = e.attr("data-invoice-Hint");
            
            var button = $(document.createElement("input"))
                .attr("id", "InvoiceModuleBtnOpen")
                .attr("type", "button")
                .attr("value", ButtonCaption )
                .attr("title", ButtonHint)
                .addClass("btn");
            button.click(function () {
                th.OpenInvoiceWindow();                
            });
            e.after(button);

            var onChangeProductArea = function () {
                debugger;
                button.hide();
                $.getJSON("/invoice/articles?productAreaId=" + th.ProductAreaElement.val(), function (data) {
                    th.ClearInvoiceArticles();
                    if (data == null || data.length == 0) {
                        return;
                    }
                    button.show();

                    for (var i = 0; i < data.length; i++) {
                        var a = data[i];
                        var article = new dhHelpdesk.CaseArticles.InvoiceArticle();
                        article.Id = a.Id;
                        article.ParentId = a.ParentId;
                        article.Number = a.Number;
                        article.Name = a.Name;
                        article.NameEng = a.NameEng;
                        article.Description = a.Description;
                        article.UnitId = a.UnitId;
                        if (a.Unit != null) {
                            article.Unit = new dhHelpdesk.CaseArticles.InvoiceArticleUnit();
                            article.Unit.Id = a.Unit.Id;
                            article.Unit.Name = a.Unit.Name;
                            article.Unit.CustomerId = a.Unit.CustomerId;
                        }
                        article.Ppu = a.Ppu;
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
                });
            };

            th.ProductAreaElement.change(function () {
                onChangeProductArea();
            });
            onChangeProductArea();

        },

        IsInteger: function(str) {
            return (str + "").match(/^\d+$/);
        },

        GetRandomInt: function (min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        },   

        ParseDate: function(date) {
            return new Date(parseInt(date.replace(/\/Date\((\d+)\)\//g, "$1")));
        },

        GenerateId: function() {
            return -this.GetRandomInt(1, 10000);
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

        InvoiceArticle: function() {
            this.Id = null;
            this.ParentId = null;
            this.Number = null;
            this.Name = null;
            this.NameEng = null;
            this.Description = null;
            this.UnitId = null;
            this.Unit = null;
            this.Ppu = null;
            this.ProductAreaId = null;
            this.CustomerId = null;

            this.Parent = null;
            this.Children = [];

            this.IsBlank = function() {
                return this.Number == null;
            };

            this.AddChild = function(child) {
                this.Children.push(child);
                child.Parent = this;
            };

            this.HasChildren = function() {
                return this.Children.length > 0;
            };

            this.GetName = function() {
                if (this.Name != null && this.Name != "") {
                    return this.Name;
                }

                return this.NameEng;
            };

            this.GetFullName = function () {
                var fullName;
                if (this.Number != null) {
                    fullName = this.Number + " - " + this.GetName();
                } else {
                    fullName = this.GetName();
                }

                if (this.Unit != null) {
                    fullName += " (" + this.Unit.Name + ")";
                }

                return fullName;
            };

            this.ToCaseArticle = function() {
                var article = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                article.Id = dhHelpdesk.CaseArticles.GenerateId();
                article.Article = this;
                article.Name = this.Name;
                article.Amount = dhHelpdesk.CaseArticles.DefaultAmount;
                article.IsInvoiced = false;
                return article;
            };
        },

        CaseInvoice: function() {
            this.Id = null;
            this.CaseId = null;
            this._orders = [];

            this.GetOrders = function () {
                return this._orders;  
            },

            this.GetOrder = function(id) {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    if (order.Id == id) {
                        return order;
                    }
                }
                return null;
            },

            this.GetLastOrder = function() {
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

            this.HasArticles = function() {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    if (order.HasArticles()) {
                        return true;
                    }
                }
                return false;
            },

            this.HasNotInvoicedArticles = function () {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    if (order.HasNotInvoicedArticles()) {
                        return true;
                    }
                }
                return false;
            },

            this.HasInvoicedArticles = function ()
            {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    if (order.HasInvoicedArticles())
                    {
                        return true;
                    }
                }
                return false;
            },

            this._refreshTabs = function() {
                var tabs = this.Container.find("#case-invoice-orders-tabs");
                tabs.tabs("refresh");
                tabs.find("ul").removeClass("ui-widget-header");
                tabs.find("li").removeClass("ui-state-default");
               

            },

            this.AddOrder = function (order) {
                
                this._orders.push(order);
                order.Invoice = this;
                order.Number = this._orders.length - 1;
                order.Initialize();
                var container = this.Container.find(".orders-container");
                container.append(order.Container);

                var tabs = this.Container.find("#case-invoice-orders-tabs");
                var orderTitle = order.CreditedFrom == null ? dhHelpdesk.CaseArticles.translate("Order") : dhHelpdesk.CaseArticles.translate("Credit Order");
                var newTab = $("<li class='case-invoice-order-tab active'><a href='#case-invoice-order" + order.Id + "'>" + orderTitle + " " + (order.Number + 1) + "</a><li>");
                if (this._orders.length == 1) {
                    tabs.find("ul").prepend(newTab);
                } else {
                    tabs.find("li.case-invoice-order-tab:last").after(newTab);
                }
                this._refreshTabs();
                dhHelpdesk.CaseArticles.FillOrganisationDataForOtherReference();

                newTab.find("a").click();
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
                for (var i = 0; i < orders.length; i++) {
                    ordersResult += orders[i].ToJson();
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

            this.DoInvoice = function() {
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    orders[i].DoInvoice();
                }
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
                model.Id = this.Id != null ? this.Id : dhHelpdesk.CaseArticles.GenerateId();
                var orders = this.GetSortedOrders();
                for (var i = 0; i < orders.length; i++) {
                    model.AddOrder(orders[i].GetViewModel());
                }
                model.Total = this.GetArticlesTotal();

                return model;
            },

            this.ShowSummary = function() {
                this.Container.find(".case-invoice-order-summary").show();
            },

            this.HideSummary = function() {
                this.Container.find(".case-invoice-order-summary").hide();
            },

            this.GetCurrentOrder = function() {
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
                        dhHelpdesk.CaseArticles.RaiseEvent("OnChangeOrder", [th.GetCurrentOrder()]);
                    }
                });

                dhHelpdesk.CaseArticles.OnEvent("OnAddArticle", function(e, order, article) {
                    th.ShowSummary();
                });

                dhHelpdesk.CaseArticles.OnEvent("OnDeleteArticle", function(e, order, articleId) {
                    if (th.HasArticles()) {
                        th.ShowSummary();
                    } else {
                        th.HideSummary();
                    }
                });

                // Set handler for OnChangeOrder event
                dhHelpdesk.CaseArticles.OnEvent("OnChangeOrder", function(e, currentOrder) {
                    if (currentOrder == null) {
                        return;
                    }

                    if (currentOrder.HasInvoicedArticles()) {
                        dhHelpdesk.CaseArticles.OrderActionsInstance.get_creditOrderButton().show();
                    } else {
                        dhHelpdesk.CaseArticles.OrderActionsInstance.get_creditOrderButton().hide();
                    }
                });
            },

            this.GetSortedOrders = function() {
                return this.GetOrders().sort(function (a1, a2) { return a1.Number - a2.Number; });
            },

            this.GetArticlesTotal = function () {
                var total = 0;
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    total += orders[i].GetArticlesTotal();
                }
                return total;
            },

            this.UpdateTotal = function() {
//                this.Container.find("#case-invoice-order-summary-total").text(this.GetArticlesTotal());
            },

            this.Validate = function () {
                var isValid = true;
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    if (!order.Validate()) {
                        isValid = false;
                    }
                }
                return isValid;
            }
        },

        CaseInvoiceOrder: function() {
            this.Id = null;
            this.Invoice = null;
            this.Number = null;
            //this.DeliveryPeriod = null;
            this.InvoiceDate = null;
            this.InvoicedByUser = null;
            this.IsInvoiced = null;
            //this.Reference = null;
            this.Date = null;


            //////////////
            /// INITIATOR FIELDS
            this.ReportedBy = null;
            this.Persons_Name = null;
            this.Persons_Phone = null;
            this.Persons_Cellphone = null;
            this.Region_Id = null;
            this.Department_Id = null;
            this.OU_Id = null;
            this.Place = null;
            this.UserCode = null;
            this.CostCentre = null;
            /////


            this._articles = [];
            this.Container = null;
            this.CreditedFrom = null;
            this.files = dhHelpdesk.CaseArticles.orderFilesCollection({});

            this.GetArticles = function() {
                return this._articles;
            },

            this.HasArticles = function() {
                return this._articles.length > 0;
            },

            this.GetArticle = function(id) {
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (article.Id == id) {
                        return article;
                    }
                }
                return null;
            },

            this.HasNotBlankArticles = function() {
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (!article.IsBlank()) {
                        return true;
                    }
                }
                return false;
            },

            this.HasNotInvoicedArticles = function() {
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (!article.IsInvoiced) {
                        return true;
                    }
                }
                return false;
            },

            this.HasInvoicedArticles = function() {
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (article.IsInvoiced) {
                        return true;
                    }
                }
                return false;
            },

            this.EnableAddBlank = function(enable) {
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

                dhHelpdesk.CaseArticles.RaiseEvent("OnAddArticle", [this, article]);
            },

            this.DeleteArticle = function(id) {
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

                dhHelpdesk.CaseArticles.RaiseEvent("OnDeleteArticle", [this, id]);
            },
           
            this.GetSortedArticles = function() {
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
                        //'"DeliveryPeriod":"' + (this.DeliveryPeriod != null ? this.DeliveryPeriod : '') + '", ' +
                        '"InvoiceDate":"' + (this.InvoiceDate != null ? this.InvoiceDate : '') + '", ' +
                        '"InvoicedByUser":"' + (this.InvoicedByUser != null ? this.InvoicedByUser : '') + '", ' +
                        //'"Reference":"' + (this.Reference != null ? this.Reference : '') + '", ' +
                        '"ReportedBy":"' + (this.ReportedBy != null ? this.ReportedBy : '') + '", ' +
                        '"Persons_Name":"' + (this.Persons_Name != null ? this.Persons_Name : '') + '", ' +
                        '"Persons_Phone":"' + (this.Persons_Phone != null ? this.Persons_Phone : '') + '", ' +
                        '"Persons_CellPhone":"' + (this.Persons_CellPhone != null ? this.Persons_CellPhone : '') + '", ' +
                        '"Region_Id":"' + (this.Region_Id != null ? this.Region_Id : '') + '", ' +
                        '"Department_Id":"' + (this.Department_Id != null ? this.Department_Id : '') + '", ' +
                        '"OU_Id":"' + (this.OU_Id != null ? this.OU_Id : '') + '", ' +
                        '"Place":"' + (this.Place != null ? this.Place : '') + '", ' +
                        '"UserCode":"' + (this.UserCode != null ? this.UserCode : '') + '", ' +
                        '"Articles": [' + articlesResult + '],' +
                        '"Files": [' + filesResult + ']' +
                        '}';
            },

            this.GetArticlesTotal = function () {
                var total = 0;
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    total += articles[i].GetTotal();
                }
                return total;
            },

            this.IsArticlesEmpty = function() {
                return this._articles.length == 0;
            },

            this.DoInvoice = function () {
                this.InvoicedByUser = " ";
                this.CreditedFrom = null;
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    articles[i].DoInvoice();
                }
            },

            this.Clone = function () {
                var clone = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                clone.Id = this.Id;
                clone.Number = this.Number;
                //clone.DeliveryPeriod = this.DeliveryPeriod;
                clone.InvoiceDate = this.InvoiceDate;
                clone.InvoicedByUser = this.InvoicedByUser;
                //clone.Reference = this.Reference;

                ////// INITIATOR FIELDS
                clone.ReportedBy = this.ReportedBy;
                clone.Persons_Name = this.Persons_Name;
                clone.Persons_Phone = this.Persons_Phone
                clone.Persons_Cellphone = this.Persons_Cellphone;
                clone.Region_Id = this.Region_Id;
                clone.Department_Id = this.Department_Id;
                clone.OU_Id = this.OU_Id;
                clone.Place = this.Place;
                clone.UserCode = this.UserCode;
                clone.CostCentre = this.CostCentre;
                


                clone.CreditedFrom = this.CreditedFrom;
                clone.Initialize();
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    clone.AddArticle(articles[i].Clone());
                }
                clone.files = this.files.clone();

                return clone;
            },            

            // Get credited from this order
            this.GetCreditedOrder = function() {
                var credited = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                credited.Id = dhHelpdesk.CaseArticles.GenerateId();
                //credited.DeliveryPeriod = this.DeliveryPeriod;
                //credited.Reference = this.Reference;
                credited.CreditedFrom = this;


                ///////////////
                credited.ReportedBy = this.ReportedBy;
                credited.Persons_Name = this.Persons_Name;
                credited.Persons_Phone = this.Persons_Phone;
                credited.Persons_Cellphone = this.Persons_Cellphone;
                credited.Region_Id = this.Region_Id;
                credited.Department_Id = this.Department_Id;
                credited.OU_Id = this.OU_Id;
                credited.Place = this.Place;
                credited.UserCode = this.UserCode;
                credited.CostCentre = this.CostCentre;




                credited.Initialize();
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var a = articles[i];
                    if (!a.IsInvoiced) {
                        continue;
                    }
                    var article = a.GetCreditedArticle();
                    credited.AddArticle(article);
                }
                return credited;
            },

            this.GetViewModel = function () {
                var model = new dhHelpdesk.CaseArticles.CaseInvoiceOrderViewModel();
                model.Id = this.Id != null ? this.Id : dhHelpdesk.CaseArticles.GenerateId();
                model.Number = this.Number + 1;
                model.Total = this.GetArticlesTotal();
                //model.DeliveryPeriod = this.DeliveryPeriod != null ? this.DeliveryPeriod : "";

                var InvoiceDateWithFormat = "";
                if (this.InvoiceDate != null)
                {
                    InvoiceDateWithFormat = new Date(parseInt(this.InvoiceDate.substr(6)));
                    InvoiceDateWithFormat = InvoiceDateWithFormat.toLocaleDateString();
                }

                model.InvoiceDate = this.InvoiceDate != null ? InvoiceDateWithFormat : "";
                model.InvoicedByUser = this.InvoicedByUser != null ? this.InvoicedByUser : "";
                if (this.InvoicedByUser == null || this.InvoicedByUser == "") {
                    model.IsInvoiced = false;
                }
                else {
                    model.IsInvoiced = true;
                }

                //model.Reference = this.Reference != null ? this.Reference : "";


                ////////initiator field
                model.ReportedBy = this.ReportedBy != null ? this.ReportedBy : "";
                model.ShowReportedBy = dhHelpdesk.CaseArticles.ShowCaseField('ReportedBy')

                model.Persons_Name = this.Persons_Name != null ? this.Persons_Name : "";
                model.ShowPersons_Name = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Name');

                model.Persons_Phone = this.Persons_Phone != null ? this.Persons_Phone : "";
                model.ShowPersons_Phone = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Phone');

                model.Persons_CellPhone = this.Persons_Cellphone != null ? this.Persons_Cellphone : "";
                model.ShowPersons_Cellphone = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Cellphone');

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

                var articles = this.GetSortedArticles();
                for (var i = 0; i < articles.length; i++) {
                    model.AddArticle(articles[i].GetViewModel());
                }
                model.files = this.files.getFiles();
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

                var ReportedBy = this.Container.find(".reportedby");
                ReportedBy.typeahead(dhHelpdesk.CaseArticles.GetUserSearchOptions());
                
                ReportedBy.blur(function () {
                    th.ReportedBy = $(this).val();
                    personsName.blur();
                    phone.blur();
                    cellphone.blur();
                    region.blur();
                    department.blur();
                    ou.blur();
                    place.blur();
                    costcentre.blur();
                    usercode.blur();
                });
                
                var personsName = this.Container.find(".name");
                personsName.blur(function () {
                    th.Persons_Name = $(this).val();
                });

                var phone = this.Container.find(".phone");
                phone.blur(function () {
                    th.Persons_Phone = $(this).val();
                });

                var cellphone = this.Container.find(".cellphone");
                cellphone.blur(function () {
                    th.Persons_Cellphone = $(this).val();
                });

                var region = this.Container.find(".region");
                region.blur(function () {
                    $(this).parent().find('input:hidden').val($(this).val());
                    th.Region_Id = $(this).val();
                    var OrderId = $(this).data("orderid");
                    dhHelpdesk.CaseArticles.FillDepartments(OrderId);
                });

                var department = this.Container.find(".department");
                department.blur(function () {
                    $(this).parent().find('input:hidden').val($(this).val());
                    th.Department_Id = $(this).val();
                    var OrderId = $(this).data("orderid");
                    dhHelpdesk.CaseArticles.FillOUs(OrderId);
                });

                var ou = this.Container.find(".ou");
                ou.blur(function () {
                    $(this).parent().find('input:hidden').val($(this).val());
                    th.OU_Id = $(this).val();
                });

                var place = this.Container.find(".place");
                place.blur(function () {
                    th.Place = $(this).val();
                });

                var costcentre = this.Container.find(".costcentre");
                costcentre.blur(function () {
                    th.CostCentre = $(this).val();
                });

                var usercode = this.Container.find(".usercode");
                usercode.blur(function () {
                    th.UserCode = $(this).val();
                });
            },
            
            this.UpdateTotal = function () {
                this.Container.find(".articles-total").text(this.GetArticlesTotal());
                if (this.Invoice != null) {
                    this.Invoice.UpdateTotal();
                }
            },

            this._deleteFromContainer = function(id) {
                this.Container.find("[data-id='" + id + "']").remove();
            },
            
            this.Validate = function () {
                var isValid = true;
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (!article.Validate()) {
                        isValid = false;
                    }
                }
                return isValid;
            },

            this.UpdateFiles = function () {
                var model = dhHelpdesk.CaseArticles.caseFilesViewModel({
                    caseId: dhHelpdesk.CaseArticles.caseFiles.getCaseId(),
                    files: this.files.getFiles()
                });
                this.Container.find("#files-container").html(dhHelpdesk.CaseArticles.CaseInvoiceOrderFilesTemplate.render(model));
            },

            this.RemoveOrderFile = function(fileName) {
                this.files.deleteFile(fileName);
                this.UpdateFiles();
            },

            this.AddFileByName = function(fileName) {
                var caseFile = dhHelpdesk.CaseArticles.caseFiles.getFile(fileName);
                if (caseFile != null) {
                    this.files.addFile(caseFile.clone());
                }
            },

            this.SelectCaseFiles = function () {
                var model = dhHelpdesk.CaseArticles.caseFilesViewModel({
                    caseId: dhHelpdesk.CaseArticles.caseFiles.getCaseId(),
                    files: dhHelpdesk.CaseArticles.caseFiles.getFiles()
                });

                var that = this;
                var d = $(dhHelpdesk.CaseArticles.CaseInvoiceCaseFilesTemplate.render(model));

                if (model.getFiles().length > 0) {
                    d.dialog({
                        buttons: [
                            {
                                text: dhHelpdesk.CaseArticles.translate("Bifoga"),
                                click: function() {
                                    d.find("input:checked").each(function() {
                                        that.AddFileByName($(this).val());
                                    });
                                    that.UpdateFiles();
                                    d.dialog("close");
                                }
                            },
                            {
                                text: dhHelpdesk.CaseArticles.translate("Stäng"),
                                click: function() {
                                    d.dialog("close");
                                }
                            }
                        ]
                    });
                } else {
                    d.dialog({
                        buttons: [{
                                text: dhHelpdesk.CaseArticles.translate("Stäng"),
                                click: function () {
                                    d.dialog("close");
                                }
                            }
                        ]
                    });
                }

                d.dialog("open");
            }
        },

        ShowCaseField: function (CaseFieldName) {
            var CaseFieldSettingsLength = CaseFieldSettings.Result.length;
            for (var i = 0; i < CaseFieldSettingsLength; i++) {
                if (CaseFieldSettings.Result[i].Name.toLowerCase() === CaseFieldName.toLowerCase())
                {
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

        CaseInvoiceArticle: function() {
            this.Id = null;
            this.Order = null;
            this.Article = null;
            this.Name = null;
            this.Amount = null;
            this.Position = null;
            this.IsInvoiced = null;
            this.Ppu = null;
            this.Container = null;
            this.CreditedFrom = null;

            this.ToJson = function() {
                return '{' +
                        '"Id":"' + (this.Id >= 0 ? this.Id : 0) + '", ' +
                        '"OrderId":"' + (this.Order.Id > 0 ? this.Order.Id : 0) + '", ' +
                        '"ArticleId":"' + (this.Article != null && this.Article.Id > 0 ? this.Article.Id : '') + '", ' +
                        '"Number":"' + this.GetNumber() + '", ' +
                        '"Name":"' + (this.Name != null ? this.Name : '') + '", ' +
                        '"Amount":"' + (this.Amount != null && !this.IsBlank() ? this.Amount : '') + '", ' +
                        '"Ppu":"' + (this.Ppu != null ? this.Ppu : '') + '", ' +
                        '"Position":"' + this.Position + '", ' +
                        '"IsInvoiced":' + (this.IsInvoiced ? '"true"' : '"false"') +
                    '}';
            };

            this.Clone = function() {
                var clone = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                clone.Id = this.Id;
                clone.Article = this.Article;
                clone.Name = this.Name;
                clone.Amount = this.Amount;
                clone.Position = this.Position;
                clone.IsInvoiced = this.IsInvoiced;
                clone.Ppu = this.Ppu;
                clone.CreditedFrom = this.CreditedFrom;
                return clone;
            };

            this.GetCreditedArticle = function() {
                var credited = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                credited.Id = dhHelpdesk.CaseArticles.GenerateId();;
                credited.Article = this.Article;
                credited.Name = this.Name;
                credited.Amount = this.Amount;
                credited.Position = this.Position;
                credited.IsInvoiced = false;
                credited.Ppu = this.Ppu;
                credited.CreditedFrom = this;
                return credited;
            };

            this.IsNew = function() {
                return this.Id <= 0;
            };

            this.IsBlank = function() {
                return this.Article == null || this.Article.IsBlank();
            };

            this.GetTotal = function() {
                if (this.Article != null) {
                    return this.Amount * this.GetPpu();
                }
                return 0;
            };

            this.GetUnitName = function() {
                if (this.Article != null && 
                    this.Article.Unit != null) {
                    return this.Article.Unit.Name;
                }
                return "";
            };

            this.GetNumber = function() {
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
                model.IsInvoiced = this.IsInvoiced;
                model.Name = this.Name != null ? this.Name : "";
                model.NameEng = this.Article != null && this.Article.NameEng != null ? this.Article.NameEng : "";
                model.Description = this.Article != null && this.Article.Description != null ? this.Article.Description : "";
                model.Id = this.Id;
                model.Number = this.GetNumber();
                model.Amount = this.Amount;
                model.UnitName = this.GetUnitName();
                model.Ppu = this.GetPpu();
                model.Total = this.GetTotal();
                model.IsArticlePpuExists = this.IsArticlePpuExists();
                model.IsCredited = this.CreditedFrom != null;
                return model;
            };

            this.IsArticlePpuExists = function() {
                return this.Article != null && this.Article.Ppu != null && this.Article.Ppu > 0;
            },

            this.Initialize = function() {
                this.Container = $(dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate.render(this.GetViewModel()));
            };

            this.Update = function () {
                if (this.IsBlank()) {
                    this.Name = this.Container.find(".article-name").val();
                    return;
                }

                var eAmount = this.Container.find(".article-amount");
                var amount = eAmount.val();
                if (!dhHelpdesk.CaseArticles.IsInteger(amount) || amount <= 0) {
                    amount = dhHelpdesk.CaseArticles.DefaultAmount;
                    eAmount.val(amount);
                }
                this.Amount = amount;

                var ePpu = this.Container.find(".article-ppu");
                if (ePpu.length > 0) {
                    var ppu = ePpu.val();
                    if (!dhHelpdesk.CaseArticles.IsInteger(ppu) || ppu <= 0) {
                        ppu = dhHelpdesk.CaseArticles.DefaultPpu;
                        ePpu.val(ppu);
                    }
                    this.Ppu = ppu;
                }

                this.Container.find(".article-total").text(this.GetTotal());
                this.Order.UpdateTotal();
            },

            this.Refresh = function() {
                var refreshed = $(dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate.render(this.GetViewModel()));
                this.Container.replaceWith(refreshed);
            },

            this.DoInvoice = function() {
                this.IsInvoiced = true;
                this.CreditedFrom = null;
                this.Refresh();
            },

            this.Validate = function () {
                var isValid = true;

                if (!this.IsArticlePpuExists()) {
                    var ePpu = this.Container.find(".article-ppu");
                    if (ePpu.length > 0) {
                        var ppu = ePpu.val();
                        if (!dhHelpdesk.CaseArticles.IsInteger(ppu) || ppu <= 0) {
                            dhHelpdesk.CaseArticles.MakeInvalid(ePpu);
                            isValid = false;
                        } else {
                            dhHelpdesk.CaseArticles.MakeValid(ePpu);
                        }
                    }
                }

                if (this.IsBlank()) {
                    var eName = this.Container.find(".article-name");
                    if (eName.length > 0) {
                        var name = eName.val();
                        if (dhHelpdesk.CaseArticles.IsNullOrEmpty(name)) {
                            dhHelpdesk.CaseArticles.MakeInvalid(eName);
                            isValid = false;
                        } else {
                            dhHelpdesk.CaseArticles.MakeValid(eName);
                        }
                    }
                }

                // Check if units number more then units of the credited article
                var eAmount = this.Container.find(".article-amount");
                if (eAmount.length > 0) {
                    var amount = eAmount.val();
                    if (!dhHelpdesk.CaseArticles.IsInteger(amount) || 
                        amount <= 0 || 
                        (this.CreditedFrom != null && amount > this.CreditedFrom.Amount)) {
                        dhHelpdesk.CaseArticles.MakeInvalid(eAmount);
                        isValid = false;
                    } else {
                        dhHelpdesk.CaseArticles.MakeValid(eAmount);
                    }
                }

                return isValid;
            }
        },

        InvoiceArticleUnit: function() {
            this.Id = null;
            this.Name = null;
            this.CustomerId = null;

            this.Clone = function() {
                var clone = new dhHelpdesk.CaseArticles.InvoiceArticleUnit();
                clone.Id = this.Id;
                clone.Name = this.Name;
                clone.CustomerId = this.CustomerId;
                return clone;
            };
        },

        CaseInvoiceViewModel: function () {
            this.Id = null;
            this.SelectItemHeader = dhHelpdesk.CaseArticles.translate("Välj objekt");
            this.UnitsHeader = dhHelpdesk.CaseArticles.translate("Enheter");
            this.DefaultAmount = dhHelpdesk.CaseArticles.DefaultAmount;
            this.AddButtonLabel = dhHelpdesk.CaseArticles.translate("Lägg till");
            this.OrderTitle = dhHelpdesk.CaseArticles.translate("Order");
            this.SummaryTitle = dhHelpdesk.CaseArticles.translate("Översikt");
            this.TotalLabel = dhHelpdesk.CaseArticles.translate("Total");
            this.Total = null;
            this.Orders = [];

            this.AddOrder = function(order) {
                this.Orders.push(order);
            }
        },

        CaseInvoiceOrderViewModel: function () {
            this.Id = null;
            this.ArtNoHeader = dhHelpdesk.CaseArticles.translate("Art nr");
            this.NameHeader = dhHelpdesk.CaseArticles.translate("Artikelnamn SWE");
            this.NameEngHeader = dhHelpdesk.CaseArticles.translate("Artikelnamn ENG");
            this.UnitsHeader = dhHelpdesk.CaseArticles.translate("Enheter");
            this.TypeHeader = dhHelpdesk.CaseArticles.translate("Typ");
            this.PpuHeader = dhHelpdesk.CaseArticles.translate("PPE");
            this.TotalHeader = dhHelpdesk.CaseArticles.translate("Total");
            this.TotalLabel = dhHelpdesk.CaseArticles.translate("Total");
            //this.DeliveryPeriodTitle = dhHelpdesk.CaseArticles.translate("Leveransperiod");
            this.ReferenceTitle = dhHelpdesk.CaseArticles.translate("Orderreferens");
            this.InvoicedByTitle = dhHelpdesk.CaseArticles.translate("Fakturerad av");

            //LABELS FOR initiator data
            this.ReportedByTitle = dhHelpdesk.CaseArticles.translateCaseFields("ReportedBy");
            this.Persons_NameTitle = dhHelpdesk.CaseArticles.translateCaseFields("Persons_Name");
            this.Persons_PhoneTitle = dhHelpdesk.CaseArticles.translateCaseFields("Persons_Phone");
            this.Persons_CellPhoneTitle = dhHelpdesk.CaseArticles.translateCaseFields("Persons_CellPhone");
            this.Region_IdTitle = dhHelpdesk.CaseArticles.translateCaseFields("Region_Id");
            this.Department_IdTitle = dhHelpdesk.CaseArticles.translateCaseFields("Department_Id");
            this.OU_IdTitle = dhHelpdesk.CaseArticles.translateCaseFields("OU_Id");
            this.PlaceTitle = dhHelpdesk.CaseArticles.translateCaseFields("Place");
            this.UserCodeTitle = dhHelpdesk.CaseArticles.translateCaseFields("UserCode");
            this.CostCentreTitle = dhHelpdesk.CaseArticles.translateCaseFields("CostCentre");

            this.Total = null;
            this.Articles = [];
            //this.DeliveryPeriod = "";
            this.InvoiceDate = "";
            this.InvoicedByUser = "";
            this.IsInvoiced = false;
            //this.Reference = "";

            this.ShowReportedBy = false;
            this.ReportedBy = "";

            this.Persons_Name = "";
            this.ShowPersons_Name = false;

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

            this.attachedFilesTitle = dhHelpdesk.CaseArticles.translate("Bifogade Filer");
            this.attachFilesTitle = dhHelpdesk.CaseArticles.translate("Lägg till");

            this.AddArticle = function(article) {
                this.Articles.push(article);
            }            
        },

        CaseArticleViewModel: function() {
            this.IsBlank = null;
            this.IsInvoiced = null;
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
        },

        // View model for order actions
        orderActionsViewModel: function(spec) {
            var that = {};

            var creditOrderEnabled = spec.creditOrderEnabled || false;
            var creditOrderTitle = spec.creditOrderTitle || dhHelpdesk.CaseArticles.translate("Kreditera order");

            var get_creditOrderEnabled = function() {
                return creditOrderEnabled;
            }

            var get_creditOrderTitle = function() {
                return creditOrderTitle;
            }

            that.get_creditOrderEnabled = get_creditOrderEnabled;
            that.get_creditOrderTitle = get_creditOrderTitle;

            return that;
        },

        caseFilesViewModel: function(spec, my) {
            var that = {};
            my = my || {};

            var caseId = spec.caseId || '';
            var files = spec.files || [];
            var title = spec.title || dhHelpdesk.CaseArticles.translate("Ärendefiler");

            var getCaseId = function() {
                return caseId;
            };

            var getFiles = function() {
                return files;
            };

            var getTitle = function() {
                return title;
            };

            that.getCaseId = getCaseId;
            that.getFiles = getFiles;
            that.getTitle = getTitle;

            return that;
        },

        orderFilesCollection: function(spec, my) {
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
                        if (fs[i].getFileName() == fileName) {
                            fs.splice(i, 1);
                            found = true;
                            break;
                        }
                    }
                } while (found);
            };

            var clone = function() {
                var c = dhHelpdesk.CaseArticles.orderFilesCollection({});
                var fs = getFiles();
                for (var i = 0; i < fs.length; i++) {
                    c.addFile(fs[i].clone());
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

        caseFile: function(spec, my) {
            var that = {};
            my = my || {};

            var fileName = spec.fileName || '';
            var size = spec.size || 0;
            var type = spec.type || '';
            var createdDate = spec.createdDate || '';
            var user = spec.user || '';

            var getFileName = function() {
                return fileName;
            };

            var getSize = function() {
                return size;
            };

            var getType = function() {
                return type;
            };

            var getCreatedDate = function() {
                return createdDate;
            };

            var getUser = function() {
                return user;
            };

            var getEncodedFileName = function() {
                return encodeURIComponent(getFileName());
            };

            var clone = function() {
                var f = dhHelpdesk.CaseArticles.caseFile({
                    fileName: getFileName(),
                    size: getSize(),
                    type: getType(),
                    createdDate: getCreatedDate(),
                    user: getUser()
                });
                return f;
            };

            var toJson = function() {
                return '{' +
                            '"FileName":"' + getEncodedFileName() + '"' +
                        '}';
            };

            that.getFileName = getFileName;
            that.getSize = getSize;
            that.getType = getType;
            that.getCreatedDate = getCreatedDate;
            that.getUser = getUser;
            that.getEncodedFileName = getEncodedFileName;
            that.clone = clone;
            that.toJson = toJson;

            return that;
        },

        caseFilesCollection: function(spec, my) {
            var that = {};
            my = my || {};

            var maxFileSizeMb = spec.maxFileSizeMb || 10;
            var allowedFileTypes = spec.allowedFileTypes || ["application/pdf"];
            var caseId = spec.caseId || '';
            var files = spec.files || [];

            var getMaxFileSizeMb = function() {
                return maxFileSizeMb;
            };

            var getAllowedFileTypes = function() {
                return allowedFileTypes;
            };

            var getCaseId = function() {
                return caseId;
            };

            var getFiles = function() {
                return files;
            };

            var getFile = function(fileName) {
                var fs = getFiles();
                for (var i = 0; i < fs.length; i++) {
                    var f = fs[i];
                    if (f.getFileName() == fileName) {
                        return f;
                    }
                }

                return null;
            };

            var addFile = function(file) {
                getFiles().push(file);
            };

            var deleteFile = function(fileName) {
                var fs = getFiles();
                for (var i = 0; i < fs.length; i++) {
                    if (fs[i].getFileName() == fileName) {
                        fs.splice(i, 1);
                        break;
                    }
                }
            };

            var isFileValid = function(file) {

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

        caseFiles: null
    }

    dhHelpdesk.CaseArticles.OnEvent("OnUploadCaseFile", function (e, uploader, files) {
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var f = dhHelpdesk.CaseArticles.caseFile({
                fileName: file.name,
                size: file.size,
                type: file.type
            });

            if (dhHelpdesk.CaseArticles.caseFiles.isFileValid(f)) {
                dhHelpdesk.CaseArticles.caseFiles.addFile(f);
            }
        }
    });

    dhHelpdesk.CaseArticles.OnEvent("OnDeleteCaseFile", function (e, caseId, fileName) {
        var f = fileName.trim();
        dhHelpdesk.CaseArticles.caseFiles.deleteFile(f);

        var orders = dhHelpdesk.CaseArticles.GetInvoice().GetOrders();
        for (var i = 0; i < orders.length; i++) {
            orders[i].files.deleteFile(f);
            orders[i].UpdateFiles();
        }
    });

    var loadCaseFiles = function () {
        dhHelpdesk.CaseArticles.CaseKey = $(document).find("[data-invoice]").attr("data-invoice-case-key");
        dhHelpdesk.CaseArticles.caseFiles = dhHelpdesk.CaseArticles.caseFilesCollection({
            caseId: dhHelpdesk.CaseArticles.CaseKey
        });

        return $.getJSON("/invoice/casefiles?id=" + dhHelpdesk.CaseArticles.CaseKey, function (data) {
            for (var i = 0; i < data.length; i++) {
                var file = data[i];
                var f = dhHelpdesk.CaseArticles.caseFile({
                    fileName: file.FileName,
                    size: file.Size,
                    type: file.Type
                });

                if (dhHelpdesk.CaseArticles.caseFiles.isFileValid(f)) {
                    dhHelpdesk.CaseArticles.caseFiles.addFile(f);
                }
            }
        });
    };

    var loadTranslationList = function () {
        return $.getJSON("/Translation/GetAllCoreTextTranslations", function (data) {
            AllTranslations = data;
        });
    };

    var loadCaseFieldTranslations = function () {
        return $.getJSON("/Translation/GetCaseFieldsForTranslation", function (data)
        {
            AllCaseFields = data;
        });
    };

    var loadCaseFieldSettings = function () {
        return $.getJSON("/Cases/GetCaseFields", function (data)
        {
            CaseFieldSettings = data;
        });
    };

    var loadCaseInvoiceTemplate = function() {
        return $.get("/content/templates/case-invoice.tmpl.html", function(caseInvoiceTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceTemplate = $.templates("caseInvoice", caseInvoiceTemplate);
        });
    };

    var loadCaseInvoiceOrderTemplate = function() {
        return $.get("/content/templates/case-invoice-order.tmpl.html", function(caseInvoiceOrderTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceOrderTemplate = $.templates("caseInvoiceOrder", caseInvoiceOrderTemplate);
        });
    };

    var loadCaseInvoiceArticleTemplate = function() {
        return $.get("/content/templates/case-invoice-article.tmpl.html", function(caseInvoiceArticleTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate = $.templates("caseInvoiceArticle", caseInvoiceArticleTemplate);
        });
    };

    var loadCaseInvoiceOverviewTemplate = function() {
        return $.get("/content/templates/case-invoice-overview.tmpl.html", function(caseInvoiceOverviewTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceOverviewTemplate = $.templates("caseInvoiceOverview", caseInvoiceOverviewTemplate);
        });
    };

    var loadCaseInvoiceArticleOverviewTemplate = function() {
        return $.get("/content/templates/case-invoice-article-overview.tmpl.html", function(caseInvoiceArticleOverviewTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceArticleOverviewTemplate = $.templates("caseInvoiceArticleOverview", caseInvoiceArticleOverviewTemplate);
        });
    };

    var loadCaseInvoiceOrderActionsTemplate = function() {
        return $.get("/content/templates/case-invoice-order-actions.tmpl.html", function(caseInvoiceOrderActionsTemplate) {
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
        .then(function() {
            $("[data-invoice]").each(function () {
                var $this = $(this);
                var data = $.parseJSON($this.attr("data-invoice-case-articles"));

                if (data == null || data.Invoices.length == 0) {
                    var blankInvoice = new dhHelpdesk.CaseArticles.CaseInvoice();
                    blankInvoice.Id = dhHelpdesk.CaseArticles.GenerateId();
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
                dhHelpdesk.CaseArticles.AddInvoice(invoice);
                dhHelpdesk.CaseArticles.Initialize($this);
                if (inv.Orders != null) {
                    for (var j = 0; j < inv.Orders.length; j++) {
                        var ord = inv.Orders[j];
                        var order = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                        order.Id = ord.Id;
                        order.Number = ord.Number;
                        //order.DeliveryPeriod = ord.DeliveryPeriod;
                        //order.Reference = ord.Reference;
                        order.InvoiceDate = ord.InvoiceDate;
                        order.InvoicedByUser = ord.InvoicedByUser;
                        if (ord.InvoicedByUserId != 0)
                        {
                            order.IsInvoiced = true;
                        }
                        

                        /////////////////////////
                        ///////Initiator labels
                        order.ReportedBy = ord.ReportedBy;
                        order.Persons_Name = ord.Persons_Name;
                        order.Persons_Phone = ord.Persons_Phone;
                        order.Persons_Cellphone = ord.Persons_Phone;
                        order.Region_Id = ord.Region_Id;
                        order.Department_Id = ord.Department_Id;
                        order.OU_Id = ord.OU_Id;
                        order.Place = ord.Place;
                        order.UserCode = ord.UserCode;
                        order.CostCentre = ord.CostCentre;




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
                                caseArticle.IsInvoiced = article.IsInvoiced;
                                order.AddArticle(caseArticle);
                            }
                        }
                    }
                }
                dhHelpdesk.CaseArticles.ApplyChanges();
            });
        });
});