$(function() {
    window.recipientEmails = [];

    $('#add_emails_button').click(function() {
        appendEmailGroupEmails();
        appendWorkingGroupEmails();
        appendAdministratorEmails();
        appendCustomEmail();
    });

    $('#delete_email_button').click(deleteSelectedRecipients);
});

function deleteSelectedRecipients() {
    $('#recipient_emails_listbox > option:selected').remove();
}

function addEmailToRecipients(email) {
    var emailExists = $('#recipient_emails_listbox').has('option[value="' + email + '"]').length;
    if (emailExists) {
        return;
    }

    $('#recipient_emails_listbox').append('<option value="' + email + '">' + email + '</option>');
}

function saveRecipientEmails() {
    var emails = $('#recipient_emails_listbox > option').map(function() {
        return $(this).val();
    });

    window.recipientEmails = emails;
}

function appendWorkingGroupEmails() {
    var selectedGroupIds = $('#working_groups_dropdown').val() || [];

    for (var i = 0; i < selectedGroupIds.length; i++) {
        var id = selectedGroupIds[i];
        var emails = workingGroupEmails[id];

        for (var j = 0; j < emails.length; j++) {
            addEmailToRecipients(emails[j]);
        }
    }
}

function appendEmailGroupEmails() {
    var selectedGroupIds = $('#email_groups_dropdown').val() || [];

    for (var i = 0; i < selectedGroupIds.length; i++) {
        var id = selectedGroupIds[i];
        var emails = emailGroupEmails[id];

        for (var j = 0; j < emails.length; j++) {
            addEmailToRecipients(emails[j]);
        }
    }
}

function appendAdministratorEmails() {
    var selectedAdministratorEmails = $('#administrators_dropdown').val() || [];

    for (var i = 0; i < selectedAdministratorEmails.length; i++) {
        addEmailToRecipients(selectedAdministratorEmails[i]);
    }
}

function appendCustomEmail() {
    var customEmail = $('#custom_email_textbox').val();
    if (!customEmail) {
        return;
    }

    addEmailToRecipients(customEmail);
    $('#custom_email_textbox').val('');
}

function loadRecipientEmails() {
    $('#recipient_emails_listbox').empty();

    for (var i = 0; i < recipientEmails.length; i++) {
        var email = recipientEmails[i];
        addEmailToRecipients(email);
    }
}