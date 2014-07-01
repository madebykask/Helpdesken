if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

    dhHelpdesk.CaseArticles = {
        DefaultAmount: 1,

        _invoiceArticles: [],

        _caseArticles: [],

        _caseArticlesBackup: [],

        _container: null,

        ProductAreaElement: null,

        AddInvoiceArticle: function(article) {
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

        ClearInvoiceArticles: function() {
            this._invoiceArticles = [];
        },

        AddCaseArticle: function(caseArticle) {
            this._caseArticles.push(caseArticle);
            if (!caseArticle.IsNew()) {
                this._caseArticlesBackup.push(caseArticle.Clone());
            }
        },

        GetCaseArticles: function() {
            return this._caseArticles;
        },

        GetCaseArticle: function(id) {
            var caseArticles = this.GetCaseArticles();
            for (var i = 0; i < caseArticles.length; i++) {
                var caseArticle = caseArticles[i];
                if (caseArticle.Id == id) {
                    return caseArticle;
                }
            }
            return null;
        },

        ApplyChanges: function() {
            this._caseArticlesBackup = [];
            for (var i = 0; i < this._caseArticles.length; i++) {
                this._caseArticlesBackup.push(this._caseArticles[i].Clone());
            }
        },

        CancelChanges: function () {
            this._caseArticles = [];
            for (var i = 0; i < this._caseArticlesBackup.length; i++) {
                this._caseArticles.push(this._caseArticlesBackup[i].Clone());
            }
        },

        Initialize: function (e) {
            var th = this;
            var button = $(document.createElement("input"))
                .attr("type", "button")
                .attr("value", "...")
                .addClass("btn");
            button.click(function() {
                th._container = dhHelpdesk.CaseArticles.GetContainer();
                th._container.dialog({
                    title: "Articles to be invoiced",
                    width: 800,
                    height: 600,
                    close: function() {
                        th._container.dialog("destroy");
                    },
                    buttons: [
                        {
                            text: "Ok",
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
                    ]
                });

                var articlesEl = th._container.find(".multiselect");
                $.getJSON("/invoice/articles?productAreaId=" + th.ProductAreaElement.val(), function (data) {
                    th.ClearInvoiceArticles();
                    articlesEl.append("<option value='-1'>Blank optional row</option>");
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
                        articlesEl.append("<option value='" + a.Id + "'>" + a.Name + "</option>");
                    }
                })
                .always(function () {
                    articlesEl.multiselect();
                });
            });
            e.after(button);
        },

        GetContainer: function() {
            var container = $(document.createElement("div"));

            var rows = "";
            var caseArticles = this.GetCaseArticles();
            for (var i = 0; i < caseArticles.length; i++) {
                rows += caseArticles[i].Render();
            }

            var parameters = "<table>" +
                "<thead>" +
                "<tr>" +
                "<th>Select item</th>" +
                "<th>Units</th>" +
                "<th></th>" +
                "</tr>" +
                "</thead>" +
                "<body>" +
                "<tr>" +
                    "<td>" +
                        "<select class='multiselect'>" +
                        "</select>" +
                    "</td>" +
                    "<td>" +
                        "<input type='text' class='input-small-important' value='" + dhHelpdesk.CaseArticles.DefaultAmount + "' />" +
                    "</td>" +
                    "<td>" +
                        "<input type='button' class='btn' value='Add' />" +
                    "</td>" +
                "</tr>" +
                "</body>" +
                "</table>";

            var table = "<table class='width100 table table-striped table-bordered table-hover'>" +
                "<thead>" +
                "<tr>" +
                "<th>Art no</th>" +
                "<th>Description</th>" +
                "<th>Units</th>" +
                "<th>PPU</th>" +
                "<th>Total</th>" +
                "<th></th>" +
                "</tr>" +
                "</thead>" +
                "<body>" +
                rows +
                "</body>" +
                "</table>";
            container.append(parameters);
            container.append(table);
            return container;
        },

        UpdateArticle: function (e) {
            var id = e.attr("data-id");
            var article = this.GetCaseArticle(id);
            if (article != null) {
                var eAmount = e.find(".article-amount");
                var amount = eAmount.val();
                if (!this.IsInteger(amount) || amount <= 0) {
                    amount = dhHelpdesk.CaseArticles.DefaultAmount;
                    eAmount.val(amount);
                }
                article.Amount = amount;
                e.find(".article-total").text(article.GetTotal() + " " + article.Unit.Name);
            }
        },

        IsInteger: function(str) {
            return (str + "").match(/^\d+$/);
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
        },

        CaseInvoiceArticle: function() {
            this.Id = null;
            this.CaseId = null;
            this.Number = null;
            this.Name = null;
            this.Amount = null;
            this.UnitId = null;
            this.Unit = null;
            this.Ppu = null;
            this.IsInvoiced = null;

            this.Clone = function() {
                var clone = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                clone.Id = this.Id;
                clone.CaseId = this.CaseId;
                clone.Number = this.Number;
                clone.Name = this.Name;
                clone.Amount = this.Amount;
                clone.UnitId = this.UnitId;
                clone.Unit = this.Unit.Clone();
                clone.Ppu = this.Ppu;
                clone.IsInvoiced = this.IsInvoiced;
                return clone;
            };

            this.Container = null;

            this.IsNew = function() {
                return this.Id <= 0;
            };

            this.GetTotal = function() {
                return this.Amount * this.Ppu;
            };

            this.Render = function () {
                if (this.IsInvoiced) {
                    return "<tr data-id='" + this.Id + "'>" +
                            "<td>" + (this.Number != null ? this.Number : "") + "</td>" +
                            "<td>" + this.Name + "</td>" +
                            "<td>" + this.Amount + "</td>" +
                            "<td>" + this.Ppu + " " + this.Unit.Name + "</td>" +
                            "<td>" + this.GetTotal() + " " + this.Unit.Name + "</td>" +
                            "<td>" + "</td>" +
                            "</tr>";
                }

                return "<tr data-id='" + this.Id + "'>" +
                        "<td>" + (this.Number != null ? this.Number : "") + "</td>" +
                        "<td>" + this.Name + "</td>" +
                        "<td>" + "<input onchange='dhHelpdesk.CaseArticles.UpdateArticle($(this).parent().parent())' type='text' class='article-amount input-small-important' value='" + this.Amount + "' />" + "</td>" +
                        "<td>" + this.Ppu + " " + this.Unit.Name + "</td>" +
                        "<td class='article-total'>" + this.GetTotal() + " " + this.Unit.Name + "</td>" +
                        "<td>" + "</td>" +
                        "</tr>";
            };
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
        }
    }

    $("[data-invoice]").each(function () {
        var $this = $(this);
        dhHelpdesk.CaseArticles.ProductAreaElement = $(document).find("." + $this.attr("data-invoice-product-area"));
        dhHelpdesk.CaseArticles.Initialize($this);
        var data = $.parseJSON($this.attr("data-invoice-case-articles"));
        for (var i = 0; i < data.CaseArticles.length; i++) {
            var article = data.CaseArticles[i];
            var caseArticle = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
            caseArticle.Id = article.Id;
            caseArticle.CaseId = article.CaseId;
            caseArticle.Number = article.Number;
            caseArticle.Name = article.Name;
            caseArticle.Amount = article.Amount;
            caseArticle.UnitId = article.UnitId;
            var unit = new dhHelpdesk.CaseArticles.InvoiceArticleUnit();
            unit.Id = article.Unit.Id;
            unit.Name = article.Unit.Name;
            unit.CustomerId = article.Unit.CustomerId;
            caseArticle.Unit = unit;
            caseArticle.Ppu = article.Ppu;
            caseArticle.IsInvoiced = article.IsInvoiced;
            dhHelpdesk.CaseArticles.AddCaseArticle(caseArticle);
        }
    });
});