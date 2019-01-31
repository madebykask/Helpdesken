'use strict';

var Utils = {
    /**
    * Builds object with fieldsToTake feilds
    * from source object srcObject
    * @param { Object } srcObject
    * @param { String[] } fieldsToTake
    * @return Object
    */
    buildParamObj: function(srcObject, fieldsToTake) {
        var res = {};
        var fieldsMap = {};
        for (var el in fieldsToTake) {
            fieldsMap[el] = true;
        }
        for (var field in srcObject) {
            if (srcObject.hasOwnProperty(field) && fieldsMap[field]) {
                res[field] = srcObject[field];
            }
        }
        return res;
    },

    /**
     * Shortcut for Object.prototype.call()
     * @param { fn } method is function to call
     * @param { Object } me is object that will act as this 
     */
    callAsMe: function(method, me) {
        return function(arg) {
            return method.call(me, arg);
        };
    },

    /**
     * Shortcut for Object.prototype.call()
     * @param { fn } method is function to call
     * @param { Object } me is object that will act as this 
     * @param { Array } args is array that will supplied to callback
     */
    applyAsMe: function(method, me, args) {
        return function() {
            return method.apply(me, args);
        }
    }
};

if (String.EMPTY === undefined) {
    String.EMPTY = '';
}

window.EMPTY_STR = String.EMPTY;
window.JOINER = EMPTY_STR;

/**
* Checks whether supplyed string empty or null
* @param { string } str
* @returns { bool }
*/
function isNullOrEmpty(str) {
    return str === null || str === EMPTY_STR;
}

function strJoin() {
    return Array.prototype.join.call(arguments, JOINER);
}

/**
* Deprecated. Use Utils.callAsMe() instead
*/
function callAsMe(method, me) {
    return Utils.callAsMe(method, me);
}

/**
* Deprecated. Use Utils.applyAsMe() instead
*/
function applyAsMe(method, me, args) {
    return Utils.applyAsMe(method, me, args);
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

function strToBoolean(value) {
    var ret = false;
    var strVal = value.toLowerCase();

    switch (strVal) {
        case "false":
        case "0":
            ret = false;
            break;
        
        case 'true':
        case '1':
            ret = true;
            break;
    }
    return ret;
}