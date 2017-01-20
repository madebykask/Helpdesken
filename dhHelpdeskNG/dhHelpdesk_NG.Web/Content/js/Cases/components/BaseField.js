'use strict';

function BaseField() {
}

/**
* @public
* @param { object } opt 
* @returns BaseField 
*/
BaseField.prototype.init = function (opt) {
    var me = this;
    opt = opt || {};
    me.$el = opt.$el;
    if (me.$el == null || me.$el.length == 0) {
        throw Error('me.$el should be JQuery element');
    }

    me.defaultValue = opt.defaultValue;

    return me;
};

/**
* @public
*/
BaseField.prototype.setValue = function (value) {
    var me = this;
    me.$el.val(value);
};

/**
* @public
*/
BaseField.prototype.getValue = function () {
    var me = this;
    return me.$el.val();
}

/**
* @public
*/
BaseField.prototype.isValueEmpty = function () {
    var me = this;
    return me.defaultValue == undefined ? window.isNullOrEmpty(me.$el.val()) : me.$el.val() === me.defaultValue;
};

/**
* @public
* Sets empty value for dropdown button group
*/
BaseField.prototype.clear = function () {
    var me = this;
    me.setValue(me.defaultValue == undefined ? window.EMTPY_STR : me.defaultValue);
};

BaseField.prototype.on = function() {
    var me = this;
    me.$el.on.apply(me.$el, arguments);
};