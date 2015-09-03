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

/**
* @public
* @param { 
*       btnYesText: string,  
*       btnNoText: string,  
*       dlgText: string
*       onYesClick: fn,
*       onNoClick: fn
* } opt 
* @returns ConfirmationDialog 
*/
ConfirmationDialog.prototype.init = function (opt) {
    var me = this;
    opt = opt || {};
    me.$el = $(me.template).modal({
        "backdrop": "static",
        "keyboard": true,
        "show": false
    });
    me.$btnYes = me.$el.find("button:eq(1)");
    me.$btnNo = me.$el.find("button:eq(2)");
    me.$el.find("p:eq(0)").text(opt.dlgText);
    me.$btnYes.text(opt.btnYesText).on('click', opt.onYesClick);
    me.$btnNo.text(opt.btnNoText).on('click', opt.onNoClick);
    me.$form = me.$el.find('form');
    me.$form.attr('action', opt.dlgAction);
    
    return me;
};

ConfirmationDialog.prototype.show = function () {
    var me = this;
    me.$el.modal('show');
};
//
//ConfirmationDialog.prototype.runAction = function () {
//    var me = this;
//    me.$form.submit();
//};

ConfirmationDialog.prototype.hide = function () {
    var me = this;
    me.$el.modal('hide');
};

