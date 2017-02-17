function applyIndexBehavior(parameters) {
    window.sortGrid = function (fieldName) {
        var $sortFieldBy = $('#sortFieldBy');
        var $sortFieldName = $('#sortFieldName');
        var sortFieldName = $sortFieldName.val();
        if (sortFieldName == null || sortFieldName !== fieldName) {
            $sortFieldName.val(fieldName);
            $sortFieldBy.val(sortBy.ASCENDING);
        } else {
            if ($sortFieldBy.val() === sortBy.ASCENDING.toString()) {
                $sortFieldBy.val(sortBy.DESCENDING);
            } else {
                $sortFieldBy.val(sortBy.ASCENDING);
            }
        }

        $('#OrdersSearchForm').submit();
    };
}
