'use strict';

function DateField() {
}

/**
* @public
* @param { object } opt 
* @returns BaseField 
*/
DateField.prototype.init = function (opt) {
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
DateField.prototype.setValue = function (value) {
    var me = this;    

    value = value || window.EMPTY_STR;
    me.$el.datepicker('update', value);
};

/**
* @public
*/
DateField.prototype.getValue = function () {
    var me = this;
    return me.$el.val();
}

/**
* @public
*/
DateField.prototype.isValueEmpty = function () {
    var me = this;
    return window.isNullOrEmpty(me.$el.val());
};

/**
* @public
* Sets empty value for dropdown button group
*/
DateField.prototype.clear = function () {
    var me = this;
    me.setValue(window.EMTPY_STR);
};
