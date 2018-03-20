 window.DataPrivacyForm =
    (function($){

        var _self = this;

        //set from init method
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

        this.emptyFavSelectText = "Create New"; //todo: translate
        this.validator$ = {}

        //form and fields
        this.form$ = $("#privacyForm");
        this.customerSelect$ = form$.find("#customerSelect");
        this.favoritesSelect$ = form$.find("#favoritesSelect");
        this.registerDateFrom$ = form$.find("#DataPrivacy_RegisterDateFrom");
        this.registerDateTo$ = form$.find("#DataPrivacy_RegisterDateTo");
        this.filterFields$ = form$.find("#lstFilterFields");
        this.calculateRegistrationDate$ = form$.find("#DataPrivacy_CalculateRegistrationDate");
        this.retentionPeriod$ = form$.find("#retentionPeriod");
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
             lstFilterFields: true
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

         this.setFormFieldsState = function(enable) {
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

         this.getFilterData = function() {
             var fields = [];
             this.filterFields$.find("option:selected").each(function() {
                 fields.push($(this).val());
             });
             return {
                 fields: fields,
                 selectedCustomerId: this.customerSelect$.val(),
                 retentionPeriod: $("#retentionPeriod").val() || "0", //todo: check
                 registerDateFrom: this.registerDateFrom$.val(),
                 calculateRegistrationDate: $("#DataPrivacy_CalculateRegistrationDate").prop("checked"),
                 registerDateTo: this.registerDateTo$.val(),
                 closedOnly: $("#DataPrivacy_ClosedOnly").prop("checked"),
                 replaceDataWith: $("#DataPrivacy_ReplaceDataWith").val(),
                 replaceDatesWith: $("#DataPrivacy_ReplaceDatesWith").val(),
                 removeCaseAttachments: $("#DataPrivacy_RemoveCaseAttachments").prop("checked"),
                 removeLogAttachments: $("#DataPrivacy_RemoveLogAttachments").prop("checked"),
                 removeCaseHistory: $("#DataPrivacy_RemoveCaseHistory").prop("checked")
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
                     } else {
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

             //workaround to trigger validation for date time and choose controls
             [this.registerDateFrom$, this.registerDateTo$].forEach(function(e) {
                 e.on('change',
                     function() {
                         $(this).valid();
                         self.onValidationChanged();
                     });
             });

             this.form$.find(".chosen-select").chosen().change(function() {
                 $(this).valid();
                 self.onValidationChanged();
             });
         };


         this.getRules = function() {
             return {
                 "DataPrivacy.SelectedCustomerId": {
                     required: true
                 },
                 "DataPrivacy.FieldsNames": {
                     required: true
                 },
                 "DataPrivacy.RegisterDateFrom": {
                     required: true
                 },
                 "DataPrivacy.RegisterDateTo": {
                     required: true
                 }
             };
         };

         this.onValidationChanged = function() {
             if (this.validator$) {
                 var numberOfInvalids = this.validator$.numberOfInvalids();

                 var selectedFavId = this.getSelectedFavoriteId();
                 if (selectedFavId > 0) {
                     this.enableControl(this.btnFavorite$, numberOfInvalids === 0);
                 }
             }
         };

         this.loadCustomerFields = function(id) {
             var self = this;
             this.blockUI(true, this.loaders.fieldsLoader);

             var jqXhr = $.ajax({
                    url: self.urls.GetCustomerCaseFieldsAction,
                    type: "POST",
                    data: $.param({ customerId: id }),
                    dataType: "json"
                })
                .done(function(result) {
                    if (result.success) {
                        self.filterFields$.empty();
                        $.each(result.data,
                            function(idx, obj) {
                                self.filterFields$.append(
                                    '<option value="' + obj.Value + '">' + obj.Text + '</option>');
                            });
                        self.filterFields$.trigger("chosen:updated");
                    }
                })
                 .always(function () {
                    self.blockUI(false);
                });

             return jqXhr;
         };

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
                 data: $.param(inputData), //todo: check
                 dataType: "json"
             }).done(function(result) {
                 if (result.success) {
                     window.ShowToastMessage(self.translations.operationSuccessMessage, "success");
                 } //todo: handle erorr?
             }).always(function() {
                 self.blockUI(false);
             });
         };

        this.onFavoritesChanged = function() {
            var favoriteId = this.getSelectedFavoriteId();
            if (favoriteId > 0) {
                this.loadFavoriteFields(favoriteId);
            } else {
                //reset validation errors if New is selected
                this.resetFormFields();
                this.validator$.resetForm();
                this.enableControl(this.btnFavorite$, true);
            }
        };

        this.loadFavoriteFields = function (favId) {
            var self = this;
            this.blockUI(true, this.loaders.favoritesLoader);

            //block UI
            //TODO: 1. Implement loading favorites data into form!!!
            $.getJSON(this.urls.LoadFavoriteDataAction, $.param({ id: favId }))
                .done(function (res) {
                    if (res.data) {

                        //todo: empty form fields?

                        self.populateFormFields(res.data)
                            .done(function () {
                                self.blockUI(false);

                                //check if required
                                self.form$.valid();
                                self.onValidationChanged(); 
                            });

                        //TODO: 2. SET locks (icons) on controls
                    }
                }).always(function() {
                    self.blockUI(false);
                });
        };

        this.setDate = function(ctrl, val) {
            //todo: check if it complete
            if (val) {
                ctrl.datepicker('setDate', val);
            }
        };

        this.resetFormFields = function () {

            self.customerSelect$.val(null);

            self.filterFields$.empty();
            self.refreshFieldsControl();

            self.setDate(registerDateFrom$, null);
            self.setDate(registerDateTo$, null);
            self.retentionPeriod$.val('');
            self.calculateRegistrationDate$.prop('checked', false);
            self.closedOnly$.prop('checked', false);
            self.replaceDataWith$.val('');
            self.setDate(self.replaceDatesWith$, null);
            self.removeCaseAttachments$.prop('checked', false);
            self.removeLogAttachments$.prop('checked', false);
        };

        this.populateFormFields = function(data) {
            var self = this;
            var customerId = data.CustomerId;

            self.customerSelect$.val(customerId);
            self.setDate(registerDateFrom$, data.RegisterDateFrom);
            self.setDate(registerDateTo$, data.RegisterDateTo);

            self.retentionPeriod$.val(data.RetentionPeriod == "0" ? '' : data.RetentionPeriod);
            self.calculateRegistrationDate$.prop('checked', data.CalculateRegistrationDate);
            self.closedOnly$.prop('checked', data.ClosedOnly);
            self.replaceDataWith$.val(data.ReplaceDataWith || '');

            self.setDate(self.replaceDatesWith$, data.ReplaceDatesWith);
            self.removeCaseAttachments$.prop('checked', data.RemoveCaseAttachments);
            self.removeLogAttachments$.prop('checked', data.RemoveLogAttachments);


            var defer = $.Deferred();
            
            this.loadCustomerFields(customerId).done(function () {
                //set case fields
                if (data.FieldsNames.length) {
                    self.filterFields$.val(data.FieldsNames);
                    self.refreshFieldsControl();
                }

                defer.resolve();
            });

            return defer;
        };
        
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

                this.blockUI(true, this.loaders.favoritesLoader);

                var isNew = (+inputData.Id) > 0;
                $.ajax({
                    url: this.urls.SaveFavoritesAction,
                    type: "POST",
                    data: JSON.stringify(inputData),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                }).done(function(res) {
                    if (res.Success) {
                        window.ShowToastMessage(self.translations.operationSuccessMessage, "success"); //todo: check message
                        self.populateFavorites(res.FavoriteId, res.Favorites, isNew);
                    } else {
                        var err = res.Error || 'Unknown error';
                        window.ShowToastMessage(err, "error");
                    }
                }).always(function() {
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
            options = '<option value="0">' + self.emptyFavSelectText + '</option>' + options;

            self.favoritesSelect$.html(options);
            self.favoritesSelect$.val(selectedId);
            if (triggerChange)
                self.favoritesSelect$.change();
        };

         this.getSelectedFavoriteId = function() {
             var val = this.favoritesSelect$.val();
             var favoriteId = val ? parseInt(val, 10) : 0;
             if (!isNaN(favoriteId) && favoriteId > 0)
                 return favoriteId;
             else
                 return 0;
         };
    
        //todo: move to other class 
        this.showSaveFavoritesDlg = function(saveNew) {
            var headerTitle$ = $("#fm_header");
            var bodyDesc$ = $("#fm_body");
            var nameField$ = $("#fm_name");

            //todo: Translate and star icon in the title!
            if (saveNew) {
                headerTitle$.html('New Favorite');
                bodyDesc$.html('Enter a name for your new favorite.');
                nameField$.val('');
                $("#btnSaveFav").prop('disabled', true);
            } else {
                //todo: set required buttons and labels for update
                var selectedFav = _self.favoritesSelect$.find(":selected").text() || '';
                headerTitle$.html('Update favorite - ' + selectedFav);
                bodyDesc$.html('Update your favorite or change the name to save it as a new favorite.');
                nameField$.val(selectedFav);
                $("#btnSaveFav").prop('disabled', selectedFav.length === 0);
            }

            $("#favoritesSaveModal").modal('show');
        };

        this.refreshFieldsControl = function () {
            // required to update chosen control after changes
            this.filterFields$.trigger("chosen:updated"); 
        };

        this.enableControl = function(el, enable) {
            if (enable) {
                el.prop('disabled', false);
            } else {
                el.prop('disabled', true);
            }
        };

        this.showControl = function(el, show) {
            if (show)
                el.show();
            else
                el.hide();
        };

        this.isEnabled = function (el) {
            return !el.prop('disabled');
        };

        //public methods
        return {
            Init: function (settings) {
                _self.urls = settings.urls;
                _self.confirmationDialog = settings.confirmDialog;
                _self.translations = settings.translations;

                _self.setupValidation();

                _self.favoritesSelect$.on('change', function() {
                    _self.onFavoritesChanged();
                });

                _self.customerSelect$.on('change', function() {
                    var customerId = $(this).val();
                    _self.loadCustomerFields(customerId);
                });

                _self.btnFavorite$.on("click", function() {
                    var favId = _self.getSelectedFavoriteId();
                    var isNew = favId === 0;
                    _self.showSaveFavoritesDlg(isNew);
                });

                //////////////////////////////////////////
                //save dlg
                $('#btnSaveFav').on("click", function() {
                    var name = $("#fm_name").val();
                    _self.addUpdateFavorites(name);
                });

                $('#btnDeleteFav').on("click", function() {
                    //todo: implement. show confirm?
                    //_self.deleteFavorites();
                });

                $("#fm_name").on("change paste keyup", function() {
                    var val = $(this).val() || '';
                    $('#btnSaveFav').prop('disabled', val === '');
                });
                //////////////////////////////////////////

                _self.privacyRunBtn$.on("click", function(e) {
                    e.stopImmediatePropagation();
                    e.preventDefault();

                    _self.runDataPrivacy();
                });
            }
        };
    })(jQuery);

 