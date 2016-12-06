$(function () {
    "use strict";

    (function initErrorHandling($) {
        $(document).ajaxError(function (event, jqXhr, ajaxSettings, thrownError) {
            if (jqXhr.status == 401) {
                //alert('You are not authorized to perform this action');
                if (jqXhr.responseJSON) {
                    if (jqXhr.responseJSON.RedirectTo) {
                        window.location.href = jqXhr.responseJSON.RedirectTo;
                    } else {
                        var message = jqXhr.responseJSON.Message || "You are not authorized to perform this action";
                        message = ajaxSettings.url + "\n" + message;
                        alert(message);
                    }
                } else {
                    var url = jqXhr.responseText;
                    window.location.href = url;
                }
            } else if (jqXhr.status == 403) {
                alert("You have no enough permissions to request this resource.");
            } else if (jqXhr.status == 0 || jqXhr.readyState == 0) { // request timeout or user leaves the page during ajax reques

            } else {
                if (jqXhr.responseJSON) {
                    if (jqXhr.responseJSON.RedirectTo) {
                        window.location.href = jqXhr.responseJSON.RedirectTo;
                    } else {
                        alert(jqXhr.responseText);
                    }
                } else {
                    try {
                        var responseJson = $.parseJSON(jqXhr.responseText);
                        if (responseJson.RedirectTo) {
                            window.location.href = responseJson.RedirectTo;
                        } else if (responseJson.guidMessage) {
                            alert(responseJson.guidMessage);
                        } else {
                            alert(jqXhr.responseText);
                        }
                    } catch (ex) {
                        alert(jqXhr.responseText);
                    }
                }

            }
        });
    })($);
});