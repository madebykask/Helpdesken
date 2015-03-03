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

        return that;
    }

    dhHelpdesk.cases.case = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.cases.object(spec, my);
        
        return that;
    }

    dhHelpdesk.cases.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var requiredFieldsMessage = spec.requiredFieldsMessage || '';

        var caseEntity = dhHelpdesk.cases.case();

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
