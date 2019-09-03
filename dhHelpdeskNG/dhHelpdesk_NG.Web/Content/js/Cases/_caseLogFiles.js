'use strict';
var params = dhHelpdesk.caseLog;

$(function () {
    
    var confirmDialog = function (text, onOk, onCancel, yesNo) {
        var firstText = params.yesText;
        var secondText = params.noText;

        var d = $('<div class="modal fade">' +
                        '<div class="modal-dialog">' +
                            '<form method="post" id="deleteDialogForm" class="modal-content">' +
                                '<div class="modal-body">' +
                                    '<button type="button" class="close" data-dismiss="modal">&times;</button>' +
                                    '<p class="alert alert-info infop">' + text + '</p>' +
                                '</div>' +
                                '<div class="modal-footer">' +
                                    '<button type="button" class="btn btn-ok">' + firstText + '</button>' +
                                    '<button type="button" class="btn btn-cancel">' + secondText + '</button>' +
                                '</div>' +
                            '</form>' +
                        '</div>' +
                    '</div>');

            d.on("show", function () {
                d.find(".btn-cancel").on("click", function (e) {
                    onCancel();
                    d.modal('hide');
                });

                d.find(".btn-ok").on("click", function (e) {
                    onOk();
                    d.modal('hide');
                });
            });

        d.on("hide", function () {
            d.find(".btn-ok").off("click");
            d.find(".btn-cancel").off("click");
        });

        d.on("hidden", function () {
            d.remove();
        });

        d.modal({
            "backdrop": "static",
            "keyboard": true,
            "show": true
        });
    }

    $('[data-field="deleteFile"]').click(function (e, args) {
        if (args && args.self) {
            return;
        }

        var that = $(this);

        e.stopImmediatePropagation();

        var deleteMessage = params.deleteLogFileConfirmMessage;

        var buttonName = '';
        buttonName = '#' + that[0].id;
        var fileName = $(buttonName).data('filename');
        if (fileName != undefined && fileName != null)
            fileName = fileName.toLowerCase();
        else
            fileName = "";

        var shouldShowExtra = false;
        if (fileName.indexOf(".pdf") >= 0)
            shouldShowExtra = true;
       
        if ($("#CaseHasInvoiceOrder").val() != '' && shouldShowExtra)
            deleteMessage += ' <br/> ' + params.deleteCaseFileExtraInvoiceMessage;
         
        confirmDialog(deleteMessage,
            function () {
                that.triggerHandler('click', [{ self: true }]);
            },
            function () {
            }, true);
    });
});