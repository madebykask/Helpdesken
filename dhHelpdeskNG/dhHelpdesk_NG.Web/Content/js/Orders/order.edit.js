function OrderEdit(parameters) {

    $("#Orderer_DepartmentId").on("change",function() {
                departmentEdit(parameters.Units, $(this).val());
            });

    function departmentEdit(units, depId) {
        "use strict";

        var unitSelect = $('#orderer_unit_select');
        unitSelect.empty();
        var availableUnits = $.grep(units,
            function(unit) {
                return (unit.DependentId === depId);
            });
        unitSelect.prepend("<option value='' selected='selected'></option>");
        $.each(availableUnits,
            function(index, unit) {
                unitSelect.append($('<option/>',
                {
                    value: unit.Value,
                    text: unit.Name
                }));
            });
    }
}

