
function CommonUtils() {}

CommonUtils.createDelayFunc = function () {
    var timer = 0;
    return function (callback, delay) {
        if (timer) {
            console.log('cancelling request!!!');
            clearTimeout(timer);
        }

        timer = window.setTimeout(function () {
            timer = null;
            console.log('now request is running!!!');
            callback();
        }, delay);
    }
};
