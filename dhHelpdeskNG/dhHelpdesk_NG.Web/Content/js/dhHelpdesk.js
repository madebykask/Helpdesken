
//tabbar
$('.nav-tabs a').click(function(e) {
    e.preventDefault();
    $(this).tab('show');
    //activeTab.val($(this).attr('href'));
});

$("input:text:visible:first").focus();

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

// Cose window or tab
function close_window() {
    //if (confirm("WARNING TEXT XXXXXXX TRANSLATE")) {
        close();
    //}
}

function CaseCascadingSelectlistChange(id, customerId, postTo, ctl, departmentFilterFormat) {
    var ctlOption = ctl + ' option';
    $.post(postTo, { 'id': id, 'customerId': customerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {
        $(ctlOption).remove();
        $(ctl).append('<option>&nbsp;</option>');
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

function CaseInitForm() {

    $('#CaseLog_TextExternal').focus(function () {
        CaseWriteTextToLogNote('');
    });

    $('#CaseLog_TextInternal').focus(function () {
        CaseWriteTextToLogNote('internal');
    });

    $('#case__ReportedBy').typeahead(GetComputerUserSearchOptions());
    //$('#case__InventoryNumber').typeahead(GetComputerSearchOptions());

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
        $("#case__CaseType_Id").val(val);
    });

    $('#divProductArea ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
        $("#case__ProductArea_Id").val(val);
    });

    $('#divFinishingType ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_FinishingType").text(getBreadcrumbs(this));
        $("#CaseLog_FinishingType").val(val);
        if (val == "") {
            $("#CaseLog_FinishingDate").val('');
        }
        else {
            if ($("#CaseLog_FinishingDate").val() == '') {
                $("#CaseLog_FinishingDate").val(today());
            }
        }

    });

    $('#file_uploader').pluploadQueue({
        url: '@this.Url.Action("UploadCaseFile")',
        multipart_params: { caseId: '@this.Model.case_.Id' },

        init: {
            FileUploaded: function () {
                $.get('@this.Url.Action("Files")', { caseId: '@this.Model.case_.Id' }, function (files) {
                    refreshCaseFilesTable(files);
                    $('#no_uploaded_files_label').hide();
                });
            },

            Error: function (uploader, e) {
                if (e.status != 409) {
                    return;
                }
                window.alreadyExistFileIds.push(e.file.id);
            },

            StateChanged: function (uploader) {
                if (uploader.state != plupload.STOPPED) {
                    return;
                }

                for (var i = 0; i < window.alreadyExistFileIds.length; i++) {
                    var fileId = window.alreadyExistFileIds[i];
                    $('#file_uploader ul[class="plupload_filelist"] li[id="' + fileId + '"] div[class="plupload_file_action"] a').prop('title', '@Translation.Get("File already exists.", Enums.TranslationSource.TextTranslation)');
                }

                window.alreadyExistFileIds.length = 0;
                uploader.refresh();
            }
        }
    });

    function refreshCaseFilesTable(files) {
        $('#files_table > tbody > tr').remove();

        for (var i = 0; i < files.length; i++) {
            var file = files[i];

            var fileMarkup =
                $('<tr>' +
                    '<td>' +
                    '<a href="@this.Url.Action("DownloadFile")?@(!string.IsNullOrEmpty(this.Model.case_.Id) ? "fileId=" + this.Model.case_.Id + "&" : string.Empty)' + 'fileName=' + file + '">' + file + '</a>' +
                    '</td>' +
                    '<td>' +
                    '<a id="delete_file_button_' + i + '" class="btn">@Translation.Get("Delete", Enums.TranslationSource.TextTranslation)</a>' +
                    '</td>' +
                    '</tr>');

            $('#files_table > tbody').append(fileMarkup);
        }

        bindDeleteCaseFileBehaviorToDeleteButtons();
    }

    function bindDeleteCaseFileBehaviorToDeleteButtons() {
        $('#files_table a[id^="delete_file_button_"]').click(function () {
            var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
            var pressedDeleteFileButton = this;

            $.post('@this.Url.Action("DeleteCaseFile")', { Id: '@this.Model.case_.Id', fileName: fileName }, function () {
                $(pressedDeleteFileButton).parents('tr:first').remove();
            });
        });
    }

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
            return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.email.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.cellphone.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.place.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.name.toLowerCase().indexOf(this.query.toLowerCase());
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
            var result = item.num + ' - ' + item.location + ' - ' + item.computertype;

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

//Datepicker
$('.date').datepicker({
    format: "yyyy-mm-dd",
    minViewMode: "days",
    startView: "month"
});


//multiselct med sök
$('.multiselect').multiselect({
    enableFiltering: true,
    filterPlaceholder: 'Sök',
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


