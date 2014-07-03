
function changeCompany() {

    var companyId = $("#Company").val();
    var selectedValue = $("#val_BusinessUnit").val();

    $('#BusinessUnit').find('option').remove().end().append('<option value=""></option>');
    $('#ServiceArea').find('option').remove().end().append('<option value=""></option>');
    $('#Department').find('option').remove().end().append('<option value=""></option>');

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {

        var url = ajaxInfo.attr('url');
        var customerId = ajaxInfo.attr('customerId');

        if (url != '' && customerId != '' && companyId != '') {

            var jqxhr = $.post(url + 'GetBusinessUnits?customerId=' + customerId + '&companyId=' + companyId, function () {
                
            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        $('#BusinessUnit').append($("<option></option>").attr("value", e.Id).text(e.Name));
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

    $('#ServiceArea').find('option').remove().end().append('<option value=""></option>');
    $('#Department').find('option').remove().end().append('<option value=""></option>');

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {
       
        var url = ajaxInfo.attr('url');
        
        if (url != '' && businessUnitId != '') {  
          
            var jqxhr = $.post(url + 'GetFunctions?businessUnitId=' + businessUnitId, function () {
                
            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        $('#ServiceArea').append($("<option></option>").attr("value", e.Id).text(e.Name));
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

function changeFunctions() {

    var serviceAreaId = $("#ServiceArea").val();
    var selectedValue = $("#val_Department").val();

    $('#Department').find('option').remove().end().append('<option value=""></option>');

    var ajaxInfo = $('#ajaxInfo');
    if (ajaxInfo.length > 0) {
       
        var url = ajaxInfo.attr('url');
        
        if (url != '' && serviceAreaId != '') {
          
            var jqxhr = $.post(url + 'GetDepartments?serviceAreaId=' + serviceAreaId, function () {
                
            })
                .done(function (data) {
                    $.each(data, function (i, e) {
                        $('#Department').append($("<option></option>").attr("value", e.Id).text(e.Name));
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
    changeFunctions();
});

$("#Company").change();