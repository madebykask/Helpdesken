﻿function applyIndexBehavior(parameters) {
    window.sortGrid = function (fieldName) {
        var sortFieldName = $('#sortFieldName').val();
        if (sortFieldName == null || sortFieldName != fieldName) {
            $('#sortFieldName').val(fieldName);
            $('#sortFieldBy').val(sortBy.ASCENDING);
        } else {
            if ($('#sortFieldBy').val() == sortBy.ASCENDING) {
                $('#sortFieldBy').val(sortBy.DESCENDING);
            } else {
                $('#sortFieldBy').val(sortBy.ASCENDING);
            }
        }

        $('#OrdersSearchForm').submit();
    };
}