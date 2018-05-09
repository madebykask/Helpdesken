var dhHelpdesk = {};

//Start FAQ Acordion

//tabbar
$('.nav-tabs li:not(.disabled) a').click(function (e) {
    e.preventDefault();
    $(this).tab('show');
    //activeTab.val($(this).attr('href'));
});

$("input:text:visible:not(.ignore-globalfocus):first").focus();

//Hämtar vald text från droptodwn button
function getBreadcrumbs(a) {
    var path = $(a).text(), $parent = $(a).parents("li").eq(1).find("a:first");

    if ($parent.length == 1) {
        path = getBreadcrumbs($parent) + " - " + path;
    }
    return path;
}


function PluploadTranslation(languageId) {
    if (languageId == 1) {
        plupload.addI18n({
            'Select files': 'Välj filer',
            'Add files to the upload queue and click start upload.': 'Lägg till filer i kön och tryck på Ladda upp.',
            'Filename': 'Filnamn',
            'Status': 'Status',
            'Size': 'Storlek',
            'Add files': 'Lägg till filer',
            'Add Files': 'Lägg till',
            'Start Upload': 'Ladda upp',
            'Stop current upload': 'Stoppa uppladdningen',
            'Start uploading queue': 'Starta uppladdningen',
            'Drag files here.': 'Dra filer hit'
        });
    }

    if (languageId == 2) {
        plupload.addI18n({
            'Select files': 'Select files',
            'Add files to the upload queue and click start upload.': 'Add files to the upload queue and click the start button.',
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

function today() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } var today = yyyy + '-' + mm + '-' + dd;
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
    $.post(postTo, { 'id': id, 'customerId': customerId, 'departmentFilterFormat': departmentFilterFormat, myTime: Date.now() }, function (data) {
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

function CaseInitForm() {

    //$('#CaseLog_TextExternal').focus(function () {
    //    CaseWriteTextToLogNote('');
    //});

    //$('#CaseLog_TextInternal').focus(function () {
    //    CaseWriteTextToLogNote('internal');
    //});

    $('#NewCase_ReportedBy').typeahead(GetComputerUserSearchOptions());
    $('#NewCase_InventoryNumber').typeahead(GetComputerSearchOptions());

    //$('#CountryId').change(function () {
    //    CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeCountry/', '#case__Supplier_Id', $('#DepartmentFilterFormat').val());
    //});

    //$('#case__Department_Id').change(function () {
    //    CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeDepartment/', '#case__Ou_Id', $('#DepartmentFilterFormat').val());
    //    $('#divInvoice').hide();
    //    $.get('/Cases/ShowInvoiceFields/', { 'departmentId': $(this).val() }, function (data) {
    //        if (data == 1) {
    //            $('#divInvoice').show();
    //        }
    //    }, 'json');
    //});

    //$('#case__Region_Id').change(function () {
    //    CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeRegion/', '#case__Department_Id', $('#DepartmentFilterFormat').val());
    //});

    //$('#case__Status_Id').change(function () {
    //    if ($(this).val() > 0) {
    //        $.post('/Cases/ChangeStatus/', { 'id': $(this).val() }, function (data) {
    //            if (data != undefined) {
    //                var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
    //                if (exists > 0 && data.WorkingGroup_Id > 0) {
    //                    $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
    //                }
    //                exists = $('#case__StateSecondary_Id option[value=' + data.StateSecondary_Id + ']').length;
    //                if (exists > 0 && data.StateSecondary_Id > 0) {
    //                    $("#case__StateSecondary_Id").val(data.Priority_Id);
    //                }
    //            }
    //        }, 'json');
    //    }

    //});

    //$('#case__CaseType_Id').change(function () {
    //    SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeCaseType/', '#case__Performer_User_Id');
    //});

    //$('#case__Priority_Id').change(function () {
    //    $.post('/Cases/ChangePriority/', { 'id': $(this).val() }, function (data) {
    //        if (data.ExternalLogText != null) {
    //            $('#CaseLog_TextExternal').val(data.ExternalLogText);
    //        }
    //    }, 'json');
    //});

    //$('#case__System_Id').change(function () {
    //    SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeSystem/', '#case__Urgency_Id');
    //    SetPriority();
    //});

    //$('#case__Impact_Id').change(function () {
    //    SetPriority();
    //});

    //$('#case__Urgency_Id').change(function () {
    //    SetPriority();
    //});

    //$('#case__ProductArea_Id').change(function () {
    //    if ($(this).val() > 0) {
    //        $.post('/Cases/ChangeProductArea/', { 'id': $(this).val() }, function (data) {
    //            //alert(JSON.stringify(data));
    //            if (data != undefined) {
    //                //debugger
    //                var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
    //                if (exists > 0 && data.WorkingGroup_Id > 0) {
    //                    $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
    //                }
    //                exists = $('#case__Priority_Id option[value=' + data.Priority_Id + ']').length;
    //                if (exists > 0 && data.Priority_Id > 0) {
    //                    $("#case__Priority_Id").val(data.Priority_Id);
    //                }
    //            }
    //        }, 'json');
    //    }
    //});

    //$('#case__WorkingGroup_Id').change(function () {
    //    // filter administrators
    //    var DontConnectUserToWorkingGroup = $('#CaseMailSetting_DontConnectUserToWorkingGroup').val();
    //    if (DontConnectUserToWorkingGroup == 0) {
    //        CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeWorkingGroupFilterUser/', '#case__Performer_User_Id', $('#DepartmentFilterFormat').val());
    //    }
    //    //set state secondery
    //    SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeWorkingGroupSetStateSecondary/', '#case__StateSecondary_Id')
    //});

    //$('#case__StateSecondary_Id').change(function () {
    //    $('#CaseLog_SendMailAboutCaseToNotifier').removeAttr('disabled');
    //    $.post('/Cases/ChangeStateSecondary', { 'id': $(this).val() }, function (data) {
    //        // disable send mail checkbox
    //        if (data.NoMailToNotifier == 1) {
    //            $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', false);
    //            $('#CaseLog_SendMailAboutCaseToNotifier').attr('disabled', true);
    //        }
    //        // set workinggroup id
    //        var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
    //        if (exists > 0 && data.WorkingGroup_Id > 0) {
    //            alert(data.WorkingGroup_Id);
    //            $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
    //        }
    //    }, 'json');
    //});

    //$('#lstStandarTexts').change(function () {
    //    var regexp = /<BR>/g
    //    var txt = $('#lstStandarTexts :selected').text().replace(regexp, "\n");
    //    var writeTextToExternalNote = $('#WriteTextToExternalNote').val();
    //    var field = '#CaseLog_TextInternal';

    //    if (writeTextToExternalNote == '') {
    //        field = '#CaseLog_TextExternal';
    //    }

    //    if (txt.length > 1) {
    //        $(field).val($(field).val() + txt);
    //        $(field).focus();

    //        var input = $(field);
    //        input[0].selectionStart = input[0].selectionEnd = input.val().length;
    //    }
    //});

    //$('#divCaseType ul.dropdown-menu li a').click(function (e) {
    //    e.preventDefault();
    //    var val = $(this).attr('value');
    //    $("#divBreadcrumbs_CaseType").text(getBreadcrumbs(this));
    //    $("#case__CaseType_Id").val(val).trigger('change');
    //});

    //$('#divProductArea ul.dropdown-menu li a').click(function (e) {
    //    e.preventDefault();
    //    var val = $(this).attr('value');
    //    $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
    //    $("#case__ProductArea_Id").val(val).trigger('change');;
    //});

    //$('#AddNotifier').click(function (e) {
    //    e.preventDefault();
    //    var win = window.open('/Notifiers/NewNotifierPopup', '_blank', 'left=100,top=100,width=900,height=700,toolbar=0,resizable=1,menubar=0,status=0,scrollbars=1');
    //    //win.onbeforeunload = function () { CaseNewNotifierEvent(win.returnValue); }
    //});

    //if (!Date.now) {
    //    Date.now = function () { return new Date().getTime(); };
    //}

    //var getCaseFiles = function () {
    //    $.get('/Case/Files', { id: $('#CaseKey').val(), now: Date.now() }, function (data) {
    //        $('#divCaseFiles').html(data);
    //        bindDeleteCaseFileBehaviorToDeleteButtons();
    //    });
    //};

    //var getLogFiles = function () {
    //    $.get('/Case/LogFiles', { id: $('#LogKey').val(), now: Date.now() }, function (data) {
    //        $('#divCaseLogFiles').html(data);
    //        bindDeleteLogFileBehaviorToDeleteButtons();
    //    });
    //};

    //var _plupload;

    //$('#upload_files_popup, #upload_logfiles_popup').on('hide', function () {

    //    if (_plupload != undefined) {
    //        if (_plupload.pluploadQueue().files.length > 0) {
    //            if (_plupload.pluploadQueue().state == plupload.UPLOADING) {
    //                _plupload.pluploadQueue().stop();
    //                for (var i = 0; i < _plupload.pluploadQueue().files.length; i++) {
    //                    _plupload.pluploadQueue().removeFile(_plupload.pluploadQueue().files[i]);
    //                }
    //                _plupload.pluploadQueue().refresh();
    //            }
    //        }
    //    }
    //});

    //$('#upload_files_popup').on('show', function () {
    //    _plupload = $('#file_uploader').pluploadQueue({
    //        runtimes: 'html5,html4',
    //        url: '/Case/UploadCaseFile',
    //        multipart_params: { id: $('#CaseKey').val() },
    //        filters: {
    //            max_file_size: '30mb',
    //        },
    //        buttons: { browse: true, start: true, stop: true, cancel: true },
    //        preinit: {
    //            Init: function (up, info) {
    //                //log('[Init]', 'Info:', info, 'Features:', up.features);
    //            },

    //            UploadFile: function (up, file) {
    //                //log('[UploadFile]', file);
    //            },

    //            UploadComplete: function (up, file) {
    //                //plupload_add
    //                $(".plupload_buttons").css("display", "inline");
    //                $(".plupload_upload_status").css("display", "inline");
    //                up.refresh();
    //            }
    //        },
    //        init: {
    //            FileUploaded: function () {
    //                getCaseFiles();
    //            },

    //            Error: function (uploader, e) {
    //                if (e.status != 409) {
    //                    return;
    //                }
    //            },

    //            StateChanged: function (uploader) {
    //                if (uploader.state != plupload.STOPPED) {
    //                    return;
    //                }
    //                uploader.refresh();
    //            }
    //        }
    //    });
    //});

    //$('#upload_logfiles_popup').on('show', function () {
        
    //    _plupload = $('#logfile_uploader').pluploadQueue({
    //        runtimes: 'html5,html4',
    //        url: '/Case/UploadLogFile',
    //        multipart_params: { id: $('#LogKey').val() },
    //        filters: {
    //            max_file_size: '30mb',
    //        },
    //        buttons: { browse: true, start: true, stop: true, cancel: true },
    //        preinit: {
    //            Init: function (up, info) {
    //                //log('[Init]', 'Info:', info, 'Features:', up.features);
    //            },

    //            UploadFile: function (up, file) {
    //                //log('[UploadFile]', file);
    //            },

    //            UploadComplete: function (up, file) {
    //                //plupload_add
    //                $(".plupload_buttons").css("display", "inline");
    //                $(".plupload_upload_status").css("display", "inline");
    //                up.refresh();
    //            }
    //        },
    //        init: {
    //            FileUploaded: function () {
    //                getLogFiles();
    //            },

    //            Error: function (uploader, e) {
    //                if (e.status != 409) {
    //                    return;
    //                }
    //            },

    //            StateChanged: function (uploader) {
    //                if (uploader.state != plupload.STOPPED) {
    //                    return;
    //                }
    //                uploader.refresh();
    //            }
    //        }
    //    });
    //});

    //LogInitForm();
    //bindDeleteCaseFileBehaviorToDeleteButtons();
    //bindDeleteLogFileBehaviorToDeleteButtons();
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
        $('#CaseLog_SendMailAboutCaseToNotifier').removeAttr('checked');
        if (this.value.length) {
            $('#CaseLog_SendMailAboutCaseToNotifier:not(:disabled)').attr('checked', 'checked');
        }
    });
}

function GetComputerUserSearchOptions() {

    
    var options = {
        items: 20,
        minLength: 2,
        
        source: function (query, process) {
            return $.ajax({
                url: '/case/searchuser',
                type: 'post',
                data: { query: query, customerId: $('#Newcase_Customer_Id').val() },
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
var ms = $('.multiselect');
if (ms.lenght > 0) {
    ms.multiselect({
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
}

function bindDeleteCaseFileBehaviorToDeleteButtons() {
    $('#case_files_table a[id^="delete_casefile_button_"]').click(function () {
        var key = $('#CaseKey').val();
        var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
        var pressedDeleteFileButton = this;
        $.post("/Cases/DeleteCaseFile", { id: key, fileName: fileName, myTime: Date.now() }, function () {
            $(pressedDeleteFileButton).parents('tr:first').remove();
        });
    });
}

function bindDeleteLogFileBehaviorToDeleteButtons() {
    $('#log_files_table a[id^="delete_logfile_button_"]').click(function () {
        var key = $('#LogKey').val();
        var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
        var pressedDeleteFileButton = this;

        $.post("/Cases/DeleteLogFile", { id: key, fileName: fileName, myTime:Date.now() }, function () {
            $(pressedDeleteFileButton).parents('tr:first').remove();
        });
    });
}

function SetPriority() {
    var impactId = $('#case__Impact_Id').val();
    var urgencyId = $('#case__Urgency_Id').val();

    if (urgencyId > 0 && impactId > 0) {
        $.post('/Cases/GetPriorityIdForImpactAndUrgency', { 'impactId': impactId, 'urgencyId': urgencyId, myTime:Date.now() }, function (data) {
            if (data != null) {
                var exists = $('#case__Priority_Id Option[value=' + data + ']').length;
                if (exists > 0) {
                    $('#case__Priority_Id').val(data);
                }
            }
        }, 'json');
    }

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

function InitDataTable(tableUniqId, lengthMenu, emptyTable, next, prev, search, info, infoEmpty, options, onError) {
    var dataTable = $('#' + tableUniqId);
    $.fn.dataTable.ext.errMode = 'none';
    if (onError && typeof onError === "function")
        dataTable.on('error.dt', function (e, settings, techNote, message) {
            onError(e, settings, techNote, message);
        });
    return dataTable.DataTable($.extend({},
    {
        language: {
            "sLengthMenu": "_MENU_ " + lengthMenu,
            "sEmptyTable": emptyTable,
            "sInfo": info,
            "sInfoEmpty": infoEmpty,
            "sSearch": search,
            "oPaginate": {
                "sFirst": "Första",
                "sLast": "Sista",
                "sNext": next,
                "sPrevious": prev
            }
}
        //'sError': (onError && typeof onError === "function") ? 'none' : 'throw',
        //"sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
        //"sPaginationType": "bootstrap",
        //"oLanguage": {
        //    "sLengthMenu": "_MENU_ " + perText,
        //    "sInfo": showingText + " _PAGE_ / _PAGES_",
        //    "oPaginate": {
        //        "sFirst": "First",
        //        "sLast": "Last",
        //        "sNext": "",
        //        "sPrevious": ""
        //    }
        //}
    }, options || {}));
}
// TABLE PAGING END

function CaseNewNotifierEvent(id) {
    alert(id);
}

$(".chosen-select").chosen({
    width: "300px",
    'placeholder_text_multiple': window.placeholder_text_multiple,
    'no_results_text': window.no_results_text
});

$(".chosen-single-select").chosen({
    width: "315px",
    'placeholder_text_multiple': window.placeholder_text_multiple,
    'no_results_text': window.no_results_text
});

