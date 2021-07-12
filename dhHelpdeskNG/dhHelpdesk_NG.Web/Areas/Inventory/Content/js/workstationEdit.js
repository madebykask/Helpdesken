var WorkstationEdit = function ($) {
    var self = this;
    var settings = {};

    var ouRegionCtrl = $('#OU_RegionId');
    var ouDepartmentCtrl = $('#OU_DepartmentId');
    var ouOUCtrl = $('#OU_OUId');

    this.init = function (options) {
        $.extend(settings, options || {});

        ouRegionCtrl.on('change', function () {
            var regionId = $(this).val();
            $.get(settings.getDepartmentsUrl,
                {
                    'regionId': regionId,
                    'administratorId': settings.userId,
                    'departmentFilterFormat': 0
                },
                function (data) {

                    var options = '<option value="">&nbsp;</option>';
                    if (data) {
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];

                            options += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                        }
                    }

                    ouDepartmentCtrl.empty();
                    ouDepartmentCtrl.append(options);
                    ouDepartmentCtrl.trigger('change');

                }, 'json').always(function () {

                });
        });

        ouDepartmentCtrl.on('change',
            function () {
                var departmentId = $(this).val();
                $.get(settings.getUnitsUrl,
                    {
                        'departmentId': departmentId,
                        'customerId': settings.customerId
                    },
                    function (data) {

                        var options = '<option value="">&nbsp;</option>';
                        if (data) {
                            for (var i = 0; i < data.length; i++) {
                                var item = data[i];

                                options += "<option value='" + item.id + "'>" + item.name + "</option>";
                            }
                        }

                        ouOUCtrl.empty();
                        ouOUCtrl.append(options);

                    }, 'json').always(function () {

                    });
            });
    },

    this.submit = function () {
        $('#workstation_edit_form').submit();
    }

    this.copy = function () {
        $('#' + settings.copyControlId).val('true');
        $('#workstation_edit_form').submit();
    }
}