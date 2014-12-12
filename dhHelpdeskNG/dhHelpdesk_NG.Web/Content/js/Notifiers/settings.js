function applySettingsViewBehavior(parameters) {
    $('#language_dropdown').change(function() {
        $.get(parameters.settingsUrl, { languageId: $(this).val(), now: Date.now() }, function (settingsMarkup) {
            $('#settings_container').html(settingsMarkup);
        });
    });
}