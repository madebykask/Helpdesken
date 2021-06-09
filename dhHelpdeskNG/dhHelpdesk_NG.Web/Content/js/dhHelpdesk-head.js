//Tooltip on startpage
$(document).ready(function () {

    $("* [rel='tooltip']").tooltip({
        html: true
    });

});

$(function () {

    // Delete dialog start 
    $(".deleteDialog").live("click", function (e) {

        e.preventDefault();

        var action = $(this).attr("href");
        var text = $(this).attr("deleteDialogText");
        var btnType = $(this).attr("buttonTypes");

        var txtDelete = "";
        var txtCancel = "";

        if (btnType == 'YesNo') {
            txtDelete = $("#DeleteDialogYesButtonText").val();
            txtCancel = $("#DeleteDialogNoButtonText").val();
        }
        else {
            txtDelete = $("#DeleteDialogDeleteButtonText").val();
            txtCancel = $("#DeleteDialogCancelButtonText").val();
        }
        
        var NewDialog = $('<div id="myModal" class="modal fade">\
                                        <div class="modal-dialog">\
                                            <form method="post" id="deleteDialogForm" class="modal-content">\
                                                <div class="modal-body">\
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>\
                                                    <p class="alert alert-info infop">\Infotext kommer här, ta ej bort</p>\
                                                </div>\
                                                <div class="modal-footer">\
                                                    <button type="button" class="btn btn-ok">Ta bort</button>\
                                                    <button type="button" class="btn btn-cancel">Avbryt</button>\
                                                </div>\
                                            </form>\
                                        </div>\
                                    </div>');

        NewDialog.on("show", function () {
            NewDialog.find("form").attr("action", action);
            NewDialog.find("p:eq(0)").text(text);
            NewDialog.find("button:eq(1)").text(txtDelete);
            NewDialog.find("button:eq(2)").text(txtCancel);

            NewDialog.find(".btn-cancel").on("click", function (e) {
                NewDialog.modal('hide');
            });

            NewDialog.find(".btn-ok").on("click", function (e) {
                $("#deleteDialogForm").submit();
                NewDialog.modal('hide');
            });
        });

        NewDialog.on("hide", function () {
            $("#myModal .btn-ok").off("click");
            $("#myModal .btn-cancel").off("click");
        });

        NewDialog.on("hidden", function () {
            $("#myModal").remove();
        });

        NewDialog.modal({
            "backdrop": "static",
            "keyboard": true,
            "show": true
        });
    });
    // Delete dialog end

});


$(function () {

    // Delete Denied dialog start 
    $(".deleteDeniedDialog").live("click", function (e) {

        e.preventDefault();

        var action = $(this).attr("href");
        var text = $(this).attr("deleteDialogText");
        var buttontext = $(this).attr("deleteDialogButtonText");
        console.log(buttontext);
        var btnType = $(this).attr("buttonTypes");

        var txtCancel = "";

        if (btnType == 'YesNo') {
            txtCancel = $("#DeleteDialogNoButtonText").val();
        }
        else {
            txtCancel = $("#DeleteDialogCancelButtonText").val();
        }

        var NewDialog = $('<div id="myModal" class="modal fade">\
                                        <div class="modal-dialog">\
                                            <form method="post" id="deleteDialogForm" class="modal-content">\
                                                <div class="modal-body">\
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>\
                                                    <p class="alert alert-info infop">\Infotext kommer här, ta ej bort</p>\
                                                </div>\
                                                <div class="modal-footer">\
                                                    <button type="button" class="btn btn-cancel">'+ buttontext+'</button>\
                                                </div>\
                                            </form>\
                                        </div>\
                                    </div>');

        NewDialog.on("show", function () {
            NewDialog.find("form").attr("action", action);
            NewDialog.find("p:eq(0)").text(text);
            NewDialog.find("button:eq(2)").text(buttontext);

            NewDialog.find(".btn-cancel").on("click", function (e) {
                NewDialog.modal('hide');
            });
        });

        NewDialog.on("hide", function () {
            $("#myModal .btn-cancel").off("click");
        });

        NewDialog.on("hidden", function () {
            $("#myModal").remove();
        });

        NewDialog.modal({
            "backdrop": "static",
            "keyboard": true,
            "show": true
        });
    });
    // Delete dialog end

});