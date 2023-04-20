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
        var txtDelete = $(this).attr("deleteDialogDeleteButtonText");
        var txtCancel = $(this).attr("deleteDialogCancelButtonText");
        var customerId = $(this).attr("customerId");

        var NewDialog = $('<div id="myModal" class="modal fade">\
                                        <div class="modal-dialog">\
                                            <form method="post" id="deleteDialogForm" class="modal-content">\
                                                <div class="modal-body">\
                                                    <input type="hidden" value="" name="customerId">\
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

        NewDialog.on("show.bs.modal", function () {
            var btnOk = NewDialog.find("button.btn-ok");
            var btnCancel = NewDialog.find("button.btn-cancel");
            NewDialog.find("form").attr("action", action);
            NewDialog.find("p:eq(0)").text(text);
            btnOk.text(txtDelete);
            btnCancel.text(txtCancel);
            NewDialog.find("input[type='hidden']").val(customerId);

            btnCancel.on("click", function (e) {
                NewDialog.modal('hide');
            });

            btnOk.on("click", function (e) {
                $("#deleteDialogForm").submit();
                NewDialog.modal('hide');
            });
        });

        NewDialog.on("hide.bs.modal", function () {
            $("#myModal .btn-ok").off("click");
            $("#myModal .btn-cancel").off("click");
        });

        NewDialog.on("hidden.bs.modal", function () {
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

//todo: use selfservice.caseLog.saveLogMessage !!!
function SaveExternalMessage() {
    if (window.CaseId == null || window.CaseId == "")
        return;
    //this.changeState(true); TODO?
    var note = $('#myNote').val();
    if (note == "") {
        ShowToastMessage('Comment text is empty!', "warning", false);
    } else {
        $.get(SaveMessageUrl, { caseId: window.CaseId, note: note, myTime: Date.now() }, function (_CaseLogNoteMarkup) {            
            $('#CaseLogPartial').html(replaceLinebreaksInString(_CaseLogNoteMarkup));
            $('#myNote').val('');
        });
    }
}

$(document).ready(function () {
    $("* [rel='tooltip']").tooltip({
        html: true
    });

    //COMMUNICATE SCRIPT
    //todo: move to a separate script!
    $('.mapButton').click(function () {
        var panel$ = $('.siteCom');
        var mapPos = parseInt(panel$.css('left'), 10);
        if (mapPos < 0) {
            var absMapPos = Math.abs(mapPos);
            panel$.animate({
                left: '+=' + absMapPos
            }, 458, 'swing', function () {
                // Animation complete.
            });
        }
        else {
            panel$.animate({
                left: '-=' + panel$.outerWidth()
            }, 458, 'swing', function () {
                // Animation complete.
            });
        }
    });

    function setComPanelLeft() {
        var panel$ = $('.siteCom');
        panel$.css('left', -panel$.outerWidth());
    }

    setComPanelLeft();
    //END COMMUNICATE SCRIPT

    //handle link buttons with disabled state
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });

    jQuery.fn.extend({
        disable: function (state) {
            return this.each(function () {
                var $this = $(this);
                if ($this.is('input, button, textarea, select'))
                    $this.prop("disabled", state);
                else
                    $this.toggleClass('disabled', state);
            });
        }
    });
});

