function applyPageBehavior(parameters) {
    $('#case_summary_tab').click(function() {
        $.post(parameters.rememberTabUrl, { topic: parameters.changeTopicName, tab: parameters.caseSummaryTabName });
        $('#new_change_button').hide();
        $('#export_to_excel_file_button').hide();
        $('#save_settings_button').hide();
    });

    $('#changes_tab').click(function() {
        $.post(parameters.rememberTabUrl, { topic: parameters.changeTopicName, tab: parameters.changesTabName });
        $('#save_settings_button').hide();
        $('#new_change_button').show();
        $('#export_to_excel_file_button').show();
    });

    $('#orders_tab').click(function() {
        $.post(parameters.rememberTabUrl, { topic: parameters.changeTopicName, tab: parameters.ordersTabName });
        $('#new_change_button').hide();
        $('#export_to_excel_file_button').hide();
        $('#save_settings_button').hide();
    });

    $('#settings_tab').click(function() {
        $.post(parameters.rememberTabUrl, { topic: parameters.changeTopicName, tab: parameters.settingsTabName });
        $('#new_change_button').hide();
        $('#export_to_excel_file_button').hide();
        $('#save_settings_button').show();
    });

    $('#save_settings_button').click(function() {
        $('#settings_form').submit();
    });
}