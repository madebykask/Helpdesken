"use strict";

$(document).ready(function() {
    var $showOnlyActive = $('input.showonly-active');
    var $inactiveProductAreas = $('#tblproductarea tr.inactive');
    $showOnlyActive.on('switchChange.bootstrapSwitch', function () {
        if ($(this).prop('checked')) {
            $inactiveProductAreas.hide(350);
        } else {
            $inactiveProductAreas.show(100);
        }
    });
});