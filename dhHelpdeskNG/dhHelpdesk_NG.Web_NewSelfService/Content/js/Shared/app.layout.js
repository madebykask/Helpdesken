var isDirty = false;

$(function () {

    $("input[type='text']").change(function () {
        isDirty = true;
    });

    // Delete dialog start 
    $(".deleteDialog").click(function (e) {

        e.preventDefault();

        var action = $(this).attr("href");
        var text = $(this).attr("deleteDialogText");

        var NewDialog = $('<div id="myModal" class="modal fade">\
                                        <div class="modal-dialog">\
                                            <form method="post" id="deleteDialogForm" class="modal-content">\
                                                <div class="modal-body">\
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>\
                                                    <p class="alert alert-info infop">\Infotext kommer här, ta ej bort</p>\
                                                </div>\
                                                <div class="modal-footer">\
                                                    <button type="button" class="btn btn-ok">@Translation.Get("Delete", Enums.TranslationSource.TextTranslation)</button>\
                                                    <button type="button" class="btn btn-cancel">@Translation.Get("Cancel", Enums.TranslationSource.TextTranslation)</button>\
                                                </div>\
                                            </form>\
                                        </div>\
                                    </div>');

        NewDialog.on("show", function () {
            NewDialog.find("form").attr("action", action);
            NewDialog.find("p:eq(0)").text(text);

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

function ChangeLang(langId) {
    if (window.ShowLanguage == false)
        return;

    var currentPath = window.location.href;

    $.ajax({
        type: "POST",
        url: window.ChangeLanguageUrl,
        contentType: "application/json; charset=utf-8",
        data: "{ languageId:" + langId + "}",
        dataType: "html",
        success: function (result) {
            window.location.reload();
        },
        error: function (result) {
            //alert('Error in change language');
        }
    });
}

function ShowToastMessage(message, msgType, isSticky) {
    var _Sticky = false;
    if (isSticky)
        _Sticky = true;
    $().toastmessage('showToast', {
        text: message,
        sticky: _Sticky,
        position: 'top-center',
        type: msgType,
        closeText: '',
        stayTime: 4000,
        inEffectDuration: 1000,
        close: function () {
            //console.log("toast is closed ...");
        }
    });
}

function SaveExternalMessage() {
    if (window.CaseId == null || window.CaseId == "")
        return;

    var note = $('#myNote').val();
    if (note == "") {
        ShowToastMessage('Comment text is empty!', "warning", false);
    } else {
        $.get(SaveMessageUrl, { caseId: window.CaseId, note: note, myTime:Date.now() }, function (_CaseLogNoteMarkup) {
            $('#CaseLogPartial').html(_CaseLogNoteMarkup);
            $('#myNote').val('');
        });
    }
}

$(document).ready(function () {
    $("* [rel='tooltip']").tooltip({
        html: true
    });

    //COMMUNICATE SCRIPT
    $('.mapButton').click(function () {
        var mapPos = parseInt($('.siteCom').css('left'), 10);
        if (mapPos < 0) {
            $('.siteCom').animate({
                left: '+=600'
            }, 458, 'swing', function () {
                // Animation complete.
            });
        }
        else {
            $('.siteCom').animate({
                left: '-=600'
            }, 458, 'swing', function () {
                // Animation complete.
            });
        }
    });
    //END COMMUNICATE SCRIPT
});

