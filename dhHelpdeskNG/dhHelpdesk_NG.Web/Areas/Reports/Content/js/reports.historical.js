function historicalReport(args) {
    var options = $.extend({}, args);

    return {
        init: function () {
            var self = this;
            if (window.chartInstance) {
                window.chartInstance.destroy();
            };
            const ctx = document.getElementById('chartjs').getContext('2d');

            window.chartInstance = new Chart(ctx,
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
                            },
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

            if (!window.chartInstance) return;

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

                    window.chartInstance.data = data;

                    window.chartInstance.update();
                    //window.chartInstance.resize();
                })
                .always(function () {
                    $('#showReportLoader').hide();
                });


        },

        toggle: function () {
            var $container = $('#jsReportContainer').find('.row').first();
            if ($container.is(':hidden')) {
                $container.show();
            } else {
                $container.hide();
            }
        },

        show: function () {
            var $container = $('#jsReportContainer').find('.row').first();
            if ($container.is(':hidden')) { $container.show() };
        },

        hide: function () {
            var $container = $('#jsReportContainer').find('.row').first();
            if (!$container.is(':hidden')) { $container.hide() };
        }

    }
}
