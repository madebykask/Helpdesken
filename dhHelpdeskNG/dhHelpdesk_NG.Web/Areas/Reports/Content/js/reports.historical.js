function historicalReport(args) {
    var options = $.extend({}, args);
    var ctx = document.getElementById('chartjs').getContext('2d');

    return {
        init: function () {
            var self = this;
            if (self.chartInstance) {
                self.chartInstance.destroy();
            };

            self.chartInstance = new Chart(ctx,
                {
                    type: 'bar',
                    data: [],
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            position: 'right'
                        },
                        plugins: {
                            colorschemes: {
                                scheme: 'tableau.Tableau20' // see https://nagix.github.io/chartjs-plugin-colorschemes/colorchart.html for other color schemes
                            }
                        },
                        scales: {
                            xAxes: [
                                {
                                    stacked: true
                                }
                            ],
                            yAxes: [
                                {
                                    stacked: true,
                                    ticks: {
                                        beginAtZero: true
                                    }
                                }
                            ]
                        }
                    }
                });
        },

        update: function (filters) {
            var self = this;

            if (!self.chartInstance) return;

            //Temp data - will be removed later
            var Samples = {};

            function fallback(/* values ... */) {
                var ilen = arguments.length;
                var i = 0;
                var v;

                for (; i < ilen; ++i) {
                    v = arguments[i];
                    if (v !== undefined) {
                        return v;
                    }
                }
            };

            Samples.srand = function (seed) {
                this._seed = seed;
            };

            Samples.rand = function (min, max) {
                var seed = this._seed;
                min = min === undefined ? 0 : min;
                max = max === undefined ? 1 : max;
                this._seed = (seed * 9301 + 49297) % 233280;
                return min + (this._seed / 233280) * (max - min);
            };

            Samples.numbers = function (config) {
                var cfg = config || {};
                var min = fallback(cfg.min, 0);
                var max = fallback(cfg.max, 1);
                var from = fallback(cfg.from, []);
                var count = fallback(cfg.count, 8);
                var decimals = fallback(cfg.decimals, 8);
                var continuity = fallback(cfg.continuity, 1);
                var dfactor = Math.pow(10, decimals) || 0;
                var data = [];
                var i, value;

                for (i = 0; i < count; ++i) {
                    value = (from[i] || 0) + this.rand(min, max);
                    if (this.rand() <= continuity) {
                        data.push(Math.round((dfactor * value) / dfactor));
                    } else {
                        data.push(null);
                    }
                }

                return data;
            };

            // INITIALIZATION

            Samples.srand(Date.now());

            //end of Temp data - will be removed later
            $('#showReportLoader').show();
            $.ajax({
                type: 'POST',
                data: JSON.stringify({
                    'filter.GroupBy': filters.groupBy,
                    'filter.StackBy': filters.stackBy,
                    'filter.Departments': filters.departments,
                    'filter.WorkingGroups': filters.workingGroups,
                    'filter.Administrators': filters.administrators,
                    'filter.CaseTypes': filters.caseTypes,
                    'filter.ProductAreas': filters.productAreas,
                    'filter.CaseStatus': filters.status,
                    'filter.RegisterFrom': filters.regDateFrom,
                    'filter.RegisterTo': filters.regDateTo,
                    'filter.CloseFrom': filters.closeDateFrom,
                    'filter.CloseTo': filters.closeDateTo,
                    'filter.HistoricalChangeDateTo': filters.historicalChangeDateTo,
                    'filter.HistoricalChangeDateFrom': filters.historicalChangeDateFrom,
                    'filter.HistoricalWorkingGroups': filters.historicalWorkingGroups

                }),
                url: options.dataUrl,
                contentType: 'application/json'
            })
                .done(function (data) {

                    self.chartInstance.data = data;

                    self.chartInstance.update();
                    //self.chartInstance.resize();
                })
                .always(function () {
                    $('#showReportLoader').hide();
                });


        },

        toggle: function () {
            var $container = $('#historicalReportContainer').find('.row').first();
            if ($container.is(':hidden')) {
                $container.show();
            } else {
                $container.hide();
            }
        },

        show: function () {
            var $container = $('#historicalReportContainer').find('.row').first();
            if ($container.is(':hidden')) { $container.show() };
        },

        hide: function () {
            var $container = $('#historicalReportContainer').find('.row').first();
            if (!$container.is(':hidden')) { $container.hide() };
        }

    }
}
