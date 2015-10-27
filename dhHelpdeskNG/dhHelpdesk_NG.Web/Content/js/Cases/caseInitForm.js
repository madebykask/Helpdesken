/**
*
* This code was taken from dhHelpdesk.js and still has dependencies from it
* @TODO: code review wanted to spare connections between functions from that file
*/

function SetFocusToReportedByOnCase() {
    if ($('#ShowReportedBy').val() == 1) {
        $('#case__ReportedBy').focus();
    }
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


function refreshOrganizationUnit(departmentId, departmentFilterFormat, selectedOrganizationUnitId) {
    $(publicOUControlName).val('');
    $(publicReadOnlyOUName).val('');
    var ctlOption = publicOUControlName + ' option';
    $(ctlOption).remove();
    $(publicOUControlName).prop('disabled', true);
    $(publicOUControlName).append('<option value="">&nbsp;</option>');
    $.post(publicChangeDepartment, { 'id': departmentId, 'customerId': publicCustomerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {
        if (data != undefined) {
            for (var i = 0; i < data.list.length; i++) {
                var item = data.list[i];
                var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                if (option.val() == selectedOrganizationUnitId) {
                    $(publicOUControlName).val(selectedOrganizationUnitId);
                    $(publicReadOnlyOUName).val(item.name);
                    option.prop("selected", true);
                }
                $(publicOUControlName).append(option);
            }
        }
    }, 'json').always(function () {
        $(publicOUControlName).prop('disabled', false);
    });
}


function refreshDepartment(regionId, departmentFilterFormat, selectedDepartmentId, selectedOU) {
    $(publicDepartmentControlName).val('');
    $(publicReadOnlyDepartmentName).val('');
    var ctlOption = publicDepartmentControlName + ' option';
    $(ctlOption).remove();
    $(publicDepartmentControlName).append('<option value="">&nbsp;</option>');
    $(publicDepartmentControlName).prop('disabled', true);
    $.post(publicChangeRegion, { 'id': regionId, 'customerId': publicCustomerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {

        if (data != undefined) {
            for (var i = 0; i < data.list.length; i++) {
                var item = data.list[i];
                var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                if (option.val() == selectedDepartmentId) {
                    $(publicDepartmentControlName).val(selectedDepartmentId);
                    $(publicReadOnlyDepartmentName).val(item.name);
                    option.prop("selected", true);
                }
                $(publicDepartmentControlName).append(option);
            }
        }
    }, 'json').always(function () {
        $(publicDepartmentControlName).prop('disabled', false);
        refreshOrganizationUnit(selectedDepartmentId, departmentFilterFormat, selectedOU);
    });
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
                                    //Changed in HotFix 5.3.13
                                    //, name: item.SurName + ' ' + item.FirstName
                                    , name: item.FirstName + ' ' + item.SurName
                                    , email: item.Email
                                    , place: item.Location
                                    , phone: item.Phone
                                    , usercode: item.UserCode
                                    , cellphone: item.CellPhone
                                    , regionid: item.Region_Id
                                    , regionname: item.RegionName
                                    , departmentid: item.Department_Id
                                    , departmentname: item.DepartmentName
                                    , ouid: item.OU_Id
                                    , ouname: item.OUName
                                    , name_family: item.SurName + ' ' + item.FirstName
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
                || ~item.name_family.toLowerCase().indexOf(this.query.toLowerCase())
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
            var orgQuery = this.query;
            var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
            var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email;
            var resultBy_NameFamily = item.name_family + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email;
                     
            if (result.toLowerCase().indexOf(orgQuery.toLowerCase()) > -1)               
                return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                    return '<strong>' + match + '</strong>';
                });
            else
                return resultBy_NameFamily.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                    return '<strong>' + match + '</strong>';
                });
           
        },

        updater: function (obj) {
            var item = JSON.parse(obj);
            var departmentFilterFormat = $('#DepartmentFilterFormat').val();
                        
            $('#case__ReportedBy').val(item.num);
            
            // Raise event about UserId changed.
            $(document).trigger("OnUserIdChanged", [item.num]);
            
            if (item.name != "" && item.name != null)
                $('#case__PersonsName').val(item.name);

            if (item.email != "" && item.email != null)
                $('#case__PersonsEmail').val(item.email);
            
            if (item.phone != "" && item.phone != null)
                $('#case__PersonsPhone').val(item.phone);

            if (item.cellphone != "" && item.cellphone != null)
                $('#case__PersonsCellphone').val(item.cellphone);

            if (item.place != "" && item.place != null)
                $('#case__Place').val(item.place);

            if (item.usercode != "" && item.usercode != null)
                $('#case__UserCode').val(item.usercode);


            if (item.regionid != "" && item.regionid != null) {
                $('#case__Region_Id').val(item.regionid);
                $('#RegionName').val(item.regionname);
            }

            if (item.regionid != "" && item.regionid != null && 
                item.departmentid != "" && item.departmentid != null) {
                $(publicDepartmentControlName).val(item.departmentid).trigger('change');
                refreshDepartment(item.regionid, departmentFilterFormat, item.departmentid, item.ouid);
            }

            return item.num;
        }
    };

    return options;
}



/**
* Initializator for case edit form
*/
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

    // Remove after implementing http://redmine.fastdev.se/issues/10995
    $('#case__Region_Id').change(function () {
        var regionId = $(this).val();
        var departmentFilterFormat = $('#DepartmentFilterFormat').val();
        refreshDepartment(regionId, departmentFilterFormat);
    });


    $(publicDepartmentControlName).change(function () {
        // Remove after implementing http://redmine.fastdev.se/issues/10995        
        var departmentId = $(this).val();
        var departmentFilterFormat = $('#DepartmentFilterFormat').val();
        refreshOrganizationUnit(departmentId, departmentFilterFormat);
        showInvoice(departmentId);
    });

    function showInvoice(departmentId) {
        $('#divInvoice').hide();
        $.get('/Cases/ShowInvoiceFields/', { 'departmentId': departmentId }, function (data) {
            if (data == 1) {
                $('#divInvoice').show();
            }
        }, 'json');
    }


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
        SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeCaseType/', '#Performer_Id');
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
        var $workingGroup = $("#case__WorkingGroup_Id");
        $("#ProductAreaHasChild").val(0);
        document.getElementById("divProductArea").classList.remove("error");
        if ($(this).val() > 0) {
            $.post('/Cases/ChangeProductArea/', { 'id': $(this).val() }, 'json').done(function(data) {
                if (data != undefined) {
                    var exists = $workingGroup.find('option[value=' + data.WorkingGroup_Id + ']').length;
                    if (exists > 0 && data.WorkingGroup_Id > 0) {
                        $workingGroup.val(data.WorkingGroup_Id).trigger('change');
                    }

                    exists = $('#case__Priority_Id option[value=' + data.Priority_Id + ']').length;
                    if (exists > 0 && data.Priority_Id > 0) {
                        $("#case__Priority_Id").val(data.Priority_Id);
                        $("#case__Priority_Id").change();
                    }

                    $("#ProductAreaHasChild").val(data.HasChild);
                }
            });
        }
    });

    $('#case__WorkingGroup_Id').change(function () {
        // Remove after implementing http://redmine.fastdev.se/issues/10995
        // filter administrators
        var DontConnectUserToWorkingGroup = $('#CaseMailSetting_DontConnectUserToWorkingGroup').val();
        if (DontConnectUserToWorkingGroup == 0) {
            CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeWorkingGroupFilterUser/', '#Performer_Id', $('#DepartmentFilterFormat').val());
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
        $(publicOUControlName).val(val).trigger('change');
    });

    $('#divProductArea ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
        $("#case__ProductArea_Id").val(val).trigger('change');;
    });

    $('#AddNotifier').click(function (e) {
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            return false;
        }

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
        if ($("#case__PersonsCellphone").val() != '')
            params += "&cellPhone=" + $("#case__PersonsCellphone").val();
        if ($("#case__Region_Id").val() != '')
            params += "&regionId=" + $("#case__Region_Id").val();
        if ($(publicDepartmentControlName).val() != '')
            params += "&departmentId=" + $(publicDepartmentControlName).val();
        if ($(publicOUControlName).val() != '')
            params += "&organizationUnitId=" + $(publicOUControlName).val();

        var win = window.open('/Notifiers/NewNotifierPopup' + params, '_blank', 'left=100,top=100,width=990,height=480,toolbar=0,resizable=1,menubar=0,status=0,scrollbars=1');

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
            // Raise event about rendering of uploaded file
            $(document).trigger("OnUploadedCaseFileRendered", []);
            bindDeleteCaseFileBehaviorToDeleteButtons();
        });
    };

    var getLogFiles = function () {
        $.get('/Cases/LogFiles', { id: $('#LogKey').val(), now: Date.now() }, function (data) {
            $('#divCaseLogFiles').html(data);
            // Raise event about rendering of uploaded file
            $(document).trigger("OnUploadedCaseLogFileRendered", []);
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
                max_file_size: '35mb',
            },
            buttons: { browse: true, start: true, stop: true, cancel: true },
            preinit: {
                Init: function (up, info) {
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

    if (window.LogInitForm != null) {
        LogInitForm();
    }

    bindDeleteCaseFileBehaviorToDeleteButtons();
    SetFocusToReportedByOnCase();
}