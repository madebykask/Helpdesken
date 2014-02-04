function onReady(data) {
    $('#save_and_close_button').click(function() {
        $('#change_form').submit();
        window.location.href = data.afterDeleteUrl;
    });

    $('#delete_button').click(function() {
        $.post(data.deleteUrl, { id: data.id });
    });

    $('#header_finishing_date_datepicker').datepicker();
    $('#registration_desired_date_datepicker').datepicker();
    $('#implementation_real_start_date_datepicker').datepicker();
    $('#implementation_finishing_date_datepicker').datepicker();

    $('#analyze_send_to_button').button().click(function() {
        $('#analyze_send_to_dialog').dialog('open');
    });

    $('#analyze_approve_dropdown').change(function() {
        var selectedValue = $(this).val();
        if (selectedValue == data.analyzeRejectValue) {
            $('#analyze_reject_explanation_textarea').show();
        } else {
            $('#analyze_reject_explanation_textarea').hide();
        }
    });

    $('#evaluation_send_to_button').button().click(function() {
        $('#evaluation_send_to_dialog').dialog('open');
    });

    $('#implementation_send_to_button').button().click(function() {
        $('#implementation_send_to_dialog').dialog('open');
    });

    $('#registration_approve_dropdown').change(function() {
        var selectedValue = $(this).val();
        if (selectedValue == data.registrationRejectValue) {
            $('#registration_reject_explanation_textarea').show();
        } else {
            $('#registration_reject_explanation_textarea').hide();
        }
    });
}