if (window.dhHelpdesk == null)
    dhHelpdesk = {};



$(function () {
    

    _INVOICE_IDLE = 0;
    _INVOICE_SAVING = 1;

    //Max floating point digits
    _MAX_FLOATING_POINT = 2;
    _IGNORE_ZERO_FLOATING_POINT = false;
    var lastRequestedKey = '';
    var regionUrl = '/Organization/GetRegions/';
    var departmentUrl = '/Organization/GetDepartments/';
    var ouUrl = '/Organization/GetOUs/';

    //used for translating casefields
    var AllCaseFields = [];

    var AllTranslations = [];

    var CaseFieldSettings = [];    

    var caseButtonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
                                 '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                                 '.btn.show-inventory, .btn.previous-case, .btn.next-case');

    var invoiceButtons = '.btn.close-invoice, .btn.save-invoice, .btn.save-close-invoice';

    var invoiceActionButtons = '.btn.orderButtons, .btn.save-invoice, .btn.save-close-invoice, .btn.invoiceFileUploader, .articles-params-add.btn';

    var saveInvoiceIndicator = '.loading-msg.save-invoice';

    var invoiceButtonIndicator = $("#invoiceButtonIndicator");

    var projectElement = $("#case__Project_Id");

    var creditAlertToShow = "";
    var creditAlert = true;

    var lastProjectSelected = $("#case__Project_Id option:selected");
    projectElement.change(function () {
        var invoiced = dhHelpdesk.CaseArticles.GetInvoicedOrders(dhHelpdesk.CaseArticles.allVailableOrders);
        if (invoiced.length > 0) {
            dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate('Projekt') + ' ' +
                                                 dhHelpdesk.Common.Translate('kan inte ändras eftersom det finns order som är skickade.'));
            lastProjectSelected.attr("selected", true);
            return;
        }
        lastProjectSelected = $("#case__Project_Id option:selected");
    });   

    /* Extenstions */
    if (typeof String.prototype.RemoveNonNumerics !== "function") {
        String.prototype.RemoveNonNumerics = function () {
            if (this == undefined)
                return "";

            return this.replace(/\D/g, '');
        };
    }

    dhHelpdesk.Const = {
        _ASC_SORT: 1,
        _DESC_SORT: 2
    }

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

        IsDouble: function (str) {
            str = str.toString();
            str = str.replace(" ", "");
            return (str + "").match(/^-?\d*(\.\d+)?$/);
        },

        GetRandomInt: function (min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        },

        GetDecimalSeparator: function () {
            var n = 1.1;
            n = n.toLocaleString().substring(1, 2);
            return n;
        },

        ConvertStrToDouble: function (val) {
            if (val == undefined)
                return undefined;

            var strVal = val.toString();
            if (dhHelpdesk.Common.IsNullOrEmpty(strVal))
                return 0;

            strVal = strVal.replace(this.GetDecimalSeparator(), ".");
            if (strVal == "." || status == ".0" || status == ".00")
                strVal = 0;

            return parseFloat(strVal);
        },

        ConvertDoubleToStr: function (val, ignoreZeroFloating) {
            if (val == undefined) {
                return '';
            }

            strVal = val.toString();
            if (dhHelpdesk.Common.IsNullOrEmpty(strVal))
                return '0';

            var sep = this.GetDecimalSeparator();
            strVal = strVal.replace(".", sep);
            var splitedValues = this.SplitFloating(strVal, sep);
            var fpTester = '';
            var floatingPointIsZero = false;
            for (var mfp = 0; mfp < _MAX_FLOATING_POINT; mfp++) {
                if (splitedValues.floatingPoint == fpTester) {
                    floatingPointIsZero = true;
                    break;
                }
                fpTester += '0';
            }

            if (ignoreZeroFloating && floatingPointIsZero) {
                return splitedValues.integral;
            } else {
                return splitedValues.integral + (splitedValues.floatingPoint != '' ? sep + splitedValues.floatingPoint : sep + this.GetMaxFloatingPointZero());
            }
        },

        SplitFloating: function (strVal, seperator) {
            var newVal = strVal.replace(".", seperator);
            var arrayOfVal = newVal.split(seperator);

            var intVal = '';
            var floatVal = '';

            if (arrayOfVal.length >= 1)
                intVal = arrayOfVal[0];

            if (arrayOfVal.length >= 2) {
                floatVal = dhHelpdesk.Common.PadRight(arrayOfVal[1], _MAX_FLOATING_POINT, '0');
                floatVal = floatVal.substring(0, _MAX_FLOATING_POINT);
            }

            var res = { integral: intVal, floatingPoint: floatVal };
            return res;
        },

        GetMaxFloatingPointZero: function () {
            return dhHelpdesk.Common.PadLeft("", _MAX_FLOATING_POINT, "0");
        }

    },

    

    dhHelpdesk.Common = {

        EncodeStrToJson: function (value) {
            if (value == undefined)
                return "";

            var data = value.toString();
            data = data.replace(/\</g, '%3C');
            data = data.replace(/\"/g, '″');
            return data;
        },

        DecodeJsonToStr: function (value) {
            if (value == undefined)
                return "";

            var data = value.toString();
            data = data.replace(/\%3C/g, '<');            
            return data;
        },                   

        QuotationUpdate: function (value) {
            if (value == undefined || value == null)
                return "";

            var data = value.toString();
            data = data.replace(/\″/g, '\"');
            return data;
        },


        HtmlQuotationUpdate: function (value) {            
            if (value == undefined || value == null)
                return "";

            var data = value.toString();
            data = data.replace(/\″/g, '&quot;');
            data = data.replace(/\</g, "&lt;")
            return data;
        },

        GenerateId: function () {
            return -dhHelpdesk.Math.GetRandomInt(1, 10000);
        },

        GenerateRandomKey: function () {
            function s3() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s3() + '-' + s3() + '-' + s3();
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
                              this.PadLeft((_date.getMonth() + 1), 2, '0') + "-" +
                              this.PadLeft(_date.getDate(), 2, '0');
            }
            return displayDate;
        },

        DateToDisplayTime: function (date) {
            var displayTime = "";
            if (date != null) {
                var _date = new Date(parseFloat(date.substr(6)));
                displayTime = this.PadLeft(_date.getHours(), 2, '0') + ":" +
                              this.PadLeft(_date.getMinutes(), 2, '0') + ":" +
                              this.PadLeft(_date.getSeconds(), 2, '0');
            }
            return displayTime;
        },

        PadLeft: function (value, totalLength, padChar) {
            var valLen = value.toString().length;
            var diff = totalLength - valLen;
            if (diff > 0) {
                for (i = 0; i < diff; i++)
                    value = padChar + value;
            }
            return value;
        },

        PadRight: function (value, totalLength, padChar) {
            var valLen = value.toString().length;
            var diff = totalLength - valLen;
            if (diff > 0) {
                for (i = 0; i < diff; i++)
                    value = value + padChar;
            }
            return value;
        },

        MakeTextNumeric: function (elementName) {
            $(elementName).on("keypress keyup blur", function (event) {
                var decimalSeparator = dhHelpdesk.Math.GetDecimalSeparator();
                var decimalChar = ".".charCodeAt(0);
                if (decimalSeparator != "") {
                    decimalChar = decimalSeparator.charCodeAt(0);
                }

                var curValue = $(this).val();
                var decimalPoint = curValue.indexOf(decimalSeparator);

                var needCheckDecimal = true;
                var sst = this.selectionStart;
                var sed = this.selectionEnd;
                if (sst != sed) {
                    var selectionValue = $(this).val().substring(sst, sed);
                    if (selectionValue.indexOf(decimalSeparator) <= -1) {
                        if (!(decimalPoint > -1 && event.which == decimalChar))
                            needCheckDecimal = false;
                    } else {

                        needCheckDecimal = false;
                    }
                } else {
                    if (decimalPoint > -1 && sst <= decimalPoint && event.which != decimalChar) {
                        needCheckDecimal = false;
                    }
                }

                var valuesToEnd = curValue.substr(sed, curValue.length);
                if (event.which == decimalChar && valuesToEnd.length > _MAX_FLOATING_POINT) {
                    event.preventDefault();
                    return;
                }

                if (needCheckDecimal) {

                    if (!dhHelpdesk.Common.IsNullOrEmpty(curValue)) {
                        var decimalPoint = curValue.indexOf(decimalSeparator);
                        if (decimalPoint > -1) {
                            if (event.which == decimalChar) {
                                event.preventDefault();
                                return;
                            }

                            if ((decimalPoint + _MAX_FLOATING_POINT) < curValue.length) {
                                event.preventDefault();
                                return;
                            }
                        }
                    }
                }

                if (event.which == 37 || event.which == 38 || event.which == 39 || event.which == 40 || event.which == decimalChar ||
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
            if (AllTranslations == undefined || AllTranslations == null || AllTranslations.length == 0)
                return text;
            
            for (var tr = 0; tr < AllTranslations.length; tr++) {
                if (AllTranslations[tr].Key.toLowerCase() == text.toLowerCase())
                    if (AllTranslations[tr].Value != null)
                        return AllTranslations[tr].Value;
                    else
                        return text;
            }

            return text;
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

    dhHelpdesk.OrganizationLevels = {
        Region: function () {
            var _region = this;
            this.Items = [];

            this.Fill = function(customerId, methodUrl) {
                if (customerId == null)
                    return null;

                var promise = $.ajax(methodUrl, { data: { customerId: customerId }});
                promise.done(function (data) {
                    _region.Items = [];
                    if (data != undefined) {
                        for (var i = 0; i < data.regions.length; i++) {
                            var item = data.regions[i];
                            _region.Items.push({ Id: item.Id, Name: item.Name, IsActive: item.IsActive });
                        }
                    }
                });

                return promise;
            }
            this.GetById = function (regionId) {
                for (var i = 0; i < _region.Items.length; i++) {
                    var item = _region.Items[i];
                    if (item.Id == regionId)
                        return item;
                }
                return null;
            }

            this.GetActiveItems = function (alternativeId) {
                var ret = [];
                for (var i = 0; i < _region.Items.length; i++) {
                    var item = _region.Items[i];
                    if (item.Id == alternativeId || item.IsActive)
                        ret.push({ Id: item.Id, Name: item.Name });
                }

                return ret.sort(SortByName);
            }

            var SortByName = function (a, b) {
                var aName = a.Name.toLowerCase();
                var bName = b.Name.toLowerCase();
                return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
            }

        },

        Department: function (region) {
            var _department = this;
            var _region = region;
            this.Items = [];

            this.Fill = function (customerId, methodUrl) {
                if (customerId == null)
                    return null;
                var promise = $.ajax(methodUrl, { data: { customerId: customerId } });
                promise.done(function (data) {
                        _department.Items = [];
                        if (data != undefined) {
                            for (var i = 0; i < data.departments.length; i++) {
                                var item = data.departments[i];
                                var curRegion = (item.RegionId != null ? _region.GetById(item.RegionId) : null);
                                _department.Items.push(
                                    {
                                        Id: item.Id,
                                        Name: item.Name,
                                        IsActive: item.IsActive,
                                        RegionId: item.RegionId,
                                        DisabledForOrder: item.DisabledForOrder,
                                        Region: curRegion
                                    });
                            }
                        }
                });

                return promise;
            }

            this.GetById = function (departmentId) {
                for (var i = 0; i < _department.Items.length; i++) {
                    var item = _department.Items[i];
                    if (item.Id == departmentId)
                        return item;
                }
                return null;
            }

            this.GetActiveItems = function (regionId, alternativeId) {
                var ret = [];
                for (var i = 0; i < _department.Items.length; i++) {
                    var item = _department.Items[i];
                    if ((item.Id == alternativeId || item.IsActive) && (regionId == null || regionId == "" || item.RegionId == regionId))
                        ret.push({ Id: item.Id, Name: item.Name });
                }

                return ret.sort(SortByName);
            }

            var SortByName = function (a, b) {
                var aName = a.Name.toLowerCase();
                var bName = b.Name.toLowerCase();
                return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
            }

        },

        OU: function (department) {
            var _ou = this;
            var _department = department;
            this.Items = [];

            this.Fill = function (customerId, methodUrl) {
                if (customerId == null)
                    return null;

                var promise = $.ajax(methodUrl, { data: { customerId: customerId } });
                promise.done(function (data) {
                        _ou.Items = [];
                        if (data != undefined) {
                            for (var i = 0; i < data.ous.length; i++) {
                                var item = data.ous[i];
                                var curDepartment = (item.DepartmentId != null ? _department.GetById(item.DepartmentId) : null);
                                _ou.Items.push({ Id: item.Id, Name: item.Name, IsActive: item.IsActive, DepartmentId: item.DepartmentId, Department: curDepartment });
                            }
                        }
                });

                return promise;
            }

            this.GetById = function (ouId) {
                for (var i = 0; i < _ou.Items.length; i++) {
                    var item = _ou.Items[i];
                    if (item.Id == ouId)
                        return item;
                }
                return null;
            }

            this.GetActiveItems = function (departmentId, alternativeId) {
                var ret = [];
                for (var i = 0; i < _ou.Items.length; i++) {
                    var item = _ou.Items[i];
                    if ((item.Id == alternativeId || item.IsActive) && item.DepartmentId == departmentId)
                        ret.push({ Id: item.Id, Name: item.Name });
                }

                return ret.sort(SortByName);
            }

            var SortByName = function (a, b) {
                var aName = a.Name.toLowerCase();
                var bName = b.Name.toLowerCase();
                return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
            }
        },

        OrganizationStruct: function (customerId, regionMethodUrl, departmentMethodUrl, ouMethodUrl) {
            var base = this;
            var RegionPath = regionMethodUrl;
            var DepartmentPath = departmentMethodUrl;
            var OUPath = ouMethodUrl;

            this.CusromerId = customerId;
            this.Region = new dhHelpdesk.OrganizationLevels.Region();
            this.Department = new dhHelpdesk.OrganizationLevels.Department(this.Region);
            this.OU = new dhHelpdesk.OrganizationLevels.OU(this.Department);

            this.Initialize = function () {
                return base.Region.Fill(base.CusromerId, RegionPath)
                    .then(function() { return base.Department.Fill(base.CusromerId, DepartmentPath); })
                    .then(function () { return base.OU.Fill(base.CusromerId, OUPath) });
            }
        }
    }

    dhHelpdesk.CaseArticles = {

        OrderStates: {
            NotSaved: 0,
            Saved: 1,
            Sent: 2,
            Deleted: 9
        },

        LastSelectedTab: null,

        LastSelectedTabCaption: "",

        MaxDescriptionCount: 50,

        DefaultAmount: 1,

        DefaultPpu: 0,

        DateFormat: null,

        _invoices: [],

        _invoicesBackup: [],

        _container: null,

        _invoiceArticles: [],

        allVailableOrders: [],

        OrganizationData: null,

        ProductAreaElement: null,

        ProjectElement: $("#case__Project_Id"),

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

        invoiceButtonPureCaption: "",

        OtherReferenceRequired: null,

        OtherReferenceMandatoryFields: function () {            
            this.ReportedBy = false;
            this.Persons_Name = false;
            this.Persons_Email = false;
            this.Persons_Phone = false;
            this.Persons_CellPhone = false;
            this.Region_Id = false;
            this.Department_Id = false;
            this.OU_Id = false;
            this.Place = false;
            this.UserCode = false;
            this.CostCentre = false;
        },

        ResetAddArticlePlace: function () {
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

        IsProductAreaChanged: function () {
            var lastProductArea = $('#LastProductAreaId').val();
            if (this.ProductAreaElement.val() != lastProductArea)
                return true;
            else
                return false;
        },

        IsProjectChanged: function () {
            var lastProjectId = $('#LastProjectId').val();
            var curProjectId = this.ProjectElement.val();
            curProjectId = curProjectId == undefined ? "" : curProjectId;
            if (curProjectId != lastProjectId)
                return true;
            else
                return false;
        },

        IsAnyFileAdded: function () {
            var isFileAdded = $('#IsAnyFileAdded').val();
            if (isFileAdded == "1")
                return true;
            else
                return false;
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

        FillInvoiceOrganisationData: function () {
            $.each(this._invoices, function (i, invoice) {
                $.each(invoice._orders, function (j, order) {
                    dhHelpdesk.CaseArticles.FillOrderOrganizationData(order);
                });
            });
        },

        FillOrderOrganizationData: function (order) {
            dhHelpdesk.CaseArticles.FillOrderRegion(order);
        },

        FillOrderRegion: function (order) {
            var regions = dhHelpdesk.CaseArticles.OrganizationData.Region.GetActiveItems(order.Region_Id);
            var elementId = "RegionSelect-Order" + order.Id;
            dhHelpdesk.CaseArticles.PopulateDropDownItem(elementId, regions, order.Region_Id);
            dhHelpdesk.CaseArticles.FillOrderDepartment(order, order.Region_Id, false);
        },

        FillOrderDepartment: function (order, regionId, noSelection) {
            var departments = dhHelpdesk.CaseArticles.OrganizationData.Department.GetActiveItems(regionId, order.Department_Id);
            var elementId = "DepartmentSelect-Order" + order.Id;

            var selectedItem = order.Department_Id;
            if (noSelection) {
                selectedItem = null;
                order.Department_Id = null;
            }

            dhHelpdesk.CaseArticles.PopulateDropDownItem(elementId, departments, selectedItem);            
            this.SetSendOrderForDepartment(order.Id, order.Department_Id);
            dhHelpdesk.CaseArticles.FillOrderOU(order, selectedItem, noSelection);
        },

        FillOrderOU: function (order, departmentId, noSelection) {
            var ous = dhHelpdesk.CaseArticles.OrganizationData.OU.GetActiveItems(departmentId, order.OU_Id);
            var elementId = "OUSelect-Order" + order.Id;
            var selectedItem = order.OU_Id;
            if (noSelection) {
                selectedItem = null;
                order.OU_Id = null;
            }

            dhHelpdesk.CaseArticles.PopulateDropDownItem(elementId, ous, selectedItem);
            dhHelpdesk.CaseArticles.UpdateAvailableOrder(order);
        },

        SetSendOrderForDepartment: function (orderId, departmentId) {
            if (departmentId == undefined) {
                $("#doInvoiceButton_" + orderId).removeAttr("disabled");
                return;
            }

            var department = dhHelpdesk.CaseArticles.OrganizationData.Department.GetById(departmentId);
            if (department != null && department.DisabledForOrder) {
                $("#doInvoiceButton_" + orderId).attr("disabled", "disabled");
                var msg = dhHelpdesk.Common.TranslateCaseFields("Department_Id") + " " + 
                            dhHelpdesk.Common.Translate("kan inte faktureras");

                $("#case-invoice-order" + orderId).find('#SelDepMessage').html(msg);
                            
                //var _hint = dhHelpdesk.Common.Translate("Vald") + " " + 
                //            dhHelpdesk.Common.TranslateCaseFields("Department_Id") + " " + 
                //            dhHelpdesk.Common.Translate("kan inte faktureras");
                            
                //$("#doInvoiceButton_" + orderId).attr("title", _hint);
            }
            else {
                $("#case-invoice-order" + orderId).find('#SelDepMessage').html('');
                $("#doInvoiceButton_" + orderId).removeAttr("disabled");
                $("#doInvoiceButton_" + orderId).attr("title", "");
            }
        },

        PopulateDropDownItem: function (elementId, items, selectedItem_Id) {
            var selector = $('#' + elementId);
            selector.empty();

            var defaultOption = "";
            if (selectedItem_Id != null)
                defaultOption = '<option value=""> </option>';
            else
                defaultOption = '<option value="" selected> </option>';

            selector.append(defaultOption);

            $.each(items, function (i, item) {
                var curOption = "";
                if (item.Id == selectedItem_Id)
                    curOption = '<option value="' + item.Id + '" selected>' + item.Name + '</option>';
                else
                    curOption = '<option value="' + item.Id + '">' + item.Name + '</option>';

                selector.append(curOption);
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
            if (this.CaseId == undefined || this.CaseId == null || this.IsNewCase()) {
                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                return;
            }

            // Keep order id to show order tab after reload
            // If order id be less than 0 we'll try to find it by Tab Caption
            var curOrder = this.GetCurrentOrder();
            if (curOrder != null) {
                dhHelpdesk.CaseArticles.LastSelectedTab = curOrder.Id;
                dhHelpdesk.CaseArticles.LastSelectedTabCaption = curOrder.Caption;
            }


            var res = "";
            var caseArticleData = this.GetSavedInvoices();            
            
            $.post('/CaseInvoice/SaveCaseInvoice/', {
                'caseInvoiceArticle': caseArticleData,
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
                        dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Ett fel har uppstått vid spara Order") + " <br/>" + returnedData.data);
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

                default:
                    dhHelpdesk.Common.ShowErrorMessage("State is not implemented!");
            }

            var caseIsLocked = window.parameters.isCaseLocked;            
            if (caseIsLocked.toLowerCase() == 'true') {                
                $(invoiceActionButtons).addClass("disabled");
                $(invoiceActionButtons).css("pointer-events", "none");
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
            dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Den här ordern är redan skickat. Du kan inte utföra den här aktiviteten."));
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
            blankOrder.CostCentre = $('#case__CostCentre').val().RemoveNonNumerics();
            blankOrder.CreditForOrder_Id = null;
            blankOrder.Project_Id = projectElement.val();
            blankOrder.IsInvoiced = false;
            blankOrder.OrderState = dhHelpdesk.CaseArticles.OrderStates.NotSaved;

            return blankOrder;
        },

        AddOrder: function (invoiceId) {
            if (this.IsFinishedCase()) {
                return
            }

            var invoice = this.GetInvoice(invoiceId);
            if (invoice != null && this.CanAddNewOrder()) {
                invoice.AddOrder(this.CreateBlankOrder());
                $('#btnAdOrder').css("display", "none");
            }
        },

        CanAddNewOrder: function () {
            if (this._invoices != null && this._invoices != undefined && this._invoices.length > 0) {
                var curInvoice = this._invoices[0];
                if (curInvoice._orders != null && curInvoice._orders.length > 0) {
                    for (i = 0; i < curInvoice._orders.length; i++)
                        if (curInvoice._orders[i].Id < 0) {
                            dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Ny order är redan skapad") + " - " + curInvoice._orders[i].Caption);
                            return false;
                        }
                }
                return true;
            }
            else {
                dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Order saknas") + "!");
                return false;
            }
        },

        DoOrderDelete: function (orderId) {
            if (this.allVailableOrders.length <= 1)
                return;

            for (var i = 0; i < this.allVailableOrders.length; i++) {
                var order = this.allVailableOrders[i];
                if (order.Id == id) {
                    this._orders.splice(i, 1);
                    var tabs = this.Container.find("#case-invoice-orders-tabs");
                    tabs.find("[href='#case-invoice-order" + order.Id + "']").parent().remove();
                    order.Container.remove();
                    this._refreshTabs();
                    this.UpdateTotal();
                    var t = dhHelpdesk.CaseArticles.allVailableOrders;
                    return;
                }
            }
        },

        CreateBlankArticle: function () {
            var blank = new dhHelpdesk.CaseArticles.InvoiceArticle();
            blank.Id = dhHelpdesk.Common.GenerateId();
            return blank;
        },

        AddTextToArticle: function (articleId) {
            var article = this.GetArticle(articleId);
            if (article != null) {
                var order = this.GetOrder(article.Order.Id);
                if (order != null) {
                    if (!this.GetInvoiceStatusOfCurrentOrder()) {
                        var article = this.CreateBlankArticle();
                        var caseArticle = article.ToCaseArticle();
                        caseArticle.Article = article;
                        caseArticle.TextForArticle_Id = articleId;
                        order.AddArticle(caseArticle);
                        return caseArticle;
                    }
                    else {
                        this.ShowAlreadyInvoicedMessage();
                        return null;
                    }
                }
            }
        },

        AddBlankArticle: function (orderId, mainArtId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                if (!this.GetInvoiceStatusOfCurrentOrder()) {
                    var article = this.CreateBlankArticle();
                    var caseArticle = article.ToCaseArticle();
                    caseArticle.TextForArticle_Id = mainArtId;
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
            $.getJSON("/CaseInvoice/InvoiceSettingsValid/?CustomerId=" + th.CustomerId, function (data) {
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
                                dhHelpdesk.Common.ShowErrorMessage(order.Caption + " " + dhHelpdesk.Common.Translate("kunde inte sparas då det saknas data i ett eller flera obligatoriska fält. Var vänlig kontrollera i ordern.") + orderValidation.Message);
                        }
                        else {
                            th.ShowAlreadyInvoicedMessage();
                        }
                    }
                    else {
                        dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Du måste spara ärendet en gång innan du kan skicka") + ".");
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
                dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate('Var vänlig spara ordern först.'));
            }
            else {
                if (this.CanAddNewOrder()) {
                    var curOrd = dhHelpdesk.CaseArticles.GetOrder(orderId);
                    var newCredit = curOrd.GetCreditedOrder();
                    if (newCredit._articles.length > 0) {
                        dhHelpdesk.CaseArticles.GetInvoice(curOrd.Invoice.Id).AddOrder(newCredit);
                        dhHelpdesk.Common.MakeTextNumeric(".article-ppu");
                        dhHelpdesk.Common.MakeTextNumeric(".article-amount");
                    }
                }
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

        ExpandOtherReference: function (orderId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                $('.InitiatorFieldsOrder' + orderId).show();
            }

            $('.icon-plus-sign.showRefrence' + orderId).css("display", "none");
            $('.icon-minus-sign.hideRefrence' + orderId).css("display", "inline-block");
        },

        CollapseOtherReference: function (orderId) {
            var order = this.GetOrder(orderId);
            if (order != null) {
                $('.InitiatorFieldsOrder' + orderId).hide();
            }

            $('.icon-plus-sign.showRefrence' + orderId).css("display", "inline-block");
            $('.icon-minus-sign.hideRefrence' + orderId).css("display", "none");
        },

        UpdateOtherReferenceTitle: function (orderId) {
            var publicClassName = ".InitiatorFields_" + orderId;

            var allTextFields = $(document).find(".characterMapping");
            $.each(allTextFields, function () {
                var tempText =  $(this).val();                                    
                $(this).val(dhHelpdesk.Common.QuotationUpdate(tempText));                
            });

            var _reportedBy = $("#ReportedBy_" + orderId).val();
            var _personName = $("#Persons_Name_" + orderId).val();
            var _costCentre = $("#CostCentre_" + orderId).val();
                      
            _reportedBy = dhHelpdesk.Common.IsNullOrEmpty(_reportedBy) ? "" : " " + _reportedBy;
            _personName = dhHelpdesk.Common.IsNullOrEmpty(_personName) ? "" : " (" + _personName + ")";
            _costCentre = dhHelpdesk.Common.IsNullOrEmpty(_costCentre) ? "" : " - " + _costCentre;

            var titleText = "-" + _reportedBy + _personName + _costCentre;
            $('#initiatorInfo_' + orderId).text(titleText);

        },

        UpdateFileAttachment: function (order) {
            if (order != null) {                
                var files = order.files.getFiles();
                this.UpdateFileAttachmentTitle(order.Id, files.length);
            }
        },

        UpdateFileAttachmentTitle: function (orderId, val) {
            if (val > 0) {
                $("#orderAttachedFilesNum_" + orderId).text(' (' + val + ')');
                this.ShowAttached(orderId);
            }
            else {
                $("#orderAttachedFilesNum_" + orderId).text('');
            }            
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
            var curAmount = dhHelpdesk.Math.ConvertStrToDouble(obj.val());
            var article = this.GetArticle(id);

            if (article != null) {
                if (curAmount <= 0) {
                    obj.val(dhHelpdesk.Math.ConvertDoubleToStr(article.Amount, true));
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("0 är inte tillåtet"));
                    return;
                }

                if (article.Amount != null && article.Order.CreditForOrder_Id != null) {
                    var validatedAmount = this.ValidateAmount(article.Order.Id, article.CreditedForArticle_Id, id, curAmount);
                    obj.val(dhHelpdesk.Math.ConvertDoubleToStr(validatedAmount, _IGNORE_ZERO_FLOATING_POINT));
                }
                article.Update();
            }
        },

        RefreshArticleData: function (article) {
            var articles = null;

            for (i = 0; i < this.allVailableOrders.length; i++) {
                if (this.allVailableOrders[i].Articles != undefined)
                    articles = this.allVailableOrders[i].Articles;
                else if (this.allVailableOrders[i]._articles != undefined)
                    articles = this.allVailableOrders[i]._articles;

                if (articles != null) {
                    for (var j = 0; j < articles.length; j++) {
                        if (articles[j].Id == article.Id) {
                            articles[j].Order = article.Order;
                            articles[j].Article = article.Article;
                            articles[j].Name = article.Name;
                            articles[j].Amount = article.Amount;
                            articles[j].Position = article.Position;
                            articles[j].Ppu = article.Ppu;
                            articles[j].HasPpu = falsearticle.HasPpu;
                            articles[j].TextForArticle_Id = falsearticle.TextForArticle_Id;
                            articles[j].CreditedForArticle_Id = article.CreditedForArticle_Id;
                            return true;
                        }
                    }
                }
            }
        },

        UpdateCharacterCount: function (objectId) {
            var len = $("#Description_" + objectId).val().length;
            var newLen =  (this.MaxDescriptionCount - len).toString() + " " + dhHelpdesk.Common.Translate("Tecken kvar");
            $("#DescriptionCount_" + objectId).text(newLen);
        },

        ValidateAmount: function (curOrder_Id, curArticleId, excludeArticleId, newAmount) {
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

            var totalOrderAmount = this.GetOrderTotalAmount(originalArticles, curArticleId);
            var totalCreditUsed = this.GetCreditUsed(originalOrderId, curArticleId, excludeArticleId);

            var newUsedAmout = totalCreditUsed + parseFloat(newAmount);
            if (totalOrderAmount < newUsedAmout) {

                if (totalOrderAmount - totalCreditUsed > 0) {
                    newAmount = totalOrderAmount - totalCreditUsed;
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Max antal för denna artikel är") + ": " + dhHelpdesk.Math.ConvertDoubleToStr(newAmount, _IGNORE_ZERO_FLOATING_POINT));
                }
                else {
                    newAmount = 0;
                }
            }

            return newAmount;
        },

        ValidateNewCreditAmount: function (originalOrderId, creditOrderId, curArticleId, newAmount) {
            var originalOrder = this.GetOrder(originalOrderId);

            var originalArticles = [];
            if (originalOrder.Articles != undefined)
                originalArticles = originalOrder.Articles;
            else if (originalOrder._articles != undefined)
                originalArticles = originalOrder._articles;

            var totalOrderAmount = this.GetOrderTotalAmount(originalArticles, curArticleId);
            var totalCreditUsed = this.GetCreditUsed(originalOrderId, curArticleId, null);

            var newUsedAmout = totalCreditUsed + parseFloat(newAmount);
            if (totalOrderAmount < newUsedAmout) {
                if (totalOrderAmount - totalCreditUsed > 0) {
                    newAmount = totalOrderAmount - totalCreditUsed;
                    creditAlertToShow = "";
                    creditAlert = false;
                }
                else {
                    newAmount = 0;
                    creditAlertToShow = dhHelpdesk.Common.Translate("Inget kvar att kreditera");
                }
            }
            else {
                creditAlertToShow = "";
                creditAlert = false;
            }

            return newAmount;
        },

        GetOrderTotalAmount: function (articles, articleId) {
            var allOrderAmount = 0;

            for (var j = 0; j < articles.length; j++) {
                var curArticle = articles[j];
                if (curArticle.Id == articleId) {
                    allOrderAmount += (parseFloat(curArticle.Amount));
                    break;
                }
            }
            return allOrderAmount;
        },

        GetCreditUsed: function (originalOrderId, curArticleId, excludeArticleId) {
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
                    if (curArticle.CreditedForArticle_Id == curArticleId && curArticle.Id != excludeArticleId) {
                        creditedAmount += (parseFloat(curArticle.Amount));
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
                    lastRequestedKey = dhHelpdesk.Common.GenerateRandomKey();
                    return $.ajax({
                        url: '/cases/search_user',
                        type: 'post',
                        data: { query: query, customerId: $('#case__Customer_Id').val(), searchKey: lastRequestedKey },
                        dataType: 'json',
                        success: function (result) {
                            if (result.searchKey != lastRequestedKey)
                                return;
                            var resultList = jQuery.map(result.result, function (item) {
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
                           || ~item.email.toLowerCase().indexOf(this.query.toLowerCase())
                           || ~item.usercode.toLowerCase().indexOf(this.query.toLowerCase());
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

                    $('#ReportedBy_' + OrderId).val(item.num).change();
                    $('#Persons_Name_' + OrderId).val(item.name).change();
                    $('#Persons_Email_' + OrderId).val(item.email).change();
                    $('#Persons_Phone_' + OrderId).val(item.phone).change();
                    $('#Persons_CellPhone_' + OrderId).val(item.CellPhone).change();

                    $('#RegionId_' + OrderId).val(item.regionid);
                    $('#RegionSelect-Order' + OrderId).val(item.regionid);
                    $('#RegionSelect-Order' + OrderId).change();

                    $('#DepartmentId_' + OrderId).val(item.departmentid);
                    $('#DepartmentSelect-Order' + OrderId).val(item.departmentid);
                    $('#DepartmentSelect-Order' + OrderId).change();

                    $('#OUId_' + OrderId).val(item.ouid);
                    $('#OUSelect-Order' + OrderId).val(item.ouid);
                    $('#OUSelect-Order' + OrderId).change();

                    $('#Place_' + OrderId).val(item.place).change();
                    $('#UserCode_' + OrderId).val(item.usercode).change();
                    if (item.costcentre != null)
                        $('#CostCentre_' + OrderId).val(item.costcentre.RemoveNonNumerics()).change();
                    else
                        $('#CostCentre_' + OrderId).val("").change();

                    dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(OrderId);

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

        CloseContainer: function () {
            var th = this;
            var invoice = th.GetInvoice();
            th._container = invoice.Container;
            th.CancelChanges();
            $('.case-invoice-container').remove();
        },

        CleanUpAllOrders: function () {
            var cleanOrders = [];
            if (this.allVailableOrders.length == 1)
                return;

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
                if (this.allVailableOrders[i].Id == orderId && this.allVailableOrders[i].OrderState != dhHelpdesk.CaseArticles.OrderStates.Deleted) {
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

        UpdateAvailableOrder: function (order) {
            for (var i = 0; i < this.allVailableOrders.length; i++) {
                if (this.allVailableOrders[i].Id == order.Id) {
                    curOrder = this.allVailableOrders[i];

                    curOrder.ReportedBy = order.ReportedBy;
                    curOrder.Persons_Name = order.Persons_Name;
                    curOrder.Persons_Email = order.Persons_Email;
                    curOrder.Persons_Phone = order.Persons_Phone;
                    curOrder.Persons_CellPhone = order.Persons_CellPhone;
                    curOrder.Region_Id = order.Region_Id;
                    curOrder.Department_Id = order.Department_Id;
                    curOrder.OU_Id = order.OU_Id;
                    curOrder.Place = order.Place;
                    curOrder.UserCode = order.UserCode;
                    curOrder.CostCentre = order.CostCentre;
                    return;
                }
            }
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

            var projectCode = "-";
            if ($('#case__Project_Id') != undefined && $('#case__Project_Id') != null) {
                var projectName = $('#case__Project_Id option:selected').text();
                if (projectName != " ")
                    if (projectName != undefined && !dhHelpdesk.Common.IsNullOrEmpty(projectName)) {
                        var splited = projectName.split(' ');
                        if (splited.length > 0)
                            projectCode = splited[0];
                        else
                            projectCode = projectName;
                    }
            }

            var caseNumber = $("#case__CaseNumber").val();

            th._container.dialog({
                title: dhHelpdesk.Common.Translate("Order") + "  (" +
                       dhHelpdesk.Common.Translate("Ärende") + " " + caseNumber + " / " +
                       dhHelpdesk.Common.Translate("Projekt") + " " + projectCode + ")",
                modal: true,
                width: 1100,
                autoResize: true,
                resizable: false,
                height: "auto",
                width: "90%",
                overflow: "auto",
                zIndex: 1100,
                resize: function (event, ui) {
                    var thisHeight = $(event.target).height();
                    var tabsheight = $('#case-invoice-orders-tabs').height();
                    event.preventDefault();
                },
                close: function () {
                    th.CloseFileDialog();
                    th.CloseInvoiceWindow(th);                    
                },
                buttons: [
                    {
                        text: dhHelpdesk.Common.Translate("Spara"),
                        'data-doInvoiceOrder': '',
                        'class': 'btn save-invoice',
                        click: function () {
                            dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_SAVING);
                            if (!th.Validate()) {
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                                return;
                            }
                            var me = $(".btn.save-invoice");
                            var orderIdToXML = me.attr('data-doInvoiceOrder');
                            me.attr('data-doInvoiceOrder', '');

                            var articlesEl = th._container.find(".articles-params-article");
                            var articleId = articlesEl.val();

                            if (!dhHelpdesk.CaseArticles.ValidateOpenInvoiceWindow(th)) {
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                                return false;
                            }
                            th.CloseFileDialog();
                            th.ApplyChanges();
                            th.SaveToDatabase(th.CloseReOpenWindow, th, orderIdToXML);                            
                        }
                    },
                    {
                        text: dhHelpdesk.Common.Translate("Spara och stäng"),
                        'class': 'btn save-close-invoice',
                        click: function () {
                            dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_SAVING);
                            if (!th.Validate()) {
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                                return;
                            }

                            if (!dhHelpdesk.CaseArticles.ValidateOpenInvoiceWindow(th)) {
                                dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);
                                return false;
                            }
                            th.CloseFileDialog();
                            th.ApplyChanges();
                            th.SaveToDatabase(th.CloseInvoiceWindow, th);                            
                        }
                    },
                    {
                        text: dhHelpdesk.Common.Translate("Avbryt"),
                        'class': 'btn close-invoice',
                        click: function () {
                            th.CloseFileDialog();
                            th.CloseInvoiceWindow(th);                            
                        }
                    }
                ],
                autoOpen: false,
                open: function (event, ui) {
                    //loadCaseFiles();
                    
                    var caseIsLocked = window.parameters.isCaseLocked;                    
                    if (caseIsLocked.toLowerCase() == 'true') {                        
                        $(invoiceActionButtons).addClass("disabled");
                        $(invoiceActionButtons).css("pointer-events", "none");
                    }

                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                    dhHelpdesk.CaseArticles.CleanUpAllOrders();
                    dhHelpdesk.System.RaiseEvent("OnChangeOrder", [th.GetCurrentOrder()]);
                    //if (dhHelpdesk.CaseArticles.IsFinishedCase()) {
                    //TODO: need to enable order tabs and others that we need to active them in Finished Case
                    //}

                    if (th._invoices[0]._orders != null && th._invoices[0]._orders.length > 0) {
                        var hasAtleastOneVisibleOrder = false;
                        for (i = 0; i < th._invoices[0]._orders.length; i++)
                            if (th._invoices[0]._orders[i].OrderState != dhHelpdesk.CaseArticles.OrderStates.Deleted &&
                                th._invoices[0]._orders[i].OrderState != dhHelpdesk.CaseArticles.OrderStates.NotSaved) {
                                hasAtleastOneVisibleOrder = true;
                                break;
                            }

                        if (hasAtleastOneVisibleOrder) {
                            var tabs = $("#case-invoice-orders-tabs");
                            tab = tabs.find("[href='#case-invoice-order-summary" + th._invoices[0].Id + "']");
                            tab.click();
                        }
                    }
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
                        var units = dhHelpdesk.Math.ConvertStrToDouble(unitsEl.val());
                        if (!dhHelpdesk.Math.IsDouble(units) || units <= 0) {
                            units = dhHelpdesk.CaseArticles.DefaultAmount;
                            unitsEl.val(units);
                        }
                        var units_PriceEl = th._container.find(".articles-params-units-price");
                        var units_Price = dhHelpdesk.Math.ConvertStrToDouble(units_PriceEl.val(), _IGNORE_ZERO_FLOATING_POINT);
                        if (isNaN(units_Price))
                            units_Price = 0;

                        var articlesEl = th._container.find(".articles-params-article");
                        var articleId = articlesEl.val();
                        var article = th.GetInvoiceArticle(articleId);
                        var elementForFocus = null;
                        if (article != null) {
                            var currentOrder = th.GetCurrentOrder();
                            var curAmount = parseFloat(units);
                            if (article.HasChildren()) {
                                for (var i = 0; i < article.Children.length; i++) {
                                    var child = article.Children[i].ToCaseArticle();
                                    child.Amount = units;

                                    // Validate Amount
                                    if (currentOrder.CreditForOrder_Id != null && units != null) {
                                        child.Order = currentOrder;
                                        var validatedAmount = dhHelpdesk.CaseArticles.ValidateAmount(child.Order.Id, child.CreditedForArticle_Id, null, units);
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
                                        var addedElement = dhHelpdesk.CaseArticles.AddBlankArticle(currentOrder.Id, child.Id);
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
                                    var validatedAmount = dhHelpdesk.CaseArticles.ValidateAmount(caseArticle.Order.Id, caseArticle.CreditedForArticle_Id, null, units);
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
                                    var addedElement = dhHelpdesk.CaseArticles.AddBlankArticle(currentOrder.Id, caseArticle.Id);
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

        ValidateOpenInvoiceWindow: function (invoice) {
            var articlesEl = invoice._container.find(".articles-params-article");
            var articleId = articlesEl.val();
            if (articleId != 0) {
                dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Du har valt en artikel, men inte lagt till den än!"));
                return false;
            }
            for (var i = 0; i < this.allVailableOrders.length; i++) {
                var order = dhHelpdesk.CaseArticles.GetOrder(this.allVailableOrders[i].Id);
                /* Deleted orders will be null */
                if (order != null && !order.InvoiceValidate())
                    return false;
            }
            return true;
        },

        CloseInvoiceWindow: function (that) {            
            dhHelpdesk.CaseArticles.LastSelectedTab = null;
            dhHelpdesk.CaseArticles.LastSelectedTabCaption = "";
            caseButtonsToDisable.removeClass('disabled');
            caseButtonsToDisable.css("pointer-events", "");
            that.CloseContainer();
        },

        CloseReOpenWindow: function (that) {            
            caseButtonsToDisable.removeClass('disabled');
            caseButtonsToDisable.css("pointer-events", "");
            that.CloseContainer();
            $('#InvoiceModuleBtnOpen').click();
        },

        CloseFileDialog: function () {
            var allOpenedDialogs = $(document).find(".selectFileDialog");
            $.each(allOpenedDialogs, function () {
                $(this).dialog("close");
            });
        },

        OpenInvoiceWindow: function (message) {
            var articleDescriptionDelimiter = '¤';

            var th = dhHelpdesk.CaseArticles;
            th.CreateContainer(message);
            var addArticleEl = th._container.find(".articles-params");
            var articlesSelectContainer = addArticleEl.find(".articles-select-container");
            articlesSelectContainer.empty();
            var articlesEl = $("<select id='articleList' class='chosen-select articles-params-article'></select>");
            articlesSelectContainer.append(articlesEl);

            var articles = th.GetInvoiceArticles();
            
            if (articles == null || articles.length == 0) {
                addArticleEl.hide();
            } else {
                addArticleEl.show();
            }
           
            resetTotal();
            function resetTotal() {
                articlesEl.empty();
                articlesEl.append("<option value='0'>  </option>");
                articlesEl.append("<option value='0'> &nbsp; </option>");
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    var artDesc = article.Description != null ? article.Description : "";

                    var text = article.GetFullName(); 
                    if (artDesc != "")
                        text = artDesc + articleDescriptionDelimiter + text;

                    articlesEl.append("<option value='" + article.Id + "' >" + text + "</option>");
                }                
            }

            $('.InitiatorField').hide();

            articlesEl.chosen({
                width: "500px",
                height: "100px",
                placeholder_text_single: dhHelpdesk.Common.Translate("Välj artikel"),
                search_contains:true,
                'no_results_text': '?',
            })
            .change(function () {                
                var selectedArticle = dhHelpdesk.CaseArticles.GetInvoiceArticle(articlesEl.chosen().val());
                var units_PriceEl = th._container.find(".articles-params-units-price");
                var t = ''
                if (selectedArticle != null) {
                   t = selectedArticle.GetFullName();
                }
                $('#articleList_chosen a span').text(t);

                if (selectedArticle != null) {
                    if (selectedArticle.HasChildren()) {
                        units_PriceEl.val('');
                        units_PriceEl.prop("readonly", false);
                        units_PriceEl.prop("disabled", false);

                    }
                    else {
                        if (!selectedArticle.HasPpu) {
                            units_PriceEl.val('');
                            units_PriceEl.prop("readonly", false);
                            units_PriceEl.prop("disabled", false);
                        }
                        else {
                            units_PriceEl.val(selectedArticle.Ppu);
                            units_PriceEl.prop("readonly", true);
                            units_PriceEl.prop("disabled", true);
                        }
                    }
                }
            });
          
            var lastSearchKey = 0;
            function resetResultSearch(searchId) {
                if (searchId != lastSearchKey) {
                    return;
                }

                resetTotal();
                $('#articleList_chosen .chosen-results li').each(function () {
                    var art = this;
                    var fullText = $(art).text();
                    if (fullText != " ") {
                        var newText = '  ';
                        var tooltip = '';
                        var splits = fullText.split(articleDescriptionDelimiter);
                        if (splits.length > 0 && splits[0] != 'null') {
                            tooltip = splits[0];
                        }

                        if (splits.length > 1) {
                            newText = splits[1];
                        }

                        if (newText == ' ' || newText == '  ')
                            newText = fullText;

                        art.title = tooltip;
                        $(this).text(newText);
                    }
                });
            }
            
            $('#articleList').on('chosen:showing_dropdown', function () {
                $('.chosen-search input').on('input', function () {                                       
                    lastSearchKey += 1;
                    setTimeout(resetResultSearch, 200, lastSearchKey);
                });                
                lastSearchKey += 1;
                setTimeout(resetResultSearch, 200, lastSearchKey);
            });

            articlesSelectContainer.find("select.chosen-select").addClass("min-width-500 max-width-500 case-invoice-multiselect");

            caseButtonsToDisable.addClass("disabled");
            caseButtonsToDisable.css("pointer-events", "none");
            th._container.dialog("open");

            if (th.IsFinishedCase()) {
                $('.case-invoice-container *').attr("disabled", true);
                $('#actions-credit-order').attr("disabled", true);
            }
            th.FillInvoiceOrganisationData();
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
            this.invoiceButtonPureCaption = ButtonCaption;

            var ButtonCaptionExtra = e.attr("data-invoice-CaptionExtra");
            var ButtonHint = e.attr("data-invoice-Hint");

            var button = $(document.createElement("input"))
                .attr("id", "InvoiceModuleBtnOpen")
                .attr("type", "button")
                .attr("value", ButtonCaption + " " + ButtonCaptionExtra)
                .attr("title", ButtonHint)
                .addClass("btn");

            button.click(function () {
                if (th.IsNewCase() || th.IsProductAreaChanged() || th.IsProjectChanged() || th.IsAnyFileAdded()) {
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Var vänlig spara ärendet och försök igen!"));
                    return;
                }

                th.OpenInvoiceWindow();
            });
            e.after(button);

            var onChangeProductArea = function () {

                button.hide();
                invoiceButtonIndicator.show();
                $.get("/Caseinvoice/articles/",
                    {
                        caseId: th.CaseId,
                        productAreaId: th.ProductAreaElement.val(),
                        myTime: Date.now
                    }, function (data) {
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

        SortArticles: function (articles) {
            return articles.sort(function (a1, a2) { return a1.Position - a2.Position; });
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

        GetInvoicedOrders: function (allOrders) {
            var ret = [];
            for (var i = 0; i < allOrders.length; i++) {
                if (allOrders[i].OrderState == this.OrderStates.Sent)
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
                article.TextForArticle_Id = null;
                article.CreditedForArticle_Id = null;

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
                if (order.OrderState == dhHelpdesk.CaseArticles.OrderStates.Deleted) {
                    return
                }
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

                // Add First order
                if (order.Id < 0 && dhHelpdesk.CaseArticles.allVailableOrders.length == 0) {
                    dhHelpdesk.CaseArticles.allVailableOrders.push(order);
                    order.Number = dhHelpdesk.CaseArticles.allVailableOrders.length;
                }

                this._orders.push(order);

                var container = this.Container.find(".orders-container");
                container.append(order.Container);

                var tabs = this.Container.find("#case-invoice-orders-tabs");
                var tabIcon = (order.IsOrderInvoiced() ? "<i class='icon-ok icon-green'></i>&nbsp;&nbsp;" : "");
                var newTab = $("<li class='case-invoice-order-tab active' ><a href='#case-invoice-order" + order.Id + "'>" + tabIcon + order.Caption + "</a><li>");
                tabs.find("li.case-invoice-order-addnew:last").before(newTab);

                this._refreshTabs();
                dhHelpdesk.CaseArticles.FillOrderOrganizationData(order);

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
                dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(order.Id);
                dhHelpdesk.CaseArticles.CollapseOtherReference(order.Id);
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
                model.Total = dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(totals.Total, _IGNORE_ZERO_FLOATING_POINT));
                model.TotalInvoiced = dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(totals.Invoiced, _IGNORE_ZERO_FLOATING_POINT));
                model.TotalNotInvoiced = dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(totals.NotInvoiced, _IGNORE_ZERO_FLOATING_POINT));

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

                dhHelpdesk.System.OnEvent("OnDeleteArticle", function (e, order, deletedArticleIds) {
                    if (th.HasArticles()) {
                        th.ShowSummary();
                    } else {
                        th.HideSummary();
                    }
                    for (i = 0; i < deletedArticleIds.length; i++)
                        dhHelpdesk.CaseArticles.UpdateAvailableArticles(order.Id, deletedArticleIds[i], false, null);
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
                        addArticleEl.hide();

                    } else {
                        dhHelpdesk.CaseArticles.OrderActionsInstance.get_creditOrderButton(currentOrder.Id).hide();
                        $("#doInvoiceButton_" + currentOrder.Id).show();
                        var articles = dhHelpdesk.CaseArticles.GetInvoiceArticles();
                        if (articles == null || articles.length == 0 || currentOrder.CreditForOrder_Id != null)
                            addArticleEl.hide();
                        else {
                            dhHelpdesk.CaseArticles.ResetAddArticlePlace();
                            addArticleEl.show();
                        }
                    }

                    if (currentOrder.IsOrderInvoiced() || currentOrder.CreditForOrder_Id != null) {
                        var allInitiatorFields = $(".InitiatorFields_" + currentOrder.Id);
                        $.each(allInitiatorFields, function (i) {
                            $(allInitiatorFields[i]).attr("disabled", true);
                        });
                        $("#invoiceSelectFile_" + currentOrder.Id).attr("disabled", true);
                    }

                    dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(currentOrder.Id);
                    dhHelpdesk.CaseArticles.UpdateFileAttachment(currentOrder);
                    var caseIsLocked = window.parameters.isCaseLocked;
                    if (caseIsLocked.toLowerCase() == 'true') {
                        $(invoiceActionButtons).addClass("disabled");
                        $(invoiceActionButtons).css("pointer-events", "none");
                    }
                });
            },

            this.GetArticlesTotal = function () {
                var total = { Invoiced: 0, NotInvoiced: 0, Total: 0 };
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var t = orders[i].GetArticlesTotal();
                    if (orders[i].CreditForOrder_Id == null) {
                        total.Invoiced += (t.Invoiced);
                        total.NotInvoiced += (t.NotInvoiced);
                        //total.Total += (t.Total);
                    }
                    else {
                        total.Invoiced -= (t.Invoiced);
                        total.NotInvoiced -= (t.NotInvoiced);
                        //total.Total -= (t.Total);
                    }
                }
                total.Total = (total.Invoiced + total.NotInvoiced).toFixed(_MAX_FLOATING_POINT);
                return total;
            },

            this.UpdateTotal = function () {

            },

            this.Validate = function () {
                var isValid = true;
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    var order = orders[i];
                    var orderValidate = order.Validate();
                    if (!orderValidate.IsValid) {
                        dhHelpdesk.Common.ShowErrorMessage(order.Caption + " " + dhHelpdesk.Common.Translate("kunde inte sparas då det saknas data i ett eller flera obligatoriska fält. Var vänlig kontrollera i ordern.") + orderValidate.Message);
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
            this.OrderState = null;
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
                return this.OrderState == dhHelpdesk.CaseArticles.OrderStates.Sent;
            },

            this.AddArticle = function (article) {
                article.Order = this;
                this._articles.push(article);
                article.Position = this.ReOrderingArticles(article.Id);

                var rows = this.Container.find(".articles-rows");
                rows.html('');
                for (ai = 0; ai < this._articles.length; ai++) {
                    this._articles[ai].Initialize();
                    rows.append(this._articles[ai].Container);
                }

                var mainArticleId = 0;
                if (article.TextForArticle_Id != null)
                    mainArticleId = article.TextForArticle_Id;
                else
                    mainArticleId = article.Id;

                this.UpdateTotal();

                dhHelpdesk.System.RaiseEvent("OnAddArticle", [this, article]);
            },

            this.ReOrderingArticles = function (articleId) {
                var newSortedArticle = [];
                var curPos = 0;
                var onlyArticles = this.GetOnlyArticles();
                var ret = 0;
                for (i = 0; i < onlyArticles.length; i++) {
                    var onlyTextes = this.GetTextsForArticle(onlyArticles[i].Id);
                    onlyArticles[i].Position = curPos;
                    curPos++;
                    newSortedArticle.push(onlyArticles[i]);
                    if (onlyArticles[i].Id == articleId)
                        ret = onlyArticles[i].Position;

                    for (j = 0; j < onlyTextes.length; j++) {
                        onlyTextes[j].Position = curPos;
                        curPos++;
                        newSortedArticle.push(onlyTextes[j]);
                        if (onlyTextes[j].Id == articleId)
                            ret = onlyTextes[j].Position;
                    }
                }
                this._articles = newSortedArticle;
                return ret;
            },

            this.GetOnlyArticles = function () {
                var onlyArticles = [];
                for (i = 0; i < this._articles.length; i++) {
                    if (!this._articles[i].IsBlank())
                        onlyArticles.push(this._articles[i]);
                }

                return onlyArticles;
            },

            this.GetTextsForArticle = function (articleId) {
                var articleTextes = [];
                for (t = 0; t < this._articles.length; t++) {
                    if (this._articles[t].TextForArticle_Id == articleId && this._articles[t].IsBlank())
                        articleTextes.push(this._articles[t]);
                }

                return articleTextes;
            },

            this.DeleteArticle = function (id) {
                var articlesToDelete = [];
                var mainArticleId = 0;
                var articleToDelete = this.GetArticle(id);
                if (articleToDelete != null) {
                    if (articleToDelete.TextForArticle_Id == null) {
                        mainArticleId = id;
                        articlesToDelete.push(mainArticleId);
                        var texts = this.GetTextsForArticle(id);
                        for (var i = 0; i < texts.length; i++) {
                            articlesToDelete.push(texts[i].Id);
                        }
                    } else {
                        mainArticleId = articleToDelete.TextForArticle_Id;
                        articlesToDelete.push(id);
                    }

                    for (var i = 0; i < articlesToDelete.length; i++) {
                        for (var j = 0; j < this._articles.length; j++) {
                            var article = this._articles[j];
                            if (article.Id == articlesToDelete[i]) {
                                this._articles.splice(j, 1);
                                this._deleteFromContainer(article.Id);
                                break;
                            }
                        }
                    }

                    this.ReOrderingArticles(0);
                } else {
                    return;
                }

                this.UpdateTotal();
                dhHelpdesk.System.RaiseEvent("OnDeleteArticle", [this, articlesToDelete]);
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
                        '"Number":"' + dhHelpdesk.Common.EncodeStrToJson(this.Number) + '", ' +
                        '"InvoiceDate":"' + dhHelpdesk.Common.DateToJSONDateTime(this.InvoiceDate) + '", ' +
                        '"InvoicedByUser":"' + (this.InvoicedByUser != null ? dhHelpdesk.Common.EncodeStrToJson(this.InvoicedByUser) : '') + '", ' +
                        '"InvoicedByUserId":"' + (this.InvoicedByUserId != null ? dhHelpdesk.Common.EncodeStrToJson(this.InvoicedByUserId) : '') + '", ' +
                        '"ReportedBy":"' + (this.ReportedBy != null ? dhHelpdesk.Common.EncodeStrToJson(this.ReportedBy) : '') + '", ' +
                        '"Persons_Name":"' + (this.Persons_Name != null ? dhHelpdesk.Common.EncodeStrToJson(this.Persons_Name) : '') + '", ' +
                        '"Persons_Email":"' + (this.Persons_Email != null ? dhHelpdesk.Common.EncodeStrToJson(this.Persons_Email) : '') + '", ' +
                        '"Persons_Phone":"' + (this.Persons_Phone != null ? dhHelpdesk.Common.EncodeStrToJson(this.Persons_Phone) : '') + '", ' +
                        '"Persons_CellPhone":"' + (this.Persons_CellPhone != null ? dhHelpdesk.Common.EncodeStrToJson(this.Persons_CellPhone) : '') + '", ' +
                        '"Region_Id":"' + (this.Region_Id != null ? this.Region_Id : '') + '", ' +
                        '"Department_Id":"' + (this.Department_Id != null ? this.Department_Id : '') + '", ' +
                        '"OU_Id":"' + (this.OU_Id != null ? this.OU_Id : '') + '", ' +
                        '"CostCentre":"' + (this.CostCentre != null ? dhHelpdesk.Common.EncodeStrToJson(this.CostCentre) : '') + '", ' +
                        '"Place":"' + (this.Place != null ? dhHelpdesk.Common.EncodeStrToJson(this.Place) : '') + '", ' +
                        '"UserCode":"' + (this.UserCode != null ? dhHelpdesk.Common.EncodeStrToJson(this.UserCode) : '') + '", ' +
                        '"CreditForOrder_Id":"' + (this.CreditForOrder_Id != null ? this.CreditForOrder_Id : '') + '", ' +
                        '"Project_Id":"' + (this.Project_Id != null ? dhHelpdesk.Common.EncodeStrToJson(this.Project_Id) : '') + '", ' +
                        '"OrderState":"' + (this.OrderState != null ? this.OrderState : '0') + '", ' +
                        '"Articles": [' + articlesResult + '],' +
                        '"Files": [' + filesResult + ']' +
                        '}';
            },

            this.GetArticlesTotal = function () {
                var total = { Invoiced: 0, NotInvoiced: 0, Total: 0 };
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    var curTotal = articles[i].GetTotal();
                    if (articles[i].Order.IsOrderInvoiced())
                        total.Invoiced += curTotal;
                    else
                        total.NotInvoiced += curTotal;

                    total.Total += curTotal;
                }
                return total;
            },

            this.IsArticlesEmpty = function () {
                return this._articles.length == 0;
            },

            this.DoInvoice = function () {
                var btnSave = $(".btn.save-invoice");
                //pass order_id to generate xml for order

                var articlesEl = $(document).find(".articles-params-article");
                var articleId = articlesEl.val();
                if (articleId != 0) {
                    dhHelpdesk.Common.ShowWarningMessage(dhHelpdesk.Common.Translate("Du har valt en artikel, men inte lagt till den än!"));
                    return false;
                }

                if (this._articles.length > 0) {
                    btnSave.attr("data-doInvoiceOrder", this.Id);
                    btnSave.click();
                    dhHelpdesk.System.RaiseEvent("OnChangeOrder", [this]);
                }
                else {
                    dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Det finns inga artiklar att skicka!"));
                }
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
                clone.OrderState = this.OrderState;
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
                creditAlertToShow = "";
                creditAlert = true;

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

                var onlyArticles = this.GetOnlyArticles();
                for (var oi = 0; oi < onlyArticles.length; oi++) {
                    var a = onlyArticles[oi];
                    var article = a.GetCreditedArticle(credited);
                    if (article.Amount > 0) {
                        credited.AddArticle(article);
                        dhHelpdesk.CaseArticles.RefreshArticleData(article);
                        var onlyTexts = this.GetTextsForArticle(a.Id);
                        for (tj = 0; tj < onlyTexts.length; tj++) {
                            var artText = onlyTexts[tj];
                            var textRow = artText.GetCreditedArticle(credited);
                            textRow.TextForArticle_Id = article.Id;
                            credited.AddArticle(textRow);
                            dhHelpdesk.CaseArticles.RefreshArticleData(textRow);
                        }
                    }
                }

                if (creditAlertToShow != "" && creditAlert) {
                    dhHelpdesk.Common.ShowWarningMessage(creditAlertToShow);
                    creditAlertToShow = "";
                }

                return credited;
            },

            this.GetViewModel = function () {
                var model = new dhHelpdesk.CaseArticles.CaseInvoiceOrderViewModel();
                model.Id = this.Id != null ? this.Id : dhHelpdesk.Common.GenerateId();
                model.Number = this.Number + 1;

                var totalPrices = this.GetArticlesTotal();
                model.Total = dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(totalPrices.Total, _IGNORE_ZERO_FLOATING_POINT));

                model.InvoiceDate = this.InvoiceDate != null && this.InvoiceDate != "" ? dhHelpdesk.Common.DateToDisplayDate(this.InvoiceDate) : "";
                model.InvoiceTime = this.InvoiceDate != null && this.InvoiceDate != "" ? dhHelpdesk.Common.DateToDisplayTime(this.InvoiceDate) : "";
                model.InvoicedByUser = this.InvoicedByUser != null ? this.InvoicedByUser : "";
                model.InvoicedByUserId = this.InvoicedByUserId != null ? this.InvoicedByUserId : "";
                model.IsInvoiced = this.IsOrderInvoiced();
                model.IsCredited = this.CreditForOrder_Id != null;
                model.InvoicedByTitle = dhHelpdesk.Common.Translate("Skickat av");
                ////////initiator field

                model.ReportedBy = this.ReportedBy != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.ReportedBy) : "";
                model.ShowReportedBy = dhHelpdesk.CaseArticles.ShowCaseField('ReportedBy');
                model.RequiredReportedBy = dhHelpdesk.CaseArticles.OtherReferenceRequired.ReportedBy;

                model.Persons_Name = this.Persons_Name != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.Persons_Name) : "";
                model.ShowPersons_Name = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Name');
                model.RequiredPersons_Name = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_Name;

                model.Persons_Email = this.Persons_Email != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.Persons_Email) : "";
                model.ShowPersons_Email = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Email');
                model.RequiredPersons_Email = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_Email;

                model.Persons_Phone = this.Persons_Phone != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.Persons_Phone) : "";
                model.ShowPersons_Phone = dhHelpdesk.CaseArticles.ShowCaseField('Persons_Phone');
                model.RequiredPersons_Phone = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_Phone;

                model.Persons_CellPhone = this.Persons_CellPhone != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.Persons_CellPhone) : "";
                model.ShowPersons_CellPhone = dhHelpdesk.CaseArticles.ShowCaseField('Persons_CellPhone');
                model.RequiredPersons_CellPhone = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_CellPhone;

                model.Region_Id = this.Region_Id != null ? this.Region_Id : "";
                model.ShowRegion_Id = dhHelpdesk.CaseArticles.ShowCaseField('Region_Id');
                model.RequiredRegion_Id = dhHelpdesk.CaseArticles.OtherReferenceRequired.Region_Id;

                model.Department_Id = this.Department_Id != null ? this.Department_Id : "";
                model.ShowDepartment_Id = dhHelpdesk.CaseArticles.ShowCaseField('Department_Id');
                model.RequiredDepartment_Id = dhHelpdesk.CaseArticles.OtherReferenceRequired.Department_Id;

                model.OU_Id = this.OU_Id != null ? this.OU_Id : "";
                model.ShowOU_Id = dhHelpdesk.CaseArticles.ShowCaseField('OU_Id');
                model.RequiredOU_Id = dhHelpdesk.CaseArticles.OtherReferenceRequired.OU_Id;

                model.Place = this.Place != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.Place) : "";
                model.ShowPlace = dhHelpdesk.CaseArticles.ShowCaseField('Place');
                model.RequiredPlace = dhHelpdesk.CaseArticles.OtherReferenceRequired.Place;

                model.UserCode = this.UserCode != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.UserCode) : "";
                model.ShowUserCode = dhHelpdesk.CaseArticles.ShowCaseField('UserCode');
                model.RequiredUserCode = dhHelpdesk.CaseArticles.OtherReferenceRequired.UserCode;

                
                model.CostCentre = this.CostCentre != null ? this.CostCentre : "";
                model.ShowCostCentre = dhHelpdesk.CaseArticles.ShowCaseField('CostCentre');
                model.RequiredCostCentre = dhHelpdesk.CaseArticles.OtherReferenceRequired.CostCentre;

                model.TitleCaption = this.Caption;

                var articles = this.GetSortedArticles();
                for (var i = 0; i < articles.length; i++) {
                    model.AddArticle(articles[i].GetViewModel());
                }
                var allFiles = this.files.getFiles();
                var strFiles = '';
                for (var af = 0; af < allFiles.length; af++)
                    if (strFiles == '')
                        strFiles += allFiles[af].getFileName();
                    else
                        strFiles += '&nbsp;&nbsp;<b>|</b>&nbsp;&nbsp;' + allFiles[af].getFileName();
                
                model.files = allFiles; //this.files.getFiles(model.IsInvoiced);
                model.FileNameStr = strFiles;

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
                if (!th.IsInvoiced) {
                    /* Disable sortable rows */
                    //rows.sortable({
                    //    stop: function () {
                    //        var newFirstArticle = th.GetArticle(rows.find("tr").first().attr("data-id"));
                    //        if (newFirstArticle.IsBlank()) {
                    //            rows.sortable("cancel");
                    //            return;
                    //        }
                    //        var i = 0;
                    //        rows.find("tr").each(function () {
                    //            var article = th.GetArticle($(this).attr("data-id"));
                    //            if (article != null) {
                    //                article.Position = i;
                    //            }
                    //            i++;
                    //        });
                    //        th.GetArticles().sort(function (a, b) { return a.Position - b.Position; });
                    //    }
                    //});

                    var publicClassName = ".InitiatorFields_" + th.Id;

                    var ReportedBy = this.Container.find(publicClassName + ".reportedby");
                    ReportedBy.typeahead(dhHelpdesk.CaseArticles.GetUserSearchOptions());
                    ReportedBy.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.ReportedBy = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                        dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(curOrderId);
                    });

                    var personsName = this.Container.find(publicClassName + ".name");
                    personsName.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.Persons_Name = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                        dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(curOrderId);
                    });

                    var email = this.Container.find(publicClassName + ".email");
                    email.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.Persons_Email = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                    });

                    var phone = this.Container.find(publicClassName + ".phone");
                    phone.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.Persons_Phone = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                    });

                    var CellPhone = this.Container.find(publicClassName + ".cellphone");
                    CellPhone.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.Persons_CellPhone = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                    });


                    var region = this.Container.find(publicClassName + ".region");
                    region.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        var curOrd = dhHelpdesk.CaseArticles.GetOrder(curOrderId);
                        if (!curOrd.IsOrderInvoiced()) {
                            $(this).parent().find('input:hidden').val($(this).val());
                            th.Region_Id = $(this).val();
                            $('#CostCentre_' + curOrderId).val('');
                            th.CostCentre = '';
                            dhHelpdesk.CaseArticles.FillOrderDepartment(curOrd, th.Region_Id, true);
                        }
                        dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(curOrd.Id);
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                    });

                    var department = this.Container.find(publicClassName + ".department");
                    department.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        var curOrd = dhHelpdesk.CaseArticles.GetOrder(curOrderId);
                        if (!curOrd.IsOrderInvoiced()) {
                            $(this).parent().find('input:hidden').val($(this).val());
                            th.Department_Id = $(this).val();
                            $('#CostCentre_' + curOrderId).val('');
                            th.CostCentre = '';
                            dhHelpdesk.CaseArticles.FillOrderOU(curOrd, th.Department_Id, true);
                        }
                        dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(curOrd.Id);
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                        dhHelpdesk.CaseArticles.SetSendOrderForDepartment(curOrd.Id, th.Department_Id);
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
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                    });

                    var place = this.Container.find(publicClassName + ".place");
                    place.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.Place = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                    });

                    var costcentre = this.Container.find(publicClassName + ".costcentre");
                    costcentre.on("keypress keyup blur", function (event) {
                        if (event.which == 37 || event.which == 38 || event.which == 39 || event.which == 40 ||
                            event.which == 9 || (event.shiftKey && event.which == 9) || (event.which >= 48 && event.which <= 57))
                            return;
                        event.preventDefault();
                    });

                    costcentre.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.CostCentre = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                        dhHelpdesk.CaseArticles.UpdateOtherReferenceTitle(curOrderId);
                    });

                    var usercode = this.Container.find(publicClassName + ".usercode");
                    usercode.change(function () {
                        var curOrderId = $(this).attr('data-orderid');
                        th.UserCode = $(this).val();
                        dhHelpdesk.CaseArticles.UpdateAvailableOrder(th);
                    });
                }
            },

            this.UpdateTotal = function () {
                this.Container.find(".articles-total").text(dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(this.GetArticlesTotal().Total, _IGNORE_ZERO_FLOATING_POINT)));
                if (this.Invoice != null) {
                    this.Invoice.UpdateTotal();
                }
            },

            this._deleteFromContainer = function (id) {
                this.Container.find("[data-id='" + id + "']").remove();
            },

            this.Validate = function () {
                var ret = { IsValid: true, Message: "" };
                if (this.OrderState == dhHelpdesk.CaseArticles.OrderStates.Sent) {
                    return ret;
                }
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
                var errorMessage = '';               

                if (this.IsInvoiced)
                    return true;
                
                var requireds = dhHelpdesk.CaseArticles.OtherReferenceRequired;

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.ReportedBy) && requireds.ReportedBy) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("ReportedBy") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.Persons_Name) && requireds.Persons_Name) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("Persons_Name") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.Persons_Email) && requireds.Persons_Email) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("Persons_Email") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.Persons_Phone) && requireds.Persons_Phone) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("Persons_Phone") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.Persons_CellPhone) && requireds.Persons_CellPhone) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("Persons_CellPhone") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && this.Region_Id <= 0 && requireds.Region_Id) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("Region_Id") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && this.Department_Id <= 0  && requireds.Department_Id) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("Department_Id") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }                
                
                if (this._articles != null && this._articles.length > 0 && this.OU_Id <= 0 && requireds.OU_Id) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("OU_Id") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.CostCentre) && requireds.CostCentre) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("CostCentre") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.Place) && requireds.Place) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("Place") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

                if (this._articles != null && this._articles.length > 0 && dhHelpdesk.Common.IsNullOrEmpty(this.UserCode) && requireds.UserCode) {
                    errorMessage += dhHelpdesk.Common.TranslateCaseFields("UserCode") + " " + dhHelpdesk.Common.Translate("saknas!") + " - " + this.Caption + " <br/> ";
                    isValid = false;
                }

               
                if (!isValid)
                    dhHelpdesk.Common.ShowErrorMessage(errorMessage);

                return isValid;
            },

            this.UpdateFiles = function () {
                var model = dhHelpdesk.CaseArticles.CaseFilesViewModel({
                    caseId: dhHelpdesk.CaseArticles.CaseFiles.getCaseId(),
                    files: this.files.getFiles(),
                    isInvoiced: this.IsOrderInvoiced()
                });

                var files = this.files.getFiles();
                dhHelpdesk.CaseArticles.UpdateFileAttachmentTitle(this.Id, files.length);
                for (var fi = 0; fi < dhHelpdesk.CaseArticles.allVailableOrders.length; fi++)
                    if (dhHelpdesk.CaseArticles.allVailableOrders[fi].Id == this.Id) {
                        dhHelpdesk.CaseArticles.allVailableOrders[fi].Files = files;
                        break;
                    }

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
                                text: dhHelpdesk.Common.Translate("Avbryt"),                                
                                click: function () {
                                    d.dialog("close");
                                },
                                class: "btn"
                            }
                        ]
                    });
                    d.dialog("open");
                } else {
                    dhHelpdesk.Common.ShowErrorMessage(dhHelpdesk.Common.Translate("Det finns inga valbara filer att lägga till på ordern."));
                }
            }
        },

        ShowCaseField: function (CaseFieldName) {
            var CaseFieldSettingsLength = CaseFieldSettings.Result.length;
            for (var i = 0; i < CaseFieldSettingsLength; i++) {
                if (CaseFieldSettings.Result[i].Name.toLowerCase() === CaseFieldName.toLowerCase()) {
                    if (CaseFieldSettings.Result[i].Show != 0) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            return false;
        },

        RequiredCaseField: function (caseFieldName) {
            var CaseFieldSettingsLength = CaseFieldSettings.Result.length;
            for (var i = 0; i < CaseFieldSettingsLength; i++) {
                if (CaseFieldSettings.Result[i].Name.toLowerCase() === caseFieldName.toLowerCase()) {
                    if (CaseFieldSettings.Result[i].Required != 0) {
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
            this.TextForArticle_Id = null;
            this.CreditedForArticle_Id = null;

            this.Container = null;

            this.ToJson = function () {
                return '{' +
                        '"Id":"' + (this.Id != null ? this.Id : 0) + '", ' +
                        '"OrderId":"' + (this.Order.Id > 0 ? this.Order.Id : 0) + '", ' +
                        '"ArticleId":"' + (this.Article != null && this.Article.Id > 0 ? this.Article.Id : '') + '", ' +
                        '"Number":"' + this.GetNumber() + '", ' +
                        '"Name":"' + (this.Name != null ? dhHelpdesk.Common.EncodeStrToJson(this.Name) : '') + '", ' +
                        '"Amount":"' + (this.Amount != null && this.Amount != undefined && !this.IsBlank() ? dhHelpdesk.Math.ConvertStrToDouble(this.Amount) : '') + '", ' +
                        '"Ppu":"' + (this.Ppu != null && this.Ppu != undefined ? dhHelpdesk.Math.ConvertStrToDouble(this.Ppu) : '') + '", ' +
                        '"Position":"' + this.Position + '", ' +
                        '"CreditedForArticle_Id":"' + (this.CreditedForArticle_Id != null && !this.IsBlank() ? this.CreditedForArticle_Id : '') + '", ' +
                        '"TextForArticle_Id":"' + (this.TextForArticle_Id != null ? this.TextForArticle_Id : '') + '"' +
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
                clone.TextForArticle_Id = this.TextForArticle_Id;
                clone.CreditedForArticle_Id = this.CreditedForArticle_Id;

                return clone;
            };

            this.GetCreditedArticle = function (newCreditedOrder) {
                var article = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                article.Id = dhHelpdesk.Common.GenerateId();
                article.CreditedForArticle_Id = this.Id;

                article.Article = this.Article;
                article.Name = this.Name;
                article.Amount = this.Amount;
                article.Position = this.Position;
                article.Ppu = this.Ppu;
                article.HasPpu = this.HasPpu;
                article.TextForArticle_Id = this.TextForArticle_Id;

                if (article.Article != null) {
                    usedAmounts = 0;
                    if (newCreditedOrder.CreditForOrder_Id != null) {
                        var validatedAmount = dhHelpdesk.CaseArticles.ValidateNewCreditAmount(newCreditedOrder.CreditForOrder_Id,
                                                                                              newCreditedOrder.Id, article.CreditedForArticle_Id, this.Amount);
                        article.Amount = validatedAmount;
                    }
                }

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
                    var ppu = this.GetPpu();
                    var res = dhHelpdesk.Math.ConvertStrToDouble(this.Amount) * dhHelpdesk.Math.ConvertStrToDouble(ppu);
                    var res = res.toFixed(_MAX_FLOATING_POINT);
                    return (dhHelpdesk.Math.ConvertStrToDouble(res));
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
                this.HasPpu = (this.Article != null && this.Article.Ppu != null && this.Article.Ppu > 0);
                model.IsBlank = this.IsBlank();
                model.Name = this.Name != null ? dhHelpdesk.Common.HtmlQuotationUpdate(this.Name.replace(/\</g, "&lt;")) : "";
                model.NameEng = this.Article != null && this.Article.NameEng != null ? this.Article.NameEng : "";
                model.Description = this.Article != null && this.Article.Description != null ? this.Article.Description : "";
                model.Id = this.Id;
                model.Number = this.GetNumber();
                model.Amount = this.Order.IsInvoiced ? dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(this.Amount, true)) :
                                                       dhHelpdesk.Math.ConvertDoubleToStr(this.Amount, true);

                model.DisplayAmount = dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(this.Amount, true));
                model.UnitName = this.GetUnitName();
                model.Ppu = this.Order.IsInvoiced ? dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(this.GetPpu(), _IGNORE_ZERO_FLOATING_POINT)) :
                                                    dhHelpdesk.Math.ConvertDoubleToStr(this.GetPpu(), _IGNORE_ZERO_FLOATING_POINT);

                model.DisplayPpu = dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(this.GetPpu(), _IGNORE_ZERO_FLOATING_POINT));
                model.Total = dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(this.GetTotal(), _IGNORE_ZERO_FLOATING_POINT));
                model.IsArticlePpuExists = this.IsArticlePpuExists();
                model.IsCredited = this.Order.CreditForOrder_Id != null;
                model.IsInvoiced = this.Order.IsInvoiced;
                model.DescriptionCount = (dhHelpdesk.CaseArticles.MaxDescriptionCount - model.Name.length) + " " + dhHelpdesk.Common.Translate("Tecken kvar");
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
                var amount = dhHelpdesk.Math.ConvertStrToDouble(eAmount.val());
                if (!dhHelpdesk.Math.IsDouble(amount)) {
                    amount = dhHelpdesk.CaseArticles.DefaultAmount;
                    eAmount.val(dhHelpdesk.Math.ConvertDoubleToStr(amount, true));
                }
                this.Amount = amount;
                eAmount.val(dhHelpdesk.Math.ConvertDoubleToStr(amount, true));

                var ePpu = this.Container.find(".article-ppu");
                if (ePpu.length > 0) {
                    var ppu = dhHelpdesk.Math.ConvertStrToDouble(ePpu.val());
                    if (!dhHelpdesk.Math.IsDouble(ppu) || ppu <= 0) {
                        ppu = dhHelpdesk.CaseArticles.DefaultPpu;
                        ePpu.val(dhHelpdesk.Math.ConvertDoubleToStr(ppu, _IGNORE_ZERO_FLOATING_POINT));
                    }
                    this.Ppu = ppu;
                    ePpu.val(dhHelpdesk.Math.ConvertDoubleToStr(ppu, _IGNORE_ZERO_FLOATING_POINT));
                }

                for (var i = 0; i < dhHelpdesk.CaseArticles.allVailableOrders.length; i++) {
                    var ord = dhHelpdesk.CaseArticles.allVailableOrders[i];
                    if (ord.Articles != undefined) {
                        for (var j = 0; j < ord.Articles.length; j++) {
                            if (this.Id == ord.Articles[j].Id) {
                                ord.Articles[j].Amount = parseFloat(this.Amount);
                                ord.Articles[j].Ppu = parseFloat(this.Ppu);
                            }
                        }
                    }
                    else
                        if (ord._articles != undefined) {
                            for (var j = 0; j < ord._articles.length; j++) {
                                if (this.Id == ord._articles[j].Id) {
                                    ord._articles[j].Amount = parseFloat(this.Amount);
                                    ord._articles[j].Ppu = parseFloat(this.Ppu);
                                }
                            }
                        }
                }

                var totalPrice = this.GetTotal().toString();
                var minusSign = "";
                if (this.CreditedForArticle_Id != null)
                    minusSign = "-";

                this.Container.find(".article-total").text(minusSign + dhHelpdesk.CaseArticles.DoDelimit(dhHelpdesk.Math.ConvertDoubleToStr(totalPrice, _IGNORE_ZERO_FLOATING_POINT)));
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
                        var ppu = dhHelpdesk.Math.ConvertStrToDouble(ePpu.val());
                        if (!dhHelpdesk.Math.IsDouble(ppu)) {
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
                                ret.Message += dhHelpdesk.Common.Translate("Textrad");
                            else
                                ret.Message += dhHelpdesk.Common.Translate("Namn");
                            ret.IsValid = false;
                        } else {
                            dhHelpdesk.Common.MakeValid(eName);
                        }
                    }
                }

                // Check if units number more then units of the credited article
                var eAmount = this.Container.find(".article-amount");
                if (eAmount.length > 0) {
                    var amount = dhHelpdesk.Math.ConvertStrToDouble(eAmount.val());
                    if (!dhHelpdesk.Math.IsDouble(amount) ||
                        amount <= 0 ||
                        (this.CreditedFrom != null && amount > this.CreditedFrom.Amount)) {
                        dhHelpdesk.Common.MakeInvalid(eAmount);
                        if (ret.Message != "")
                            ret.Message += " ,";
                        ret.Message += dhHelpdesk.Common.Translate("Enheter");
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
                        if (this.Position == thisPosition && this.IsBlank()) {
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
                        ret.Message += dhHelpdesk.Common.Translate("Textrad");
                    }
                    else {
                        dhHelpdesk.Common.MarkTextValid(eArticleRow);
                    }
                }
                return ret;
            }
        },

        DoDelimit: function (val) {
            if (val == null || val == undefined)
                return "";

            val = val.toString();
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
            this.TotalAllLabel = dhHelpdesk.Common.Translate("Alla Totalt");
            this.TotalInvoicedLabel = dhHelpdesk.Common.Translate("Skickat Totalt");
            this.TotalNotInvoicedLabel = dhHelpdesk.Common.Translate("Ej skickat Totalt");            
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
            this.NameHeader = dhHelpdesk.Common.Translate("Artikelnamn SVE");
            this.NameEngHeader = dhHelpdesk.Common.Translate("Artikelnamn ENG");
            this.UnitsHeader = dhHelpdesk.Common.Translate("Enheter");
            this.UnitsPriceHeader = dhHelpdesk.Common.Translate("PPE SEK");
            this.TypeHeader = dhHelpdesk.Common.Translate("Typ");
            this.PpuHeader = dhHelpdesk.Common.Translate("PPE");
            this.TotalHeader = dhHelpdesk.Common.Translate("Total");
            this.TotalLabel = dhHelpdesk.Common.Translate("Total");
            this.TotalAllLabel = dhHelpdesk.Common.Translate("Totalt alla ordrar");
            this.ReferenceTitle = dhHelpdesk.Common.Translate("Orderreferens");
            this.InvoicedByTitle = dhHelpdesk.Common.Translate("Skickat av");

            //LABELS FOR initiator data
            
            var extraCaption = "";

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.ReportedBy ? " *" : "";
            this.ReportedByTitle = dhHelpdesk.Common.TranslateCaseFields("ReportedBy") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_Name ? " *" : "";
            this.Persons_NameTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_Name") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_Email ? " *" : "";
            this.Persons_EmailTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_Email") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_Phone ? " *" : "";
            this.Persons_PhoneTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_Phone") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.Persons_CellPhone ? " *" : "";
            this.Persons_CellPhoneTitle = dhHelpdesk.Common.TranslateCaseFields("Persons_CellPhone") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.Region_Id? " *" : "";
            this.Region_IdTitle = dhHelpdesk.Common.TranslateCaseFields("Region_Id") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.Department_Id ? " *" : "";
            this.Department_IdTitle = dhHelpdesk.Common.TranslateCaseFields("Department_Id") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.OU_Id ? " *" : "";
            this.OU_IdTitle = dhHelpdesk.Common.TranslateCaseFields("OU_Id") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.Place ? " *" : "";
            this.PlaceTitle = dhHelpdesk.Common.TranslateCaseFields("Place") + extraCaption;

            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.UserCode ? " *" : "";
            this.UserCodeTitle = dhHelpdesk.Common.TranslateCaseFields("UserCode") + extraCaption;
            
            extraCaption = dhHelpdesk.CaseArticles.OtherReferenceRequired.CostCentre ? " *" : "";
            this.CostCentreTitle = dhHelpdesk.Common.TranslateCaseFields("CostCentre") + extraCaption;

            this.Total = null;
            this.Articles = [];
            this.InvoiceDate = "";
            this.InvoiceTime = "";
            this.InvoicedByUser = "";
            this.InvoicedByUserId = "";
            this.IsInvoiced = false;
            this.IsCredited = false;
            this.TitleCaption = "";

            this.RequiredReportedBy = false;
            this.ShowReportedBy = false;
            this.ReportedBy = "";

            this.Persons_Name = "";
            this.ShowPersons_Name = false;
            this.RequiredPerson_Name = false;

            this.Persons_Email = "";
            this.ShowPersons_Email = false;
            this.RequiredPersons_Email = false;

            this.Persons_Phone = "";
            this.ShowPersons_phone = false;
            this.RequiredPersons_Phone = false;

            this.Persons_CellPhone = "";
            this.ShowPersons_CellPhone = false;
            this.RequiredPersons_CellPhone = false;

            this.Region_Id = "";
            this.ShowRegion_Id = false;
            this.RequiredRegion_Id = false;

            this.Department_Id = "";
            this.ShowDepartment_Id = false;
            this.RequiredDepartment_Id = false;

            this.OU_Id = "";
            this.ShowOU_Id = false;
            this.RequiredOU_Id = false;

            this.Place = "";
            this.ShowPlace = false;
            this.RequiredPlace = false;

            this.UserCode = "";
            this.ShowUserCode = false;
            this.RequiredUserCode = false;

            this.CostCentre = "";
            this.ShowCostCentre = false;
            this.RequiredCostCentre = false;

            this.CreditForOrder_Id = null;
            this.Project_Id = null;            

            this.OrderState = 0;

            this.CreditOrderEnabled = false;
            this.CreditOrderTitle = "";

            this.CreditOrderId = "";
           
            this.attachedFilesTitle = dhHelpdesk.Common.Translate("Bifogade Filer");
            this.attachFilesTitle = dhHelpdesk.Common.Translate("Lägg till");
            this.doInvoiceTitle = dhHelpdesk.Common.Translate("Skicka");
            this.doCreditTitle = dhHelpdesk.Common.Translate("Kreditera");
            this.doDeleteTitle = dhHelpdesk.Common.Translate("Ta bort");
            this.SavingMessage = dhHelpdesk.Common.Translate("Spara...");


            this.FileNameStr = "";

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
            this.DisplayAmount = null;
            this.UnitName = null;
            this.Ppu = null;
            this.DisplayPpu = null;
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
                    isOrderInvoiced: (isOrderInvoiced != undefined ? isOrderInvoiced : false)
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

            var getFile = function (fileName) { 
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

        ChangeTab: function (orderId) {
            if (orderId != null) {
                if (orderId <= 0 && dhHelpdesk.CaseArticles.LastSelectedTabCaption != "") {
                    instanceOrder = this.GetOrderByCaption(dhHelpdesk.CaseArticles.LastSelectedTabCaption);
                    if (instanceOrder != null)
                        orderId = instanceOrder.Id;
                }

                var tabs = $("#case-invoice-orders-tabs");
                tab = tabs.find("[href='#case-invoice-order" + orderId + "']");
                tab.click();
                dhHelpdesk.CaseArticles.LastSelectedTab = null;
                dhHelpdesk.CaseArticles.LastSelectedTabCaption = "";
            }
        },

        GetOrderByCaption: function (orderCaption) {
            for (i = 0; i < this.allVailableOrders.length; i++) {
                if (this.allVailableOrders[i].Caption == orderCaption) {
                    return this.allVailableOrders[i];
                }
            }
            return null;
        },

        UpdateInvoiceButtonStatistics: function () {
            var ordersCount = 0;
            for (i = 0; i < dhHelpdesk.CaseArticles.allVailableOrders.length; i++) {
                if (dhHelpdesk.CaseArticles.allVailableOrders[i].OrderState != this.OrderStates.Deleted)
                    ordersCount++;
            }

            var sentCount = 0;
            var invoicedOrders = dhHelpdesk.CaseArticles.GetInvoicedOrders(dhHelpdesk.CaseArticles.allVailableOrders)
            if (invoicedOrders != null)
                sentCount = invoicedOrders.length;

            if (ordersCount > 0) {
                $('#InvoiceModuleBtnOpen').val(dhHelpdesk.CaseArticles.invoiceButtonPureCaption + " (" + (ordersCount) + "/" + (sentCount) + ")");
                var buttonHint = dhHelpdesk.Common.Translate("Order") + "   " + ordersCount + "\n" +
                                 dhHelpdesk.Common.Translate("Skickat") + " " + sentCount;

                $('#InvoiceModuleBtnOpen').attr("title", buttonHint);
            } else {
                $('#InvoiceModuleBtnOpen').val(dhHelpdesk.CaseArticles.invoiceButtonPureCaption);
                $('#InvoiceModuleBtnOpen').attr("title", "");
            }
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
        $('#InvoiceModuleBtnOpen').remove();
        invoiceButtonIndicator.show();
        loadAllData(hideButtonInicator);              
    });
   
    var hideButtonInicator = function () {
        invoiceButtonIndicator.hide();
    };

    var loadOrganizationData = function () {
        var customerId = $('#case__Customer_Id').val();
        dhHelpdesk.CaseArticles.OrganizationData = new dhHelpdesk.OrganizationLevels.OrganizationStruct(customerId, regionUrl, departmentUrl, ouUrl);
        return dhHelpdesk.CaseArticles.OrganizationData.Initialize();
    };

    var loadCaseFiles = function () {
        dhHelpdesk.CaseArticles.CaseKey = $(document).find("[data-invoice]").attr("data-invoice-case-key");
        dhHelpdesk.CaseArticles.LogKey = $(document).find("[data-invoice]").attr("data-invoice-log-key");
        dhHelpdesk.CaseArticles.CaseFiles = dhHelpdesk.CaseArticles.CaseFilesCollection({
            caseId: dhHelpdesk.CaseArticles.CaseKey
        });        
        
        return $.get('/CaseInvoice/CaseFiles', {
            id: dhHelpdesk.CaseArticles.CaseKey,
            logKey: dhHelpdesk.CaseArticles.LogKey,
            mytime: Date.now
        }, function (data) {
            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    var file = data[i];
                    var f = dhHelpdesk.CaseArticles.CaseFile({
                        fileName: file.FileName,
                        size: file.Size,
                        type: file.Type,
                        category: file.Category == 'L' ? "Log" : "Case"
                    });
                    if (dhHelpdesk.CaseArticles.CaseFiles.isFileValid(f)) {
                        dhHelpdesk.CaseArticles.CaseFiles.addFile(f);
                    }                    
                }
            }
        });
        
    };

    var loadTranslationList = function () {
        return $.get('/Translation/GetAllTextTranslations', {
            curTime: Date.now
        });
    };

    var loadCaseFieldTranslations = function () {
        return $.get('/Translation/GetCaseFieldsForTranslation', {
            curTime: Date.now
        });
    };

    var loadCaseFieldSettings = function () {
        return $.getJSON("/Cases/GetCaseFields");
    };

    var processCaseFieldSettings = function(data) {
        CaseFieldSettings = data;

        var caseFieldSettings = new dhHelpdesk.CaseArticles.OtherReferenceMandatoryFields();
        if (CaseFieldSettings.Result != null) {
            for (var i = 0; i < CaseFieldSettings.Result.length; i++) {
                switch (CaseFieldSettings.Result[i].Name.toLowerCase()) {
                case 'reportedby':
                    caseFieldSettings.ReportedBy = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'persons_name':
                    caseFieldSettings.Persons_Name = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'persons_email':
                    caseFieldSettings.Persons_Email = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'persons_phone':
                    caseFieldSettings.Persons_Phone = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'persons_cellphone':
                    caseFieldSettings.Persons_CellPhone = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'region_id':
                    caseFieldSettings.Region_Id = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'department_id':
                    caseFieldSettings.Department_Id = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'ou_id':
                    caseFieldSettings.OU_Id = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'place':
                    caseFieldSettings.Place = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'usercode':
                    caseFieldSettings.UserCode = CaseFieldSettings.Result[i].Required != 0;
                    break;

                case 'costcentre':
                    // Always required
                    caseFieldSettings.CostCentre = true;
                    break;

                }
            }

            dhHelpdesk.CaseArticles.OtherReferenceRequired = caseFieldSettings;
        }
    }

    var loadCaseInvoiceTemplate = function () {
        return $.get("/content/templates/case-invoice.tmpl.html");
    };

    var loadCaseInvoiceOrderTemplate = function () {
        return $.get("/content/templates/case-invoice-order.tmpl.html");
    };

    var loadCaseInvoiceArticleTemplate = function () {
        return $.get("/content/templates/case-invoice-article.tmpl.html");
    };

    var loadCaseInvoiceOverviewTemplate = function () {
        return $.get("/content/templates/case-invoice-overview.tmpl.html");
    };

    var loadCaseInvoiceArticleOverviewTemplate = function () {
        return $.get("/content/templates/case-invoice-article-overview.tmpl.html");
    };

    var loadCaseInvoiceOrderActionsTemplate = function () {
        return $.get("/content/templates/case-invoice-order-actions.tmpl.html");
    };

    var loadCaseInvoiceCaseFilesTemplate = function () {
        return $.get("/content/templates/case-invoice-case-files.tmpl.html");
    };

    var loadCaseInvoiceOrderFilesTemplate = function () {
        return $.get("/content/templates/case-invoice-order-files.tmpl.html");
    };

    var loadAllData = function (callBack, obj) {
        $.when(loadTranslationList(),
                loadCaseFieldTranslations(), loadOrganizationData(), loadCaseFieldSettings(), loadCaseInvoiceTemplate(), loadCaseInvoiceOrderTemplate(),
                loadCaseInvoiceArticleTemplate(), loadCaseInvoiceOverviewTemplate(), loadCaseInvoiceArticleOverviewTemplate(), loadCaseInvoiceOrderActionsTemplate(),
                loadCaseInvoiceCaseFilesTemplate(), loadCaseInvoiceOrderFilesTemplate())
            .then(function(translationList,
                caseFieldTranslations,
                organizationData,
                caseFieldSettings,
                caseInvoiceTemplate,
                caseInvoiceOrderTemplate,
                caseInvoiceArticleTemplate,
                caseInvoiceOverviewTemplate,
                caseInvoiceArticleOverviewTemplate,
                caseInvoiceOrderActionsTemplate,
                caseInvoiceCaseFilesTemplate,
                caseInvoiceOrderFilesTemplate) {
                AllTranslations = translationList;
                AllCaseFields = caseFieldTranslations;
                //organizationData
                processCaseFieldSettings(caseFieldSettings);
                dhHelpdesk.CaseArticles.CaseInvoiceTemplate = $.templates("caseInvoice", caseInvoiceTemplate);
                dhHelpdesk.CaseArticles.CaseInvoiceOrderTemplate =
                    $.templates("caseInvoiceOrder", caseInvoiceOrderTemplate);
                dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate =
                    $.templates("caseInvoiceArticle", caseInvoiceArticleTemplate);
                dhHelpdesk.CaseArticles.CaseInvoiceOverviewTemplate =
                    $.templates("caseInvoiceOverview", caseInvoiceOverviewTemplate);
                dhHelpdesk.CaseArticles.CaseInvoiceArticleOverviewTemplate =
                    $.templates("caseInvoiceArticleOverview", caseInvoiceArticleOverviewTemplate);
                dhHelpdesk.CaseArticles.CaseInvoiceOrderActionsTemplate =
                    $.templates("caseInvoiceOrderActions", caseInvoiceOrderActionsTemplate);
                dhHelpdesk.CaseArticles.CaseInvoiceCaseFilesTemplate =
                    $.templates("CaseInvoiceCaseFiles", caseInvoiceCaseFilesTemplate);
                dhHelpdesk.CaseArticles.CaseInvoiceOrderFilesTemplate =
                    $.templates("CaseInvoiceOrderFiles", caseInvoiceOrderFilesTemplate);
            })
            .then(loadCaseFiles())
            .then(function(){
                $("[data-invoice]").each(function () {
                    var $this = $(this);                                        
                    var data = $.parseJSON($this.attr("data-invoice-case-articles"));
                    $('#CaseHasInvoiceOrder').val('');
                    var emptyOrders = false;
                    var invoice = new dhHelpdesk.CaseArticles.CaseInvoice();
                    var inv = null;

                    /* Note: Now there is only one Invoice per case, if needs more we should change this line */
                    if (data == null || data.Invoices.length == 0) {
                        invoice.Id = dhHelpdesk.Common.GenerateId();
                        invoice.Initialize();
                        dhHelpdesk.CaseArticles.AddInvoice(invoice);
                        dhHelpdesk.CaseArticles.Initialize($this);
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

                    dhHelpdesk.CaseArticles.allVailableOrders = [];

                    if (data.Invoices.length != 0 && (data.Invoices[0].Orders == null || data.Invoices[0].Orders.length == 0))
                        emptyOrders = true;

                    if (!emptyOrders && (data.Invoices[0].Orders != null || data.Invoices[0].Orders.length > 0)) {
                        var hasAtleastOneOrder = false;
                        for (i = 0; i < data.Invoices[0].Orders.length; i++)
                            if (data.Invoices[0].Orders[i].OrderState != dhHelpdesk.CaseArticles.OrderStates.Deleted) {
                                hasAtleastOneOrder = true;
                                break;
                            }

                        if (!hasAtleastOneOrder) {
                            emptyOrders = true;
                            for (i = 0; i < data.Invoices[0].Orders.length; i++)
                                dhHelpdesk.CaseArticles.allVailableOrders.push(data.Invoices[0].Orders[i]);
                        }

                    }

                    if (emptyOrders || (data.Invoices.length != 0 && (data.Invoices[0].Orders == null || data.Invoices[0].Orders.length == 0))) {

                        dhHelpdesk.CaseArticles.UpdateInvoiceButtonStatistics();
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
                            if (ord.OrderState == dhHelpdesk.CaseArticles.OrderStates.Sent)
                                order.IsInvoiced = true;
                            else
                                order.IsInvoiced = false;

                            /////////////////////////
                            ///////Initiator labels
                            order.ReportedBy = dhHelpdesk.Common.DecodeJsonToStr(ord.ReportedBy);
                            order.Persons_Name = dhHelpdesk.Common.DecodeJsonToStr(ord.Persons_Name);
                            order.Persons_Email = dhHelpdesk.Common.DecodeJsonToStr(ord.Persons_Email);
                            order.Persons_Phone = dhHelpdesk.Common.DecodeJsonToStr(ord.Persons_Phone);
                            order.Persons_CellPhone = dhHelpdesk.Common.DecodeJsonToStr(ord.Persons_Cellphone);
                            order.Region_Id = ord.Region_Id;
                            order.Department_Id = ord.Department_Id;
                            order.OU_Id = ord.OU_Id;
                            order.Place = dhHelpdesk.Common.DecodeJsonToStr(ord.Place);
                            order.UserCode = dhHelpdesk.Common.DecodeJsonToStr(ord.UserCode);
                            if (ord.CostCentre != null)
                                order.CostCentre = dhHelpdesk.Common.DecodeJsonToStr(ord.CostCentre.RemoveNonNumerics());
                            else
                                order.CostCentre = "";
                            order.CreditForOrder_Id = ord.CreditForOrder_Id;
                            order.Project_Id = ord.Project_Id;
                            order.OrderState = ord.OrderState;
                            order.Date = ord.Date;

                            if (ord.Files != null) {
                                for (var m = 0; m < ord.Files.length; m++) {
                                    order.AddFileByName(ord.Files[m].FileName);
                                }
                            }

                            invoice.AddOrder(order);
                            if (ord.Articles != null) {
                                ord.Articles = dhHelpdesk.CaseArticles.SortArticles(ord.Articles);
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
                                    caseArticle.Name = dhHelpdesk.Common.DecodeJsonToStr(article.Name);
                                    caseArticle.Amount = article.Amount;
                                    caseArticle.Ppu = article.Ppu;
                                    caseArticle.UnitId = article.UnitId;
                                    caseArticle.Position = article.Position;
                                    if (article.TextForArticle_Id == undefined)
                                        caseArticle.TextForArticle_Id = null;
                                    else
                                        caseArticle.TextForArticle_Id = article.TextForArticle_Id;

                                    caseArticle.CreditedForArticle_Id = article.CreditedForArticle_Id;

                                    order.AddArticle(caseArticle);
                                    $('#CaseHasInvoiceOrder').val('yes');
                                }
                            }
                        }
                    }

                    dhHelpdesk.CaseArticles.ApplyChanges();
                    dhHelpdesk.CaseArticles.UpdateInvoiceButtonStatistics();

                    if (callBack != undefined && callBack != null) {
                        callBack(obj);
                    }

                    dhHelpdesk.CaseArticles.ChangeTab(dhHelpdesk.CaseArticles.LastSelectedTab);
                    dhHelpdesk.CaseArticles.SetInvoiceState(_INVOICE_IDLE);

                });
            });      
    };
    
    loadAllData();
});
