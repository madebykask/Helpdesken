$(function() {
    $('#case__StateSecondary_Id').change(function (d, source) {
        $('#CaseLog_SendMailAboutCaseToNotifier').removeAttr('disabled');
        var curVal = $('#case__StateSecondary_Id').val();
        $('#case__StateSecondary_Id option[value=' + curVal + ']').attr('selected', 'selected');
        $.post('/Cases/ChangeStateSecondary', { 'id': $(this).val() }, function (data) {
            // disable send mail checkbox
            if (data.NoMailToNotifier == 1) {
                $('#CaseLog_SendMailAboutCaseToNotifier')
                    .prop('checked', false)
                    .attr('disabled', true);
            }
            else {
                if ($('#CaseLog_TextExternal').val() === '') {
                    $('#CaseLog_SendMailAboutCaseToNotifier')
                        .prop('checked', false)
                        .attr('disabled', false);
                } else {
                    $('#CaseLog_SendMailAboutCaseToNotifier')
                        .prop('checked', true)
                        .attr('disabled', false);
                }
            }
            // set workinggroup id
            var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
            if (exists > 0 && data.WorkingGroup_Id > 0 && source !== 'case__WorkingGroup_Id') {
                $('#case__WorkingGroup_Id')
                    .val(data.WorkingGroup_Id)
                    .trigger('change', 'case__StateSecondary_Id');
            }

            if (data.ReCalculateWatchDate == 1) {
                var $departments = $('.departments-list');
                if ($departments.length > 0) {
                    var $priority = $('#case__Priority_Id');
                    var deptId = parseInt($departments.val(), 10);
                    var sla = parseInt($priority.find('option:selected').attr('data-sla'), 10);
                    if (isNaN(sla)) {
                        sla = parseInt($priority.attr('data-sla'), 10);
                    }

                    if (!isNaN(deptId) && (!isNaN(sla) && sla === 0) && window.page) {
                        window.page.fetchWatchDateByDept.call(window.page, deptId);
                    }
                }
            }
        }, 'json');
    });
});