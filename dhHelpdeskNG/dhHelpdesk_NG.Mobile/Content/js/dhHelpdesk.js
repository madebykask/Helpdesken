﻿var dhHelpdesk = {};


//Start FAQ Acordion

//tabbar
$('.nav-tabs li:not(.disabled) a').click(function (e) {
    e.preventDefault();
    $(this).tab('show');
});

$(".nav-tabs-actions a").unbind("click");

$(".content input:text, .content textarea").eq(0).focus()


function ShowToastMessage(message, msgType) {
    $().toastmessage('showToast', {
        text: message,
        sticky: false,
        position: 'top-center',
        type: msgType,
        closeText: '',
        stayTime: 3000,
        inEffectDuration: 1000,
        close: function () {
            //console.log("toast is closed ...");
        }
    });
}

//Hämtar vald text från droptodwn button
function getBreadcrumbs(a) {
    var path = $(a).text(), $parent = $(a).parents("li").eq(1).find("a:first");

    if ($parent.length == 1) {
        path = getBreadcrumbs($parent) + " - " + path;
    }
    return path;
}

function today() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } var today = yyyy + '-' + mm + '-' + dd;
    return today;
}

function getNow() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var hh = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } if (hh < 10) { hh = '0' + hh }
    if (m < 10) { m = '0' + m } if (s < 10) { s = '0' + s }
    var today = yyyy + '-' + mm + '-' + dd + ' ' + hh + '.' + m + '.' + s;
    return today;
}

// Cose window or tab
function close_window() {
    //if (confirm("WARNING TEXT XXXXXXX TRANSLATE")) {
    close();
    //}
}

function SelectValueInOtherDropdownOnChange(id, postTo, ctl) {
    var ctlOption = ctl + ' option';
    $.post(postTo, { 'id': id }, function (data) {
        if (data != null) {
            var exists = $(ctlOption + '[value=' + data + ']').length;
            if (exists > 0) {
                $(ctl).val(data);
            }
        }
    }, 'json');
}

function CaseCascadingSelectlistChange(id, customerId, postTo, ctl, departmentFilterFormat) {
    var ctlOption = ctl + ' option';
    $.post(postTo, { 'id': id, 'customerId': customerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {
        $(ctlOption).remove();
        $(ctl).append('<option value="">&nbsp;</option>');
        if (data != undefined) {
            for (var i = 0; i < data.list.length; i++) {
                $(ctl).append('<option value="' + data.list[i].id + '">' + data.list[i].name + '</option>');
            }
        }
    }, 'json');
}


function CaseWriteTextToLogNote(value) {
    $('#WriteTextToExternalNote').val(value);
}

function EditCaseAndClose() {
    $("#target").attr("action", '/Cases/EditAndClose');
    $("#target").submit();
}

function EditCaseAndAddCase() {
    $("#target").attr("action", '/Cases/EditAndAddCase');
    $("#target").submit();
}

function NewCaseAndClose() {
    $("#target").attr("action", '/Cases/NewAndClose');
    $("#target").submit();
}

function NewCaseAndAddCase() {
    $("#target").attr("action", '/Cases/NewAndAddCase');
    $("#target").submit();
}

var allFileNames = [];

function FAQInitForm() {
    var _plupload;
    var getFAQFiles = function () {
        $.get('/FAQ/Files', { faqId: $('#FAQKey').val(), now: Date.now() }, function (data) {
            $('#divFAQFiles').html(data);
            bindDeleteFAQFileBehaviorToDeleteButtons();
        });
    };
    $('#upload_FAQfiles_popup').on('hide', function () {

        if (_plupload != undefined) {
            if (_plupload.pluploadQueue().files.length > 0) {
                if (_plupload.pluploadQueue().state == plupload.UPLOADING) {
                    _plupload.pluploadQueue().stop();
                    for (var i = 0; i < _plupload.pluploadQueue().files.length; i++) {
                        _plupload.pluploadQueue().removeFile(_plupload.pluploadQueue().files[i]);
                    }
                    _plupload.pluploadQueue().refresh();
                }
            }
        }        
    });

    PluploadTranslation($('#CurLanguageId').val());

    $('#upload_FAQfiles_popup').on('show', function () {
        _plupload = $('#FAQfile_uploader').pluploadQueue({
            runtimes: 'html5,html4',
            url: '/FAQ/UploadFile',

            multipart_params: { faqId: $('#FAQKey').val() },
            filters: {
                max_file_size: '10mb',
            },
            buttons: { browse: true, start: true, stop: true, cancel: true },
            preinit: {
                Init: function (up, info) {
                    //console.log('1:init', info);                    
                },


                UploadFile: function (up, file) {

                    var strFiles = $('#FAQFileNames').val();
                    if (strFiles != undefined) {
                        var allFileNames = strFiles.split('|');

                        var fn = file.name;

                        for (var i = 0; i < allFileNames.length; i++) {
                            if (fn == allFileNames[i]) {
                                var findName = false;
                                for (var j = 1; j < 100 && !findName; j++) {
                                    findName = true;
                                    for (var k = 0; k < allFileNames.length; k++) {
                                        if (j.toString() + '-' + fn == allFileNames[k])
                                            findName = false;
                                    }

                                    if (findName) {
                                        var d = getNow().toString();
                                        file.name = d + '-' + fn;
                                    }
                                } // for j
                            }
                        } // for i
                    }
                    $('#FAQFileNames').val(strFiles + "|" + file.name);
                },

                UploadComplete: function (up, file) {

                    //console.log('3:uploaded complete',file.name);
                    //plupload_add
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");
                    up.refresh();
                }
            },
            init: {
                FileUploaded: function () {
                    getFAQFiles();
                },

                Error: function (uploader, e) {
                    if (e.status != 409) {
                        return;
                    }
                },

                StateChanged: function (uploader) {
                    if (uploader.state != plupload.STOPPED) {
                        return;
                    }
                    uploader.refresh();
                }
            }
        });
    });
    
    
    bindDeleteFAQFileBehaviorToDeleteButtons();

}
function CaseInitForm() {

    $('#CaseLog_TextExternal').focus(function () {
        CaseWriteTextToLogNote('');
    });

    $('#CaseLog_TextInternal').focus(function () {
        CaseWriteTextToLogNote('internal');
    });

    $('#case__ReportedBy').typeahead(GetComputerUserSearchOptions());
    $('#case__InventoryNumber').typeahead(GetComputerSearchOptions());

    $('#CountryId').change(function () {
        CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeCountry/', '#case__Supplier_Id', $('#DepartmentFilterFormat').val());
    });

    $('#case__Department_Id').change(function () {
        CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeDepartment/', '#case__Ou_Id', $('#DepartmentFilterFormat').val());
        $('#divInvoice').hide();
        $.get('/Cases/ShowInvoiceFields/', { 'departmentId': $(this).val() }, function (data) {
            if (data == 1) {
                $('#divInvoice').show();
            }
        }, 'json');
    });

    $('#case__Region_Id').change(function () {
        CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeRegion/', '#case__Department_Id', $('#DepartmentFilterFormat').val());
    });

    $('#case__Status_Id').change(function () {
        if ($(this).val() > 0) {
            $.post('/Cases/ChangeStatus/', { 'id': $(this).val() }, function (data) {
                if (data != undefined) {
                    var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
                    if (exists > 0 && data.WorkingGroup_Id > 0) {
                        $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
                    }
                    exists = $('#case__StateSecondary_Id option[value=' + data.StateSecondary_Id + ']').length;
                    if (exists > 0 && data.StateSecondary_Id > 0) {
                        $("#case__StateSecondary_Id").val(data.StateSecondary_Id);
                    }
                }
            }, 'json');
        }

    });

    $('#case__CaseType_Id').change(function () {
        SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeCaseType/', '#case__Performer_User_Id');
    });

    $('#case__Priority_Id').change(function () {
        $.post('/Cases/ChangePriority/', { 'id': $(this).val() }, function (data) {
            if (data.ExternalLogText != null) {
                $('#CaseLog_TextExternal').val(data.ExternalLogText);
            }
        }, 'json');
    });

    $('#case__System_Id').change(function () {
        SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeSystem/', '#case__Urgency_Id');
        SetPriority();
    });

    $('#case__Impact_Id').change(function () {
        SetPriority();
    });

    $('#case__Urgency_Id').change(function () {
        SetPriority();
    });

    $('#case__ProductArea_Id').change(function () {
        if ($(this).val() > 0) {
            $.post('/Cases/ChangeProductArea/', { 'id': $(this).val() }, function (data) {
                //alert(JSON.stringify(data));
                if (data != undefined) {
                    //debugger
                    var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
                    if (exists > 0 && data.WorkingGroup_Id > 0) {
                        $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
                    }
                    exists = $('#case__Priority_Id option[value=' + data.Priority_Id + ']').length;
                    if (exists > 0 && data.Priority_Id > 0) {
                        $("#case__Priority_Id").val(data.Priority_Id);
                    }
                }
            }, 'json');
        }
    });

    $('#case__WorkingGroup_Id').change(function () {
        // filter administrators
        var DontConnectUserToWorkingGroup = $('#CaseMailSetting_DontConnectUserToWorkingGroup').val();
        if (DontConnectUserToWorkingGroup == 0) {
            CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeWorkingGroupFilterUser/', '#case__Performer_User_Id', $('#DepartmentFilterFormat').val());
        }
        //set state secondery
        SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeWorkingGroupSetStateSecondary/', '#case__StateSecondary_Id')
    });

    $('#case__StateSecondary_Id').change(function () {
        $('#CaseLog_SendMailAboutCaseToNotifier').removeAttr('disabled');
        $.post('/Cases/ChangeStateSecondary', { 'id': $(this).val() }, function (data) {
            // disable send mail checkbox
            if (data.NoMailToNotifier == 1) {
                $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', false);
                $('#CaseLog_SendMailAboutCaseToNotifier').attr('disabled', true);
            }
            // set workinggroup id
            var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
            if (exists > 0 && data.WorkingGroup_Id > 0) {
                //alert(data.WorkingGroup_Id);
                $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
            }
        }, 'json');
    });

    $('#lstStandarTexts').change(function () {
        var regexp = /<BR>/g
        var txt = $('#lstStandarTexts :selected').text().replace(regexp, "\n");
        var writeTextToExternalNote = $('#WriteTextToExternalNote').val();
        var field = '#CaseLog_TextInternal';

        if (writeTextToExternalNote == '') {
            field = '#CaseLog_TextExternal';
        }

        if (txt.length > 1) {
            $(field).val($(field).val() + txt);
            $(field).focus();

            var input = $(field);
            input[0].selectionStart = input[0].selectionEnd = input.val().length;
        }
    });

    $('#divCaseType ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_CaseType").text(getBreadcrumbs(this));
        $("#case__CaseType_Id").val(val).trigger('change');
    });

    $('#divOU ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_OU").text(getBreadcrumbs(this));
        $("#case__Ou_Id").val(val).trigger('change');
    });

    $('#divProductArea ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
        $("#case__ProductArea_Id").val(val).trigger('change');;
    });

    $('#AddNotifier').click(function (e) {
        e.preventDefault();


        var params = "?def=1"; // this is Not applied in form 
        
        if ($("#case__ReportedBy").val() != '') 
            params += "&userId=" + $("#case__ReportedBy").val();
        if ($("#case__PersonsName").val() != '')
            params += "&fName=" + $("#case__PersonsName").val();
        if ($("#case__PersonsEmail").val() != '')
            params += "&email=" + $("#case__PersonsEmail").val();
        if ($("#case__PersonsPhone").val() != '')
            params += "&phone=" + $("#case__PersonsPhone").val();
        if ($("#case__Place").val() != '')
            params += "&placement=" + $("#case__Place").val();
        
        var win = window.open('/Notifiers/NewNotifierPopup' + params, '_blank', 'left=100,top=100,width=990,height=480,toolbar=0,resizable=1,menubar=0,status=0,scrollbars=1');
        //win.onbeforeunload = function () { CaseNewNotifierEvent(win.returnValue); }
        //$(win).on('beforeunload', function () { CaseNewNotifierEvent(win.returnValue); });
    });

    $('#AddFAQ').click(function (e) {
        e.preventDefault();
        
        var question = $('#case__Caption').val();
        var answer = $('#CaseLog_TextExternal').val();
        var internalanswer = $('#CaseLog_TextInternal').val();
        var win = window.open('/Faq/NewFAQPopup?question=' + question + '&answer=' + answer + '&internalanswer=' + internalanswer, '_blank', 'left=100,top=100,width=700,height=700,toolbar=0,resizable=1,menubar=0,status=0,scrollbars=1');
    });

    if (!Date.now) {
        Date.now = function () { return new Date().getTime(); };
    }

    var getCaseFiles = function () {
        $.get('/Cases/Files', { id: $('#CaseKey').val(), now: Date.now() }, function (data) {
            $('#divCaseFiles').html(data);
            bindDeleteCaseFileBehaviorToDeleteButtons();
        });
    };

    //var GetCurrentFiles() {
    //   //
        
    //};

    var getLogFiles = function () {
        $.get('/Cases/LogFiles', { id: $('#LogKey').val(), now: Date.now() }, function (data) {
            $('#divCaseLogFiles').html(data);
            bindDeleteLogFileBehaviorToDeleteButtons();
        });
    };

    var _plupload;

    $('#upload_files_popup, #upload_logfiles_popup').on('hide', function () {

        if (_plupload != undefined) {
            if (_plupload.pluploadQueue().files.length > 0) {
                if (_plupload.pluploadQueue().state == plupload.UPLOADING) {
                    _plupload.pluploadQueue().stop();
                    for (var i = 0; i < _plupload.pluploadQueue().files.length; i++) {
                        _plupload.pluploadQueue().removeFile(_plupload.pluploadQueue().files[i]);
                    }
                    _plupload.pluploadQueue().refresh();
                }
            }
        }
    });

    PluploadTranslation($('#CurLanguageId').val());    

    var newFileName = "";
    $('#upload_files_popup').on('show', function () {
        _plupload = $('#file_uploader').pluploadQueue({
            runtimes: 'html5,html4',
            url: '/Cases/UploadCaseFile',
            multipart_params: { id: $('#CaseKey').val() },
            filters: {
                max_file_size: '30mb',
            },
            buttons: { browse: true, start: true, stop: true, cancel: true },
            preinit: {
                Init: function (up, info) {
                    
                    //console.log('1:init', info);                    
                },

                
                UploadFile: function (up, file) {                                        
                    var strFiles = $('#CaseFileNames').val();
                    var allFileNames = strFiles.split('|');

                    var fn = file.name;

                    for (var i = 0; i < allFileNames.length; i++) {
                        if (fn == allFileNames[i]) {
                            var findName = false;
                            for (var j = 1; j < 100 && !findName; j++) {
                                findName = true;
                                for (var k = 0; k < allFileNames.length; k++) {
                                    if (j.toString() + '-' + fn == allFileNames[k])
                                        findName = false;
                                }

                                if (findName) {                                    
                                    var d = getNow().toString();
                                    file.name = d + '-' + fn;                                        
                                }
                            } // for j
                        }
                    } // for i
                                            
                    $('#CaseFileNames').val(strFiles + "|" + file.name);                                       
                },

                UploadComplete: function (up, file) {

                    //console.log('3:uploaded complete',file.name);
                    //plupload_add
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");                    
                    up.refresh();

                    // Raise event about uploaded file
                    $(document).trigger("OnUploadCaseFile", [up, file]);
                }
            },
            init: {
                FileUploaded: function () {                   
                    getCaseFiles();
                },

                Error: function (uploader, e) {
                    if (e.status != 409) {
                        return;
                    }
                },

                StateChanged: function (uploader) {
                    if (uploader.state != plupload.STOPPED) {
                        return;
                    }
                    uploader.refresh();
                }
            }
        });
    });

    PluploadTranslation($('#CurLanguageId').val());

    $('#upload_logfiles_popup').on('show', function () {
        _plupload = $('#logfile_uploader').pluploadQueue({
            runtimes: 'html5,html4',
            url: '/Cases/UploadLogFile',
            
            multipart_params: { id: $('#LogKey').val() },
            filters: {                
                max_file_size: '30mb',
            },
            buttons: { browse: true, start: true, stop: true, cancel: true },
            preinit: {
                Init: function (up, info) {
                    //log('[Init]', 'Info:', info, 'Features:', up.features);                                        
                },

                UploadFile: function (up, file) {
                    //log('[UploadFile]', file);
                    var strFiles = $('#LogFileNames').val();
                    var allFileNames = strFiles.split('|');

                    var fn = file.name;

                    for (var i = 0; i < allFileNames.length; i++) {
                        if (fn == allFileNames[i]) {
                            var findName = false;
                            for (var j = 1; j < 100 && !findName; j++) {
                                findName = true;
                                for (var k = 0; k < allFileNames.length; k++) {
                                    if (j.toString() + '-' + fn == allFileNames[k])
                                        findName = false;
                                }

                                if (findName) {
                                    var d = getNow().toString();
                                    file.name = d + '-' + fn;
                                }
                            } // for j
                        }
                    } // for i

                    $('#LogFileNames').val(strFiles + "|" + file.name);
                },

                UploadComplete: function (up, file) {
                    //plupload_add
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");                    
                    up.refresh();
                }
            },
            init: {
                FileUploaded: function () {                    
                    getLogFiles();
                },

                Error: function (uploader, e) {
                    if (e.status != 409) {
                        return;
                    }
                },

                StateChanged: function (uploader) {
                    if (uploader.state != plupload.STOPPED) {
                        return;
                    }
                    uploader.refresh();
                }
            }
        });
    });

    LogInitForm();
    bindDeleteCaseFileBehaviorToDeleteButtons();
    SetFocusToReportedByOnCase();
}

function SetFocusToReportedByOnCase() {
    if ($('#ShowReportedBy').val() == 1) {
        $('#case__ReportedBy').focus();
    }
}

function SendToDialogCaseCallback(email) {
    if (email.length > 0) {

        var str = '';
        for (var i = 0; i < email.length; i++) {
            str += email[i] + '\r\n';
        }
        $('#divEmailRecepientsInternalLog').show();
        $('#CaseLog_EmailRecepientsInternalLog').val(str);
    }
}

function LogInitForm() {

    $('#divFinishingType ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var value = $(this).attr('value');
        $("#divBreadcrumbs_FinishingType").text(getBreadcrumbs(this));
        $("#CaseLog_FinishingType").val(value);
        if (value == '' || value === undefined) {
            $("#CaseLog_FinishingDate").val('');
        }
        else {
            if ($("#CaseLog_FinishingDate").val() == '') {
                var today = $("#Today").val();
                $("#CaseLog_FinishingDate").val(today);
            }
        }
    });

    $("#CaseLog_SendMailAboutLog").change(function () {
        $('#divEmailRecepientsInternalLog').hide();
        if (this.checked) {
            $("#divSendToDialogCase").dialog("option", "width", 450);
            $("#divSendToDialogCase").dialog("option", "height", 550);
            $("#divSendToDialogCase").dialog("open");
        }
    });

    $('#CaseLog_TextExternal').bind('input propertychange', function () {
        var informNotifier = $('#CaseLog_SendMailAboutCaseToNotifier');
        var isInformNotifierBehavior = informNotifier.attr("InformNotifierBehavior");
        if (isInformNotifierBehavior == "false") {
            return;
        }

        informNotifier.removeAttr('checked');
        if (this.value.length) {
            $('#CaseLog_SendMailAboutCaseToNotifier:not(:disabled)').attr('checked', 'checked');
        }
    });

    bindDeleteLogFileBehaviorToDeleteButtons();
}

function GetComputerUserSearchOptions() {

    var options = {
        items: 20,
        minLength: 2,

        source: function (query, process) {
            return $.ajax({
                url: '/cases/search_user',
                type: 'post',
                data: { query: query, customerId: $('#case__Customer_Id').val() },
                dataType: 'json',
                success: function (result) {
                    var resultList = jQuery.map(result, function (item) {
                        var aItem = {
                            id: item.Id
                                    , num: item.UserId
                                    , name: item.SurName + ' ' + item.FirstName
                                    , email: item.Email
                                    , place: item.Location
                                    , phone: item.Phone
                                    , usercode: item.UserCode
                                    , cellphone: item.CellPhone
                                    , regionid: item.Region_Id
                                    , departmentid: item.Department_Id
                                    , ouid: item.OU_Id
                        };
                        return JSON.stringify(aItem);
                    });

                    return process(resultList);
                }
            });
        },

        matcher: function (obj) {
            var item = JSON.parse(obj);
            //console.log(JSON.stringify(item));
            return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.email.toLowerCase().indexOf(this.query.toLowerCase());
        },

        sorter: function (items) {
            var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
            while (aItem = items.shift()) {
                var item = JSON.parse(aItem);
                if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                else caseInsensitive.push(JSON.stringify(item));
            }

            return beginswith.concat(caseSensitive, caseInsensitive);
        },

        highlighter: function (obj) {
            var item = JSON.parse(obj);
            var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
            var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email;

            return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                return '<strong>' + match + '</strong>';
            });
        },

        updater: function (obj) {
            var item = JSON.parse(obj);
            //console.log(JSON.stringify(item));
            $('#case__ReportedBy').val(item.num);
            $('#case__PersonsName').val(item.name);
            $('#case__PersonsEmail').val(item.email);
            $('#case__PersonsPhone').val(item.phone);
            $('#case__PersonsCellphone').val(item.cellphone);
            $('#case__Place').val(item.place);
            $('#case__UserCode').val(item.usercode);
            $('#case__Region_Id').val(item.regionid);
            $('#case__Department_Id').val(item.departmentid);
            $('#case__Ou_Id').val(item.ouid);

            return item.num;
        }
    };

    return options;
}

function GetComputerSearchOptions() {

    var options = {
        items: 20,
        minLength: 2,

        source: function (query, process) {
            return $.ajax({
                url: '/cases/search_computer',
                type: 'post',
                data: { query: query, customerId: $('#case__Customer_Id').val() },
                dataType: 'json',
                success: function (result) {
                    var resultList = jQuery.map(result, function (item) {
                        var aItem = {
                                id: item.Id
                                , num: item.ComputerName
                                , location: item.Location
                                , computertype: item.ComputerTypeDescription 
                        };
                        return JSON.stringify(aItem);
                    });

                    return process(resultList);
                }
            });
        },

        matcher: function (obj) {
            var item = JSON.parse(obj);
            //console.log(JSON.stringify(item));
            return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.computertype.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.location.toLowerCase().indexOf(this.query.toLowerCase());
        },

        sorter: function (items) {
            var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
            while (aItem = items.shift()) {
                var item = JSON.parse(aItem);
                if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                else caseInsensitive.push(JSON.stringify(item));
            }

            return beginswith.concat(caseSensitive, caseInsensitive);
        },

        highlighter: function (obj) {
            var item = JSON.parse(obj);
            var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
            var result = item.num + ' - ' + item.location + ' - ' + (item.computertype == null ? ' ' : item.computertype);

            return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                return '<strong>' + match + '</strong>';
            });
        },

        updater: function (obj) {
            var item = JSON.parse(obj);
            //console.log(JSON.stringify(item));
            $('#case__InventoryNumber').val(item.num);
            $('#case__InventoryType').val(item.computertype);
            $('#case__InventoryLocation').val(item.location);

            return item.num;
        }
    };

    return options;
}

//multiselct med sök
$('.multiselect').multiselect({
    enableFiltering: true,
    filterPlaceholder: '',
    maxHeight: 250,
    //maxHeight: false,
    buttonClass: 'btn',
    buttonWidth: '220px',
    buttonContainer: '<span class="btn-group" />',
    buttonText: function (options) {
        if (options.length == 0) {
            return '-- <i class="caret"></i>';
        }
        else if (options.length > 2) {
            return options.length + ' selected  <i class="caret"></i>';
        }
        else {
            var selected = '';
            options.each(function () {
                selected += $(this).text() + ', ';
            });
            return selected.substr(0, selected.length - 2) + ' <i class="caret"></i>';
        }
    }
});

function PluploadTranslation(languageId) {
    if (languageId == 1)
    {        
        plupload.addI18n({
            'Select files': 'Välj filer',
            'Add files to the upload queue and click the start button.': 'Lägg till filer till kön och tryck på start.',
            'Filename': 'Filnamn',
            'Status': 'Status',
            'Size': 'Storlek',
            'Add files': 'Lägg till filer',
            'Add files.': 'nnnnn',
            'Start upload': 'ssss',            
            'Stop current upload': 'Stoppa uppladdningen',
            'Start uploading queue': 'Starta uppladdningen',
            'Drag files here.': 'Dra filer hit'
            });
    }

    if (languageId == 2)
    {
            plupload.addI18n({
                'Select files': 'Select files',
                'Add files to the upload queue and click the start button.': 'Add files to the upload queue and click the start button.',
                'Filename': 'Filename',
                'Status': 'Status',
                'Size': 'Size',
                'Add files': 'Add files',
                'Stop current upload': 'Stop current upload',
                'Start uploading queue': 'Start uploading queue',
                'Drag files here.': 'Drag files here.'
            });     
    }

}
function bindDeleteFAQFileBehaviorToDeleteButtons() {
    $('#faq_files_table a[id^="delete_faqfile_button_"]').click(function () {
        var key = $('#FAQKey').val();
        var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
        var pressedDeleteFileButton = this;
        $.post("/FAQ/DeleteFile", { faqId: key, fileName: fileName }, function () {
            $(pressedDeleteFileButton).parents('tr:first').remove();
            var fileNames = $('#FAQFileNames').val();
            fileNames = fileNames.replace("|" + fileName.trim(), "");
            fileNames = fileNames.replace(fileName.trim() + "|", "");
            $('#FAQFileNames').val(fileNames);
        });
    });
}
function bindDeleteCaseFileBehaviorToDeleteButtons() {
    $('#case_files_table a[id^="delete_casefile_button_"]').click(function () {
        var key = $('#CaseKey').val();
        var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
        var pressedDeleteFileButton = this;
        $.post("/Cases/DeleteCaseFile", { id: key, fileName: fileName }, function () {
            $(pressedDeleteFileButton).parents('tr:first').remove();
            var fileNames = $('#CaseFileNames').val();            
            fileNames = fileNames.replace("|" + fileName.trim(), "");
            fileNames = fileNames.replace(fileName.trim() + "|", "");            
            $('#CaseFileNames').val(fileNames);

            // Raise event about deleted file
            $(document).trigger("OnDeleteCaseFile", [key, fileName]);
        });
    });
}

function bindDeleteLogFileBehaviorToDeleteButtons() {
    $('#log_files_table a[id^="delete_logfile_button_"]').click(function () {
        var key = $('#LogKey').val();
        var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
        var pressedDeleteFileButton = this;

        $.post("/Cases/DeleteLogFile", { id: key, fileName: fileName }, function () {
            $(pressedDeleteFileButton).parents('tr:first').remove();
            var fileNames = $('#LogFileNames').val();
            fileNames = fileNames.replace("|" + fileName.trim(), "");
            fileNames = fileNames.replace(fileName.trim() + "|", "");
            $('#LogFileNames').val(fileNames);
        });
    });
}

function SetPriority() {
    var impactId = $('#case__Impact_Id').val();
    var urgencyId = $('#case__Urgency_Id').val();

    if (urgencyId > 0 && impactId > 0) {
        $.post('/Cases/GetPriorityIdForImpactAndUrgency', { 'impactId': impactId, 'urgencyId': urgencyId }, function (data) {
            if (data != null) {
                var exists = $('#case__Priority_Id Option[value=' + data + ']').length;
                if (exists > 0) {
                    $('#case__Priority_Id').val(data);
                }
            }
        }, 'json');
    }
}

function NewNotifierEvent(id) {
    $.post('/Cases/Get_User', { 'Id': id }, function (data) {
        if (data != undefined) {
            $('#case__ReportedBy').val(data.num);
            $('#case__PersonsName').val(data.name);
            $('#case__PersonsEmail').val(data.email);
            $('#case__PersonsPhone').val(data.phone);
            $('#case__PersonsCellphone').val(data.cellphone);
            $('#case__Place').val(data.place);
            $('#case__UserCode').val(data.usercode);
            $('#case__Region_Id').val(data.regionid);
            $('#case__Department_Id').val(data.departmentid);
            $('#case__Ou_Id').val(data.ouid);
        }
    }, 'json');
}

function moveCase(id) {
    var customerId = $('#moveCaseToCustomerId').val();
    if (customerId.length > 0) {
        var url = '/cases/edit/' + id + '?moveToCustomerId=' + customerId;
        window.location.href = url;
    }
}

function copyCase(id, customerId) {
   
        var url = '/cases/copy/' + id + '?customerId=' + customerId;
        window.location.href = url;
    
}

// calculate utc time
$(function(){
    $('[data-datetimeutc]', 'body').each(function () {
        // the date construct will automatically convert to local time
        var localDate = new Date(parseInt($(this).attr('data-datetimeutc')));
        $(this).html(localDate.toLocaleDateString() + ' ' + localDate.toLocaleTimeString());
    });
});

$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
};

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
};


 //TABLE PAGING
$.extend($.fn.dataTable.defaults, {
    "searching": false,
    "ordering": false
});

/* Default class modification */
$.extend($.fn.dataTableExt.oStdClasses, {
    "sWrapper": "dataTables_wrapper form-inline"
});

/* API method to get paging information */
$.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
    return {
        "iStart": oSettings._iDisplayStart,
        "iEnd": oSettings.fnDisplayEnd(),
        "iLength": oSettings._iDisplayLength,
        "iTotal": oSettings.fnRecordsTotal(),
        "iFilteredTotal": oSettings.fnRecordsDisplay(),
        "iPage": oSettings._iDisplayLength === -1 ?
            0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
        "iTotalPages": oSettings._iDisplayLength === -1 ?
            0 : Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
    };
};

/* Bootstrap style pagination control */
$.extend($.fn.dataTableExt.oPagination, {
    "bootstrap": {
        "fnInit": function (oSettings, nPaging, fnDraw) {
            var oLang = oSettings.oLanguage.oPaginate;
            var fnClickHandler = function (e) {
                e.preventDefault();
                if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                    fnDraw(oSettings);
                }
            };

            $(nPaging).addClass('pagination').append(
                '<ul>' +
                    '<li class="prev disabled"><a href="#">&larr; ' + oLang.sPrevious + '</a></li>' +
                    '<li class="next disabled"><a href="#">' + oLang.sNext + ' &rarr; </a></li>' +
                '</ul>'
            );
            var els = $('a', nPaging);
            $(els[0]).bind('click.DT', { action: "previous" }, fnClickHandler);
            $(els[1]).bind('click.DT', { action: "next" }, fnClickHandler);
        },

        "fnUpdate": function (oSettings, fnDraw) {
            var iListLength = 5;
            var oPaging = oSettings.oInstance.fnPagingInfo();
            var an = oSettings.aanFeatures.p;
            var i, ien, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

            if (oPaging.iTotalPages < iListLength) {
                iStart = 1;
                iEnd = oPaging.iTotalPages;
            }
            else if (oPaging.iPage <= iHalf) {
                iStart = 1;
                iEnd = iListLength;
            } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                iStart = oPaging.iTotalPages - iListLength + 1;
                iEnd = oPaging.iTotalPages;
            } else {
                iStart = oPaging.iPage - iHalf + 1;
                iEnd = iStart + iListLength - 1;
            }

            for (i = 0, ien = an.length ; i < ien ; i++) {
                // Remove the middle elements
                $('li:gt(0)', an[i]).filter(':not(:last)').remove();

                // Add the new list items and their event handlers
                for (j = iStart ; j <= iEnd ; j++) {
                    sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                    $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
                        .insertBefore($('li:last', an[i])[0])
                        .bind('click', function (e) {
                            e.preventDefault();
                            oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
                            fnDraw(oSettings);
                        });
                }

                // Add / remove disabled classes from the static elements
                if (oPaging.iPage === 0) {
                    $('li:first', an[i]).addClass('disabled');
                } else {
                    $('li:first', an[i]).removeClass('disabled');
                }

                if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                    $('li:last', an[i]).addClass('disabled');
                } else {
                    $('li:last', an[i]).removeClass('disabled');
                }
            }
        }
    }
});

/*
 * TableTools Bootstrap compatibility
 * Required TableTools 2.1+
 */
if ($.fn.DataTable.TableTools) {
    // Set the classes that TableTools uses to something suitable for Bootstrap
    $.extend(true, $.fn.DataTable.TableTools.classes, {
        "container": "DTTT btn-group",
        "buttons": {
            "normal": "btn",
            "disabled": "disabled"
        },
        "collection": {
            "container": "DTTT_dropdown dropdown-menu",
            "buttons": {
                "normal": "",
                "disabled": "disabled"
            }
        },
        "select": {
            "row": "active"
        }
    });

    // Have the collection use a bootstrap compatible dropdown
    $.extend(true, $.fn.DataTable.TableTools.DEFAULTS.oTags, {
        "collection": {
            "container": "ul",
            "button": "li",
            "liner": "a"
        }
    });
}



/* Table initialisation */
function ResetDataTable(tableUniqId) {
    //alert('reset: ' + tableUniqId);
    var oTable = $('#' + tableUniqId).dataTable();
    oTable.fnPageChange('first');
};

function DestroyDataTable(tableUniqId) {
    var oTable = $('#' + tableUniqId).dataTable();
    oTable.destroy();
};

function InitDataTable(tableUniqId, perText, showingText) {
    //alert('init: ' + tableUniqId);

    $('#' + tableUniqId).dataTable({
        stateSave: true,
        //stateDuration: 10,
        "sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "oLanguage": {
            "sLengthMenu": "_MENU_ " + perText ,
            "sInfo": showingText + " _PAGE_ / _PAGES_",
            "oPaginate": {
                "sFirst": "First",
                "sLast": "Last",
                "sNext": "",
                "sPrevious":""
                //"sNext": "@Translation.Get('Nästa', Enums.TranslationSource.TextTranslation)",
                //"sPrevious": "@Translation.Get('Föregående', Enums.TranslationSource.TextTranslation)"
            }
        }
    });
}
// TABLE PAGING END



// YES and NO SWITCH FOR CHECKBOXES
$('.switchcheckbox').bootstrapSwitch('onText', trans_yes);
$('.switchcheckbox').bootstrapSwitch('offText', trans_no);
$('.switchcheckbox').bootstrapSwitch('size', 'small');
$('.switchcheckbox').bootstrapSwitch('onColor', 'success');
//$('.switchcheckbox').bootstrapSwitch('offColor', 'danger');

// YES and NO SWITCH FOR CHECKBOXES END


$(".chosen-select").chosen({ width: "300px" });
