"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var productAreasList = new ToggableInactiveList();
    productAreasList.init({
        saveStateUrl: 'ProductArea/SetShowOnlyActiveProductAreasInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});