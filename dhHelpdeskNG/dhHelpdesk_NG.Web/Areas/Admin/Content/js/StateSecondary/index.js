"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var stateSecondaryList = new ToggableInactiveList();
    stateSecondaryList.init({
        saveStateUrl: 'StateSecondary/SetShowOnlyActiveStateSecondariesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});