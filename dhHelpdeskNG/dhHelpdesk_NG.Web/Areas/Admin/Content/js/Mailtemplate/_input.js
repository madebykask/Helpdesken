$(document).ready(function() {
    'use strict';
    var $container = $("#languageList");
    var $subject = $('#MailTemplateLanguage_Subject');
    var $body = $('#MailTemplateLanguage_Body');

    function checkSubjBodyLength() {
        var $subject = $('#MailTemplateLanguage_Subject');
        var $body = $('#MailTemplateLanguage_Body');
        var $message = $('.alert-block');
        if ($subject.val().length === 0 || $body.val().length === 0) {
            $message.show();
        } else {
            $message.hide();
        }
    }

    // checking is it "editing" or "add" mode
    if ($subject.length === 0) {
        $container.on('keyup', $subject, checkSubjBodyLength);
        $container.on('keyup', $body, checkSubjBodyLength);
    } else {
        $subject.on('keyup', checkSubjBodyLength);
        $body.on('keyup', checkSubjBodyLength);
    }
});
