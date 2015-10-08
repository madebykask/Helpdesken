"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var emailgroupList = new ToggableInactiveList();
    emailgroupList.init({
        saveStateUrl: 'EMailGroup/SetShowOnlyActiveEMailGroupsInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});