function applyIndexViewBehavior() {
    $('#notifiers_tab').click(function() {
        $('#save_button').hide();
        $('#new_notifier_button').show();
    });

    $('#settings_tab').click(function() {
        $('#new_notifier_button').hide();
        $('#save_button').show();
    });

    $('#save_button').click(function() {
        $('#settings_form').submit();
    });
}