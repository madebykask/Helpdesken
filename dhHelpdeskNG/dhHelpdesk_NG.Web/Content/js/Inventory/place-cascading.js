function applyCascadingBehivarior(parameters) {
    if (!parameters.floorsSearchUrl) throw new Error('floorsSearchUrl must be specified.');
    if (!parameters.roomsSearchUrl) throw new Error('roomsSearchUrl must be specified.');

    $(function () {
        $('#buildings_dropdown').change(function () {
            var selectedBuilding = $(this).val();
            $.get(parameters.floorsSearchUrl, { selected: selectedBuilding }, function (json) {

                var sel = $('#floors_dropdown');
                $('#rooms_dropdown').empty();
                fillSelect(sel, json);
            });
        });

        $('#floors_dropdown').change(function () {
            var selectedFloor = $(this).val();
            $.get(parameters.roomsSearchUrl, { selected: selectedFloor }, function (json) {

                var sel = $('#rooms_dropdown');
                fillSelect(sel, json);
            });
        });
    });

    function fillSelect(select, json) {
        select.empty();
        select.prepend("<option></option>");

        for (var i = 0; i < json.length; i++) {
            var e = json[i];
            $('<option>').text(e.Name).val(e.Value).appendTo(select);
        }
    }
};