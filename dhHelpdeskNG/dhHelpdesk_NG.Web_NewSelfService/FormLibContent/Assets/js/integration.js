
function changeCompany() {
    var bunit = $('#BusinessUnit');

    if (!bunit.is("select"))
        return;

    var companyId = $("#Company").val();
    var selectedValue = $("#val_BusinessUnit").val();

    //$('#BusinessUnit').find('option').remove().end().append('<option value=""></option>');
    //$('#ServiceArea').find('option').remove().end().append('<option value=""></option>');
    //$('#Department').find('option').remove().end().append('<option value=""></option>');

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

        if (url != '' && customerId != '' && companyId != '' && (!isNaN(parseFloat(companyId)) && isFinite(companyId))) {

            var jqxhr = $.post(url + 'GetBusinessUnits?customerId=' + customerId + '&companyId=' + companyId + "&ie=" + (new Date()).getTime(), function () {

            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        //$('#BusinessUnit').append($("<option></option>").attr("value", e.Id).text(e.Name)); 

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

    var companyId = $("#NewCompany").val();
    var hidden = $("#hidden_NewCompany");

    if (hidden.length > 0)
        hidden.val(companyId);

    var selectedValue = $("#val_NewBusinessUnit").val();
    var oldValue = $('#OLD_NewBusinessUnit').val();

    //$('#NewBusinessUnit').find('option').remove().end().append('<option value=" "></option>');
    //$('#NewServiceArea').find('option').remove().end().append('<option value=" "></option>');
    //$('#NewDepartment').find('option').remove().end().append('<option value=" "></option>');

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

        if (url != '' && customerId != '' && companyId != '' && companyId != undefined) {

            var jqxhr = $.post(url + 'GetBusinessUnits?customerId=' + customerId + '&companyId=' + companyId + "&ie=" + (new Date()).getTime(), function () { })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        //$('#NewBusinessUnit').append($("<option></option>").attr("value", e.Id).text(e.Name));

                        $("#NewBusinessUnit")[0].add(new Option(e.Name, e.Id))
                    });

                    if (clear && oldValue != '') {
                        $('#NewBusinessUnit option').filter(function () {
                            return $(this).text() == oldValue;
                        }).prop('selected', true);
                    }

                    if (!clear && selectedValue != '') {
                        $('#NewBusinessUnit').val(selectedValue);
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

function changeBusinessUnit() {

    var businessUnitId = $("#BusinessUnit").val();
    var selectedValue = $("#val_ServiceArea").val();

    //$('#ServiceArea').find('option').remove().end().append('<option value=""></option>');
    //$('#Department').find('option').remove().end().append('<option value=""></option>');

    $('#ServiceArea').find('option').remove();
    $("#ServiceArea")[0].add(new Option("", " "));
    $('#Department').find('option').remove();
    $("#Department")[0].add(new Option("", " "));


    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');

        if (url != '' && businessUnitId != '') {

            var jqxhr = $.post(url + 'GetFunctions?businessUnitId=' + businessUnitId + "&ie=" + (new Date()).getTime(), function () {

            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        //$('#ServiceArea').append($("<option></option>").attr("value", e.Id).text(e.Name));

                        $("#ServiceArea")[0].add(new Option(e.Name, e.Id))
                    });

                    if (selectedValue != '') {
                        $('#ServiceArea').val(selectedValue);
                        changeFunctions();
                    }
                })
                .fail(function () { })
                .always(function () { });
        }
    }
}

function changeNewBusinessUnit(clear) {

    var businessUnitId = $("#NewBusinessUnit").val();
    var selectedValue = $("#val_NewServiceArea").val();
    var oldValue = $('#OLD_NewServiceArea').val();

    //$('#NewServiceArea').find('option').remove().end().append('<option value=" "></option>');
    //$('#NewDepartment').find('option').remove().end().append('<option value=" "></option>');

    $('#NewServiceArea').find('option').remove();
    $("#NewServiceArea")[0].add(new Option("", " "));
    $('#NewDepartment').find('option').remove();
    $("#NewDepartment")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');

        if (url != '' && $.trim(businessUnitId) != '') {

            var jqxhr = $.post(url + 'GetFunctions?businessUnitId=' + businessUnitId + "&ie=" + (new Date()).getTime(), function () { })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        //$('#NewServiceArea').append($("<option></option>").attr("value", e.Id).text(e.Name));

                        $("#NewServiceArea")[0].add(new Option(e.Name, e.Id))
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

    //$('#Department').find('option').remove().end().append('<option value=""></option>');

    $('#Department').find('option').remove();
    $("#Department")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');

        if (url != '' && serviceAreaId != '') {

            var jqxhr = $.post(url + 'GetDepartments?serviceAreaId=' + serviceAreaId + "&ie=" + (new Date()).getTime(), function () {

            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        //$('#Department').append($("<option></option>").attr("value", e.Id).text(e.Name));

                        $("#Department")[0].add(new Option(e.Name, e.Id))
                    });

                    if (selectedValue != '')
                        $('#Department').val(selectedValue);
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

    //$('#NewDepartment').find('option').remove().end().append('<option value=" "></option>');

    $('#NewDepartment').find('option').remove();
    $("#NewDepartment")[0].add(new Option("", " "));

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');

        if (url != '' && $.trim(serviceAreaId) != '') {

            var jqxhr = $.post(url + 'GetDepartments?serviceAreaId=' + serviceAreaId + "&ie=" + (new Date()).getTime(), function () { })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        //$('#NewDepartment').append($("<option></option>").attr("value", e.Id).text(e.Name));

                        $("#NewDepartment")[0].add(new Option(e.Name, e.Id))
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
}

