"use strict";

window.extendedCasePage =
    (function ($) {

        function ExtendedCasePage() {
        };

        /*** CONST BEGIN ***/
        ExtendedCasePage.prototype.CASE_IN_IDLE = 'case_in_idle';
        ExtendedCasePage.prototype.CASE_IN_SAVING = 'case_in_saving';
        ExtendedCasePage.prototype.Ex_Container_Prefix = 'iframe_';
        ExtendedCasePage.prototype.ExTab_Prefix = '#extendedcase-tab-';
        ExtendedCasePage.prototype.ExTab_Indicator_Prefix = '#exTabIndicator_';


        /*** Variables ***/
        ExtendedCasePage.prototype.$caseButtonsToLock = null;
        ExtendedCasePage.prototype.case_Is_In_Saving_Mode = false;
        ExtendedCasePage.prototype.$Form = null;
        ExtendedCasePage.prototype.SAVE_CASE_URL = "";
        ExtendedCasePage.prototype.GET_WORKFLOWFINISHINGCAUSE_URL = "";
        ExtendedCasePage.prototype.Case_Field_Ids = null;
        ExtendedCasePage.prototype.Case_Field_Init_Values = null;
        ExtendedCasePage.prototype.Current_EC_FormId = "";
        ExtendedCasePage.prototype.Current_EC_Guid = "";
        ExtendedCasePage.prototype.Current_EC_LanguageId = "";
        ExtendedCasePage.prototype.Current_EC_Path = "";
        ExtendedCasePage.prototype.UserRole = "";
        ExtendedCasePage.prototype.CurrentUser = "";
        ExtendedCasePage.prototype.CaseStatus = 0;


        /*** Common Area ***/

        ExtendedCasePage.prototype.isNullOrEmpty = function (val) {
            return val == undefined || val === "";
        }

        ExtendedCasePage.prototype.isNullOrUndefined = function (val) {
            return val == undefined;
        }

        ExtendedCasePage.prototype.setValueToBtnGroup = function (domContainer, domText, domValue, value) {
            var self = this;
            var $domValue = $(domValue);
            var oldValue = $domValue.val();
            var el = $(domContainer).find('a[value="' + value + '"]');
            if (el) {
                var _txt = self.getBreadcrumbs(el);
                if (_txt == undefined || _txt === "")
                    _txt = "--";
                $(domText).text(_txt);
                $domValue.val(value);
                if (oldValue !== value) {
                    $domValue.trigger('change');
                }
            }
        }

        ExtendedCasePage.prototype.getBreadcrumbs = function (el) {
            self = this;
            var path = $(el).text();
            var $parent = $(el).parents("li").eq(1).find("a:first");
            if ($parent.length == 1) {
                path = self.getBreadcrumbs($parent) + " - " + path;
            }
            return path;
        }

        ExtendedCasePage.prototype.parseDate = function (dateStr) {
            if (dateStr == undefined || dateStr == "")
                return null;

            var dateArray = dateStr.split("-");
            if (dateArray.length != 3)
                return null;

            return new Date(parseInt(dateArray[0]), parseInt(dateArray[1] - 1), parseInt(dateArray[2]));
        }

        ExtendedCasePage.prototype.dateToDisplayFormat = function (dateValue) {
            var self = this;
            if (Object.prototype.toString.call(dateValue) === "[object Date]") {
                if (isNaN(dateValue.getTime())) {
                    return "";
                }
                else {
                    return dateValue.getFullYear() + "-" +
                        self.padLeft(dateValue.getMonth() + 1, 2, "0") + "-" +
                        self.padLeft(dateValue.getDate(), 2, "0");
                }
            }
            else {
                return "";
            }
        }

        ExtendedCasePage.prototype.padLeft = function (value, totalLength, padChar) {
            var valLen = value.toString().length;
            var diff = totalLength - valLen;
            if (diff > 0) {
                for (var i = 0; i < diff; i++)
                    value = padChar + value;
            }
            return value;
        }

        ExtendedCasePage.prototype.getDate = function (val) {
            if (val == undefined || val === "")
                return null;
            else {
                var dateStr = val.split(' ');
                if (dateStr.length > 0)
                    return new Date(dateStr[0]);
                else
                    return null;
            }
        };

        /*** Extended Case Area ***/
        ExtendedCasePage.prototype.getECContainerTemplate = function (objId, target) {
            return $('<iframe id="' + objId + '"  scrolling="no" frameBorder="0" width="100%" src="' + target + '"></iframe>');
        }

        ExtendedCasePage.prototype.getExtendedCaseContainer = function () {
            var self = this;
            return document.getElementById(self.Ex_Container_Prefix + self.Current_EC_FormId);
        };

        ExtendedCasePage.prototype.getECTargetUrl = function () {
            var path = decodeURIComponent(this.Current_EC_Path.replace(/&amp;/g, '&'));
            return path;
        }

        ExtendedCasePage.prototype.loadExtendedCaseIfNeeded = function () {
            var self = this;

            if (self.isNullOrEmpty(self.Current_EC_FormId)) {
                return;
            }

            var extendedCaseDiv = $('#container_' + self.Current_EC_FormId);
            if (typeof extendedCaseDiv === "undefined" || extendedCaseDiv.length === 0) {
                return;
            }

            var $indicator = $(self.ExTab_Indicator_Prefix + self.Current_EC_FormId);
            $indicator.css("display", "inline-block");

            var $iframe = extendedCaseDiv.next('iframe');
            if ($iframe.length !== 0) {
                $iframe.remove();
            }

            var targetUrl = self.getECTargetUrl();

            var iframeId = self.Ex_Container_Prefix + self.Current_EC_FormId;
            var $placeHolder = self.getECContainerTemplate(iframeId, targetUrl);
            $placeHolder.appendTo(extendedCaseDiv);

            var $elm = document.getElementById(iframeId);
            if (!self.isNullOrUndefined($elm)) {
                var iframeOptions = {
                    log: false,
                    sizeHeight: true,
                    checkOrigin: false,
                    enablePublicMethods: true,
                    resizedCallback: function (messageData) {
                    },
                    bodyMargin: '0 0 0 0',
                    //messageCallback: function (messageData) {
                    //    if (messageData.message === 'cancelCase') {
                    //        var elem = $('#case-action-close');
                    //        location.href = elem.attr('href');
                    //    }
                    //},
                    closedCallback: function (id) {
                    },
                    heightCalculationMethod: 'grow'
                };

                $placeHolder.load(function () {

                    $placeHolder.addClass("hidden2");
                    self.loadExtendedCase(iframeId);
                    $placeHolder.removeClass('hidden2');
                    $placeHolder.iFrameResize(iframeOptions);
                    $indicator.css("display", "none");
                });
            } else {
                $indicator.css("display", "none");
            }
        };

        ExtendedCasePage.prototype.loadExtendedCase = function () {
            var self = this;
            var $_ex_Container = self.getExtendedCaseContainer();

            var formParameters = $_ex_Container.contentWindow.getFormParameters();
            formParameters.languageId = self.Current_EC_LanguageId;
            formParameters.extendedCaseGuid = self.Current_EC_Guid;
            formParameters.caseStatus = self.CaseStatus;
            formParameters.userRole = self.UserRole;
            formParameters.currentUser = self.CurrentUser;
            formParameters.userGuid = '';
            formParameters.caseId = self.Case_Field_Init_Values.CaseId;
            formParameters.caseNumber = self.Case_Field_Init_Values.CaseNumber;
            formParameters.caseGuid = self.Case_Field_Init_Values.CaseGuid;
            formParameters.applicationType = self.ApplicationType;
            formParameters.useInitiatorAutocomplete = self.UseInitiatorAutocomplete;
            formParameters.whiteFilesList = self.whiteFilesList;
            formParameters.maxFileSize = self.maxFileSize;

            $_ex_Container.contentWindow.setInitialData({ step: 0, isNextValidation: false });

            var fieldValues = self.Case_Field_Init_Values;
            if (fieldValues != null) {
                var promise = $_ex_Container.contentWindow.loadExtendedCase(
                    {
                        formParameters: formParameters,
                        caseValues: {
                            administrator_id: { Value: fieldValues.AdministratorId },
                            reportedby: { Value: fieldValues.ReportedBy },
                            persons_name: { Value: fieldValues.PersonsName },
                            persons_phone: { Value: fieldValues.PersonsPhone },
                            usercode: { Value: fieldValues.UserCode },
                            region_id: { Value: fieldValues.RegionId },
                            department_id: { Value: fieldValues.DepartmentId },
                            ou_id_1: { Value: fieldValues.ParentOUId },
                            ou_id_2: { Value: fieldValues.ChildOUId },
                            productarea_id: { Value: fieldValues.ProductAreaId },
                            status_id: { Value: fieldValues.StatusId },
                            subStatus_id: { Value: fieldValues.SubStatusId },
                            plandate: { Value: fieldValues.PlanDate },
                            watchdate: { Value: fieldValues.WatchDate },
                            priority_id: { Value: fieldValues.PriorityId },
                            log_textinternal: { Value: '' },
                            persons_email: { Value: fieldValues.PersonsEmail },
                            persons_cellphone: { Value: fieldValues.PersonsCellphone },
                            place: { Value: fieldValues.Place },
                            costcentre: { Value: fieldValues.CostCentre },
                            caption: { Value: fieldValues.Caption },
                            inventorytype: { Value: fieldValues.InventoryType },
                            inventorylocation: { Value: fieldValues.InventoryLocation },
                            case_files: { Value: fieldValues.CaseFiles }
                        }
                    });
                promise.then(function () { self.onExtendedCaseLoaded() });
            } else {
                var promise = $_ex_Container.contentWindow.loadExtendedCase(
                    {
                        formParameters: formParameters, caseValues: {
                            log_textinternal: { Value: '' }
                        }
                    });
                promise.then(function () { self.onExtendedCaseLoaded() });
            }
        }

        ExtendedCasePage.prototype.isExtendedCaseValid = function (showToast, isOnNext, finishingCauseId = 0) {
            var self = this;

            //if no input param sent in, set show toast to true
            if (showToast == null) {
                showToast = true;
            }

            //if no input param sent in, set isOnNext to false
            if (isOnNext == null) {
                isOnNext = false;
            }

            var $exTab = $(self.ExTab_Prefix + self.Current_EC_FormId);
            var $exCaseContainer = self.getExtendedCaseContainer();


            var validationResult = $exCaseContainer.contentWindow.validateExtendedCase(isOnNext, finishingCauseId);

            if (validationResult == null) {
                //Change color
                if ($exTab.parent().hasClass('error')) {
                    $exTab.parent().removeClass('error');
                }
                return true;
            } else {
                //Change color      
                if (!$exTab.parent().hasClass('error')) {
                    $exTab.parent().addClass('error');
                }

                if (showToast) {
                    ShowToastMessage(self.extendedCaseInvalidMessage + '!', "error", false);
                }
                return false;
            }
        };

        ExtendedCasePage.prototype.setNextStep = function (nextStepNumber) {
            var self = this;
            var isNextStepValidation = false;

            if (!isNaN(nextStepNumber) && nextStepNumber > 0)
                isNextStepValidation = true;

            var $exCaseContainer = self.getExtendedCaseContainer();
            $exCaseContainer.contentWindow.setNextStep(nextStepNumber, isNextStepValidation);
        };

        ExtendedCasePage.prototype.syncCaseFromExCaseIfExists = function () {
            var self = this;

            var $exCaseContainer = self.getExtendedCaseContainer();
            if ($exCaseContainer == undefined) {
                return;
            }

            var fieldData = $exCaseContainer.contentWindow.getCaseValues();
            if (fieldData == undefined) {
                return;
            }

            var _caseFields = self.Case_Field_Ids;

            var _adminstratorId = fieldData.administrator_id;
            var _reportedby = fieldData.reportedby;
            var _persons_name = fieldData.persons_name;
            var _persons_phone = fieldData.persons_phone;
            var _usercode = fieldData.usercode;
            var _region_id = fieldData.region_id;
            var _department_id = fieldData.department_id;
            var _ou_id_1 = fieldData.ou_id_1;
            var _ou_id_2 = fieldData.ou_id_2;
            var _productarea_id = fieldData.productarea_id;
            var _status_id = fieldData.status_id;
            var _subStatus_id = fieldData.subStatus_id;
            var _plandate = fieldData.plandate;
            var _watchdate = fieldData.watchdate;
            var _log_textinternal = fieldData.log_textinternal;
            var _priority_id = fieldData.priority_id;
            var _persons_email = fieldData.persons_email;
            var _persons_cellphone = fieldData.persons_cellphone;
            var _place = fieldData.place;
            var _costcentre = fieldData.costcentre;
            var _caption = fieldData.caption;
            var _inventorytype = fieldData.inventorytype;
            var _inventorylocation = fieldData.inventorylocation;

            if (_adminstratorId != undefined)
                $('#' + _caseFields.AdministratorId).val(_adminstratorId.Value);

            if (_reportedby != undefined)
                $('#' + _caseFields.ReportedBy).val(_reportedby.Value);

            if (_persons_name != undefined)
                $('#' + _caseFields.PersonsName).val(_persons_name.Value);

            if (_persons_phone != undefined)
                $('#' + _caseFields.PersonsPhone).val(_persons_phone.Value);

            if (_usercode != undefined)
                $('#' + _caseFields.UserCode).val(_usercode.Value);

            if (_log_textinternal != undefined)
                $('#' + _caseFields.log_InternalText).val(_log_textinternal.Value);

            if (_department_id != undefined) {
                $('#' + _caseFields.DepartmentId).val(_department_id.Value);
            }

            if (_ou_id_1 != undefined) {
                var selectedOU_Id = _ou_id_1.Value;

                if (_ou_id_2 != undefined && _ou_id_2.Value != null) {
                    selectedOU_Id = _ou_id_2.Value;
                }

                $('#' + _caseFields.OUId).val(selectedOU_Id);
            }

            if (_region_id != undefined) {
                $('#' + _caseFields.RegionId).val(_region_id.Value);
            }

            if (_priority_id != undefined) {
                $('#' + _caseFields.PriorityId).val(_priority_id.Value);
            }

            if (_productarea_id != undefined) {
                $('#' + _caseFields.ProductAreaId).val(_productarea_id.Value);
            }

            if (_status_id != undefined) {
                $('#' + _caseFields.StatusId).val(_status_id.Value).change();
            }

            if (_subStatus_id !== undefined) {
                $('#' + _caseFields.SubStatusId).val(_subStatus_id.Value).change();
                $('#' + _caseFields.SubStatusName).val(_subStatus_id.SecondaryValue);
            }

            if (_plandate && _plandate.Value) {
                $('#' + _caseFields.PlanDate).val(_plandate.Value);
            }

            if (_watchdate && _watchdate.Value) {
                $('#' + _caseFields.WatchDate).val(_watchdate.Value);
            }

            if (_department_id != undefined && _priority_id != undefined) {
                $('#' + _caseFields.PriorityId).trigger('change');
            };

            if (_persons_email != undefined)
                $('#' + _caseFields.PersonsEmail).val(_persons_email.Value);

            if (_persons_cellphone != undefined)
                $('#' + _caseFields.PersonsCellphone).val(_persons_cellphone.Value);

            if (_place != undefined)
                $('#' + _caseFields.Place).val(_place.Value);

            if (_costcentre != undefined)
                $('#' + _caseFields.CostCentre).val(_costcentre.Value);

            if (_caption != undefined)
                $('#' + _caseFields.Caption).val(_caption.Value);

            if (_inventorytype != undefined)
                $('#' + _caseFields.InventoryType).val(_inventorytype.Value);

            if (_inventorylocation != undefined)
                $('#' + _caseFields.InventoryLocation).val(_inventorylocation.Value);
        }

        ExtendedCasePage.prototype.setCaseStatus = function (status) {
            var self = this;
            switch (status) {
                case self.CASE_IN_IDLE:
                    self.case_Is_In_Saving_Mode = false;
                    self.$caseButtonsToLock.removeClass('disabled');
                    self.$caseButtonsToLock.css("pointer-events", "");
                    return true;

                case self.CASE_IN_SAVING:
                    self.case_Is_In_Saving_Mode = true;
                    self.$caseButtonsToLock.addClass('disabled');
                    self.$caseButtonsToLock.css("pointer-events", "none");
                    return true;

                default:
                    ShowToastMessage("Case status is not defined!", "error", true);
                    return false;
            }
        };

        ExtendedCasePage.prototype.doSaveCase = function (submitUrl) {
            var self = this;

            self.$Form.attr("action", submitUrl);
            self.syncCaseFromExCaseIfExists();
            self.$Form.submit();
        };

        ExtendedCasePage.prototype.onSaveClick = function () {
            var self = this;
            var id = self.Case_Field_Init_Values.CaseId;
            var url = self.SAVE_CASE_URL;
            var $exCaseContainer = self.getExtendedCaseContainer();

            if (recaptchaKey !== "" && id === 0) {
                var res = captchaChecker();
                if (res == "") {
                    ShowToastMessage(recaptchaMessage, "warning", false);
                    $('html,body').animate({ scrollTop: 9999 }, 'slow');
                    return;
                }
            }

            self.setCaseStatus(self.CASE_IN_SAVING);

            if (!self.isNullOrUndefined($exCaseContainer)) {
                var promise = $exCaseContainer.contentWindow.saveExtendedCase(false);
                promise.then(function (res) {
                    self.doSaveCase(url);
                }, function (err) {
                    self.onSaveError(err);
                });
                return true;
            } else {
                self.setCaseStatus(self.CASE_IN_IDLE);
                ShowToastMessage("Can not find Extended Case form!", "error", true);
                return false;
            }
        };

        ExtendedCasePage.prototype.onSaveError = function (err) {
            ShowToastMessage("Error in save extended case!", "error", false);
            return false;
        }

        ExtendedCasePage.prototype.onExtendedCaseLoaded = function () {
            var self = this;
            var $indicator = $(self.ExTab_Indicator_Prefix + self.Current_EC_FormId);
            /*temp disabled - Majid/Alex */
            //self.setNextStep();
            $indicator.css("display", "none");
        };

        /***** Initiator *****/
        ExtendedCasePage.prototype.init = function (params) {
            var self = this;

            self.GET_WORKFLOWFINISHINGCAUSE_URL = params.getWorkflowFinishingCauseUrl;
            self.SAVE_CASE_URL = params.saveCaseUrl;
            self.Case_Field_Ids = params.caseFieldIds;
            self.Case_Field_Init_Values = params.caseInitValues;
            self.Current_EC_FormId = params.extendedCaseFormId;
            self.Current_EC_Guid = params.extendedCaseGuid;
            self.Current_EC_LanguageId = params.extendedCaseLanguageId;
            self.UserRole = params.userRole;
            self.CaseStatus = params.caseStatus;
            self.CurrentUser = params.currentUser;
            self.Current_EC_Path = params.extendedCasePath;
            self.ApplicationType = params.applicationType;
            self.UseInitiatorAutocomplete = params.useInitiatorAutocomplete;
            self.extendedCaseInvalidMessage = params.extendedCaseInvalidMessage;
            self.whiteFilesList = params.whiteFilesList;
            self.maxFileSize = params.maxFileSize;
            var lastError = params.lastError;
            var lastClickTimeStamp = null;
            var nextAllowedClickDelay = 5000;

            // controls binding
            self.$caseButtonsToLock = $('input.save-button, input.go-button');
            self.$Form = $('#extendedCaseForm');
            self.$caseTab = $("#tabsArea li a");
            self.$selectedWorkflow = $('#SelectedWorkflowStep');
            self.$selectListStep = $('select[name="steps"]').first(); //should first only

            self.loadExtendedCaseIfNeeded();

            //SAVE
            $('.save-button').on('click', function (e) {
                e.preventDefault();

                if (lastClickTimeStamp == null || lastClickTimeStamp + nextAllowedClickDelay < event.timeStamp) {
                    lastClickTimeStamp = e.timeStamp;
                } else {
                    return;
                }

                // validate before save
                var isValid = self.isExtendedCaseValid(true, false);
                if (isValid) {
                    self.onSaveClick(self);
                }
            });

            // Go - Change Workflow
            $('.go-button').on("click", function (e) {
                e.preventDefault();

                if (lastClickTimeStamp == null || lastClickTimeStamp + nextAllowedClickDelay < event.timeStamp) {
                    lastClickTimeStamp = e.timeStamp;
                } else {
                    return;
                }

                var templateId = parseInt(self.$selectListStep.first().val()) || 0;

                var finishingCauseId = 0;

                if (templateId > 0) {
                    var url = self.GET_WORKFLOWFINISHINGCAUSE_URL + "?templateId=" + templateId;

                    //Do ajax
                    $.ajax({
                        type: "GET",
                        url: url,
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (result) {
                            finishingCauseId = result.Data;
                        },
                        error: function (result) {
                            finishingCauseId = result.Data;
                            if (result.Error) {
                                ShowToastMessage(result.Error, "error", false);
                            }
                            else {
                                ShowToastMessage("Error", "error", false);
                            }

                        }
                    });

                    if (finishingCauseId == null || finishingCauseId == "" || typeof finishingCauseId == 'undefined' || finishingCauseId == NaN) {
                        finishingCauseId = 0;
                    }
                }


                //only load if templateId exist
                if (templateId > 0) {
                    var isValid = false;

                    var stepId = parseInt(self.$selectListStep.first().val()) || 0;
                    if (stepId > 0) {
                        isValid = self.isExtendedCaseValid(true, true, finishingCauseId);
                    }
                    else {
                        isValid = self.isExtendedCaseValid(true, false, finishingCauseId);
                    }

                    if (isValid) {
                        self.$selectedWorkflow.val(templateId);
                        self.onSaveClick(self);
                    }
                    else {
                        self.$selectedWorkflow.val(0);
                    }
                }
            });

            self.$caseTab.on('shown', function (e) {
                window.scrollTo(0, 0);
            });

            if (lastError !== "")
                ShowToastMessage(lastError, "error", false);
        };

        return new ExtendedCasePage();
    })(jQuery);


