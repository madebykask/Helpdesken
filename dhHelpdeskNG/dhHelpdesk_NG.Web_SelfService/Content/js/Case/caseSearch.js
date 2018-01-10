
function CaseSearch() {

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
        self.blockUI(true);

        var data = this.prepareSearchInputData(customerId);

        this.runSearchRequest(data,
            function (cusId) {
                self.updateCasesCount(customerId);
                self.blockUI(false);
            });
    };

    //search all cases from all customers of the user
    this.searchMultipleCustomers = function (customerIds) {

        //clear search results
        this.requests = {};
        if (customerIds.length) {

            this.blockUI(true);

            for (var i = 0; i < customerIds.length; i++) {
                var customerId = customerIds[i];
                this.requests[customerId] = false;
                var searchData = this.prepareSearchInputData(customerId);
                this.toggleMultiCustomerProgress(customerId, true);
                this.runSearchRequest(searchData, this.onMultipleCustomersSearchComplete);
            }
        }

        //console.log('Search cases called. Action: ' + this.action + ', Data: ' + JSON.stringify(data));
    };

    this.onMultipleCustomersSearchComplete = function (customerId) {
        console.log("onMultipleCustomersSearchComplete called. CustomerId: " + customerId);

        //hide progress for multi customer
        this.toggleMultiCustomerProgress(customerId, false);

        this.updateCasesCount(customerId);

        this.requests[customerId] = true;

        var allComplete = true;
        for (var key in this.requests) {
            if (this.requests.hasOwnProperty(key) && this.requests[key] === false) {
                allComplete = false;
                break;
            }
        }

        if (allComplete) {
            this.blockUI(false);
        }
    };

    this.runSearchRequest = function (searchData, completeCallback) {

        var self = this;
        var customerId = searchData.CustomerId;
        console.log('Search cases called. Action: ' + this.action + ', Data: ' + JSON.stringify(searchData));

        $.ajax({
            cache: false,
            type: "POST",
            url: this.action,
            contentType: "application/json;charset=utf-8",
            dataType: "html",
            data: JSON.stringify(searchData),
            success: function (response) {
                self.setResult(customerId, response);
            },
            error: function (err) {
                //todo: handle error
                console.error(err);
            },
            complete: function (data) {
                if (completeCallback)
                    completeCallback.call(self, searchData.CustomerId);
            }
        });
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
    }

    this.blockUI = function (block) {
        this.$btnSearch.prop("disabled", block);
        if (block) {
            this.$searchIndicator.show();
        } else {
            this.$searchIndicator.hide();
        }
    };

    this.toggleMultiCustomerProgress = function (customerId, isRunning) {
        var searchGroupCaption$ = $("#searchGroup_" + customerId).find("div.searchGroupCaption:first");
        if (isRunning) {
            searchGroupCaption$.addClass("loading");
        } else {
            searchGroupCaption$.removeClass("loading");
        }
    }

    this.setResult = function(customerId, content) {
        $('#result_' + customerId).html(content);
    };

    this.sortCases = function (customerId, newSortBy) {

        var $sortByCtl = $("#SortBy_" + customerId);
        var $sortOrderCtl = $("#SortOrder_" + customerId);
        
        var curSortBy = $sortByCtl.val();
        var curSortOrder = $sortOrderCtl.val() || "0";
        
        var newSortOrder = "0";
        if (curSortBy == newSortBy) {
            newSortOrder = curSortOrder == "0" ? "1" : "0";
        } 

        console.log('Sorting changed. SortBy: ' + newSortBy + ', SortOrder: ' + newSortOrder);

        $sortOrderCtl.val(newSortOrder);
        $sortByCtl.val(newSortBy);

        this.searchCustomerCases(customerId);
    };
}






