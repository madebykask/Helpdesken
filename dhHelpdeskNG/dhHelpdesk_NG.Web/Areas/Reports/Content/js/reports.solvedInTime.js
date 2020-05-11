function solvedInTimeReport(args) {
    var options = $.extend({}, args);

    return {
        init: function() {
            var self = this;
            if (window.chartInstance) {
                window.chartInstance.destroy();
            };
            const ctx = document.getElementById('chartjs').getContext('2d');

            window.chartInstance =
                new Chart(ctx,
                    {
                        type: 'bar',
                        data: [],
                        plugins: [window.ChartDataLabels],
                        options: {
                            maintainAspectRatio: false,
                            title: {
                                display: true,
                                padding: 20,
                                fontSize: 16,
                                text: ''
                            },
                            legend: {
                                position: 'right'
                            },
                            plugins: {
                                colorschemes: {
                                    scheme: 'tableau.Tableau20' // see https://nagix.github.io/chartjs-plugin-colorschemes/colorchart.html for other color schemes
                                },
                                datalabels: {
                                    anchor: 'end',
                                    align: 'end',
                                    formatter: function (value, context) {
                                        var rawData = context.dataset.rawData[context.dataIndex];
                                        var values = rawData ?' (' + rawData.solvedInTimeTotal + '/' + rawData.total + ')' : '';
                                        return value + '%' + values;
                                    }
                                }
                            },
                            scales: {
                                yAxes: [
                                    {
                                        ticks: {
                                            beginAtZero: true
                                        }
                                    }
                                ]
                            }
                        }
                    });
        },

        update: function(filters) {
            var self = this;

            if (!window.chartInstance) return;
            $('#showReportLoader').show();
            $.ajax({
                    type: 'POST',
                    data: JSON.stringify({
                        'filter.GroupBy': filters.groupBy,
                        'filter.Departments': filters.departments,
                        'filter.WorkingGroups': filters.workingGroups,
                        'filter.Administrators': filters.administrators,
                        'filter.CaseTypes': filters.caseTypes,
                        'filter.ProductAreas': filters.productAreas,
                        'filter.CaseStatus': filters.status,
                        'filter.CloseFrom': filters.closeDateFrom,
                        'filter.CloseTo': filters.closeDateTo,

                    }),
                    url: options.dataUrl,
                    contentType: 'application/json'
                }).done(function (responce) {
                    window.chartInstance.options.title.text = responce.totalLabel;
                    window.chartInstance.data = responce.data;
                    window.chartInstance.update();
                    //window.chartInstance.resize();
                })
                .always(function () {
                    $('#showReportLoader').hide();
                });

        },

        toggle: function () {
            const $container = $('#jsReportContainer').find('.row').first();
            if ($container.is(':hidden')) {
                $container.show();
            } else {
                $container.hide();
            }
        },

        show: function () {
            const $container = $('#jsReportContainer').find('.row').first();
            if ($container.is(':hidden')) { $container.show() };
        },

        hide: function () {
            const $container = $('#jsReportContainer').find('.row').first();
            if (!$container.is(':hidden')) { $container.hide() };
        }
    }
};