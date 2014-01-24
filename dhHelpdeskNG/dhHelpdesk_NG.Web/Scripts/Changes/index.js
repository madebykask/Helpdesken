$(function () {
    $('#changes_tab').click(function() {
        $('#save_settings_button').hide();
        $('#new_change_button').show();
    });

    $('#settings_tab').click(function() {
        $('#new_change_button').hide();
        $('#save_settings_button').show();
    });

    $('#save_settings_button').click(function() {
        $('#settings_form').submit();
    });
});