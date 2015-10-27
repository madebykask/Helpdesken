"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var categoryList = new ToggableInactiveList();
    categoryList.init({
        saveStateUrl: 'Category/SetShowOnlyActiveCategoriesInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});