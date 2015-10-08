"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var operationlogcategoriesList = new ToggableInactiveList();
    operationlogcategoriesList.init({
        saveStateUrl: 'OperationLogCategory/SetShowOnlyActiveOperationLogCategoriesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});