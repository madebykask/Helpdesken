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

        $.validator.unobtrusive.adapters.add("remote", [], function (options) {
            options.rules["remote"] = true;
            options.messages["remote"] = options.message;
        });

        $.validator.addMethod("remote",
            function (value, element, param) {
                if (this.optional(element)) {
                    return "dependency-mismatch";
                }

                var previous = this.previousValue(element);
                if (!this.settings.messages[element.name]) {
                    this.settings.messages[element.name] = {};
                }
                previous.originalMessage = this.settings.messages[element.name].remote;
                this.settings.messages[element.name].remote = $(element).attr('data-val-remote');

                param = $(element).attr('data-val-remote-url');
                if (previous.old === value) {
                    return previous.valid;
                }

                previous.old = value;
                var validator = this;
                this.startRequest(element);
                var data = {
                    value: value,
                    currentId: settings.id
                };
                data[element.name] = value;
                var valid = "pending";
                $.ajax($.extend(true, {
                    url: param,
                    async: false,
                    mode: "abort",
                    port: "validate" + element.name,
                    dataType: "json",
                    data: data,
                    success: function (response) {
                        validator.settings.messages[element.name].remote = previous.originalMessage;
                        valid = response === true || response === "true";
                        if (valid) {
                            var submitted = validator.formSubmitted;
                            validator.prepareElement(element);
                            validator.formSubmitted = submitted;
                            validator.successList.push(element);
                            delete validator.invalid[element.name];
                            validator.showErrors();
                        } else {
                            var errors = {};
                            var message = response || validator.defaultMessage(element, "remote");
                            errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
                            validator.invalid[element.name] = true;
                            validator.showErrors(errors);
                            if (response.from === 'nameCheck') {
                                $("[data-valmsg-for='WorkstationFieldsViewModel.WorkstationFieldsModel.Name.Value']")
                                    .addClass("field-validation-error")
                                    .text(response.message);
                            }
                        }
                        previous.valid = valid;
                        validator.stopRequest(element, valid);
                    }
                }, param));
                return valid;
            }
        );

        self.removeValidation();
        $.validator.unobtrusive.parse($('#workstation_edit_form'));

        ouRegionCtrl.on('change',
            function () {
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
            }
        );

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
            }
        );
    }

    this.removeValidation = function() {
        $('#workstation_edit_form')
            .removeData("validator")
            .removeData("unobtrusiveValidation");
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
        $("#workstation_edit_form").attr('action', settings.copyUrl);
        $("#workstation_edit_form").validate().destroy();
        self.changeCaseButtonsState(false);
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