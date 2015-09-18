"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var oUList = new ToggableInactiveList();
    oUList.init({
        saveStateUrl: 'OU/SetShowOnlyActiveOUInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});