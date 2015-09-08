'use strict';

$(document).ready(function () {
    var settings = window.parameters || {};
    if (settings.currentCaseId == null) {
        throw new Error('bad page init options');
    }
    $.fn.dataTable.ext.errMode = 'throw';
    $('table.child-cases').DataTable({
        "paging":   false,
        "info": false,
        "ordering": true,
        'order': [0, 'desc']
    });
});
