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
            var result =item.ctype + ": " + item.num + ' - ' + item.location + ' - ' + (item.computertype == null ? ' ' : item.computertype);

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
            delayFunc(function() {
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
        resizedCallback: function(messageData) {
            //console.log('>>>iFrame sesize callback called.!');
        },
        bodyMargin: '0 0 0 0',
        closedCallback: function (id) {},
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

function setPcNumber(userId)
{       
    $.ajax({
        url: '/cases/Search_PcNumber',
        type: 'post',
        data: { userId: userId, customerId: $('#case__Customer_Id').val() },
        dataType: 'json',
        success: function (result) {
            if (result != null){
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
function CaseInitForm() {

    

    $('#CaseLog_TextExternal').focus(function () {
        CaseWriteTextToLogNote('');
    });

    $('#CaseLog_TextInternal').focus(function () {
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
        .fail(function(jqXHR, textStatus, errorThrown) {
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

        var textExternalLogNote = $('#CaseLog_TextExternal').val();
  
        $.post('/Cases/ChangePriority/', { 'id': $(this).val(), 'textExternalLogNote': textExternalLogNote }, function (data) {
            const $txtExternal = $('#CaseLog_TextExternal');
            if (data.ExternalLogText != null && data.ExternalLogText !== '') {
                $txtExternal.val(data.ExternalLogText);
                $txtExternal.trigger('propertychange');
            } else {
                $txtExternal.val('');
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

    // Product Area change
    $('#case__ProductArea_Id').change(function (e) {
        console.log('>>> ProductArea changed event.');
        $("#ProductAreaHasChild").val(0);        
        document.getElementById("divProductArea").classList.remove("error");

        var templateWgId = $('#CaseTemplate_WorkingGroup_Id').val() || '';
        var templatePriorityId = $('#CaseTemplate_Priority_Id').val() || '';

        if ($(this).val() > 0 ) {
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
        }
        if (source !== 'case__StateSecondary_Id') {
            //set state secondary
            SelectValueInOtherDropdownOnChange($(this).val(),
                    '/Cases/ChangeWorkingGroupSetStateSecondary/',
                    '#case__StateSecondary_Id',
                    '.readonlySubstate')
                .done(function() {
                    $('#case__StateSecondary_Id').trigger('change', 'case__WorkingGroup_Id');
                });
        }
    });


    $("#standardtextDropdownMenu div.case-div-standardtext").click(function () {
        addTextToLog($(this).children("span").html());
    });

    function addTextToLog(text) {
        var regexp = /<BR>/g;
        var txt = text.replace(regexp, "\n");
        var writeTextToExternalNote = $("#WriteTextToExternalNote").val();
        var field = "#CaseLog_TextInternal";

        if (writeTextToExternalNote == "") {
            field = "#CaseLog_TextExternal";
        }

        if (txt.length > 1) {
            $(field).val($(field).val() + txt);
            $(field).focus();
            $(field).trigger("propertychange");

            var input = $(field);
            input[0].selectionStart = input[0].selectionEnd = input.val().length;
        }
        $("#standardtextDropdownMenu").click();
    }

    $("#standardtextDropdownMenu").click(function() {
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
    $("ul.dropdown-menu li a").click(function(e) {
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
    if ($(publicDepartmentControlName) != undefined)
    {
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
         if (userCategory != ''){
            params += "&userCategory=" + userCategory; 
         }

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
        $.get('/Cases/Files', { id: $('#CaseKey').val(), savedFiles: $('#SavedFiles').val(),  now: Date.now() }, function (data) {
            $('#divCaseFiles').html(data);
            // Raise event about rendering of uploaded file
            $(document).trigger("OnUploadedCaseFileRendered", []);
            bindDeleteCaseFileBehaviorToDeleteButtons();
        });
    };

    var getLogFiles = function () {
        $.get('/Cases/LogFiles', { id: $('#LogKey').val(), now: Date.now(), caseId: $("#case__Id").val() }, function (data) {
            $('#divCaseLogFiles').html(data);
            // Raise event about rendering of uploaded file
            $(document).trigger("OnUploadedCaseLogFileRendered", []);
            bindDeleteLogFileBehaviorToDeleteButtons();
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
    function ClipboardClass() {
        var self = this;
        var ctrlPressed = false;
        var pasteCatcher;
        var pasteMode;
        var source;
        var mimeTypes = { "3dm": "x-world/x-3dmf", "3dmf": "x-world/x-3dmf", "a": "application/octet-stream", "aab": "application/x-authorware-bin", "aam": "application/x-authorware-map", "aas": "application/x-authorware-seg", "abc": "text/vnd.abc", "acgi": "text/html", "afl": "video/animaflex", "ai": "application/postscript", "aif": "audio/x-aiff", "aifc": "audio/x-aiff", "aiff": "audio/x-aiff", "aim": "application/x-aim", "aip": "text/x-audiosoft-intra", "ani": "application/x-navi-animation", "aos": "application/x-nokia-9000-communicator-add-on-software", "aps": "application/mime", "arc": "application/octet-stream", "arj": "application/octet-stream", "art": "image/x-jg", "asf": "video/x-ms-asf", "asm": "text/x-asm", "asp": "text/asp", "asx": "video/x-ms-asf-plugin", "au": "audio/x-au", "avi": "video/x-msvideo", "avs": "video/avs-video", "bcpio": "application/x-bcpio", "bin": "application/x-macbinary", "bm": "image/bmp", "bmp": "image/x-windows-bmp", "boo": "application/book", "book": "application/book", "boz": "application/x-bzip2", "bsh": "application/x-bsh", "bz": "application/x-bzip", "bz2": "application/x-bzip2", "c": "text/x-c", "c++": "text/plain", "cat": "application/vnd.ms-pki.seccat", "cc": "text/x-c", "ccad": "application/clariscad", "cco": "application/x-cocoa", "cdf": "application/x-netcdf", "cer": "application/x-x509-ca-cert", "cha": "application/x-chat", "chat": "application/x-chat", "class": "application/x-java-class", "com": "text/plain", "conf": "text/plain", "cpio": "application/x-cpio", "cpp": "text/x-c", "cpt": "application/x-cpt", "crl": "application/pkix-crl", "crt": "application/x-x509-user-cert", "csh": "text/x-script.csh", "css": "text/css", "cxx": "text/plain", "dcr": "application/x-director", "deepv": "application/x-deepv", "def": "text/plain", "der": "application/x-x509-ca-cert", "dif": "video/x-dv", "dir": "application/x-director", "dl": "video/x-dl", "doc": "application/msword", "dot": "application/msword", "dp": "application/commonground", "drw": "application/drafting", "dump": "application/octet-stream", "dv": "video/x-dv", "dvi": "application/x-dvi", "dwf": "model/vnd.dwf", "dwg": "image/x-dwg", "dxf": "image/x-dwg", "dxr": "application/x-director", "el": "text/x-script.elisp", "elc": "application/x-elc", "env": "application/x-envoy", "eps": "application/postscript", "es": "application/x-esrehber", "etx": "text/x-setext", "evy": "application/x-envoy", "exe": "application/octet-stream", "f": "text/x-fortran", "f77": "text/x-fortran", "f90": "text/x-fortran", "fdf": "application/vnd.fdf", "fif": "image/fif", "fli": "video/x-fli", "flo": "image/florian", "flx": "text/vnd.fmi.flexstor", "fmf": "video/x-atomic3d-feature", "for": "text/x-fortran", "fpx": "image/vnd.net-fpx", "frl": "application/freeloader", "funk": "audio/make", "g": "text/plain", "g3": "image/g3fax", "gif": "image/gif", "gl": "video/x-gl", "gsd": "audio/x-gsm", "gsm": "audio/x-gsm", "gsp": "application/x-gsp", "gss": "application/x-gss", "gtar": "application/x-gtar", "gz": "application/x-gzip", "gzip": "multipart/x-gzip", "h": "text/x-h", "hdf": "application/x-hdf", "help": "application/x-helpfile", "hgl": "application/vnd.hp-hpgl", "hh": "text/x-h", "hlb": "text/x-script", "hlp": "application/x-winhelp", "hpg": "application/vnd.hp-hpgl", "hpgl": "application/vnd.hp-hpgl", "hqx": "application/x-mac-binhex40", "hta": "application/hta", "htc": "text/x-component", "htm": "text/html", "html": "text/html", "htmls": "text/html", "htt": "text/webviewhtml", "htx": "text/html", "ice": "x-conference/x-cooltalk", "ico": "image/x-icon", "idc": "text/plain", "ief": "image/ief", "iefs": "image/ief", "iges": "application/iges", "iges": "model/iges", "igs": "model/iges", "ima": "application/x-ima", "imap": "application/x-httpd-imap", "inf": "application/inf", "ins": "application/x-internett-signup", "ip": "application/x-ip2", "isu": "video/x-isvideo", "it": "audio/it", "iv": "application/x-inventor", "ivr": "i-world/i-vrml", "ivy": "application/x-livescreen", "jam": "audio/x-jam", "jav": "text/x-java-source", "java": "text/plain", "java": "text/x-java-source", "jcm": "application/x-java-commerce", "jfif": "image/pjpeg", "jfif-tbnl": "image/jpeg", "jpe": "image/pjpeg", "jpeg": "image/pjpeg", "jpg": "image/pjpeg", "jps": "image/x-jps", "js": "application/x-javascript", "jut": "image/jutvision", "kar": "music/x-karaoke", "ksh": "text/x-script.ksh", "la": "audio/x-nspaudio", "lam": "audio/x-liveaudio", "latex": "application/x-latex", "lha": "application/x-lha", "lhx": "application/octet-stream", "list": "text/plain", "lma": "audio/x-nspaudio", "log": "text/plain", "lsp": "text/x-script.lisp", "lst": "text/plain", "lsx": "text/x-la-asf", "ltx": "application/x-latex", "lzh": "application/x-lzh", "lzx": "application/x-lzx", "m": "text/x-m", "m1v": "video/mpeg", "m2a": "audio/mpeg", "m2v": "video/mpeg", "m3u": "audio/x-mpequrl", "man": "application/x-troff-man", "map": "application/x-navimap", "mar": "text/plain", "mbd": "application/mbedlet", "mc$": "application/x-magic-cap-package-1.0", "mcd": "application/x-mathcad", "mcf": "text/mcf", "mcp": "application/netmc", "me": "application/x-troff-me", "mht": "message/rfc822", "mhtml": "message/rfc822", "mid": "x-music/x-midi", "midi": "x-music/x-midi", "mif": "application/x-mif", "mime": "www/mime", "mjf": "audio/x-vnd.audioexplosion.mjuicemediafile", "mjpg": "video/x-motion-jpeg", "mm": "application/x-meme", "mme": "application/base64", "mod": "audio/x-mod", "moov": "video/quicktime", "mov": "video/quicktime", "movie": "video/x-sgi-movie", "mp2": "video/x-mpeq2a", "mp3": "video/x-mpeg", "mpa": "video/mpeg", "mpc": "application/x-project", "mpe": "video/mpeg", "mpeg": "video/mpeg", "mpg": "video/mpeg", "mpga": "audio/mpeg", "mpp": "application/vnd.ms-project", "mpt": "application/x-project", "mpv": "application/x-project", "mpx": "application/x-project", "mrc": "application/marc", "ms": "application/x-troff-ms", "mv": "video/x-sgi-movie", "my": "audio/make", "mzz": "application/x-vnd.audioexplosion.mzz", "nap": "image/naplps", "naplps": "image/naplps", "nc": "application/x-netcdf", "ncm": "application/vnd.nokia.configuration-message", "nif": "image/x-niff", "niff": "image/x-niff", "nix": "application/x-mix-transfer", "nsc": "application/x-conference", "nvd": "application/x-navidoc", "o": "application/octet-stream", "oda": "application/oda", "omc": "application/x-omc", "omcd": "application/x-omcdatamaker", "omcr": "application/x-omcregerator", "p": "text/x-pascal", "p10": "application/x-pkcs10", "p12": "application/x-pkcs12", "p7a": "application/x-pkcs7-signature", "p7c": "application/x-pkcs7-mime", "p7m": "application/x-pkcs7-mime", "p7r": "application/x-pkcs7-certreqresp", "p7s": "application/pkcs7-signature", "part": "application/pro_eng", "pas": "text/pascal", "pbm": "image/x-portable-bitmap", "pcl": "application/x-pcl", "pct": "image/x-pict", "pcx": "image/x-pcx", "pdb": "chemical/x-pdb", "pdf": "application/pdf", "pfunk": "audio/make.my.funk", "pgm": "image/x-portable-greymap", "pic": "image/pict", "pict": "image/pict", "pkg": "application/x-newton-compatible-pkg", "pko": "application/vnd.ms-pki.pko", "pl": "text/x-script.perl", "plx": "application/x-pixclscript", "pm": "text/x-script.perl-module", "pm4": "application/x-pagemaker", "pm5": "application/x-pagemaker", "png": "image/png", "pnm": "image/x-portable-anymap", "pot": "application/vnd.ms-powerpoint", "pov": "model/x-pov", "ppa": "application/vnd.ms-powerpoint", "ppm": "image/x-portable-pixmap", "pps": "application/vnd.ms-powerpoint", "ppt": "application/x-mspowerpoint", "ppz": "application/mspowerpoint", "pre": "application/x-freelance", "prt": "application/pro_eng", "ps": "application/postscript", "psd": "application/octet-stream", "pvu": "paleovu/x-pv", "pwz": "application/vnd.ms-powerpoint", "py": "text/x-script.phyton", "pyc": "applicaiton/x-bytecode.python", "qcp": "audio/vnd.qcelp", "qd3": "x-world/x-3dmf", "qd3d": "x-world/x-3dmf", "qif": "image/x-quicktime", "qt": "video/quicktime", "qtc": "video/x-qtc", "qti": "image/x-quicktime", "qtif": "image/x-quicktime", "ra": "audio/x-realaudio", "ram": "audio/x-pn-realaudio", "ras": "image/x-cmu-raster", "rast": "image/cmu-raster", "rexx": "text/x-script.rexx", "rf": "image/vnd.rn-realflash", "rgb": "image/x-rgb", "rm": "audio/x-pn-realaudio", "rmi": "audio/mid", "rmm": "audio/x-pn-realaudio", "rmp": "audio/x-pn-realaudio-plugin", "rng": "application/vnd.nokia.ringing-tone", "rnx": "application/vnd.rn-realplayer", "roff": "application/x-troff", "rp": "image/vnd.rn-realpix", "rpm": "audio/x-pn-realaudio-plugin", "rt": "text/vnd.rn-realtext", "rtf": "text/richtext", "rtx": "text/richtext", "rv": "video/vnd.rn-realvideo", "s": "text/x-asm", "s3m": "audio/s3m", "saveme": "application/octet-stream", "sbk": "application/x-tbook", "scm": "video/x-scm", "sdml": "text/plain", "sdp": "application/x-sdp", "sdr": "application/sounder", "sea": "application/x-sea", "set": "application/set", "sgm": "text/x-sgml", "sgml": "text/x-sgml", "sh": "text/x-script.sh", "shar": "application/x-shar", "shtml": "text/html", "shtml": "text/x-server-parsed-html", "sid": "audio/x-psid", "sit": "application/x-stuffit", "skd": "application/x-koan", "skm": "application/x-koan", "skp": "application/x-koan", "skt": "application/x-koan", "sl": "application/x-seelogo", "smi": "application/smil", "smil": "application/smil", "snd": "audio/x-adpcm", "sol": "application/solids", "spc": "text/x-speech", "spl": "application/futuresplash", "spr": "application/x-sprite", "sprite": "application/x-sprite", "src": "application/x-wais-source", "ssi": "text/x-server-parsed-html", "ssm": "application/streamingmedia", "sst": "application/vnd.ms-pki.certstore", "step": "application/step", "stl": "application/x-navistyle", "stp": "application/step", "sv4cpio": "application/x-sv4cpio", "sv4crc": "application/x-sv4crc", "svf": "image/x-dwg", "svr": "x-world/x-svr", "swf": "application/x-shockwave-flash", "t": "application/x-troff", "talk": "text/x-speech", "tar": "application/x-tar", "tbk": "application/x-tbook", "tcl": "text/x-script.tcl", "tcsh": "text/x-script.tcsh", "tex": "application/x-tex", "texi": "application/x-texinfo", "texinfo": "application/x-texinfo", "text": "text/plain", "tgz": "application/x-compressed", "tif": "image/x-tiff", "tiff": "image/x-tiff", "tr": "application/x-troff", "tsi": "audio/tsp-audio", "tsp": "audio/tsplayer", "tsv": "text/tab-separated-values", "turbot": "image/florian", "txt": "text/plain", "uil": "text/x-uil", "uni": "text/uri-list", "unis": "text/uri-list", "unv": "application/i-deas", "uri": "text/uri-list", "uris": "text/uri-list", "ustar": "multipart/x-ustar", "uu": "text/x-uuencode", "uue": "text/x-uuencode", "vcd": "application/x-cdlink", "vcs": "text/x-vcalendar", "vda": "application/vda", "vdo": "video/vdo", "vew": "application/groupwise", "viv": "video/vnd.vivo", "vivo": "video/vnd.vivo", "vmd": "application/vocaltec-media-desc", "vmf": "application/vocaltec-media-file", "voc": "audio/x-voc", "vos": "video/vosaic", "vox": "audio/voxware", "vqe": "audio/x-twinvq-plugin", "vqf": "audio/x-twinvq", "vql": "audio/x-twinvq-plugin", "vrml": "x-world/x-vrml", "vrt": "x-world/x-vrt", "vsd": "application/x-visio", "vst": "application/x-visio", "vsw": "application/x-visio", "w60": "application/wordperfect6.0", "w61": "application/wordperfect6.1", "w6w": "application/msword", "wav": "audio/x-wav", "wb1": "application/x-qpro", "wbmp": "image/vnd.wap.wbmp", "web": "application/vnd.xara", "wiz": "application/msword", "wk1": "application/x-123", "wmf": "windows/metafile", "wml": "text/vnd.wap.wml", "wmlc": "application/vnd.wap.wmlc", "wmls": "text/vnd.wap.wmlscript", "wmlsc": "application/vnd.wap.wmlscriptc", "word": "application/msword", "wp": "application/wordperfect", "wp5": "application/wordperfect6.0", "wp6": "application/wordperfect", "wpd": "application/x-wpwin", "wq1": "application/x-lotus", "wri": "application/x-wri", "wrl": "x-world/x-vrml", "wrz": "x-world/x-vrml", "wsc": "text/scriplet", "wsrc": "application/x-wais-source", "wtk": "application/x-wintalk", "xbm": "image/xbm", "xdr": "video/x-amt-demorun", "xgz": "xgl/drawing", "xif": "image/vnd.xiff", "xl": "application/excel", "xla": "application/x-msexcel", "xlb": "application/x-excel", "xlc": "application/x-excel", "xld": "application/x-excel", "xlk": "application/x-excel", "xll": "application/x-excel", "xlm": "application/x-excel", "xls": "application/x-msexcel", "xlt": "application/x-excel", "xlv": "application/x-excel", "xlw": "application/x-msexcel", "xm": "audio/xm", "xml": "text/xml", "xmz": "xgl/movie", "xpix": "application/x-vnd.ls-xpix", "xpm": "image/xpm", "x-png": "image/png", "xsr": "video/x-amt-showrun", "xwd": "image/x-xwindowdump", "xyz": "chemical/x-pdb", "z": "application/x-compressed", "zip": "multipart/x-zip", "zoo": "application/octet-stream", "zsh": "text/x-script.zsh" };
        var getExtensionByType = function(type) {
            for (var i in mimeTypes) {
                if (mimeTypes.hasOwnProperty(i)) {
                    if (mimeTypes[i] === type) {
                        return i;
                    }
                }
            }
            return "png";//default extension
        }
        
        //constructor - prepare
        this.init = function (src) {
            source = src;

            var $document = $(document);
            //handlers
            $document.on('keydown', function (e) {
                self.on_keyboard_action(e);
            }); //firefox fix
            $document.on('keyup', function (e) {
                self.on_keyboardup_action(e);
            }); //firefox fix
            $document.on('paste', function (e) {
                self.paste_auto(e);
            }); //official paste handler

            //if using auto
            if (window.Clipboard)
                return true;

            pasteCatcher = document.createElement("div");
            pasteCatcher.setAttribute("id", "paste_ff");
            pasteCatcher.setAttribute("contenteditable", "");
            pasteCatcher.style.cssText = 'opacity:0;position:fixed;top:0px;left:0px;';
            pasteCatcher.style.marginLeft = "-20px";
            pasteCatcher.style.width = "10px";
            document.body.appendChild(pasteCatcher);
            document.getElementById('paste_ff').addEventListener('DOMSubtreeModified', function() {
                if (pasteMode === 'auto' || ctrlPressed === false)
                    return true;
                //if paste handle failed - capture pasted object manually
                if (pasteCatcher.children.length === 1) {
                    if (pasteCatcher.firstElementChild.src != undefined) {
                        //image
                        self.paste_createImage(pasteCatcher.firstElementChild.src);
                        var blob = self.dataURItoBlob(pasteCatcher.firstElementChild.src);
                        self.allowSave(blob);
                    }
                }
                //register cleanup after some time.
                setTimeout(function() {
                    pasteCatcher.innerHTML = '';
                }, 20);
            }, false);
        };

        this.reset = function () {
            var $document = $(document);
            $document.off('keydown');
            $document.off('keyup');
            $document.off('paste');
            $("#paste_ff").remove();
        };
        //default paste action
        this.paste_auto = function (e) {
            pasteMode = '';
            if (pasteCatcher) {
                pasteCatcher.innerHTML = '';
            }
            var clipboardData = (e.clipboardData || e.originalEvent.clipboardData);
            if (clipboardData) {
                var items = clipboardData.items;
                if (items) {
                    pasteMode = 'auto';
                    var blob = null;
                    //access data directly
                    for (var i = 0; i < items.length; i++) {
                        if (items[i].type.indexOf("image") !== -1) {
                            //image
                            blob = items[i].getAsFile();
                        }
                    }
                    if (blob !== null) {
                        var URLObj = window.URL || window.webkitURL;
                        var source = URLObj.createObjectURL(blob);
                        this.paste_createImage(source);
                        this.allowSave(blob);
                    }
                    e.preventDefault();
                }
                else {
                    //wait for DOMSubtreeModified event
                    //https://bugzilla.mozilla.org/show_bug.cgi?id=891247
                }
            }
        };
        //on keyboard press - 
        this.on_keyboard_action = function (event) {
            k = event.keyCode;
            //ctrl
            if (k === 17 || event.metaKey || event.ctrlKey) {
                if (ctrlPressed === false)
                    ctrlPressed = true;
            }
            //v
            if (k === 86) {

                if (ctrlPressed === true && !window.Clipboard)
                    pasteCatcher.focus();
            }
        };
        //on kaybord release
        this.on_keyboardup_action = function (event) {
            k = event.keyCode;
            //ctrl
            if (k === 17 || event.metaKey || event.ctrlKey || event.key == 'Meta')
                ctrlPressed = false;
        };

        this.dataURItoBlob = function(dataURI) {
            var byteString;

            if (dataURI.split(',')[0].indexOf('base64') !== -1) {
                byteString = atob(dataURI.split(',')[1]);
            } else {
                byteString = decodeURI(dataURI.split(',')[1]);
            }

            var mimestring = dataURI.split(',')[0].split(':')[1].split(';')[0];

            var content = new Array();
            for (var i = 0; i < byteString.length; i++) {
                content[i] = byteString.charCodeAt(i);
            }

            return new Blob([new Uint8Array(content)], { type: mimestring });
        };

        //draw image
        this.paste_createImage = function (dataUrl) {
            var $previewPnl = $('#previewPnl');
            var imgCtrl = $previewPnl.find('img');
            if (imgCtrl.length === 0) {
                $previewPnl.append('<img style="width:400px;height:400px;" />');
                imgCtrl = $previewPnl.find('img');
            }
            imgCtrl[0].src = dataUrl;
        };

        this.allowSave = function (blob) {
            var uploadModal = $('#upload_clipboard_file_popup');
            var $btnSave = uploadModal.find('#btnSave');
            var extension = getExtensionByType(blob.type);
            var imgFilenameCtrl = uploadModal.find("#imgFilename");
            var key;
            var submitUrl;
            var refredhCallback;
            var imgFilename = imgFilenameCtrl.val();

            if (source == 'case') {
                key = $('#CaseKey').val();
                submitUrl = '/Cases/UploadCaseFile';
                refredhCallback = getCaseFiles;
            } else {
                key = $('#LogKey').val();
                submitUrl = '/Cases/UploadLogFile';
                refredhCallback = getLogFiles;
            }

            if (imgFilename.length === 0) {
                imgFilename = 'image_' + key;
            }
            if (imgFilename.indexOf('.') === -1) {
                imgFilename = imgFilename + '.' + extension;
            }

            imgFilenameCtrl.val(imgFilename);
            $btnSave.on('click', function () {
                var fd = new FormData();
                uploadModal.find('form').submit();

                var regexp = new RegExp("[!@#=\$&\?\*]");
                var match = regexp.exec(imgFilenameCtrl.val());
                if (match) {
                    ShowToastMessage(window.parameters.fileNameError, "error");
                } else {
                    if (imgFilenameCtrl[0].validity.valid) {
                        fd.append('name', getFileName(imgFilenameCtrl.val()));
                        fd.append('id', key);
                        fd.append('file', blob);
                        $.ajax({
                                type: 'POST',
                                url: submitUrl,
                                data: fd,
                                processData: false,
                                contentType: false
                            })
                            .done(function(data) {
                                //console.log(data);
                                refredhCallback();
                                uploadModal.modal("hide");
                            });
                    }
                }
            });
            $btnSave.show();

        }
    }
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
    $('#upload_clipboard_file_popup').on('hide', function() {
        $("#previewPnl").empty();
        var $uploadModal = $('#upload_clipboard_file_popup');
        var $btnSave = $uploadModal.find('#btnSave');
        $btnSave.hide();
        $btnSave.off('click');
        $uploadModal.find("input").val('');
        var clipboard = new ClipboardClass();
        clipboard.reset();
    });
    $("a[href='#upload_clipboard_file_popup']").on('click', function (e) {
        var $src = $(this);
        var $target = $('#upload_clipboard_file_popup');
        $target.attr('data-src', $src.attr('data-src'));
        $target.modal('show');
    });
    $('#upload_clipboard_file_popup').on('show', function (e) {
        var clipboard = new ClipboardClass();
        clipboard.init.call(clipboard, $(e.target).attr('data-src'));
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
                    $("#IsAnyFileAdded").val("1");
                    //$(document).trigger("OnUploadLogFile", [up, file]);
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

    $("#caseAddExtraFollowersBtn").click(function () {
        $("#caseAddFollowersDialog").dialog("option", "width", 350);
        $("#caseAddFollowersDialog").dialog("option", "height", 350);
        $("#caseAddFollowersDialog").dialog("open");
        var existEmails = $("#caseFollowerUsers").val();
        $("#caseAddFollowersSendInput").val(existEmails);
        $("#caseAddFollowersSendInput").focus();
    });
}