function applyPageBehavior(parameters) {
    $('#region_dropdown').change(function() {
        $('#department_dropdown_container').load(parameters.captionsUrl, { regionId: $(this).val() }, function() {
            $('#department_dropdown').change(function() {
                $('#search_form').submit();
            });
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