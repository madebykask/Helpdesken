"use strict";

function EditPage() { };

var caseButtonsToLock = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
    '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
    '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton');


/*** CONST BEGIN ***/
EditPage.prototype.DELETE_CASE_URL = '/Cases/DeleteCase';
EditPage.prototype.NEW_CLOSE_CASE_URL = '/Cases/NewAndClose';
EditPage.prototype.NEW_CLOSE_SPLIT_URL = '/Cases/NewCloseAndSplit';
EditPage.prototype.EDIT_CLOSE_SPLIT_URL = '/Cases/EditCloseAndSplit';
EditPage.prototype.EDIT_CASE_URL = '/Cases/Edit';
EditPage.prototype.SAVE_GOTO_PARENT_CASE_URL = '/Cases/NewAndGotoParentCase';
EditPage.prototype.SAVE_ADD_CASE_URL = '/Cases/NewAndAddCase';
EditPage.prototype.CASE_OVERVIEW_URL = '/Cases';

EditPage.prototype.CHLID_CASES_TAB = 'childcases-tab';
EditPage.prototype.CASE_IN_IDLE = 'case_in_idle';
EditPage.prototype.CASE_IN_SAVING = 'case_in_saving';
EditPage.prototype.Ex_Container_Prefix = 'iframe_';
EditPage.prototype.ExTab_Prefix = '#extendedcase-tab';
EditPage.prototype.ExTab_Indicator_Prefix = '#exTabIndicator_';

/*** CONST END ***/

/*** Variables ***/
EditPage.prototype.Case_Field_Ids = null;
EditPage.prototype.Case_Field_Init_Values = null;
EditPage.prototype.Current_EC_FormId = "";
EditPage.prototype.Current_EC_Guid = "";
EditPage.prototype.Current_EC_LanguageId = "";
EditPage.prototype.Current_EC_Path = "";

EditPage.prototype.Contains_Eform = false; //DH+


/*** Common Area ***/

EditPage.prototype.isNullOrEmpty = function (val) {
    return val == undefined || val == null || val == "";
}

EditPage.prototype.isNullOrUndefined = function (val) {
    return val == undefined || val == null;
}

EditPage.prototype.setValueToBtnGroup = function (domContainer, domText, domValue, value) {
    var self = this;
    var $domValue = $(domValue);
    var oldValue = $domValue.val();
    var el = $(domContainer).find('a[value="' + value + '"]');
    if (el) {
        var _txt = self.getBreadcrumbs(el);
        if (_txt == undefined || _txt == "")
            _txt = "--";
        $(domText).text(_txt);
        $domValue.val(value);
        if (oldValue !== value) {
            $domValue.trigger('change');
        }
    }
}

EditPage.prototype.getBreadcrumbs = function (el) {
    self = this;
    var path = $(el).text();
    var $parent = $(el).parents("li").eq(1).find("a:first");
    if ($parent.length == 1) {
        path = self.getBreadcrumbs($parent) + " - " + path;
    }
    return path;
}

EditPage.prototype.parseDate = function (dateStr) {
    if (dateStr == undefined || dateStr == "")
        return null;

    var dateArray = dateStr.split("-");
    if (dateArray.length != 3)
        return null;

    return new Date(parseInt(dateArray[0]), parseInt(dateArray[1] - 1), parseInt(dateArray[2]));
}

EditPage.prototype.dateToDisplayFormat = function (dateValue) {
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

EditPage.prototype.padLeft = function (value, totalLength, padChar) {
    var valLen = value.toString().length;
    var diff = totalLength - valLen;
    if (diff > 0) {
        for (var i = 0; i < diff; i++)
            value = padChar + value;
    }
    return value;
}

EditPage.prototype.getDate = function (val) {
    if (val === undefined || val === null || val === "")
        return null;
    else {
        var dateStr = val.split(' ');
        if (dateStr.length > 0)
            return new Date(dateStr[0]);
        else
            return null;
    }
};

EditPage.prototype.ReturnFalse = function () {
    return false;
};

/*** Extended Case Area ***/
EditPage.prototype.getECContainerTemplate = function (objId, target) {
    return $('<iframe id="' + objId + '"  scrolling="no" frameBorder="0" width="100%" src="' + target + '"></iframe>');
}

EditPage.prototype.getExtendedCaseContainer = function () {
    var self = this;
    return document.getElementById(self.Ex_Container_Prefix + self.Current_EC_FormId);
};

EditPage.prototype.getECTargetUrl = function () {
    var self = this;
    var path = self.Current_EC_Path;
    path = path.replace('[ExtendedCaseFormId]', self.Current_EC_FormId);
    return decodeURIComponent(path.replace(/&amp;/g, '&'));
}

EditPage.prototype.getExtendedCaseSectionUrl = function (path, formID) {
    path = path.replace('[ExtendedCaseFormId]', formID);
    return decodeURIComponent(path.replace(/&amp;/g, '&'));
}

EditPage.prototype.loadExtendedCaseIfNeeded = function () {
    "use strict";
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

    self.ieVer = CommonUtils.detectIE();
    var $elm = document.getElementById(iframeId);

    if (!self.isNullOrUndefined($elm)) {

        self.exCaseFrameHeight = +$elm.style.height;
        var iframeOptions = {
            log: false,
            sizeHeight: true,
            checkOrigin: false,
            enablePublicMethods: true,
            resizedCallback: function (messageData) {
                if (!self.ieVer.IE)
                    return;

                var newHeight = messageData && messageData.hasOwnProperty('height') ? +messageData.height : 0;
                var prevHeight = self.exCaseFrameHeight || 0;
                //console.log('@resizedCallback: prevHeight: ' + prevHeight.toString() + ', newHeight: ' + newHeight.toString());
                if (newHeight < prevHeight) {
                    //restore max height
                    messageData.iframe.style.height = prevHeight + 'px';
                } else {
                    //save last max height
                    self.exCaseFrameHeight = newHeight;
                }
            },
            bodyMargin: '0 0 0 0',
            closedCallback: function (id) { },
            heightCalculationMethod: 'grow'
        };

        $placeHolder.load(function () {
            $placeHolder.addClass("hidden2");
            self.loadExtendedCase(iframeId);
            $placeHolder.removeClass('hidden2');
            $placeHolder.iFrameResize(iframeOptions);
        });
    } else {
        $indicator.css("display", "none");
    }
};

EditPage.prototype.loadExtendedSectionsIfNeeded = function () {
    var self = this;

    // Todo refactor and automate all sections
    //  if (self.extendedSections.length > 0) {
    if (self.extendedSections) {

        //initiator
        var initiatorSection = self.extendedSections.Initiator;
        if (initiatorSection) {
            var $initiatorFrame = $(initiatorSection.iframeId);
            $initiatorFrame.on('load', function () {
                self.processExtendedSectionLoad($(this), initiatorSection);
            });
            $initiatorFrame[0].src = initiatorSection.path;
        }

        //regarding 
        var regardingSection = self.extendedSections.Regarding;
        if (regardingSection) {
            var $regardingFrame = $(regardingSection.iframeId);
            $regardingFrame.on('load', function () {
                self.processExtendedSectionLoad($(this), regardingSection);
            });
            $regardingFrame[0].src = regardingSection.path;
        }
    }
}

// This method only processes extended case loading to set helpdesk existing data. 
// Extended case url and loading is implemented in caseInitForm.js\loadExtendedCaseSection()
EditPage.prototype.processExtendedSectionLoad = function ($frame, extendedSection) {
    if ($frame.attr('src').length) {
        var frame = $frame[0];
        var isLockedValue = window.parameters.isCaseLocked || '';
        var formParameters = frame.contentWindow.getFormParameters();
        formParameters.languageId = extendedSection.languageId;
        formParameters.extendedCaseGuid = extendedSection.guid;
        formParameters.isCaseLocked = isLockedValue.toLowerCase() === 'true'; //important to pass boolean type value
        frame.contentWindow.setInitialData({ step: 0, isNextValidation: false }); //todo: check if required

        var fieldValues = self.Case_Field_Init_Values;
        if (fieldValues != null) {
            var pr = frame.contentWindow.loadExtendedCase({
                formParameters: formParameters,
                caseValues: {
                    reportedby: { Value: $('#case__ReportedBy').val() },
                }
            });
            pr.then(function () {
                frame.iFrameResizer.resize();
            });
        }
        $(extendedSection.container).show();
    }
}

EditPage.prototype.loadExtendedCase = function () {
    var self = this;
    var $_ex_Container = self.getExtendedCaseContainer();
    var formParameters = $_ex_Container.contentWindow.getFormParameters();
    var window_params = window.parameters;

    formParameters.languageId = self.Current_EC_LanguageId;
    formParameters.extendedCaseGuid = self.Current_EC_Guid;
    formParameters.caseId = window_params.currentCaseId;
    formParameters.caseNumber = window_params.currentCaseNumber;
    formParameters.caseGuid = window_params.currentCaseGuid;
    formParameters.currentUser = window_params.currentUserName;
    formParameters.applicationType = window_params.applicationType;
    formParameters.whiteFilesList = window_params.whiteFilesList;
    formParameters.maxFileSize = window_params.maxFileSize;

    var isLockedValue = window_params.isCaseLocked || '';
    formParameters.isCaseLocked = isLockedValue.toLowerCase() === 'true'; //important to pass boolean type value

    var fieldValues = self.Case_Field_Init_Values;

    $_ex_Container.contentWindow.setInitialData({ step: 0, isNextValidation: false });
    
    if (fieldValues != null) {
        $_ex_Container.contentWindow.loadExtendedCase(
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
                    case_relation_type: { Value: fieldValues.CaseRelationType },
                    persons_email: { Value: fieldValues.PersonsEmail },
                    persons_cellphone: { Value: fieldValues.PersonsCellphone },
                    place: { Value: fieldValues.Place },
                    costcentre: { Value: fieldValues.CostCentre },
                    caption: { Value: fieldValues.Caption },
                    inventorytype: { Value: fieldValues.InventoryType },
                    inventorylocation: { Value: fieldValues.InventoryLocation },
                    case_files: { Value: fieldValues.CaseFiles }
                }
            }).then(function () {
                self.onExtendedCaseLoaded();
            });
    } else {
        $_ex_Container.contentWindow.loadExtendedCase(
            {
                formParameters: formParameters, caseValues: {
                    log_textinternal: { Value: '' }
                }
            }).then(function () {
                self.onExtendedCaseLoaded();
            });
    }
}

EditPage.prototype.isExtendedCaseValid = function (showToast, isOnNext) {

    var self = this;

    //only if extended case exist

    if (window.parameters.containsExtendedCase === "True") {


        //if no input param sent in, set show toast to true
        if (showToast == null) {
            showToast = true;
        }

        //if no input param sent in, set isOnNext to false
        if (isOnNext == null) {
            isOnNext = false;
        }


        var $exTab = $(self.ExTab_Prefix + self.Current_EC_FormId);

        if ($('#steps').length && $('#ButtonClick').length && $('#ButtonClick').val() == 'btn-go') {
            //check if value is selected in steps, then isOnNext should be true;
            var templateId = parseInt($('#steps').val()) || 0;
            //only load if templateId exist
            if (templateId > 0) {

                isOnNext = true;
            }
        }

        var finishingType = parseInt(document.getElementById("CaseLog_FinishingType").value);

        if (isNaN(finishingType)) {
            finishingType = 0;
        }


        var $_ex_Container = self.getExtendedCaseContainer();
        var validationResult = $_ex_Container.contentWindow.validateExtendedCase(isOnNext, finishingType);

        
        
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
                var tabName = ($('a#extendedcase-tab').text() || '').trim();
                ShowToastMessage(self.extendedCaseInvalidMessage + '! (' + tabName + ')', "error", false);
            }
            return false;
        }
    }
    else {
        return true;
    }
};

EditPage.prototype.setNextStep = function () {

    var self = this;

    if (window.parameters.containsExtendedCase == "True") {
        var nextStep = 0;
        var tempId = $("#steps option:selected");
        nextStep = parseInt($("#steps option:selected").attr('data-next-step')) || 0;

        var $_ex_Container = self.getExtendedCaseContainer();

        var isNextStepValidation = true;

        if (nextStep == 0) {
            isNextStepValidation = false;
        }

        $_ex_Container.contentWindow.setNextStep(nextStep, isNextStepValidation);
    }
};

EditPage.prototype.syncCaseFromExCaseIfExists = function () {
    var self = this;

    var $_ex_Container = self.getExtendedCaseContainer();
    if ($_ex_Container == undefined) {
        return;
    }

    var fieldData = $_ex_Container.contentWindow.getCaseValues();
    if (fieldData == undefined) {
        return;
    }

    var _caseFields = self.Case_Field_Ids;

    var _administratorId = fieldData.administrator_id;
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

    if (_administratorId != undefined) {
        $('#' + _caseFields.AdministratorId).val(_administratorId.Value);
        $('#' + _caseFields.AdministratorId).trigger('change');  
    }

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

    if (_region_id != undefined) {
        $('#' + _caseFields.RegionId).val(_region_id.Value);
        $('#' + _caseFields.RegionName).val(_region_id.SecondaryValue);
    }

    if (_department_id != undefined) {
        $("#CaseTemplate_Department_Id").val('');

        if (_department_id != "0") {
            $("#CaseTemplate_Department_Id").val(_department_id.Value);
        }

        $('#' + _caseFields.DepartmentId).val(_department_id.Value);
        $('#' + _caseFields.DepartmentName).val(_department_id.SecondaryValue);
    }

    if (_caption != undefined)
        $('#' + _caseFields.Caption).val(_caption.Value);

    var selectedOU_Id = '';
    var selectedOU_Name = '';

    if (_ou_id_1 != undefined) {
        selectedOU_Id = _ou_id_1.Value;
        selectedOU_Name = _ou_id_1.SecondaryValue;

        if (selectedOU_Name == null)
            selectedOU_Name = "";

        var ouId2Val = _ou_id_2 != undefined && _ou_id_2 != null ? _ou_id_2.Value : "";
        if (ouId2Val != null && ouId2Val != "" && _ou_id_2.SecondaryValue != null) {
            selectedOU_Id = ouId2Val;
            if (_ou_id_2.SecondaryValue != null)
                selectedOU_Name = selectedOU_Name + ' - ' + _ou_id_2.SecondaryValue;
        }

        if (selectedOU_Id != "0") {
            $("#CaseTemplate_OU_Id").val(selectedOU_Id);
        }

        //console.log(">>> OU changed to: " + selectedOU_Id);

        var $ouSelect = $('#' + _caseFields.OUId);

        if ($ouSelect.length && selectedOU_Id != "" && selectedOU_Name != "") {
            //add option to the list if it doesn't exist yet
            if ($ouSelect.find("option[value=" + selectedOU_Id + "]").length === 0)
                $ouSelect.append("<option value='" + selectedOU_Id + "' selected>" + selectedOU_Name + "</option>");
        }

        $ouSelect.val(selectedOU_Id);
        $('#' + _caseFields.OUName).val(selectedOU_Name);
    }

    //trigger department change to load child OUs items
    if (_department_id != undefined && _department_id != "") {
        $('#' + _caseFields.DepartmentId).trigger('change');
    }

    if (_priority_id != undefined) {
        $('#' + _caseFields.PriorityId).val(_priority_id.Value);
        $('#' + _caseFields.PriorityName).val(_priority_id.SecondaryValue);
    }

    if (_department_id != undefined && _priority_id != undefined) {
        $('#' + _caseFields.PriorityId).trigger('change');
    };

    if (_productarea_id != undefined) {
        self.setValueToBtnGroup('#divProductArea',
            "#divBreadcrumbs_ProductArea",
            '#' + _caseFields.ProductAreaId,
            _productarea_id.Value);
    }

    if (_status_id !== undefined) {
        $('#' + _caseFields.StatusId).val(_status_id.Value).change();
        $('#' + _caseFields.StatusName).val(_status_id.SecondaryValue);
    }

    if (_subStatus_id !== undefined) {
        $('#' + _caseFields.SubStatusId).val(_subStatus_id.Value).change();
        $('#' + _caseFields.SubStatusName).val(_subStatus_id.SecondaryValue);
    }

    if (_plandate != undefined) {
        var _date = self.parseDate(_plandate.Value);
        if (_date != null) {
            if ($('#' + _caseFields.PlanDate).is('[readonly]'))
                $('#' + _caseFields.PlanDate).val(self.dateToDisplayFormat(_date));
            else
                $('#' + _caseFields.PlanDate).datepicker({
                    format: "yyyy-mm-dd",
                    autoclose: true
                }).datepicker('setDate', _date);
        }
    }

    if (_watchdate != undefined) {
        var _date = self.parseDate(_watchdate.Value);

        if (_date != null) {
            if ($('#' + _caseFields.WatchDate).is('[readonly]'))
                $('#' + _caseFields.WatchDate).val(self.dateToDisplayFormat(_date));
            else
                $('#' + _caseFields.WatchDate).datepicker({
                    format: "yyyy-mm-dd",
                    autoclose: true
                }).datepicker('setDate', _date);
        }
    }

    if (_persons_email != undefined)
        $('#' + _caseFields.PersonsEmail).val(_persons_email.Value);

    if (_persons_cellphone != undefined)
        $('#' + _caseFields.PersonsCellphone).val(_persons_cellphone.Value);

    if (_place != undefined)
        $('#' + _caseFields.Place).val(_place.Value);

    if (_costcentre != undefined)
        $('#' + _caseFields.CostCentre).val(_costcentre.Value);

    if (_inventorytype != undefined)
        $('#' + _caseFields.InventoryType).val(_inventorytype.Value);

    if (_inventorylocation != undefined)
        $('#' + _caseFields.InventoryLocation).val(_inventorylocation.Value);

}

EditPage.prototype.propagateLogNote = function () {
    var data = $(this).val();
    var propagateMessage = { log_textinternal: { Value: data } };

    var $_ex_Container = EditPage.prototype.getExtendedCaseContainer();
    if ($_ex_Container != null) {
        $_ex_Container.contentWindow.updateCaseFields(propagateMessage);
    }
};

EditPage.prototype.onExtendedCaseLoaded = function () {
    var self = this;
    window.parameters.containsExtendedCase = "True";
    $("#ContainsExtendedCase").val("True");
    var $indicator = $(self.ExTab_Indicator_Prefix + self.Current_EC_FormId);
    /*Disabled because form is not ready yet - Majid/Alex */
    //self.setNextStep();
    $indicator.css("display", "none");
};

EditPage.prototype.refreshCasePage = function (updatedInfo) {
    if (updatedInfo == null)
        return;

    var self = this;
    var _caseFields = self.Case_Field_Ids;

    self.changeCaseButtonsState(false);

    $('#' + _caseFields.ReportedBy).val(updatedInfo.ReportedBy);
    $('#' + _caseFields.PersonsName).val(updatedInfo.PersonsName);
    $('#' + _caseFields.PersonsPhone).val(updatedInfo.PersonsPhone);


    var caseTypeId$ = $('#' + _caseFields.CaseTypeId);
    if (caseTypeId$.val() != updatedInfo.CaseType_Id) {
        caseTypeId$.val(updatedInfo.CaseType_Id).change();
    }

    var productAreaId$ = $('#' + _caseFields.ProductAreaId);
    if (productAreaId$.val() != updatedInfo.ProductArea_Id) {
        productAreaId$.val(updatedInfo.ProductArea_Id).change();
    }

    //trigger change only if value has been changed
    var workingGroupId$ = $('#' + _caseFields.WorkingGroupId);
    if (workingGroupId$.val() != updatedInfo.WorkingGroup_Id) {
        workingGroupId$.val(updatedInfo.WorkingGroup_Id).change();
    }

    var priorityId$ = $('#' + _caseFields.PriorityId);
    if (priorityId$.val() != updatedInfo.Priority_Id) {
        priorityId$.val(updatedInfo.Priority_Id).change();
    }

    $('#' + _caseFields.WorkingGroupName).val(updatedInfo.WorkingGroupName);
    $('#' + _caseFields.PersonsEmail).val(updatedInfo.PersonsEmail);
    $('#' + _caseFields.PersonsCellphone).val(updatedInfo.PersonsCellphone);
    $('#' + _caseFields.Place).val(updatedInfo.Place);
    $('#' + _caseFields.CostCentre).val(updatedInfo.CostCentre);

    //state (status)
    $('#' + _caseFields.SubStatusId).val(updatedInfo.StateSecondary_Id);
    var subStateName$ = $('#' + _caseFields.SubStatusName);
    if (subStateName$.length && updatedInfo.SubStateName)
        subStateName$.val(updatedInfo.SubStateName);

    $('#' + _caseFields.PlanDate).datepicker({
        format: updatedInfo.DateFormat.toLowerCase(),
        autoclose: true
    }).datepicker('setDate', updatedInfo.PlanDateJS);

    $('#' + _caseFields.WatchDate).datepicker({
        format: updatedInfo.DateFormat.toLowerCase(),
        autoclose: true
    }).datepicker('setDate', updatedInfo.WatchDateJS);

    $("#CaseTemplate_Department_Id").val();
    $("#CaseTemplate_OU_Id").val();

    if (updatedInfo.Department_Id != null && updatedInfo.Department_Id != 0) {
        $("#CaseTemplate_Department_Id").val(updatedInfo.Department_Id);
    }

    if (updatedInfo.OU_Id != null && updatedInfo.OU_Id != 0) {
        $("#CaseTemplate_OU_Id").val(updatedInfo.OU_Id);
    }

    var regionId$ = $('#' + _caseFields.RegionId);
    if (regionId$.val() != updatedInfo.Region_Id) {
        regionId$.val(updatedInfo.Region_Id).change();
    }

    $('#' + _caseFields.RegionName).val(updatedInfo.RegionName);
    $('#' + _caseFields.DepartmentName).val(updatedInfo.DepartmentName);
    $('#' + _caseFields.OUName).val(updatedInfo.OUName);

    $.get('/Cases/GetCaseFilesJS', { caseId: updatedInfo.Id, now: Date.now() }, function (data) {
        $('#divCaseFiles').html(data);
        $(document).trigger("OnUploadedCaseFileRendered", []);
        bindDeleteCaseFileBehaviorToDeleteButtons();
    });

    $.get('/Cases/GetCaseInputModelForLog', { caseId: updatedInfo.Id, now: Date.now() }, function (data) {
        $('#logtab').html(data);
    }).done(function () {

    });

    $.get('/Cases/GetCaseInputModelForHistory', { caseId: updatedInfo.Id, now: Date.now() }, function (data) {
        $('#' + _caseFields.StatusId).val(updatedInfo.Status_Id).change();
        $('#' + _caseFields.StatusName).val(updatedInfo.StatusName).change();
        $('#' + _caseFields.SubStatusId).val(updatedInfo.StateSecondary_Id).change();
        $(".readonlySubstate").val(updatedInfo.StateSecondary_Id);
        $('#' + _caseFields.SubStatusName).val(updatedInfo.SubStateName);
        $('#historytab').html(data);
    }).done(function () {
        self.changeCaseButtonsState(true);
    });
}

EditPage.prototype.fetchWatchDateByDept = function (deptId) {
    var me = this;
    if (deptId == null) {
        return;
    }

    $.getJSON(
        '/cases/GetWatchDateByDepartment',
        { 'departmentId': deptId, 'myTime': Date.now() },
        function (response) {
            if (response.result === 'success') {
                if (response.data != null) {

                    var utcTime = parseInt(response.data.replace("/Date(", "").replace(")/", ""), 10);

                    var dt = new Date(utcTime);
                    dt = new Date(utcTime - (dt.getTimezoneOffset() * 1000 * 60));
                    me.$watchDate.datepicker('update', dt);

                    var readOnly = $(me.$watchDateEdit).attr("readonly");
                    if (readOnly != undefined && readOnly.toLowerCase() === 'readonly') {
                        var dateText = dt.format('yyyy-MM-dd');
                        me.$watchDateEdit.val(dateText);
                    }
                }
            }
        });
};

EditPage.prototype.reExtendCaseLock = function () {
    var self = this;
    var p = self.p;

    var data = {
        lockGuid: p.caseLockGuid,
        extendValue: p.extendValue,
        caseId: parseInt(p.currentCaseId || self.case.id)
    };
    if (!data || !data.caseId || !data.lockGuid || !data.extendValue) {
        console.log(`Missing data for reextendcase: ${JSON.stringify(data)}`);
        return;
    }
    if ($.parseJSON(p.isCaseLocked.toLowerCase())) return;

    $.post(p.caseLockExtender, data, function (data) {
        if (data && data.length) {
            var caseLockedWarning$ = $('#caseLockedWarning');
            caseLockedWarning$.off('show.bs.modal').on('show.bs.modal',
                function () {
                    var caseLockedByLabel = p.caseLockedByLabel.replace('&lt;name&gt;', data);
                    caseLockedWarning$.find('.modal-body').html(caseLockedByLabel);
                    caseButtons
                        .addClass('disabled')
                        .css('pointer-events', 'none');
                    $('#case-action-close')
                        .removeClass('disabled')
                        .css('pointer-events', '');

                    caseLockedWarning$.find('.btn.ok').off('click').on('click', function (e) {
                        caseLockedWarning$.modal('hide');
                        //window.location.reload();
                    });
                });
            caseLockedWarning$.modal('show');
            self.stopCaseLockTimer();
        }
        else {
            console.log("Data has no length");
        }
    });
};

EditPage.prototype.resetSaving = function () {
    var self = this;
    self.setCaseStatus(self.CASE_IN_IDLE);
};

EditPage.prototype.isProductAreaValid = function () {
    var me = this;
    var res = me.$productAreaChildObj.val();
    if (res == '1')
        return false;
    else
        return true;
};
EditPage.prototype.isFinishingTypeValid = function () {
    var me = this;
    var res = me.$finishingCauseChildObj.val();
    if (res == '1')
        return false;
    else
        return true;
};

EditPage.prototype.getValidationErrorMessage = function (extraMessage) {
    var me = this;
    var validationMessages = me.p.casesScopeInitParameters.validationMessages || '';
    var requiredFieldsMessage = me.p.casesScopeInitParameters.requiredFieldsMessage || '';
    var mandatoryFieldsText = me.p.casesScopeInitParameters.mandatoryFieldsText || '';
    var messages = [requiredFieldsMessage, '<br />', mandatoryFieldsText, ':'];
    $('label.error').each(function (key, el) {
        if ($(el).css('display') === 'none') {
            return true;
        }
        var errorText = $(el).text();
        $.each(validationMessages, function (index, validationMessage) {
            errorText = '<br />' + '[' + dhHelpdesk.cases.utils.replaceAll(errorText, validationMessage, '').trim() + ']';
        });
        messages.push(errorText);
    });

    messages.push(extraMessage);

    return messages.join('');
};

var isExternalSummernoteEmpty = function () {
    // Get the HTML content of the Summernote editor
    var content = $('.summernoteexternal').summernote('code');

    var plainText = $('<div>').html(content).text().trim(); // Strip HTML and trim whitespace
    var isEmpty = !plainText.length && !(/<img[^>]*>/g).test(content); // Check if the plain text is empty and not contains any image

    return isEmpty;
};

EditPage.prototype.isFormValid = function () {
    var me = this;
    $('#btnAddCaseFile').removeClass('error');

    if (!me.isProductAreaValid()) {
        me.$productAreaObj.addClass("error");
        dhHelpdesk.cases.utils.showError(me.productAreaErrorMessage);
        return false;
    }
    if (!me.isFinishingTypeValid()) {
        me.$finishingCauseObj.addClass("error");
        dhHelpdesk.cases.utils.showError(me.finishingTypeErrorMessage);
        return false;
    }
    var curFinishDate = $('#' + me.p.caseFieldIds.FinishingDate).val();
    if (curFinishDate != undefined && curFinishDate != '') {
        var regDate = me.getDate(me.p.caseRegDate);
        var finishDate = me.getDate(curFinishDate);
        if (regDate > finishDate) {
            dhHelpdesk.cases.utils.showError(me.p.finishingDateMessage);
            $('#' + me.p.caseFieldIds.FinishingDate).addClass("error");
            return false;
        };
    }

    var emptyLog = isExternalSummernoteEmpty();

    if (emptyLog && $('#divCaseLogFiles.externalLog-files tr').length > 0) {
        $("#textExternalOuter").find(".note-editor").addClass("error");
        $("#externalError").css('display', 'inline');
        return false;
    }
    else {
        $("#textExternalOuter").find(".note-editor").removeClass("error");
        $("#externalError").css('display', 'none');
    }


    var finished = true;
    var finishingTypeId = "CaseLog_FinishingType";
    if (document.getElementById(finishingTypeId) != null) {
        if (document.getElementById(finishingTypeId).value == "" || document.getElementById(finishingTypeId).value == null || typeof document.getElementById(finishingTypeId).value == 'undefined') {
            finished = false;
        }
    }

    var isCaseFileValid = true;
    var err = '';

    if (me.isCaseFileMandatory != 0) {

        var caseFiles = $('#case_files_table tbody tr');
        if (caseFiles != null && caseFiles != undefined) {
            if (caseFiles.length < 1) {
                $('#btnAddCaseFile').addClass('error');
                isCaseFileValid = false;
                err = '<br />' + '[' + me.p.caseFileCaption + ']';
            }
        }
    }

    if (me.isCaseFileMandatoryOnReOpen == 1 && finished == true) {

        var caseFiles = $('#case_files_table tbody tr');
        if (caseFiles != null && caseFiles != undefined) {
            if (caseFiles.length < 1) {
                $('#btnAddCaseFile').addClass('error');
                isCaseFileValid = false;
                err = '<br />' + '[' + me.p.caseFileCaption + ']';
            }
        }
    }


    if (!me.$form.valid()) {
        if (isCaseFileValid)
            dhHelpdesk.cases.utils.showError(me.getValidationErrorMessage());
        else
            dhHelpdesk.cases.utils.showError(me.getValidationErrorMessage(err));
        return false;
    } else {
        if (!isCaseFileValid) {
            dhHelpdesk.cases.utils.showError(me.getValidationErrorMessage(err));
            return false;
        }
    }

    return true;
};


EditPage.prototype.primaryValidation = function (submitUrl) {


    ShowToastMessage("Blabla", "error", true);
    return;

    var me = this;
    me.syncCaseFromExCaseIfExists();
    var finishDate = $('#CaseLog_FinishingDate').val();

    /* Check FinishigTime */
    if (me.CaseWillFinish() && finishDate != null && finishDate != undefined) {
        $.get('/Cases/IsFinishingDateValid/', { changedTime: me.p.caseChangedTime, finishingTime: finishDate, myTime: Date.now() }, function (res) {
            if (res != null && res) {
                me.startSaveProcess(me, submitUrl);
            }
            else {
                dhHelpdesk.cases.utils.showError(me.p.finishingDateMessage2);
            }
        });
    } else {
        me.startSaveProcess(me, submitUrl);
    }
}

EditPage.prototype.startSaveProcess = function (sender, submitUrl) {
    //Check if there is Order which is not invoiced yet
    var me = sender;

    if (me.invoiceIsActive && me.CaseWillFinish()) {
        $.get('/CaseInvoice/IsThereNotSentOrder/', { caseId: me.p.currentCaseId, myTime: Date.now() }, function (res) {
            if (res != null && res) {
                dhHelpdesk.cases.utils.showError(me.p.invoicePreventsToCloseCaseMessage);
            }
            else {
                me.checkAndSave(submitUrl);
            }
        });
    }
    else {
        me.checkAndSave(submitUrl);
    }
}

EditPage.prototype.checkAndSave = function (submitUrl) {
    var me = this;
    var action = submitUrl || me.EDIT_CASE_URL;
    if (me._inSaving) {
        return false;
    }

    me.setCaseStatus(me.CASE_IN_SAVING);
    var params = me.p;
    if (params.preventToSaveCaseWithInactiveValue != null && params.preventToSaveCaseWithInactiveValue == 1) {
        if (me.$form == undefined || me.$form.index(0) == -1) {
            ShowToastMessage("Case form is not valid!", "error", true);
            return;
        }

        try {
            var form = me.$form[0];
            var fieldIds = params.caseFieldIds;
            var fieldsToCheck = {
                CustomerId: (form.elements[fieldIds.CustomerId] == undefined) ? null : form.elements[fieldIds.CustomerId].value,
                RegionId: (form.elements[fieldIds.RegionId] == undefined) ? null : form.elements[fieldIds.RegionId].value,
                DepartmentId: (form.elements[fieldIds.DepartmentId] == undefined) ? null : form.elements[fieldIds.DepartmentId].value,
                OUId: (form.elements[fieldIds.OUId] == undefined) ? null : form.elements[fieldIds.OUId].value,
                SourceId: (form.elements[fieldIds.SourceId] == undefined) ? null : form.elements[fieldIds.SourceId].value,
                CaseTypeId: (form.elements[fieldIds.CaseTypeId] == undefined) ? null : form.elements[fieldIds.CaseTypeId].value,
                ProductAreaId: (form.elements[fieldIds.ProductAreaId] == undefined) ? null : form.elements[fieldIds.ProductAreaId].value,
                SystemId: (form.elements[fieldIds.SystemId] == undefined) ? null : form.elements[fieldIds.SystemId].value,
                CategoryId: (form.elements[fieldIds.CategoryId] == undefined) ? null : form.elements[fieldIds.CategoryId].value,
                SupplierId: (form.elements[fieldIds.SupplierId] == undefined) ? null : form.elements[fieldIds.SupplierId].value,
                WorkingGroupId: (form.elements[fieldIds.WorkingGroupId] == undefined) ? null : form.elements[fieldIds.WorkingGroupId].value,
                ResponsibleId: (form.elements[fieldIds.ResponsibleId] == undefined) ? null : form.elements[fieldIds.ResponsibleId].value,
                AdministratorId: (form.elements[fieldIds.AdministratorId] == undefined) ? null : form.elements[fieldIds.AdministratorId].value,
                PriorityId: (form.elements[fieldIds.PriorityId] == undefined) ? null : form.elements[fieldIds.PriorityId].value,
                StatusId: (form.elements[fieldIds.StatusId] == undefined) ? null : form.elements[fieldIds.StatusId].value,
                SubStatusId: (form.elements[fieldIds.SubStatusId] == undefined) ? null : form.elements[fieldIds.SubStatusId].value,
                CausingPartId: (form.elements[fieldIds.CausingPartId] == undefined) ? null : form.elements[fieldIds.CausingPartId].value,
                ClosingReasonId: (form.elements[fieldIds.ClosingReasonId] == undefined) ? null : form.elements[fieldIds.ClosingReasonId].value
            }
        }
        catch (exception) {
            ShowToastMessage("An error has occurred in field validation: " + exception.message, "error", true);
            me.setCaseStatus(me.CASE_IN_IDLE);
            return;
        }

        $.ajax({
            type: "POST",
            data: JSON.stringify(fieldsToCheck),
            url: params.caseActiveDataChecker,
            contentType: "application/json",
            success: function (data) {
                if (data == 'valid') {
                    return me.doTotalValidationAndSave(submitUrl);
                } else {
                    //Case has inactive value(s)                    
                    ShowToastMessage(data, "error", true);
                    me.setCaseStatus(me.CASE_IN_IDLE);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                ShowToastMessage(params.checkInactiveDataErrorMessage + " <br/> \"" + thrownError + "\"", "error", true);
                me.setCaseStatus(me.CASE_IN_IDLE);
                return;
            }
        });
    }
    else
        return me.doTotalValidationAndSave(submitUrl);

}

EditPage.prototype.doTotalValidationAndSave = function (submitUrl) {
    var self = this;

    var finishingType = parseInt(document.getElementById("CaseLog_FinishingType"));

    if (isNaN(finishingType.value)) {
        finishingType = 0;
    }

   


    if ($("#ExtendedInitiatorGUID").val().length > 0) {
        $('#extendedSection-iframe-Initiator')[0].contentWindow.saveExtendedCase(false, finishingType);
    }

    if ($("#ExtendedRegardingGUID").val().length > 0) {
        $('#extendedSection-iframe-Regarding')[0].contentWindow.saveExtendedCase(false, finishingType);
    }

    var $_ex_Container = self.getExtendedCaseContainer();
    if (!self.isNullOrUndefined($_ex_Container)) {
        if (!self.isExtendedCaseValid()) {
            self.setCaseStatus(self.CASE_IN_IDLE);
            return false;
        }

        if (!self.isFormValid()) {
            self.setCaseStatus(self.CASE_IN_IDLE);
            return false;
        }

        var promise = $_ex_Container.contentWindow.saveExtendedCase(false, finishingType);
        promise.then(
            function (res) {
                if (res != null)
                    $("#ExtendedCaseGuid").val(res.extendedCaseGuid);
                self.doSaveCase(submitUrl);
            },
            function (err) {
                self.onSaveError(err);
            });
        return false;
    } else {
        if (!self.isFormValid()) {
            self.setCaseStatus(self.CASE_IN_IDLE);
            return false;
        }
        return self.doSaveCase(submitUrl);
    }
}

EditPage.prototype.doSaveCase = function (submitUrl) {
    var self = this;
    var action = submitUrl || self.p.editCaseUrl;
    self.$form.attr("action", action);

    if (self.case.isNew()) {
        self.stopCaseLockTimer();
        self.$form.submit();
        return false;
    }

    self.stopCaseLockTimer();

    self.checkCaseIsAvaialble().done(function (data) {
        if (data) {
            self.$form.submit();
        } else {
            //Case is Locked
            ShowToastMessage(self.p.saveLockedCaseMessage, "error", true);
            self.setCaseStatus(self.CASE_IN_IDLE);
        }
    });

    return false;
};

EditPage.prototype.stopCaseLockTimer = function () {
    var self = this;
    if (self.timerId != undefined)
        clearInterval(self.timerId);
};

EditPage.prototype.setCaseStatus = function (status) {
    var me = this;
    switch (status) {
        case me.CASE_IN_IDLE:
            me._inSaving = false;
            me.$buttonsToDisable.removeClass('disabled');
            caseButtons.css("pointer-events", "");
            $(templateQuickButtonIndicator).css("display", "none");
            return true;

        case me.CASE_IN_SAVING:
            me._inSaving = true;
            me.$buttonsToDisable.addClass('disabled');
            caseButtons.css("pointer-events", "none");
            $(templateQuickButtonIndicator).css("display", "block");
            return true;

        default:
            ShowToastMessage("Case status is not defined!", "error", true);
            return false;
    }
};

EditPage.prototype.confirmDialog = function (d, onOk, onCancel) {
    var title = dhHelpdesk.CaseArticles.translate("Meddelande");
    var ok = dhHelpdesk.CaseArticles.translate("Ok");
    var decline = dhHelpdesk.CaseArticles.translate("Avbryt");
    return d.dialog({
        title: title,
        buttons: [
            {
                text: ok,
                click: function () {
                    onOk();
                    d.dialog("close");
                },
                class: 'btn'
            },
            {
                text: decline,
                click: function () {
                    onCancel();
                    d.dialog("close");
                },
                class: 'btn'
            }
        ],
        modal: true
    });
};

EditPage.prototype.CaseWillFinish = function () {
    if ($('#CaseLog_FinishingDate').val() != '' || $('#CaseLog_FinishingType').val() != '') {
        return true;
    }
    else {
        return false;
    }
};

EditPage.prototype.onSaveYes = function (e) {
    e.preventDefault();

    var self = this;
    var c = self.case;
    var url = self.EDIT_CASE_URL;
    if (c.isNew() && c.isChildCase()) {
        url = self.SAVE_GOTO_PARENT_CASE_URL;
    }

    self.primaryValidation(url);
    return false;
};

EditPage.prototype.onSaveAndNewYes = function (e) {
    e.preventDefault();

    var self = this;
    self.primaryValidation(self.SAVE_ADD_CASE_URL);
    return false;
};

EditPage.prototype.onSaveAndCloseYes = function (e) {
    e.preventDefault();

    var self = this;
    self.primaryValidation(self.NEW_CLOSE_CASE_URL);
    return false;
};

EditPage.prototype.onSaveClick = function (e) {
    e.preventDefault();
    //debugger;

    var self = this;
    var c = self.case;
    var url = self.EDIT_CASE_URL;
    if (c.isNew() && c.isChildCase()) {
        url = self.SAVE_GOTO_PARENT_CASE_URL;
    }
    self.primaryValidation(url);
    return false;
};

EditPage.prototype.onNewCloseAndSplitClick = function (e) {
    e.preventDefault();

    var self = this;
    self.primaryValidation(self.NEW_CLOSE_SPLIT_URL);
    return false;
};

EditPage.prototype.onEditCloseAndSplitClick = function (e) {
    e.preventDefault();

    var self = this;
    self.primaryValidation(self.EDIT_CLOSE_SPLIT_URL);
    return false;
};

EditPage.prototype.onSaveAndCloseClick = function (e) {
    e.preventDefault();

    var self = this;
    self.primaryValidation(self.NEW_CLOSE_CASE_URL);
    return false;
};

EditPage.prototype.onSaveAndNewClick = function (e) {
    e.preventDefault();

    var self = this;
    self.primaryValidation(self.SAVE_ADD_CASE_URL);
    return false;
};

EditPage.prototype.onSaveError = function (err) {
    ShowToastMessage("Extended Case save was not succeed!", "error", false);
    this.setCaseStatus(this.CASE_IN_IDLE);
    return false;
}

EditPage.prototype.canDeleteCase = function () {
    var me = this;
    return !(me.$btnDelete.hasClass('disabled') || me.case.isAnyNotClosedChild());
};

EditPage.prototype.showDeleteConfirmationDlg = function () {
    var me = this;
    if (!me.canDeleteCase()) {
        return false;
    }
    me.deleteDlg.show();
    return false;
};

EditPage.prototype.makeDeleteParams = function (c) {
    var me = this;
    var res = {
        caseId: c.id
    };

    if (c.customerId != 0) {
        res.customerId = c.customerId;
    }

    if (c.parentCaseId != 0) {
        res.parentCaseId = c.parentCaseId;
    }

    if (me.p.backUrl != null) {
        res.backUrl = me.p.backUrl;
    }

    return res;
};

EditPage.prototype.doDeleteCase = function (c) {
    var me = this;
    var $form = $(['<form action="', me.DELETE_CASE_URL, '?', $.param(me.makeDeleteParams(c)), '" method="post"></form>'].join(String.EMPTY));
    $('body').append($form);
    me.stopCaseLockTimer();
    $form.submit();
};

EditPage.prototype.onDeleteDlgClick = function (res) {
    var me = this;

    if (res === ConfirmationDialog.NO) {
        me.deleteDlg.hide();
        return false;
    }

    if (!me.canDeleteCase()) {
        return false;
    }

    var params = me.p;

    me.checkCaseIsAvaialble().done(callAsMe(function (data) {
        me.showDeleteConfirmationDlg();
        if (data) {
            me.doDeleteCase(me.case);
        } else {
            ShowToastMessage(params.deleteLockedCaseMessage, "error", true);
        }
    }));
    return true;
};

EditPage.prototype.initDeleteConfirmationDlg = function () {
    var me = this;
    // Delete case confirmation dialogue
    var dlgOptions = {
        btnYesText: $("#DeleteDialogDeleteButtonText").val(),
        btnNoText: $("#DeleteDialogCancelButtonText").val(),
        onClick: Utils.callAsMe(me.onDeleteDlgClick, me),
        dlgText: me.$btnDelete.attr('deleteDialogText'),
    };
    if (me.$btnDelete.attr("buttonTypes") === 'YesNo') {
        dlgOptions.btnYesText = $("#DeleteDialogYesButtonText").val();
        dlgOptions.btnNoText = $("#DeleteDialogNoButtonText").val();
    }
    return CreateInstance(ConfirmationDialog, dlgOptions);
};

EditPage.prototype.initLeaveConfirmationDlg = function () {
    var me = this;
    // Delete case confirmation dialogue
    var dlgOptions = {
        btnYesText: $("#DeleteDialogYesButtonText").val(),
        btnNoText: $("#DeleteDialogNoButtonText").val(),
        dlgText: me.p.lostChangesConfirmation
    };

    return CreateInstance(ConfirmationDialog, dlgOptions);
};

EditPage.prototype.onPageLeave = function (ev) {
    var me = this;
    var gotoUrl = ev.target;
    if (me.formOnBootValues !== me.$form.serialize()) {
        me.leaveDlg.show().done(function (result) {
            me.leaveDlg.hide();
            if (result === ConfirmationDialog.YES) {
                me.stopCaseLockTimer();
                window.location.href = gotoUrl;
            }
        });
        ev.preventDefault();
        return false;
    }
};

EditPage.prototype.recoverTokenIfNeeded = function () {
    var needToGetToken = false;
    var _tokenData = localStorage.getItem("TokenData");

    if (_tokenData !== null) {
        var _details = JSON.parse(_tokenData);
        if (_details != undefined) {
            needToGetToken = _details.AccessToken === '' || _details.RefreshToken === '';
        }
    } else {
        needToGetToken = true;
    }

    if (needToGetToken) {
        $.get(_parameters.getTokenUrl,
            {
                curTime: Date.now()
            },
            function (res) {
                if (res != null) {
                    if (res.result) {
                        localStorage.removeItem("TokenData");
                        var newTokenData = { 'AccessToken': res.accessToken, 'RefreshToken': res.refreshToken };
                        localStorage.setItem("TokenData", JSON.stringify(newTokenData));
                    }
                }
            }
        );
    }
}

EditPage.prototype.onCloseClick = function (e) {
    e.preventDefault();

    var me = this;
    var c = me.case;
    var url;
    if (c.isChildCase() && c.isNew()) {
        url = me.EDIT_CASE_URL + '/' + c.parentCaseId;
    } else {
        url = me.CASE_OVERVIEW_URL;
    }

    me.stopCaseLockTimer();
    window.location.href = url;
    return false;
};

EditPage.prototype.changeCaseButtonsState = function (state) {
    if (state) {
        caseButtons.removeClass('disabled');
        caseButtons.css("pointer-events", "");
        $(templateQuickButtonIndicator).css("display", "none");
    }
    else {
        caseButtons.addClass("disabled");
        caseButtons.css("pointer-events", "none");
        $(templateQuickButtonIndicator).css("display", "block");
    }
}

EditPage.prototype.getLanguage = function () {
    var self = this;

    var _fieldIds = self.Case_Field_Ids;
    var caseId = document.getElementById(_fieldIds.CaseId).value;
    if (caseId == 0) {
        var reported = document.getElementById(_fieldIds.UserCode).value;
        var reportedId = document.getElementById(_fieldIds.ReportedBy).value;

        var region = document.getElementById(_fieldIds.RegionId).value;
        var dep = document.getElementById(_fieldIds.DepartmentId).value;
        var customer = document.getElementById(_fieldIds.CustomerId).value;

        $.get("/cases/LookupLanguage/", {
            customerid: "'" + customer.toString() + "'",
            notifier: "'" + reported.toString() + "'",
            region: "'" + region.toString() + "'",
            department: "'" + dep.toString() + "'",
            notifierid: "'" + reportedId.toString() + "'"
        }, function (result) {
            if (parseInt(result) > 0) {
                $("#case__RegLanguage_Id").val(result);
            }
        });
    }
}

EditPage.prototype.moveCaseToCustomer = function (caseId, customerId, isExternal, caseLockGuid) {
    var self = this;

    if (isExternal) {
        var inputData = {
            caseId: caseId,
            customerId: customerId,
            lockGuid: caseLockGuid
        };

        $.post(self.p.moveCaseToExternalCustomerUrl, $.param(inputData), function (result) {
            if (result.Success) {
                self.stopCaseLockTimer();
                window.location.href = result.Location || '/Cases/';
            } else {
                self.enableMoveCaseControls(true);
                ShowToastMessage(result.Error, "error", true);
            }
        }).fail(function (err) {
            self.enableMoveCaseControls(true);
            ShowToastMessage('Case move failed.', "error", true);
        });
    } else {
        //do normal case move
        var url = '/cases/edit/' + caseId + '?moveToCustomerId=' + customerId;
        window.location.href = url;
    }
};

EditPage.prototype.enableMoveCaseControls = function (state) {
    this.$moveCaseCustomerSelect.prop('disabled', !state);
    this.$moveCaseButton.prop('disabled', !state);
};

EditPage.prototype.buildExtendedCasePrintMarkup = function () {
    var self = this;

    var exContainer = document.getElementById(self.Ex_Container_Prefix + self.Current_EC_FormId);
    var formData = exContainer.contentWindow.getFormData();

    var printMarkup = '';

    //tabs
    for (var i = 0; i < formData.tabs.length; i++) {

        var tab = formData.tabs[i];

        printMarkup +=
            '<tr><td style="width:20%;word-wrap: break-word; margin-left:20px;line-height:10px;" class="textbold H"><p> ' + tab.name + ' </p></td>' +
            '<td style="width:80%; word-wrap: break-word;line-height:10px;" class="H"><p> </p></td></tr>';

        // sections 
        for (var j = 0; j < tab.sections.length; j++) {
            var section = tab.sections[j];

            printMarkup += '<tr><td style="width:20%;word-wrap: break-word; margin-left:20px;line-height:10px;" class="textbold G"><p>' + section.name + '</p></td>' +
                '<td style="width:80%; word-wrap: break-word;line-height:10px;" class="G"><p>  </p></td></tr>';

            //section instances
            for (var k = 0; k < section.instances.length; k++) {
                var sectionInstance = section.instances[k];

                //fields
                for (var n = 0; n < sectionInstance.fields.length; n++) {
                    var field = sectionInstance.fields[n];

                    var fieldValue = (field.secondaryValue || '').length ? field.secondaryValue : field.value;

                    printMarkup +=
                        '<tr><td style="width:20%;word-wrap: break-word; margin-left:20px;line-height:10px;" class="textbold R"><p>' + field.label + '</p></td>' +
                        '<td style="width:80%; word-wrap: break-word;line-height:10px;" class="R"><p>' + fieldValue + '</p></td></tr>';
                }
            }
        }
    }
    return printMarkup;
};

EditPage.prototype.checkCaseIsAvaialble = function () {
    var self = this;
    var p = self.p;
    var data = {
        caseId: p.currentCaseId,
        caseChangedTime: p.caseChangedTime,
        lockGuid: p.caseLockGuid
    };

    var jqXhr = $.post(p.caseLockChecker, $.param(data));
    return jqXhr;
};

EditPage.prototype.markAsUnread = function () {
    var self = this;
    var p = self.p;

    self.checkCaseIsAvaialble().done(function (res) {
        if (res) {
            var inputData = {
                id: p.currentCaseId,
                customerId: p.customerId
            };
            $.post(p.markAsUnreadUrl, $.param(inputData), function (data) {
                if (data === "Success") {
                    $('#case__Unread').val(1);
                    ShowToastMessage(p.caseMarkedAsUnreadMessage, "notice");
                }
            });
        }
        else {
            ShowToastMessage(p.caseOpenedByOtherUserMessage, "error", false);
        }
    });
};

EditPage.prototype.unlockCase = function (lockGuid, url) {
    var self = this;
    var p = self.p;

    $.post(p.unlockCaseUrl, $.param({ lockGUID: lockGuid }), function (data) {
        if (data !== "Success") {
            ShowToastMessage(p.caseUnlockErrorMessage, "Error");
        }

        if (url) {
            self.redirectToUrl(url);
        }
    });
};

EditPage.prototype.unlockCaseById = function (caseId, url) {
    var self = this;
    var p = self.p;
    console.log("Unlocking");
    $.post(p.unlockCaseByCaseIdUrl, $.param({ caseId: caseId }), function (data) {
        if (data !== "Success") {
            console.log(data);
            ShowToastMessage(p.caseUnlockErrorMessage, "Error");
        }
        if (url) {
            self.redirectToUrl(url);
        }
    });
};

EditPage.prototype.redirectToUrl = function (url) {
    window.location.href = url;
};

/***** Initiator *****/
EditPage.prototype.init = function (p) {
    var self = this;
    self._inSaving = false;
    self.p = p; //window.parameters

    EditPage.prototype.Case_Field_Ids = p.caseFieldIds;
    EditPage.prototype.Case_Field_Init_Values = p.caseInitValues;
    EditPage.prototype.Current_EC_FormId = p.extendedCaseFormId;
    EditPage.prototype.Current_EC_Guid = p.extendedCaseGuid;
    EditPage.prototype.Current_EC_LanguageId = p.extendedCaseLanguageId;
    EditPage.prototype.Current_EC_Path = p.extendedCasePath;

    EditPage.prototype.extendedSections = p.extendedSections;


    /*Debug mode*/
    //EditPage.prototype.Current_EC_Path = "http://localhost:8099" + p.extendedCasePath.replace('ExtendedCase/', '');

    /// controls binding
    self.extendedCaseInvalidMessage = p.extendedCaseInvalidMessage;
    self.$form = $('#target');
    self.$watchDateChangers = $('.departments-list, #case__Priority_Id, #case__StateSecondary_Id');
    self.$department = $('.departments-list');
    self.$SLASelect = $('#case__Priority_Id');
    self.$SLAText = $('#case__Priority_Id');
    self.$watchDateEdit = $('#case__WatchDate');
    self.$watchDate = $('#divCase__WatchDate');
    self.$buttonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
        '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,.btn.new-close-split,.btn.edit-close-split' +
        '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton');

    self.$productAreaObj = $('#divProductArea');
    self.$productAreaChildObj = $('#ProductAreaHasChild');
    self.$finishingCauseObj = $('#divFinishingType');
    self.$finishingCauseChildObj = $('#FinishingCauseHasChild');
    self.productAreaErrorMessage = self.p.productAreaErrorMessage;
    self.finishingTypeErrorMessage = self.p.finishingTypeErrorMessage;
    self.$moveCaseButton = $("#btnMoveCase");
    self.$moveCaseCustomerSelect = $('#moveCaseToCustomerId');
    self.$btnSave = $('.btn.save');
    self.$btnSaveClose = $('.btn.save-close');
    self.$btnSaveNew = $('.btn.save-new');
    self.$btnDelete = $('.caseDeleteDialog.btn');
    self.$btnClose = $('.btn.close-page');
    self.$btnNewCloseAndSplit = $('.btn.new-close-split');
    self.$btnEditCloseAndSplit = $('.btn.edit-close-split');
    self.deleteDlg = self.initDeleteConfirmationDlg();
    self.leaveDlg = self.initLeaveConfirmationDlg();

    self.$btnPrint = $('.btn.print-case');
    self.$printArea = $('#CasePrintArea');
    self.$printDialog = $('#PrintCaseDialog');
    self.$descriptionDialog = $('#caseDescriptionPreview');
    self.$caseDescriptionEl = $(".case-description")
    self.$caseTab = $("#tabsArea li a");//$('#case-tab');
    self.$activeTabHolder = $('#ActiveTab');
    self.$internalLogNote = $('#' + self.Case_Field_Ids.log_InternalText);

    self.$selectListStep = $("#steps");
    self.$btnGo = $('#btnGo');
    self.isRelated = self.p.isRelated;
    self.isCaseFileMandatory = self.p.isCaseFileMandatory;
    self.isCaseFileMandatoryOnReOpen = self.p.isCaseFileMandatoryOnReOpen;
    self.isCaseReOpen = self.p.isCaseReOpen;

    var invoiceElm = $('#CustomerSettings_ModuleCaseInvoice').val();
    self.invoiceIsActive = invoiceElm != undefined && invoiceElm != null && invoiceElm.toString().toLowerCase() == 'true';

    ///////////////////////     events binding      /////////////////////////////////
    self.$btnDelete.on('click', callAsMe(self.showDeleteConfirmationDlg, self));
    self.$btnClose.on('click', Utils.callAsMe(self.onCloseClick, self));
    self.$btnSave.on('click', Utils.callAsMe(self.onSaveClick, self));
    self.$btnSaveClose.on('click', Utils.callAsMe(self.onSaveAndCloseClick, self));
    self.$btnSaveNew.on('click', Utils.callAsMe(self.onSaveAndNewClick, self));
    self.$btnNewCloseAndSplit.on('click', Utils.callAsMe(self.onNewCloseAndSplitClick, self));
    self.$internalLogNote.on('change', self.propagateLogNote);
    self.$btnEditCloseAndSplit.on('click', Utils.callAsMe(self.onEditCloseAndSplitClick, self));

    $('.lang.dropdown-submenu a').on('click', Utils.callAsMe(self.onPageLeave, self));


    /*Load extended case*/
    self.loadExtendedCaseIfNeeded();
    self.loadExtendedSectionsIfNeeded();

    const logTab$ = $('#logtab');
    if (logTab$.length > 0) {
        const maxRows = 3;
        const table$ = logTab$.find("table[name='caseLogTable']");
        const rows$ = table$.find("tr[name='caseLogRow']");

        // process rows height
        const lineHeight = 20;
        const classPreffix = 'less';
        rows$.each((i, row) => {
            const row$ = $(row);
            const isFirstRow = i === 0;
            const maxTdRows = isFirstRow ? 7 : 3;
            const className = classPreffix + maxTdRows;
            let isRowAllVisible = false;
            const tds = row$.find('td');

            const externalCellRowsCount = Math.ceil(($(tds[3]).find("div[name='divTextExternal']").height() || 1) / lineHeight);
            const internalCellRowsCount = Math.ceil(($(tds[4]).find("div[name='divTextInternal']").height() || 1) / lineHeight);
            const filesCellRowsCount = Math.ceil(($(tds[6]).find("div[name='divLogNoteFiles']").height() || 1) / lineHeight);

            if (externalCellRowsCount > maxTdRows) {
                $(tds[3]).find('.row-all').show();
                isRowAllVisible = true; 
            }
            if (internalCellRowsCount > maxTdRows) {
                $(tds[4]).find('.row-all').show();
                isRowAllVisible = true;
            }
            if (filesCellRowsCount > maxTdRows) {
                $(tds[6]).find('.row-all').show();
                isRowAllVisible = true;
            }
            if (isRowAllVisible) {
                row$.find('.row-all').on('click',
                    function (e) {
                        e.preventDefault();
                        const currRow$ = $(e.target).closest('tr');
                        currRow$.toggleClass(className);
                    });
            } else {
                row$.removeClass(className);
            }
        });

        // process table height
        const logRowsCount = rows$.length;
        if (logRowsCount > maxRows) {
            const more$ = logTab$.find('.all.more');
            const less$ = logTab$.find('.all.less');
            const toggleRows = function () {
                if (more$.is(':visible')) {
                    
                    table$.find('tr.less3:gt(' + (maxRows - 2) + ')').hide();
                    rows$.each((i, row) => {
                        let r$ = $(row);

                        if (i > 2) {
                            r$.hide();
                            r$.removeClass("less-ignoreheight");
                        }
                        else {
                            let td$ = r$.find("div[name='divTextExternal']");
                            if (td$.width() > 300) {
                                r$.find('td').addClass("scrollMe");
                            }
                            if (i == 0 && r$.height() > 140) {
                                r$.addClass("less7");
                            }
                            if (i != 0  && r$.height() > 60) {
                                r$.addClass("less3")
                            }
                            r$.show();
                            
                            r$.removeClass("less-ignoreheight");
                        }
                    });
                }

                else {
                    table$.find('tr.less3:gt(' + (maxRows - 2) + ')').show();
                    rows$.each((i, row) => {
                        let r$ = $(row);
                        r$.show();
                        r$.removeClass("less3");
                        r$.removeClass("less7");
                    });
                }
            }
            toggleRows();
            more$.add(less$).on('click', function (e) {
                e.preventDefault();
                more$.toggle();
                less$.toggle();
                toggleRows();
            });

        }
    }

    self.$watchDateChangers.on('change', function (d, source) {
        var deptId = parseInt(self.$department.val(), 10);
        var SLA = parseInt(self.$SLASelect.find('option:selected').attr('data-sla'), 10);
        if (isNaN(SLA)) {
            SLA = parseInt(self.$SLAText.attr('data-sla'), 10);
        }
        if (this.id === "case__StateSecondary_Id") {
            // Moved to commonHandlers,js
        } else {

            if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
                self.fetchWatchDateByDept.call(self, deptId);
            }
        }
        //else {
        //    if (self.$watchDateEdit.val() == '')
        //        self.$watchDate.datepicker('update', '');
        //}

        return false;
    });

    $("#divMoveCase").on('shown', function () {
        self.$moveCaseCustomerSelect.change();
    });

    self.$moveCaseCustomerSelect.change(function (e) {
        var $selectedOption = $("option:selected", this);
        if ($selectedOption && !self.isNullOrEmpty($selectedOption.val())) {

            var isExternal = (+($selectedOption.data('external') || 0)) > 0;
            var hasExtendedCase =
                !self.isNullOrEmpty(self.Current_EC_FormId || '') || !self.isNullOrEmpty(self.extendedSections || '');
            var isRelated = self.p.isRelated == 'true';

            // disable move and show warning if has Child-Parent
            if (isExternal && isRelated) {
                $('#extendedCaseNote').hide();
                $('#externalCustomerNote').hide();
                $('#childParentNote').show();
                self.$moveCaseButton.prop("disabled", true);
                return;
            }

            //allow move but show warnings
            $('#childParentNote').hide();
            $('#externalCustomerNote').toggle(isExternal);
            $('#extendedCaseNote').toggle(hasExtendedCase);
            self.$moveCaseButton.prop("disabled", false);
        }
    });

    self.$moveCaseButton.click(function (e) {
        e.preventDefault();

        var customerId = +$('#moveCaseToCustomerId').val() || 0;
        if (customerId > 0) {
            self.enableMoveCaseControls(false);
            self.checkCaseIsAvaialble().done(function (data) {
                if (data) {
                    var isExternal = +($('#moveCaseToCustomerId').find(':selected').data('external') || 0);
                    self.moveCaseToCustomer(p.currentCaseId, customerId, isExternal > 0, p.caseLockGuid);
                } else {
                    ShowToastMessage(p.moveLockedCaseMessage, "error", true);
                    self.enableMoveCaseControls(true);
                }
            });
        }
    });

    $('.date').each(function () {
        var $this = $(this);
        var errorLabel = $this.find('label.error:visible');
        if (errorLabel.length > 0) {
            var calendarIcon = $this.find('.add-on');
            if (calendarIcon.length > 0) {
                errorLabel.detach().insertAfter(calendarIcon);
            }
        }
    });

    self.$btnPrint.click(function (e) {

        $.get("/Cases/ShowCasePrintPreview/",
            {
                caseId: p.currentCaseId,
                caseNumber: p.currentCaseNumber,
                curTime: Date.now()
            },
            function (reportPresentation) {

                var $reportTable = $(reportPresentation);

                //EXTENDED CASE handling
                if (self.Current_EC_FormId) {

                    var printMarkup = self.buildExtendedCasePrintMarkup();

                    if (printMarkup && printMarkup.length) {
                        $reportTable.find('#caseReportContainer table.printcase').append(printMarkup);
                    }
                }

                self.$printArea.html('');
                self.$printArea.append($reportTable);

                $('#PrintCaseDialog').draggable({
                    handle: ".modal-header"
                });

                /* show true if you need to show case print preview*/
                $('#PrintCaseDialog').modal({
                    "backdrop": "static",
                    "keyboard": true,
                    "show": false
                });

                //var _iframe = $("#caseReportContainer").find("iframe");
                //if (_iframe != null && _iframe != undefined) {
                //    update_iFrame(_iframe.attr("id"));                        
                //}
            }
        );
    });

    self.$descriptionDialog.click(function () {
        var description = self.$caseDescriptionEl
            .attr("value");

        description = replaceLinebreaksInString(description);

        var d = $(document.createElement("div"))
            .html(description)
            .dialog({
                title:
                    self.p.descriptionText,
                modal: true,
                width: 600,
                height: 400,
                close: function () {
                    d.dialog("destroy");
                }
            });
    });

    self.$caseDescriptionEl.keyup(function (ev) {
        var visibility = 'visible';
        if (ev.target.value.length == 0) {
            visibility = 'hidden';
        }
        self.$descriptionDialog.css('visibility', visibility);
    });

    if ($(".chosen-single-select").length > 0) {
        $(".chosen-single-select").each(function () {
            if ($(this).attr('aria-invalid') !== "true") {
                $(this).on("change", function () {
                    $(this).valid();
                });
            }
        });
    }

    /*Enable if you have case print preview*/
    //var update_iFrame = function (iframeId) {
    //    setTimeout(function () {           
    //        var elm = document.getElementById(iframeId);
    //        if (elm != null && elm != undefined) {
    //            var newH = $(elm).height() + 50;
    //            $(elm).css({ height: newH.toString() + "px" });
    //        }            
    //    }, 3000);
    //}

    self.case = new Case({
        id: parseInt(p.currentCaseId, 10),
        customerId: parseInt(p.customerId, 10),
        parentCaseId: parseInt(p.parentCaseId, 10),
        isAnyNotClosedChild: p.isAnyNotClosedChild === 'True'
    });

    if (self.case.isAnyNotClosedChild()) {
        self.$btnDelete.addClass('disabled');
    }

    if (p.currentCaseId > 0 && self.p.timerInterval > 0) {
        self.timerId = setInterval(callAsMe(self.reExtendCaseLock, self), self.p.timerInterval * 1000);
    }

    self.formOnBootValues = self.$form.serialize();

    self.ExternalInvoice = new ExternalInvoice({
        requiredMessage: p.casesScopeInitParameters.mandatoryFieldsText,
        mustBeNumberMessage: p.casesScopeInitParameters.formatFieldsText + ": #.##"
    });

    self.$caseTab.click(function (ev) {
        if (window.parameters.containsExtendedCase == "True") {
            if (ev.target.className == "case") {
                self.$activeTabHolder.val('case-tab');
                self.syncCaseFromExCaseIfExists();

                //validate extended case
                //TODO: shold we have this here?
                //self.isExtendedCaseValid(false);
            }

            if (ev.target.className == "extendedcase") {
                self.$activeTabHolder.val('extendedcase-tab');
            }
        }

        if (self.p.containsEForm == 'True') {
            if (ev.target.className == "case") {
                if ($(".secnav").hasClass("hide")) {
                    $(".secnav").fadeIn(300, function () {
                        $(".secnav").removeClass("hide");
                    });
                }

                $.ajax({
                    url: "/Cases/GetCaseInfo",
                    type: "GET",
                    data: { caseId: self.p.currentCaseId, curTime: Date.now() }
                })
                    .done(function (result) {
                        if (result.needUpdate) {
                            if (result.shouldReload) {
                                location.reload(true);
                            } else {
                                self.refreshCasePage(result.newData);
                            }
                        }
                    });
            }

            if (ev.target.className == "eform") {
                var secnav = $(".secnav");
                if (secnav.hasClass("hide") == false) {
                    secnav.fadeOut(300, function () {
                        secnav.addClass("hide");
                    });
                }
            }
        }
    });

    self.$btnGo.on("click", function (e) {
        e.preventDefault();
        $('#ButtonClick').val('btn-go');

        var templateId = parseInt($('#steps').val()) || 0;


        //only load if templateId exist
        if (templateId > 0) {

            var isValid = false;

            var stepId = parseInt($('#steps').val()) || 0;

            if (stepId > 0) {
                isValid = self.isExtendedCaseValid(false, true);
            }
            else {
                isValid = self.isExtendedCaseValid(false, false);
            }


            if (isValid) {
                $('#SelectedWorkflowStep').val(templateId);
                LoadTemplate(templateId);
            }
            else {
                $('#SelectedWorkflowStep').val(0);
            }
        }
    });

    self.$selectListStep.on('change', function () {
        //only if extended case exist
        if (window.parameters.containsExtendedCase != "False") {
            self.setNextStep();
        }
    });

    self.$caseTab.on('shown', function (e) {
        window.scrollTo(0, 0);
    });

    /* Temporary disabled since Extended Case does not use token */
    //self.recoverTokenIfNeeded();
};



