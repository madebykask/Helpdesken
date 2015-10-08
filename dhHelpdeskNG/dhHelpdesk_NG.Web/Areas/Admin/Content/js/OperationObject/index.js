"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var operationobjectsList = new ToggableInactiveList();
    operationobjectsList.init({
        saveStateUrl: 'OperationObject/SetShowOnlyActiveOperationObjectsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});