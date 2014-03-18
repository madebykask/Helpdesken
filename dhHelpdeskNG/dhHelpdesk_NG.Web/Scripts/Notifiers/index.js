function applyPageBehavior(parameters) {
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

    $('#language_dropdown').change(function () {
        $.get(parameters.captionsUrl, { languageId: $(this).val() }, function(captions) {
            var settingsTableBody = $('#settings_table tbody');

            for (var i = 0; i < captions.length; i++) {
                var caption = captions[i];
                var input = settingsTableBody.find('input[type="hidden"][value="' + caption.FieldName + '"]').parent().siblings('td').children('input[type="text"]').first();
                input.val(caption.Text);
            }
        });
    });
}