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

function replaceLinebreaksInString(input) {
    return input.replace(/\r\n|\r|\n/g, "<br />");
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

    //Self service selector(Not the same as HD)
    $('ul.dropdown-menu.subddMenu.parentddMenu').prevAll('button').first().on('click', function () {
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

initDynamicDropDowns();
initDynamicDropDownsKeysBehaviour();

