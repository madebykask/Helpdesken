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

    // http://redmine.fastdev.se/issues/10554
    $('#target').submit(function () {
        if (!$(this).valid()) {
            $('#requiredFieldsMessage').show();
        } else {
            $('#requiredFieldsMessage').hide();
        }
    });
});
