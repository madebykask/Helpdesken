'use strict';

function JQueryChosenField() {
}

/**
* @public
* @param { object } opt 
* @returns BaseField 
*/
JQueryChosenField.prototype.init = function (opt) {
    var me = this;
    opt = opt || {};
    me.$el = opt.$el;
    if (me.$el == null || me.$el.length == 0) {
        throw Error('me.$el should be JQuery element');
    }
    return me;
};

/**
* @public
*/
JQueryChosenField.prototype.setValue = function (value) {
    var me = this;
    if (value instanceof Array) {
        me.$el.val(value);
    } else {
        if (value == null) {
            me.$el.find('options:selected').prop('selected', false);
        } else {
            me.$el.val(value);
        }
    }
    me.$el.trigger('chosen:updated');
};

/**
* @public
*/
JQueryChosenField.prototype.getValue = function () {
    var me = this;
    return me.$el.val();
}

/**
* @public
*/
JQueryChosenField.prototype.isValueEmpty = function () {
    var me = this;
    var value = me.$el.val();
    return window.isNullOrEmpty(value) || value.length == 0;
};

/**
* @public
* Sets empty value for dropdown button group
*/
JQueryChosenField.prototype.clear = function () {
    var me = this;
    me.setValue([]);
};
