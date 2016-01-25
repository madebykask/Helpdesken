$(function () {
    "use strict";

    (function initErrorHandling($) {
        $(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
            if (jqXHR.status == 401) {
                alert('You are not authorized to perform this action');
                /*if (jqXHR.responseJSON) {
                    if (jqXHR.responseJSON.RedirectTo) {
                        window.location.href = jqXHR.responseJSON.RedirectTo;
                    } else {
                        var message = jqXHR.responseJSON.Message || 'You are not authorized to perform this action';
                        message = ajaxSettings.url + '\n' + message;
                        alert(message);
                    }
                } else {
                    var url = jqXHR.responseText;
                    window.location.href = url;
                }*/
            } else if (jqXHR.status == 403) {
                alert("You have no enough permissions to request this resource.");
            } else if (jqXHR.status == 0 || jqXHR.readyState == 0) { // request timeout or user leaves the page during ajax reques

            } else {
                if (jqXHR.responseJSON) {
                    if (jqXHR.responseJSON.RedirectTo) {
                        window.location.href = jqXHR.responseJSON.RedirectTo;
                    } else {
                        alert(jqXHR.responseText);
                    }
                } else {
                    try {
                        var responseJSON = $.parseJSON(jqXHR.responseText);
                        if (responseJSON.RedirectTo) {
                            window.location.href = responseJSON.RedirectTo;
                        } else if (responseJSON.guidMessage) {
                            alert(responseJSON.guidMessage);
                        } else {
                            alert(jqXHR.responseText);
                        }
                    } catch (ex) {
                        alert(jqXHR.responseText);
                    }
                }

            }
        });
    })($);
});