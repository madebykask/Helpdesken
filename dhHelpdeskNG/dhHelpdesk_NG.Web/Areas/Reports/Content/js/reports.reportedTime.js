function reportedTimeReport(args) {
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
                                align: 'end'
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

        update: function (filters) {
            var self = this;

            if (!self.chartInstance) return;

            //end of Temp data - will be removed later
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
                    'filter.RegisterFrom': filters.regDateFrom,
                    'filter.RegisterTo': filters.regDateTo,
                    'filter.CloseFrom': filters.closeDateFrom,
                    'filter.CloseTo': filters.closeDateTo
                }),
                url: options.dataUrl,
                contentType: 'application/json'
            })
                .done(function (responce) {

                    self.chartInstance.options.title.text = responce.totalLabel;
                    self.chartInstance.data = responce.data;

                    self.chartInstance.update();
                    //self.chartInstance.resize();
                })
                .always(function () {
                    $('#showReportLoader').hide();
                });


        },

        show: function () {
            var $container = $('#historicalReportContainer').find('.row').first();
            if ($container.is(':hidden')) { $container.show() };
        }
    }
}