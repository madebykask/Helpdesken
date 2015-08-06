'use strict';

$(function () {
    $(window.parameters.regionControlName).change(function () {
        var regionId = $(this).val();
        refreshDepartment(regionId);
    });

    function refreshDepartment(regionId) {
        $(window.parameters.departmentControlName).val('');
        var ctlOption = window.parameters.departmentControlName + ' option';
        $(ctlOption).remove();
        $(window.parameters.departmentControlName).append('<option value="">&nbsp;</option>');
        $(window.parameters.departmentControlName).prop('disabled', true);
        $.post(window.parameters.refreshDepartmentUrl, { 'regionId': regionId }, function (data) {

            if (data != undefined) {
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    var option = $("<option value='" + item.Value + "'>" + item.Name + "</option>");
                    $(window.parameters.departmentControlName).append(option);
                }
            }
        }, 'json').always(function () {
            $(window.parameters.departmentControlName).prop('disabled', false);
        });
    }
});
