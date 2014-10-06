function applySettingsBehavior(parameters) {
    if (!parameters.saveSettingsUrl) throw new Error('saveSettingsUrl must be specified.');
    if (!parameters.settingsSavedSuccessfullyMessage) throw new Error('settingsSavedSuccessfullyMessage must be specified.');

    $('#settings_language_dropdown').change(function() {
        var selectedLanguageId = $(this).val();
        $.get(parameters.saveSettingsUrl, { languageId: selectedLanguageId }, function(settingsMarkup) {
            $('#settings_container').html(settingsMarkup);
        });
    });

    window.onSettingsSavedSuccessfully = function() {
        $().toastmessage('showToast', {
            text: parameters.settingsSavedSuccessfullyMessage,
            sticky: false,
            position: 'top-center',
            type: 'success',
            stayTime: 3000,
            inEffectDuration: 1000
        });
    };
}