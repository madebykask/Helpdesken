'use strict';

$(function() {

    $("a.attachExistingFile").on("click", function(e) {
        e.preventDefault();
        var $src = $(this);
        var $target = $("#case_attach_ex_file_popup");
        $target.data("src", $src.data("logtype"));
        
        const isInternalLog = $src.data('logtype') === 'internalLog';
        
        var data = {
            caseId: $('#case__Id').val(),
            isInternalLog: isInternalLog === true
        };

        $.get('/Cases/GetCaseExistingFilesToAttach', $.param(data), function (res) {
            $('#attachedFiles').html(res);
            $target.modal("show");
        });

        return false;
    });

    $("#btnAttach").on("click", function (e) {
        var $popup = $("#case_attach_ex_file_popup");
        var isInternalLog = $popup.data('src') === 'internalLog';
        e.preventDefault();
        var divs = $("#table_attached_files").find("div.checkbox");
        var caseId = $("#case__Id").val();
        var selectedFiles = [];
        divs.each(function(i) {
            var logId = null;
            var checked = $(divs[i]).find("input[type=checkbox]:checked");
            var id = checked.val();
            var isCaseFile = $(divs[i]).find("#isCaseFile").val();
            var logFileType = $(divs[i]).find("#file_logType").val();
            if (isCaseFile === "False") {
                logId = $(divs[i]).find("#file_logId").val();
            }
            var name = $(divs[i]).find("span.lbl").text();
            if (id > 0) {
                selectedFiles.push({
                    Id: id,
                    FileName: name,
                    IsCaseFile: isCaseFile,
                    IsLogFile: !isCaseFile,
                    CaseId: caseId,
                    LogId: logId,
                    LogType: logFileType
                });
                checked.attr("checked", false);
            }
        });
        //send selected files
        $.ajax({
            url: "/cases/AttachExistingFile",
            type: "POST",
            data: $.param({ files: selectedFiles, caseId: caseId, isInternalLogNote: isInternalLog === true }),
            dataType: "json",
            success: function(result) {
                if (result.success) {
                    var reqData = {
                        id: $('#LogKey').val(), 
                        now: Date.now(), 
                        caseId: caseId,
                        isInternalLog: isInternalLog === true 
                    };
                    //refresh log files
                    $.get('/Cases/LogFiles', $.param(reqData), function (res) {
                        var $divOutput = isInternalLog === true ? $('div.internalLog-files') : $('div.externalLog-files');
                        $divOutput.html(res);
                        // Raise event about rendering of uploaded file
                        $(document).trigger("OnUploadedCaseLogFileRendered", []);
                        bindDeleteLogFileBehaviorToDeleteButtons(isInternalLog);
                    });
                    $("#case_attach_ex_file_popup").modal("hide");
                    return false;
                }
            }
        });
        return false;
    });

    $("a.isExisted").on("click", function (e) {
        var exists = $(this).checkUrlFileExists();
        if (!exists) {
            e.preventDefault();
            ShowToastMessage(window.parameters.fileExistError, "error");
        }
    });
});