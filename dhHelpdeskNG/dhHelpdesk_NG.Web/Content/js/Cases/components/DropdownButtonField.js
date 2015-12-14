'use strict';

function DropdownButtonField() {
}

/**
* @public
* @param { object } opt 
*/
DropdownButtonField.prototype.init = function (opt) {
    var me = this;
    opt = opt || {};
    me.$baseEl = opt.$el;
    me.$breadcrumbs = me.$baseEl.find('.breadcrumbs');
    me.$hiddenValue = me.$baseEl.find('input[type=hidden]');
    if (me.$baseEl == null || me.$baseEl[0] == null || me.$breadcrumbs[0] == null || me.$hiddenValue[0] == null) {
        throw Error('me.$el should be JQuery element');
    }
    me.$emptyValueEl = me.$baseEl.find('.dropdown-menu li a').first();
    me.$baseEl.find('ul.dropdown-menu li a').click(function (ev) {
        var sender = this;
        ev.preventDefault();
        me.setValueFromDomElement.call(me, sender);
    });
    return me;
};

/**
* @public
*/
DropdownButtonField.prototype.setValue = function (value) {
    var me = this;

    if (value.length >= 1)
        value = value[0];

    var el = me.$baseEl.find('.dropdown-menu a[value=' + value + ']');
    if (el.length > 0) {
        me.setValueFromDomElement(el[0]);
    } else {
        me.clear();
    }
};

DropdownButtonField.prototype.isValueEmpty = function () {
    var me = this;
    return window.isNullOrEmpty(me.$hiddenValue.val());
};

/**
* @public
* Sets empty value for dropdown button group
*/
DropdownButtonField.prototype.clear = function () {
    var me = this;
    me.setValueFromDomElement(me.$emptyValueEl);
};


/**
* @private
*/
DropdownButtonField.prototype.setValueFromDomElement = function (element) {
    var me = this;
    var val = $(element).attr('value') || window.EMPTY_STR;
    if (val == null) {
        return;
    }
    me.$hiddenValue.val(val);
    me.$breadcrumbs.text(me.getBreadcrumbs(element));
};

/**
* @private
*/
DropdownButtonField.prototype.getBreadcrumbs = function (a) {
    var path = $(a).text();
    var $parent = $(a).parents("li").eq(1).find("a:first");
    if ($parent.length == 1) {
        path = getBreadcrumbs($parent) + " - " + path;
    }

    return path;
};

