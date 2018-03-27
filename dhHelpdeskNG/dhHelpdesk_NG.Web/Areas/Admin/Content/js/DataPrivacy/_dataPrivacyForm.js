/// <reference path="../../../../../Content/js/moment.js" />

window.DataPrivacyForm =
    (function($){

        var _self = this;

        //set from init method
        this.dateformat = '';
        this.translations = {};
        this.confirmationDialog = {};
        this.urls = {};

        // constants and Ids
        this.loaders = {
            fieldsLoader: $('#fieldsLoader'),
            favoritesLoader: $('#favoritesLoader'),
            inProcessLoader: $('#inProcessLoader'),
            saveFavoritesLoader: $('#saveFavoritesLoader')
        };

        this.validator$ = {}

        //form and fields
        this.form$ = $("#privacyForm");
        this.btnLock$ = form$.find("#btnLock");
        this.btnUnLock$ = form$.find("#btnUnLock");
        this.customerSelect$ = form$.find("#customerSelect");
        this.favoritesSelect$ = form$.find("#favoritesSelect");
        this.registerDateFrom$ = form$.find("#DataPrivacy_RegisterDateFrom");
        this.registerDateTo$ = form$.find("#DataPrivacy_RegisterDateTo");
        
        this.calculateRegistrationDate$ = form$.find("#DataPrivacy_CalculateRegistrationDate");
        this.retentionPeriod$ = form$.find("#retentionPeriod");
        this.filterFields$ = form$.find("#lstFilterFields");
        this.closedOnly$ = form$.find("#DataPrivacy_ClosedOnly");
        this.replaceDataWith$ = form$.find("#DataPrivacy_ReplaceDataWith");
        this.replaceDatesWith$ = form$.find("#DataPrivacy_ReplaceDatesWith");
        this.removeCaseAttachments$ = form$.find("#DataPrivacy_RemoveCaseAttachments");
        this.removeLogAttachments$ = form$.find("#DataPrivacy_RemoveLogAttachments");


        //buttons
        this.btnFavorite$ = $("#btnFavorite");
        this.privacyRunBtn$ = $("#privacyRunBtn");

        //private functions:

         this.formFieldsState = {
             btnFavorite: true,
             privacyRunBtn: true,
             favoritesSelect: true,
             customerSelect: true,
             lstFilterFields: true,
             DataPrivacy_CalculateRegistrationDate: false
         };

        this.isBlocked = false;

        this.blockUI = function(block, loaderEl) {

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
            this.filterFields$.trigger("chosen:updated");
        };

         this.saveControlsState = function() {
             for (var controlId in this.formFieldsState) {
                 if (this.formFieldsState.hasOwnProperty(controlId)) {
                     var ctrl$ = $('#' + controlId);
                     if (ctrl$.length) {
                         this.formFieldsState[controlId] = this.isEnabled(ctrl$);
                     }
                 }
             }
         };

         this.restoreControlsState = function() {
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

         this.togglePrivacyRunBtn = function (enable) {
             var wrapper$ = $('#tooltip-button-wrapper');
             if (enable) {
                 wrapper$.tooltip('destroy');
             } else {
                 wrapper$.tooltip({container: 'body'});
             }
            _self.enableControl(_self.privacyRunBtn$, enable);

        }

        this.lockFormFields = function (lock) {
            var self = this;
            self.showLocks(lock, !lock);

            this.form$.find("input, select").each(function(index, el) {
                var id = el.id;
                if (id !== self.favoritesSelect$[0].id) {
                    var ctl$ = $('#' + id);
                    self.enableControl(ctl$, !lock);
                }  
            });

            self.refreshFieldsControl();

            //override unlock state if calc is checked
            if (!lock && self.calculateRegistrationDate$.prop('checked')) {
                self.enableControl(self.registerDateTo$);
            }

            //enable run button only when form is locked!
            _self.togglePrivacyRunBtn(lock);
            _self.enableControl(_self.btnFavorite$, !lock);
        }

        this.getFilterData = function() {
            var fields = [];
            this.filterFields$.find("option:selected").each(function() {
                fields.push($(this).val());
            });
            return {
                fields: fields,
                selectedCustomerId: this.customerSelect$.val(),
                retentionPeriod: this.retentionPeriod$.val() || "0",
                calculateRegistrationDate: this.calculateRegistrationDate$.prop("checked"),
                registerDateFrom: this.registerDateFrom$.val(),
                registerDateTo: this.registerDateTo$.val(),
                closedOnly: $("#DataPrivacy_ClosedOnly").prop("checked"),
                replaceDataWith: $("#DataPrivacy_ReplaceDataWith").val(),
                replaceDatesWith: $("#DataPrivacy_ReplaceDatesWith").val(),
                removeCaseAttachments: $("#DataPrivacy_RemoveCaseAttachments").prop("checked"),
                removeLogAttachments: $("#DataPrivacy_RemoveLogAttachments").prop("checked"),
                removeCaseHistory: $("#DataPrivacy_RemoveCaseHistory").prop("checked") // todo: remove after case history is implemented
            };
        };

        this.setupValidation = function() {
            var self = this;
            var caseFieldsDropDown = "lstFilterFields";

            this.validator$ = this.form$.validate({
                ignore: '*:not([name])',
                //debug: true,
                rules: self.getRules(),
                errorPlacement: function(error, element) {
                    if (element.is("#DataPrivacy_RegisterDateFrom") ||
                        element.is("#DataPrivacy_RegisterDateTo")) {
                        $("#datesErrorLabel").html(error);
                    } else if (element.is('#retentionPeriod')) {
                        $("#retentionDaysError").html(error);
                    }
                    else {
                        if (element.is("select.chosen-select")) {
                            error.insertAfter($("#fieldsLoader"));
                            error.css("width", "315px");
                            $("#" + element.attr("id") + "_chosen").addClass("error");
                        } else {
                            error.insertAfter(element);
                        }
                    }
                },
                highlight: function(element) {
                    if (element.id === caseFieldsDropDown) {
                        $("#" + element.id + "_chosen").removeClass('error success').addClass('error');
                    } else {
                        $(element).removeClass('error success').addClass('error');
                    }
                },
                success: function(label) {
                    if (label.attr("for") === caseFieldsDropDown) {
                        $("#" + label.attr("for") + "_chosen").removeClass("error");
                    }
                },

                messages: {
                    "DataPrivacy.SelectedCustomerId": {
                        required: self.translations.SelectedCustomerIdRequired
                    },
                    "DataPrivacy.FieldsNames": {
                        required: self.translations.FieldsNamesRequired
                    },
                    "DataPrivacy.RegisterDateFrom": {
                        required: self.translations.RegisterDateFromRequired
                    },
                    "DataPrivacy.RegisterDateTo": {
                        required: self.translations.RegisterDateToRequired
                    }
                }
            });

            jQuery.validator.addMethod("checkDateRange", function (value, element, param) {
                var isValid = self.validateDateRange(value, element);
                return isValid;
            }, function (param, element) {
                return _self.translations.InvalidDateRange;
            });

            jQuery.validator.addMethod("checkRetentionDate", function (value, element, param) {
                var isValid = self.validateRetentionPeriodDate(value, element);
                return isValid;

            }, function (param, element) {
                var dateTo = self.getSelectedDate(self.registerDateTo$);
                var validDateTo = self.calculateValidRetentionPeriodEndDate(dateTo);
                //return 'End date shall be less or equal than ' + moment(validDateTo).format("YYYY-MM-DD"); 
                return _self.translations.InvalidDateRange;
            });

            //workaround to trigger validation for date time range 
            this.registerDateFrom$.on('change', function () { self.validateDateRangeControls(); });
            this.registerDateTo$.on('change', function () { self.validateDateRangeControls(); });

            this.validateDateRangeControls = function() {
                _self.registerDateFrom$.valid();
                _self.registerDateTo$.valid();
            };

            this.form$.find(".chosen-select").chosen().change(function() {
                $(this).valid();
            });
        };

        this.validateDateRange = function (value, element, param) {
            var dateFrom = _self.getSelectedDate(_self.registerDateFrom$);
            var dateTo = _self.getSelectedDate(_self.registerDateTo$);

            //check if valid date range is specified: < today, > dateFrom
            if (moment(dateFrom).isAfter(dateTo, 'day') || moment(dateTo).isAfter(moment(), 'day')) {
                return false;
            }
            return true;
        };

        this.validateRetentionPeriodDate = function (value, element, param) {
            var dateTo = _self.getSelectedDate(_self.registerDateTo$);

            //check if dateTo falls into retention period if its specified
            var retentionDaysVal = _self.retentionPeriod$.val();
            if (retentionDaysVal) {
                var validEndDate = this.calculateValidRetentionPeriodEndDate(dateTo);
                if (moment(dateTo).isAfter(validEndDate, 'day')) {
                    return false;
                }
            }
            return true;
        };

         this.getRules = function () {
             var self = this;

             return {
                 "DataPrivacy.SelectedCustomerId": {
                     required: true
                 },
                 "DataPrivacy.FieldsNames": {
                     required: true,
                 },
                 "DataPrivacy.RegisterDateFrom": {
                     required: true,
                     checkDateRange: function () {
                         var hasVal = !!self.registerDateTo$.val();
                         return hasVal;
                     }
                 },
                 "DataPrivacy.RegisterDateTo": {
                     required: true,
                     checkDateRange: function() {
                         var hasVal = !!self.registerDateFrom$.val();
                         return hasVal;
                     },
                     checkRetentionDate: function () {
                        var hasVal = !!self.registerDateTo$.val() && !!self.retentionPeriod$.val();
                        return hasVal;
                     }
                 }
             };
         };
        
         this.onRetentionPeriodChanged = function () {

             var text = this.retentionPeriod$.val() || '';
            if (!text.length) {
                this.emptyDatePicker(this.registerDateTo$);
             } 

             this.setRetentionPeriodChangedState();
             this.updateDateRange();
             this.validateDateRangeControls();
        };

        this.onCalculateRetentionPeriodChanged = function(checked) {
            if (!checked) {
                //empty dateTo to require user to enter a new date.
                this.emptyDatePicker(this.registerDateTo$);
            }

            this.setRetentionPeriodChangedState();
            this.updateDateRange();
            this.validateDateRangeControls();
        };

        this.setRetentionPeriodChangedState = function () {
            var isCalcChecked = this.calculateRegistrationDate$.is(':checked');
            var text = this.retentionPeriod$.val() || '';
            if (text && text.length) {
                this.enableControl(this.calculateRegistrationDate$, true);
                this.enableControl(this.registerDateTo$, !isCalcChecked);
            } else {
                this.calculateRegistrationDate$.prop('checked', false);
                this.enableControl(this.calculateRegistrationDate$, false);
                this.enableControl(this.registerDateTo$, true);
            }
        }

        this.calculateValidRetentionPeriodEndDate = function () {
            var dateTo = moment(); //today

            var retentionDaysVal = _self.retentionPeriod$.val();
            if (retentionDaysVal) {
                var retentionDays = parseInt(retentionDaysVal.toString(), 10);
                if (!isNaN(retentionDays) && retentionDays > 0) {
                    dateTo = dateTo.subtract(retentionDays, 'days');
                }
            }
            return dateTo.toDate();
        };

        this.onValidationChanged = function() {
             if (this.validator$) {
                 var numberOfInvalids = this.validator$.numberOfInvalids();
                 this.enableControl(this.btnFavorite$, numberOfInvalids === 0);
             }
         };

         this.loadCustomerFields = function(customerId) {
             var self = this;
             if (customerId) {
                 this.blockUI(true, this.loaders.fieldsLoader);
                 self.filterFields$.empty();

                 this.execLoadCustomerFieldsRequest(customerId)
                     .done(function(response) {
                         self.blockUI(false);
                         if (response.success) {
                             self.populateCustomerFields(response.data);
                         }
                     })
                     .fail(function() {
                         self.blockUI(false);
                     });
             } else {
                 self.filterFields$.empty();
                 self.refreshFieldsControl();
             }
         };

        this.execLoadCustomerFieldsRequest = function(customerId) {
            var jqXhr = $.ajax({
                url: self.urls.GetCustomerCaseFieldsAction,
                type: "POST",
                data: $.param({ customerId: customerId }),
                dataType: "json"
            });
            return jqXhr;
        };

        this.populateCustomerFields = function(items) {
            var self = this;
            this.filterFields$.empty();
            $.each(items,
                function (idx, obj) {
                    self.filterFields$.append(
                        '<option value="' + obj.Value + '">' + obj.Text + '</option>');
                });
            self.filterFields$.trigger("chosen:updated");
        }


         this.runDataPrivacy = function() {
             var self = this;
             var isValid = this.form$.valid();
             if (isValid) {
                 this.confirmationDialog.showConfirmation(
                     this.translations.dataPrivacyConfirmation,
                     function() {
                         self.execDataPrivacyRequest();
                     },
                     function() {
                     });
             }
         };

         this.execDataPrivacyRequest = function() {

             this.blockUI(true, this.loaders.inProcessLoader);

             var filter = this.getFilterData();

             var inputData = {
                 SelectedCustomerId: filter.selectedCustomerId,
                 CalculateRegistrationDate: filter.calculateRegistrationDate,
                 RegisterDateFrom: filter.registerDateFrom,
                 RegisterDateTo: filter.registerDateTo,
                 ClosedOnly: filter.closedOnly,
                 FieldsNames: filter.fields,
                 ReplaceDataWith: filter.replaceDataWith,
                 ReplaceDatesWith: filter.replaceDatesWith,
                 RemoveCaseAttachments: filter.removeCaseAttachments,
                 RemoveLogAttachments: filter.removeLogAttachments,
                 RemoveCaseHistory: filter.removeCaseHistory
             };

             $.ajax({
                 url: self.urls.DataPrivacyAction,
                 type: "POST",
                 data: $.param(inputData), 
                 dataType: "json"
             }).done(function(result) {
                 if (result.success) {
                     window.ShowToastMessage(self.translations.operationSuccessMessage, "success");
                 } else {
                     window.ShowToastMessage("Operation has failed.", "error");
                 }

             }).always(function() {
                 self.blockUI(false);
             });
         };

        this.onFavoritesChanged = function(favoriteId) {
            if (favoriteId > 0) {
                this.loadFavoriteFields(favoriteId);
            } else {
                //reset validation errors if New is selected
                this.lockFormFields(false);
                this.resetFormFields();
                this.validator$.resetForm();
            }
        };

        this.loadFavoriteFields = function (favId) {
            var self = this;
            this.blockUI(true, this.loaders.favoritesLoader);
            
            $.getJSON(this.urls.LoadFavoriteDataAction, $.param({ id: favId }))
                .done(function (res) {
                    self.blockUI(false);
                    if (res.data) {
                        self.populateFormFields(res.data)
                            .done(function () {
                                self.form$.validate();
                                if (self.form$.valid()) {
                                    self.lockFormFields(true);
                                } else {
                                    self.lockFormFields(false);
                                }
                            });
                    }
                }).fail(function() {
                    self.blockUI(false);
                });
        };
        
        this.saveFavorites = function() {
            var isValid = this.form$.valid();
            if (isValid) {
                var favId = _self.getSelectedFavoriteId();
                var isNew = favId === 0;
                this.showSaveFavoritesDlg(isNew);
            }
        };

        this.resetFormFields = function () {
            var self = this;
            self.customerSelect$.val(null);
            self.filterFields$.empty();
            self.refreshFieldsControl();

            self.enableControl(this.registerDateTo$, true);
            self.enableControl(this.calculateRegistrationDate$);

            self.setDatepickerDate(registerDateFrom$, null);
            self.setDatepickerDate(registerDateTo$, null);
            
            self.retentionPeriod$.val('');
            self.calculateRegistrationDate$.prop('checked', false);
            self.replaceDataWith$.val('');
            self.setDatepickerDate(self.replaceDatesWith$, null);
            self.removeCaseAttachments$.prop('checked', false);
            self.removeLogAttachments$.prop('checked', false);
            self.closedOnly$.prop('checked', false);


            //set today as a max date for date range
            self.setDatepickerEndDate(registerDateFrom$, new Date());
            self.setDatepickerEndDate(registerDateTo$, new Date());

            //reset locking/unlocking states to default
            _self.togglePrivacyRunBtn(false);
            self.enableControl(self.btnFavorite$, true);
            self.showLocks(false, false);
        };

        this.updateDateRange = function () {
            var self = this;

            var dateTo$ = moment(this.getSelectedDate(self.registerDateTo$));
            var dateFrom$ = moment(this.getSelectedDate(self.registerDateFrom$));

            // calc dates end limits
            var maxDates = this.calcDateRangeLimits();
            var dateFromMax = maxDates.dateFrom;
            var dateToMax = maxDates.dateTo;

            //set max date limits
            this.setDatepickerEndDate(self.registerDateFrom$, maxDates.dateFrom);
            this.setDatepickerEndDate(self.registerDateTo$, maxDates.dateTo);

            var isCalcChecked = self.calculateRegistrationDate$.is(':checked');
            if (isCalcChecked) {
                //set calced max allowed value 
                this.setDatepickerDate(this.registerDateTo$, dateToMax);
            } else if (dateTo$.isValid() && dateTo$.isAfter(dateToMax, 'days')) {
                //fix dateTo if it doesn't fall into correct range
                this.emptyDatePicker(self.registerDateTo$);
            }

            //fix dateFrom if its after max end date or dateTo date
            if (dateFrom$.isValid() && dateFrom$.isAfter(dateFromMax, 'days')) {
                this.emptyDatePicker(self.registerDateFrom$);
            }
        }

        this.calcDateRangeLimits = function () {
            var self = this;
            var dateTo$ = moment(this.getSelectedDate(self.registerDateTo$));
            
            //calc max date for dateTo
            var dateToMax = this.calculateValidRetentionPeriodEndDate();
            
            var isCalcChecked = self.calculateRegistrationDate$.is(':checked');

            //calc max date for dateFrom: should be less then dateFrom;
            var dateFromMax = dateTo$.isValid() && dateTo$.isSameOrBefore(dateToMax, 'days') && !isCalcChecked 
                ? dateTo$.toDate()
                : dateToMax;

            return {
                dateFrom: dateFromMax,
                dateTo: dateToMax
            };
        };

        this.populateFormFields = function(data) {
            var self = this;
            var customerId = data.CustomerId;

            self.customerSelect$.val(customerId);

            //date range
            self.setDatepickerDate(registerDateFrom$, data.RegisterDateFrom);
            self.setDatepickerDate(registerDateTo$, data.RegisterDateTo);
            self.retentionPeriod$.val(data.RetentionPeriod == "0" ? '' : data.RetentionPeriod);
            self.calculateRegistrationDate$.prop('checked', data.CalculateRegistrationDate);
            if (data.CalculateRegistrationDate) {
                self.enableControl(self.registerDateTo$, false);
            }
            
            // update date range to correct values
            self.updateDateRange();

            self.closedOnly$.prop('checked', data.ClosedOnly);
            self.replaceDataWith$.val(data.ReplaceDataWith || '');
            self.setDatepickerDate(self.replaceDatesWith$, data.ReplaceDatesWith);
            self.removeCaseAttachments$.prop('checked', data.RemoveCaseAttachments);
            self.removeLogAttachments$.prop('checked', data.RemoveLogAttachments);

            var defer = $.Deferred();
            self.filterFields$.empty();

            this.execLoadCustomerFieldsRequest(customerId)
                .done(function (response) {
                    if (response.success) {
                        self.populateCustomerFields(response.data);
                    }

                    //set case fields
                    if (data.FieldsNames.length) {
                        self.filterFields$.val(data.FieldsNames);
                        self.refreshFieldsControl();
                    }
                })
                .always(function () {
                    defer.resolve(); //notify all processing is complete
                });

            return defer;
        };
        
        this.deleteFavorite = function(favId) {
            var self = this;
            if (favId) {
                self.blockUI(true, self.loaders.saveFavoritesLoader);

                $.ajax({
                    url: self.urls.DeleteFavoriteAction,
                    type: "POST",
                    data: $.param({ id: favId}),
                    dataType: "json"
                }).done(function (res) {
                    self.blockUI(false);
                    if (res.Success) {
                        window.ShowToastMessage(self.translations.operationSuccessMessage, "success");
                        self.populateFavorites(0, res.Favorites, true);
                    } else {
                        window.ShowToastMessage('Operation has failed.', "error");
                    }
                }).fail(function(res) {
                    self.blockUI(false);
                });
            }
        }

        this.addUpdateFavorites = function(name) {
            var self = this;

            if (this.form$.valid()) {

                var favoriteId = this.getSelectedFavoriteId();
                var filter = this.getFilterData();

                var inputData = {
                    Id: favoriteId,
                    Name: name || '',
                    CustomerId: filter.selectedCustomerId,
                    RetentionPeriod: filter.retentionPeriod,
                    CalculateRegistrationDate: filter.calculateRegistrationDate,
                    RegisterDateFrom: filter.registerDateFrom,
                    RegisterDateTo: filter.registerDateTo,
                    ClosedOnly: filter.closedOnly,
                    FieldsNames: filter.fields,
                    ReplaceDataWith: filter.replaceDataWith,
                    ReplaceDatesWith: filter.replaceDatesWith,
                    RemoveCaseAttachments: filter.removeCaseAttachments,
                    RemoveLogAttachments: filter.removeLogAttachments,
                    RemoveCaseHistory: filter.removeCaseHistory
                };

                this.blockUI(true, this.loaders.saveFavoritesLoader);

                var isNew = (+inputData.Id) > 0;
                $.ajax({
                    url: this.urls.SaveFavoritesAction,
                    type: "POST",
                    data: JSON.stringify(inputData),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                }).done(function (res) {
                    self.blockUI(false);
                    if (res.Success) {
                        window.ShowToastMessage(self.translations.operationSuccessMessage, "success"); 
                        if (res.FavoriteId) {
                            self.populateFavorites(res.FavoriteId, res.Favorites, isNew);
                            // lock form if favorite is selected
                            self.lockFormFields(true);
                        }
                    } else {
                        var err = res.Error || 'Unknown error';
                        window.ShowToastMessage(err, "error");
                    }
                }).fail(function() {
                    self.blockUI(false);
                });
            }
        };

        this.populateFavorites = function(selectedId, items, triggerChange) {
            var self = this;

            //build options
            var options = items.map(function(item) {
                return '<option value="' + item.value + '">' + item.text + '</option>';
            });

            var emptyFavSelectText = self.translations.createNewFavorite || 'Create New';
            options = '<option value="0">' + emptyFavSelectText + '</option>' + options;

            self.favoritesSelect$.html(options);
            self.favoritesSelect$.val(selectedId);

            if (triggerChange) {
                self.favoritesSelect$.change();
            }
        };

        this.showSaveFavoritesDlg = function(saveNew) {
            
            var nameField$ = $("#fm_name");

            $("#fm_header_new").toggle(saveNew);
            $("#fm_header_update").toggle(!saveNew);
            $("#fm_body_new").toggle(saveNew);
            $("#fm_body_update").toggle(!saveNew);

            $("#btnDeleteFav").prop('disabled', saveNew);

            if (saveNew) {
                nameField$.val('');
                $("#btnSaveFav").prop('disabled', true);
                $("#selectedFavoriteText").html('').hide();
            } else {
                var selectedFav = _self.favoritesSelect$.find(":selected").text() || '';
                $("#selectedFavoriteText").html(selectedFav).show();
                nameField$.val(selectedFav);
                $("#btnSaveFav").prop('disabled', selectedFav.length === 0);
            }

            $("#favoritesSaveModal").modal('show');
        };

        /// HELPER METHODS

        this.showLocks = function (showLock, showUnlock) {
            this.showControl(this.btnLock$, showLock);
            this.showControl(this.btnUnLock$, showUnlock);
        }

        this.getSelectedFavoriteId = function () {
            var val = this.favoritesSelect$.val();
            var favoriteId = val ? parseInt(val, 10) : 0;
            if (!isNaN(favoriteId) && favoriteId > 0)
                return favoriteId;
            else
                return 0;
        };

        this.refreshFieldsControl = function () {
            // required to update chosen control after changes
            this.filterFields$.trigger("chosen:updated");
        };

        this.getSelectedDate = function (ctrl) {
            var date = ctrl.parent().datepicker('getDate');
            return date;
        }
        
        this.setDatepickerDate = function (ctrl, date, raiseEvent) {
            var cmd = raiseEvent ? 'setDate' : 'update';
            //console.log('>>>setDatepickerDate. Date: ' + date + ', Cmd: ' + cmd);

            var date$ = moment(date);
            if (date$.isValid()) {
                ctrl.parent().datepicker(cmd, date$.format('YYYY-MM-DD'));
            } else {
                ctrl.parent().datepicker(cmd, '');
            }
        };

        this.emptyDatePicker = function (ctrl, raiseEvent) {
            this.setDatepickerDate(ctrl, '', raiseEvent);
        }

        this.setDatepickerEndDate = function (ctrl, maxDate) {
            ctrl.parent().datepicker('setEndDate', maxDate);
        }

        this.enableControl = function (el, enable) {
            var attr = el.attr('type') == 'text' ? 'readonly' : 'disabled';
            if (enable) {
                el.prop(attr, false);
            } else {
                el.prop(attr, true);
            }
        };

        this.showControl = function(el, show) {
            if (show)
                el.show();
            else
                el.hide();
        };

        this.isEnabled = function (el) {
            return !el.prop('disabled') && !el.prop('readonly');
        };

        /// INIT

        return {
            Init: function (settings) {
                _self.dateformat = settings.dateformat;
                _self.urls = settings.urls;
                _self.confirmationDialog = settings.confirmDialog;
                _self.translations = settings.translations;

                _self.setupValidation();

                _self.favoritesSelect$.on('change', function () {
                    var favoriteId = _self.getSelectedFavoriteId();
                    _self.onFavoritesChanged(favoriteId);
                });

                _self.customerSelect$.on('change', function() {
                    var customerId = $(this).val();
                    _self.loadCustomerFields(customerId);
                });

                _self.btnFavorite$.on('click', function () {
                    _self.saveFavorites();
                });

                _self.btnLock$.on('click', function () {
                    _self.lockFormFields(false);

                });

                _self.btnUnLock$.on('click', function () {
                    _self.saveFavorites();
                });

                _self.privacyRunBtn$.on('click', function (e) {
                    e.stopImmediatePropagation();
                    e.preventDefault();

                    _self.runDataPrivacy();
                });

                _self.retentionPeriod$.on('input', function(e) {
                    _self.onRetentionPeriodChanged();
                });

                _self.calculateRegistrationDate$.on('change', function (e) {
                    _self.onCalculateRetentionPeriodChanged(this.checked);
                });
                
                //dateFrom change event
                _self.registerDateFrom$.parent().datepicker().on('changeDate', function (selected) {
                    console.log('>>> registerDateFrom$: change event');
                    _self.updateDateRange();
                });

                //dateTo change event
                _self.registerDateTo$.parent().datepicker().on('changeDate', function (e) {
                    console.log('>>> registerDateTo$: change event');
                    _self.updateDateRange();
                });

                //////////////////////////////////////////
                //save dlg
                $('#btnSaveFav').on("click", function() {
                    var name = $("#fm_name").val();
                    _self.addUpdateFavorites(name);
                });

                $('#btnDeleteFav').on("click", function() {
                    var favoriteId = _self.getSelectedFavoriteId();
                    _self.deleteFavorite(favoriteId);
                });
                
                $("#fm_name").on("input", function() {
                    var val = $(this).val() || '';
                    $('#btnSaveFav').prop('disabled', val === '');
                });

                ////////////////////////////////////////////////////
                //select empty to set defaults 
                _self.onFavoritesChanged(0);
            }
        };
    })(jQuery);

 