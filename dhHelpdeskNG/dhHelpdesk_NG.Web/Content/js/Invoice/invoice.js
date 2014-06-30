if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

    dhHelpdesk.CaseArticles = {
        
        _caseArticles: [],

        AddCaseArticle: function(caseArticle) {
            this._caseArticles.push(caseArticle);
        },

        CaseInvoiceArticle: function() {
            this.Id = null;
            this.CaseId = null;
            this.Name = null;
            this.UnitId = null;
            this.Unit = null;
            this.Ppu = null;
            this.IsInvoiced = null;
        },

        InvoiceArticleUnit: function() {
            this.Id = null;
            this.Name = null;
            this.CustomerId = null;
        }
    }

    $("[data-invoice]").each(function () {
        var $this = $(this);
        var caseArticles = $.parseJSON($this.attr("data-invoice-case-articles"));

    });
});