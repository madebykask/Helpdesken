function applySettingsViewBehavior(parameters) {
    $('#language_dropdown').change(function() {
        $.get(parameters.settingsUrl, { languageId: $(this).val() }, function(settingsMarkup) {
            $('#settings_container').html(settingsMarkup);
        });
    });
}