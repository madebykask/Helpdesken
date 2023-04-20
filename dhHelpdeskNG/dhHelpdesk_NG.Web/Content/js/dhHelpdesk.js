// Global variables 
var dhHelpdesk = window.dhHelpdesk || {};
var publicCustomerId = $('#case__Customer_Id').val();

var _departmentControlName = '#case__Department_Id';


//tabbar
$('.nav-tabs li:not(.disabled) a').click(function (e) {
    e.preventDefault();
    $(this).tab('show');
});

$(".nav-tabs-actions a").unbind("click");

if (typeof (window.Params) === 'undefined' || window.Params.autoFocus !== false) {
    $(".content input:text:not(.chosen-container input:text), .content textarea").eq(0).focus();
}


$('#case__RegLanguage_Id').change(function () {
    ChangeCaseLanguageTo($("#case__RegLanguage_Id").val());
});

function ChangeCaseLanguageTo(newLanguageId, updateDropDown) {
    var langItems = document.getElementsByClassName('langItem');
    for (var i = 0; i < langItems.length; ++i) {
        var item = langItems[i];
        if (item.id == "langItem" + newLanguageId)
            item.innerHTML = item.innerText + ' <i class="icon-ok"></i>';
        else
            item.innerHTML = item.innerText;
    }

    if (updateDropDown == true)
        $("#case__RegLanguage_Id").val(newLanguageId).change();

    $("#case_.RegLanguage_Id").val(newLanguageId);
}

function ShowToastMessage(message, msgType, isSticky) {
    var sticky = false;
    if (isSticky)
        sticky = true;
    $().toastmessage('showToast', {
        text: message,
        sticky: sticky,
        position: 'top-center',
        type: msgType,
        closeText: '',
        stayTime: 5000,
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
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }
    var res = yyyy + '-' + mm + '-' + dd;
    return res;
}

function getNow() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var hh = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }
    if (hh < 10) { hh = '0' + hh }
    if (m < 10) { m = '0' + m }
    if (s < 10) { s = '0' + s }
    var res = yyyy + '-' + mm + '-' + dd + ' ' + hh + '.' + m + '.' + s;
    return res;
}

function replaceLinebreaksInString(input) {
    return input.replace(/\r\n|\r|\n/g, "<br />");
}

// Cose window or tab
function close_window() {
    //if (confirm("WARNING TEXT XXXXXXX TRANSLATE")) {
    close();
    //}
}

function SelectValueInOtherDropdownOnChange(id, postTo, ctl, readonlyElement, raiseEvent) {
    raiseEvent = raiseEvent || false;
    return $.post(postTo, { 'id': id }, function (data) {
        if (data != null) {
            SetSelectValue(ctl, data, readonlyElement, raiseEvent);
        }
    }, 'json');
}

function SetSelectValue(selId, val, readonlyElementId, raiseEvent) {
    raiseEvent = raiseEvent || false;
    var $sel = $(selId);
    var exists = $sel.find('option[value=' + val + ']').length > 0;
    if (exists) {
        $sel.val(val);
        if (raiseEvent) $sel.change();
        if (readonlyElementId && readonlyElementId.length) {
            var $readonlyElement = $(readonlyElementId);
            if ($readonlyElement.length) {
                $readonlyElement.val(val);
            }
        }
        return true;
    }
    return false;
}

function CaseCascadingSelectlistChange(id, customerId, postTo, ctl, departmentFilterFormat) {
    var ctlOption = ctl + ' option';
    $.post(postTo, { 'id': id, 'customerId': customerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {
        var $ctl = $(ctl);
        var selected = $ctl.val();
        $(ctlOption).remove();
        $ctl.append('<option value="">&nbsp;</option>');
        if (data != undefined) {
            for (var i = 0; i < data.list.length; i++) {
                $ctl.append('<option value="' + data.list[i].id + '">' + data.list[i].name + '</option>');
            }
            $ctl.val(selected);
            $ctl.trigger('applyValue');
            if (ctl == '#Performer_Id') {
                $ctl.trigger('change');
            }
        }
    });
}

function CaseWriteTextToLogNote(value) {
    $('#WriteTextToExternalNote').val(value);
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
                FilesAdded: function (up, files) {
                    var fileUploadWhiteList = window.parameters.fileUploadWhiteList;
                    var invalidFileExtensionText = window.parameters.invalidFileExtensionText;

                    if (fileUploadWhiteList != null) {
                        var whiteList = fileUploadWhiteList;

                        var isFileInWhiteList = function (filename, whiteList) {
                            if (filename.indexOf('.') !== -1) {
                                var extension = filename.split('.').reverse()[0].toLowerCase();
                                if (whiteList.indexOf(extension) >= 0)
                                    return true;
                            }
                            else {
                                if (whiteList.indexOf('') >= 0)
                                    return true;
                            }
                            return false;
                        }

                        files.forEach(function (e) {
                            if (!isFileInWhiteList(e.name, whiteList)) {
                                up.removeFile(e);
                                alert(e.name + ' ' + invalidFileExtensionText);
                            }
                        })

                    }
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

function productAreaHasChild(productAreaId) {
    $.get('/Cases/ProductAreaHasChild', { pId: productAreaId, now: Date.now() }, function (data) {
        return data;
    });
}

async function finishingCauseHasChild(finishingCauseId) {
    let ret = await $.get('/Cases/FinishingCauseHasChild', { fId: finishingCauseId, now: Date.now() });
    return ret;
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

function SendToDialogOperationLogCallback(email) {
    if (email.length > 0) {

        var str = '';
        for (var i = 0; i < email.length; i++) {
            str += email[i] + '\r\n';
        }
        $('#divEmailRecepientsOperationLog').show();
        $('#OperationLogList_EmailRecepientsOperationLog').val(str);
    }
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
            'Add files to the upload queue and click start upload.': 'Add files to the upload queue and click start upload.',
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
        var isTemporary = $(this).data('temporary');
        var pressedDeleteFileButton = this;
        $.post("/Cases/DeleteCaseFile", { id: key, fileName: fileName, isTemporary: isTemporary }, function () {
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

function bindDeleteLogFileBehaviorToDeleteButtons(isInternalLog) {
    // add delete btn handler
    var $container = isInternalLog === true ? $('div.internalLog-files') : $('div.externalLog-files');
    if ($container.length) {
        $container.find('a[id^="delete_logfile_button_"]').click(function () {
            var key = $('#LogKey').val();
            var $row = $(this).closest('tr');
            var $fileLinkEl = $row.find('a');
            var fileName = $fileLinkEl.text();
            var isAttached = $fileLinkEl.hasClass('isExisted');
            var logFileId = isAttached ? $row.find("#logfile_id").val() : null;
            var isInternalLog = $row.closest('table').data('logtype') === 'internalLog';

            //prepare input data
            var data = {
                id: key,
                fileName: fileName,
                isExisting: isAttached,
                fileId: logFileId,
                logType: isInternalLog === true ? 1 : 0
            };

            $.post("/Cases/DeleteLogFile", $.param(data), function () {
                $row.remove();
                var $logFileNames = isInternalLog ? $('#LogFileNamesInternal') : $('#LogFileNames');
                var fileNames = $logFileNames.val();
                fileNames = fileNames.replace("|" + fileName.trim(), "");
                fileNames = fileNames.replace(fileName.trim() + "|", "");
                $logFileNames.val(fileNames);

                // Raise event about deleted file
                $(document).trigger("OnDeleteCaseLogFile", [key, fileName]);
            });
        });
    }
}

function SetPriority() {
    var impactId = $('#case__Impact_Id').val();
    var urgencyId = $('#case__Urgency_Id').val();

    if (urgencyId > 0 && impactId > 0) {
        $.post('/Cases/GetPriorityIdForImpactAndUrgency', { 'impactId': impactId, 'urgencyId': urgencyId }, function (data) {
            if (data != null) {
                var exists = $('#case__Priority_Id Option[value=' + data + ']').length;
                if (exists > 0) {
                    const $pririty = $('#case__Priority_Id');
                    $pririty.val(data);
                    $pririty.change();
                }
            }
        }, 'json');
    }
}

function NewNotifierEvent(id) {
    $.post('/Cases/Get_User', { 'Id': id }, function (data) {
        if (data != undefined) {
            var departmentFilterFormat = $('#DepartmentFilterFormat').val();
            $('#case__ReportedBy').val(data.num);

            // Raise event about UserId changed.
            $(document).trigger("OnUserIdChanged", [data.num]);
            
            $('#case__PersonsName').val(data.name);
            $('#case__PersonsEmail').val(data.email);
            $('#case__PersonsPhone').val(data.phone);
            $('#case__PersonsCellphone').val(data.cellphone);
            $('#case__Place').val(data.place);
            $('#case__UserCode').val(data.usercode);
            $('#case__CostCentre').val(data.costcentre);

            $('#case__Region_Id').val(data.regionid);
            $('#RegionName').val(data.regionname);

            $(_departmentControlName).val(data.departmentid);
            refreshDepartment(data.regionid, departmentFilterFormat, data.departmentid, data.ouid);

        }
    }, 'json');
}

function copyCase(id, customerId) {

    var url = '/cases/copy/' + id + '?customerId=' + customerId;
    window.location.href = url;

}

// calculate utc time
$(function () {
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

            $("#cbxBulkCaseEditAll").prop('checked', false);
            $('#liBulkCaseEdit').hide();

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

function InitDataTable(tableUniqId, perText, showingText, options, onError, emptyTable, infoEmpty, sanitizeDataFunc) {
    var dataTable = $('#' + tableUniqId);
    $.fn.dataTable.ext.errMode = 'none';
    if (onError && typeof onError === "function")
        dataTable.on('error.dt', function (e, settings, techNote, message) {
            onError(e, settings, techNote, message);
        });
    if (typeof sanitizeDataFunc === "function") {
        dataTable
            .on('xhr.dt',
                function(e, settings, json, xhr) {
                    sanitizeDataFunc(json.data);
                });
    }
    return dataTable
        .DataTable($.extend({}, {
        //'sError': (onError && typeof onError === "function") ? 'none' : 'throw',
        "sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "stateSave": true,
        "oLanguage": {
            "sLengthMenu": "_MENU_ " + perText,
            "sInfo": showingText + " _PAGE_ / _PAGES_",
            "sEmptyTable": emptyTable,
            "sInfoEmpty": infoEmpty,
            "oPaginate": {
                "sFirst": "First",
                "sLast": "Last",
                "sNext": "",
                "sPrevious": ""
            }
        }
    }, options || {}));
}
// TABLE PAGING END



// YES and NO SWITCH FOR CHECKBOXES
    $(".switchcheckbox").bootstrapSwitch({
        "onText": window.trans_yes,
        "offText": window.trans_no,
        "size": "small",
        "onColor": "success"
    });
//$('.switchcheckbox').bootstrapSwitch('offColor', 'danger');

// YES and NO SWITCH FOR CHECKBOXES END


$(".chosen-select:not(.custom)").chosen({
    width: "300px",
    'placeholder_text_multiple': placeholder_text_multiple,
    'no_results_text': no_results_text
});

$(".chosen-single-select:not(.custom)").chosen({
    width: "315px",
    'placeholder_text_single': placeholder_text_single,
    'no_results_text': no_results_text
});


/////////////////////////////////////////////////////////////////////////////////////////
// Button Dropdown Menu fixes
//function setDynamicDropDowns() {
//    var dynamicDropDownClass = '.DynamicDropDown';
//    var fixedArea = 90;
//    var pageSize = $(window).height() - fixedArea;
//    var scrollPos = $(window).scrollTop();
//    var elementToTop = $(dynamicDropDownClass).offset().top - scrollPos - fixedArea;
//    var elementToDown = pageSize - elementToTop;
//    if (elementToTop < -$(dynamicDropDownClass).height())
//        $(dynamicDropDownClass).removeClass('open');

//    if (elementToTop <= elementToDown) {
//        $(dynamicDropDownClass).removeClass('dropup');
//    } else {
//        $(dynamicDropDownClass).addClass('dropup');
//    }
//}

function getObjectPosInView(element) {
    var fixedArea = 90;
    var pageSize = $(window).height() - fixedArea;
    var scrollPos = $(window).scrollTop();
    var elementToTop = $(element).offset().top - scrollPos - fixedArea;
    var elementToDown = pageSize - elementToTop;
    return { ToTop: elementToTop, ToDown: elementToDown };
}

function updateDropdownPosition(element) {
    var objPos = getObjectPosInView(element);
    if (objPos.ToTop < objPos.ToDown) {
        $('.dropdown-menu.subddMenu').css('max-height', objPos.ToDown - 50 + 'px');
    } else {
        $('.dropdown-menu.subddMenu').css('max-height', objPos.ToTop + 'px');
    }
}

function dynamicDropDownBehaviorOnMouseMove(target) {
    var target$ = $(target);
    if (target$ != undefined && target$.hasClass('DynamicDropDown_Up') && target$.index(0) !== -1) {
        var objPos = getObjectPosInView(target$[0]);
        var subMenu$ = $(target$[0]).children('ul');
        var targetPos = target$[0].getBoundingClientRect();

        subMenu$.css({
            bottom: 'auto',
            position: 'fixed',
            top: $(window).height() - objPos.ToDown + 'px',
            left: targetPos.left + target$.width() + 'px',
            'max-height': $(window).innerHeight() - objPos.ToDown + 'px'
        });


        target$.children('.subddMenu').children('.dropdown-submenu').css('position', 'static');

        var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
        if (isChrome) {
            if (target$.parent().hasClass('parentddMenu') === false && subMenu$.get(0).scrollHeight <= subMenu$.innerHeight())
                subMenu$.css('left', targetPos.left + target$.innerWidth() - target$.position().left + 'px');
        }

        var submenuPos = subMenu$[0].getBoundingClientRect();
        if ((submenuPos.top + submenuPos.height) > window.innerHeight) {
            var offset = (submenuPos.top + submenuPos.height) - window.innerHeight;
            if (offset > 0) {
                var top = $(window).height() - objPos.ToDown - offset;
                subMenu$.css('top', top);
            }
        }
    }

}

function initDynamicDropDowns() {
    $('ul.dropdown-menu.subddMenu.parentddMenu').on('mouseenter', function () {
        var $html = $('html');
        $html.data('previous-overflow', $html.css('overflow'));
        $html.css('overflow', 'hidden');
    });

    $('ul.dropdown-menu.subddMenu.parentddMenu').on('mouseleave', function () {
        var $html = $('html');
        $html.css('overflow', $html.data('previous-overflow'));
    });

    $('.dropdown-submenu.DynamicDropDown_Up').on('mousemove', function (event) {
        dynamicDropDownBehaviorOnMouseMove(event.target.parentElement);
    });

    $('ul.dropdown-menu.subddMenu.parentddMenu').prev('button').on('click', function () {
        updateDropdownPosition(this);
    });
}

function initDynamicDropDownsKeysBehaviour() {
    $("button.dropdown-toggle[data-toggle=dropdown]").on("click", function (e) {
        $(this).parent().find("li.dropdown-submenu > ul").css("display", "");
    });

    $('button.dropdown-toggle[data-toggle=dropdown], ul.dropdown-menu').on('keydown', function (e) {
        if (!/(37|38|39|40|27)/.test(e.keyCode)) return true;

        var target = $(e.target).closest('ul.dropdown-menu');
        var $this = target.length > 0 ? $(target[0]) : $(this);

        e.preventDefault();
        e.stopPropagation();

        if ($this.is('.disabled, :disabled')) return true;

        var $group = $this.closest('.btn-group');
        var isActive = $group.hasClass('open');
        var $parent = $this.parent();
        var $items = $parent.children('ul.dropdown-menu').children('li:not(.divider):visible').children('a');
        var index = 0;

        if (isActive && e.keyCode === 27) {
            if (e.which === 27) $group.find('button.dropdown-toggle[data-toggle=dropdown]').focus();
            return $this.click();
        }

        if (!isActive && e.keyCode === 40) {
            if (!$items.length) return $this.click();
            $items.eq(index).focus();
            return $this.click();
        }

        if (!$items.length) return true;

        index = $items.index($items.filter(':focus'));

        if (e.keyCode === 38 && index > 0) index--; // up
        if (e.keyCode === 40 && index < $items.length - 1) index++; // down
        if (!~index) index = 0;

        var currentItem = $items.eq(index);

        if (e.keyCode === 39) {
            var currentLi = currentItem.parent();
            if (currentLi.hasClass('dropdown-submenu')) {
                currentLi.children('ul.dropdown-menu').css('display', 'block');
                currentItem = currentLi.children('ul.dropdown-menu').children('li:not(.divider):visible:first')
                    .children('a').first();
                if (currentLi.hasClass('DynamicDropDown_Up')) {
                    dynamicDropDownBehaviorOnMouseMove(currentLi);
                }
            }
        }

        if (e.keyCode === 37) {
            if ($parent.hasClass('dropdown-submenu')) {
                currentItem = $parent.children('a:first');
                $this.css('display', '');
            }
        }

        currentItem.focus();

        return true;
    });
}
/////////////////////////////////////////////////////////////////////////////////////////

$.fn.checkUrlFileExists = function () {
    "use strict";
    var url = $(this)[0].href;
    var http = new XMLHttpRequest();
    http.open("HEAD", url, false);
    http.send();
    return http.status === 200;
};

$(function() {
    initDynamicDropDowns();
});