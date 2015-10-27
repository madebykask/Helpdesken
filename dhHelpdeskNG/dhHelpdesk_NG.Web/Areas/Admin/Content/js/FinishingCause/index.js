"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var finishingCausesList = new ToggableInactiveList();
    finishingCausesList.init({
        saveStateUrl: 'FinishingCause/SetShowOnlyActiveFinishingCausesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});