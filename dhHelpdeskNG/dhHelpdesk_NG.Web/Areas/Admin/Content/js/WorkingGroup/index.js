"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var workingGroupList = new ToggableInactiveList();
    workingGroupList.init({
        saveStateUrl: 'WorkingGroup/SetShowOnlyActiveWorkingGroupsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});