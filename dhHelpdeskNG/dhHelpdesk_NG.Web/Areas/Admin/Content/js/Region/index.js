"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var regionList = new ToggableInactiveList();
    regionList.init({
        saveStateUrl: 'Region/SetShowOnlyActiveRegionInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});