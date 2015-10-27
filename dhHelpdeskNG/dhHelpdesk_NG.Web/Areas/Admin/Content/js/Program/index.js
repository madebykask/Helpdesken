"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var programsList = new ToggableInactiveList();
    programsList.init({
        saveStateUrl: 'Program/SetShowOnlyActiveProgramsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});