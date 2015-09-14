'use strict';

function ConfirmationDialog() {
    this.template = '<div class="modal fade">\
                                        <div class="modal-dialog">\
                                                <div class="modal-body">\
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>\
                                                    <p class="alert alert-info infop">\Infotext kommer här, ta ej bort</p>\
                                                </div>\
                                                <div class="modal-footer">\
                                                    <button type="button" class="btn btn-ok">Ta bort</button>\
                                                    <button type="button" class="btn btn-cancel">Avbryt</button>\
                                                </div>\
                                        </div>\
                                    </div>';
}

ConfirmationDialog.YES = 0;

ConfirmationDialog.NO = 1;

/**
* @public
* @param { 
*       btnYesText: string,  
*       btnNoText: string,  
*       dlgText: string
*       onClick: fn
* } opt 
* @returns ConfirmationDialog 
*/
ConfirmationDialog.prototype.init = function (opt) {
    var me = this;
    me.opt = opt || {};
    me.$el = $(me.template).modal({
        "backdrop": "static",
        "keyboard": true,
        "show": false
    });
    me.$btnYes = me.$el.find("button:eq(1)");
    me.$btnNo = me.$el.find("button:eq(2)");
    me.$el.find("p:eq(0)").text(me.opt.dlgText);
    me.$btnYes.text(me.opt.btnYesText).on('click', Utils.callAsMe(me.onClick, me));
    me.$btnNo.text(me.opt.btnNoText).on('click', Utils.callAsMe(me.onClick, me));
    me.$form = me.$el.find('form');
    me.$form.attr('action', me.opt.dlgAction);
    
    return me;
};

ConfirmationDialog.prototype.onClick = function(ev) {
    var me = this;
    var res = ConfirmationDialog.NO;
    ev.preventDefault();
    if (ev.target === me.$btnYes[0]) {
        res = ConfirmationDialog.YES;
    }

    if (me.opt.onClick != null) {
        me.opt.onClick(res);
    }

    me.$defferred.resolve(res);
};

/**
* @returns { jQuery.Defferred }
*/
ConfirmationDialog.prototype.show = function () {
    var me = this;
    me.$defferred = $.Deferred();
    me.$el.modal('show');
    return me.$defferred;
};

ConfirmationDialog.prototype.hide = function () {
    var me = this;
    me.$el.modal('hide');
};
