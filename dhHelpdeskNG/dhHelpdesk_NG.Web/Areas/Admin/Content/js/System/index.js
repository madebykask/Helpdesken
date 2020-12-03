"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var systemList = new ToggableInactiveList();
    systemList.init({
        saveStateUrl: 'System/SetShowOnlyActiveSystemsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});