function applySettingsBehavior(parameters) {
    if (!parameters.saveSettingsUrl) throw new Error('saveSettingsUrl must be specified.');
    if (!parameters.settingsSavedSuccessfullyMessage) throw new Error('settingsSavedSuccessfullyMessage must be specified.');
    if (!parameters.showRoomId) throw new Error('showRoomId must be specified.');
    if (!parameters.showBuildingId) throw new Error('showBuildingId must be specified.');
    if (!parameters.showFloorId) throw new Error('showFloorId must be specified.');

    $('#settings_language_dropdown').change(function() {
        $.get(parameters.saveSettingsUrl, { languageId: $(this).val(), tabLanguageId: $(this).val() }, function(settingsMarkup) {
            $('#settings_container').html(settingsMarkup);
        });
    });

    var room$ = $('#' + parameters.showRoomId);
    var building$ = $('#' + parameters.showBuildingId);
    var floor$ = $('#' + parameters.showFloorId);

    if (!room$.bootstrapSwitch('state')) {
        floor$.bootstrapSwitch('disabled', true);
        building$.bootstrapSwitch('disabled', true);
    }

    if (!floor$.bootstrapSwitch('state')) {
        building$.bootstrapSwitch('disabled', true);
    }

    room$.on('switchChange.bootstrapSwitch', function (event, state) {
        floor$.bootstrapSwitch('state', state);
        floor$.bootstrapSwitch('disabled', !state);
    });

    floor$.on('switchChange.bootstrapSwitch', function (event, state) {
        building$.bootstrapSwitch('state', state);
        building$.bootstrapSwitch('disabled', !state);
    });

    //$('#tab_settings_language_dropdown').change(function () {
    //    $.get(parameters.saveSettingsUrl, { languageId: $('#settings_language_dropdown').val(), tabLanguageId: $(this).val() }, function (settingsMarkup) {
    //        $('#settings_container').html(settingsMarkup);
    //    });
    //});

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