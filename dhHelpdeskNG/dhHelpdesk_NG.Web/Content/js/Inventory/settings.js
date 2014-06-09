function applyInventorySettingsBehavior(parameters) {
    if (!parameters.textModeId) throw new Error('textModeId must be specified.');

    $(".inventory_field_type_class option:not([value=" + parameters.textModeId + "]):selected, .new_inventory_field_type_class option:not([value=" + parameters.textModeId + "]):selected")
        .closest("tr")
        .find(".inventory_property_size_class, .new_inventory_property_size_class")
        .hide();
    $(".inventory_field_type_class option:not([value=" + parameters.textModeId + "]):selected, .new_inventory_field_type_class option:not([value=" + parameters.textModeId + "]):selected")
        .closest("tr")
        .find(".inventory_property_size_class, .new_inventory_property_size_class")
        .disable = true;

    $(".inventory_field_type_class").change(function () {
        var value = $(this).val();
        showHidePropertySizeTextBox($(this).closest("tr").find(".inventory_property_size_class"), value);
    });

    $(".new_inventory_field_type_class").change(function () {
        var value = $(this).val();
        showHidePropertySizeTextBox($(this).closest("tr").find(".new_inventory_property_size_class"), value);
    });

    function showHidePropertySizeTextBox(textBox, value) {
        if (value != parameters.textModeId) {
            textBox.hide();
            textBox.disable = true;
        } else {
            textBox.show();
            textBox.disable = false;
        }
    }
};

function updateForm() {
    $("#settings_form").each(function () { $.data($(this)[0], 'validator', false); });
    $.validator.unobtrusive.parse("#settings_form");
}