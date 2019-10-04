/// <reference path="../../../../../Content/js/moment.js" />

function FileViewLog() {
    var _self = this;

    //set from init method
    this.translations = {};
    this.urls = {};
    this.sortBy = '';
    this.sortOrder = 0;

    // constants and Ids
    this.loaders = {
        departmentsLoader: $('#departmentsLoader'),
        inProcessLoader: $('#logFilesInProcessLoader')
    };

    this.validator$ = {}

    this.form$ = $("#fileViewLogForm");
    this.customersSelect$ = this.form$.find("#fvl_customerId");
    this.departmentsSelect$ = this.form$.find("#fvl_lstFilterDepartments");
    this.periodFromDate$ = this.form$.find("#fvl_periodFrom");
    this.periodToDate$ = this.form$.find("#fvl_periodTo");
    this.logAmount$ = this.form$.find("#fvl_logsAmount");

    //buttons
    this.runBtn$ = $("#logFilesRunBtn");

    this.formFieldsState = {
        logFilesRunBtn: true,
        fvl_customerId: true,
        fvl_lstFilterDepartments: true,
        fvl_periodFrom: true,
        fvl_periodTo: true,
        fvl_logsAmount: false
    };

    this.loadDepartments = function (custId) {
        var self = this;
        this.blockUI(true, this.loaders.departmentsLoader);
        $.getJSON(this.urls.LoadDepartmentsUrl, $.param({ id: custId }))
            .done(function (res) {
                self.blockUI(false);
                if (res.data) {
                    self.populateDepartmentsField(res.data);
                }
            })
            .fail(function () {
                self.blockUI(false);
            });
    }

    this.populateDepartmentsField = function (items) {
        var self = this;
        this.departmentsSelect$.empty();
        $.each(items,
            function (idx, obj) {
                self.departmentsSelect$.append(
                    '<option value="' + obj.Value + '">' + obj.Text + '</option>');
            });
        self.departmentsSelect$.trigger('chosen:updated');
    }

            this.getSelectedDate = function (ctrl) {
            var date = ctrl.datepicker('getDate');
            return date;
        }

    this.validateDateRange = function (value, element, param) {
        var dateFrom = _self.getSelectedDate(_self.periodFromDate$);
        var dateTo = _self.getSelectedDate(_self.periodToDate$);

        //check if valid date range is specified: < today, > dateFrom
        if (moment(dateFrom).isAfter(dateTo, 'day') || moment(dateTo).isAfter(moment(), 'day')) {
            return false;
        }
        return true;
    };

    this.isValidDate = function(date) {
        return (date instanceof Date && !isNaN(date));
    }

    this.setupValidation = function () {
        var self = this;
        this.validator$ = this.form$.validate({
            ignore: '*:not([name])',
            rules: {
                "SelectedCustomerId": {
                    required: true
                },
                "SelectedDepartmetsIds": {
                    required: true
                },
                "PeriodFrom": {
                    checkDateRange: function () {
                        var hasVal = self.isValidDate(self.getSelectedDate(self.periodFromDate$));
                        return hasVal;
                    }
                },
                "PeriodTo": {
                    checkDateRange: function () {
                        var hasVal = self.isValidDate(self.getSelectedDate(self.periodToDate$));
                        return hasVal;
                    }
                }
            },
            messages: {
                "SelectedCustomerId": {
                    required: self.translations.SelectedCustomerIdRequired
                },
                "SelectedDepartmetsIds": {
                    required: self.translations.DepartmentsRequired
                }
            },
            errorPlacement: function (error, element) {
                if (element.is('select.chosen-select') || element.is('select.chosen-single-select')) {
                    var errorPlaceholder = element.next();
                    if ($(errorPlaceholder).next().hasClass('gif-loader')) {
                        errorPlaceholder = $(errorPlaceholder).next();
                    }
                    error.insertAfter(errorPlaceholder);
                    error.css('width', '315px');
                    $('#' + element.attr('id') + '_chosen').addClass('error');
                } else {
                    var ctrl$ = $('[errorFor~="' + element.attr('id') + '"]');
                    if (ctrl$.length > 0) {
                        ctrl$.html(error);
                    } else {
                        error.insertAfter(element);
                    }
                }
            },
            highlight: function (element) {
                if ($(element).is('select.chosen-select') || $(element).is('select.chosen-single-select')) {
                    $('#' + element.id + '_chosen').removeClass('error success').addClass('error');
                } else {
                    $(element).removeClass('error success').addClass('error');
                }
            },
            success: function (label) {
                var element$ = $('#' + label.attr('for'));
                if (element$.is('select.chosen-select') || element$.is('select.chosen-single-select')) {
                    $("#" + label.attr('for') + '_chosen').removeClass('error');
                }
            }
        });

        $.validator.addMethod("checkDateRange", function (value, element, param) {
            var isValid = self.validateDateRange(value, element);
            return isValid;
        }, function (param, element) {
            return _self.translations.InvalidDateRange;
        });
    }

    this.setFormFieldsState = function (enable) {
        if (enable) {
            this.restoreControlsState();
        } else {
            // save cur state
            this.saveControlsState();

            //disable controls
            for (var controlId in this.formFieldsState) {
                if (this.formFieldsState.hasOwnProperty(controlId)) {
                    var ctrl$ = $('#' + controlId);
                    if (ctrl$.length) {
                        this.enableControl(ctrl$, false);
                    }
                }
            }
        }

        //required to notify chosen to update its state
        this.departmentsSelect$.trigger('chosen:updated');
    };

    this.enableControl = function (el, enable) {
        var attr = el.attr('type') == 'text' ? 'readonly' : 'disabled';
        if (enable) {
            el.prop(attr, false);
        } else {
            el.prop(attr, true);
        }
    };

    this.isEnabled = function (el) {
        return !el.prop('disabled') && !el.prop('readonly');
    };

    this.saveControlsState = function () {
        for (var controlId in this.formFieldsState) {
            if (this.formFieldsState.hasOwnProperty(controlId)) {
                var ctrl$ = $('#' + controlId);
                if (ctrl$.length) {
                    this.formFieldsState[controlId] = this.isEnabled(ctrl$);
                }
            }
        }
    };

    this.restoreControlsState = function () {
        for (var controlId in this.formFieldsState) {
            if (this.formFieldsState.hasOwnProperty(controlId)) {
                var ctrl$ = $('#' + controlId);
                if (ctrl$.length) {
                    var prevState = this.formFieldsState[controlId];
                    this.enableControl(ctrl$, prevState);
                }
            }
        }
    };

    this.showControl = function (el, show) {
        if (show)
            el.show();
        else
            el.hide();
    };

    this.isBlocked = false;

    this.blockUI = function (block, loaderEl) {

        //ignore same action 
        if ((block && this.isBlocked) || (!block && !this.isBlocked))
            return;

        var self = this;
        this.isBlocked = block;

        if (loaderEl) {
            self.showControl(loaderEl, block);
        }

        this.setFormFieldsState(!block);

        if (!block) {
            //hide all loaders
            for (var loader in this.loaders) {
                if (this.loaders.hasOwnProperty(loader))
                    this.loaders[loader].hide();
            }
        }
    };

    this.run = function () {
        var self = this;
        var isValid = this.form$.valid();
        if (isValid) {
            this.blockUI(true, this.loaders.inProcessLoader);
            var dateFrom = this.getSelectedDate(this.periodFromDate$);
            var dateTo = this.getSelectedDate(this.periodToDate$);
            var inputData = {
                CustomerId: this.customersSelect$.val(),
                DepartmentsIds: this.departmentsSelect$.chosen().val(),
                PeriodFrom: (dateFrom instanceof Date && !isNaN(dateFrom)) ? dateFrom.toISOString() : '',
                PeriodTo: (dateTo instanceof Date && !isNaN(dateTo)) ? dateTo.toISOString() : '',
                AmountPerPage: this.logAmount$.val(),
                Sort: {
                    Name: this.sortBy,
                    SortBy: this.sortOrder
                }
            };

            $.ajax({
                url: this.urls.RunUrl,
                cache: 'false',
                type: 'POST',
                data: $.param(inputData),
                dataType: 'json'
            }).done(function (res) {
                if (res.content) {
                    $('#fvl_Table_rows').html(res.content);
                }
            }).always(function (res) {
                _self.blockUI(false);
            });
        }
    }

    return {
        init: function (settings) {
            _self.urls = settings.urls;
            _self.translations = settings.translations;
            _self.sortBy = settings.sortBy;
            _self.sortOrder = settings.sortOrder;

            _self.logAmount$.on('keypress keyup blur', function (event) {
                $(this).val($(this).val().replace(/[^\d].+/, ""));
                if ((event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
                if (event.type === 'blur' && $(this).val() === '') {
                    $(this).val('500');
                }
            });

            _self.setupValidation();

            var customerId = _self.customersSelect$.val();
            if (customerId) {
                _self.loadDepartments(customerId);
            }

            _self.customersSelect$.on('change', function () {
                var customerId = $(this).val();
                _self.loadDepartments(customerId);
            });

            _self.runBtn$.on('click', function (e) {
                e.stopImmediatePropagation();
                e.preventDefault();

                _self.run();
            });
        }
    };
};