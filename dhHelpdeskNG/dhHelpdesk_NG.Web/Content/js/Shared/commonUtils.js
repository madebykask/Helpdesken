
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
