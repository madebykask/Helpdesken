//----------JQuery Ajax wrappers---------
(function ($) {
    $.getAntiForgeryToken = function (tokenWindow, appPath) {
        tokenWindow = tokenWindow &&
        typeof tokenWindow === typeof window ? tokenWindow : window;
        appPath = appPath && typeof appPath === "string" ? "_" + appPath.toString() : "";
        var tokenName = "__RequestVerificationToken" + appPath;
        var inputElements = tokenWindow.document.getElementsByTagName("input");
        for (var i = 0; i < inputElements.length; i++) {
            var inputElement = inputElements[i];
            if (inputElement.type === "hidden" && inputElement.name === tokenName) {
                return {
                    name: tokenName,
                    value: inputElement.value
                };
            }
        }
    };

    $.appendAntiForgeryToken = function (data, token) {

        if (data instanceof FormData) {
            //formdata
            if (token) {
                data.append(encodeURIComponent(token.name), encodeURIComponent(token.value));
            }
            return data;
        } else if (data && typeof data !== "string") {
            //object
            if (token) {
                data[encodeURIComponent(token.name)] = encodeURIComponent(token.value);
            }
            //data = $.param(data);
            return data;
        } else {
            //string or other
            data = data ? data + "&" : "";
            // If token exists, appends {token.name}={token.value} to data.
            return token ? data + encodeURIComponent(token.name) +
                "=" + encodeURIComponent(token.value) : data;
        }
    };

    $.getAntiForgerySettings = function (settings) {
        var token = settings.token
            ? settings.token
            : $.getAntiForgeryToken(settings.tokenWindow, settings.appPath) || $.getAntiForgeryToken();
        settings.headers = settings.headers || {};
        settings.headers["X-XSRF-Token"] = token.value;

        return settings;
    };

    var originalAjax = $.ajax;
    $.ajax = function (url, settings) {
        if (typeof url === "object") {
            settings = url;;
        } else {
            settings.url = url;
        }
        var jqxhr = originalAjax(settings)
            .then(function (data) {
                if (data !== null && typeof data !== "undefined") {
                    var isError = typeof data.ErrorMessage !== "undefined";
                    if (isError) {
                        alert(data.ErrorMessage);
                        return $.Deferred().reject(jqxhr).promise();
                    }
                }
                return data;
            });

        return jqxhr;
    };

    // Wraps $.post(url, data, callback, type) for most common scenarios.
    $.postAntiForgery = function (settings) {
        settings = $.getAntiForgerySettings(settings);
        settings.type = "POST";

        return $.ajax(settings);
    };

    // Wraps $.ajax(settings).
    $.ajaxAntiForgery = function (settings) {
        settings = $.getAntiForgerySettings(settings);

        return $.ajax(settings);
    };
})(jQuery);
//----------JQuery Ajax wrappers---------