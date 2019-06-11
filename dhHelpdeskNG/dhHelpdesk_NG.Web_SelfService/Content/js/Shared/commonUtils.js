function CommonUtils() { }

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

CommonUtils.generateRandomKey = function() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    return s4() + '-' + s4() + '-' + s4();
};
