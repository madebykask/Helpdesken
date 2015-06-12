'use strict';

window.EMPTY_STR = '';
window.JOINER = EMPTY_STR;

/**
* Checks whether supplyed string empty or null
* @param { string } str
* @returns { bool }
*/
function isNullOrEmpty(str) {
    return str == null || str == EMPTY_STR;
}

function strJoin() {
    return Array.prototype.join.call(arguments, JOINER);
}

function callAsMe(method, me) {
    return function(arg) {
        return method.call(me, arg);
    };
}

function applyAsMe(method, me, args) {
    return function() {
        return method.apply(me, args);
    }
}

/**
* Helper for create and init class instances
* @param { function } _class class to create instance of
* @param { object } initOptions initizalization options
*/
function CreateInstance(_class, initOptions) {
    var res = new _class();
    return res.init(initOptions);
}

/**
* Shortcut to check empty results of JQuery.find()
*/
function is$ElEmpty($el) {
    return $el == null || $el[0] == null;
}