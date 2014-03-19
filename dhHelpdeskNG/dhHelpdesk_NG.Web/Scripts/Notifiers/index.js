function applyPageBehavior(parameters) {
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

    $('#language_dropdown').change(function() {
        $.get(parameters.captionsUrl, { languageId: $(this).val() }, function(captions) {
            alert('debug this');
            var settingsBody = $('#settings_table tbody');

            for (var i = 0; i < captions.length; i++) {
                var caption = captions[i];
                var input = settingsBody.find('input[type="hidden"][value="' + caption.FieldName + '"]').parent().siblings('td').children('input[type="text"]').first();
                input.val(caption.Text);
            }
        });
    });
}