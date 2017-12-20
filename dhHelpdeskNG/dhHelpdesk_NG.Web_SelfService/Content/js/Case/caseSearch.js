
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
            if (this.requests.hasOwnProperty(key) && this.requests[key] == false) {
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
        var casesCount = $("#hdnCasesCount_" + customerId).val();
        $("#casesCount_" + customerId).text(casesCount);
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
        var captionDiv = $('#searchGroup_' + customerId).children(":first");

        if (isRunning) {
            captionDiv.removeClass("searchGroupCaption").addClass("searchGroupCaptionLoading");
        } else {
            captionDiv.removeClass("searchGroupCaptionLoading").addClass("searchGroupCaption");
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






