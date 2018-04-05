
function changeCompany() {
    var bunit = $('#BusinessUnit');

    if (!bunit.is("select"))
        return;

    var companyId = $("#Company").val();
    var selectedValue = $("#val_BusinessUnit").val();

    $('#BusinessUnit').find('option').remove();
    $("#BusinessUnit")[0].add(new Option("", " "));
    $('#ServiceArea').find('option').remove();
    $("#ServiceArea")[0].add(new Option("", " "));
    $('#Department').find('option').remove();
    $("#Department")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');
        var customerId = ajaxInfo.attr('customerId');

        if (url && customerId && companyId && (!isNaN(parseFloat(companyId)) && isFinite(companyId))) {
            var jqxhr = $.post(url + 'GetBusinessUnits?customerId=' + customerId + '&companyId=' + companyId + "&ie=" + (new Date()).getTime(), function () {

            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        $("#BusinessUnit")[0].add(new Option(e.Name, e.Id))
                    });

                    if (selectedValue != '') {
                        $('#BusinessUnit').val(selectedValue);
                        changeBusinessUnit();
                    }
                })
                .fail(function () { })
                .always(function () { });
        }
    }
}

function changeNewCompany(clear) {

    if ($("#NewCompany option").length == 2) {
        $("#NewCompany option").eq(1).attr('selected', true);
    }

    var companyId = $("#NewCompany").val();
    var hidden = $("#hidden_NewCompany");

    if (hidden.length > 0)
        hidden.val(companyId);

    var selectedValue = $("#val_NewBusinessUnit").val();
    var oldValue = $('#OLD_NewBusinessUnit').val();

    $('#NewBusinessUnit').find('option').remove();
    $("#NewBusinessUnit")[0].add(new Option("", " "));
    $('#NewServiceArea').find('option').remove();
    $("#NewServiceArea")[0].add(new Option("", " "));
    $('#NewDepartment').find('option').remove();
    $("#NewDepartment")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');
        var customerId = ajaxInfo.attr('customerId');

        if (url && companyId) {

            var jqxhr = $.post(url + 'GetBusinessUnits?customerId=' + customerId + '&companyId=' + companyId + "&ie=" + (new Date()).getTime(), function () { })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        $("#NewBusinessUnit")[0].add(new Option(e.Name, e.Id))
                    });

                    if (clear && oldValue != '') {
                        $('#NewBusinessUnit option').filter(function () {
                            return $(this).text() == oldValue;
                        }).prop('selected', true);

                        if ($('#PrimarySite')[0].selectize) {
                            $('#PrimarySite')[0].selectize.setValue(oldValue);
                        }
                        else
                        {
                            $('#PrimarySite').val(oldValue);
                        }

                        
                    }

                    if (!clear && selectedValue != '') {
                        $('#NewBusinessUnit').val(selectedValue);                     
                    }
                  
                    if ($("#NewBusinessUnit option").length == 2) {
                        $("#NewBusinessUnit option").eq(1).attr('selected', true);
                        
                        //Commented out because it gave complication on IE change t&c. Overwriting default values when saving.
                        //$("#NewBusinessUnit").change();
                        
                        if ($('#PrimarySite')[0].selectize) {
                            $('#PrimarySite')[0].selectize.setValue($('#NewBusinessUnit').text());
                        }
                        else {
                            $('#PrimarySite').val($('#NewBusinessUnit').text());
                        }

                    }

                    changeNewBusinessUnit(clear);

                })
                .fail(function () { })
                .always(function () { });                  
        }
    }

    if (typeof (changeNewCompanyCallback) == "function") {
        changeNewCompanyCallback();
    }
}

//Used for Incorrect Worked Hours //NO
function setCompanyValue(elementId, value, triggerChange) {
    if ($("#" + elementId + " option").length == 2) {
        $("#" + elementId + " option").eq(1).attr('selected', true);
    }

    var hidden = $("#hidden_" + elementId);

    if (hidden.length > 0)
        hidden.val(value);

    if (triggerChange) {
        $('#' + elementId).trigger('change');
    }
}

function changeBusinessUnit() {
    // Please don't check in Any alert , Debuggers and so on S.G
    //alert();

    var businessUnitId = $("#BusinessUnit").val();
    var selectedValue = $("#val_ServiceArea").val();

    $('#ServiceArea').find('option').remove();
    $("#ServiceArea")[0].add(new Option("", " "));
    $('#Department').find('option').remove();
    $("#Department")[0].add(new Option("", " "));


    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');

        if (url && businessUnitId && $.trim(businessUnitId) != '') {

            var jqxhr = $.post(url + 'GetFunctions?businessUnitId=' + businessUnitId + "&ie=" + (new Date()).getTime(), function () {

            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        // Only adds departments which are active or saved to case before -- AC 2014-10-24
                        if (e.Status == 1 || e.Id == selectedValue) {
                            $("#ServiceArea")[0].add(new Option(e.Name, e.Id))
                        }
                    });

                    if (selectedValue != '') {
                        $('#ServiceArea').val(selectedValue);
                        changeFunctions();
                    }

                    if (typeof (changeBusinessUnitCallback) == "function") {
                        changeBusinessUnitCallback();
                    }
                })
                .fail(function () { })
                .always(function () { });
        }
    }
}

function changeNewBusinessUnit(clear) {

    var businessUnitId = $("#NewBusinessUnit").val();

    var hidden = $("#hidden_NewBusinessUnit");

    if (hidden.length > 0)
        hidden.val(businessUnitId);

    var selectedValue = $("#val_NewServiceArea").val();
    var oldValue = $('#OLD_NewServiceArea').val();

    $('#NewServiceArea').find('option').remove();
    $("#NewServiceArea")[0].add(new Option("", " "));
    $('#NewDepartment').find('option').remove();
    $("#NewDepartment")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');

        if (url && businessUnitId && $.trim(businessUnitId) != '') {

            var jqxhr = $.post(url + 'GetFunctions?businessUnitId=' + businessUnitId + "&ie=" + (new Date()).getTime(), function () { })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        // Only adds departments which are active or saved to case before -- AC 2014-10-24
                        if (e.Status == 1 || e.Id == selectedValue) {
                            $("#NewServiceArea")[0].add(new Option(e.Name, e.Id))
                        }
                    });

                    if (clear && oldValue != '') {
                        $('#NewServiceArea option').filter(function () {
                            return $(this).text() == oldValue;
                        }).prop('selected', true);
                    }

                    if (!clear && selectedValue != '') {
                        $('#NewServiceArea').val(selectedValue);
                    }

                    if ($.trim($("#NewServiceArea").val()) == '') {
                        $("#NewServiceArea").val($("#NewServiceArea option:first").val());
                    }

                    if (typeof (changeBusinessUnitCallback) == "function") {
                        changeBusinessUnitCallback();
                    }

                    changeNewFunctions(clear);
                })
                .fail(function () { })
                .always(function () { });
        }
    }

    if (typeof (changeNewBusinessUnitCallback) == "function") {
        changeNewBusinessUnitCallback();
    }
}

function changeFunctions() {
    var serviceAreaId = $("#ServiceArea").val();
    var selectedValue = $("#val_Department").val();

    $('#Department').find('option').remove();
    $("#Department")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');
        
        if (url && serviceAreaId && $.trim(serviceAreaId) != '') {

            var jqxhr = $.post(url + 'GetDepartments?serviceAreaId=' + serviceAreaId + "&ie=" + (new Date()).getTime(), function () {

            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        // Only adds departments which are active or saved to case before -- AC 2014-10-24
                        if (e.Status == 1 || e.Id == selectedValue) {

                            //Add data-code if exist -- TAN 2015-11-17
                            var newOption = new Option(e.Name, e.Id);
                            if (e.Code.length > 0) {
                                newOption.setAttribute("data-code", e.Code);
                            }
                            
                            $("#Department")[0].add(newOption);

                        }
                    });

                    if (selectedValue != '')
                        $('#Department').val(selectedValue);

                    if (typeof (changeFunctionsCallback) == "function")
                    {
                        changeFunctionsCallback();
                    }
                })
                .fail(function () { })
                .always(function () { });
        }
    }
}

function changeNewFunctions(clear) {

    var serviceAreaId = $("#NewServiceArea").val();
    var selectedValue = $("#val_NewDepartment").val();
    var oldValue = $('#OLD_NewDepartment').val();

    $('#NewDepartment').find('option').remove();
    $("#NewDepartment")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');

        if (url && serviceAreaId && $.trim(serviceAreaId) != '') {

            var jqxhr = $.post(url + 'GetDepartments?serviceAreaId=' + serviceAreaId + "&ie=" + (new Date()).getTime(), function () { })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        // Only adds departments which are active or saved to case before -- AC 2014-10-24
                        if (e.Status == 1 || e.Id == selectedValue) {
                            $("#NewDepartment")[0].add(new Option(e.Name, e.Id))
                        }
                    });

                    if (clear && oldValue != '') {
                        $('#NewDepartment option').filter(function () {
                            return $(this).text() == oldValue;
                        }).prop('selected', true);
                    }

                    if (!clear && selectedValue != '')
                        $('#NewDepartment').val(selectedValue);

                    if ($.trim($("#NewDepartment").val()) == '') {
                        $("#NewDepartment").val($("#NewDepartment option:first").val());
                    }

                    if (typeof (changeFunctionsCallback) == "function") {
                        changeFunctionsCallback();
                    }

                    
                })
                .fail(function () { })
                .always(function () { });
        }
    }
}
function InitIntegration() {
    var company = $('#Company');
    var businessUnit = $('#BusinessUnit');
    var serviceArea = $('#ServiceArea');

    if (serviceArea.length > 0 && serviceArea.is("select")) {
        serviceArea.change(function () {
            changeFunctions();
        });
    }

    if (businessUnit.length > 0 && businessUnit.is("select")) {
        businessUnit.change(function () {
            changeBusinessUnit();
        });

    }

    if (company.length > 0 && company.is("select")) {
        company.change(function () {
            changeCompany();
        });

        company.change();
    }

    var newCompany = $('#NewCompany');
    var newBusinessUnit = $('#NewBusinessUnit');
    var newServiceArea = $('#NewServiceArea');

    if (newServiceArea.length > 0 && newServiceArea.is("select")) {
        newServiceArea.change(function () {
            changeNewFunctions();
        });
    }

    if (newBusinessUnit.length > 0 && newBusinessUnit.is("select")) {
        newBusinessUnit.change(function () {
            changeNewBusinessUnit();
        });
    }

    if (newCompany.length > 0 && newCompany.is("select")) {
        newCompany.change(function () {
            changeNewCompany();
        });

        newCompany.change();
    }
    if ($('#pickfiles').length > 0)
    initUpload();
}

