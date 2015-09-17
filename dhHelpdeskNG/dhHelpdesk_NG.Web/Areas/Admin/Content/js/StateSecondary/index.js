"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var stateSecondaryList = new ToggableInactiveList();
    stateSecondaryList.init({
        saveStateUrl: 'StateSecondary/ShowOnlyActiveStateSecondariesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});