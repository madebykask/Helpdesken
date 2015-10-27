"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var causingPartList = new ToggableInactiveList();
    causingPartList.init({
        saveStateUrl: 'CausingPart/SetShowOnlyActiveCausingPartsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});