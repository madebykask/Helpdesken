/**
*
* This code was taken from dhHelpdesk.js and still has dependencies from it
* @TODO: code review wanted to spare connections between functions from that file
*/
var publicCustomerId = $('#case__Customer_Id').val();

var publicDepartmentControlName = '#case__Department_Id';
var publicReadOnlyDepartmentName = '#DepartmentName';

var publicOUControlName = '#case__Ou_Id';
var publicReadOnlyOUName = '#OuName';

// Case Is About
var publicIsAboutDepartmentControlName = '#case__IsAbout_Department_Id';
var publicIsAboutReadOnlyDepartmentName = '#IsAboutDepartmentName';

var publicIsAboutOUControlName = '#case__IsAbout_Ou_Id';
var publicIsAboutReadOnlyOUName = '#IsAboutOUName';


// controller methods:
var publicChangeRegion = '/Cases/ChangeRegion/';
var publicChangeDepartment = '/Cases/ChangeDepartment/';

var skipRefreshOU = false;
var skipRefreshIsAbout_OU = false;

var lastInitiatorSearchKey = ''
var lastIsAboutSearchKey = '';

function SetFocusToReportedByOnCase() {
    if ($('#ShowReportedBy').val() == 1) {
        $('#case__ReportedBy').focus();
    }
}

$(document).ready(function () {

    $('#AddNotifier')[0].prevState = $('#AddNotifier').is(':visible');

    var initiatorId = $('#InitiatorCategory').val();
    if (initiatorId) {
        var initiatorCategory = window.parameters.computerUserCategories[initiatorId];
        var intiatorReadOnly = initiatorCategory == null ? false : initiatorCategory.IsReadOnly;
        applyReadOnlyOn(readOnlyExpressions['initiator'], intiatorReadOnly);

        if (intiatorReadOnly) {
            $('#AddNotifier').hide();
        }
    }

    var regardingId = $('#IsAboutCategory').val();
    if (regardingId) {
        var regardingCategory = window.parameters.computerUserCategories[regardingId];
        var regardingReadOnly = regardingCategory == null ? false : regardingCategory.IsReadOnly;
        applyReadOnlyOn(readOnlyExpressions['regarding'], regardingReadOnly);
    }

    if (window.parameters.hasExtendedComputerUsers === "True") {
        initExtendedComputerUserSections();
    }
});

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
                            , num: item.Name
                            , location: item.Location
                            , computertype: item.TypeDescription
                            , ctype: item.TypeName
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
            var result = item.ctype + ": " + item.num + ' - ' + item.location + ' - ' + (item.computertype == null ? ' ' : item.computertype);

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
    var $publicOUControlName = $(publicOUControlName);
    var $publicReadOnlyOUName = $(publicReadOnlyOUName);

    //$publicOUControlName.val(''); // breaks validation on case save
    $publicReadOnlyOUName.val('');

    $.get(publicChangeDepartment, { 'id': departmentId, 'customerId': publicCustomerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {

        var selectedItem = null;
        var options = '<option value="">&nbsp;</option>';
        if (data) {
            for (var i = 0; i < data.list.length; i++) {
                var item = data.list[i];

                if (item.id == selectedOrganizationUnitId) {
                    selectedItem = item;
                    options += "<option value='" + item.id + "' selected>" + item.name + "</option>";
                } else {
                    options += "<option value='" + item.id + "'>" + item.name + "</option>";
                }
            }
        }

        $publicOUControlName.empty();
        $publicOUControlName.append(options);

        if (selectedItem) {
            $publicOUControlName.val(selectedItem.id);
            $publicReadOnlyOUName.val(selectedItem.name);
        }
    }, 'json').always(function () {
        //$(publicOUControlName).prop('disabled', false);
    });
}

function refreshIsAboutOrganizationUnit(departmentId, departmentFilterFormat, selectedOrganizationUnitId) {
    $(publicIsAboutOUControlName).val('');
    $(publicIsAboutReadOnlyOUName).val('');
    var ctlOption = publicIsAboutOUControlName + ' option';
    $.get(publicChangeDepartment, { 'id': departmentId, 'customerId': publicCustomerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {
        $(ctlOption).remove();
        $(publicIsAboutOUControlName).append('<option value="">&nbsp;</option>');
        if (data != undefined) {
            for (var i = 0; i < data.list.length; i++) {
                var item = data.list[i];
                var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                if (option.val() == selectedOrganizationUnitId) {
                    $(publicIsAboutOUControlName).val(selectedOrganizationUnitId);
                    $(publicIsAboutReadOnlyOUName).val(item.name);
                    option.prop("selected", true);
                }
                $(publicIsAboutOUControlName).append(option);
            }
        }
    }, 'json').always(function () {
        //$(publicIsAboutOUControlName).prop('disabled', false);
    });
}

function refreshDepartment(regionId, departmentFilterFormat, selectedDepartmentId, selectedOU) {
    var $publicDepartmentControlName = $(publicDepartmentControlName);
    var $publicReadOnlyDepartmentName = $(publicReadOnlyDepartmentName);

    $publicDepartmentControlName.val('');
    $publicReadOnlyDepartmentName.val('');

    $publicDepartmentControlName.prop('disabled', true);
    $(publicOUControlName).prop('disabled', true);

    $.get(publicChangeRegion, { 'id': regionId, 'customerId': publicCustomerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {

        // Don't disable the dropdown only disable non selected options
        if (IsInitiatorCategoryReadOnly()) {
            $publicDepartmentControlName.removeAttr('disabled');
        }

        var options = '<option value="">&nbsp;</option>';
        var selectedItem = null;

        if (data) {
            for (var i = 0; i < data.list.length; i++) {
                var item = data.list[i];
                if (item.id == selectedDepartmentId) {
                    selectedItem = item;
                    options += "<option value='" + item.id + "' selected>" + item.name + "</option>";
                } else {
                    options += "<option value='" + item.id + "'>" + item.name + "</option>";
                }
            }
        }

        $publicDepartmentControlName.empty();
        $publicDepartmentControlName.append(options);

        if (selectedItem) {
            skipRefreshOU = true;
            $publicDepartmentControlName.val(selectedItem.id);
            $publicReadOnlyDepartmentName.val(selectedItem.name);
            $publicDepartmentControlName.trigger('change');
        }
    }, 'json').always(function () {
        $publicDepartmentControlName.prop('disabled', false);
        skipRefreshOU = false;
        refreshOrganizationUnit(selectedDepartmentId, departmentFilterFormat, selectedOU);
    }).done(function () {
        $(publicOUControlName).prop('disabled', false);
    });
}

function refreshIsAboutDepartment(regionId, departmentFilterFormat, selectedDepartmentId, selectedOU) {
    $(publicIsAboutDepartmentControlName).val('');
    $(publicIsAboutReadOnlyDepartmentName).val('');
    var ctlOption = publicIsAboutDepartmentControlName + ' option';
    $(publicIsAboutDepartmentControlName).prop('disabled', true);
    $(publicIsAboutOUControlName).prop('disabled', true);
    $.get(publicChangeRegion, { 'id': regionId, 'customerId': publicCustomerId, 'departmentFilterFormat': departmentFilterFormat }, function (data) {
        $(ctlOption).remove();
        if (IsAboutCategoryReadOnly()) {
            $(publicIsAboutDepartmentControlName).removeAttr('disabled');
        }
        $(publicIsAboutDepartmentControlName).append('<option value="">&nbsp;</option>');
        if (data != undefined) {
            for (var i = 0; i < data.list.length; i++) {
                var item = data.list[i];
                var option = $("<option value='" + item.id + "'>" + item.name + "</option>");
                if (option.val() == selectedDepartmentId) {
                    $(publicIsAboutDepartmentControlName).val(selectedDepartmentId);
                    $(publicIsAboutReadOnlyDepartmentName).val(item.name);
                    option.prop("selected", true);
                    $(publicIsAboutDepartmentControlName).append(option);
                    skipRefreshIsAbout_OU = true;
                    $(publicIsAboutDepartmentControlName).trigger('change');
                }
                else
                    $(publicIsAboutDepartmentControlName).append(option);
            }

            //if (IsAboutCategoryReadOnly()) {
            //    $(publicIsAboutDepartmentControlName).find('option:not(:selected)').attr('disabled', 'disabled');
            //}
            //else {
            //    $(publicIsAboutDepartmentControlName).find('option').removeAttr('disabled');
            //}
        }
    }, 'json').always(function () {
        if (!IsRegardingCategoryReadOnly()) {
            $(publicIsAboutDepartmentControlName).prop('disabled', false);
        }
        skipRefreshIsAbout_OU = false;
        refreshIsAboutOrganizationUnit(selectedDepartmentId, departmentFilterFormat, selectedOU);
    }).done(function () {
        if (!IsRegardingCategoryReadOnly()) {
            $(publicIsAboutOUControlName).prop('disabled', false);
        }
    });
}

function IsInitiatorCategoryReadOnly() {
    var categoryID, category;
    categoryID = $('#InitiatorCategory').val();
    if (categoryID) {
        category = window.parameters.computerUserCategories[categoryID];
    }
    if (category != null && category.IsReadOnly)
        return true;
    return false;
}

function IsAboutCategoryReadOnly() {
    var categoryID, category;
    categoryID = $('#IsAboutCategory').val();
    if (categoryID != 'null') {
        category = window.parameters.computerUserCategories[categoryID];
    }
    if (category != null && category.IsReadOnly)
        return true;
    return false;
}


function IsRegardingCategoryReadOnly() {
    var categoryID, category;
    categoryID = $('#IsAboutCategory').val();
    if (categoryID != 'null') {
        category = window.parameters.computerUserCategories[categoryID];
    }
    if (category != null && category.IsReadOnly)
        return true;
    return false;
}


function generateRandomKey() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + '-' + s4() + '-' + s4();
}

var readOnlyExpressions = {
    'initiator': [
        '#case__PersonsName',
        '#case__PersonsPhone',
        '#case__PersonsCellphone',
        '#case__Place',
        '#case__UserCode',
        '#case__CostCentre',
        '#AddNotifier'
    ],
    'regarding': [
        '#case__IsAbout_Person_Name',
        '#case__IsAbout_Person_Phone'
        /*'#case__IsAbout_Region_Id',
        '#case__IsAbout_Department_Id',
        '#case__IsAbout_Ou_Id',
        '#case__IsAbout_Person_Email'*/
    ]
}


function GetComputerUserSearchOptions() {

    var delayFunc = CommonUtils.createDelayFunc();

    var options = {
        items: 20,
        minLength: 2,
        source: function (query, process) {
            lastInitiatorSearchKey = generateRandomKey();
            delayFunc(function () {
                //console.log('GetComputerUserSearchOptions: running ajax request');

                var categoryID = null;
                var initiatorCategory = $('#InitiatorCategory');
                if (initiatorCategory.length > 0)
                    categoryID = initiatorCategory.val()

                $.ajax({
                    url: '/cases/search_user',
                    type: 'post',
                    data: { query: query, customerId: $('#case__Customer_Id').val(), searchKey: lastInitiatorSearchKey, categoryID: categoryID },
                    dataType: 'json',
                    success: function (result) {
                        if (result.searchKey != lastInitiatorSearchKey) {
                            return;
                        }

                        var resultList = jQuery.map(result.result, function (item) {
                            var aItem = {
                                id: item.Id
                                , num: item.UserId
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
                                , customername: item.CustomerName
                                , costcentre: item.CostCentre
                                , isReadOnly: item.IsReadOnly
                                , categoryID: item.CategoryID
                                , categoryName: item.CategoryName
                            };
                            return JSON.stringify(aItem);
                        });
                        if (resultList.length === 0) {
                            var noRes = {
                                name: window.parameters.noResultLabel,
                                isNoResult: true
                            }
                            resultList.push(JSON.stringify(noRes));
                        }

                        process(resultList);
                    }
                });
            }, 300);
        },

        matcher: function (obj) {
            var item = JSON.parse(obj);
            if (~item.isNoResult) {
                return 1;
            }
            //console.log(JSON.stringify(item));
            return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.name_family.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.email.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.usercode.toLowerCase().indexOf(this.query.toLowerCase());
        },

        sorter: function (items) {
            var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
            while (aItem = items.shift()) {
                var item = JSON.parse(aItem);
                if (!item.isNoResult) {
                    if (!item.num
                        .toLowerCase()
                        .indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                    else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                    else caseInsensitive.push(JSON.stringify(item));
                }
                else caseInsensitive.push(JSON.stringify(item));
            }
            return beginswith.concat(caseSensitive, caseInsensitive);
        },

        highlighter: function (obj) {
            var item = JSON.parse(obj);
            if (item.isNoResult) {
                return item.name;
            }
            var orgQuery = this.query;
            if (item.departmentname == null)
                item.departmentname = ""
            var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
            var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;
            var resultBy_NameFamily = item.name_family + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;

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
            if (!item.isNoResult) {
                var departmentFilterFormat = $('#DepartmentFilterFormat').val();

                $('#case__ReportedBy').val(item.num);

                // Raise event about UserId changed.
                $(document).trigger("OnUserIdChanged", [item.num]);

                if (item.name != "" && item.name != null)
                    $('#case__PersonsName').val(item.name);

                if (item.email != "" && item.email != null)
                    $('#case__PersonsEmail').val(item.email);

                if (item.phone != null)
                    $('#case__PersonsPhone').val(item.phone);

                if (item.cellphone != null)
                    $('#case__PersonsCellphone').val(item.cellphone);

                if (item.place != null)
                    $('#case__Place').val(item.place);

                if (item.usercode != null)
                    $('#case__UserCode').val(item.usercode);

                if (item.costcentre != null)
                    $('#case__CostCentre').val(item.costcentre);

                if (item.regionid != "" && item.regionid != null) {
                    // SHould not be rad only anymore, todo: find better way to configure read only per category
                    //if (IsInitiatorCategoryReadOnly()) {
                    //    $('#case__Region_Id').removeAttr('disabled')
                    //    $('#case__Region_Id').val(item.regionid);
                    //    $('#case__Region_Id').find('option:selected').removeAttr('disabled');
                    //    $('#case__Region_Id').find('option:not(:selected)').attr('disabled', 'disabled');
                    //}
                    //else {
                    //    $('#case__Region_Id').find('option').removeAttr('disabled');
                    //    $('#case__Region_Id').val(item.regionid);
                    //}
                    $('#case__Region_Id').val(item.regionid);
                    $('#RegionName').val(item.regionname);
                }

                if (item.regionid != "" &&
                    item.regionid != null &&
                    item.departmentid != "" &&
                    item.departmentid != null) {
                    //$(publicDepartmentControlName).val(item.departmentid).trigger('change');
                    refreshDepartment(item.regionid, departmentFilterFormat, item.departmentid, item.ouid);
                }

                // Category marked as readonly (lock part from edit)
                applyReadOnlyOn(readOnlyExpressions['initiator'], item.isReadOnly);

                var initiatorSectionType = 0;
                loadExtendedCaseSectionIfExist(item.categoryID, initiatorSectionType);

                //Get PCNumber by UserId
                if (item.id != null) {
                    setPcNumber(item.id);
                }


                return item.num;
            }
            return this.query;
        }
    };

    return options;
}

function initExtendedComputerUserSections() {
    var iframeOptions = getExtendedCaseSectionIFrameOptions();
    $('iframe[id^="extendedSection-iframe"]').iFrameResize(iframeOptions);
}

function loadExtendedCaseSectionIfExist(categoryId, sectionType) {

    var inputData = {
        categoryID: categoryId,
        caseSectionType: sectionType // 0 - Initiator, 1 - Regarding
    };

    if (categoryId != null) {
        $.ajax({
            url: '/cases/GetExtendedCaseUrlForCategoryAndSection',
            type: 'POST',
            data: inputData,
            dataType: 'json',
            success: function (ext) {
                // TODO: set part dynamically
                if (ext.url && ext.url.length) {
                    loadExtendedCaseSection(ext.url, ext.guid, sectionType);
                }
                else {
                    resetExtendedCaseSection(sectionType);
                }
            }
        });
    }
    else {
        resetExtendedCaseSection(sectionType);
    }
}

function loadExtendedCaseSection(extendedFormUrl, userGuid, sectionType) {
    var elements = getExCaseSectionElements(sectionType);
    elements.$frame.css('height', '180px'); // restore orignial height
    elements.$userGuidHidden.val(userGuid);
    elements.$frame.attr('src', extendedFormUrl);
    elements.$exCaseSection.show();
}

function resetExtendedCaseSection(sectionType) {
    var elements = getExCaseSectionElements(sectionType);
    elements.$userGuidHidden.val('');
    elements.$exCaseSection.hide();
    elements.$frame.attr('src', '');
}

function getExCaseSectionElements(sectionType) {
    // 0 - Initiator, 1 - Regarding
    var elements = Number(sectionType) === 0
        ? {
            $exCaseSection: $('#extendedSection-Initiator'),
            $frame: $('#extendedSection-iframe-Initiator'),
            $userGuidHidden: $('#ExtendedInitiatorGUID')
        }
        : {
            $exCaseSection: $('#extendedSection-Regarding'),
            $frame: $('#extendedSection-iframe-Regarding'),
            $userGuidHidden: $('#ExtendedRegardingGUID')
        };
    return elements;
}

function getExtendedCaseSectionIFrameOptions() {
    var iframeOptions = {
        log: true,
        sizeHeight: true,
        checkOrigin: false,
        enablePublicMethods: true,
        resizedCallback: function (messageData) {
            //console.log('>>>iFrame sesize callback called.!');
        },
        bodyMargin: '0 0 0 0',
        closedCallback: function (id) { },
        heightCalculationMethod: 'grow'
    };
    return iframeOptions;
}

function applyReadOnlyOn(readOnlyExpressions, readOnly) {
    var combined = readOnlyExpressions.join();
    if (readOnly) {
        $(combined).each(function (index, value) {
            if (!value.prevReadOnlyState) {
                value.prevReadOnlyState = { readonly: $(value).prop('readonly'), disabled: $(value).prop('disabled') };
            }
            if ($(value).is('select')) {
                $(value).removeAttr('disabled');
                $(value).find('option:not(:selected)').attr('disabled', 'disabled');
            }
            else {
                $(value).prop('readonly', true);
            }
        });
    }
    else {
        $(combined).each(function (index, value) {

            if (value.prevReadOnlyState) {
                if ($(value).is('select')) {
                    if (value.prevReadOnlyState.disabled) {
                        $(value).prop('disabled', true);
                    }
                    $(value).find('option:not(:selected)').removeAttr('disabled');
                }
                else {
                    if (!value.prevReadOnlyState.readonly) {
                        $(value).removeAttr('readonly');
                    }
                    if (!value.prevReadOnlyState.disabled) {
                        $(value).removeAttr('disabled');
                    }
                }

            }
        });
    }
}

function GetComputerUserSearchOptionsForIsAbout() {
    var delayFunc = CommonUtils.createDelayFunc();

    var options = {
        items: 20,
        minLength: 2,
        source: function (query, process) {
            lastIsAboutSearchKey = generateRandomKey();

            delayFunc(function () {

                var categoryID = null;
                var isAboutCategory = $('#IsAboutCategory');
                if (isAboutCategory.length > 0)
                    categoryID = isAboutCategory.val();

                $.ajax({
                    url: '/cases/search_user',
                    type: 'post',
                    data: { query: query, customerId: $('#case__Customer_Id').val(), searchKey: lastIsAboutSearchKey, categoryID: categoryID },
                    dataType: 'json',
                    success: function (result) {
                        if (result.searchKey != lastIsAboutSearchKey)
                            return;

                        var resultList = jQuery.map(result.result, function (item) {
                            var aItem = {
                                id: item.Id
                                , num: item.UserId
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
                                , customername: item.CustomerName
                                , costcentre: item.CostCentre
                                , isReadOnly: item.IsReadOnly
                                , categoryID: item.CategoryID
                                , categoryName: item.CategoryName

                            };
                            return JSON.stringify(aItem);
                        });
                        if (resultList.length === 0) {
                            var noRes = {
                                name: window.parameters.noResultLabel,
                                isNoResult: true
                            }
                            resultList.push(JSON.stringify(noRes));
                        }

                        return process(resultList);
                    }
                });
            },
                300);
        },

        matcher: function (obj) {
            var item = JSON.parse(obj);
            if (~item.isNoResult) {
                return 1;
            }
            //console.log(JSON.stringify(item));
            return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.name_family.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.email.toLowerCase().indexOf(this.query.toLowerCase())
                || ~item.usercode.toLowerCase().indexOf(this.query.toLowerCase());
        },

        sorter: function (items) {
            var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
            while (aItem = items.shift()) {
                var item = JSON.parse(aItem);
                if (!item.isNoResult) {
                    if (!item.num
                        .toLowerCase()
                        .indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                    else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                    else caseInsensitive.push(JSON.stringify(item));
                } else caseInsensitive.push(JSON.stringify(item));
            }

            return beginswith.concat(caseSensitive, caseInsensitive);
        },

        highlighter: function (obj) {
            var item = JSON.parse(obj);
            if (item.isNoResult) {
                return item.name;
            }
            var orgQuery = this.query;
            if (item.departmentname == null)
                item.departmentname = ""
            var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
            var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;
            var resultBy_NameFamily = item.name_family + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;

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
            if (!item.isNoResult) {
                var regardingSectionType = 1;
                var departmentFilterFormat = $('#DepartmentFilterFormat').val();

                $('#case__IsAbout_ReportedBy').val(item.num);

                // Raise event about UserId changed.
                $(document).trigger("OnUserIdChanged", [item.num, regardingSectionType]);


                // Case Is About
                if (item.name != "" && item.name != null)
                    $('#case__IsAbout_Person_Name').val(item.name);

                if (item.email != null)
                    $('#case__IsAbout_Person_Email').val(item.email);

                if (item.phone != null)
                    $('#case__IsAbout_Person_Phone').val(item.phone);

                if (item.cellphone != null)
                    $('#case__IsAbout_Person_Cellphone').val(item.cellphone);

                if (item.place != null)
                    $('#case__IsAbout_Place').val(item.place);

                if (item.usercode != null)
                    $('#case__IsAbout_UserCode').val(item.usercode);

                if (item.costcentre != null)
                    $('#case__IsAbout_CostCentre').val(item.costcentre);

                if (item.regionid != "" && item.regionid != null) {
                    // Should not be read only, TODO: proper configuration of read only values
                    //if (IsAboutCategoryReadOnly()) {
                    //    $('#case__IsAbout_Region_Id').removeAttr('disabled')
                    //    $('#case__IsAbout_Region_Id').val(item.regionid);
                    //    $('#case__IsAbout_Region_Id').find('option:selected').removeAttr('disabled');
                    //    $('#case__IsAbout_Region_Id').find('option:not(:selected)').attr('disabled', 'disabled');
                    //}
                    //else {
                    //    $('#case__IsAbout_Region_Id').find('option').removeAttr('disabled');
                    //    $('#case__IsAbout_Region_Id').val(item.regionid);
                    //}
                    $('#case__IsAbout_Region_Id').val(item.regionid);
                    $('#IsAboutRegionName').val(item.regionname);
                }

                if (item.regionid != "" &&
                    item.regionid != null &&
                    item.departmentid != "" &&
                    item.departmentid != null) {
                    refreshIsAboutDepartment(item.regionid, departmentFilterFormat, item.departmentid, item.ouid);
                }

                // Category marked as readonly (lock part from edit)
                applyReadOnlyOn(readOnlyExpressions['regarding'], item.isReadOnly);

                /*if (item.categoryID != null){
                    window.page.loadExtendedInitiator();
                }*/

                loadExtendedCaseSectionIfExist(item.categoryID, regardingSectionType);

                return item.num;
            }
            return this.query;
        }
    };

    return options;
}

function setPcNumber(userId) {
    $.ajax({
        url: '/cases/Search_PcNumber',
        type: 'post',
        data: { userId: userId, customerId: $('#case__Customer_Id').val() },
        dataType: 'json',
        success: function (result) {
            if (result != null) {
                if (result.Name != null || result.Name != "")
                    $('#case__InventoryNumber').val(result.Name);
                $('#case__InventoryType').val(result.TypeDescription);
                $('#case__InventoryLocation').val(result.Location);
                $("#ShowInventoryBtn").show();
            }
        }
    });

}

/**
* Initializator for case edit form
*/
function CaseInitForm(opt) {

    var self = this;
    var options = opt || { twoAttacmentsMode: false, fileUploadWhiteList: null };
    var twoAttachmentsMode = opt.twoAttacmentsMode;


    $("#CaseLog_TextExternal").on("summernote.focus", function (e) {
        CaseWriteTextToLogNote('');
    });

    $("#CaseLog_TextInternal").on("summernote.focus", function (e) {
        CaseWriteTextToLogNote('internal');
    });

    $('#case__ReportedBy').typeahead(GetComputerUserSearchOptions());

    $('#case__InventoryNumber').typeahead(GetComputerSearchOptions());
    $('#case__IsAbout_ReportedBy').typeahead(GetComputerUserSearchOptionsForIsAbout());

    $('#CountryId').change(function () {
        CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeCountry/', '#case__Supplier_Id', $('#DepartmentFilterFormat').val());
    });

    // Remove after implementing http://redmine.fastdev.se/issues/10995
    $('#case__Region_Id').change(function () {
        var regionId = $(this).val();
        var departmentFilterFormat = $('#DepartmentFilterFormat').val();

        var templateDep_Id = $("#CaseTemplate_Department_Id").val();
        $("#CaseTemplate_Department_Id").val('');

        if (templateDep_Id != undefined && templateDep_Id != "") {
            var templateOU_Id = $("#CaseTemplate_OU_Id").val();
            $("#CaseTemplate_OU_Id").val('');
            if (templateOU_Id != undefined && templateOU_Id != "")
                refreshDepartment(regionId, departmentFilterFormat, templateDep_Id, templateOU_Id);
            else
                refreshDepartment(regionId, departmentFilterFormat, templateDep_Id);
        } else {
            refreshDepartment(regionId, departmentFilterFormat);
        }

    });

    $('#case__IsAbout_Region_Id').change(function () {
        var regionId = $(this).val();
        var departmentFilterFormat = $('#DepartmentFilterFormat').val();
        var templateDep_Id = $("#CaseTemplate_IsAbout_Department_Id").val();
        $("#CaseTemplate_IsAbout_Department_Id").val('');

        if (templateDep_Id != undefined && templateDep_Id != "") {
            var templateOU_Id = $("#CaseTemplate_IsAbout_OU_Id").val();
            $("#CaseTemplate_IsAbout_OU_Id").val('');
            if (templateOU_Id != undefined && templateOU_Id != "")
                refreshIsAboutDepartment(regionId, departmentFilterFormat, templateDep_Id, templateOU_Id);
            else
                refreshIsAboutDepartment(regionId, departmentFilterFormat, templateDep_Id);
        } else {
            refreshIsAboutDepartment(regionId, departmentFilterFormat);
        }
    });

    $(publicDepartmentControlName).change(function () {
        // Remove after implementing http://redmine.fastdev.se/issues/10995        
        var departmentId = $(this).val();
        showInvoice(departmentId);

        if (skipRefreshOU)
            return;

        var departmentFilterFormat = $('#DepartmentFilterFormat').val();
        var templateOU_Id = $("#CaseTemplate_OU_Id").val();
        $("#CaseTemplate_OU_Id").val('');

        if (templateOU_Id != undefined && templateOU_Id != "")
            refreshOrganizationUnit(departmentId, departmentFilterFormat, templateOU_Id);
        else
            refreshOrganizationUnit(departmentId, departmentFilterFormat);
    });

    $(publicOUControlName).change(function () {
        var depId = $(publicDepartmentControlName).val();
        var ouId = $(this).val();
        showInvoice(depId, ouId);
    });

    $(publicIsAboutDepartmentControlName).change(function () {
        if (skipRefreshIsAbout_OU)
            return;

        var departmentId = $(this).val();
        var departmentFilterFormat = $('#DepartmentFilterFormat').val();

        var templateOU_Id = $("#CaseTemplate_IsAbout_OU_Id").val();
        $("#CaseTemplate_IsAbout_OU_Id").val('');
        if (templateOU_Id != undefined && templateOU_Id != "")
            refreshIsAboutOrganizationUnit(departmentId, departmentFilterFormat, templateOU_Id);
        else
            refreshIsAboutOrganizationUnit(departmentId, departmentFilterFormat);
    });

    function showInvoice(departmentId, ouId) {
        var _ouId = ouId == undefined ? null : ouId;
        var invoiceSelector = "#divInvoice, #btnCaseCharge, #tblCaseChargeSummary";
        var externalInvoiceSelector = "#divExternalInvoice, #totalExternalAmountRow, #externalInvoiceGrid";
        var invoiceFields = "#invoiceField-Time, #invoiceField-Overtime, #invoiceField-Material, #invoiceField-Price";

        $(invoiceSelector).hide();
        $(externalInvoiceSelector).hide();
        $(invoiceFields).hide();
        $.get("/Cases/GetDepartmentInvoiceParameters/", { departmentId: departmentId, ouId: _ouId }, function (data) {
            if (data) {

                //update values required for EditPage.js
                _parameters.departmentInvoiceMandatory = data.ChargeMandatory;
                _parameters.showInvoiceTime = data.ShowInvoiceTime;
                _parameters.showInvoiceOvertime = data.ShowInvoiceOvertime;

                if (data.Charge) {
                    $(invoiceSelector).show();
                }
                if (data.ShowInvoice) {
                    $(externalInvoiceSelector).show();
                }
                if (data.ShowInvoiceTime) {
                    $('#invoiceField-Time').show();
                }
                if (data.ShowInvoiceOvertime) {
                    $('#invoiceField-Overtime').show();
                }
                if (data.ShowInvoiceMaterial) {
                    $('#invoiceField-Material').show();
                }
                if (data.ShowInvoicePrice) {
                    $('#invoiceField-Price').show();
                }
            }
        }, "json");
    }

    $('#InitiatorCategory').change(function () {
        var id = $('#InitiatorCategory').val();

        var category = window.parameters.computerUserCategories[id];

        clearInitiator();

        if (category != null && category.IsReadOnly) {
            readOnlySection('initiator', true);
            $('#AddNotifier').hide();
        }
        else {
            //  $('#case__Region_Id').find('option').removeAttr('disabled');
            readOnlySection('initiator', false);
            if ($('#AddNotifier')[0].prevState) {
                $('#AddNotifier').show();
            }
        }
    });

    $('#IsAboutCategory').change(function () {
        var id = $('#IsAboutCategory').val();

        clearIsAbout();

        var category = window.parameters.computerUserCategories[id];
        if (category != null && category.IsReadOnly) {
            readOnlySection('regarding', true);
        }
        else {
            readOnlySection('regarding', false);
        }
    });


    $('#case__Status_Id').change(function (e) {
        console.log('>>> CaseStatus changed event.');
        var templateStateSecondartId = $('#CaseTemplate_StateSecondary_Id').val() || '';
        var templateWgId = $('#CaseTemplate_WorkingGroup_Id').val() || '';

        if ($(this).val() > 0) {
            $.post('/Cases/ChangeStatus/', { 'id': $(this).val() }, function (data) {
                if (data) {
                    //check if working group has been set by case template
                    if (templateWgId === '') {
                        changeWorkingGroupValue(data.WorkingGroup_Id, null, 'changeStatus');
                    }

                    var stateSecondaryId = Number(data.StateSecondary_Id);
                    if (stateSecondaryId > 0 && templateStateSecondartId === '') {
                        var exists = $('#case__StateSecondary_Id option[value=' + data.StateSecondary_Id + ']').length > 0;
                        if (exists) {
                            $("#case__StateSecondary_Id").val(data.StateSecondary_Id);
                            $(".readonlySubstate").val(data.StateSecondary_Id);
                        }
                    }
                }
            }, 'json');
        }
    });

    $('#case__CaseType_Id').change(function (e, source) {
        var caseTypeId = $(this).val();
        console.log('>>> CaseType changed event.');

        var templateWgId = $('#CaseTemplate_WorkingGroup_Id').val() || '';
        var templatePerformerId = $("#CaseTemplate_Performer_Id").val() || '';

        $.post('/Cases/ChangeCaseType/', { id: caseTypeId }).done(function (res) {
            if (res) {
                //set performer only if it was not set by case template before
                var $performerId = $('#Performer_Id');
                var performerUserId = Number(res.UserId);

                if (!isNaN(performerUserId) && performerUserId > 0 && templatePerformerId === '') {
                    var exists = $performerId.find('option[value=' + res.UserId + ']').length > 0;
                    if (!exists)
                        $performerId.append("<option value='" + res.UserId + "'>" + res.UserName + "</option>");
                    $performerId.val(res.UserId);
                }

                //chek if workginGroup has already been set by template load
                if (templateWgId === '') {
                    changeWorkingGroupValue(res.WorkingGroupId, res.WorkingGroupName, 'changeCaseType');
                }
            }
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == 404 || errorThrown == 'Not Found') {
                    console.warn('Case type (id=%s) not found.', caseTypeId);
                    ShowToastMessage('CaseType not found', 'warning', false);
                }
            });

        resetProductareaByCaseType(caseTypeId);
    });

    function resetProductareaByCaseType(caseTypeId) {
        var paId = parseInt($('#case__ProductArea_Id').val());
        $.post('/Cases/GetProductAreaByCaseType/', { caseTypeId: caseTypeId, customerId: publicCustomerId, myTime: Date.now(), productAreaIdToInclude: paId }, function (result) {
            if (result.success) {
                $('#divProductArea > ul.dropdown-menu').html("<li><a href='#'>--</a></li>" + result.data);
                if (result.praIds && result.praIds.indexOf(parseInt($('#case__ProductArea_Id').val())) < 0) {
                    var emptyElement = $('#divProductArea > ul.dropdown-menu').children().first();
                    $('#divBreadcrumbs_ProductArea').text(getBreadcrumbs(emptyElement));
                    $('#case__ProductArea_Id').val('').trigger('change');
                }
                bindProductAreasEvents();
            }
        }, 'json');
    }

    function bindProductAreasEvents() {
        $('#divProductArea ul.dropdown-menu li a').on('click', function (e) {
            e.preventDefault();
            onProductAreaChanged(this);
        });
        $('#divProductArea .dropdown-submenu.DynamicDropDown_Up').off('mousemove').on('mousemove', function (event) {
            dynamicDropDownBehaviorOnMouseMove(event.target.parentElement);
        });
    }

    function readOnlySection(sectionName, isReadOnly) {
        applyReadOnlyOn(readOnlyExpressions[sectionName], isReadOnly);
    }

    function clearInitiator() {
        // Clear fields except email (address should be preserved to allow notification)
        var fields = [
            '#case__PersonsName',
            '#case__PersonsPhone',
            '#case__PersonsCellphone',
            '#case__Place',
            '#case__UserCode',
            '#case__CostCentre',
            '#case__Region_Id',
            '#RegionName',
            '#case__Department_Id',
            '#case__Ou_Id',
            '#case__Region_Id',
            '#case__ReportedBy'
        ];

        var query = fields.join();
        $(query).val('');

        // reset initiator extended section
        var initiatorSectionType = 0;
        resetExtendedCaseSection(initiatorSectionType);
    }

    function clearIsAbout() {
        var fields = [
            '#case__IsAbout_Person_Name',
            '#case__IsAbout_Person_Phone',
            '#case__IsAbout_Region_Id',
            '#case__IsAbout_Department_Id',
            '#case__IsAbout_Ou_Id',
            '#case__IsAbout_Person_Email',
            '#case__IsAbout_ReportedBy'
        ];

        var query = fields.join();
        $(query).val('');

        // reset initiator extended section
        var regardingSectionType = 1;
        resetExtendedCaseSection(regardingSectionType);
    }

    function onProductAreaChanged(sender) {
        var me = sender;
        var val = $(me).attr('value');
        var curCaseId = $('#case__Id').val();
        var caseInvoiceModule = $('#CustomerSettings_ModuleCaseInvoice').val();
        var caseInvoiceIsActive = (caseInvoiceModule != undefined && caseInvoiceModule.toLowerCase() == 'true');

        /* When invoice is active, user can not change the product area while */
        if (caseInvoiceIsActive) {
            $.get('/CaseInvoice/IsThereNotSentOrder/',
                { caseId: curCaseId, myTime: Date.now },
                function (res) {
                    if (res != null && res) {
                        var mes = window.parameters.productAreaChangeMessage || '';
                        ShowToastMessage(mes, 'warning', false);
                    } else {
                        $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(me));
                        $("#case__ProductArea_Id").val(val).trigger('change');
                    }
                });
        } else {
            $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(me));
            $("#case__ProductArea_Id").val(val).trigger('change');
        }
    }

    $('#case__Priority_Id').change(function () {

        
        var isInheritingMode = $('#CaseTemplate_ExternalLogNote').val();
        if (isInheritingMode === 'True') {
            $('#CaseTemplate_ExternalLogNote').val('');
            return;
        }

        if ($('#CaseLog_TextExternal').summernote('isEmpty')) {
            $("#CaseLog_TextExternal").html('');
        }

        var textExternalLogNote = $('#CaseLog_TextExternal').html();

  
        $.post('/Cases/ChangePriority/', { 'id': $(this).val(), 'textExternalLogNote': textExternalLogNote }, function (data) {
            const $txtExternal = $('#CaseLog_TextExternal');
            if (data.ExternalLogText != null && data.ExternalLogText !== '') {
                var txt = data.ExternalLogText.replace(/\n/g, "<br />");
                txt = txt.replace("/&/g", "&amp");

                $txtExternal.summernote("code", txt);
                $txtExternal.trigger('propertychange');
            } else {
                $txtExternal.html();
                $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', false);
            }
        }, 'json');
    });

    $('#case__System_Id').change(function () {
        SelectValueInOtherDropdownOnChange($(this).val(), '/Cases/ChangeSystem/', '#case__Urgency_Id', null, true);
    });

    $('#case__Impact_Id').change(function () {
        SetPriority();
    });

    $('#case__Urgency_Id').change(function () {
        SetPriority();
    });

    // CaseLog_FinishingType Area change
    $('#CaseLog_FinishingType').change(function (e) {
        console.log('>>> CaseLog_FinishingType changed event.');
        $("#FinishingCauseHasChild").val(0);
        document.getElementById("divFinishingType").classList.remove("error");
        if ($(this).val() > 0) {
            $.post('/Cases/ChangeFinishingType/', { 'id': $(this).val() }, 'json')
                .done(function (data) {
                    if (data) {

                        var child = $('#FinishingCauseHasChild').val(data.HasChild);
                        $('#FinishingCauseHasChild').val(data.HasChild);

                    }
                });
        }
    });
    // Product Area change
    $('#case__ProductArea_Id').change(function (e) {
        console.log('>>> ProductArea changed event.');
        $("#ProductAreaHasChild").val(0);
        document.getElementById("divProductArea").classList.remove("error");

        var templateWgId = $('#CaseTemplate_WorkingGroup_Id').val() || '';
        var templatePriorityId = $('#CaseTemplate_Priority_Id').val() || '';

        if ($(this).val() > 0) {
            $.post('/Cases/ChangeProductArea/', { 'id': $(this).val() }, 'json')
                .done(function (data) {
                    if (data) {
                        //change only if it wasn't set by a template
                        if (templateWgId === '') {
                            changeWorkingGroupValue(data.WorkingGroup_Id, data.WorkingGroup_Name, 'changeProductArea');
                        }

                        var priorityId = Number(data.Priority_Id || '0');
                        var $casePriorityId = $('#case__Priority_Id');

                        if (priorityId > 0 && templatePriorityId === '') {
                            var exists = $casePriorityId.find('option[value=' + data.Priority_Id + ']').length > 0;
                            var $priorityName = $('#priority_Name');
                            if (exists || $priorityName.length) {
                                $casePriorityId.val(data.Priority_Id);
                                $casePriorityId.attr('data-sla', data.SLA);
                                $casePriorityId.change();
                            }

                            if ($priorityName.length && data.PriorityName) {
                                $priorityName.val(data.PriorityName);
                            }
                        }

                        if (priorityId > 0)
                            $('.sla-value').eq(0).val(data.Priority_Id);
                        $('#ProductAreaHasChild').val(data.HasChild);

                    }
                });
        }
    });

    function changeWorkingGroupValue(newWorkingGroupId, newWorkingGroupName, srcName) {
        console.log('>>> ChangeWorkingGroupValue: wgId %s, wgName: %s, src: %s', newWorkingGroupId, newWorkingGroupName, srcName);

        if (Number(newWorkingGroupId) > 0) {
            const $workingGroup = $('#case__WorkingGroup_Id');
            const $workingGroupName = $('#workingGroup_Name');
            const exists = $workingGroup.find('option[value=' + newWorkingGroupId + ']').length > 0;

            if (exists || $workingGroupName.length) {
                $workingGroup.val(newWorkingGroupId);
                if ($workingGroupName.length) {
                    $workingGroupName.val(newWorkingGroupName || '');
                }
                $workingGroup.change();
            }
        }
    }

    $('#case__WorkingGroup_Id').change(function (d, source) {
        console.log('>>> Working group changed event.');
        // Remove after implementing http://redmine.fastdev.se/issues/10995
        // filter administrators

        var dontConnectUserToWorkingGroup = Number($('#CaseMailSetting_DontConnectUserToWorkingGroup').val() || '0');
        if (dontConnectUserToWorkingGroup === 0) {
            CaseCascadingSelectlistChange($(this).val(), $('#case__Customer_Id').val(), '/Cases/ChangeWorkingGroupFilterUser/', '#Performer_Id', $('#DepartmentFilterFormat').val());
        } else {
            $('#Performer_Id').off('applyValue');
        }
        if (source !== 'case__StateSecondary_Id') {
            //set state secondary
            SelectValueInOtherDropdownOnChange($(this).val(),
                '/Cases/ChangeWorkingGroupSetStateSecondary/',
                '#case__StateSecondary_Id',
                '.readonlySubstate')
                .done(function () {
                    $('#case__StateSecondary_Id').trigger('change', 'case__WorkingGroup_Id');
                });
        }
    });


    $("#standardtextDropdownMenu div.case-div-standardtext").click(function () {
        addTextToLog($(this).children("span").html());
    });

    function addTextToLog(text) {

        
        var txt = "<p>" + text.replace(/\n/g, "</p><p>").replace("/&/g", "&amp") + "</p>";
        var writeTextToExternalNote = $("#WriteTextToExternalNote").val();
        var field = "#CaseLog_TextInternal";

        if (writeTextToExternalNote == "") {
            field = "#CaseLog_TextExternal";
        }
        
        if (txt.length > 1) {
            if ($($(field).summernote('code')).text() == "") {
                $(field).html('');
            }

            let htmlContent = $(field).summernote('code');
            htmlContent = htmlContent + txt;
            $(field).summernote('code', htmlContent.replace("<p><br></p>", ""));
            $(field).focus();
            $(field).trigger("propertychange");

            var input = $(field);
            input[0].selectionStart = input[0].selectionEnd = input.val().length;
        }
        $("#standardtextDropdownMenu").click();
    }

    $("#standardtextDropdownMenu").click(function () {
        $("#standardTextAccordion").accordion("activate", false);
    });

    $("#standardtextDropdownMenu div.standardtext-header").click(function (e) {
        var elId = $(this).attr("id").replace("stHeader_", "stText_");
        var text = $("#" + elId).html();
        addTextToLog(text);
        return false;
    });

    $("#standardTextAccordion").accordion({
        active: false,
        collapsible: true,
        heightStyle: "content",
        icons: { "header": "ui-icon-plus", "activeHeader": "ui-icon-minus" },
        header: "h3"
    });

    $("#standardtextDropdownMenu ul.case-standardText-keep-open").on("click", function (e) {
        e.stopPropagation();
    });

    // TODO: CHECK IF COMPATIBLE WITH ALL DropDown buttons and if should be moved to a separate file
    initDynamicDropDownsKeysBehaviour();

    //todo: check CaseTemplae
    $("ul.dropdown-menu li a").click(function (e) {
        //var toggler = $(this).parents("ul.dropdown-menu").prevAll("button.dropdown-toggle[data-toggle=dropdown]");
        var toggler = $(this).closest(".btn-group.open").find("button.dropdown-toggle[data-toggle=dropdown]");
        if (toggler.length) {
            toggler.focus();
        }
        return true;
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

    $(window).scroll(function () {
        var controls = $('.dropdown-menu-parent');
        for (var i = 0; i < controls.length; i++) {
            updateDropdownPosition(controls[i]);
            var objPos = getObjectPosInView(controls[i]);
            if (objPos.ToTop <= objPos.ToDown) {
                $(controls[i]).removeClass('dropup');
            } else {
                $(controls[i]).addClass('dropup');
            }
        }
    });

    bindProductAreasEvents();

    resetProductareaByCaseType($('#case__CaseType_Id').val());

    /*Show|hide invocie time on initiating*/
    if ($(publicDepartmentControlName) != undefined) {
        var depId = $(publicDepartmentControlName).val();
        var ouId = null;
        if ($(publicOUControlName) != undefined)
            ouId = $(publicOUControlName).val();

        showInvoice(depId, ouId);
    }

    $('#divCategory ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var me = this;
        var val = $(me).attr('value');
        $("#divBreadcrumbs_Category").text(getBreadcrumbs(me));
        $("#case__Category_Id").val(val).trigger('change');
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
        if ($("#case__CostCentre").val() != '')
            params += "&costcentre=" + $("#case__CostCentre").val();

        var userCategory = $("#InitiatorCategory").val() || '';
        if (userCategory != '') {
            params += "&userCategory=" + userCategory;
        }

        var win = window.open('/Notifiers/NewNotifierPopup' + params, '_blank', 'left=100,top=100,width=990,height=480,toolbar=0,resizable=1,menubar=0,status=0,scrollbars=1');

    });

    $('#AddFAQ').click(function (e) {
        e.preventDefault();

        var question = $('#case__Caption').val();
        var answer = $('#CaseLog_TextExternal').summernote('code');
        var internalanswer = $('#CaseLog_TextInternal').summernote('code');

        answer = stripHtml(answer);
        internalanswer = stripHtml(internalanswer);
        var win = window.open('/Faq/NewFAQPopup?question=' + question + '&answer=' + answer + '&internalanswer=' + internalanswer, '_blank', 'left=100,top=100,width=700,height=700,toolbar=0,resizable=1,menubar=0,status=0,scrollbars=1');
    });

    function stripHtml(html) {
        //Special from Summernote. Cant send html tags in query string but want to keep linebreaks //KA 20230601
        html = html.replace(/<\/p><p>/g, "<br>");
        html = html.replace(/<br>/g, "|");
       
        return html.replace(/<[^>]+>/g, "");
    }

    if (!Date.now) {
        Date.now = function () { return new Date().getTime(); };
    }

    var getCaseFiles = function () {
        $.get('/Cases/Files', { id: $('#CaseKey').val(), savedFiles: $('#SavedFiles').val(), now: Date.now() }, function (data) {
            $('#divCaseFiles').html(data);
            // Raise event about rendering of uploaded file
            $(document).trigger("OnUploadedCaseFileRendered", []);
            bindDeleteCaseFileBehaviorToDeleteButtons();
        });
    };

    var getLogFiles = function (isInternal) {
        const params = {
            id: $('#LogKey').val(),
            now: Date.now(),
            caseId: $("#case__Id").val(),
            isInternalLog: isInternal === true
        };
        $.get('/Cases/LogFiles', $.param(params), function (data) {
            var $divOutput = isInternal === true ? $('div.internalLog-files') : $('div.externalLog-files');
            $divOutput.html(data);
            $(document).trigger("OnUploadedCaseLogFileRendered", []);

            bindDeleteLogFileBehaviorToDeleteButtons(isInternal);
        });
    };

    var getFileName = function (fileName) {
        var strFiles = $('#CaseFileNames').val();
        var allFileNames = strFiles.split('|');

        var fn = fileName;

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
                        fileName = d + '-' + fn;
                    }
                } // for j
            }
        } // for i

        $('#CaseFileNames').val(strFiles + "|" + fileName);
        return fileName;
    }

    var invalidFileExtensionText = opt.invalidFileExtensionText;
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

    //////////////////////////////////////////////////////////////////////////
    //////// Attachments /////////////////////////////////////////////////////

    ////////  Clipboard File Upload //////////////////////////////////////////

    var clipboardFileUpload = null;

    ////a[href='#upload_clipboard_file_popup']
    $("a.uploadCaseFileFromClipboardBtn").on('click', function (e) {
        var opt = {
            fileKey: $('#CaseKey').val(),
            submitUrl: '/Cases/UploadCaseFile',
            refreshCallback: getCaseFiles
        };
        clipboardFileUpload = new ClipboardFileUpload(opt);
        clipboardFileUpload.show();
    });

    //a[href='#upload_clipboard_file_popup']
    $("a.uploadLogFileFromClipboardBtn").on('click', function (e) {

        const isInternal = $(this).data('logtype') === 'internalLog';

        var opt = {
            fileKey: $('#LogKey').val(),
            refreshCallback: function () { getLogFiles(isInternal); },
            submitUrl: '/Cases/UploadLogFile',
            uploadParams: {
                isInternalLog: isInternal === true
            }
        };
        clipboardFileUpload = new ClipboardFileUpload(opt);
        clipboardFileUpload.show();
    });

    ////////  Files Upload (PLUPLOAD) //////////////////////////////////////////
    var _plupload;

    $('#upload_files_popup, #upload_logfiles_popup').on('hide', function () {
        if (_plupload != undefined) {
            if (_plupload.pluploadQueue().files.length > 0) {
                if (_plupload.pluploadQueue().state === plupload.UPLOADING) {
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

    // case files upload
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
                    file.name = getFileName(file.name);
                },

                UploadComplete: function (up, file) {
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");
                    up.refresh();
                    $("#IsAnyFileAdded").val("1");
                    // Raise event about uploaded file
                    //$(document).trigger("OnUploadCaseFile", [up, file]);
                }
            },
            init: {
                FileUploaded: function () {
                    getCaseFiles();
                },

                FilesAdded: function (up, files) {
                    if (opt.fileUploadWhiteList != null) {
                        var whiteList = opt.fileUploadWhiteList;

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

    PluploadTranslation($('#CurLanguageId').val());

    $("a.uploadLogFilesBtn").on('click', function (e) {
        const logType = $(this).data('logtype');
        const $target = $('#upload_logfiles_popup');
        $target.data('src', logType || '');
        $target.modal('show');
    });

    //log files upload
    $('#upload_logfiles_popup').on('show', function (e) {
        const isInternalLog = $(e.target).data('src') === 'internalLog';
        console.log('internal log was clicked: %s', isInternalLog);

        _plupload = $('#logfile_uploader').pluploadQueue({
            runtimes: 'html5,html4',
            url: '/Cases/UploadLogFile',
            // pass addition params
            multipart_params: {
                id: $('#LogKey').val(),
                isInternalLog: isInternalLog === true
            },
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
                    const $logFileNames = isInternalLog ? $('#LogFileNamesInternal') : $('#LogFileNames');
                    var strFiles = $logFileNames.val();
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

                    //TODO: check if its a internal or external log file upload
                    $logFileNames.val(strFiles + "|" + file.name);
                },

                UploadComplete: function (up, file) {
                    //plupload_add
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");
                    up.refresh();
                    $("#IsAnyFileAdded").val("1");
                    //$(document).trigger("OnUploadLogFile", [up, file]);
                }
            },
            init: {
                FileUploaded: function () {
                    getLogFiles.call(self, isInternalLog);
                },

                FilesAdded: function (up, files) {
                    if (opt.fileUploadWhiteList != null) {
                        var whiteList = opt.fileUploadWhiteList;

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
    ////////  Files Upload: END /////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////

    if (window.LogInitForm != null) {
        LogInitForm();
    }

    bindDeleteCaseFileBehaviorToDeleteButtons();
    SetFocusToReportedByOnCase();

    $("#caseAddExtraFollowersBtn").click(function () {
        $("#caseAddFollowersDialog").dialog("option", "width", 350);
        $("#caseAddFollowersDialog").dialog("option", "height", 350);
        $("#caseAddFollowersDialog").dialog("open");
        var existEmails = $("#caseFollowerUsers").val();
        $("#caseAddFollowersSendInput").val(existEmails);
        $("#caseAddFollowersSendInput").focus();
    });
}