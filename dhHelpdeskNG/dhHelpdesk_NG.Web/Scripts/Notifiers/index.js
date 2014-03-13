$(function() {
    $('#tab_1').click(function() {
        $('#save_button').hide();
        $('#new_notifier_button').show();
    });

    $('#tab_2').click(function() {
        $('#new_notifier_button').hide();
        $('#save_button').show();
    });

    $('#save_button').click(function() {
        $('#settings_form').submit();
    });
});