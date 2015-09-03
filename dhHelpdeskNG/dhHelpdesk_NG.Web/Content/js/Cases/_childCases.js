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
//        "columnDefs": [{
//            "targets": 0,
//            "sortable": true,
//            'title': 'asdfasdfsadf'
//        }]
//        ajax: '/Cases/ChildCases/?caseId=' + settings.currentCaseId,
//        dataSrc: 'data',
//        
//        'columns': [
//            { 'data': 'CaseNo' },
//            { 'data': 'CasePerformer' },
//            { 'data': 'CaseType' },
//            { 'data': 'ClosingDate' },
//            { 'data': 'Id' },
//            { 'data': 'RegistrationDate' },
//            { 'data': 'SubStatus' },
//            { 'data': 'Subject'}
//        ]
    });
});
