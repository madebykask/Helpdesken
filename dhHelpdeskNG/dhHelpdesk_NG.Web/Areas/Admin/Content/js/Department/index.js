"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var departmentList = new ToggableInactiveList();
    departmentList.init({
        saveStateUrl: 'Department/SetShowOnlyActiveDepartmentInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});