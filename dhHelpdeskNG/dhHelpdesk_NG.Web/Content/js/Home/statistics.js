function Statistics() {

    var _self = this;

    this.loadedData = null;
    this.isLoading = true;
    this.statisticsData$ = null;
    this.retries = 10;
    this.notAvailableString = '-';

    this.applyData = function() {
        console.log('applyData');
        if (this.loadedData === null) {
            $('#st_inProgress, #st_unopend, #st_onhold, #st_delayed, #st_newToday, #st_delayedToday, #st_solved, #st_solvedInTime').text(this.notAvailableString);
            return;
        }
        $('#st_inProgress').text(this.loadedData.InProgress);
        $('#st_unopend').text(this.loadedData.Unopened);
        $('#st_onhold').text(this.loadedData.Onhold);
        $('#st_delayed').text(this.loadedData.Overdue);
        $('#st_newToday').text(this.loadedData.NewToday);
        $('#st_delayedToday').text(this.loadedData.DueToday);
        $('#st_solved').text(this.loadedData.SolvedToday);
        $('#st_solvedInTime').text(this.loadedData.SolvedInTimeToday);
    }

    this.stopLoading = function () {
        _self.loadedData = null;
        _self.applyData();
    }

    return {
        init: function(options) {
            _self.options = options;
            _self.statisticsData$ = $('#statisticsData');
        },

        loadStatistics: function() {
            _self.isLoading = true;
            return $.ajax({
                    url: _self.options.dataUrl,
                    type: 'get',
                    dataType: 'json'
                })
                .done(function(data) {
                    _self.loadedData = data;
                })
                .always(function() {
                    _self.isLoading = false;
                })
                .fail(function() {
                    _self.loadedData = null;
                });
        },

        tryApplyData: function() {
            if (_self.retries <= 0) {
                _self.stopLoading();
                 return;
            }
            console.log('tryApplyData ' + _self.retries);
            if (!_self.isLoading) {
                _self.applyData();
                _self.retries = 0;
                return;
            }
            _self.retries--;
            setTimeout(this.tryApplyData.bind(this), 500);
        }
    }
};

if (window.dhHelpdesk == null)
    dhHelpdesk = {};

dhHelpdesk.statistics = new Statistics();
