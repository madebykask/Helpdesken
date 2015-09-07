"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var caseTypesList = new ToggableInactiveList();
    caseTypesList.init({
        saveStateUrl: 'CaseType/SetShowOnlyActiveCaseTypesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});