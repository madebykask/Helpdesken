"use strict";
/**
* This component handles show/hide inactive items in list (usual and with hierarchy)
*   by clicking on bootstrapswitch
*/
function ToggableInactiveList() { }

/**
* @param { 
*    saveStateUrl: string,
*    $baseContainer: jQueryElement
*   } opt
**/
ToggableInactiveList.prototype.init = function (opt) {
    opt = opt || {};
    var me = this;
    me.saveStateUrl = opt.saveStateUrl;
    me.$itemsHolder = opt.$baseContainer.find('table').first();
    me.$checkbox = opt.$baseContainer.find('input.switchcheckbox');
    if (me.$itemsHolder == null || me.$checkbox == null) {
        throw Error('could not find checkbox or table');
    }
    /// events binding
    me.$checkbox.on('switchChange.bootstrapSwitch', function() {
        me.onCheckboxChange.call(me);
    });
    //// initialization actions
    if (value) {
        me.$itemsHolder.find('tr.inactive').hide();
    } else {
        me.$itemsHolder.find('tr.inactive').show();
    }
};

ToggableInactiveList.prototype.onCheckboxChange = function () {
    var me = this;
    var value = me.$checkbox.prop('checked');
    var $inactiveItems;
    $.post(me.saveStateUrl, { 'value': value });
    $inactiveItems = me.$itemsHolder.find('tr.inactive');
    if (value) {
        $inactiveItems.hide(350);
    } else {
        $inactiveItems.show(100);
    }
};
