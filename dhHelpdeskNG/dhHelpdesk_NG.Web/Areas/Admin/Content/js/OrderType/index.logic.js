"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var orderTypesList = new ToggableInactiveList();
    orderTypesList.init({
        saveStateUrl: 'OrderType/SetShowOnlyActiveOrderTypesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});