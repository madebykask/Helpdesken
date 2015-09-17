"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var registrationSourceCustomerListList = new ToggableInactiveList();
    registrationSourceCustomerListList.init({
        saveStateUrl: 'RegistrationSourceCustomer/SetShowOnlyActiveRegistrationSourceCustomerInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});