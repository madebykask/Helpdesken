'use strict';

$(function () {
    var alertMessage = window.parameters.alertMessage;
    var userSearchForm = window.parameters.userSearchForm;
    var customerList = window.parameters.customerList;
    var userStatusList = window.parameters.userStatusList;
    var loggedInUser_Customers = window.parameters.loggedInUser_CustomerList;
    var loggedInUser_Partial = window.parameters.loggedInUser_Partial;
    var loggedInUser_Url = window.parameters.loggedInUser_Url;
    
    $(document).ready(function () {
        var newLine = "<br>";
        var e = alertMessage;
        if (e != "") {
            e = replaceAll(e, "|", newLine);
            ShowToastMessage(e, "warning");
        }
    });
 
    function replaceAll (string, omit, place, prevstring) {
        if (prevstring && string === prevstring)
            return string;
        prevstring = string.replace(omit, place);
        return replaceAll(prevstring, omit, place, string)
    }

    function ShowToastMessage(message, msgType) {
        $().toastmessage('showToast', {
            text: message,
            sticky: true,
            position: 'top-center',
            type: msgType,
            closeText: '',
            stayTime: 10000,
            inEffectDuration: 1000,
            width: 700,
            close: function () {
                //console.log("toast is closed ...");
            }
        });
    }

    $(customerList).change(function () {
        $(userSearchForm).submit();
    });

    $(userStatusList).change(function () {
        $(userSearchForm).submit();
    });

    $(loggedInUser_Customers).change(function () {        
        var customerId = $(loggedInUser_Customers).val();
        
        $.get(loggedInUser_Url, {            
            selectedCustomerId: customerId,            
            curTime: new Date().getTime()
        }, function (_loggedInUsers) {
            $(loggedInUser_Partial).html(_loggedInUsers);
        });
        
    });

    var   activeTab = $.cookie('admin.users.active_tab');
    $('#myTab a[href="' + activeTab + '"]').tab('show');
    $('#myTab a').on('click', function(ev) {
        ev.preventDefault();
        $.cookie('admin.users.active_tab', $(ev.target).attr('href'));
    });
});

$(function () {
    if (!window.dhHelpdesk) {
        window.dhHelpdesk = {};
    }

    if (!window.dhHelpdesk.admin) {
        window.dhHelpdesk.admin = {};
    }

    if (!window.dhHelpdesk.admin.users) {
        window.dhHelpdesk.admin.users = {};
    }

    dhHelpdesk.admin.users.utils = {
        showMessage: function (message, type) {
            $().toastmessage('showToast', {
                text: dhHelpdesk.admin.users.utils.replaceAll(message, '|', '<br />'),
                sticky: true,
                position: 'top-center',
                type: type || 'notice',
                closeText: '',
                stayTime: 10000,
                inEffectDuration: 1000,
                width: 700
            });
        },

        showWarning: function(message) {
            dhHelpdesk.admin.users.utils.showMessage(message, 'warning');
        },

        replaceAll: function (string, omit, place, prevstring) {
            if (prevstring && string === prevstring)
                return string;
            prevstring = string.replace(omit, place);
            return dhHelpdesk.admin.users.utils.replaceAll(prevstring, omit, place, string);
        }
    }
});