"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var priorityList = new ToggableInactiveList();
    priorityList.init({
        saveStateUrl: 'Priority/SetShowOnlyActivePrioritiesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});