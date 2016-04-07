function applyPageBehavior(parameters) {
    $('#region_dropdown').change(function() {
        $.get(parameters.departmentDropDownUrl, { regionId: $(this).val() }, function(departmentDropDownMarkup) {
            $('#department_dropdown').html(departmentDropDownMarkup);
        });
    });
}

function sortGrid(fieldName) {
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

    $('#search_form').submit();
}