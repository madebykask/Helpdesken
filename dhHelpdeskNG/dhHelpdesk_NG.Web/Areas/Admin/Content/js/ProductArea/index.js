"use strict";

$(document).ready(function() {
    var $showOnlyActive = $('input.showonly-active');
    var $inactiveProductAreas = $('#tblproductarea tr.inactive');
    if ($showOnlyActive.prop('checked')) {
        $inactiveProductAreas.hide();
    }

    $showOnlyActive.on('switchChange.bootstrapSwitch', function () {
        $.post('ProductArea/SetShowOnlyActiveProductAreasInAdmin', { 'value': $(this).prop('checked') });
        if ($(this).prop('checked')) {
            $inactiveProductAreas.hide(350);
        } else {
            $inactiveProductAreas.show(100);
        }
    });
});