function historicalReport(args) {
    var options = $.extend({}, args);

    return {
        init: function () {
            var self = this;
            if (window.chartInstance) {
                window.chartInstance.destroy();
            };
            const ctx = document.getElementById('chartjs').getContext('2d');

            window.chartInstance =
                new Chart(ctx, {
                    type: 'bar',
                    data: [],
                    plugins: [window.ChartDataLabels],
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            position: 'right'
                        },
                        layout: {
                            padding: {
                                top: 25
                            }
                        },
                        plugins: {
                            colorschemes: {
                                scheme: 'tableau.Tableau20' // see https://nagix.github.io/chartjs-plugin-colorschemes/colorchart.html for other color schemes
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'end',
                                formatter: function (value, context) {
                                    //console.log('Context: { dataIndex: %s, datasetIndex: %s, dataSet:  %s }', context.dataIndex, context.datasetIndex, context.dataset.label);
                                    const chart = context.chart;
                                    const visibleDatasets = context.chart.data.datasets.filter(function (ds, index) {
                                        return chart.isDatasetVisible(index);
                                    });

                                    // do calculation if this is the last dataset of the bar 
                                    const lastVisibleDataset = visibleDatasets[visibleDatasets.length - 1];
                                    if (context.dataset.label === lastVisibleDataset.label) {
                                        let sum = 0;
                                        visibleDatasets.map(function (dataset) {
                                            sum += dataset.data[context.dataIndex];
                                        });
                                        return sum.toLocaleString();
                                    }
                                    else {
                                        return '';
                                    }
                                }
                            }
                        },
                        scales: {
                            xAxes: [{ stacked: true }],
                            yAxes: [{
                                stacked: true,
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
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
            }).done(function (data) {
                window.chartInstance.data = data;
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
}
