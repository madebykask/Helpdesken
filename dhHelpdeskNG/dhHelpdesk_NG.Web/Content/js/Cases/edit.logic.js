"use strict";

$(function () {
    var langEl = $('#case__RegLanguage_Id'),
        doNotSendEl = $("#CaseMailSetting_DontSendMailToNotifier");
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

    if (!window.dhHelpdesk) {
        window.dhHelpdesk = {};
    }

    if (!window.dhHelpdesk.cases) {
        window.dhHelpdesk.cases = {};
    }

    dhHelpdesk.cases.utils = {
        showMessage: function (message, type) {
            $().toastmessage('showToast', {
                text: dhHelpdesk.cases.utils.replaceAll(message, '|', '<br />'),
                sticky: false,
                position: 'top-center',
                type: type || 'notice',
                closeText: '',
                stayTime: 3000,
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

        refreshDepartments: function (caseEntity) {
            var regions = caseEntity.getUser().getRegion().getElement();
            var departments = caseEntity.getUser().getDepartment().getElement();
            var administrators = caseEntity.getOther().getAdministrator().getElement();
            var departmentFilterFormat = caseEntity.getUser().getDepartmentFilterFormat().getElement();

            var selectedDepartment = departments.val();
            departments.prop('disabled', true);
            departments.empty();
            departments.append('<option />');

            $.getJSON(caseEntity.getGetDepartmentsUrl() + '?regionId=' + regions.val() +
                                        '&administratorId=' + administrators.val() + 
                                        '&departmentFilterFormat=' + departmentFilterFormat.val(), function (data) {
                                            for (var i = 0; i < data.length; i++) {
                                                var item = data[i];
                                                var option = $("<option value='" + item.Value + "'>" + item.Name + "</option>");
                                                if (option.val() == selectedDepartment) { 
                                                    option.prop("selected", true);
                                                }
                                                departments.append(option);
                                            }
                                        })
            .always(function () {
                departments.prop('disabled', false);
            });
        },

        refreshAdministrators: function (caseEntity) {
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
                                    if (option.val() == selectedAdministrator) {
                                        option.prop("selected", true);
                                    }
                                    administrators.append(option);
                                }
                            })
            .always(function () {
                administrators.prop('disabled', false);
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

        that.getElement = getElement;

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

        var userId = spec.userId || {};
        var region = spec.region || {};
        var department = spec.department || {};
        var departmentFilterFormat = spec.departmentFilterFormat || {};
        var dontConnectUserToWorkingGroup = spec.dontConnectUserToWorkingGroup || {};

        var getUserId = function() {
            return userId;
        }

        var getRegion = function() {
            return region;
        }

        var getDepartment = function() {
            return department;
        }

        var getDepartmentFilterFormat = function() {
            return departmentFilterFormat;
        }

        var getDontConnectUserToWorkingGroup = function() {
            return dontConnectUserToWorkingGroup;
        }

        that.getUserId = getUserId;
        that.getRegion = getRegion;
        that.getDepartment = getDepartment;
        that.getDepartmentFilterFormat = getDepartmentFilterFormat;
        that.getDontConnectUserToWorkingGroup = getDontConnectUserToWorkingGroup;

        that.init = function (caseEntity) {
            /*var relatedCasesTimeout = null;

            var checkRelatedCases = function () {
                clearTimeout(relatedCasesTimeout);
            }

            userId.getElement().keyup(function() {
                relatedCasesTimeout = setTimeout(checkRelatedCases, 1000);
            });*/

            region.getElement().change(function() {
                dhHelpdesk.cases.utils.refreshDepartments(caseEntity);
            });

            department.getElement().change(function () {
                dhHelpdesk.cases.utils.refreshAdministrators(caseEntity);
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
                dhHelpdesk.cases.utils.refreshDepartments(caseEntity);
            });

            workingGroup.getElement().change(function () {
                dhHelpdesk.cases.utils.refreshAdministrators(caseEntity);
            });
        }

        return that;
    }

    dhHelpdesk.cases.log = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.cases.caseFields(spec, my);
        
        return that;
    }

    dhHelpdesk.cases.case = function (spec, my) {
        my = my || {};
        var that = {};

        var user = spec.user || {};
        var computer = spec.computer || {};
        var caseInfo = spec.caseInfo || {};
        var other = spec.other || {};
        var log = spec.log || {};
        var getDepartmentsUrl = spec.getDepartmentsUrl || '';
        var getDepartmentUsersUrl = spec.getDepartmentUsersUrl || '';

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

        that.getUser = getUser;
        that.getComputer = getComputer;
        that.getCaseInfo = getCaseInfo;
        that.getOther = getOther;
        that.getLog = getLog;
        that.getGetDepartmentsUrl = getGetDepartmentsUrl;
        that.getGetDepartmentUsersUrl = getGetDepartmentUsersUrl;

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

        dhHelpdesk.cases.utils.refreshDepartments(that);
        dhHelpdesk.cases.utils.refreshAdministrators(that);

        return that;
    }

    dhHelpdesk.cases.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var requiredFieldsMessage = spec.requiredFieldsMessage || '';
        var getDepartmentsUrl = spec.getDepartmentsUrl || '';
        var getDepartmentUsersUrl = spec.getDepartmentUsersUrl || '';

        var user = dhHelpdesk.cases.user({
            userId: dhHelpdesk.cases.object({ element: $('[data-field="userId"]') }),
            region: dhHelpdesk.cases.object({ element: $('[data-field="region"]') }),
            department: dhHelpdesk.cases.object({ element: $('[data-field="department"]') }),
            departmentFilterFormat: dhHelpdesk.cases.object({ element: $('[data-field="departmentFilterFormat"]') }),
            dontConnectUserToWorkingGroup: dhHelpdesk.cases.object({ element: $('[data-field="dontConnectUserToWorkingGroup"]') })
        });
        var computer = dhHelpdesk.cases.computer({});
        var caseInfo = dhHelpdesk.cases.caseInfo({});
        var other = dhHelpdesk.cases.other({
            administrator: dhHelpdesk.cases.object({ element: $('[data-field="administrator"]') }),
            workingGroup: dhHelpdesk.cases.object({ element: $('[data-field="workingGroup"]') })
        });
        var log = dhHelpdesk.cases.log({});

        var caseEntity = dhHelpdesk.cases.case({
            user: user,
            computer: computer,
            caseInfo: caseInfo,
            other: other,
            log: log,
            getDepartmentsUrl: getDepartmentsUrl,
            getDepartmentUsersUrl: getDepartmentUsersUrl
        });

        var getCase = function() {
            return caseEntity;
        }

        that.getCase = getCase;

        // http://redmine.fastdev.se/issues/10554
        // http://redmine.fastdev.se/issues/11179
        $('#target').submit(function () {
            if (!$(this).valid()) {
                dhHelpdesk.cases.utils.showError(requiredFieldsMessage);
            } 
        });

        return that;
    }
});
