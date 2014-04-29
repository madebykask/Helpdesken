function applyInventoryDialogBehavior() {
    $('#general_inventory_dialog').dialog({
        autoOpen: false,
        modal: false,
        resizable: false,
        height: 600,
        width: 350,

        buttons: [
            {
                text: 'OK',
                click: function () {
                    saveSelectedInventoriesToCache();
                    $(this).dialog("close");
                },
                'class': 'btn'
            },
            {
                text: 'Cancel',
                click: function () {
                    $(this).dialog("close");
                },
                'class': 'btn'
            }
        ],

        open: function() {
            loadSelectedItemsFromCache();
        } 
    }).parent().appendTo('#change_form');

    $('#delete_selected_inventory_button').click(function() {
        $('#selected_inventories_listbox option:selected').remove();
    });

    $('#flush_selected_inventories_button').click(function() {
        var inventorySelects = $('#general_inventory_dialog > select.inventory-selection-dropdown');

        for (var i = 0; i < inventorySelects.length; i++) {
            var inventorySelect = inventorySelects[i];
            var selectedItems = $('option:selected', inventorySelect);

            for (var j = 0; j < selectedItems.length; j++) {
                var $selectedItem = $(selectedItems[j]);
                var selectedValue = $selectedItem.val();
                $(inventorySelect).multiselect('deselect', selectedValue);

                var selectedName = $selectedItem.text();
                appendInventoryNameToInventoriesListBox(selectedName);
            }
        }
    });

    function appendInventoryNameToInventoriesListBox(inventoryName) {
        var $selectedInventoriesTextArea = $('#selected_inventories_listbox');
        var newItem = '<option>' + inventoryName + '</option>';
        $selectedInventoriesTextArea.append(newItem);
    }

    function loadSelectedItemsFromCache() {

        $('#selected_inventories_listbox').empty();

        var items = $('#selected_inventories_cache option');
        
        for (var i = 0; i < items.length; i++) {
            var newItem = '<option>' + $(items[i]).val() + '</option>';
            $('#selected_inventories_listbox').append(newItem);
        }
    }

    function saveSelectedInventoriesToCache() {
        $('#selected_inventories_cache').empty();
        var itemsToAdd = $('#selected_inventories_listbox option');

        for (var i = 0; i < itemsToAdd.length; i++) {
            var newItem = '<option value="' + $(itemsToAdd[i]).text() + '" selected="selected"></option>';
            $('#selected_inventories_cache').append(newItem);
        }
    }
}