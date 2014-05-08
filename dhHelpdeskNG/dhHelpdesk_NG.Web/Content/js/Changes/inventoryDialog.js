function applyInventoryDialogBehavior(parameters) {
    if (!parameters.okButtonText) throw new Error('okButtonText must be specified.');
    if (!parameters.cancelButtonText) throw new Error('cancelButtonText must be specified.');

    $('#general_inventory_dialog').dialog({
        autoOpen: false,
        modal: false,
        resizable: false,
        height: 600,
        width: 350,

        buttons: [
            {
                text: parameters.okButtonText,
                click: function() {
                    saveChosenInventoriesToCache();
                    $(this).dialog("close");
                },
                'class': 'btn'
            },
            {
                text: parameters.cancelButtonText,
                click: function() {
                    $(this).dialog("close");
                },
                'class': 'btn'
            }
        ],

        open: function() {
            loadChosenInventoriesFromCache();
        }
    }).parent().appendTo('#change_form');

    $('#delete_chosen_inventory_button').click(function() {
        $('#chosen_inventories_listbox option:selected').remove();
    });

    $('#flush_selected_inventories_button').click(function () {
        var dropDowns = $('#general_inventory_dialog > select.inventory-selection-dropdown');

        for (var i = 0; i < dropDowns.length; i++) {
            var dropDown = dropDowns[i];
            var selectedInventories = $('option:selected', dropDown);

            for (var j = 0; j < selectedInventories.length; j++) {
                var $selectedInventory = $(selectedInventories[j]);
                var selectedValue = $selectedInventory.val();
                $(dropDown).multiselect('deselect', selectedValue);

                var selectedName = $selectedInventory.text();
                addChosenInventoryToCollection(selectedName);
            }
        }
    });

    function loadChosenInventoriesFromCache() {
        $('#chosen_inventories_listbox').empty();
        var itemsFromCache = $('#chosen_inventories_cache option');

        for (var i = 0; i < itemsFromCache.length; i++) {
            var newItem = '<option>' + $(itemsFromCache[i]).val() + '</option>';
            $('#chosen_inventories_listbox').append(newItem);
        }
    }

    function saveChosenInventoriesToCache() {
        $('#chosen_inventories_cache').empty();
        var itemsToSave = $('#chosen_inventories_listbox option');

        for (var i = 0; i < itemsToSave.length; i++) {
            var newItem = '<option value="' + $(itemsToSave[i]).text() + '" selected="selected"></option>';
            $('#chosen_inventories_cache').append(newItem);
        }
    }

    function addChosenInventoryToCollection(inventoryName) {
        var $selectedInventoriesTextArea = $('#chosen_inventories_listbox');
        var newItem = '<option>' + inventoryName + '</option>';
        $selectedInventoriesTextArea.append(newItem);
    }
}