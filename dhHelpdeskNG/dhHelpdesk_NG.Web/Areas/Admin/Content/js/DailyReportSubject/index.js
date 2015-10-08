"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var dailyreportsubjectsList = new ToggableInactiveList();
    dailyreportsubjectsList.init({
        saveStateUrl: 'DailyReportSubject/SetShowOnlyActiveDailyReportSubjectsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});