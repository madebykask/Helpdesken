'use strict';

$(function asssaaaac() {

    $("#attachExistingFile").on("click", function(e) {
                e.preventDefault();
                var $src = $(this);
                var $target = $("#case_attach_ex_file_popup");
                $target.attr("data-src", $src.attr("data-src"));
                $target.modal("show");
            });

    $("#btnAttach").on("click", function(e) {
                e.preventDefault();
                var divs = $("#table_attached_files").find("div.checkbox");
                var caseId = $("#case__Id").val();
                var selectedFiles = [];
                divs.each(function (i) {
                    var logId = null;
                    var checked = $(divs[i]).find("input[type=checkbox]:checked");
                    var id = checked.val();
                    var isCaseFile = $(divs[i]).find("#isCaseFile").val();
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
                            LogId: logId
                        });
                        checked.attr("checked", false);
                    }
                });
            $.ajax({
                url: "/cases/AttachExistingFile",
                type: "POST",
                data: { files: selectedFiles, caseId: caseId },
                dataType: "json",
                success: function(result) {
                    if (result.success) {
                        $.get('/Cases/LogFiles',
                            { id: $('#LogKey').val(), now: Date.now(), caseId: caseId },
                            function(data) {
                                $('#divCaseLogFiles').html(data);
                                // Raise event about rendering of uploaded file
                                $(document).trigger("OnUploadedCaseLogFileRendered", []);
                                bindDeleteLogFileBehaviorToDeleteButtons();
                            });
                        $("#case_attach_ex_file_popup").modal("hide");
                        return false;
                    } else {
                        ShowToastMessage("Error", "error");
                    }
                }
            });
        return false;
            });
});