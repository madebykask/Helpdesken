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

        var region = spec.region || {};
        var department = spec.department || {};        

        var getRegion = function() {
            return region;
        }

        var getDepartment = function() {
            return department;
        }

        that.getRegion = getRegion;
        that.getDepartment = getDepartment;

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
        var getDepartmentsUrl = spec.getDepartmentsUrl || '';

        var getAdministrator = function() {
            return administrator;
        }

        that.getAdministrator = getAdministrator;

        that.init = function(caseEntity) {
            var onChangeAdministrator = function () {
                var regions = caseEntity.getUser().getRegion().getElement();
                var departments = caseEntity.getUser().getDepartment().getElement();
                var administrators = administrator.getElement();

                departments.prop('disabled', true);
                departments.empty();
                departments.append('<option />');

                $.getJSON(getDepartmentsUrl + '?regionId=' + regions.val() + '&administratorId=' + administrators.val(), function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        departments.append("<option value='" + item.Value + "'>" + item.Name + "</option>");
                    }
                })
                .always(function () {
                    departments.prop('disabled', false);
                });
            }
            administrator.getElement().change(onChangeAdministrator);
            onChangeAdministrator();
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

        that.getUser = getUser;
        that.getComputer = getComputer;
        that.getCaseInfo = getCaseInfo;
        that.getOther = getOther;
        that.getLog = getLog;

        user.setCase(that);
        computer.setCase(that);
        caseInfo.setCase(that);
        other.setCase(that);
        log.setCase(that);

        /*user.init(that);
        computer.init(that);
        caseInfo.init(that);
        other.init(that);
        log.init(that);*/

        return that;
    }

    dhHelpdesk.cases.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var requiredFieldsMessage = spec.requiredFieldsMessage || '';
        var getDepartmentsUrl = spec.getDepartmentsUrl || '';

        var user = dhHelpdesk.cases.user({
            region: dhHelpdesk.cases.object({ element: $('[data-field="region"]') }),
            department: dhHelpdesk.cases.object({ element: $('[data-field="department"]') })
        });
        var computer = dhHelpdesk.cases.computer({});
        var caseInfo = dhHelpdesk.cases.caseInfo({});
        var other = dhHelpdesk.cases.other({
            administrator: dhHelpdesk.cases.object({ element: $('[data-field="administrator"]') }),
            getDepartmentsUrl: getDepartmentsUrl
        });
        var log = dhHelpdesk.cases.log({});

        var caseEntity = dhHelpdesk.cases.case({
            user: user,
            computer: computer,
            caseInfo: caseInfo,
            other: other,
            log: log
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
