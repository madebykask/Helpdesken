"use strict";

var _parameters = window.parameters;
   

$(function () {
    var $userId = $('#case__ReportedBy');
    
    var $updateUserInfo = $('#UpdateNotifierInformation');

    function hideShowSaveUserInfoBtn(userId) {
        if (userId && userId.length > 0) {
            $updateUserInfo.bootstrapSwitch('readonly', false);
        } else {
            $updateUserInfo.bootstrapSwitch('state', false).bootstrapSwitch('readonly', true);
        }
    }

    $('.icon-info-sign.ml15.expander').on('click', function () {
        var expandEl = $(this).attr('data-expand-element');
        var logId = $(this).attr('data-expand-log-id');
        if (logId == undefined)
            logId = "";

        var lastSelected = $('#lastMailSelected' + logId).val();
        var displayType = '';
        if (lastSelected == expandEl) {
            displayType = 'none';
            $('#lastMailSelected' + logId).val('');
        }
        else {
            displayType = 'block';
            $('#lastMailSelected' + logId).val(expandEl);
        }

        switch (expandEl) {
            case "mailto":                
                $('#mailto' + logId ).css('display', displayType);
                $('#mailcc' + logId).css('display', 'none');
                break;
            case "mailcc":                
                $('#mailcc' + logId).css('display', displayType);
                $('#mailto' + logId).css('display', 'none');
                break;
        }
    });

    hideShowSaveUserInfoBtn($userId.val());
    
    $userId.on('change', function (ev) {
        
        var userId = $(ev.target).val();
        
        hideShowSaveUserInfoBtn(userId);
    });

    $("#case-order-url").off("click").on('click', function (e) {
        e.preventDefault();

        var href = $(this).attr("href");
        document.location.href = href;
    });

    $("#case-orderaccount-url").off("click").on('click', function (e) {
        e.preventDefault();

        var href = $(this).attr("href");
        document.location.href = href;
    });

    var langEl = $('#case__RegLanguage_Id'),
        doNotSendEl = $("#CaseMailSetting_DontSendMailToNotifier");

    var page = new EditPage();
    page.init(window.parameters);
    
    doNotSendEl.on('change', function() {
        if ($(doNotSendEl).is(':checked')) {
            langEl.show();
        } else {
            langEl.hide();
        }
    });
    if ($(doNotSendEl).is(':checked')) {
        langEl.show();
    } else {
        langEl.hide();
    }

    $("a.btn.show-inventory").on("click", function (e) {
        $.ajax({
            url: "/workstation/GetWorkstationId",
            type: "POST",
            async: false,
            data:
            {
                computerName: $("#case__InventoryNumber").val()
            },
            success: function (pcId) {
                if (pcId > 0) {
                    var newWindow = window.open("", "_blank", "width=1400,height=600,menubar=no,toolbar=no,location=no,status=no,left=100,top=100,scrollbars=yes,resizable=yes");
                    var url = window.parameters.casesScopeInitParameters.getWorkstationIdUrl + "?id=" + pcId + "&dialog=true";
                    newWindow.location.href = url;
                } else {
                    ShowToastMessage(window.parameters.noResultLabel, "warning");
                }
            },
            error: function () {
                ShowToastMessage(window.parameters.noResultLabel, "warning");
            }
        });
    });

    if (!window.dhHelpdesk) {
        window.dhHelpdesk = {};
    }

    if (!window.dhHelpdesk.cases) {
        window.dhHelpdesk.cases = {};
    }

    dhHelpdesk.cases.utils = {
        okText: '',
        cancelText: '',
        yesText: '',
        noText: '',

        init: function (okText, cancelText, yesText, noText) {
            dhHelpdesk.cases.utils.okText = okText;
            dhHelpdesk.cases.utils.cancelText = cancelText;
            dhHelpdesk.cases.utils.yesText = yesText;
            dhHelpdesk.cases.utils.noText = noText;
        },

        showMessage: function (message, type) {
            $().toastmessage('showToast', {
                text: dhHelpdesk.cases.utils.replaceAll(message, '|', '<br />'),
                sticky: false,
                position: 'top-center',
                type: type || 'notice',
                closeText: '',
                stayTime: 10000,
                inEffectDuration: 1000,
                width: 700
            });
        },

        showWarning: function (message) {
            dhHelpdesk.cases.utils.showMessage(message, 'warning');
        },

        showError: function (message) {
            dhHelpdesk.cases.utils.showMessage(message, 'error');
        },

        replaceAll: function (string, omit, place, prevstring) {
            if (prevstring && string === prevstring)
                return string;
            prevstring = string.replace(omit, place);
            return dhHelpdesk.cases.utils.replaceAll(prevstring, omit, place, string);
        },

        refreshDepartments: function (caseEntity, keepOldValue) {
            var regions = caseEntity.getUser().getRegion().getElement();
            var departments = caseEntity.getUser().getDepartment().getElement();
            var administrators = caseEntity.getOther().getAdministrator().getElement();
            var departmentFilterFormat = caseEntity.getUser().getDepartmentFilterFormat().getElement();
            var selectedDepartment = departments.val();
            departments.prop('disabled', true);                        

            $.getJSON(caseEntity.getGetDepartmentsUrl() + '?regionId=' + regions.val() +
                                        '&administratorId=' + administrators.val() + 
                                        '&departmentFilterFormat=' + departmentFilterFormat.val(), function (data) {
                                            departments.empty();
                                            departments.append('<option />');
                                            for (var i = 0; i < data.length; i++) {
                                                var item = data[i];
                                                var option = $("<option value='" + item.Value + "'>" + item.Name + "</option>");
                                                if (keepOldValue && option.val() == selectedDepartment) { 
                                                    option.prop("selected", true);
                                                }
                                                departments.append(option);
                                            }

                                            dhHelpdesk.cases.utils.refreshOus(caseEntity, keepOldValue);
                                        })
            .always(function () {
                departments.prop('disabled', false);
            });
        },

        refreshOus: function(caseEntity, keepOldValue) {
            var customerId = caseEntity.getCustomerId().getElement().val();
            var departments = caseEntity.getUser().getDepartment().getElement();
            var ous = caseEntity.getUser().getOu().getElement();
            var departmentFilterFormat = caseEntity.getUser().getDepartmentFilterFormat().getElement();

            var selectedOu = ous.val();
            ous.prop('disabled', true);            

            $.getJSON(caseEntity.getGetDepartmentOusUrl() +
                    '?id=' + departments.val() +
                    '&customerId=' + customerId +
                    '&departmentFilterFormat=' + departmentFilterFormat.val(), function (data) {                                                
                        ous.empty();
                        ous.append('<option />');
                        for (var i = 0; i < data.list.length; i++) {
                            var item = data.list[i];
                            var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                            if (keepOldValue && option.val() == selectedOu) {
                                option.prop("selected", true);
                            }
                            ous.append(option);
                        }
                    })
            .always(function () {
                ous.prop('disabled', false);
            });
        },

        refreshAdministrators: function (caseEntity, keepOldValue) {
            var departments = caseEntity.getUser().getDepartment().getElement();
            var administrators = caseEntity.getOther().getAdministrator().getElement();
            var workingGroups = caseEntity.getOther().getWorkingGroup().getElement();
            var dontConnectUserToWorkingGroup = caseEntity.getUser().getDontConnectUserToWorkingGroup().getElement();

            var selectedAdministrator = administrators.val();

            administrators.prop('disabled', true);
            administrators.empty();
            administrators.append('<option />');

            $.getJSON(caseEntity.getGetDepartmentUsersUrl() + 
                                '?departmentId=' + departments.val() +
                                '&workingGroupId=' + (dontConnectUserToWorkingGroup.val() == 0 ? workingGroups.val() : ''), function (data) {
                                for (var i = 0; i < data.length; i++) {
                                    var item = data[i];
                                    var option = $("<option value='" + item.Value + "'>" + item.Name + "</option>");
                                    if (keepOldValue && option.val() == selectedAdministrator) {
                                        option.prop("selected", true);
                                    }
                                    administrators.append(option);
                                }
                            })
            .always(function () {
                administrators.prop('disabled', false);
            });
        },

        delay: function() {
            var timer = 0;
            return function(callback, ms){
                clearTimeout (timer);
                timer = setTimeout(callback, ms);
            };
        }(),

        raiseEvent: function (eventType, extraParameters) {
            $(document).trigger(eventType, extraParameters);
        },

        onEvent: function (event, handler) {
            $(document).on(event, handler);
        },

        confirmDialog: function (text, onOk, onCancel, yesNo) {
            var firstText = yesNo ? dhHelpdesk.cases.utils.yesText : dhHelpdesk.cases.utils.okText;
            var secondText = yesNo ? dhHelpdesk.cases.utils.noText : dhHelpdesk.cases.utils.cancelText;

            var d = $('<div class="modal fade">' +
                            '<div class="modal-dialog">' +
                                '<form method="post" id="deleteDialogForm" class="modal-content">' +
                                    '<div class="modal-body">' +
                                        '<button type="button" class="close" data-dismiss="modal">&times;</button>' +
                                        '<p class="alert alert-info infop">' + text + '</p>' +
                                    '</div>' +
                                    '<div class="modal-footer">' +
                                        '<button type="button" class="btn btn-ok">' + firstText + '</button>' +
                                        '<button type="button" class="btn btn-cancel">' + secondText + '</button>' +
                                    '</div>' +
                                '</form>' +
                            '</div>' +
                        '</div>');

            d.on("show", function () {
                d.find(".btn-cancel").on("click", function (e) {
                    onCancel();
                    d.modal('hide');
                });

                d.find(".btn-ok").on("click", function (e) {
                    onOk();
                    d.modal('hide');
                });
            });

            d.on("hide", function () {
                d.find(".btn-ok").off("click");
                d.find(".btn-cancel").off("click");
            });

            d.on("hidden", function () {
                d.remove();
            });

            d.modal({
                "backdrop": "static",
                "keyboard": true,
                "show": true
            });
        }
    }

    dhHelpdesk.cases.object = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var element = spec.element || {};

        my.element = element;

        var getElement = function() {
            return element;
        }

        var isEmpty = function() {
            return element.length == 0;
        }

        that.getElement = getElement;
        that.isEmpty = isEmpty;

        return that;
    }

    dhHelpdesk.cases.file = function (spec, my) {
        my = my || {};
        var that = {};

        var id = spec.id || null;
        var name = spec.name || '';
        var deleteFile = spec.deleteFile || {};

        var getId = function() {
            return id;
        }

        var getName = function() {
            return name;
        }

        var getDeleteFile = function () {
            return deleteFile;
        }

        that.getId = getId;
        that.getName = getName;
        that.getDeleteFile = getDeleteFile;

        return that;
    }

    dhHelpdesk.cases.caseFields = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var caseEntity = spec.caseEntity || {};

        var getCase = function () {
            return caseEntity;
        }

        var setCase = function (c) {
            caseEntity = c;
        }

        var init = function() {
            
        }

        that.getCase = getCase;
        that.setCase = setCase;
        that.init = init;

        my.caseEntity = caseEntity;

        return that;
    }
   
    dhHelpdesk.cases.user = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.cases.caseFields(spec, my);

        var relatedCasesUrl = spec.relatedCasesUrl || '';
        var relatedInventoryUrl = spec.relatedInventoryUrl || '';
        var relatedCasesCountUrl = spec.relatedCasesCountUrl || '';
        var relatedInventoryCountUrl = spec.relatedInventoryCountUrl || '';
        var userId = spec.userId || {};
        var region = spec.region || {};
        var department = spec.department || {};
        var ou = spec.ou || {};
        var departmentFilterFormat = spec.departmentFilterFormat || {};
        var dontConnectUserToWorkingGroup = spec.dontConnectUserToWorkingGroup || {};
        var relatedCases = spec.relatedCases || {};
        var relatedInventory = spec.relatedInventory || {};
        relatedCases.getElement().hide();
        relatedInventory.getElement().hide();

        var getRelatedCasesUrl = function() {
            return relatedCasesUrl;
        }

        var getRelatedInventoryUrl = function () {
            return relatedInventoryUrl;
        }

        var getRelatedCasesCountUrl = function() {
            return relatedCasesCountUrl;
        }

        var getRelatedInventoryCountUrl = function () {
            return relatedInventoryCountUrl;
        }

        var getUserId = function() {
            return userId;
        }

        var getRegion = function() {
            return region;
        }

        var getDepartment = function() {
            return department;
        }

        var getOu = function() {
            return ou;
        }

        var getDepartmentFilterFormat = function() {
            return departmentFilterFormat;
        }

        var getDontConnectUserToWorkingGroup = function() {
            return dontConnectUserToWorkingGroup;
        }

        var getRelatedCases = function() {
            return relatedCases;
        }

        var getRelatedInventory = function () {
            return relatedInventory;
        }

        that.getRelatedCasesUrl = getRelatedCasesUrl;
        that.getRelatedCasesCountUrl = getRelatedCasesCountUrl;
        that.getRelatedInventoryCountUrl = getRelatedInventoryCountUrl;
        that.getUserId = getUserId;
        that.getRegion = getRegion;
        that.getDepartment = getDepartment;
        that.getOu = getOu;
        that.getDepartmentFilterFormat = getDepartmentFilterFormat;
        that.getDontConnectUserToWorkingGroup = getDontConnectUserToWorkingGroup;
        that.getRelatedCases = getRelatedCases;

        that.init = function (caseEntity) {
            var checkRelatedCases = function (uId) {
                var caseId = that.getCase().getCaseId().getElement();
                var userIdValue = uId || userId.getElement().val();
                if (userIdValue == null || userIdValue.trim() == '') {
                    relatedCases.getElement().hide();
                    return;
                }
                $.getJSON(getRelatedCasesCountUrl() + 
                            '?caseId=' + caseId.val() +
                            '&userId=' + encodeURIComponent(userIdValue), function (data) {
                                if (data > 0) {
                                    relatedCases.getElement().show();
                                } else {
                                    relatedCases.getElement().hide();
                                }
                            });
            }
            if (!relatedCases.isEmpty()) {
                checkRelatedCases();
            }

            var checkRelatedInventory = function (uId) {
                var userIdValue = uId || userId.getElement().val();
                if (userIdValue == null || userIdValue.trim() == '') {
                    relatedInventory.getElement().hide();
                    return;
                }
                $.getJSON(getRelatedInventoryCountUrl() +
                            "?userId=" + encodeURIComponent(userIdValue), function (data) {
                                if (data > 0) {
                                    relatedInventory.getElement().show();
                                } else {
                                    relatedInventory.getElement().hide();
                                }
                            });
            }
            if (!relatedInventory.isEmpty()) {
                checkRelatedInventory();
            }

            userId.getElement().keyup(function() {
                dhHelpdesk.cases.utils.delay(checkRelatedCases, 500);
                dhHelpdesk.cases.utils.delay(checkRelatedInventory, 500);
            });

            relatedCases.getElement().click(function () {
                var caseId = that.getCase().getCaseId().getElement();
                var userIdValue = encodeURIComponent(userId.getElement().val());
                window.open(getRelatedCasesUrl() + '?caseId=' + caseId.val() + '&userId=' + userIdValue, '_blank');
            });

            relatedInventory.getElement().click(function () {
                var caseId = that.getCase().getCaseId().getElement();
                var userIdValue = encodeURIComponent(userId.getElement().val());
                window.open(getRelatedInventoryUrl() + "?userId=" + userIdValue, "_blank", "width=1400,height=600,menubar=no,toolbar=no,location=no,status=no,left=100,top=100,scrollbars=yes,resizable=yes");
            });

            region.getElement().change(function () {
                //ClearCostCentre();
                // uncomment for implementing http://redmine.fastdev.se/issues/10995
                // dhHelpdesk.cases.utils.refreshDepartments(caseEntity);
            });

            department.getElement().change(function () {
                //ClearCostCentre();
                // uncomment for implementing http://redmine.fastdev.se/issues/10995
                /*dhHelpdesk.cases.utils.refreshOus(caseEntity);
                dhHelpdesk.cases.utils.refreshAdministrators(caseEntity, true);*/
            });
            ou.getElement().change(function () {
                //ClearCostCentre();
            });

            dhHelpdesk.cases.utils.onEvent("OnUserIdChanged", function(e, uId) {
                checkRelatedCases(uId);
            });
        }

        return that;
    }
    
    dhHelpdesk.cases.computer = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.cases.caseFields(spec, my);

        return that;
    }

    dhHelpdesk.cases.caseInfo = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.cases.caseFields(spec, my);

        var deleteCaseFileConfirmMessage = spec.deleteCaseFileConfirmMessage || '';
        var caseFiles = spec.caseFiles || [];

        var getDeleteCaseFileConfirmMessage = function() {
            return deleteCaseFileConfirmMessage;
        }

        var getCaseFiles = function() {
            return caseFiles;
        }

        var addCaseFile = function(caseFile) {
            caseFiles.push(caseFile);

            caseFile.getDeleteFile().getElement().click(function (e, args) {
                if (args && args.self) {
                    return;
                }

                e.stopImmediatePropagation();
                dhHelpdesk.cases.utils.confirmDialog(deleteCaseFileConfirmMessage,
                    function() {
                        caseFile.getDeleteFile().getElement().triggerHandler('click', [{ self: true }]);
                    },
                    function() {                        
                    }, true);
            });
        }

        var getCaseFile = function(id) {
            for (var i = 0; i < caseFiles.length; i++) {
                var caseFile = caseFiles[i];
                if (caseFile.getId() == id) {
                    return caseFile;
                }
            }

            return null;
        }

        var deleteCaseFile = function(caseFile) {
            for (var i = 0; i < caseFiles.length; i++) {
                var file = caseFile[i];
                if (file.getId() == caseFile.getId()) {
                    caseFiles.splice(i, 1);
                    return;
                }
            }
        }

        var clearCaseFiles = function() {
            caseFiles = [];
        }

        that.getDeleteCaseFileConfirmMessage = getDeleteCaseFileConfirmMessage;
        that.getCaseFiles = getCaseFiles;
        that.addCaseFile = addCaseFile;
        that.getCaseFiles = getCaseFile;
        that.deleteCaseFile = deleteCaseFile;
        that.clearCaseFiles = clearCaseFiles;

        return that;
    }

    dhHelpdesk.cases.other = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.cases.caseFields(spec, my);

        var administrator = spec.administrator || {};
        var workingGroup = spec.workingGroup || {};

        var getAdministrator = function() {
            return administrator;
        }

        var getWorkingGroup = function() {
            return workingGroup;
        }

        that.getAdministrator = getAdministrator;
        that.getWorkingGroup = getWorkingGroup;

        that.init = function(caseEntity) {            
            administrator.getElement().change(function () {
                // uncomment for implementing http://redmine.fastdev.se/issues/10995
                // dhHelpdesk.cases.utils.refreshDepartments(caseEntity, true);
            });

            workingGroup.getElement().change(function () {
                // uncomment for implementing http://redmine.fastdev.se/issues/10995
                // dhHelpdesk.cases.utils.refreshAdministrators(caseEntity);
            });
        }

        return that;
    }

    dhHelpdesk.cases.log = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.cases.caseFields(spec, my);
        
        var deleteCaseLogFileConfirmMessage = spec.deleteCaseLogFileConfirmMessage || '';
        var caseLogFiles = spec.caseLogFiles || [];

        var getDeleteCaseLogFileConfirmMessage = function () {
            return deleteCaseLogFileConfirmMessage;
        }

        var getCaseLogFiles = function () {
            return caseLogFiles;
        }

        var addCaseLogFile = function (caseLogFile) {
            caseLogFiles.push(caseLogFile);

            caseLogFile.getDeleteFile().getElement().click(function (e, args) {
                if (args && args.self) {
                    return;
                }

                e.stopImmediatePropagation();
                dhHelpdesk.cases.utils.confirmDialog(deleteCaseLogFileConfirmMessage,
                    function () {
                        caseLogFile.getDeleteFile().getElement().triggerHandler('click', [{ self: true }]);
                    },
                    function () {
                    }, true);
            });
        }

        var getCaseLogFile = function (id) {
            for (var i = 0; i < caseLogFiles.length; i++) {
                var caseLogFile = caseLogFiles[i];
                if (caseLogFile.getId() == id) {
                    return caseLogFile;
                }
            }

            return null;
        }

        var deleteCaseLogFile = function (caseLogFile) {
            for (var i = 0; i < caseLogFiles.length; i++) {
                var file = caseLogFile[i];
                if (file.getId() == caseLogFile.getId()) {
                    caseLogFiles.splice(i, 1);
                    return;
                }
            }
        }

        var clearCaseLogFiles = function () {
            caseLogFiles = [];
        }

        that.getDeleteCaseLogFileConfirmMessage = getDeleteCaseLogFileConfirmMessage;
        that.getCaseLogFiles = getCaseLogFiles;
        that.addCaseLogFile = addCaseLogFile;
        that.getCaseLogFiles = getCaseLogFile;
        that.deleteCaseLogFile = deleteCaseLogFile;
        that.clearCaseLogFiles = clearCaseLogFiles;

        return that;
    }

    dhHelpdesk.cases.case = function (spec, my) {
        my = my || {};
        var that = {};

        var caseId = spec.caseId || {};
        var customerId = spec.customerId || {};
        var user = spec.user || {};
        var computer = spec.computer || {};
        var caseInfo = spec.caseInfo || {};
        var other = spec.other || {};
        var log = spec.log || {};
        var getDepartmentsUrl = spec.getDepartmentsUrl || '';
        var getDepartmentUsersUrl = spec.getDepartmentUsersUrl || '';
        var getDepartmentOusUrl = spec.getDepartmentOusUrl || '';

        var getCaseId = function() {
            return caseId;
        }

        var getCustomerId = function() {
            return customerId;
        }

        var getUser = function() {
            return user;
        }

        var getComputer = function() {
            return computer;
        }

        var getCaseInfo = function() {
            return caseInfo;
        }

        var getOther = function() {
            return other;
        }

        var getLog = function() {
            return log;
        }
        var getGetDepartmentsUrl = function() {
            return getDepartmentsUrl;
        }

        var getGetDepartmentUsersUrl = function () {
            return getDepartmentUsersUrl;
        }

        var getGetDepartmentOusUrl = function() {
            return getDepartmentOusUrl;
        }

        that.getCaseId = getCaseId;
        that.getCustomerId = getCustomerId;
        that.getUser = getUser;
        that.getComputer = getComputer;
        that.getCaseInfo = getCaseInfo;
        that.getOther = getOther;
        that.getLog = getLog;
        that.getGetDepartmentsUrl = getGetDepartmentsUrl;
        that.getGetDepartmentUsersUrl = getGetDepartmentUsersUrl;
        that.getGetDepartmentOusUrl = getGetDepartmentOusUrl;

        user.setCase(that);
        computer.setCase(that);
        caseInfo.setCase(that);
        other.setCase(that);
        log.setCase(that);

        user.init(that);
        computer.init(that);
        caseInfo.init(that);
        other.init(that);
        log.init(that);

        // uncomment for implementing http://redmine.fastdev.se/issues/10995
        /*dhHelpdesk.cases.utils.refreshDepartments(that, true);
        dhHelpdesk.cases.utils.refreshAdministrators(that, true);*/

        return that;
    }

    dhHelpdesk.cases.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var requiredFieldsMessage = spec.requiredFieldsMessage || '';
        var getDepartmentsUrl = spec.getDepartmentsUrl || '';
        var getDepartmentUsersUrl = spec.getDepartmentUsersUrl || '';
        var relatedCasesUrl = spec.relatedCasesUrl || '';
        var relatedInventoryUrl = spec.relatedInventoryUrl || '';
        var relatedCasesCountUrl = spec.relatedCasesCountUrl || '';
        var relatedInventoryCountUrl = spec.relatedInventoryCountUrl || '';
        var getDepartmentOusUrl = spec.getDepartmentOusUrl || '';
        var deleteCaseFileConfirmMessage = spec.deleteCaseFileConfirmMessage || '';
        var okText = spec.okText || '';
        var cancelText = spec.cancelText || '';
        var yesText = spec.yesText || '';
        var noText = spec.noText || '';
        var validationMessages = spec.validationMessages || [];
        var mandatoryFieldsText = spec.mandatoryFieldsText || '';

        dhHelpdesk.cases.utils.init(okText, cancelText, yesText, noText);

        var user = dhHelpdesk.cases.user({
            userId: dhHelpdesk.cases.object({ element: $('[data-field="userId"]') }),
            region: dhHelpdesk.cases.object({ element: $('[data-field="region"]') }),
            department: dhHelpdesk.cases.object({ element: $('[data-field="department"]') }),
            ou: dhHelpdesk.cases.object({ element: $('[data-field="ou"]') }),
            departmentFilterFormat: dhHelpdesk.cases.object({ element: $('[data-field="departmentFilterFormat"]') }),
            dontConnectUserToWorkingGroup: dhHelpdesk.cases.object({ element: $('[data-field="dontConnectUserToWorkingGroup"]') }),
            relatedCases: dhHelpdesk.cases.object({ element: $('[data-field="relatedCases"]') }),
            relatedInventory: dhHelpdesk.cases.object({ element: $('[data-field="relatedInventory"]') }),
            relatedCasesUrl: relatedCasesUrl,
            relatedInventoryUrl: relatedInventoryUrl,
            relatedCasesCountUrl: relatedCasesCountUrl,
            relatedInventoryCountUrl: relatedInventoryCountUrl
        });
        var computer = dhHelpdesk.cases.computer({});

        var caseInfo = dhHelpdesk.cases.caseInfo({
            deleteCaseFileConfirmMessage: deleteCaseFileConfirmMessage
        });

        var other = dhHelpdesk.cases.other({
            administrator: dhHelpdesk.cases.object({ element: $('[data-field="administrator"]') }),
            workingGroup: dhHelpdesk.cases.object({ element: $('[data-field="workingGroup"]') })
        });

        var log = dhHelpdesk.cases.log({
            deleteCaseLogFileConfirmMessage: deleteCaseFileConfirmMessage
        });

        var refreshCaseFiles = function () {
            caseInfo.clearCaseFiles();

            $('[data-field="caseFile"]').each(function () {
                var $this = $(this);
                var caseFile = dhHelpdesk.cases.file({
                    id: $this.attr("data-field-id"),
                    name: $this.attr("data-field-name"),
                    deleteFile: dhHelpdesk.cases.object({ element: $this.find('[data-field="deleteFile"]') })
                });

                caseInfo.addCaseFile(caseFile);
            });

           
            if ($('#attachment-tab').length)
            {
                var nrOfFiles = parseInt($('#case_files_table tr[data-field="caseFile"]').length) || 0;
                
                $('#nrOfAttachedFiles').html('(' + nrOfFiles + ')');
            }

        }
        refreshCaseFiles();

        dhHelpdesk.cases.utils.onEvent("OnUploadedCaseFileRendered", function () {
            refreshCaseFiles();
        });

        dhHelpdesk.cases.utils.onEvent("OnDeleteCaseFile", function () {
            refreshCaseFiles();
        });

        var refreshCaseLogFiles = function () {
            log.clearCaseLogFiles();

            $('[data-field="caseLogFile"]').each(function () {
                var $this = $(this);
                var caseLogFile = dhHelpdesk.cases.file({
                    id: $this.attr("data-field-id"),
                    name: $this.attr("data-field-name"),
                    deleteFile: dhHelpdesk.cases.object({ element: $this.find('[data-field="deleteFile"]') })
                });

                log.addCaseLogFile(caseLogFile);
            });
        }
        refreshCaseLogFiles();

        dhHelpdesk.cases.utils.onEvent("OnUploadedCaseLogFileRendered", function () {
            refreshCaseLogFiles();
        });

        dhHelpdesk.cases.utils.onEvent("OnDeleteCaseLogFile", function () {
            refreshCaseLogFiles();
        });

        var caseEntity = dhHelpdesk.cases.case({
            caseId: dhHelpdesk.cases.object({ element: $('[data-field="caseId"]') }),
            customerId: dhHelpdesk.cases.object({ element: $('[data-field="customerId"]') }),
            user: user,
            computer: computer,
            caseInfo: caseInfo,
            other: other,
            log: log,
            getDepartmentsUrl: getDepartmentsUrl,
            getDepartmentUsersUrl: getDepartmentUsersUrl,
            getDepartmentOusUrl: getDepartmentOusUrl
        });

        var getCase = function() {
            return caseEntity;
        }

        that.getCase = getCase;

        return that;
    }
  
    function ClearCostCentre() {
        $('#case__CostCentre').val('');
    }

    

});
