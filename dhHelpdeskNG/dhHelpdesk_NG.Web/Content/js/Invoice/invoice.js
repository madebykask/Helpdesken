if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

    dhHelpdesk.CaseArticles = {
        DefaultAmount: 1,

        _caseArticles: [],

        _caseArticlesBackup: [],

        _container: null,

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

        CancelChanges: function() {
            this._caseArticles = this._caseArticlesBackup;
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
                    modal: true,
                    close: function() {
                        th._container.dialog("destroy");
                    },
                    buttons: [
                        {
                            text: "Ok",
                            click: function() {
                                th._container.dialog("close");
                            }
                        },
                        {
                            text: "Cancel",
                            click: function () {
//                                th.CancelChanges();
                                th._container.dialog("close");
                            }
                        }
                    ]
                });
            });
            e.after(button);
        },

        GetContainer: function() {
            var container = $(document.createElement("div"))
                .addClass("case-invoice-articles");

            var rows = "";
            var caseArticles = this.GetCaseArticles();
            for (var i = 0; i < caseArticles.length; i++) {
                rows += caseArticles[i].Render();
            }

            var table = "<table class='table table-striped table-bordered table-hover'>" +
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
            container.append(table);
            return container;
        },

        UpdateArticle: function (e) {
            var id = e.attr("data-id");
            var article = this.GetCaseArticle(id);
            if (article != null) {
                var eAmount = e.find(".article-amount");
                var amount = eAmount.val();
                if (!this.IsInteger(amount)) {
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