
function CaseSearch() {
    this.sortOrder = {
        Asc: 0,
        Desc: 1
    };

    this.init = function (opt) {
        this.action = opt.searchAction;
        this.$searchIndicator = $("#searchIndicator");
        this.$btnSearch = $("#btnSearch");
        this.$progressId = $("#ProgressId");
        this.$pharasSearch = $("#PharasSearch");
    }

    // used to search one customer cases 
    this.searchCustomerCases = function (customerId) {
        var self = this;
        this.blockUI(true);
        this.showProgress(true);

        var data = this.prepareSearchInputData(customerId);

        this.runSearchRequest(data).done(function(cusId) {
            self.updateCasesCount(cusId);
            self.blockUI(false);
            self.showProgress(false);
        });
    };

    //search all cases from all customers of the user
    this.searchMultipleCustomers = function (customerIds) {

        //clear search results
        var self = this;
        this.requests = [];
        if (customerIds.length) {

            this.blockUI(true);
            this.showProgress(true);

            for (var i = 0; i < customerIds.length; i++) {

                var customerId = customerIds[i];
                var searchData = this.prepareSearchInputData(customerId);

                this.toggleMultiCustomerProgress(customerId, true);

                var request = this.runSearchRequest(searchData);
                this.requests.push(request);

                request.done(function(cusId) {
                    self.onMultipleCustomersSearchComplete(cusId);
                });
            }

            //handle all requests complete
            if (this.requests.length > 0) {
                $.when.apply($, this.requests).done(function () {
                    //console.warn('All requests are complete.'); // todo: comment
                    self.blockUI(false);
                    self.showProgress(false);
                });
            }
        }

        //console.log('Search cases called. Action: ' + this.action + ', Data: ' + JSON.stringify(data));
    };
  
    this.onMultipleCustomersSearchComplete = function (customerId) {
        this.toggleMultiCustomerProgress(customerId, false);
        this.updateCasesCount(customerId);
    };

    this.runSearchRequest = function (searchData) {
        var $searchReq= $.Deferred();
        var self = this;
        var customerId = searchData.CustomerId;
        //console.warn('Customer search cases called. Action: ' + this.action + ', Data: ' + JSON.stringify(searchData));

        $.ajax({
            cache: false,
            type: "POST",
            url: this.action,
            contentType: "application/json;charset=utf-8",
            dataType: "html",
            data: JSON.stringify(searchData),
            success: function (response) {
                self.setResult(customerId, response);
                $searchReq.resolve(searchData.CustomerId);
            },
            error: function (err) {
                console.error(err);
                $searchReq.fail(err);
            }
        });

        return $searchReq;
    };

    this.prepareSearchInputData = function (customerId) {
        var $sortByCtl = $("#SortBy_" + customerId);
        var $sortOrderCtl = $("#SortOrder_" + customerId);
        return {
            CustomerId: customerId,
            ProgressId: this.$progressId.val(),
            PharasSearch: this.$pharasSearch.val(),
            SortBy: $sortByCtl == undefined ? '' : $sortByCtl.val(),
            SortOrder: $sortOrderCtl == undefined ? '0' : $sortOrderCtl.val()
        }
    };

    this.updateCasesCount = function (customerId) {
        var casesCount = parseInt($("#hdnCasesCount_" + customerId).val(), 10);
        $("#casesCount_" + customerId).text(casesCount);

        //collapse/expand results
        //this.toggleSearchResults(customerId, casesCount > 0);

        //hide empty results
        var searchGroup$ = $("#searchGroup_" + customerId);
        if (casesCount > 0) {
            searchGroup$.show();
        } else {
            searchGroup$.hide();
        }
    }

    this.toggleSearchResults = function(customerId, expand) {
        var searchGroup$ = $("#searchGroup_" + customerId);
        var searchResults$ = searchGroup$.find("div.searchResults:first");
        var searchGroupCaption$ = searchGroup$.find("div.searchGroupCaption:first");
        var iconEl$ = $("#searchGroup_" + customerId).find(".fa:first");

        if (expand) {
            iconEl$.removeClass('fa-angle-double-down').addClass('fa-angle-double-up');
            searchGroupCaption$.removeClass("collapsed");
            searchResults$.show();
        } else {
            iconEl$.removeClass('fa-angle-double-up').addClass('fa-angle-double-down');
            searchGroupCaption$.addClass("collapsed");
            searchResults$.hide();
        }
    };

    this.blockUI = function (block) {
        this.$btnSearch.prop("disabled", block);
    };

    this.showProgress = function(show) {
        if (show) {
            this.$searchIndicator.show();
        } else {
            this.$searchIndicator.hide();
        }
    };

    this.toggleMultiCustomerProgress = function(customerId, isRunning) {

        var $searchIndicator = $('#searchIndicator_' + customerId);
        var searchGroupCaption$ = $('#searchGroup_' + customerId).find("div.searchGroupCaption:first");

        // can use class to style caption
        if (isRunning) {
            searchGroupCaption$.addClass("loading");
            $searchIndicator.show();
        } else {
            searchGroupCaption$.removeClass("loading");
            $searchIndicator.hide();
        }
    };

    this.setResult = function(customerId, content) {
        $('#result_' + customerId).html(content);
    };

    this.sortCasesMulti = function (customerId, newSortBy) {
        var self = this;
        this.updateSortingState(customerId, newSortBy);
        var searchData = this.prepareSearchInputData(customerId);

        self.blockUI(true);
        this.toggleMultiCustomerProgress(customerId, true);

        this.runSearchRequest(searchData).done(function (cusId) {
            self.blockUI(false);
            self.toggleMultiCustomerProgress(cusId, false);
        });
    };

    this.sortCases = function (customerId, newSortBy) {
        var self = this;
        
        this.updateSortingState(customerId, newSortBy);
        var data = this.prepareSearchInputData(customerId);

        this.blockUI(true);

        this.runSearchRequest(data).done(function (cusId) {
            self.updateCasesCount(cusId);
            self.blockUI(false);
        });
    };

    this.updateSortingState = function (customerId, newSortBy) {
        var $sortByCtl = $("#SortBy_" + customerId);
        var $sortOrderCtl = $("#SortOrder_" + customerId);

        var curSortBy = $sortByCtl.val() || '';
        var curSortOrder = +$sortOrderCtl.val();
        if (isNaN(curSortOrder))
            curSortOrder = this.sortOrder.Asc;

        //console.warn('Cur sorting. SortBy: ' + curSortBy + ', SortOrder: ' + curSortOrder);

        var newSortOrder = this.sortOrder.Asc;
        if (curSortBy === newSortBy) {
            newSortOrder = curSortOrder === this.sortOrder.Asc ? this.sortOrder.Desc : this.sortOrder.Asc;
        }

        //console.warn('Sorting changed. SortBy: ' + newSortBy + ', SortOrder: ' + newSortOrder);

        $sortOrderCtl.val(newSortOrder);
        $sortByCtl.val(newSortBy);
    };
}






