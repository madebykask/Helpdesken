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
                        //plugins: {
                        //    datalabels: {
                        //        color: 'white',
                        //        display: function(context) {
                        //            return context.dataset.data[context.dataIndex] > 15;
                        //        },
                        //        font: {
                        //            weight: 'bold'
                        //        },
                        //        formatter: Math.round
                        //    }
                        //},
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
            var DATA_COUNT = 10;

            var Samples = {};
            var Color = window.Color;

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

            Samples.COLORS = [
                '#FF3784',
                '#36A2EB',
                '#4BC0C0',
                '#F77825',
                '#9966FF',
                '#00A8C6',
                '#379F7A',
                '#CC2738',
                '#8B628A',
                '#8FBE00',
                '#606060'
            ];

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

            Samples.color = function (offset) {
                var count = Samples.COLORS.length;
                var index = offset === undefined ? ~~Samples.rand(0, count) : offset;
                return Samples.COLORS[index % count];
            };

            Samples.colors = function (config) {
                var cfg = config || {};
                var color = cfg.color || Samples.color(0);
                var count = cfg.count !== undefined ? cfg.count : 8;
                var method = cfg.mode ? Color.prototype[cfg.mode] : null;
                var values = [];
                var i, f, v;

                for (i = 0; i < count; ++i) {
                    f = i / count;

                    if (method) {
                        v = method.call(Color(color), f).rgbString();
                    } else {
                        v = Samples.color(i);
                    }

                    values.push(v);
                }

                return values;
            };

            Samples.transparentize = function (color, opacity) {
                var alpha = opacity === undefined ? 0.5 : 1 - opacity;
                return Color(color).alpha(alpha).rgbString();
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
                    'filter.HistoricalDateFrom': filters.historicalChangeDateFrom,
                    'filter.HistoricalWorkingGroups': filters.historicalWorkingGroups

                }),
                url: options.dataUrl,
                contentType: 'application/json'
            })
                .done(function (data) {
                    var dataCount = data.CaseTypes.length;
                    var labels = data.WorkingGroups;
                    var datasets = [];

                    for (var i = 0; i < dataCount; i++) {
                        datasets.push({
                            backgroundColor: Samples.color(i),
                            label: data.CaseTypes[i],
                            data: Samples.numbers({
                                count: dataCount,
                                min: 0,
                                max: 100
                            })
                        });
                    }

                    self.chartInstance.data = {
                        labels: labels,
                        datasets: datasets
                    }

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
