function applyIndexBehavior(parameters) {
    if (!parameters.rememberTabUrl) throw new Error('rememberTabUrl must be specified.');
    if (!parameters.changeTopicName) throw new Error('changeTopicName must be specified.');
    if (!parameters.changesTabName) throw new Error('changesTabName must be specified.');
    if (!parameters.settingsTabName) throw new Error('settingsTabName must be specified.');

    $('#changes_tab').click(function() {
        $.post(parameters.rememberTabUrl, { topic: parameters.changeTopicName, tab: parameters.changesTabName });
        $('#save_settings_button').hide();
        $('#new_change_button').show();
        $('#export_to_excel_file_button').show();
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

    window.sortGrid = function(fieldName) {
        var sortFieldName = $('#sortFieldName').val();
        if (sortFieldName == null || sortFieldName != fieldName) {
            $('#sortFieldName').val(fieldName);
            $('#sortFieldBy').val(sortBy.ASCENDING);
        } else {
            if ($('#sortFieldBy').val() == sortBy.ASCENDING) {
                $('#sortFieldBy').val(sortBy.DESCENDING);
            } else {
                $('#sortFieldBy').val(sortBy.ASCENDING);
            }
        }

        $('#search_form').submit();
    };
}