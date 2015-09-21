"use strict";
// require '../Common/ToggableInactiveList.js'

$(document).ready(function () {
    var registrationSourceCustomerList = new ToggableInactiveList();
    registrationSourceCustomerList.init({
        saveStateUrl: 'RegistrationSourceCustomer/SetShowOnlyActiveRegistrationSourceCustomerInAdmin',
        $baseContainer: $('.tab-pane').first()
    });
});