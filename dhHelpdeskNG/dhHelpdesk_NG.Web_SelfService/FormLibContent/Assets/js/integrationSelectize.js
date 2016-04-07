$(function () {

    if ($('#BusinessUnit')[0].selectize) {
        $('#BusinessUnit')[0].selectize.destroy();
    }
    if ($('#ServiceArea')[0].selectize) {
        $('#ServiceArea')[0].selectize.destroy();
    }
    if ($('#Department')[0].selectize) {
        $('#Department')[0].selectize.destroy();
    }

    if ($('#BusinessUnit').hasClass("search-select")) {
        $('#BusinessUnit').selectize({
            valueField: 'Id',
            labelField: 'Name',
            searchField: ['Name']
        });
    }

    if ($('#ServiceArea').hasClass("search-select")) {
        $('#ServiceArea').selectize({
            valueField: 'Id',
            labelField: 'Name',
            searchField: ['Name']
        });
    }

    if ($('#Department').hasClass("search-select")) {
        $('#Department').selectize({
            valueField: 'Id',
            labelField: 'Name',
            searchField: ['Name']
        });
    }

    function changeCompany() {

        var companyId = $("#Company").val();
        var selectedValue = $("#val_BusinessUnit").val();

        var businessUnitSelectize = $('#BusinessUnit')[0].selectize;
        var serviceAreaSelectize = $('#ServiceArea')[0].selectize;
        var departmentUnitSelectize = $('#Department')[0].selectize;

        businessUnitSelectize.clear();
        businessUnitSelectize.clearOptions();
        serviceAreaSelectize.clear();
        serviceAreaSelectize.clearOptions();
        departmentUnitSelectize.clear();
        departmentUnitSelectize.clearOptions();

        var ajaxInfo = $('#ajaxInfo');
        if (ajaxInfo.length > 0) {

            var url = ajaxInfo.attr('url');
            var customerId = ajaxInfo.attr('customerId');

            if (url != '' && customerId != '' && companyId != '') {

                var jqxhr = $.post(url + 'GetBusinessUnits?customerId=' + customerId + '&companyId=' + companyId, function () {

                })
                    .done(function (data) {
                        businessUnitSelectize.load(function (callback) {
                            callback(data);
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

    function changeBusinessUnit() {

        var businessUnitId = $("#BusinessUnit").val();
        var selectedValue = $("#val_ServiceArea").val();

        
        var serviceAreaSelectize = $('#ServiceArea')[0].selectize;
        var departmentUnitSelectize = $('#Department')[0].selectize;

        serviceAreaSelectize.clear();
        serviceAreaSelectize.clearOptions();
        departmentUnitSelectize.clear();
        departmentUnitSelectize.clearOptions();

        var ajaxInfo = $('#ajaxInfo');
        if (ajaxInfo.length > 0) {

            var url = ajaxInfo.attr('url');

            if (url != '' && businessUnitId != '') {

                var jqxhr = $.post(url + 'GetFunctions?businessUnitId=' + businessUnitId, function () {

                })
                    .done(function (data) {
                        serviceAreaSelectize.load(function (callback) {
                            callback(data);
                        });

                        if (selectedValue != '') {
                            $('#ServiceArea').val(selectedValue);
                            changeServiceArea();
                        }
                    })
                    .fail(function () { })
                    .always(function () { });
            }
        }
    }

    function changeServiceArea() {

        var serviceAreaId = $("#ServiceArea").val();
        var selectedValue = $("#val_Department").val();

        var departmentUnitSelectize = $('#Department')[0].selectize;

        departmentUnitSelectize.clear();
        departmentUnitSelectize.clearOptions();

        var ajaxInfo = $('#ajaxInfo');
        if (ajaxInfo.length > 0) {

            var url = ajaxInfo.attr('url');

            if (url != '' && serviceAreaId != '') {

                var jqxhr = $.post(url + 'GetDepartments?serviceAreaId=' + serviceAreaId, function () {

                })
                    .done(function (data) {
                        departmentUnitSelectize.load(function (callback) {
                            callback(data);
                        });

                        if (selectedValue != '')
                            $('#Department').val(selectedValue);
                    })
                    .fail(function () { })
                    .always(function () { });
            }
        }
    }

    $("#Company").change(function () {
        changeCompany();
    });

    $("#BusinessUnit").change(function () {
        changeBusinessUnit();
    });

    $("#ServiceArea").change(function () {
        changeServiceArea();
    });

    $("#Company").change();

});

//$(function () {

//    if ($('#NewBusinessUnit')[0].selectize) {
//        $('#NewBusinessUnit')[0].selectize.destroy();
//    }
//    if ($('#NewServiceArea')[0].selectize) {
//        $('#NewServiceArea')[0].selectize.destroy();
//    }
//    if ($('#NewDepartment')[0].selectize) {
//        $('#NewDepartment')[0].selectize.destroy();
//    }

//    if ($('#NewBusinessUnit').hasClass("search-select")) {
//        $('#NewBusinessUnit').selectize({
//            valueField: 'Id',
//            labelField: 'Name',
//            searchField: ['Name']
//        });
//    }

//    if ($('#NewServiceArea').hasClass("search-select")) {
//        $('#NewServiceArea').selectize({
//            valueField: 'Id',
//            labelField: 'Name',
//            searchField: ['Name']
//        });
//    }

//    if ($('#NewDepartment').hasClass("search-select")) {
//        $('#NewDepartment').selectize({
//            valueField: 'Id',
//            labelField: 'Name',
//            searchField: ['Name']
//        });
//    }

//    function changeCompany() {

//        var companyId = $("#NewCompany").val();
//        var selectedValue = $("#val_BusinessUnit").val();

//        var businessUnitSelectize = $('#NewBusinessUnit')[0].selectize;
//        var serviceAreaSelectize = $('#NewServiceArea')[0].selectize;
//        var departmentUnitSelectize = $('#NewDepartment')[0].selectize;

//        businessUnitSelectize.clear();
//        businessUnitSelectize.clearOptions();
//        serviceAreaSelectize.clear();
//        serviceAreaSelectize.clearOptions();
//        departmentUnitSelectize.clear();
//        departmentUnitSelectize.clearOptions();

//        var ajaxInfo = $('#ajaxInfo');
//        if (ajaxInfo.length > 0) {

//            var url = ajaxInfo.attr('url');
//            var customerId = ajaxInfo.attr('customerId');

//            if (url != '' && customerId != '' && companyId != '') {

//                var jqxhr = $.post(url + 'GetBusinessUnits?customerId=' + customerId + '&companyId=' + companyId, function () {

//                })
//                    .done(function (data) {
//                        businessUnitSelectize.load(function (callback) {
//                            callback(data);
//                        });

//                        if (selectedValue != '') {
//                            $('#NewBusinessUnit').val(selectedValue);
//                            changeNewBusinessUnit();
//                        }
//                    })
//                    .fail(function () { })
//                    .always(function () { });
//            }
//        }
//    }

//    function changeBusinessUnit() {

//        var businessUnitId = $("#NewBusinessUnit").val();
//        var selectedValue = $("#val_ServiceArea").val();


//        var serviceAreaSelectize = $('#NewServiceArea')[0].selectize;
//        var departmentUnitSelectize = $('#NewDepartment')[0].selectize;

//        serviceAreaSelectize.clear();
//        serviceAreaSelectize.clearOptions();
//        departmentUnitSelectize.clear();
//        departmentUnitSelectize.clearOptions();

//        var ajaxInfo = $('#ajaxInfo');
//        if (ajaxInfo.length > 0) {

//            var url = ajaxInfo.attr('url');

//            if (url != '' && businessUnitId != '') {

//                var jqxhr = $.post(url + 'GetFunctions?businessUnitId=' + businessUnitId, function () {

//                })
//                    .done(function (data) {
//                        serviceAreaSelectize.load(function (callback) {
//                            callback(data);
//                        });

//                        if (selectedValue != '') {
//                            $('#NewServiceArea').val(selectedValue);
//                            changeNewServiceArea();
//                        }
//                    })
//                    .fail(function () { })
//                    .always(function () { });
//            }
//        }
//    }

//    function changeServiceArea() {

//        var serviceAreaId = $("#NewServiceArea").val();
//        var selectedValue = $("#val_Department").val();

//        var departmentUnitSelectize = $('#NewDepartment')[0].selectize;

//        departmentUnitSelectize.clear();
//        departmentUnitSelectize.clearOptions();

//        var ajaxInfo = $('#ajaxInfo');
//        if (ajaxInfo.length > 0) {

//            var url = ajaxInfo.attr('url');

//            if (url != '' && serviceAreaId != '') {

//                var jqxhr = $.post(url + 'GetDepartments?serviceAreaId=' + serviceAreaId, function () {

//                })
//                    .done(function (data) {
//                        departmentUnitSelectize.load(function (callback) {
//                            callback(data);
//                        });

//                        if (selectedValue != '')
//                            $('#NewDepartment').val(selectedValue);
//                    })
//                    .fail(function () { })
//                    .always(function () { });
//            }
//        }
//    }

//    $("#NewCompany").change(function () {
//        changeNewCompany();
//    });

//    $("#NewBusinessUnit").change(function () {
//        changeNewBusinessUnit();
//    });

//    $("#NewServiceArea").change(function () {
//        changeNewServiceArea();
//    });

//    $("#NewCompany").change();

//});