
function CommonUtils() {}

CommonUtils.createDelayFunc = function () {
    var timer = 0;
    return function (callback, delay) {
        if (timer) {
            clearTimeout(timer);
        }

        timer = window.setTimeout(function () {
            timer = null;
            callback();
        }, delay);
    }
};

//returns false or IE version
CommonUtils.detectIE = function() {

    var ua = window.navigator.userAgent;

    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return { IE: true, ver: parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10) };
    }

    var trident = ua.indexOf('Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        return { IE: true, ver: parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10) };
    }

    var edge = ua.indexOf('Edge/');
    if (edge > 0) {
        // Edge (IE 12+) => return version number
        return { IE: true, ver: parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10)};
    }

    // other browser
    return { IE: false, ver: '' };
}


CommonUtils.copyToClipBoard = function copyToClipboard(elSelector) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(elSelector).text()).select();
    document.execCommand("copy");
    $temp.remove();
}