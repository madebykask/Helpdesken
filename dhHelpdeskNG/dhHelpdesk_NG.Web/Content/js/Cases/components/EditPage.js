"use strict";

function EditPage() { };

var caseButtonsToLock = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
                          '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                          '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton');


/*** CONST BEGIN ***/
EditPage.prototype.DELETE_CASE_URL = '/Cases/DeleteCase';
EditPage.prototype.NEW_CLOSE_CASE_URL = '/Cases/NewAndClose';
EditPage.prototype.EDIT_CASE_URL = '/Cases/Edit';
EditPage.prototype.SAVE_GOTO_PARENT_CASE_URL = '/Cases/NewAndGotoParentCase';
EditPage.prototype.SAVE_ADD_CASE_URL = '/Cases/NewAndAddCase';
EditPage.prototype.CASE_OVERVIEW_URL = '/Cases';

EditPage.prototype.DYNAMIC_DROPDOWNS = '.DynamicDropDown';
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
    if (val == undefined || val == null || val == "")
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

EditPage.prototype.getObjectPosInView = function (objectId) {
    var fixedArea = 90;
    var pageSize = $(window).height() - fixedArea;
    var scrollPos = $(window).scrollTop();
    var elementToTop = $("#" + objectId).offset().top - scrollPos - fixedArea;
    var elementToDown = pageSize - elementToTop;
    return { ToTop: elementToTop, ToDown: elementToDown };
}

EditPage.prototype.setDynamicDropDowns = function () {
    var self = this;
    var fixedArea = 90;
    var pageSize = $(window).height() - fixedArea;
    var scrollPos = $(window).scrollTop();
    var elementToTop = $(self.DYNAMIC_DROPDOWNS).offset().top - scrollPos - fixedArea;
    var elementToDown = pageSize - elementToTop;
    if (elementToTop < -$(self.DYNAMIC_DROPDOWNS).height())
        $(self.DYNAMIC_DROPDOWNS).removeClass('open');

    if (elementToTop <= elementToDown) {
        $(self.DYNAMIC_DROPDOWNS).removeClass('dropup');
    } else {
        $(self.DYNAMIC_DROPDOWNS).addClass('dropup');
    }
}

EditPage.prototype.dynamicDropDownBehaviorOnMouseMove = function () {
    var self = this;
    var target = $(event.target.parentElement);
    if (target != undefined && target.hasClass('DynamicDropDown_Up') && target.index(0) != -1) {
        var objPos = self.getObjectPosInView(target[0].id);
        var subMenu = "#subDropDownMenu_" + target[0].id;
        $(subMenu).css("bottom", "auto");
        $(subMenu).css("top", "auto");
        if ($(self.DYNAMIC_DROPDOWNS).hasClass('dropup')) {
            if (objPos.ToTop < objPos.ToDown) {
                var h = -$(subMenu).height() + 25;
                var hstr = h + "px";
                $(subMenu).css("bottom", hstr);
            } else
                $(subMenu).css("bottom", "0");
        } else {
            if (objPos.ToTop < objPos.ToDown || $(subMenu).height() < objPos.ToDown)
                $(subMenu).css("top", "0");
            else {
                var h = -$(subMenu).height() + 25;
                var hstr = h + "px";
                $(subMenu).css("top", hstr);
            }
        }
    }
}


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

    var $elm = document.getElementById(iframeId);
    if (!self.isNullOrUndefined($elm)) {
        var iframeOptions = {
            log: false,
            sizeHeight: true,
            checkOrigin: false,
            enablePublicMethods: true,
            resizedCallback: function (messageData) {
            },
            bodyMargin: '0 0 200px 0',
            messageCallback: function (messageData) {
                if (messageData.message === 'cancelCase') {
                    var elem = $('#case-action-close');
                    location.href = elem.attr('href');
                }
            },
            closedCallback: function (id) {
            }
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

EditPage.prototype.loadExtendedCase = function () {
    var self = this;
    var $_ex_Container = self.getExtendedCaseContainer();
    var formParameters = $_ex_Container.contentWindow.getFormParameters();
    formParameters.languageId = self.Current_EC_LanguageId;
    formParameters.extendedCaseGuid = self.Current_EC_Guid;
    var fieldValues = self.Case_Field_Init_Values;
    if (fieldValues != null) {
        $_ex_Container.contentWindow.loadExtendedCase(
            {
                formParameters: formParameters,
                caseValues: {
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
                    plandate: { Value: fieldValues.PlanDate },
                    watchdate: { Value: fieldValues.WatchDate },
                    priority_id: { Value: fieldValues.PriorityId },
                    log_textinternal: { Value: '' }
                }
            });
    } else {
        $_ex_Container.contentWindow.loadExtendedCase(
            {
                formParameters: formParameters, caseValues: {
                    log_textinternal: { Value: '' }
                }
            });
    }
}

EditPage.prototype.isExtendedCaseValid = function (showToast, isOnNext) {
    //if no input param sent in, set show toast to true
    if (showToast == null)
    {
        showToast = true;
    }

    //if no input param sent in, set isOnNext to false
    if (isOnNext == null) {
        isOnNext = false;
    }

    var self = this;
    
    var $exTab = $(self.ExTab_Prefix + self.Current_EC_FormId);

    if ($('#steps').length && $('#ButtonClick').length && $('#ButtonClick').val() == 'btn-go') {
        //check if value is selected in steps, then isOnNext should be true;
        var templateId = parseInt($('#steps').val()) || 0;
        //only load if templateId exist
        if (templateId > 0) {
            
            isOnNext = true;
        }
    }

    var $_ex_Container = self.getExtendedCaseContainer();
    var validationResult = $_ex_Container.contentWindow.validateExtendedCase(isOnNext);
    if (validationResult == null) {
        //Change color
        if ($exTab.parent().hasClass('error')) {
            $exTab.parent().removeClass('error');
        }

        return true;
    } else {

        //Change color      
            if (!$exTab.parent().hasClass('error'))
            {
                $exTab.parent().addClass('error');
            }

        if (showToast)
        {
            ShowToastMessage("Extended case is not valid!", "error", false);
        }
        return false;
    }
};

EditPage.prototype.setNextStep = function () {
    var self = this;
    var nextStep = 0;
    nextStep = parseInt($("#steps option:selected").attr('data-next-step')) || 0;
    var $_ex_Container = self.getExtendedCaseContainer();
    var validationResult = $_ex_Container.contentWindow.setNextStep(nextStep);
};

EditPage.prototype.syncCaseFromExCaseIfExists = function () {
    var self = this;

    var $_ex_Container = self.getExtendedCaseContainer();
    if ($_ex_Container == undefined) {
        return;
    }

    var fieldData = $_ex_Container.contentWindow.getCaseValues()
    if (fieldData == undefined) {
        return;
    }

    var _caseFields = self.Case_Field_Ids;

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
    var _plandate = fieldData.plandate;
    var _watchdate = fieldData.watchdate;
    var _log_textinternal = fieldData.log_textinternal;
    var _priority_id = fieldData.priority_id;

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
        $("#CaseTemplate_Department_Id").val();

        if (_department_id != 0) {
            $("#CaseTemplate_Department_Id").val(_department_id.Value);
        }
        $('#' + _caseFields.DepartmentId).val(_department_id.Value);
        $('#' + _caseFields.DepartmentName).val(_department_id.SecondaryValue);
    }
    
    if (_ou_id_1 != undefined) {
        $("#CaseTemplate_OU_Id").val();
        var selectedOU_Id = _ou_id_1.Value;
        var selectedOU_Name = _ou_id_1.SecondaryValue;
        if (selectedOU_Name == null)
            selectedOU_Name = "";

        if (_ou_id_2 != undefined && _ou_id_2.Value != null && _ou_id_2.SecondaryValue != null) {
            selectedOU_Id = _ou_id_2.Value;
            if (_ou_id_2.SecondaryValue != null)
                selectedOU_Name = selectedOU_Name + ' - ' + _ou_id_2.SecondaryValue;
        }

        if (selectedOU_Id != 0) {
            $("#CaseTemplate_OU_Id").val(selectedOU_Id);
        }
        $('#' + _caseFields.OUId).val(selectedOU_Id);
        $('#' + _caseFields.OUName).val(selectedOU_Name);
    }


    if (_region_id != undefined) {
        $('#' + _caseFields.RegionId).val(_region_id.Value);
        $('#' + _caseFields.RegionName).val(_region_id.SecondaryValue);
    }

    if (_priority_id != undefined) {
        $('#' + _caseFields.PriorityId).val(_priority_id.Value);
        $('#' + _caseFields.PriorityName).val(_priority_id.SecondaryValue);
    }

    if (_productarea_id != undefined) {
        self.setValueToBtnGroup('#divProductArea', "#divBreadcrumbs_ProductArea", '#' + _caseFields.ProductAreaId, _productarea_id.Value)
    }

    if (_status_id != undefined) {
        $('#' + _caseFields.StatusId).val(_status_id.Value).change();
        $('#' + _caseFields.StatusName).val(_status_id.SecondaryValue);
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
}



EditPage.prototype.refreshCasePage = function (updatedInfo) {
    if (updatedInfo == null)
        return;

    var self = this;
    var _caseFields = self.Case_Field_Ids;

    self.changeCaseButtonsState(false);

    $('#' + _caseFields.ReportedBy).val(updatedInfo.ReportedBy);
    $('#' + _caseFields.PersonsName).val(updatedInfo.PersonsName);
    $('#' + _caseFields.PersonsPhone).val(updatedInfo.PersonsPhone);

    $('#' + _caseFields.CaseTypeId).val(updatedInfo.CaseType_Id).change();

    $('#' + _caseFields.ProductAreaId).val(updatedInfo.ProductArea_Id).change();
    $('#' + _caseFields.WorkingGroupId).val(updatedInfo.WorkingGroup_Id).change();
    $('#' + _caseFields.WorkingGroupName).val(updatedInfo.WorkingGroupName);
    $('#' + _caseFields.PriorityId).val(updatedInfo.Priority_Id).change();

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

    $('#' + _caseFields.Region_Id).val(updatedInfo.Region_Id).change();
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
        { 'departmentId': deptId, 'myTime' : Date.now() },
        function (response) {
            if (response.result === 'success') {
                if (response.data != null) {
                    var dt = new Date(parseInt(response.data.replace("/Date(", "").replace(")/", ""), 10));                                        
                    me.$watchDate.datepicker('update', dt);

                    var readOnly = $(me.$watchDateEdit).attr("readonly");
                    if (readOnly != undefined && readOnly.toLowerCase() == 'readonly') {
                        var dateText = dt.format('yyyy-MM-dd');
                        me.$watchDateEdit.val(dateText);
                    }
                }               
            }
        });
};

EditPage.prototype.reExtendCaseLock = function () {
    var self = this;
    var _parameters = self.p;
    $.post(_parameters.caseLockExtender, { lockGuid: _parameters.caseLockGuid, extendValue: _parameters.extendValue },
        function (data) {
            if (data == false) {
                clearInterval(self.timerId);
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

EditPage.prototype.getValidationErrorMessage = function (extraMessage) {
    var me = this;
    var validationMessages = me.p.casesScopeInitParameters.validationMessages || '';
    var requiredFieldsMessage = me.p.casesScopeInitParameters.requiredFieldsMessage || '';
    var mandatoryFieldsText = me.p.casesScopeInitParameters.mandatoryFieldsText || '';
    var messages = [requiredFieldsMessage, '<br />', mandatoryFieldsText, ':'];
    $('label.error').each(function (key, el) {
        var errorText;
        if ($(el).css('display') === 'none') {
            return true;
        }
        errorText = $(el).text();
        $.each(validationMessages, function (index, validationMessage) {
            errorText = '<br />' + '[' + dhHelpdesk.cases.utils.replaceAll(errorText, validationMessage, '').trim() + ']';
        });
        messages.push(errorText);
    });    

    messages.push(extraMessage);

    return messages.join('');
};

EditPage.prototype.isFormValid = function() {
    var me = this;
    $('#btnAddCaseFile').removeClass('error');

    if (!me.isProductAreaValid()) {
        me.$productAreaObj.addClass("error");
        dhHelpdesk.cases.utils.showError(me.productAreaErrorMessage);
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

        try 
        {
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

    if (!self.isFormValid()) {
        self.setCaseStatus(self.CASE_IN_IDLE);
        return false;
    }

    var $_ex_Container = self.getExtendedCaseContainer();
    if (!self.isNullOrUndefined($_ex_Container)) {
        if (!self.isExtendedCaseValid()) {
            self.setCaseStatus(self.CASE_IN_IDLE);            
            return false;
        }

        var promise = $_ex_Container.contentWindow.saveExtendedCase(false);
        return promise.then(self.doSaveCase(submitUrl), self.onSaveError);
    } else {
        return self.doSaveCase(submitUrl);
    }
    
}

EditPage.prototype.doSaveCase = function (submitUrl) {
    var self = this;
    var action = submitUrl || '/Cases/Edit';
    self.$form.attr("action", action);
    
    if (self.case.isNew()) {
        self.stopCaseLockTimer();
        self.$form.submit();
        return false;
    }

    self.stopCaseLockTimer();
    $.post(window.parameters.caseLockChecker, {
        caseId: self.p.currentCaseId,
        caseChangedTime: self.p.caseChangedTime,
        lockGuid: self.p.caseLockGuid
    }, function (data) {
        if (data == true) {
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
    switch (status)
    {
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

EditPage.prototype.confirmDialog = function (d, onOk, onCancel)
{
    var title = dhHelpdesk.CaseArticles.translate("Meddelande");
    var ok = dhHelpdesk.CaseArticles.translate("Ok");
    var decline = dhHelpdesk.CaseArticles.translate("Avbryt");
    return d.dialog({
        title: title,
        buttons: [
            {
                text: ok,
                click: function(){
                    onOk();
                    d.dialog("close");
                },
                class: 'btn'
            },
            {
                text: decline,
                click: function(){
                    onCancel();
                    d.dialog("close");
                },
                class: 'btn'
            }
        ],
        modal: true
    });
};

EditPage.prototype.CaseWillFinish = function ()
{
    if ($('#CaseLog_FinishingDate').val() != '' || $('#CaseLog_FinishingType').val() != '') {
        return true;
    }
    else {
        return false;
    }
};

EditPage.prototype.onSaveYes = function () {    
    var self = this;
    var c = self.case;
    var url = self.EDIT_CASE_URL;
    if (c.isNew() && c.isChildCase()) {
        url = self.SAVE_GOTO_PARENT_CASE_URL;
    }
    return self.primaryValidation(url);   
};

EditPage.prototype.onSaveAndNewYes = function (){
    var self = this;
    return self.primaryValidation(self.SAVE_ADD_CASE_URL);    
};

EditPage.prototype.onSaveAndCloseYes = function () {
    var self = this;
    return self.primaryValidation(self.NEW_CLOSE_CASE_URL);       
};

EditPage.prototype.onSaveClick = function () {
    var self = this;
    var c = self.case;
    var url = self.EDIT_CASE_URL;
    if (c.isNew() && c.isChildCase()) {
        url = self.SAVE_GOTO_PARENT_CASE_URL;
    }
    return self.primaryValidation(url);   
};

EditPage.prototype.onSaveAndCloseClick = function () {
    var self = this;
    return self.primaryValidation(self.NEW_CLOSE_CASE_URL);   
};

EditPage.prototype.onSaveAndNewClick = function () {
    var self = this;
    return self.primaryValidation(self.SAVE_ADD_CASE_URL);    
};

EditPage.prototype.onSaveError = function (err) {    
    ShowToastMessage("Extended Case save was not succeed!", "error", false);
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

EditPage.prototype.doDeleteCase = function(c) {
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

    $.post(_parameters.caseLockChecker,
        {
            caseId: _parameters.currentCaseId,
            caseChangedTime: _parameters.caseChangedTime,
            lockGuid: _parameters.caseLockGuid
        })
        .done(callAsMe(function(data) {
            me.showDeleteConfirmationDlg();
            if (data == true) {
                me.doDeleteCase(me.case);
            } else {
                ShowToastMessage(_parameters.deleteLockedCaseMessage, "error", true);
            }
        }));
    return true;
};

EditPage.prototype.initDeleteConfirmationDlg = function() {
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

EditPage.prototype.onPageLeave = function(ev) {
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

EditPage.prototype.onCloseClick = function(ev) {
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

EditPage.prototype.changeCaseButtonsState = function(state) {
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

/***** Initiator *****/
EditPage.prototype.init = function (p) {
    var self = this;
    self._inSaving = false;
    self.p = p;

    EditPage.prototype.Case_Field_Ids = p.caseFieldIds;
    EditPage.prototype.Case_Field_Init_Values = p.caseInitValues;
    EditPage.prototype.Current_EC_FormId = p.extendedCaseFormId;
    EditPage.prototype.Current_EC_Guid = p.extendedCaseGuid;
    EditPage.prototype.Current_EC_LanguageId = p.extendedCaseLanguageId;
    EditPage.prototype.Current_EC_Path = p.extendedCasePath;
        
    /// controls binding
    self.$form = $('#target');
    self.$watchDateChangers = $('.departments-list, #case__Priority_Id, #case__StateSecondary_Id');
    self.$department = $('.departments-list');
    self.$SLASelect = $('#case__Priority_Id');
    self.$SLAText = $('#case__Priority_Id');    
    self.$watchDateEdit = $('#case__WatchDate');
    self.$watchDate = $('#divCase__WatchDate');      
    self.$buttonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
                             '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                             '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton');

    self.$productAreaObj = $('#divProductArea');
    self.$productAreaChildObj = $('#ProductAreaHasChild');
    self.productAreaErrorMessage = self.p.productAreaErrorMessage;
    self.$moveCaseButton = $("#btnMoveCase");    
    self.$btnSave = $('.btn.save');
    self.$btnSaveClose = $('.btn.save-close');
    self.$btnSaveNew = $('.btn.save-new');
    self.$btnDelete = $('.caseDeleteDialog.btn');
    self.$btnClose = $('.btn.close-page');
    self.deleteDlg = self.initDeleteConfirmationDlg();
    self.leaveDlg = self.initLeaveConfirmationDlg();
    
    self.$btnPrint = $('.btn.print-case');    
    self.$printArea = $('#CasePrintArea');
    self.$printDialog = $('#PrintCaseDialog');
    self.$descriptionDialog = $('#caseDescriptionPreview');
    self.$caseDescriptionEl = $(".case-description")
    self.$caseTab = $("#tabsArea li a");//$('#case-tab');
    self.$activeTabHolder = $('#ActiveTab');

    self.$selectListStep = $("#steps");
    self.$btnGo = $('#btnGo');

    self.isCaseFileMandatory = self.p.isCaseFileMandatory;

    var invoiceElm = $('#CustomerSettings_ModuleCaseInvoice').val();
    self.invoiceIsActive = invoiceElm != undefined && invoiceElm != null && invoiceElm.toString().toLowerCase() == 'true';

    ///////////////////////     events binding      /////////////////////////////////
    self.$btnDelete.on('click', callAsMe(self.showDeleteConfirmationDlg, self));
    self.$btnClose.on('click', Utils.callAsMe(self.onCloseClick, self));
    self.$btnSave.on('click', Utils.callAsMe(self.onSaveClick, self));
    self.$btnSaveClose.on('click', Utils.callAsMe(self.onSaveAndCloseClick, self));
    self.$btnSaveNew.on('click', Utils.callAsMe(self.onSaveAndNewClick, self));
    $('.lang.dropdown-submenu a').on('click', Utils.callAsMe(self.onPageLeave, self));

    $(".dropdown-submenu.DynamicDropDown_Up").mousemove(function (event) {
        self.dynamicDropDownBehaviorOnMouseMove();
    });

    $(window).scroll(function () {
        self.setDynamicDropDowns();
    });

    self.setDynamicDropDowns();

    /*Load extended case*/
    self.loadExtendedCaseIfNeeded();    

    self.$watchDateChangers.on('change', function () {        
        var deptId = parseInt(self.$department.val(), 10);
        var SLA = parseInt(self.$SLASelect.find('option:selected').attr('data-sla'), 10);
        if (isNaN(SLA)) {            
            SLA = parseInt(self.$SLAText.attr('data-sla'), 10);
        }
        if (this.id == "case__StateSecondary_Id") {
            $.post('/Cases/ChangeStateSecondary', { 'id': $(this).val() }, function (data) {
                if (data.ReCalculateWatchDate == 1) {
                    if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
                        return self.fetchWatchDateByDept.call(self, deptId);
                    }
                }
            }, 'json');
            return;
        }

        if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
            return self.fetchWatchDateByDept.call(self, deptId);
        }
        //else {
        //    if (self.$watchDateEdit.val() == '')
        //        self.$watchDate.datepicker('update', '');
        //}

        return false;
    });
    
    self.$moveCaseButton.click(function (e) {
        e.preventDefault();
        $.post(p.caseLockChecker,
            {
                caseId: p.currentCaseId,
                caseChangedTime: p.caseChangedTime,
                lockGuid: p.caseLockGuid
            },
            function (data) {
                if (data == true) {
                    window.moveCase(p.currentCaseId);
                } else {
                    ShowToastMessage(p.moveLockedCaseMessage, "error", true);
                }
        });
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
                curTime: new Date.now()
            },                

            function (_reportPresentation) {
                self.$printArea.html(_reportPresentation);
                                        
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
            .attr("value")
            .replace(/\r\n|\r|\n/g, "<br />");

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
        if (self.p.containsExtendedCase) {
            if (ev.target.className == "case") {
                self.$activeTabHolder.val('case-tab');                
                self.syncCaseFromExCaseIfExists();
                
                //validate extended case
                //TODO: shold we have this here?
                self.isExtendedCaseValid(false);
            }
            
            if (ev.target.className == "extendedcase") {
                self.$activeTabHolder.val('extendedcase-tab');
            }


        }

        if (self.p.Contains_Eform) {
            if (ev.target.className.indexOf("case") >= 0) {
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

            var isValid = self.isExtendedCaseValid(true);

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
        if (self.p.containsExtendedCase)
        {
            self.setNextStep();

            var stepId = parseInt($('#steps').val()) || 0;

            if (stepId > 0)
            {
                self.isExtendedCaseValid(false,true);
            }
            else
            {
                self.isExtendedCaseValid(false,false);
            }
        }
    });

    self.$caseTab.on('shown', function (e) {
        window.scrollTo(0, 0);
    });

    /* Temporary disabled since Extended Case does not use token */
    //self.recoverTokenIfNeeded();
};



