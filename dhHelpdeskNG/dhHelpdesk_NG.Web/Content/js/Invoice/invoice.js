if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

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

        CaseId: null,

        CustomerId: null,

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
            this._invoicesBackup = [];
            for (var i = 0; i < this._invoices.length; i++) {
                var invoice = this._invoices[i];
                invoice.DoInvoice();
                this._invoicesBackup.push(invoice.Clone());
            }
            this.SaveInvoices();
            /*var th = this;
            $.post("/invoice/SaveArticles", {
                caseId: this.CaseId,
                invoices: this.ToJson()
            },
            function() {
                th.ShowMessage('Articles invoiced.');
            });*/

            this.ShowMessage('Articles invoiced.');
        },

        ToJson: function () {
            return this.GetInvoice().ToJson();
        },

        ShowMessage: function(message) {
            $().toastmessage('showToast', {
                text: message,
                sticky: false,
                position: 'top-center',
                type: 'success',
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
                var article = this.CreateBlankArticle();
                var caseArticle = article.ToCaseArticle();
                caseArticle.Article = article;
                order.AddArticle(caseArticle);
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
            return this.GetOrder(orderId);
        },

        GetUserSearchOptions: function() {
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
                    return item.name;
                }
            };

            return options;
        },

        CreateContainer: function () {
            var th = this;
            var invoice = th.GetInvoice();
            th._container = invoice.Container;
            th._container.dialog({
                title: "Articles to be invoiced",
                width: 800,
                height: 600,
                close: function () {
                    th._container.dialog("destroy");
                },
                buttons: [
                    {
                        text: "Save",
                        click: function () {
                            th.ApplyChanges();
                            th._container.dialog("close");
                        }
                    },
                    {
                        text: "Cancel",
                        click: function () {
                            th.CancelChanges();
                            th._container.dialog("close");
                        }
                    }
                ],
                autoOpen: false
            });

            if (!th.IsNewCase()) {
                var invoiceBtn = $("<button>Invoice</button>");
                invoiceBtn.click(function () {
                    th.InvoiceArticles();
                    th._container.dialog("close");
                });
                th._container.parent().find(".ui-dialog-buttonset").prepend(invoiceBtn);
            }
        },

        Initialize: function (e) {
            var th = this;
            th.ProductAreaElement = $(document).find("." + e.attr("data-invoice-product-area"));
            th.CaseId = e.attr("data-invoice-case-id");
            th.CustomerId = e.attr("data-invoice-customerId");
            th.CaseInvoicesElement = $(document).find("." + e.attr("data-invoice-articles-for-save"));
            th.DateFormat = e.attr("data-invoice-date-format");

            th.CreateContainer();

            var unitsEl = th._container.find(".articles-params-units");
            var addEl = th._container.find(".articles-params-add");

            addEl.click(function () {
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
                }
            });

            var button = $(document.createElement("input"))
                .attr("type", "button")
                .attr("value", "Invoice articles")
                .attr("title", "Articles to be invoiced")
                .addClass("btn");
            button.click(function () {
                var addArticleEl = th._container.find(".articles-params");
                var articlesSelectContainer = addArticleEl.find(".articles-select-container");
                articlesSelectContainer.empty();
                var articlesEl = $("<select class='multiselect articles-params-article'></select>");
                articlesSelectContainer.append(articlesEl);

                $.getJSON("/invoice/articles?productAreaId=" + th.ProductAreaElement.val(), function (data) {
                    th.ClearInvoiceArticles();
                    if (data == null || data.length == 0) {
                        addArticleEl.hide();
                        return;
                    } else {
                        addArticleEl.show();
                    }
                    for (var i = 0; i < data.length; i++) {
                        var a = data[i];
                        var article = new dhHelpdesk.CaseArticles.InvoiceArticle();
                        article.Id = a.Id;
                        article.ParentId = a.ParentId;
                        article.Number = a.Number;
                        article.Name = a.Name;
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
                        articlesEl.append("<option value='" + article.Id + "'>" + article.GetFullName() + "</option>");
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
                })
                .always(function () {
                    articlesEl.multiselect();
                    th.Restore();
                    th.CreateContainer();
                    th._container.dialog("open");
                });
            });
            e.after(button);
        },

        IsInteger: function(str) {
            return (str + "").match(/^\d+$/);
        },

        GetRandomInt: function (min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        },   

        GenerateId: function() {
            return -this.GetRandomInt(1, 10000);
        },

        InvoiceArticle: function() {
            this.Id = null;
            this.ParentId = null;
            this.Number = null;
            this.Name = null;
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

            this.GetFullName = function () {
                var fullName;
                if (this.Number != null) {
                    fullName = this.Number + " - " + this.Name;
                } else {
                    fullName = this.Name;
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

            this.GetOrders = function() {
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

            this.AddOrder = function(order) {
                this._orders.push(order);
                order.Invoice = this;
                order.Number = this._orders.length - 1;
                order.Initialize();
                var container = this.Container.find(".orders-container");
                container.append(order.Container);

                var tabs = this.Container.find("#case-invoice-orders-tabs");
                var newTab = $("<li><a href='#case-invoice-order" + order.Id + "'>Order " + (order.Number + 1) + "</a><li>");
                tabs.find("ul").append(newTab);
                tabs.tabs("refresh");
                newTab.find("a").click();

                this.Container.find(".delivery-period").datepicker({
                    format: dhHelpdesk.CaseArticles.DateFormat,
                    autoclose: true
                });

                this.Container.find(".reference").typeahead(dhHelpdesk.CaseArticles.GetUserSearchOptions());
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
                        tabs.tabs("refresh");
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
                var clone = {};
                jQuery.extend(true, clone, this);
                return clone;

                /*var clone = new dhHelpdesk.CaseArticles.CaseInvoice();
                clone.Id = this.Id;
                clone.CaseId = this.CaseId;
                var orders = this.GetOrders();
                for (var i = 0; i < orders.length; i++) {
                    clone.AddOrder(orders[i].Clone());
                }
                return clone;*/
            },

            this.GetViewModel = function () {
                var model = new dhHelpdesk.CaseArticles.CaseInvoiceViewModel();
                model.Id = this.Id != null ? this.Id : dhHelpdesk.CaseArticles.GenerateId();
                var orders = this.GetSortedOrders();
                for (var i = 0; i < orders.length; i++) {
                    model.AddOrder(orders[i].GetViewModel());
                }

                return model;
            },

            this.Initialize = function () {
                this.Container = $(dhHelpdesk.CaseArticles.CaseInvoiceTemplate.render(this.GetViewModel()));

                var tabs = this.Container.find("#case-invoice-orders-tabs");
                tabs.tabs();
            },

            this.GetSortedOrders = function() {
                return this._orders.sort(function (a1, a2) { return a1.Number - a2.Number; });
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
                this.Container.find("#case-invoice-order-summary-total").text(this.GetArticlesTotal());
            }
        },

        CaseInvoiceOrder: function() {
            this.Id = null;
            this.Invoice = null;
            this.Number = null;
            this.DeliveryPeriod = null;
            this._articles = [];
            this.Container = null;

            this.GetArticles = function() {
                return this._articles;
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

            this.AddArticle = function(article) {
                this._articles.push(article);
                article.Order = this;
                article.Position = this._articles.length - 1;
                article.Initialize();
                var rows = this.Container.find(".articles-rows");
                rows.append(article.Container);
                this.UpdateTotal();
            },

            this.DeleteArticle = function(id) {
                for (var i = 0; i < this._articles.length; i++) {
                    var article = this._articles[i];
                    if (article.Id == id) {
                        this._articles.splice(i, 1);
                        this._deleteFromContainer(article.Id);
                        this.UpdateTotal();
                        return;
                    }
                }
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

                return '{' +
                        '"Id":"' + (this.Id >= 0 ? this.Id : 0) + '", ' +
                        '"InvoiceId":"' + (this.Invoice.Id > 0 ? this.Invoice.Id : 0) + '", ' +
                        '"Number":"' + this.Number + '", ' +
                        '"DeliveryPeriod":"' + (this.DeliveryPeriod != null ? this.DeliveryPeriod : '') + '", ' +
                        '"Articles": [' + articlesResult + ']' +
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

            this.DoInvoice = function() {
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    articles[i].DoInvoice();
                }
            },

            this.Clone = function() {
                var clone = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                clone.Id = this.Id;
                clone.Invoice = this.Invoice;
                clone.Number = this.Number;
                clone.DeliveryPeriod = this.DeliveryPeriod;
                var articles = this.GetArticles();
                for (var i = 0; i < articles.length; i++) {
                    clone.AddArticle(articles[i].Clone());
                }
                return clone;
            },            

            this.GetViewModel = function () {
                var model = new dhHelpdesk.CaseArticles.CaseInvoiceOrderViewModel();
                model.Id = this.Id != null ? this.Id : dhHelpdesk.CaseArticles.GenerateId();
                model.Total = this.GetArticlesTotal();
                var articles = this.GetSortedArticles();
                for (var i = 0; i < articles.length; i++) {
                    model.AddArticle(articles[i].GetViewModel());
                }

                return model;
            },

            this.Initialize = function () {
                this.Container = $(dhHelpdesk.CaseArticles.CaseInvoiceOrderTemplate.render(this.GetViewModel()));

                var rows = this.Container.find(".articles-rows");
                var th = this;
                rows.sortable({
                    stop: function () {
                        var i = 0;
                        rows.find("tr").each(function () {
                            var article = th.GetArticle($(this).attr("data-id"));
                            if (article != null) {
                                article.Position = i;
                            }
                            i++;
                        });
                    }
                });
            },
            
            this.UpdateTotal = function() {
                this.Container.find(".articles-total").text(this.GetArticlesTotal());
                this.Invoice.UpdateTotal();
            },

            this._deleteFromContainer = function(id) {
                this.Container.find("[data-id='" + id + "']").remove();
            }            
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
                clone.Order = this.Order;
                clone.Article = this.Article;
                clone.Name = this.Name;
                clone.Amount = this.Amount;
                clone.Position = this.Position;
                clone.IsInvoiced = this.IsInvoiced;
                clone.Ppu = this.Ppu;
                return clone;
            };

            this.Container = null;

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
                model.Id = this.Id;
                model.Number = this.GetNumber();
                model.Amount = this.Amount;
                model.UnitName = this.GetUnitName();
                model.Ppu = this.GetPpu();
                model.Total = this.GetTotal();
                model.IsArticlePpuExists = this.Article != null && this.Article.Ppu != null;
                return model;
            };

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

                this.Container.find(".article-total").text(this.GetTotal() + " " + this.GetUnitName());
                this.Order.UpdateTotal();
            },

            this.Refresh = function() {
                var refreshed = $(dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate.render(this.GetViewModel()));
                this.Container.replaceWith(refreshed);
            },

            this.DoInvoice = function() {
                this.IsInvoiced = true;
                this.Refresh();
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
            this.SelectItemHeader = "Select item";
            this.UnitsHeader = "Units";
            this.DefaultAmount = dhHelpdesk.CaseArticles.DefaultAmount;
            this.AddButtonLabel = "Add";
            this.OrderTitle = "Order";
            this.SummaryTitle = "Summary";
            this.TotalLabel = "Total";
            this.Orders = [];

            this.AddOrder = function(order) {
                this.Orders.push(order);
            }
        },

        CaseInvoiceOrderViewModel: function () {
            this.Id = null;
            this.ArtNoHeader = "Art no";
            this.DescriptionHeader = "Description";
            this.UnitsHeader = "Units";
            this.TypeHeader = "Type";
            this.PpuHeader = "PPU";
            this.TotalHeader = "Total";
            this.TotalLabel = "Total";
            this.DeliveryPeriodTitle = "Delivery period";
            this.ReferenceTitle = "Other reference";
            this.Total = null;
            this.Articles = [];

            this.AddArticle = function(article) {
                this.Articles.push(article);
            }            
        },

        CaseArticleViewModel: function() {
            this.IsBlank = null;
            this.IsInvoiced = null;
            this.Id = null;
            this.Name = null;
            this.Number = null;
            this.Amount = null;
            this.UnitName = null;
            this.Ppu = null;
            this.Total = null;
            this.IsArticlePpuExists = null;
        }
    }

    $.get("/content/templates/case-invoice.tmpl.html", function(caseInvoiceTemplate) {
        dhHelpdesk.CaseArticles.CaseInvoiceTemplate = $.templates("caseInvoice", caseInvoiceTemplate);

        $.get("/content/templates/case-invoice-order.tmpl.html", function(caseInvoiceOrderTemplate) {
            dhHelpdesk.CaseArticles.CaseInvoiceOrderTemplate = $.templates("caseInvoiceOrder", caseInvoiceOrderTemplate);

            $.get("/content/templates/case-invoice-article.tmpl.html", function (caseInvoiceArticleTemplate) {
                dhHelpdesk.CaseArticles.CaseInvoiceArticleTemplate = $.templates("caseInvoiceArticle", caseInvoiceArticleTemplate);

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
                    invoice.Initialize();
                    dhHelpdesk.CaseArticles.AddInvoice(invoice);
                    dhHelpdesk.CaseArticles.Initialize($this);
                    invoice.Id = inv.Id;
                    invoice.CaseId = inv.CaseId;
                    if (inv.Orders != null) {
                        for (var j = 0; j < inv.Orders.length; j++) {
                            var ord = inv.Orders[j];
                            var order = new dhHelpdesk.CaseArticles.CaseInvoiceOrder();
                            order.Id = ord.Id;
                            order.Number = ord.Number;
                            order.DeliveryPeriod = ord.DeliveryPeriod;
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
                            invoice.AddOrder(order);
                        }
                    }
                    dhHelpdesk.CaseArticles.AddInvoice(invoice);
                    dhHelpdesk.CaseArticles.ApplyChanges();
                });
            });        
        });
    });
});