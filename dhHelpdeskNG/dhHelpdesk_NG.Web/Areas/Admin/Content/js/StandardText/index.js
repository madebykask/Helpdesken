"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var statusList = new ToggableInactiveList();
    statusList.init({
        saveStateUrl: 'StandardText/SetShowOnlyActiveStandardTextsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});