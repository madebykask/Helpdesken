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

        CaseInvoiceArticlesElement: null,

        CaseId: null,

        IsNewCase: function() {
            return !(this.CaseId > 0);
        },

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
            caseArticle.Position = this._caseArticles.length - 1;
            if (!caseArticle.IsNew()) {
                this._caseArticlesBackup.push(caseArticle.Clone());
            }
        },

        DeleteCaseArticle: function(id) {
            var caseArticles = this.GetCaseArticles();
            for (var i = 0; i < caseArticles.length; i++) {
                var caseArticle = caseArticles[i];
                if (caseArticle.Id == id) {
                    this._caseArticles.splice(i, 1);
                    return;
                }
            }
        },

        GetCaseArticles: function() {
            return this._caseArticles;
        },


        GetSortedCaseArticles: function() {
            return this._caseArticles.sort(function (a1, a2) { return a1.Position - a2.Position; });
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

        ToCaseArticlesJson: function () {
            var articles = "";
            var caseArticles = this.GetCaseArticles();
            for (var i = 0; i < caseArticles.length; i++) {
                articles += caseArticles[i].ToJSON();
                if (i < caseArticles.length - 1) {
                    articles += ", ";
                }
            }
            return "[" + articles + "]";
        },

        SaveCaseArticles: function () {
            this.CaseInvoiceArticlesElement.val(this.ToCaseArticlesJson());
        },

        GetSavedArticles: function() {
            return this.CaseInvoiceArticlesElement.val();
        },

        GetCaseArticlesTotal: function () {
            var total = 0;
            var caseArticles = this.GetCaseArticles();
            for (var i = 0; i < caseArticles.length; i++) {
                total += caseArticles[i].GetTotal();
            }
            return total;
        },

        IsCaseArticlesEmpty: function() {
            return this._caseArticles.length == 0;
        },

        ApplyChanges: function() {
            this._caseArticlesBackup = [];
            for (var i = 0; i < this._caseArticles.length; i++) {
                this._caseArticlesBackup.push(this._caseArticles[i].Clone());
            }
            this.SaveCaseArticles();
        },

        CancelChanges: function () {
            this._caseArticles = [];
            for (var i = 0; i < this._caseArticlesBackup.length; i++) {
                this._caseArticles.push(this._caseArticlesBackup[i].Clone());
            }
        },

        InvoiceArticles: function() {
            this._caseArticlesBackup = [];
            for (var i = 0; i < this._caseArticles.length; i++) {
                var article = this._caseArticles[i];
                article.IsInvoiced = true;
                this._caseArticlesBackup.push(article.Clone());
            }
            this.SaveCaseArticles();
            var th = this;
            $.post("/invoice/SaveArticles", {
                caseId: this.CaseId,
                articles: this.GetSavedArticles()
            },
            function() {
                th.ShowMessage('Articles invoiced.');
            });
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

        CreateBlankArticle: function() {
            var blank = new dhHelpdesk.CaseArticles.InvoiceArticle();
            blank.Id = -1;
            return blank;
        },

        AddBlankArticle: function() {
            var article = this.CreateBlankArticle();
            var caseArticle = article.ToCaseArticle();
            caseArticle.Article = article;
            this.AddCaseArticle(caseArticle);
            this.AddToContainer(caseArticle);
        },

        Initialize: function (e) {
            var th = this;
            th.ProductAreaElement = $(document).find("." + e.attr("data-invoice-product-area"));
            th.CaseId = e.attr("data-invoice-case-id");
            th.CaseInvoiceArticlesElement = $(document).find("." + e.attr("data-invoice-articles-for-save"));

            var button = $(document.createElement("input"))
                .attr("type", "button")
                .attr("value", "Invoice articles")
                .attr("title", "Articles to be invoiced")
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

                if (!th.IsNewCase()) {
                    var invoiceBtn = $("<button>Invoice</button>");
                    invoiceBtn.click(function() {
                        th.InvoiceArticles();
                        th._container.dialog("close");
                    });
                    th._container.parent().find(".ui-dialog-buttonset").prepend(invoiceBtn);
                }

                var addArticleEl = th._container.find(".articles-params");
                var articlesEl = th._container.find(".articles-params-article");
                $.getJSON("/invoice/articles?productAreaId=" + th.ProductAreaElement.val(), function (data) {
                    th.ClearInvoiceArticles();
                    if (data == null || data.length == 0) {
                        addArticleEl.hide();
                        return;
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
                });

                var unitsEl = th._container.find(".articles-params-units");
                var addEl = th._container.find(".articles-params-add");

                addEl.click(function () {
                    var units = unitsEl.val();
                    if (!th.IsInteger(units) || units <= 0) {
                        units = dhHelpdesk.CaseArticles.DefaultAmount;
                        unitsEl.val(units);
                    }
                    var articleId = articlesEl.val();
                    var article = th.GetInvoiceArticle(articleId);
                    if (article != null) {
                        if (article.HasChildren()) {
                            for (var i = 0; i < article.Children.length; i++) {
                                var child = article.Children[i].ToCaseArticle();
                                child.Article = article.Children[i];
                                child.Amount = units;                                
                                th.AddCaseArticle(child);
                                th.AddToContainer(child);
                                th.UpdateTotal();
                            }
                        } else {
                            var caseArticle = article.ToCaseArticle();
                            caseArticle.Amount = units;
                            caseArticle.Article = article;
                            th.AddCaseArticle(caseArticle);
                            th.AddToContainer(caseArticle);
                            th.UpdateTotal();
                        } 
                    }
                });

                var rows = th._container.find(".articles-rows");
                rows.sortable({
                    stop: function() {
                        var i = 0;
                        rows.find("tr").each(function() {
                            var article = th.GetCaseArticle($(this).attr("data-id"));
                            if (article != null) {
                                article.Position = i;
                            }
                            i++;
                        });

                    }
                });
            });
            e.after(button);
        },

        AddToContainer: function(article) {
            var rows = this._container.find(".articles-rows");
            rows.append(article.Render());
        },

        DeleteFromContainer: function(id) {
            this._container.find("[data-id='" + id + "']").remove();
        },

        GetContainer: function() {
            var container = $(document.createElement("div"));

            var rows = "";
            var caseArticles = this.GetSortedCaseArticles();
            for (var i = 0; i < caseArticles.length; i++) {
                rows += caseArticles[i].Render();
            }

            var parameters = "<table class='articles-params'>" +
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
                        "<select class='multiselect articles-params-article'>" +
                        "</select>" +
                    "</td>" +
                    "<td>" +
                        "<input type='text' class='articles-params-units input-small-important' value='" + dhHelpdesk.CaseArticles.DefaultAmount + "' />" +
                    "</td>" +
                    "<td>" +
                        "<input type='button' class='articles-params-add btn' value='Add' />" +
                    "</td>" +
                "</tr>" +
                "</body>" +
                "</table>";

            var table = "<table><tr><td class='width100'><table class='width100 table table-striped table-bordered table-hover'>" +
                "<tr>" +
                "<th>Art no</th>" +
                "<th>Description</th>" +
                "<th>Units</th>" +
                "<th>Type</th>" +
                "<th>PPU</th>" +
                "<th>Total</th>" +
                "<th></th>" +
                "</tr>" +
                "<tbody class='articles-rows'>" +
                rows +
                "</tbody>" +
                "</table></td><td style='vertical-align: top;'><input type='button' onclick='dhHelpdesk.CaseArticles.AddBlankArticle(); return false;' value='\+' /></td></tr></table>";

            var total = "<div class='width100 textbold' style='text-align: right;'>Total: <span class='articles-total'>" + this.GetCaseArticlesTotal() + "</span></div>";

            container.append(parameters);
            container.append(table);
            container.append(total);
            return container;
        },

        UpdateTotal: function() {
           this._container.find(".articles-total").text(this.GetCaseArticlesTotal());
        },

        DeleteArticle: function(e) {
            var id = e.attr("data-id");
            this.DeleteCaseArticle(id);
            this.DeleteFromContainer(id);
            this.UpdateTotal();
        },

        UpdateArticle: function (e) {
            var id = e.attr("data-id");
            var article = this.GetCaseArticle(id);
            if (article != null) {

                if (article.IsBlank()) {
                    article.Name = e.find(".article-name").val();
                    return;
                }

                var eAmount = e.find(".article-amount");
                var amount = eAmount.val();
                if (!this.IsInteger(amount) || amount <= 0) {
                    amount = dhHelpdesk.CaseArticles.DefaultAmount;
                    eAmount.val(amount);
                }
                article.Amount = amount;
                e.find(".article-total").text(article.GetTotal() + " " + article.GetUnitName());
                this.UpdateTotal();
            }
        },

        IsInteger: function(str) {
            return (str + "").match(/^\d+$/);
        },

        GetRandomInt: function (min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        },   

        GenerateArticleId: function() {
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
                article.Id = dhHelpdesk.CaseArticles.GenerateArticleId();
                article.CaseId = dhHelpdesk.CaseArticles.CaseId;
                article.Number = this.Number;
                article.Name = this.Name;
                article.Amount = dhHelpdesk.CaseArticles.DefaultAmount;
                article.UnitId = this.UnitId;
                article.Unit = this.Unit;
                article.Ppu = this.Ppu;
                article.IsInvoiced = false;
                return article;
            };
        },

        CaseInvoiceArticle: function() {
            this.Id = null;
            this.CaseId = null;
            this.Article = null;
            this.Name = null;
            this.Amount = null;
            this.Position = null;
            this.IsInvoiced = null;

            this.ToJSON = function() {
                return '{' +
                        '"Id":"' + (this.Id >= 0 ? this.Id : 0) + '", ' +
                        '"CaseId":"' + this.CaseId + '", ' +
                        '"ArticleId":"' + (this.Article != null && this.Article.Id > 0 ? this.Article.Id : '') + '", ' +
                        '"Number":"' + this.GetNumber() + '", ' +
                        '"Name":"' + (this.Name != null ? this.Name : '') + '", ' +
                        '"Amount":"' + (this.Amount != null && !this.IsBlank() ? this.Amount : '') + '", ' +
                        '"Position":"' + this.Position + '", ' +
                        '"IsInvoiced":' + (this.IsInvoiced ? '"true"' : '"false"') +
                    '}';
            };

            this.Clone = function() {
                var clone = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
                clone.Id = this.Id;
                clone.CaseId = this.CaseId;
                clone.Article = this.Article;
                clone.Name = this.Name;
                clone.Amount = this.Amount;
                clone.Position = this.Position;
                clone.IsInvoiced = this.IsInvoiced;
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
                    return this.Amount * this.Article.Ppu;
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

            this.GetPpu = function() {
                if (this.Article != null) {
                    return this.Article.Ppu;
                }
                return 0;
            };

            this.Render = function () {
                if (this.IsBlank()) {
                    if (this.IsInvoiced) {
                        return "<tr data-id='" + this.Id + "'>" +
                                "<td colspan='6'>" + (this.Name != null ? this.Name : "") + "</td>" +
                                "<td><i class='icon-thumbs-up icon-green'></i></td>" +
                                "</tr>";
                    }
                    return "<tr data-id='" + this.Id + "'>" +
                            "<td colspan='6'>" +
                            "<input onchange='dhHelpdesk.CaseArticles.UpdateArticle($(this).parent().parent())' type='text' class='width80 article-name' maxlength='100' value='" + (this.Name != null ? this.Name : "") + "' />" +
                            "</td>" +
                            "<td><a href='javascript:void()' onclick='dhHelpdesk.CaseArticles.DeleteArticle($(this).parent().parent())'>x</a></td>" +
                            "</tr>";
                }

                if (this.IsInvoiced) {
                    return "<tr data-id='" + this.Id + "'>" +
                            "<td>" + this.GetNumber() + "</td>" +
                            "<td>" + this.Name + "</td>" +
                            "<td>" + this.Amount + "</td>" +
                            "<td>" + this.GetUnitName() + "</td>" +
                            "<td>" + this.GetPpu() + "</td>" +
                            "<td>" + this.GetTotal() + "</td>" +
                            "<td><i class='icon-thumbs-up icon-green'></i></td>" +
                            "</tr>";
                }

                return "<tr data-id='" + this.Id + "'>" +
                        "<td>" + this.GetNumber() + "</td>" +
                        "<td>" + this.Name + "</td>" +
                        "<td>" + "<input onchange='dhHelpdesk.CaseArticles.UpdateArticle($(this).parent().parent())' type='text' maxlength='5' class='article-amount input-small-important' value='" + this.Amount + "' />" + "</td>" +
                        "<td>" + this.GetUnitName() + "</td>" +
                        "<td>" + this.GetPpu() + "</td>" +
                        "<td class='article-total'>" + this.GetTotal() + "</td>" +
                        "<td><a href='javascript:void()' onclick='dhHelpdesk.CaseArticles.DeleteArticle($(this).parent().parent())'>x</a></td>" +
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
        if (data == null) {
            return;
        }
        for (var i = 0; i < data.CaseArticles.length; i++) {
            var article = data.CaseArticles[i];
            var caseArticle = new dhHelpdesk.CaseArticles.CaseInvoiceArticle();
            caseArticle.Id = article.Id;
            caseArticle.CaseId = article.CaseId;
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
            caseArticle.UnitId = article.UnitId;
            caseArticle.Position = article.Position;
            caseArticle.IsInvoiced = article.IsInvoiced;
            dhHelpdesk.CaseArticles.AddCaseArticle(caseArticle);
        }
        dhHelpdesk.CaseArticles.SaveCaseArticles();
    });
});