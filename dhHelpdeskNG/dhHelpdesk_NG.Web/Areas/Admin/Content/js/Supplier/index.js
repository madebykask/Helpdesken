"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var supplierList = new ToggableInactiveList();
    supplierList.init({
        saveStateUrl: 'Supplier/SetShowOnlyActiveSuppliersInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});