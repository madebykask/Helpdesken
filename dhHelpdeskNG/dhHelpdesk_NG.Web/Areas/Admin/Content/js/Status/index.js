"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var statusList = new ToggableInactiveList();
    statusList.init({
        saveStateUrl: 'Status/SetShowOnlyActiveStatusesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});