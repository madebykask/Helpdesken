var WorkstationEdit = function ($) {
    var self = this;
    var settings = {};

    var ouRegionCtrl = $('#OU_RegionId');
    var ouDepartmentCtrl = $('#OU_DepartmentId');
    var ouOUCtrl = $('#OU_OUId');
    var loaderIndicatorId = "loaderIndicator";
    var controlButtonsCtrl = $('.btn.controlBtn');

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
    }

    this.submit = function () {
        $("#workstation_edit_form").data("validator").settings.submitHandler = function (form) {
            if ($(form).valid()) {
                self.changeCaseButtonsState(false);
                form.submit();
            }
        };
        $('#' + settings.copyControlId).val('false');
        $('#workstation_edit_form').submit();
    }

    this.delete = function () {
        this.changeCaseButtonsState(false);
        var form = $('<form method="post" id="deleteDialogForm" action=""></form>');
        form.attr('action', $('#deleteActionBtn').attr('href')).appendTo("body");
        form[0].submit();
    }

    this.copy = function () {
        $("#workstation_edit_form").data("validator").settings.submitHandler = function (form) {
            if ($(form).valid()) {
                self.changeCaseButtonsState(false);
                form.submit();
            }
        };
        $('#' + settings.copyControlId).val('true');
        $('#workstation_edit_form').submit();
    }

    this.changeCaseButtonsState = function (state) {
        if (state) {
            controlButtonsCtrl.removeClass('disabled');
            controlButtonsCtrl.css("pointer-events", "");
            $('#' + loaderIndicatorId).css("display", "none");
        }
        else {
            controlButtonsCtrl.addClass("disabled");
            controlButtonsCtrl.css("pointer-events", "none");
            $('#' + loaderIndicatorId).css("display", "block");
        }
    }
}